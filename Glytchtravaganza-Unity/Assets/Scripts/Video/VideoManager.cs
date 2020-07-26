using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
	[SerializeField]
	private Camera _camera;

	[SerializeField]
	private VideoPlayer _videoPlayer;

	private float _videoEndTime = -1f;
	private bool _jumpStart = false;
	// Start is called before the first frame update
	void Awake()
	{
		VideoController.Instance.RegisterManager(this);
		StopVideo();
	}

	// Update is called once per frame
	void Update()
	{
		if (_videoEndTime > 0f)
		{
			if (_videoPlayer.isPlaying)
			{
				if (_jumpStart && _videoPlayer.length > 0f)
				{
					JumpStart();
				}

				if (Time.unscaledTime >= _videoEndTime)
				{
					StopVideo();
				}
			}
		}
	}

	internal void StopVideo()
	{
		_camera.enabled = false;
		_videoPlayer.Stop();
		_videoEndTime = -1f;
	}

	internal void PlayVideo(string url, float duration = -1f, bool jumpStart = false)
	{
		if (duration > 0)
		{
			_videoEndTime = Time.unscaledTime + duration;
		}
		else
		{
			_videoEndTime = -1f;
		}
		
		_jumpStart = jumpStart;
		_camera.enabled = true;
		_videoPlayer.url = url;
		_videoPlayer.Prepare();
		_videoPlayer.Play();
	}

	private void JumpStart()
	{
		_jumpStart = false;
		_videoPlayer.time = (double)Random.Range(0f, (float)_videoPlayer.length);
	}
}
