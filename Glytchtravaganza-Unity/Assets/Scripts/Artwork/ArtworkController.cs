using System.Collections;
using System.Collections.Generic;
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

	public void RegisterManager(ArtworkManager manager)
	{
		_artworkManager = manager;
	}

	public void ArtworkSelected(ArtworkClickable artworkClickable)
	{
		_artworkManager.ArtworkSelected(artworkClickable);
	}

	public void ArtworkClosed()
	{
		
	}

}
