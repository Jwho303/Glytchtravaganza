using System.Collections;
using System.Collections.Generic;
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

	public void Init()
	{
		_artworkData = Resources.LoadAll<ArtworkData>("").FirstOrDefault();
	}

	public void RegisterManager(ArtworkManager manager)
	{
		_artworkManager = manager;
	}

	public void ArtworkSelected(ArtworkClickable artworkClickable)
	{
		_artworkManager.ArtworkSelected(_artworkData.Get(artworkClickable.Key));
	}

	public void ArtworkClosed()
	{
		
	}

}
