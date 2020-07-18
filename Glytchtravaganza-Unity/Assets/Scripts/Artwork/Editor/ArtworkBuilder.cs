#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using UnityEditor.Formats.Fbx.Exporter;
public static class ArtworkBuilder
{
	[MenuItem("Assets / Create / Build Artwork Object", priority = 0)]
	public static void Test()
	{
		if (Selection.activeObject is Texture)
		{
			Texture tex = Selection.activeObject as Texture;
			Debug.LogError(string.Format("Texture Selected!!! {0} x {1}", (tex.width), (tex.height)));

			if (HasPath(Path.Combine(Application.dataPath, "Artwork/Models")))
			{
				string path = (Path.Combine(Application.dataPath, "Artwork/Models", tex.name +".fbx"));
                GameObject mesh = BuildPaintingObject((tex.width), (tex.height));
				ModelExporter.ExportObject(path, mesh);
				AssetDatabase.Refresh();
			}
		}
		else
		{
			Debug.LogError("Please select a texture!!!");
		}

	}

	private static GameObject BuildPaintingObject(int width, int height)
	{
        GameObject gameObject = new GameObject();
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(0, 0, 0),
            new Vector3(width/1000f, 0, 0),
            new Vector3(0, height/1000f, 0),
            new Vector3(width/1000f, height/1000f, 0)
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