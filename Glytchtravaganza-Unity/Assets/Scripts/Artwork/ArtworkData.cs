using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArtworkData", menuName = "ArtworkData", order = 1)]
public class ArtworkData : ScriptableObject
{
	public List<Artwork> Artworks = new List<Artwork>();
	public Artwork Get(string key)
	{
		for (int i = 0; i < Artworks.Count; i++)
		{
			if (Artworks[i].Key == key)
			{
				return Artworks[i];
			}
		}

		Debug.LogErrorFormat("[{0}] Could not find artwork with key {1}", this.name, key);
		return null;
	}
}

[System.Serializable]
public class Artwork
{
	public string Key;
	public int SlideCount = 5;
}
