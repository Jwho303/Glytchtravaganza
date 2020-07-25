using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	private GlitchManager _gameManager;
	public void RegisterManager(GlitchManager manager)
	{
		_gameManager = manager;
	}

	public bool CanGlitch()
	{
		return _gameManager.CanGlitch();
	}

	internal void Init()
	{
		
	}

	public void RandomGlitch(GlitchManager.GlitchIntensity glitchIntensity)
	{
		_gameManager.RandomGlitch(glitchIntensity);
	}
}
