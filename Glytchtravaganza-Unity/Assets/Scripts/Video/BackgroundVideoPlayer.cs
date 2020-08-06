using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class BackgroundVideoPlayer : MonoBehaviour
{
	[SerializeField]
	private VideoPlayer _videoPlayer;

	private VideoPath _videoPath;
	private bool _isDefaultPlayer = false;
	public Material Material
	{
		get
		{
			return GetComponent<MeshRenderer>().sharedMaterial;
		}
		set
		{
			GetComponent<MeshRenderer>().sharedMaterial = value;

		}
	}

	void Awake()
	{
		VideoController.Instance.RegisterBackgroundPlayer(this);
		_videoPlayer.prepareCompleted += Play;
	}

	private void Play(VideoPlayer source)
	{
		_videoPlayer.time = Random.Range(_videoPath.JumpStartTime, _videoPath.JumpEndTime);
		_videoPlayer.SetDirectAudioMute(0, true);
		_videoPlayer.Play();
	}

	public void LoadVideo(VideoPath videoPath)
	{
		_videoPath = videoPath;
		_videoPlayer.url = videoPath.VideoURL;
		_videoPlayer.Prepare();
		_videoPlayer.audioOutputMode = VideoAudioOutputMode.None;
		_videoPlayer.isLooping = true;

		Debug.Log("Load video");
		_isDefaultPlayer = true;
	}

	public void Slave()
	{
		_isDefaultPlayer = false;
		GameObject.Destroy(this._videoPlayer);
		_videoPlayer = null;
	}

	private void Update()
	{
		if (_isDefaultPlayer)
		{
			if (_videoPlayer.isPlaying)
			{
				if (_videoPlayer.time > _videoPath.JumpEndTime)
				{
					_videoPlayer.time = _videoPath.JumpStartTime;
				}
			}
		}
	}
}
