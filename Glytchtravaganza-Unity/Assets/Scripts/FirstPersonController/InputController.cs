using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController
{
	private static InputController instance;
	public static InputController Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new InputController();
			}

			return instance;
		}
	}

	private Action<float, float> _translateSubscription = delegate { };
	private Action<float, float> _rotationSubscription = delegate { };
	private Action<Vector2> _tapSubscription = delegate { };
	private Action<GameObject> _gameObjectHitSubscription = delegate { };
	private Action<ArtworkClickable> _artworkHitSubscription = delegate { };
	private Action _artworkMissedSubscription = delegate { };

	public void SubscribeToTranslate(Action<float, float> action)
	{
		_translateSubscription += action;
	}

	public void SubscribeToRotation(Action<float, float> action)
	{
		_rotationSubscription += action;
	}
	public void SubscribeToTap(Action<Vector2> action)
	{
		_tapSubscription += action;
	}

	public void SubscribeToGameObjectHit(Action<GameObject> action)
	{
		_gameObjectHitSubscription += action;
	}

	public void SubscribeToArtworkHit(Action<ArtworkClickable> action)
	{
		_artworkHitSubscription += action;
	}

	public void SubscribeToArtworkMissed(Action action)
	{
		_artworkMissedSubscription += action;
	}

	public void Translate(float direction, float magnitude)
	{
		_translateSubscription(direction, magnitude);
	}
	public void Rotate(float direction, float magnitude)
	{
		_rotationSubscription(direction, magnitude);
	}

	internal void MissedArtwork()
	{
		_artworkMissedSubscription();
	}

	internal void HitArtwork(ArtworkClickable artwork)
	{
		_artworkHitSubscription(artwork);
	}

	public void Tap(Vector2 position)
	{
		_tapSubscription(position);
	}

	public void Hit(GameObject gameObject)
	{
		_gameObjectHitSubscription(gameObject);
	}

	internal void Interaction()
	{
		throw new NotImplementedException();
	}
}
