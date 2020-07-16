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

	private GameManager _gameManager;
	public void RegisterManager(GameManager manager)
	{
		_gameManager = manager;
	}

	internal void Init()
	{
		InputController.Instance.SubscribeToGameObjectHit(Hit);
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
