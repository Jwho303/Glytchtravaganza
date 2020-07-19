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

public static class ArtworkBuilder
{
    private static Vector3 position;

	[MenuItem("Assets / Create / Build Artwork Object", priority = 0)]
	public static void Test()
	{
		if (Selection.activeObject is Texture)
		{
			Texture tex = Selection.activeObject as Texture;
			

			if (HasPath(Path.Combine(Application.dataPath, "Prefabs/Artwork")))
			{
                AssetDatabase.Refresh();
                string path = (Path.Combine(Application.dataPath, "Prefabs/Artwork", tex.name +".prefab"));
                GameObject go = BuildPaintingObject(tex);
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

	private static GameObject BuildPaintingObject(Texture tex)
	{
        GameObject gameObject = new GameObject(tex.name);
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        Material mat = new Material(Shader.Find("Standard"));
        mat.mainTexture = tex;
        meshRenderer.sharedMaterial = mat;

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(0, 0, 0),
            new Vector3(tex.width/1000f, 0, 0),
            new Vector3(0, tex.height/1000f, 0),
            new Vector3(tex.width/1000f, tex.height/1000f, 0)
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
        collider.size = new Vector3(collider.size.x, collider.size.y, collider.size.x/10f);
        ArtworkClickable artworkClickable = gameObject.AddComponent<ArtworkClickable>();
        artworkClickable.Key = tex.name;

        ArtworkData artworkData = Resources.LoadAll<ArtworkData>("").FirstOrDefault();
        artworkData.AddArtwork(tex.name);
       
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