using UnityEngine;
using VENTUS.UnitySTEPImporter.DataIO;

namespace VENTUS.ModelImporter
{
    public static class STEPImporter
    {
        public static ModelObjectData ParseFile(string path)
        {
            ImportModel importModel = new();
            importModel.InitGeomKernel();
            int objectManagerId = importModel.LoadModelobjectFromFile(path, 4);
            Modelobject modelObject = importModel.getModelobjectFromKernel(objectManagerId);

            if (modelObject == null)
            {
                Debug.Log("The given file could not be loaded!");
                return null;
            }

            modelObject.ObjectManagerId = objectManagerId;
            modelObject.Path = path;

            ModelObjectData modelParent = new()
            {
                ModelType = EModelType.ModelParent,
                Name = modelObject.Name,
                Transformation = modelObject.Transformation * Matrix4x4.Rotate(Quaternion.Euler(90, 0, 0)),
                Bounds = modelObject.Bounds,
                Mesh = null,
                Color = Color.white,
                Texture = null
            };

            foreach (var subModel in modelObject.Submodels)
                modelParent.Children.Add(CreateModelObjectData(subModel));
            
            return modelParent;
        }

        private static ModelObjectData CreateModelObjectData(Submodel subModelObject)
        {
            switch (subModelObject)
            {
                case Modelpart modelPart:
                {
                    return new()
                    {
                        ModelType = EModelType.ModelPart,
                        Name = modelPart.Name,
                        Transformation = modelPart.Transformation,
                        Bounds = modelPart.Bounds,
                        Mesh = modelPart.Modelmesh.Mesh,
                        Color = modelPart.Modelmesh.Graphicinfo.Color,
                        Texture = modelPart.Modelmesh.Graphicinfo.Texture,
                    };
                }
                case Modelproduct modelProduct:
                {
                    ModelObjectData modelObject = new()
                    {
                        ModelType = EModelType.ModelProduct,
                        Name = modelProduct.Name,
                        Transformation = modelProduct.Transformation,
                        Bounds = modelProduct.Bounds,
                        Mesh = null,
                        Color = Color.white,
                        Texture = null,
                    };

                    foreach (var subModel in modelProduct.Modelparts)
                        modelObject.Children.Add(CreateModelObjectData(subModel));
                    foreach (var subModel in modelProduct.Modelproducts)
                        modelObject.Children.Add(CreateModelObjectData(subModel));
                    return modelObject;
                }
                default: 
                    Debug.Log("Something went wrong creating the step model!");
                    return null;
            }
        }
    }
}
