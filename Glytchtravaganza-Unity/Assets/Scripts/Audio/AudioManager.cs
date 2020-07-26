using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AudioManager : MonoBehaviour
{
	[SerializeField]
	private List<AudioPlayer> _audioPlayers = new List<AudioPlayer>();

	[SerializeField]
	private string _suburbiaKey;

	[SerializeField]
	private string _cbdKey;

	private GlitchIntensity _glitchIntensity;
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
		Debug.LogFormat("[{0}] intenisty change: {1}", this.name, _glitchIntensity);

		_glitchIntensity = intensity;

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
		if (_glitchIntensity == GlitchIntensity.None || _glitchIntensity == GlitchIntensity.Low)
		{
			if (!IsPlaying(_suburbiaKey))
			{
				StopAllPlayers();
				PlayClip(_suburbiaKey);
			}
		}
		else
		{
			if (!IsPlaying(_cbdKey))
			{
				StopAllPlayers();
				PlayClip(_cbdKey);
			}
		}
	}

	private bool IsPlaying(string key)
	{
		bool result = false;
		AudioPlayer audioPlayer = _audioPlayers.Find(item => item.Key == key);
		for (int i = 0; i < audioPlayer.AudioSources.Count; i++)
		{
			if (audioPlayer.AudioSources[i].isPlaying)
			{
				result = true;
				break;
			}
		}

		return result;
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
