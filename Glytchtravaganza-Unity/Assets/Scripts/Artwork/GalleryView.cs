using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GalleryView : MonoBehaviour
{
	[SerializeField]
	private RawImage _image;

	[SerializeField]
	private TextMeshProUGUI _text;

	public RectTransform RectTransform
	{
		get
		{
			return this.GetComponent<RectTransform>();
		}
	}

	internal void SetContent(ArtContent content)
	{
		if (content.ContentType == ContentType.Image)
		{
			float maxSize = RectTransform.sizeDelta.y;
			_image.texture = content.Image;
			_image.SetNativeSize();
			float ratio = _image.rectTransform.sizeDelta.y / _image.rectTransform.sizeDelta.x;
			_image.rectTransform.sizeDelta = (_image.rectTransform.sizeDelta.x > _image.rectTransform.sizeDelta.y) ? new Vector2(maxSize, maxSize*ratio) : new Vector2(maxSize/ratio, maxSize);
			_text.enabled = false;
		}
		else if (content.ContentType == ContentType.Text)
		{
			_text.SetText(content.Text);
			_image.enabled = false;
		}
	}
}
