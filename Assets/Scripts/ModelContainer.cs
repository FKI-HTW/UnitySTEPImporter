using System;
using UnityEngine;
using VENTUS.ModelImporter;
using VENTUS.UnitySTEPImporter.UnityExtentions;

public class ModelContainer : MonoBehaviour
{
    public EModelType mType = EModelType.ModelParent;
    public ModelContainer mParentContainer;
    public int mId = 0; // not used yet
    public int mObjectManagerId = 0;  // not used yet
    public string mName;
    public Matrix4x4 mTransformation;
    public string mPath;
//    public List<Mesh> mMeshForLoD = new() { null, null, null, null, null, null }; // not used yet
    public Color mColor;
    public Texture mTexture;
    public int mLoD = 2;  // not used yet
    public IntPtr mCppModelPointer = IntPtr.Zero; //not used yet
    public Bounds mBounds;

    [SerializeField] private Mesh _mesh;
    public Mesh Mesh
    {
        get => _mesh;
        set
        {
            _mesh = value;

            //if (value != null)
            //    mMeshForLoD[mLoD] = CopyMesh(value);
        }
    }

    [SerializeField] private MeshFilter     _meshFilter;
    [SerializeField] private MeshRenderer   _meshRenderer;
    [SerializeField] private MeshCollider   _meshCollider;

	public void Initialise()
	{
        name = mName;
        transform.FromMatrix(mTransformation);

        if (mType == EModelType.ModelPart)
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

   public void InitializeFromData(ModelObjectData modelObjectData, string path, ModelContainer parentContainer = null)
   {
       
       mParentContainer = parentContainer;
       if (mParentContainer == null)
         mType = EModelType.ModelParent;
       else
         mType = modelObjectData.ModelType;
       mName = modelObjectData.Name;
       mTransformation = modelObjectData.Transformation;
       mPath = path;
       mColor = modelObjectData.Color;
       mTexture = modelObjectData.Texture;
       mBounds = modelObjectData.Bounds;
       Mesh = modelObjectData.Mesh;

       Initialise();
   }

   public void ApplyMeshAndMaterial()
    {
        if (mType != EModelType.ModelPart)
            return;

        _meshFilter.mesh = Mesh;

        _meshCollider.enabled = false;
        _meshCollider.sharedMesh = Mesh;
        _meshCollider.enabled = true;

        //if (material != null)
          //  _meshRenderer.material = material;

        _meshRenderer.material.color = mColor;

        if (mTexture != null)
            _meshRenderer.material.mainTexture = mTexture;
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
