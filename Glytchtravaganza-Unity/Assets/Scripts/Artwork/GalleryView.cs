using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalleryView : MonoBehaviour
{
	public RectTransform RectTransform
	{
		get
		{
			return this.GetComponent<RectTransform>();
		}
	}
}
