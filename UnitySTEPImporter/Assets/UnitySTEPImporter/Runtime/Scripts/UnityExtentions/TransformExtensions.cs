using UnityEngine;

namespace VENTUS.StepImporter.UnityExtentions
{
	public static class TransformExtensions
	{
		/// <summary>
		/// Extension function for the Transform class. Applies the given transformation matrix to the transform.
		/// </summary>
		/// <param name="transform">The used Transform.</param>
		/// <param name="matrix">The transformation matrix.</param>
		public static void FromMatrix(this Transform transform, Matrix4x4 matrix)
		{
			transform.localScale = matrix.ExtractScale();
			transform.localRotation = matrix.ExtractRotation();
			transform.localPosition = matrix.ExtractPosition();
		}
	}
}
