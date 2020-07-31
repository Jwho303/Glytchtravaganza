using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class ArtworkController
{
	private static ArtworkController instance;
	public static ArtworkController Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new ArtworkController();
			}

			return instance;
		}
	}

	private ArtworkManager _artworkManager;
	private ArtworkData _artworkData;

	private Action<Artwork> _galleryOpenSubscription = delegate { };
	private Action _galleryClosedSubscription = delegate { };
	public bool IsOpen { get; private set; }

	public void Init()
	{
		_artworkData = Resources.LoadAll<ArtworkData>("").FirstOrDefault();
	}

	public void RegisterManager(ArtworkManager manager)
	{
		_artworkManager = manager;
	}

	internal void OpenArtWork(string key)
	{
		_artworkManager.ArtworkSelected(_artworkData.Get(key));
		_galleryOpenSubscription(_artworkData.Get(key));
		IsOpen = true;
	}

	public void SubscribeToOpenGallery(Action<Artwork> action)
	{
		_galleryOpenSubscription += action;
	}

	public void SubscribeToCloseGallery(Action action)
	{
		_galleryClosedSubscription += action;
	}

	public void ArtworkSelected(ArtworkClickable artworkClickable)
	{
		_artworkManager.ArtworkSelected(_artworkData.Get(artworkClickable.Key));
		_galleryOpenSubscription(_artworkData.Get(artworkClickable.Key));
		IsOpen = true;
	}

	public void ArtworkClosed()
	{
		_galleryClosedSubscription();
		IsOpen = false;
	}

	public void NextItem()
	{
		_artworkManager.NextItem();
	}

	public void PreviousItem()
	{
		_artworkManager.PreviousItem();
	}

}
