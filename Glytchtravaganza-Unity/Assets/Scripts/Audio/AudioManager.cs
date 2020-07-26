using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	[SerializeField]
	private List<AudioPlayer> _audioPlayers = new List<AudioPlayer>();

	[SerializeField]
	private string _suburbiaKey;

	[SerializeField]
	private string _cbdKey;

	private GlitchIntensity _glitchIntensity;

	private void Awake()
	{
		AudioController.Instance.RegisterManager(this);
		GlitchController.Instance.SubscribeToGlitchIntensityChange(GlitchIntensityChange);
	}

	private void GlitchIntensityChange(GlitchIntensity intensity)
	{

		Debug.LogFormat("[{0}] intenisty change: {1}", this.name, _glitchIntensity);

		if (_glitchIntensity == GlitchIntensity.None || _glitchIntensity == GlitchIntensity.Low)
		{
			PlayClip(_suburbiaKey);
		}
		else
		{
			PlayClip(_cbdKey);
		}

		_glitchIntensity = intensity;
	}

	internal void SpawnClips(AudioData audioData)
	{
		CreatePlayer(audioData.GetAudioObject(_suburbiaKey));
		CreatePlayer(audioData.GetAudioObject(_cbdKey));
	}

	private void CreatePlayer(AudioObject audioObject)
	{
		AudioPlayer audioPlayer = new AudioPlayer(audioObject, this.gameObject);
		_audioPlayers.Add(audioPlayer);
	}

	internal void PlayClip(string key)
	{
		
	}

	internal void StopAllPlayers()
	{
		for (int i = 0; i < _audioPlayers.Count; i++)
		{
			for (int ii = 0; ii < _audioPlayers[i].AudioSources.Count; ii++)
			{
				_audioPlayers[i].AudioSources[ii].Stop();
			}
		}
	}
}

[System.Serializable]
public class AudioPlayer
{
	public string Key;
	public List<AudioSource> AudioSources = new List<AudioSource>();

	public AudioPlayer(AudioObject audioObject, GameObject parent)
	{
		this.Key = audioObject.Key;

		for (int i = 0; i < audioObject.AudioClips.Length; i++)
		{
			AudioSource source = parent.AddComponent<AudioSource>();
			source.playOnAwake = false;
			source.clip = audioObject.AudioClips[i];
			source.Stop();
			AudioSources.Add(source);
		}
	}
}
