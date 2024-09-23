using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace Gdz
{
    public class AudioManager : MonoBehaviour
    {
        private static string[] p_fileTypes = new string[3]
        {
            "*.mp3",
            "*.wav",
            "*.ogg"
        };

        private GameObject p_go_music;

        private GameObject p_go_sound;

        private Dictionary<string, string> fileNames;

        private List<string> playList;

        private List<AudioSource> allClips = new List<AudioSource>();

        private AudioSource backgroundMusic;

        private bool p_paused;

        private bool p_pausedBackground;

        private float p_soundVolume = 1f;

        private float p_musicVolume = 0.75f;

        private bool _enabledSound
        {
            get
            {
                return Audio.soundEnabled;
            }
        }

        private bool _enabledMusic
        {
            get
            {
                return Audio.musicEnabled;
            }
        }

        private static AudioManager _instance;

        public bool paused
        {
            get
            {
                return p_paused;
            }
            set
            {
                p_paused = value;
                if (value)
                {
                    Pause();
                }
                else
                {
                    Resume();
                }
            }
        }

        public bool pausedBackground
        {
            get
            {
                return p_pausedBackground;
            }
            set
            {
                p_pausedBackground = value;
                if (value)
                {
                    PauseBackgroundMusic();
                }
                else
                {
                    ResumeBackgroundMusic();
                }
            }
        }

        public float soundVolume
        {
            get
            {
                return p_soundVolume;
            }
            set
            {
                p_soundVolume = value;
                List<AudioSource> list = new List<AudioSource>(allClips);
                foreach (AudioSource item in list)
                {
                    item.volume = p_soundVolume;
                }
            }
        }

        public float musicVolume
        {
            get
            {
                return p_musicVolume;
            }
            set
            {
                p_musicVolume = value;
                if (backgroundMusic != null)
                {
                    backgroundMusic.volume = p_musicVolume;
                }
            }
        }

        public bool isBackgroundMusicPlaying
        {
            get
            {
                if (!backgroundMusic)
                {
                    return false;
                }
                if (backgroundMusic.isPlaying)
                {
                    return true;
                }
                return false;
            }
        }

        public static AudioManager instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject gameObject = new GameObject("_audio controller");
                    _instance = gameObject.AddComponent<AudioManager>();
                    _instance.Create();
                }
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        private void Create()
        {
            Object.DontDestroyOnLoad(base.gameObject);
            base.gameObject.AddComponent<AudioListener>();
            p_go_music = new GameObject();
            p_go_music.name = "_music";
            p_go_music.transform.parent = base.gameObject.transform;
            p_go_sound = new GameObject();
            p_go_sound.name = "_sounds";
            p_go_sound.transform.parent = base.gameObject.transform;
            if (fileNames == null)
            {
                LoadAudioFiles();
            }
            if (playList == null)
            {
                playList = new List<string>();
            }
        }

        private void LoadAudioFiles()
        {
            fileNames = new Dictionary<string, string>();
            TextAsset textAsset = Resources.Load("audio") as TextAsset;
            AudioSerialization audioSerialization = Deserialize<AudioSerialization>(textAsset.text);
            for (int i = 0; i < audioSerialization.files.Count; i++)
            {
                fileNames.Add(audioSerialization.files[i].fileName, audioSerialization.files[i].fullPath);
            }
        }

        private static string Serialize<T>(object details)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            MemoryStream memoryStream = new MemoryStream();
            xmlSerializer.Serialize(memoryStream, details);
            StreamReader streamReader = new StreamReader(memoryStream);
            memoryStream.Position = 0L;
            string result = streamReader.ReadToEnd();
            memoryStream.Flush();
            memoryStream = null;
            streamReader = null;
            return result;
        }

        private static T Deserialize<T>(string details)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            XmlReader xmlReader = XmlReader.Create(new StringReader(details));
            return (T)xmlSerializer.Deserialize(xmlReader);
        }

        private void addToPlaylist(string clipName)
        {
            playList.Add(clipName);
        }

        private string getFullFileName(string shortName)
        {
            string value = null;
            fileNames.TryGetValue(shortName, out value);
            return value;
        }

        private void removeFromPlaylist(string clipName)
        {
            playList.Remove(clipName);
        }

        private void Update()
        {
            List<AudioSource> list = new List<AudioSource>(allClips);
            foreach (AudioSource item in list)
            {
                if (!item.isPlaying && item.time == 0f)
                {
                    removeFromPlaylist(item.clip.name);
                    allClips.Remove(item);
                    UnityEngine.Object.Destroy(item);
                }
            }

            AudioListener[] audioListeners = FindObjectsOfType<AudioListener>();
            if (audioListeners.Length > 1)
            {
                for (int i = 1; i < audioListeners.Length; i++)
                {
                    Destroy(audioListeners[i]);
                }
            }
        }

        public string Play(string fileName, float volume, bool loop, bool async)
        {
            if (!_enabledSound || !Audio.soundEnabled)
            {
                return null;
            }
            fileName = Path.GetFileNameWithoutExtension(fileName);
            string fullFileName = getFullFileName(fileName);
            if (fullFileName == null)
            {
                UnityEngine.Debug.LogWarning($"{fileName} was not found. Check the file and do  \"Tools/Audio/Generate Audio\" to generate this file.");
                return string.Empty;
            }
            string result = null;
            if (!async && playList.Contains(fileName))
            {
                return result;
            }
            AudioSource audioSource = null;
            audioSource = p_go_sound.AddComponent<AudioSource>();
            audioSource.clip = (Resources.Load(fullFileName + fileName) as AudioClip);
            audioSource.volume = volume;
            audioSource.loop = loop;
            if (!async)
            {
                addToPlaylist(fileName);
            }
            allClips.Add(audioSource);
            result = audioSource.clip.name;
            audioSource.clip.LoadAudioData();
           /* while (backgroundMusic.clip.loadState != AudioDataLoadState.Loaded)
            {

            }*/
            audioSource.Play();
            return result;
        }

        public void Pause()
        {
            if (_enabledSound)
            {
                p_paused = true;
                List<AudioSource> list = new List<AudioSource>(allClips);
                foreach (AudioSource item in list)
                {
                    item.Pause();
                }
            }
        }

        public void Resume()
        {
            if (_enabledSound)
            {
                List<AudioSource> list = new List<AudioSource>(allClips);
                foreach (AudioSource item in list)
                {
                    item.Play();
                }
                p_paused = false;
            }
        }

        public void Stop(string id)
        {
            if (id != null && !(id == string.Empty))
            {
                List<AudioSource> list = new List<AudioSource>(allClips);
                foreach (AudioSource item in list)
                {
                    if (item.clip.name == id)
                    {
                        item.Stop();
                    }
                }
            }
        }

        public void StopAll()
        {
            List<AudioSource> list = new List<AudioSource>(allClips);
            foreach (AudioSource item in list)
            {
                item.Stop();
            }
            p_paused = false;
        }

        async public void PlayBackgroundMusic(string fileName, float volume)
        {
            p_pausedBackground = false;
            fileName = Path.GetFileNameWithoutExtension(fileName);
            string fullFileName = getFullFileName(fileName);
            if (fullFileName == null)
            {
                UnityEngine.Debug.LogWarning($"{fileName} was not found. Check the file and do  \"Tools/Audio/Generate Audio\" to generate this file.");
                return;
            }
            if (backgroundMusic != null)
            {
                StopBackgroundMusic();
            }
            backgroundMusic = p_go_music.AddComponent<AudioSource>();
            backgroundMusic.clip = (Resources.Load(fullFileName + fileName) as AudioClip);
            backgroundMusic.volume = volume;
            backgroundMusic.loop = true;
            backgroundMusic.clip.LoadAudioData();

            while (backgroundMusic.clip.loadState != AudioDataLoadState.Loaded)
            {
                await UniTask.Yield();
            }    

            backgroundMusic.Play();

            if (!_enabledMusic)
            {
                while (!backgroundMusic.isPlaying)
                {
                    await UniTask.Yield();
                }
                PauseBackgroundMusic();
            }
        }

        public void PauseBackgroundMusic()
        {
            if (backgroundMusic != null)
            {
                if (backgroundMusic.isPlaying)
                {
                    backgroundMusic.Pause();
                }
                p_pausedBackground = true;
            }
        }

        public void ResumeBackgroundMusic()
        {
            if (_enabledMusic && !(backgroundMusic == null))
            {
                if (!backgroundMusic.isPlaying)
                {
                    backgroundMusic.Play();
                }
                p_pausedBackground = false;
            }
        }

        public void StopBackgroundMusic()
        {
            if (!(backgroundMusic == null))
            {
                backgroundMusic.Stop();
                UnityEngine.Object.Destroy(backgroundMusic);
                backgroundMusic = null;
                p_pausedBackground = false;
            }
        }

        public bool IsSoundPlaying(string id)
        {
            if (id == null || id.Equals(string.Empty))
            {
                return false;
            }
            List<AudioSource> list = new List<AudioSource>(allClips);
            foreach (AudioSource item in list)
            {
                if (item.clip.name == id)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
