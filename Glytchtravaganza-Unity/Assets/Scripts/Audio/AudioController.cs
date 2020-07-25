using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AudioController
{
	private static AudioController instance;
	public static AudioController Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new AudioController();
			}

			return instance;
		}
	}

	private AudioManager _audioManager;
	private AudioData _audioData;
	public void RegisterManager(AudioManager manager)
	{
		_audioManager = manager;
	}

	public void Init()
	{
		_audioData = Resources.LoadAll<AudioData>("").FirstOrDefault();
		_audioManager.SpawnClips(_audioData);
	}

	public void PlayClip(string key)
	{
		_audioManager.PlayClip(key);
	}
}
