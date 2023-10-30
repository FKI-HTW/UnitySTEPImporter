using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace VENTUS.UnitySTEPImporter.DataIO
{
    static class TransformationUtilities
    {
        public static void
            TranslateCoordSysOriginToBBoxCenter( Modelobject modelobject)
        {
            TranslatorToBoundingBoxCenter.TranslateModelobject( modelobject);
        }

        public static void
                TranslateCoordSysOriginToBBoxCenterNewMesh(Modelpart modelpart)
        {
            modelpart.Modelmesh.Mesh.vertices
                = TranslatorToBoundingBoxCenter.TranslateVertices(modelpart.Modelmesh.Mesh.vertices,
                                                                    modelpart.Bounds.center);
        }

        private static class TranslatorToBoundingBoxCenter
        {
            public static void TranslateModelobject( Modelobject modelobject)
            {
                Vector3 transformationTranslationVector = modelobject.Bounds.center;

                modelobject.Transformation = TranslateTransformation(modelobject.Transformation,
                                                                        transformationTranslationVector);

                TranslateSubmodelsOfModelobject(modelobject.Submodels, modelobject.Bounds.center);

                modelobject.Bounds = TranslateBoundingBoxOriginToOwnCenter(modelobject.Bounds);
            }

            private static void TranslateSubmodelsOfModelobject( List<Submodel> submodels,
                                                                    Vector3 translationVector)
            {
                foreach (Submodel submodel in submodels)
                {
                    TranslateSubmodel(submodel, translationVector);
                }
            }

            private static void TranslateSubmodel( Submodel submodel,
                                                    Vector3 parentTranslationVector)
            {
                if (typeof(Modelproduct) == submodel.GetType())
                {
                    TranslateModelproduct((Modelproduct)submodel,
                                        parentTranslationVector);
                }
                else
                {
                    TranslateModelpart((Modelpart)submodel,
                                    parentTranslationVector);
                }
            }
            
            private static void TranslateModelproduct(Modelproduct modelproduct,
                                                        Vector3 parentTranslationVector)
            {   // see mathematical vector substraction
                Vector3 transformationTranslationVector = modelproduct.Bounds.center - parentTranslationVector;

                modelproduct.Transformation = TranslateTransformation(modelproduct.Transformation, 
                                                                        transformationTranslationVector);

                TranslateSubmodelsOfModelproduct(modelproduct,
                                                    modelproduct.Bounds.center);

                modelproduct.Bounds = TranslateBoundingBoxOriginToOwnCenter(modelproduct.Bounds);
            }

            private static void TranslateModelproducts(List<Modelproduct> modelproducts,
                                                        Vector3 translationVector)
            {
                foreach (Modelproduct subModelproduct in modelproducts)
                {
                    TranslateModelproduct(subModelproduct, translationVector);
                }
            }

            private static void TranslateSubmodelsOfModelproduct(Modelproduct modelproduct,
                                                                    Vector3 parentTranslationVector)
            {
                TranslateModelproducts(modelproduct.Modelproducts,
                                        parentTranslationVector);

                TranslateModelparts(modelproduct.Modelparts,
                                    parentTranslationVector);
            }
            
            private static void TranslateModelparts(List<Modelpart> modelparts,
                                                    Vector3 translationVector)
            {
                foreach (Modelpart subModelpart in modelparts)
                {
                    TranslateModelpart(subModelpart, translationVector);
                }
            }

            private static void TranslateModelpart(Modelpart modelpart,
                                                    Vector3 parentTranslationVector)
            {
                // see mathematical vector substraction
                Vector3 transformationTranslationVector = modelpart.Bounds.center - parentTranslationVector;

                modelpart.Transformation = TranslateTransformation(modelpart.Transformation,
                                                                    transformationTranslationVector);

                modelpart.Modelmesh.Mesh.vertices = TranslateVertices(modelpart.Modelmesh.Mesh.vertices,
                                                        modelpart.Bounds.center);

                modelpart.Bounds = TranslateBoundingBoxOriginToOwnCenter(modelpart.Bounds);
            }

            public static Vector3[] TranslateVertices(Vector3[] vertices,
                                                        Vector3 translationVector)
            {
                Vector3[] translatedVertices = new Vector3[vertices.Length];

                for (int i = 0; i < vertices.Length; ++i)
                {
                    translatedVertices[i] = TranslateVertex(vertices[i], translationVector);
                }

                return translatedVertices;
            }

            private static Matrix4x4 TranslateTransformation(Matrix4x4 matrix,
                                                                Vector3 translationVector)
            {
                Matrix4x4 translationMatrix = Matrix4x4.Translate(translationVector);

                return translationMatrix*matrix;
            }

            private static Bounds TranslateBoundingBoxOriginToOwnCenter(Bounds bounds)
            {
                Vector3 newBoundingBoxCenter = new Vector3(0, 0, 0);

                return new Bounds(newBoundingBoxCenter,
                                    bounds.size);
            }

            private static Vector3 TranslateVertex(Vector3 vertex,
                                                    Vector3 translationVector)
            {   
                // see mathematical vector substraction
                return (vertex - translationVector);
            }
        };
    }
}
