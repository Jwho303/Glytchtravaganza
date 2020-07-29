using System;
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

	private VideoPath _videoPath;

	private float _videoEndTime = -1f;
	private bool _jumpStart = false;
	private float _duration = 0;

	private Coroutine _jumpCoroutine;

	[SerializeField]
	private bool _isPlaying = false;
	[SerializeField]
	private double _videoTime = 0;
	[SerializeField]
	[Range(0f, 1f)]
	private float _videoSeek;

	public bool IsPlaying => _videoPlayer.isPlaying;

	// Start is called before the first frame update
	void Awake()
	{
		VideoController.Instance.RegisterManager(this);
		StopVideo();
	}

	public void LoadVideo(VideoPath videoPath)
	{
		_videoPath = videoPath;
		_videoPlayer.url = videoPath.VideoURL;
		_videoPlayer.Prepare();

		_videoPlayer.loopPointReached += VideoComplete;

		Debug.Log("Load video");
	}

	private void VideoComplete(VideoPlayer source)
	{
		VideoController.Instance.VideoComplete();
	}

	// Update is called once per frame
	void Update()
	{
		_isPlaying = _videoPlayer.isPlaying;
		_videoTime = _videoPlayer.time;
		_videoSeek = Mathf.Clamp01((float)_videoPlayer.time / (float)_videoPlayer.length);
	}

	internal void StopVideo()
	{
		Debug.Log("Stop video");
		_camera.enabled = false;
		_videoPlayer.Pause();
		_videoEndTime = -1f;

		if (_jumpCoroutine != null)
		{
			StopCoroutine(_jumpCoroutine);
		}
	}

	internal void PlayVideo(float duration = -1, bool jumpStart = false)
	{
		Debug.Log("Play video");
		_jumpStart = jumpStart;
		_duration = duration;
		

		
		if (_jumpCoroutine != null)
		{
			StopCoroutine(_jumpCoroutine);
		}

		_jumpCoroutine = StartCoroutine(JumpStart());


	}

	private IEnumerator JumpStart()
	{
		double jumpTime = 0;

		if (_jumpStart)
		{
			jumpTime = (double)UnityEngine.Random.Range(_videoPath.JumpStartTime, _videoPath.JumpEndTime);
		}

		_videoPlayer.Prepare();

		while (!_videoPlayer.isPrepared)
		{
			Debug.LogFormat("Preparing Video");
			yield return new WaitForEndOfFrame();
		}

		_videoPlayer.time = jumpTime;

		_videoPlayer.Play();
		_camera.enabled = true;

		if (_duration > 0)
		{
			_videoEndTime = Time.unscaledTime + _duration;

			while (Time.unscaledTime < _videoEndTime)
			{
				yield return new WaitForEndOfFrame();
			}
			StopVideo();
		}
		else
		{
			_videoEndTime = -1f;
		}

		_jumpStart = false;

		_jumpCoroutine = null;
	}

}
