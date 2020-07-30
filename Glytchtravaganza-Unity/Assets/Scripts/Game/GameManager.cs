using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField]
	private GlitchIntensity glitchIntensity = GlitchIntensity.Low;
	[SerializeField]
	private int _artworksOpened = -1;
	[SerializeField]
	private bool _countingDown = false;
	[SerializeField]
	private float _glitchCountDown = -1f;
	[SerializeField]
	private float _IntensityCountDown = -1f;

	GlitchController GlitchController;
	// Start is called before the first frame update

	private void Awake()
	{
		GameController.Instance.RegisterManager(this);
		GameController.Instance.Init();
	}


	void Start()
	{
		ArtworkController.Instance.Init();

		AudioController.Instance.Init();
		GlitchController = GlitchController.Instance;
		GlitchController.Init();
		VideoController.Instance.Init();

		GlitchController.Instance.SetGlitchIntensity(GlitchIntensity.None);
	}

	// Update is called once per frame
	void Update()
	{
		glitchIntensity = GlitchController.Instance.GlitchIntensity;
		_artworksOpened = GameController.Instance.ArtworkOpenCount;

		if (_countingDown)
		{
			if (GlitchController.CanGlitch())
			{
				if (_glitchCountDown <= 0)
				{
					GlitchController.RandomGlitch();
					ResetGlitchTimer();
				}
				else
				{
					_glitchCountDown -= Time.deltaTime;
				}
			}

			if (GlitchController.Instance.Settings.SecondsRequired > 0)
			{
				if (_IntensityCountDown <= 0)
				{
					GameController.Instance.IntensityTimeOut();
					ResetIntensityTimer();
				}
				else
				{
					_IntensityCountDown -= Time.deltaTime;
				}
			}
		}
	}

	internal void ResumeUpdate()
	{
		_countingDown = true;
	}

	private void OnValidate()
	{
		GlitchController.Instance.SetGlitchIntensity(glitchIntensity);
	}

	internal void ResetIntensityTimer()
	{
		_IntensityCountDown = GlitchController.Instance.Settings.SecondsRequired;
		ResetGlitchTimer();
	}

	internal void ResetGlitchTimer()
	{
		_glitchCountDown = GlitchController.Instance.Settings.GlitchFrequency;
	}

	internal void PauseUpdate()
	{
		_countingDown = false;
	}
}
