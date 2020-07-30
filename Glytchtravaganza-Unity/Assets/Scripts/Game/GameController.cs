using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController
{
	private static GameController instance;
	public static GameController Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new GameController();
			}

			return instance;
		}
	}
	public int ArtworkOpenCount { get; private set; } = -1;

	private GameManager _gameManager;
	public void RegisterManager(GameManager manager)
	{
		_gameManager = manager;
	}

	internal void Init()
	{
		InputController.Instance.SubscribeToGameObjectHit(Hit);
		ArtworkController.Instance.SubscribeToOpenGallery(GalleryOpen);
		ArtworkController.Instance.SubscribeToCloseGallery(GalleryClose);
		VideoController.Instance.SubscribeToVideoComplete(VideoComplete);
	}

	private void GalleryClose()
	{
		_gameManager.ResumeUpdate();
		ArtworkCount();
	}

	private void ArtworkCount()
	{
		ArtworkOpenCount++;
		ArtworkTriggerCheck();

	}

	private void ArtworkTriggerCheck()
	{
		if (ArtworkOpenCount >= GlitchController.Instance.Settings.ArtClickRequired && GlitchController.Instance.Settings.ArtClickRequired > -1)
		{
			GlitchController.Instance.IncreaseIntensity();
			_gameManager.ResetIntensityTimer();
		}
	}

	private void GalleryOpen(Artwork obj)
	{
		_gameManager.PauseUpdate();
	}

	internal void IntensityTimeOut()
	{
		GlitchController.Instance.IncreaseIntensity();
	}

	private void VideoComplete()
	{
		Debug.Log("VIDEO COMPLETED!");

		if (GlitchController.Instance.GlitchIntensity == GlitchIntensity.Payoff)
		{
			ArtworkOpenCount = 0;
			GlitchController.Instance.SetGlitchIntensity(GlitchIntensity.None);
		}
	}

	private void Hit(GameObject gameObject)
	{
		if (HasComponent<ArtworkClickable>(gameObject))
		{
			ArtworkController.Instance.ArtworkSelected(gameObject.GetComponent<ArtworkClickable>());
		}
	}

	private bool HasComponent<T>(GameObject gameObject)
	{
		return gameObject.GetComponent<T>() != null;
	}

}
