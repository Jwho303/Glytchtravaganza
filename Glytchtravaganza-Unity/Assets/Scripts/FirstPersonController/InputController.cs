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

	public void SubscribeToTranslate (Action<float, float> action)
	{
		_translateSubscription += action;
	}

	public void SubscribeToRotation(Action<float, float> action)
	{
		_rotationSubscription += action;
	}

	public void Translate(float direction, float magnitude)
	{
		_translateSubscription(direction, magnitude);
	}
	public void Rotate(float direction, float magnitude)
	{
		_rotationSubscription(direction, magnitude);
	}

}
