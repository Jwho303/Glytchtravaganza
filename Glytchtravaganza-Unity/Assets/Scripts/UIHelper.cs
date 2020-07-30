using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIHelper 
{
	public static int MinimumDisplayLength
	{
		get
		{
			if (Camera.main.pixelHeight < Camera.main.pixelWidth)
			{
				return Camera.main.pixelHeight;
			}
			else
			{
				return Camera.main.pixelWidth;
			}
		}
	}

	public static int MaximumDisplayLength
	{
		get
		{
			if (Camera.main.pixelHeight > Camera.main.pixelWidth)
			{
				return Camera.main.pixelHeight;
			}
			else
			{
				return Camera.main.pixelWidth;
			}
		}
	}

	public static Vector2 FarOffScreen
	{
		get
		{
			return new Vector2(9999, 9999);
		}
	}

	public static float Map(float value, float low1, float high1, float low2, float high2)
	{
		return low2 + (value - low1) * (high2 - low2) / (high1 - low1);
	}

	public static void TurnOnCanvas(CanvasGroup canvasGroup)
	{
		canvasGroup.alpha = 1f;
		canvasGroup.interactable = true;
		canvasGroup.blocksRaycasts = true;
	}

	public static void TurnOffCanvas(CanvasGroup canvasGroup)
	{
		canvasGroup.alpha = 0f;
		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = false;
	}
}
