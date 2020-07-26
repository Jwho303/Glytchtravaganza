using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AudioManager : MonoBehaviour
{
	[SerializeField]
	private List<AudioPlayer> _audioPlayers = new List<AudioPlayer>();

	private bool _pause = false;

	private void Awake()
	{
		AudioController.Instance.RegisterManager(this);
		GlitchController.Instance.SubscribeToGlitchIntensityChange(GlitchIntensityChange);
		ArtworkController.Instance.SubscribeToOpenGallery(OpenArtWork);
		ArtworkController.Instance.SubscribeToCloseGallery(CloseArtWork);
	}

	private void OpenArtWork(Artwork obj)
	{
		StopAllPlayers();
		_pause = true;
	}

	private void CloseArtWork()
	{
		_pause = false;
	}

	private void GlitchIntensityChange(GlitchIntensity intensity)
	{
		PlayAmbientSounds();
	}

	public void Update()
	{
		if (!_pause)
		{
			PlayAmbientSounds();
		}
	}

	private void PlayAmbientSounds()
	{
		if (!IsPlaying(GlitchController.Instance.Settings.SoundKey))
		{
			StopAllPlayers();
			PlayClip(GlitchController.Instance.Settings.SoundKey);
		}
	}

	private bool IsPlaying(string key)
	{
		bool result = false;
		AudioPlayer audioPlayer = _audioPlayers.Find(item => item.Key == key);
		if (audioPlayer != null)
		{
			for (int i = 0; i < audioPlayer.AudioSources.Count; i++)
			{
				if (audioPlayer.AudioSources[i].isPlaying)
				{
					result = true;
					break;
				}
			}
		}

		return result;
	}

	internal void SpawnClips(AudioData audioData)
	{
		for (int i = 0; i < audioData.AudioObjects.Count; i++)
		{
			if (audioData.AudioObjects[i].SpawnOnStart)
			{
				CreatePlayer(audioData.GetAudioObject(audioData.AudioObjects[i].Key));
			}
		}
	}

	private void CreatePlayer(AudioObject audioObject)
	{
		AudioPlayer audioPlayer = new AudioPlayer(audioObject, this.gameObject);
		_audioPlayers.Add(audioPlayer);
	}

	internal void PlayClip(string key)
	{
		AudioPlayer audioPlayer = _audioPlayers.Find(item => item.Key == key);
		AudioSource source = audioPlayer.AudioSources[Random.Range(0, audioPlayer.AudioSources.Count)];
		source.time = Random.Range(0f, source.clip.length);
		source.Play();
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
