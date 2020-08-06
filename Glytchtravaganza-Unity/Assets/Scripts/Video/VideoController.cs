using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VideoController
{
	private static VideoController instance;
	public static VideoController Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new VideoController();
			}

			return instance;
		}
	}

	private List<BackgroundVideoPlayer> _backgroundVideoPlayers = new List<BackgroundVideoPlayer>();
	internal void RegisterBackgroundPlayer(BackgroundVideoPlayer backgroundVideoPlayer)
	{
		_backgroundVideoPlayers.Add(backgroundVideoPlayer);
	}

	public bool IsPlaying => _manager.IsPlaying;

	private VideoManager _manager;
	private VideoData _videoData;
	private Action _videoCompleteSubscription = delegate { };

	private string _shortsKey = "Shorts";
	public void RegisterManager(VideoManager manager)
	{
		_manager = manager;
	}

	public void Init()
	{
		_videoData = Resources.LoadAll<VideoData>("").FirstOrDefault();
		_manager.LoadVideo(_videoData.GetVideoPath(_shortsKey));
		GlitchController.Instance.SubscribeToGlitchIntensityChange(GlitchIntensityChange);
		GlitchController.Instance.SubscribeToGlitch(Glitch);
		ArtworkController.Instance.SubscribeToOpenGallery(OpenArtWork);

		_backgroundVideoPlayers[0].LoadVideo(_videoData.GetVideoPath(_shortsKey));

		for (int i = 1; i < _backgroundVideoPlayers.Count; i++)
		{
			_backgroundVideoPlayers[i].Slave();
		}
	}

	public void SubscribeToVideoComplete(Action action)
	{
		_videoCompleteSubscription += action;
	}

	internal void VideoComplete()
	{
		_videoCompleteSubscription();
	}

	private void OpenArtWork(Artwork obj)
	{
		StopVideo();
	}

	private void Glitch(GlitchIntensity intensity)
	{
		if (GlitchController.Instance.Settings.PlayVideo)
		{
			PlayVideo(GlitchController.Instance.Settings);
		}
	}

	private void GlitchIntensityChange(GlitchIntensity intensity)
	{
		if (intensity == GlitchIntensity.None || intensity == GlitchIntensity.Low)
		{
			StopVideo();
		}
	}

	public void PlayVideo(string key)
	{
		_manager.PlayVideo();
	}

	public void PlayVideo(string key, float duration, bool jumpStart)
	{
		_manager.PlayVideo(duration, jumpStart);
	}

	public void PlayVideo(GlitchSettings settings)
	{
		PlayVideo(settings.VideoKey, settings.VideoDuration, settings.JumpVideo);
	}

	public void StopVideo()
	{
		_manager.StopVideo();
	}
}
