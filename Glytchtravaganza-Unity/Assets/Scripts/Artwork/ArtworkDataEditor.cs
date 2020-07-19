#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(ArtworkData))]
public class ArtworkDataEditor : Editor
{
	private int _viewIndex = -1;
	ArtworkData data;
	public override void OnInspectorGUI()
	{
		data = (ArtworkData)target;

		EditorGUILayout.BeginVertical();
		DrawNavigationControls();
		if (_viewIndex == -1)
		{
			DrawIndex(data.Artworks);
		}
		else
		{
			DrawArtworkControls(data.Artworks[_viewIndex]);
			DrawArtwork(data.Artworks[_viewIndex]);
		}
		EditorGUILayout.EndVertical();

		serializedObject.ApplyModifiedProperties();
	}

	private void OnDisable()
	{
		serializedObject.ApplyModifiedProperties();
		EditorUtility.SetDirty(data);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
	}

	private void OnValidate()
	{
		serializedObject.ApplyModifiedProperties();
		EditorUtility.SetDirty(data);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();

	}

	private void DrawArtworkControls(Artwork artwork)
	{
		EditorGUILayout.Space();
		EditorGUILayout.BeginHorizontal();
		List<GUIButton> buttons = new List<GUIButton>();

		buttons.Add(new GUIButton("Add Image", artwork.AddImage));
		buttons.Add(new GUIButton("Add Text", artwork.AddText));

		foreach (var item in buttons)
		{
			item.Draw();
		}

		EditorGUILayout.EndHorizontal();
	}

	private void DrawArtwork(Artwork artwork)
	{
		EditorGUILayout.Space();
		EditorGUILayout.LabelField(string.Format("{0} : {1} | Items : {2}", _viewIndex, artwork.Key, artwork.ArtContents.Count));
		artwork.Draw();
	}

	private void DrawNavigationControls()
	{
		EditorGUILayout.BeginHorizontal();

		List<GUIButton> buttons = new List<GUIButton>();

		buttons.Add(new GUIButton("Home", Home, () => { return _viewIndex > -1; }));
		buttons.Add(new GUIButton("Next", NextItem, () => { return _viewIndex < data.Artworks.Count - 1; }));
		buttons.Add(new GUIButton("Prev", PreviousItem, () => { return _viewIndex > -1; }));
		buttons.Add(new GUIButton("New", NewItem));
		buttons.Add(new GUIButton("Remove", RemoveItem, () => { return _viewIndex > -1; }));

		foreach (var item in buttons)
		{
			item.Draw();
		}

		EditorGUILayout.EndHorizontal();
	}

	private void Home()
	{
		_viewIndex = -1;
	}

	private void NextItem()
	{
		_viewIndex = (_viewIndex < (data.Artworks.Count - 1)) ? _viewIndex + 1 : (data.Artworks.Count - 1);
	}

	private void PreviousItem()
	{
		_viewIndex = (_viewIndex > -1) ? _viewIndex - 1 : -1;
	}

	private void NewItem()
	{
		data.NewArtwork();
		_viewIndex = data.Artworks.Count - 1;
	}
	private void RemoveItem()
	{
		data.RemoveArtwork(_viewIndex);
		_viewIndex = _viewIndex - 1;
	}

	private void DrawIndex(List<Artwork> artworks)
	{

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Artwork Index");
		EditorGUILayout.BeginVertical();

		for (int i = 0; i < artworks.Count; i++)
		{
			EditorGUILayout.BeginHorizontal();
			GUIButton goToButton = new GUIButton(i.ToString(), () => { _viewIndex = i; });
			goToButton.Draw();
			string s = string.Format("{0}", artworks[i].Key);
			EditorGUILayout.LabelField(s);
			EditorGUILayout.EndHorizontal();
		}

		EditorGUILayout.EndVertical();
	}
}

public class GUIButton
{
	public string DisplayText;
	public Action Action;
	public Func<bool> CanDisplay;

	public GUIButton(string displayText, Action action, Func<bool> canDisplay)
	{
		DisplayText = displayText;
		Action = action;
		CanDisplay = canDisplay;
	}

	public GUIButton(string displayText, Action action)
	{
		DisplayText = displayText;
		Action = action;
		CanDisplay = () => { return true; };
	}

	public void Draw()
	{
		if (CanDisplay())
		{
			if (GUILayout.Button(DisplayText))
			{
				Debug.Log("Pressed " + DisplayText);
				Action();
			}
		}
	}
}
#endif