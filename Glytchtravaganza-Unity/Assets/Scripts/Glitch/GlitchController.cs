using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GlitchIntensity
{
	None = 0,
	Low = 1,
	Medium = 2,
	High = 3,
	Payoff = 4
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

	private GlitchData _glitchData;
	public GlitchSettings Settings => _glitchData.GlitchSettings.Find(item => item.GlitchIntensity == GlitchIntensity);

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
		return _glitchManager.CanGlitch() && GlitchIntensity != GlitchIntensity.None && GlitchIntensity != GlitchIntensity.Payoff && !ArtworkController.Instance.IsOpen && !VideoController.Instance.IsPlaying && Settings.GlitchFrequency > 0;
	}

	internal void Init()
	{
		_glitchData = Resources.LoadAll<GlitchData>("").FirstOrDefault();
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

	internal void IncreaseIntensity()
	{
		switch (GlitchIntensity)
		{
			case GlitchIntensity.None:
				SetGlitchIntensity(GlitchIntensity.Low);
				break;
			case GlitchIntensity.Low:
				SetGlitchIntensity(GlitchIntensity.Medium);
				break;
			case GlitchIntensity.Medium:
				SetGlitchIntensity(GlitchIntensity.High);
				break;
			case GlitchIntensity.High:
				SetGlitchIntensity(GlitchIntensity.Payoff);
				RandomGlitch();
				break;
			case GlitchIntensity.Payoff:
				SetGlitchIntensity(GlitchIntensity.None);
				break;
		}
		
	}
}

