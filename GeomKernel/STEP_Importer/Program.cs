using System;

using static StepImporter.ImportGeomKernel;
using StepImporter;
using System.Drawing;
using StepImporter.ModuleAssembly.Model;
using StepImporter.ModuleAssembly.Mesh;
using System.Collections.Generic;
//using StepImporter.GeomKernel;

namespace STEP_IMPORTER_TEST {
    class Program {
        static void Main(string[] args)
        {
            Console.WriteLine("C# Test for importing a STP file from GeomKernel.");
            Console.WriteLine("-------------------------------------------------");

            // Copy the VENTUS_GeomKernel.dll to curren working folder
            Console.WriteLine("Copy newest GeomKernel.dll to own folder");
            string batFileName = "copyDll.bat";
            System.Diagnostics.Process.Start(batFileName);
            Console.WriteLine("-------------------------------------------------");

            string path = "../Test_Files/mixed.stp";

            ImportModuleAssembly modAss = new ImportModuleAssembly(2);
            ModelObject modelObject = modAss.transferModelObject(path);

            printModelObject(modelObject);

        }

        private static void printModelObject(ModelObject modelObject)
        {
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("| ID | Name | Type | ");
            Console.WriteLine("-------------------------------------------------");
            foreach (ModelBase root in modelObject.RootModels)
            {
                PrintModelBaseEntities(root);
                if(root.Type == ModelBase.ModelType.PRODUCT) { 
                    ModelProduct product = (ModelProduct)root;
                    foreach (ModelBase child in product.Childs) {
                        PrintModelBaseEntities(child);
                    }
                }
            }
        }

        private static void PrintModelBaseEntities(ModelBase modelBase) {
            Console.WriteLine("| " + modelBase.Id + " | "
                                + modelBase.Name + " | "
                                + modelBase.Type + " | ");
            if(modelBase.Type == ModelBase.ModelType.PART){
                ModelPart part = (ModelPart)modelBase;
                Console.WriteLine("| Part has : " + part.Meshes.Count + " meshes. |");
                int count = 0;
                foreach (var mesh in part.Meshes) {
                    count++;
                    int index = 0;
                    Console.WriteLine("| Color : (" + mesh.GraphicInfo.Color.Red + ", "
                                                    + mesh.GraphicInfo.Color.Green + ", "
                                                    + mesh.GraphicInfo.Color.Blue + ", "
                                                    + mesh.GraphicInfo.Color.Alpha + ") |");
                    Console.WriteLine("| " + count + " Mesh has : " + mesh.Coordinates.Count + " Points. |");
                    foreach (var coord in mesh.Coordinates) {
                        Console.WriteLine("| Point " + index + " : " + coord.X + ", " + coord.Y + ", " + coord.Z + " |");
                        index++;
                    }
                    
                    index = 0;
                    Console.WriteLine("| " + count + " Mesh has : " + mesh.Triangles.Count + " triangles. |");
                    foreach (var triangle in mesh.Triangles) {
                        Console.WriteLine("| triangle " + index + " has Indices : "  + triangle[0] + " : | Coordinates : (" + mesh.Coordinates[triangle[0]].X + ", " + mesh.Coordinates[triangle[0]].Y + ", " + mesh.Coordinates[triangle[0]].Z + ")");
                        Console.WriteLine("                           "  + triangle[1] + " : | Coordinates : (" + mesh.Coordinates[triangle[1]].X + ", " + mesh.Coordinates[triangle[1]].Y + ", " + mesh.Coordinates[triangle[1]].Z + ")");
                        Console.WriteLine("                           "  + triangle[2] + " : | Coordinates : (" + mesh.Coordinates[triangle[2]].X + ", " + mesh.Coordinates[triangle[2]].Y + ", " + mesh.Coordinates[triangle[2]].Z + ")");
                        index++;
                    }
                }
            }
            Console.WriteLine("-------------------------------------------------");
        }
    }
}
