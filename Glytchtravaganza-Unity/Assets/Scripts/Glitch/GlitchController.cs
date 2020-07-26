using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GlitchIntensity
{
	None = 0,
	Low = 5,
	Medium = 10,
	High = 20
}

public class GlitchController
{
	private static GlitchController instance;
	public static GlitchController Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new GlitchController();
			}

			return instance;
		}
	}

	public GlitchIntensity GlitchIntensity { get; private set; } = GlitchIntensity.None;

	private Action<GlitchIntensity> _glitchIntensitySubscription = delegate { };
	private Action<GlitchIntensity> _glitchSubscription = delegate { };

	public void SubscribeToGlitchIntensityChange(Action<GlitchIntensity> action)
	{
		_glitchIntensitySubscription += action;
	}

	public void SubscribeToGlitch(Action<GlitchIntensity> action)
	{
		_glitchSubscription += action;
	}

	public void SetGlitchIntensity(GlitchIntensity intensity)
	{
		GlitchIntensity = intensity;
		_glitchIntensitySubscription(GlitchIntensity);
		if (intensity == GlitchIntensity.None && _glitchManager != null)
		{
			_glitchManager.StopGlitches();
		}
	}

	private GlitchManager _glitchManager;
	public void RegisterManager(GlitchManager manager)
	{
		_glitchManager = manager;
	}

	public bool CanGlitch()
	{
		return _glitchManager.CanGlitch() && GlitchIntensity != GlitchIntensity.None && !ArtworkController.Instance.IsOpen;
	}

	internal void Init()
	{
		ArtworkController.Instance.SubscribeToOpenGallery(OpenArtWork);
	}

	private void OpenArtWork(Artwork obj)
	{
		_glitchManager.StopGlitches();
	}

	public void RandomGlitch()
	{
		_glitchManager.RandomGlitch(GlitchIntensity);
		_glitchSubscription(GlitchIntensity);
	}
}
