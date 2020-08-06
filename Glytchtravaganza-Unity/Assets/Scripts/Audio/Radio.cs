
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : GlitchListener
{
	[SerializeField]
	private AudioSource _audioSource;

	[SerializeField]
	private AudioClip[] _clips;

	private int _playIndex = 0;

	[SerializeField]
	[Range(0f, 1f)]
	private float _trackSeek;

	public override void Start()
	{
		_onAction += PlaySound;
		_offAction += StopSound;
		_openArtworkAction += ArtworkOpened;
		_closeArtworkAction += ArtworkClosed;

		base.Start();
		_playIndex = 0;
		LoadClip(_playIndex);
	}
	private void ArtworkOpened()
	{
		if (!_isOn)
		{
			LoadClip(1);
			_audioSource.time = Random.Range(0f, _audioSource.clip.length);
			_audioSource.Play();
		}
	}

	private void ArtworkClosed()
	{
		if (!_isOn)
		{
			LoadClip(1);
			_audioSource.Stop();
		}
	}


	public void Update()
	{
		_trackSeek = (_audioSource.clip != null) ? _audioSource.time / _audioSource.clip.length : 0f;

		if (_trackSeek >= 1f)
		{
			ClipFinished();
		}
	}

	public void OnValidate()
	{
		if (_audioSource.clip != null)
		{
			_audioSource.time = _trackSeek * _audioSource.clip.length;
		}
	}

	private void ClipFinished()
	{
		_playIndex = (_playIndex + 1 < _clips.Length) ? (_playIndex + 1) : 0;
		LoadClip(_playIndex);
	}

	private void PlaySound()
	{
		if (_audioSource != null)
		{
			if (!_audioSource.isPlaying)
			{
				_playIndex = 0;
				LoadClip(_playIndex);
			}
		}
	}

	private void StopSound()
	{
		if (_audioSource != null)
		{
			_audioSource.Stop();
		}
	}

	private void LoadClip(int index)
	{
		_audioSource.clip = _clips[index];
		_audioSource.time = 0f;
		_audioSource.Play();
	}
}
