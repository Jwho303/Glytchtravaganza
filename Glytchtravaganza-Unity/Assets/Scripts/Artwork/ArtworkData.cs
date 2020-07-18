using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

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

	internal void NewArtwork()
	{
		Artworks.Add(Artwork.MakeDefault());
	}

	internal void RemoveArtwork(int index)
	{
		Artworks.RemoveAt(index);
	}
}

[System.Serializable]
public class Artwork
{
	public string Key;
	public List<ArtContent> ArtContents = new List<ArtContent>();

	public void AddImage()
	{
		ArtContents.Add(ArtContent.ImageContent());
	}

	public void AddText()
	{
		ArtContents.Add(ArtContent.TextContent());
	}

	public Artwork()
	{
	}

	public static Artwork MakeDefault()
	{
		return new Artwork();
	}

#if UNITY_EDITOR
	public void Draw()
	{
		Key = EditorGUILayout.TextField("Key", Key);

		ArtContent contentToRemove = null;
		EditorGUILayout.BeginVertical();
		for (int i = 0; i < ArtContents.Count; i++)
		{

			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField(i + ": ");
				GUIButton removebutton = (new GUIButton("X", () => contentToRemove = ArtContents[i]));
				removebutton.Draw();
			}
			EditorGUILayout.EndHorizontal();
			ArtContents[i].Draw();
		}
		EditorGUILayout.EndVertical();

		if (contentToRemove != null)
		{
			ArtContents.Remove(contentToRemove);
		}
	}
#endif
}

[System.Serializable]
public class ArtContent
{
	public ContentType ContentType = ContentType.Image;
	public Texture2D Image;
	public string Text;

#if UNITY_EDITOR
	public void Draw()
	{
		if (ContentType == ContentType.Image)
		{
			Image = (Texture2D)EditorGUILayout.ObjectField(Image, typeof(Texture2D), false);
			if (Image != null)
			{
				GUILayout.Label(AssetPreview.GetAssetPreview(Image));
			}
		}
		else if (ContentType == ContentType.Text)
		{
			Text = EditorGUILayout.TextArea(Text,GUILayout.MinHeight(100));
		}
	}

#endif

	public static ArtContent ImageContent()
	{
		ArtContent content = new ArtContent();
		content.ContentType = ContentType.Image;
		return content;
	}

	public static ArtContent TextContent()
	{
		ArtContent content = new ArtContent();
		content.ContentType = ContentType.Text;
		return content;
	}
}
public enum ContentType
{
	Image,
	Text
}
