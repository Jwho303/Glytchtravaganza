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
    private List<GalleryView> _galleryItems = new List<GalleryView>();
    // Start is called before the first frame update
    void Start()
    {
        ArtworkController.Instance.RegisterManager(this);
    }

    // Update is called once per frame
    void Update()
    {
        
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
        _galleryItems.Clear();
	}
}
