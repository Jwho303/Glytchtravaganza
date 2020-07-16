using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtworkManager : MonoBehaviour
{
	[SerializeField]
	private RectTransform _galleryContainer;
	[SerializeField]
	private GalleryView _galleryPrefab;
	[SerializeField]
	private int _galleryIndex = 0;
	[SerializeField]
	private List<GalleryView> _galleryItems = new List<GalleryView>();

	private Vector2 _offscreenRight
	{
		get
		{
			return new Vector2(Screen.currentResolution.width / 2f + UIHelper.MinimumDisplayLength / 2f, 0f);
		}
	}
	private Vector2 _offscreenLeft
	{
		get
		{
			return new Vector2(-_offscreenRight.x, 0f);
		}
	}
	// Start is called before the first frame update
	void Start()
	{
		ArtworkController.Instance.RegisterManager(this);
	}

	internal void ArtworkSelected(Artwork artwork)
	{
		Debug.Log(artwork.Key);
		BuildGallery(artwork);
	}

	internal void BuildGallery(Artwork artwork)
	{
		ClearGallery();
		for (int i = 0; i < artwork.SlideCount; i++)
		{
			_galleryItems.Add(CreateView(artwork));
		}
		SetGalleryPosition(_galleryIndex);
	}

	private void SetGalleryPosition(int galleryIndex)
	{
		for (int i = 0; i < _galleryItems.Count; i++)
		{
			Vector2 pos = Vector2.zero;
			if (i < galleryIndex)
			{
				pos = _offscreenLeft;
			}
			else if (i > galleryIndex)
			{
				pos = _offscreenRight;
			}

			_galleryItems[i].RectTransform.anchoredPosition = pos;
		}
	}

	[ContextMenu("Next")]
	public void NextItem()
	{
		_galleryIndex = (_galleryIndex < (_galleryItems.Count - 1)) ? _galleryIndex + 1 : 0;
		SetGalleryPosition(_galleryIndex);
	}

	[ContextMenu("Prev")]
	public void PreviousItem()
	{
		_galleryIndex = (_galleryIndex > 0) ? _galleryIndex - 1 : (_galleryItems.Count - 1);
		SetGalleryPosition(_galleryIndex);
	}

	private GalleryView CreateView(Artwork artwork)
	{
		GalleryView galleryView = Instantiate(_galleryPrefab, _galleryContainer.transform);
		return galleryView;
	}

	private void ClearGallery()
	{
		for (int i = 0; i < _galleryItems.Count; i++)
		{
			Destroy(_galleryItems[i].gameObject);
		}
		_galleryIndex = 0;
		_galleryItems.Clear();
	}
}
