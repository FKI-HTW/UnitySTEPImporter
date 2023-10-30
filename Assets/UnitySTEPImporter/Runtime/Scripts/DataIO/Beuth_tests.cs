using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace VENTUS.UnitySTEPImporter.DataIO
{
    class Beuth_tests
    {
        public static void displayBoundingBox(Bounds bounds)
        {

        }

        public static void print(Matrix4x4 matrix)
        {
            Debug.Log("Matrix:");
            Debug.Log(matrix.GetRow(0));
            Debug.Log(matrix.GetRow(1));
            Debug.Log(matrix.GetRow(2));
            Debug.Log(matrix.GetRow(3));
            
        }
    }
}
