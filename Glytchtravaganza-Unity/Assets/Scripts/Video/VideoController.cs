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

	private VideoManager _manager;
	private VideoData _videoData;

	private string _shortsKey = "Shorts";
	public void RegisterManager(VideoManager manager)
	{
		_manager = manager;
	}

	public void Init()
	{
		_videoData = Resources.LoadAll<VideoData>("").FirstOrDefault();
		GlitchController.Instance.SubscribeToGlitchIntensityChange(GlitchIntensityChange);
		GlitchController.Instance.SubscribeToGlitch(Glitch);
	}

	private void Glitch(GlitchIntensity intensity)
	{
		if (intensity == GlitchIntensity.Medium)
		{
			PlayVideo(_shortsKey, 0.5f, true);
		}
		else if (intensity == GlitchIntensity.High)
		{
			PlayVideo(_shortsKey, 2f,true);
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
		_manager.PlayVideo(_videoData.GetVideoURL(key));
	}

	public void PlayVideo(string key, float duration, bool jumpStart)
	{
		_manager.PlayVideo(_videoData.GetVideoURL(key), duration, jumpStart);
	}

	public void StopVideo()
	{
		_manager.StopVideo();
	}
}
