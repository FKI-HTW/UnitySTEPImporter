#include "ModelAttributesToExport.h"

// EDGE_BASE INCLUDES
#include "gp_Trsf.hxx"
#include "Bnd_Box.hxx"

namespace ventus
{
  namespace Export
  {
    const ExportTransformation4d ModelAttributesToExport::
      transfer( const gp_Trsf& cpTransformation )
    {
      // short name reference for better view
      //const edGe::Matrix4d& mat = cpTransformation->getTransformationMatrix();

	 /*
      const ExportTransformation4d cExportTransformation(
        (float)mat[0][0], (float)mat[0][1], (float)mat[0][2], (float)mat[0][3],
        (float)mat[1][0], (float)mat[1][1], (float)mat[1][2], (float)mat[1][3],
        (float)mat[2][0], (float)mat[2][1], (float)mat[2][2], (float)mat[2][3],
        (float)mat[3][0], (float)mat[3][1], (float)mat[3][2], (float)mat[3][3]
      );
	  */

		const gp_Mat& mat = cpTransformation.VectorialPart();
		const gp_XYZ& vec = cpTransformation.TranslationPart();

		const ExportTransformation4d cExportTransformation(
			(float)mat.Value(1, 1), (float)mat.Value(1, 2), (float)mat.Value(1, 3), (float)vec.X(),
			(float)mat.Value(2, 1), (float)mat.Value(2, 2), (float)mat.Value(2, 3), (float)vec.Y(),
			(float)mat.Value(3, 1), (float)mat.Value(3, 2), (float)mat.Value(3, 3), (float)vec.Z(),
			(float)0, (float)0, (float)0, (float)1
		);

      return cExportTransformation;
    }

    const ExportBoundingBox ModelAttributesToExport::transfer(const Bnd_Box& crBox) {
      const gp_Pnt cBoxMinPoint = crBox.CornerMin();
      const gp_Pnt cBoxMaxPoint = crBox.CornerMax();
      const ExportBoundingBox cExportBoundingBox(cBoxMinPoint.X(), cBoxMinPoint.Y(), cBoxMinPoint.Z(),
                                                 cBoxMaxPoint.X(), cBoxMaxPoint.Y(), cBoxMaxPoint.Z());
      return cExportBoundingBox;
    }

    // TODO
    // Currently don't work and currently implemented in Export function directly
    /*char* ModelAttributesToExport::
      transfer( const string cName )
    {
      const int nameSize = cName.size();
      char* nameCharArray = new char[nameSize + 1];

      for ( int i = 0; i < nameSize; ++i )
      {
        nameCharArray[i] = cName[i];
      }

      return nameCharArray;
    }*/
  }
}