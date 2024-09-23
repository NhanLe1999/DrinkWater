namespace Gdz
{
	public class AudioFile
	{
		public string fileName;

		public string fullPath;

		public AudioFile()
		{
		}

		public AudioFile(string _name, string _path)
		{
			fileName = _name;
			fullPath = _path;
		}
	}
}
