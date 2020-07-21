using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectGlitch : MonoBehaviour
{
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
    // Start is called before the first frame update
    void Awake()
    {
        _baseMesh = MeshFilter.mesh;
    }

    public void ReverseNormals()
	{
		if (_isGLitched)
		{
			return;
		}

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

		_isGLitched = true;
	}

    public void ResetMesh()
	{
		if (!_isGLitched)
		{
			return;
		}

		MeshFilter.mesh = _baseMesh;
		_isGLitched = false;
	}
}
