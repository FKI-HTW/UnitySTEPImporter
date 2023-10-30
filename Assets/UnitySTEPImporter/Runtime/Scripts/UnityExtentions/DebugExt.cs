using UnityEngine;

namespace VENTUS.UnitySTEPImporter.UnityExtentions
{
    public static class DebugExt {
        public static void DrawPlane(Vector3 normal, Vector3 position) {
             DrawPlane(normal, position, Color.green);
        }

        public static void DrawPlane(Vector3 normal, Vector3 position, Color color) {
            Vector3 v3;

            if (normal.normalized != Vector3.forward) {
                v3 = Vector3.Cross(normal, Vector3.forward).normalized * normal.magnitude;
            } else {
                v3 = Vector3.Cross(normal, Vector3.up).normalized * normal.magnitude;
            }

            var corner0 = position + v3;
            var corner2 = position - v3;
            var q = Quaternion.AngleAxis(90.0f, normal);
            v3 = q * v3;
            var corner1 = position + v3;
            var corner3 = position - v3;

            Debug.DrawLine(corner0, corner2, color);
            Debug.DrawLine(corner1, corner3, color);
            Debug.DrawLine(corner0, corner1, color);
            Debug.DrawLine(corner1, corner2, color);
            Debug.DrawLine(corner2, corner3, color);
            Debug.DrawLine(corner3, corner0, color);
            Debug.DrawRay(position, normal, Color.red);
        }

        public static void ClearLog() {
			// UnityEditorInternal.LogEntries removed in 2017.1.0f3
			/*Assembly assembly = Assembly.GetAssembly (typeof(SceneView));

			Type logEntries = assembly.GetType ("UnityEditorInternal.LogEntries");
			MethodInfo clearConsoleMethod = logEntries.GetMethod ("Clear");
			clearConsoleMethod.Invoke (new object (), null);*/
        }
    }
}
