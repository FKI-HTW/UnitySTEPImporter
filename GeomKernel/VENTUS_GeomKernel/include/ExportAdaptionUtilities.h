#ifndef Ventus_Export_ExportAdaptionUtilities_h_
#define Ventus_Export_ExportAdaptionUtilities_h_

//includes
#include "gp_Pnt.hxx"
#include "gp_Trsf.hxx"
#include "Bnd_Box.hxx"

namespace ventus
{
	namespace Export
	{
		class ExportAdaptionUtilities
		{
		private:
			//Adaption Length Dimension
			static const double
				LengthDimensionOccToUnityAdapt(const double);

			static const gp_Trsf
				LengthDimensionOccToUnityAdapt(const gp_Trsf&);

			static const gp_Pnt
				LengthDimensionOccToUnityAdapt(const gp_Pnt&);

			//Adaption Coordinate System
			static const gp_Pnt CoordinateSystemOccToUnityAdapt(const gp_Pnt&);

			//static const gp_Trsf createEdGeToUnityAdaptationOrthogonalMatrix4d();

			static const gp_Trsf CoordinateSystemOccToUnityAdapt(const gp_Trsf&);

			//Adaption Matrix Convention
			//static const gp_Trsf MatrixConventionOccToUnityAdapt(const gp_Trsf&);

			//Base Attributes Adaption
			static const gp_Pnt BaseAttributesOccToUnityAdapt(const gp_Pnt&);

			static const gp_Trsf BaseAttributesOccToUnityAdapt(const gp_Trsf&);

		public:
			//Mesh Attributes
			static const gp_Pnt MeshOccToUnityAdapt(const gp_Pnt& cCoordinate);

			static const int MeshOccToUnityAdapt(const int cIndex);

			//Model Attributes
			static const gp_Trsf* ModelOccToUnityAdapt(const gp_Trsf&);

			static const Bnd_Box ModelOccToUnityAdapt(const Bnd_Box&);
		};
	}
}
#endif // !1
