using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class BackgroundVideoPlayer : MonoBehaviour
{
	[SerializeField]
	private VideoPlayer _videoPlayer;

	private VideoPath _videoPath;

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
		Material material = new Material(Shader.Find("Unlit/Texture"));
		GetComponent<MeshRenderer>().sharedMaterial = material;
		Debug.Log("Load video");
	}

	private void Update()
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
