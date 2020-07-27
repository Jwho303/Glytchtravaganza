#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Linq;
using UnityEditor.Formats.Fbx.Exporter;
using System.Runtime.CompilerServices;

public class ShowPopupExample : EditorWindow
{
	string imageWidthText = "265";
	string imageHeightText = "300";
	string frameWidthText = "265";
	string frameHeightText = "300";

	[MenuItem("Assets / Create / Build Artwork Object", priority = 0)]
	static void Init()
	{
		ShowPopupExample window = ScriptableObject.CreateInstance<ShowPopupExample>();
		window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 150);
		window.ShowPopup();
	}

	void OnGUI()
	{

		EditorGUILayout.BeginVertical();
		EditorGUILayout.LabelField(string.Format("Building new artwork {0}", Selection.activeObject.name), EditorStyles.wordWrappedLabel);
		imageWidthText = EditorGUILayout.TextField("Image Width in mm", imageWidthText);
		imageHeightText = EditorGUILayout.TextField("Image Height in mm", imageHeightText);
		frameWidthText = EditorGUILayout.TextField("Frame Width in mm", frameWidthText);
		frameHeightText = EditorGUILayout.TextField("Frame Height in mm", frameHeightText);
		if (GUILayout.Button("Build"))
		{
			float imageWidth = 0;
			float imageHeight = 0;
			float frameWidth = 0;
			float frameHeight = 0;

			if (float.TryParse(imageWidthText, out imageWidth) && float.TryParse(imageHeightText, out imageHeight) && float.TryParse(frameWidthText, out frameWidth) && float.TryParse(frameHeightText, out frameHeight))
			{
				ArtworkBuilder.BuildArtwork(imageWidth, imageHeight, frameWidth, frameHeight);
			}
			else
			{
				Debug.LogFormat("[ArtworkBuilder] measurements are not numbers");
			}
			this.Close();
		}

		EditorGUILayout.EndVertical();
	}
}

public static class ArtworkBuilder
{

	public static void BuildArtwork(float imageWidth, float imageHeight, float frameWidth, float frameHeight)
	{
		if (Selection.activeObject is Texture)
		{
			Texture tex = Selection.activeObject as Texture;


			if (HasPath(Path.Combine(Application.dataPath, "Prefabs/Artwork")))
			{
				AssetDatabase.Refresh();
				//string path = (Path.Combine(Application.dataPath, "Prefabs/Artwork", tex.name + ".prefab"));
				GameObject go = BuildPaintingObject(tex, imageWidth, imageHeight, frameWidth, frameHeight);
				// ModelExporter.ExportObject(path, go);

				// path = AssetDatabase.GenerateUniqueAssetPath(path);

				// PrefabUtility.SaveAsPrefabAssetAndConnect(go, path, InteractionMode.UserAction);
				AssetDatabase.Refresh();
				Debug.LogFormat("[ArtworkBuilder] Asset created: {0}", tex.name);
			}
		}
		else
		{
			Debug.LogError("Please select a texture!!!");
		}

	}

	private static GameObject BuildPaintingObject(Texture tex, float imageWidth, float imageHeight, float frameWidth, float frameHeight)
	{
		GameObject gameObject = new GameObject(tex.name);
		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		Material mat = new Material(Shader.Find("Unlit/Texture"));
		mat.mainTexture = tex;
		meshRenderer.sharedMaterial = mat;

		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

		Mesh mesh = new Mesh();

		imageWidth = imageWidth / 2000f;
		imageHeight = imageHeight / 2000f;

		Vector3[] vertices = new Vector3[4]
		{
			new Vector3(-imageWidth, -imageHeight, 0),
			new Vector3(imageWidth, -imageHeight, 0),
			new Vector3(-imageWidth, imageHeight, 0),
			new Vector3(imageWidth, imageHeight, 0)
		};
		mesh.vertices = vertices;

		int[] tris = new int[6]
		{
            // lower left triangle
            0, 2, 1,
            // upper right triangle
            2, 3, 1
		};
		mesh.triangles = tris;

		Vector3[] normals = new Vector3[4]
		{
			-Vector3.forward,
			-Vector3.forward,
			-Vector3.forward,
			-Vector3.forward
		};
		mesh.normals = normals;

		Vector2[] uv = new Vector2[4]
		{
			new Vector2(0, 0),
			new Vector2(1, 0),
			new Vector2(0, 1),
			new Vector2(1, 1)
		};
		mesh.uv = uv;

		meshFilter.mesh = mesh;

		BoxCollider collider = gameObject.AddComponent<BoxCollider>();
		collider.size = new Vector3(frameWidth / 1000f, 3f, collider.size.x / 10f);
		ArtworkClickable artworkClickable = gameObject.AddComponent<ArtworkClickable>();
		artworkClickable.Key = tex.name;

		ArtworkData artworkData = Resources.LoadAll<ArtworkData>("").FirstOrDefault();
		artworkData.AddArtwork(tex.name);

		string path = Path.Combine(Application.dataPath, "Artwork/Prefabs/PictureFrame.prefab");

		//Debug.Log(path + " : " + PrefabUtility.LoadPrefabContents(path) != null);

		GameObject pictureFrame = GameObject.Instantiate(PrefabUtility.LoadPrefabContents(path));
		pictureFrame.transform.localScale = new Vector3(frameWidth / 1000f, frameHeight / 1000f, pictureFrame.transform.localScale.z);

		pictureFrame.transform.SetParent(gameObject.transform);
		//pictureFrame.transform.localPosition = new Vector3(imageWidth, imageHeight, 0f);

		return gameObject;
	}

	private static bool HasPath(string path)
	{
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}

		return Directory.Exists(path);
	}
}
#endif