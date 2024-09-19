using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
    public static partial class UniTaskAsyncEnumerable
    {
        public static IUniTaskAsyncEnumerable<TSource> Queue<TSource>(this IUniTaskAsyncEnumerable<TSource> source)
        {
            return new QueueOperator<TSource>(source);
        }
    }

    internal sealed class QueueOperator<TSource> : IUniTaskAsyncEnumerable<TSource>
    {
        readonly IUniTaskAsyncEnumerable<TSource> source;

        public QueueOperator(IUniTaskAsyncEnumerable<TSource> source)
        {
            this.source = source;
        }

        public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new _Queue(source, cancellationToken);
        }

        sealed class _Queue : IUniTaskAsyncEnumerator<TSource>
        {
            readonly IUniTaskAsyncEnumerable<TSource> source;
            CancellationToken cancellationToken;

            Channel<TSource> channel;
            IUniTaskAsyncEnumerator<TSource> channelEnumerator;
            IUniTaskAsyncEnumerator<TSource> sourceEnumerator;
            bool channelClosed;

            public _Queue(IUniTaskAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
            {
                this.source = source;
                this.cancellationToken = cancellationToken;
            }

            public TSource Current => channelEnumerator.Current;

            public UniTask<bool> MoveNextAsync()
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (sourceEnumerator == null)
                {
                    sourceEnumerator = source.GetAsyncEnumerator(cancellationToken);
                    channel = Channel.CreateSingleConsumerUnbounded<TSource>();

                    channelEnumerator = channel.Reader.ReadAllAsync().GetAsyncEnumerator(cancellationToken);

                    ConsumeAll(this, sourceEnumerator, channel).Forget();
                }

                return channelEnumerator.MoveNextAsync();
            }

            static async UniTaskVoid ConsumeAll(_Queue self, IUniTaskAsyncEnumerator<TSource> enumerator, ChannelWriter<TSource> writer)
            {
                try
                {
                    while (await enumerator.MoveNextAsync())
                    {
                        _ = writer.TryWrite(enumerator.Current);
                    }
                    _ = writer.TryComplete();
                }
                catch (Exception ex)
                {
                    _ = writer.TryComplete(ex);
                }
                finally
                {
                    self.channelClosed = true;
                    await enumerator.DisposeAsync();
                }
            }

            public async UniTask DisposeAsync()
            {
                if (sourceEnumerator != null)
                {
                    await sourceEnumerator.DisposeAsync();
                }
                if (channelEnumerator != null)
                {
                    await channelEnumerator.DisposeAsync();
                }

                if (!channelClosed)
                {
                    channelClosed = true;
                    _ = channel.Writer.TryComplete(new OperationCanceledException());
                }
            }
        }
    }
}