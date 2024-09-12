using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using VENTUS.ModelImporter;
using VENTUS.UnitySTEPImporter.UnityExtentions;

public class TestImportModel : MonoBehaviour
{
	[SerializeField] private string _path;

	[SerializeField] private Bounds _bounds;
	[SerializeField] private GameObject _modelObjectPrefab;
	[SerializeField] private GameObject _modelProductPrefab;
	[SerializeField] private GameObject _modelPartPrefab;

	[SerializeField] private bool _drawOriginalBoundingBoxes;
	[SerializeField] private bool _drawTransformedBoundingBoxes;

	private List<Bounds> _originalBounds = new();
	private List<Bounds> _transformedBounds = new();

	private void OnDrawGizmos()
	{
		if (_drawOriginalBoundingBoxes)
         foreach (Bounds submodelBounds in _originalBounds)
         {
            Gizmos.color = UnityEngine.Color.red;
            Gizmos.DrawWireCube(submodelBounds.center, submodelBounds.size);
         }
      if (_drawTransformedBoundingBoxes)
			foreach (Bounds submodelBounds in _transformedBounds)
			{
				Gizmos.color = UnityEngine.Color.blue;
				Gizmos.DrawWireCube(submodelBounds.center, submodelBounds.size);
			}
	}

	public void ImportModel()
	{
      _originalBounds.Clear();
		_transformedBounds.Clear();

      ModelObjectData modelObjectData = STEPImporter.ParseFile(_path);
		if (modelObjectData == null)
		{
			Debug.LogError("Something went wrong while creating the model!");
			return;
		}

		SpawnModel(modelObjectData);
	}

	private void SpawnModel(ModelObjectData modelObjectData)
	{
      // spawn parent
      GameObject go = Instantiate(_modelObjectPrefab);
		ModelContainer modelContainer = go.GetComponent<ModelContainer>();
		modelContainer.InitializeFromData(modelObjectData, _path);

      // spawn children
      foreach (var child in modelObjectData.Children)
			SpawnSubModel(child, modelContainer);
	}

	private void SpawnSubModel(ModelObjectData modelObjectData, ModelContainer parentContainer)
	{
      _originalBounds.Add(modelObjectData.Bounds);
		GameObject go;

		switch (modelObjectData.ModelType)
		{
			case EModelType.ModelPart:
				go = Instantiate(_modelPartPrefab, parentContainer.transform);
				break;

			case EModelType.ModelProduct:
				go = Instantiate(_modelProductPrefab, parentContainer.transform);
				break;

			default:
				go = null;
				break;
		}
		if (go != null) 
		{
				go.transform.FromMatrix(modelObjectData.Transformation);
				ModelContainer modelContainer = go.GetComponent<ModelContainer>();
				modelContainer.InitializeFromData(modelObjectData, _path, parentContainer);
            _transformedBounds.Add(TransformBounds(go.transform, modelObjectData.Bounds));

            foreach (var child in modelObjectData.Children)
		         SpawnSubModel(child, modelContainer);
		}
	}

	private Bounds TransformBounds(Transform transform, Bounds bounds)
	{
		Vector3 centerTransformed = transform.TransformPoint(bounds.center);
		Vector3 sizeTransformed = transform.TransformDirection(bounds.size);

      return new Bounds(centerTransformed, sizeTransformed);
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
