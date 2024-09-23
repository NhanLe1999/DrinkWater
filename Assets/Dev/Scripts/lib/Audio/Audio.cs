using Gdz;
using UnityEngine;

public static class Audio
{
	public static bool pausedBackground => AudioManager.instance.pausedBackground;

	public static bool paused => AudioManager.instance.paused;

	public static bool soundEnabled
	{
		get
		{
			return PlayerPrefs.GetString(ScStaticScene.KEY_SAVE_SOUND, "true").Equals("true");
		}
		set
		{
            PlayerPrefs.SetString(ScStaticScene.KEY_SAVE_SOUND, value ? "true" : "false");
		}
	}

	public static bool musicEnabled
	{
		get
		{
            return PlayerPrefs.GetString(ScStaticScene.KEY_SAVE_MUSIC, "true").Equals("true");
        }
        set
		{
            PlayerPrefs.SetString(ScStaticScene.KEY_SAVE_MUSIC, value ? "true" : "false");
        }
    }

	public static bool isBackgroundMusicPlaying => AudioManager.instance.isBackgroundMusicPlaying;

	public static string Play(string fileName)
	{
		return AudioManager.instance.Play(fileName, AudioManager.instance.soundVolume, loop: false, async: false);
	}

	public static string Play(string fileName, float volume)
	{
		return AudioManager.instance.Play(fileName, volume, loop: false, async: false);
	}

	public static string Play(string fileName, float volume, bool loop, bool async = false)
	{
		return AudioManager.instance.Play(fileName, volume, loop, async);
	}

	public static string PlayRandom(params string[] names)
	{
		return Play(names[Random.Range(0, names.Length)]);
	}

	public static string PlayAsync(string fileName)
	{
		return AudioManager.instance.Play(fileName, AudioManager.instance.soundVolume, loop: false, async: true);
	}

	public static string PlayAsync(string fileName, float volume)
	{
		return AudioManager.instance.Play(fileName, volume, loop: false, async: true);
	}

	public static string PlayAsync(string fileName, float volume, bool loop)
	{
		return AudioManager.instance.Play(fileName, volume, loop, async: true);
	}

	public static string PlayAsyncRandom(params string[] names)
	{
		return PlayAsync(names[Random.Range(0, names.Length)]);
	}

	public static void PlayBackgroundMusic(string fileName)
	{
		AudioManager.instance.PlayBackgroundMusic(fileName, AudioManager.instance.musicVolume);
    }

    public static void PlayBackgroundMusic(string fileName, float volume)
	{
		AudioManager.instance.PlayBackgroundMusic(fileName, volume);
	}

	public static void PlayBackgroundMusicRandom(params string[] names)
	{
		PlayBackgroundMusic(names[Random.Range(0, names.Length)]);
	}

	public static void Pause()
	{
		AudioManager.instance.Pause();
	}

	public static void Resume()
	{
		AudioManager.instance.Resume();
	}

	public static void Stop(string id = null)
	{
		if (id == null)
		{
			AudioManager.instance.StopAll();
		}
		else
		{
			AudioManager.instance.Stop(id);
		}
	}

	public static void PauseBackgroundMusic()
	{
		AudioManager.instance.PauseBackgroundMusic();
	}

	public static void ResumeBackgroundMusic()
	{
		AudioManager.instance.ResumeBackgroundMusic();
	}

	public static void StopBackgroundMusic()
	{
		AudioManager.instance.StopBackgroundMusic();
	}

	public static bool IsSoundPlaying(string id)
	{
		return AudioManager.instance.IsSoundPlaying(id);
	}
}
