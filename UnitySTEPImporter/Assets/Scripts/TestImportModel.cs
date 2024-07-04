using UnityEngine;
using UnityEditor;

using VENTUS.StepImporter.ModuleAssembly.Model;
using VENTUS.StepImporter.GeomKernel;
using VENTUS.StepImporter.UnityExtentions;
using VENTUS.StepImporter.ModuleAssembly.Mesh;
using static VENTUS.StepImporter.ModuleAssembly.Model.ModelBase;
using VENTUS.StepImporter.Assets.UnitySTEPImporter.Runtime.Scripts.ModuleAssembly;
using VENTUS.StepImporter.ModuleAssembly.OldModel;

public class TestImportModel : MonoBehaviour
{
	[SerializeField] private string _path;

	[SerializeField] private Bounds _bounds;
	[SerializeField] private GameObject _modelObjectPrefab;
	[SerializeField] private GameObject _modelProductPrefab;
	[SerializeField] private GameObject _modelPartPrefab;

	[SerializeField] private bool _drawOriginalBoundingBox;
	[SerializeField] private bool _drawScaledBoundingBox;

	private Bounds _originalBounds;
	private Bounds _scaledBounds;

	private void OnDrawGizmos()
	{
		if (_drawOriginalBoundingBox)
		{
			Gizmos.color = UnityEngine.Color.red;
			Gizmos.DrawWireCube(_originalBounds.center, _originalBounds.size);
		}
		if (_drawScaledBoundingBox)
		{
			Gizmos.color = UnityEngine.Color.green;
			Gizmos.DrawWireCube(_scaledBounds.center, _scaledBounds.size);
		}
	}

	public void ImportModel() {

        ImportModuleAssembly modAss = new ImportModuleAssembly(2);
        ModelObject modelObject = modAss.transferModelObject(_path);
        Modelobject modelobject = ModelObjectConverter.convertToOldModel(modelObject, _path);

        if (modelobject == null)
            return;

        _originalBounds = modelobject.Bounds;
        modelobject = AutoPositionAndScaleModel(modelobject, _bounds);
        _scaledBounds = modelobject.Bounds;

        modelobject.ObjectManagerId = 0;
        modelobject.Path = _path;

        foreach (Submodel submodel in modelobject.Submodels)
            SetObjectManagerID(submodel, 0);

        SpawnModel(modelobject);
    }

    private Modelobject AutoPositionAndScaleModel(Modelobject modelobject, Bounds scaleBounds)
    {
        Vector3 position = modelobject.Transformation.ExtractPosition();
        Quaternion rotation = modelobject.Transformation.ExtractRotation();
        Vector3 scale = modelobject.Transformation.ExtractScale();

        Bounds bounds = new Bounds(modelobject.Bounds.center, modelobject.Bounds.size);
        bounds.center += position;

        if (scaleBounds.center != modelobject.Bounds.center)
        {
            Vector3 offCenter = bounds.center - scaleBounds.center;
            offCenter.y = 0.0f;

            bounds.center -= offCenter;

            position -= offCenter;
        }

        if (!(scaleBounds.size.x >= modelobject.Bounds.size.x) ||
            !(scaleBounds.size.y >= modelobject.Bounds.size.y) ||
            !(scaleBounds.size.z >= modelobject.Bounds.size.z))
        {
            float overScaleFactor = 1.0f;

            overScaleFactor = Mathf.Min(overScaleFactor, scaleBounds.size.x / modelobject.Bounds.size.x);
            overScaleFactor = Mathf.Min(overScaleFactor, scaleBounds.size.y / modelobject.Bounds.size.y);
            overScaleFactor = Mathf.Min(overScaleFactor, scaleBounds.size.z / modelobject.Bounds.size.z);

            bounds.size *= overScaleFactor;
            scale *= overScaleFactor;
        }

        position.y = bounds.extents.y;
        bounds.center = new Vector3(bounds.center.x, position.y, bounds.center.z);

        modelobject.Bounds = bounds;
        modelobject.Transformation = Matrix4x4.TRS(position, rotation, scale);

        return modelobject;
    }


    private void SetObjectManagerID(Submodel submodel, int objectManagerId)
    {
        submodel.ObjectManagerId = objectManagerId;

        if (submodel.GetType() == typeof(Modelproduct))
        {
            Modelproduct modelproduct = (Modelproduct)submodel;

            foreach (Modelproduct mp in modelproduct.Modelproducts)
                SetObjectManagerID(mp, objectManagerId);

            foreach (Modelpart mp in modelproduct.Modelparts)
                SetObjectManagerID(mp, objectManagerId);
        }
    }

    private GameObject SpawnModel(Modelobject modelobject, Transform transformation = null)
    {
        GameObject go = Instantiate(_modelObjectPrefab);

        ModelContainer modelContainer = go.GetComponent<ModelContainer>();
        if (transformation != null)
            modelContainer.Transformation = Matrix4x4.TRS(transformation.position, transformation.rotation, transformation.localScale);
        else
            modelContainer.Transformation = modelobject.Transformation;
        modelContainer.ModelType = VENTUS.StepImporter.ModuleAssembly.OldModel.ModelType.Modelobject;
        modelContainer.Id = modelobject.Id;
        modelContainer.ObjectManagerId = modelobject.ObjectManagerId;
        modelContainer.Name = modelobject.Name;
        modelContainer.Path = modelobject.Path;
        modelContainer.LoD = 2;
        modelContainer.Mesh = null;
        modelContainer.Color = UnityEngine.Color.white;
        modelContainer.Texture = null;
        modelContainer.CppModelPointer = modelobject.CppModelPointer;
        modelContainer.Initialise();

        foreach (Submodel submodel in modelobject.Submodels)
            SpawnSubModel(submodel, go, modelContainer);

        return go;
    }

    private void SpawnSubModel(Submodel submodel, GameObject parent, ModelContainer container)
    {
        if (submodel.GetType() == typeof(Modelpart))
        {
            Modelpart modelpart = (Modelpart)submodel;

            GameObject go = Instantiate(_modelPartPrefab, parent.transform);

            ModelContainer modelContainer = go.GetComponent<ModelContainer>();
            modelContainer.ModelParent = container;
            modelContainer.ModelType = VENTUS.StepImporter.ModuleAssembly.OldModel.ModelType.Modelpart;
            modelContainer.Id = modelpart.Id;
            modelContainer.ObjectManagerId = modelpart.ObjectManagerId;
            modelContainer.Name = modelpart.Name;
            modelContainer.Transformation = modelpart.Transformation;
            modelContainer.LoD = 2;
            modelContainer.Mesh = modelpart.Modelmesh.Mesh;
            modelContainer.Color = modelpart.Modelmesh.Graphicinfo.Color;
            modelContainer.Texture = modelpart.Modelmesh.Graphicinfo.Texture;
            modelContainer.CppModelPointer = modelpart.CppModelPointer;
            modelContainer.Initialise();
        }
        else if (submodel.GetType() == typeof(Modelproduct))
        {
            Modelproduct modelproduct = (Modelproduct)submodel;

            GameObject go = Instantiate(_modelProductPrefab, parent.transform);

            ModelContainer modelContainer = go.GetComponent<ModelContainer>();
            modelContainer.ModelParent = container;
            modelContainer.ModelType = VENTUS.StepImporter.ModuleAssembly.OldModel.ModelType.Modelproduct;
            modelContainer.Id = modelproduct.Id;
            modelContainer.ObjectManagerId = modelproduct.ObjectManagerId;
            modelContainer.Name = modelproduct.Name;
            modelContainer.Transformation = modelproduct.Transformation;
            modelContainer.LoD = 2;
            modelContainer.Mesh = null;
            modelContainer.Color = UnityEngine.Color.white;
            modelContainer.Texture = null;
            modelContainer.CppModelPointer = modelproduct.CppModelPointer;
            modelContainer.Initialise();

            foreach (Modelproduct mp in modelproduct.Modelproducts)
                SpawnSubModel(mp, go, container);

            foreach (Modelpart mp in modelproduct.Modelparts)
                SpawnSubModel(mp, go, container);
        }
    }
}

[CustomEditor(typeof(TestImportModel))]
internal class TestImportModelEditor : Editor
{
    private TestImportModel _importer;

    private void OnEnable()
    {
        _importer = (TestImportModel)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Import Model"))
            _importer.ImportModel();
    }
}
