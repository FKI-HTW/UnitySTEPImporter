using UnityEngine;

namespace VENTUS.StepImporter.UnityExtentions
{ 
	public static class MatrixExtention
	{
		/// <summary>
		/// Extension function for the Matrix class. Extracts the rotation form the matrix.
		/// </summary>
		/// <param name="matrix">The used Matrix.</param>
		/// <returns>The extracted rotation.</returns>
		public static Quaternion ExtractRotation(this Matrix4x4 matrix)
		{
			Vector3 forward;
			forward.x = matrix.m02;
			forward.y = matrix.m12;
			forward.z = matrix.m22;

			Vector3 upwards;
			upwards.x = matrix.m01;
			upwards.y = matrix.m11;
			upwards.z = matrix.m21;

			return Quaternion.LookRotation(forward, upwards);
		}

		/// <summary>
		/// Extension function for the Matrix class. Extracts the position form the matrix.
		/// </summary>
		/// <param name="matrix">The used Matrix.</param>
		/// <returns>The extracted position.</returns>
		public static Vector3 ExtractPosition(this Matrix4x4 matrix)
		{
			Vector3 position;
			position.x = matrix.m03;
			position.y = matrix.m13;
			position.z = matrix.m23;

			return position;
		}

		/// <summary>
		/// Extension function for the Matrix class. Extracts the scale form the matrix.
		/// </summary>
		/// <param name="matrix">The used Matrix.</param>
		/// <returns>The extracted scale.</returns>
		public static Vector3 ExtractScale(this Matrix4x4 matrix)
		{
			Vector3 scale;
			scale.x = new Vector4(matrix.m00, matrix.m10, matrix.m20, matrix.m30).magnitude;
			scale.y = new Vector4(matrix.m01, matrix.m11, matrix.m21, matrix.m31).magnitude;
			scale.z = new Vector4(matrix.m02, matrix.m12, matrix.m22, matrix.m32).magnitude;

			return scale;
		}
	}
}
