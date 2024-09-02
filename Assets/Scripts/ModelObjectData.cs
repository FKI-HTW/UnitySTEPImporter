using System;
using System.Collections.Generic;
using UnityEngine;

namespace VENTUS.ModelImporter
{
	[Serializable]
	public class ModelObjectData
    {
		public EModelType ModelType;
        public string Name;
        public Matrix4x4 Transformation;
        public Bounds Bounds;
        public Mesh Mesh;
        public Color Color;
        public Texture2D Texture;

		public List<ModelObjectData> Children = new();
		
		public ModelObjectData() {}

		public ModelObjectData(SingleModelObjectData singleModelObjectData)
		{
			ModelType = singleModelObjectData.ModelType;
			Name = singleModelObjectData.Name;
			Transformation = singleModelObjectData.Transformation;
			Bounds = singleModelObjectData.Bounds;
			Mesh = singleModelObjectData.Mesh;
			Color = singleModelObjectData.Color;
			Texture = singleModelObjectData.Texture;
		}
	}
	
	[Serializable]
	public class SingleModelObjectData
	{
		public EModelType ModelType;
		public string Name;
		public Matrix4x4 Transformation;
		public Bounds Bounds;
		public Mesh Mesh;
		public Color Color;
		public Texture2D Texture;
		
		public SingleModelObjectData() {}

		public SingleModelObjectData(ModelObjectData modelObjectData)
		{
			ModelType = modelObjectData.ModelType;
			Name = modelObjectData.Name;
			Transformation = modelObjectData.Transformation;
			Bounds = modelObjectData.Bounds;
			Mesh = modelObjectData.Mesh;
			Color = modelObjectData.Color;
			Texture = modelObjectData.Texture;
		}
	}
}
