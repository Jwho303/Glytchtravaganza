﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "AudioData", menuName = "AudioData", order = 1)]
public class AudioData : ScriptableObject
{
	public List<AudioObject> AudioObjects = new List<AudioObject>();
	public AudioClip GetAudioClip(string key)
	{
		return GetRandomClip(AudioObjects.Find((item) => item.Key == key).AudioClips);
	}

	private AudioClip GetRandomClip(AudioClip[] audioClips)
	{
		return audioClips[Random.Range(0, audioClips.Length - 1)];
	}
}


[System.Serializable]
public class AudioObject
{
	public string Key;
	public AudioClip[] AudioClips;
}

