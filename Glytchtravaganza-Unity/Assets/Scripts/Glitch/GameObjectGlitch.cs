using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class GameObjectGlitch : MonoBehaviour
{
	public enum ObjectType
	{
		Fixture,
		Furniture
	}

	public ObjectType Type = ObjectType.Furniture;
	private MeshFilter _meshFilter;
	public MeshFilter MeshFilter
	{
		get
		{
			if (_meshFilter == null)
			{
				_meshFilter = GetComponent<MeshFilter>();
			}
			return _meshFilter;
		}
	}

	private Mesh _baseMesh;
	private bool _isGLitched = false;
	public bool IsGLitched
	{
		get
		{
			return _isGLitched;
		}
		set
		{
			if (_isGLitched && value == false)
			{
				ResetMesh();
			}

			_isGLitched = value;
		}
	}
	private float _timeSinceLastGlitch = 0f;
	private float _glitchFrequence = 0.1f;
	// Start is called before the first frame update
	void Awake()
	{
		_baseMesh = CopyMesh(MeshFilter.mesh);
	}

	private void Update()
	{
		if (IsGLitched)
		{
			ContinuousGlitchOut();
		}
	}

	private void ReverseNormals()
	{
		Mesh mesh = MeshFilter.mesh;

		Vector3[] normals = mesh.normals;
		for (int i = 0; i < normals.Length; i++)
			normals[i] = -normals[i];
		mesh.normals = normals;

		for (int m = 0; m < mesh.subMeshCount; m++)
		{
			int[] triangles = mesh.GetTriangles(m);
			for (int i = 0; i < triangles.Length; i += 3)
			{
				int temp = triangles[i + 0];
				triangles[i + 0] = triangles[i + 1];
				triangles[i + 1] = temp;
			}
			mesh.SetTriangles(triangles, m);
		}
	}

	private void RandomizeVerts()
	{
		Mesh mesh = MeshFilter.mesh;
		Vector3[] verts = mesh.vertices;
		Dictionary<Vector3, List<int>> dictionary = new Dictionary<Vector3, List<int>>();
		for (int x = 0; x < verts.Length; x++)
		{
			if (!dictionary.ContainsKey(verts[x]))
			{
				dictionary.Add(verts[x], new List<int>());
			}
			dictionary[verts[x]].Add(x);
		}
		foreach (KeyValuePair<Vector3, List<int>> pair in dictionary)
		{
			Vector3 newPos = pair.Key * Random.Range(0.9f, 1.1f);
			foreach (int i in pair.Value)
			{
				verts[i] = newPos;
			}
		}

		mesh.SetVertices(verts);
	}

	private void GlitchOut()
	{

		//if (this.Type == ObjectType.Furniture)
		//{
		if (Random.Range(0, 4) <= 2)
		{
			RandomizeVerts();
		}
		//}

		//if (Random.Range(0, 2) <= 0)
		//{
		//	ReverseNormals();
		//}

		IsGLitched = true;
	}

	private void ContinuousGlitchOut()
	{
		if (Time.unscaledTime >= _timeSinceLastGlitch + _glitchFrequence)
		{
			ResetMesh();
			GlitchOut();
			_timeSinceLastGlitch = Time.unscaledTime;
		}
	}

	private void ResetMesh()
	{
		MeshFilter.mesh = CopyMesh(_baseMesh);
		//MeshFilter.sharedMesh = _baseMesh;
	}

	private Mesh CopyMesh(Mesh targetMesh)
	{
		Mesh copyMesh = new Mesh();
		copyMesh.vertices = targetMesh.vertices;
		copyMesh.triangles = targetMesh.triangles;
		copyMesh.uv = targetMesh.uv;
		copyMesh.normals = targetMesh.normals;
		copyMesh.colors = targetMesh.colors;
		copyMesh.tangents = targetMesh.tangents;
		return copyMesh;
	}
}
