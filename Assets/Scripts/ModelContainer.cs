using System;
using System.Collections.Generic;
using UnityEngine;
using VENTUS.UnitySTEPImporter.DataIO;
using VENTUS.UnitySTEPImporter.UnityExtentions;

public class ModelContainer : MonoBehaviour
{
    public ModelType ModelType = ModelType.None;
    public ModelContainer ModelParent;
    public int Id;
    public int ObjectManagerId;
    public string Name;
    public Matrix4x4 Transformation;
    public string Path;
    public List<Mesh> MeshForLoD = new() { null, null, null, null, null, null };
    public Color Color;
    public Texture Texture;
    public int LoD;
    public IntPtr CppModelPointer;
    public Bounds Bounds;

    [SerializeField] private Mesh _mesh;
    public Mesh Mesh
    {
        get => _mesh;
        set
        {
            _mesh = value;

            if (value != null)
                MeshForLoD[LoD] = CopyMesh(value);
        }
    }

    [SerializeField] private MeshFilter     _meshFilter;
    [SerializeField] private MeshRenderer   _meshRenderer;
    [SerializeField] private MeshCollider   _meshCollider;

	public void Initialise()
	{
        name = Name;
        transform.FromMatrix(Transformation);

        if (ModelType == ModelType.Modelpart)
        {
            if (_meshFilter == null)
                _meshFilter = GetComponent<MeshFilter>();
            if (_meshRenderer == null)
                _meshRenderer = GetComponent<MeshRenderer>();
            if (_meshCollider == null)
                _meshCollider = GetComponent<MeshCollider>();

            ApplyMeshAndMaterial();
        }
    }

	public void ApplyMeshAndMaterial()
    {
        if (ModelType != ModelType.Modelpart)
            return;

        _meshFilter.mesh = Mesh;

        _meshCollider.enabled = false;
        _meshCollider.sharedMesh = Mesh;
        _meshCollider.enabled = true;

        //if (material != null)
          //  _meshRenderer.material = material;

        _meshRenderer.material.color = Color;

        if (Texture != null)
            _meshRenderer.material.mainTexture = Texture;
    }

    private Mesh CopyMesh(Mesh oriMesh)
    {
        Mesh copyMesh = new();

        copyMesh.vertices = oriMesh.vertices;
        copyMesh.triangles = oriMesh.triangles;
        copyMesh.uv = oriMesh.uv;

        copyMesh.RecalculateBounds();
        copyMesh.RecalculateNormals();
        copyMesh.RecalculateTangents();

        return copyMesh;
    }
}
