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
	private int _artworkOpenCount = 0;

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
	}

	private void GalleryClose()
	{

	}

	private void GalleryOpen(Artwork obj)
	{
		_artworkOpenCount++;

		if (_artworkOpenCount >= 2 && GlitchController.Instance.GlitchIntensity == GlitchIntensity.None)
		{
			GlitchController.Instance.SetGlitchIntensity(GlitchIntensity.Low);
		}

		if (_artworkOpenCount >= 4 && GlitchController.Instance.GlitchIntensity == GlitchIntensity.Low)
		{
			GlitchController.Instance.SetGlitchIntensity(GlitchIntensity.Medium);
		}

		if (_artworkOpenCount >= 6 && GlitchController.Instance.GlitchIntensity == GlitchIntensity.Medium)
		{
			GlitchController.Instance.SetGlitchIntensity(GlitchIntensity.High);
		}

		if (_artworkOpenCount >= 8 && GlitchController.Instance.GlitchIntensity == GlitchIntensity.High)
		{
			_artworkOpenCount = 0;
			GlitchController.Instance.SetGlitchIntensity(GlitchIntensity.None);
		}
	}

	private void Hit(GameObject gameObject)
	{
		Debug.Log("Hit : " + gameObject.name);
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
