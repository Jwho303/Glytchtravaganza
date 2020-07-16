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
}
