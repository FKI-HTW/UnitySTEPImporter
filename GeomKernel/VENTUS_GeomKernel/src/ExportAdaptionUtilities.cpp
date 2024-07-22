#include "ExportAdaptionUtilities.h"

// PROJECT INCLUDES
#include "SetUpsToUnity.h"
#include "AdaptationFirstIndex.h"
#include "gp_Ax3.hxx"

namespace ventus
{
	namespace Export
	{
		/**Private Methode***/
		//Adaption Length Dimension
		const double
			ExportAdaptionUtilities::LengthDimensionOccToUnityAdapt(const double edGeLengthValue)
		{
			const double unityLengthValue = SetUpsToUnity::lengthDimensionScale * edGeLengthValue;

			return unityLengthValue;
		}

		const gp_Trsf
			ExportAdaptionUtilities::LengthDimensionOccToUnityAdapt(const gp_Trsf& crMatrix4d)
		{
			gp_Trsf adaptedMatrix = crMatrix4d;

			const double x_translationsvector = LengthDimensionOccToUnityAdapt(adaptedMatrix.Value(1, 4));
			const double y_translationsvector = LengthDimensionOccToUnityAdapt(adaptedMatrix.Value(2, 4));
			const double z_translationsvector = LengthDimensionOccToUnityAdapt(adaptedMatrix.Value(3, 4));

			//Translationsvektor
			gp_Vec translationvector = gp_Vec(x_translationsvector, y_translationsvector, z_translationsvector);
			adaptedMatrix.SetTranslationPart(translationvector);

			return adaptedMatrix;
		}

		const gp_Pnt
			ExportAdaptionUtilities::LengthDimensionOccToUnityAdapt(const gp_Pnt& crPoint3d)
		{
			const double cAdaptedX = LengthDimensionOccToUnityAdapt(crPoint3d.X());
			const double cAdaptedY = LengthDimensionOccToUnityAdapt(crPoint3d.Y());
			const double cAdaptedZ = LengthDimensionOccToUnityAdapt(crPoint3d.Z());

			const gp_Pnt adaptedPoint3d(cAdaptedX, cAdaptedY, cAdaptedZ);

			return adaptedPoint3d;
		}

		//Adaption Coordinate System
		const gp_Pnt ExportAdaptionUtilities::
			CoordinateSystemOccToUnityAdapt(const gp_Pnt& cPoint)
		{
			const std::tuple<int, int, int>& adaptationOrder
				= SetUpsToUnity::coordinateSystemAdaptionOrder;

			const std::tuple<int, int, int>& adaptationOrderSign
				= SetUpsToUnity::coordinateSystemAdaptionOrderSign;

			int newXFromOldXYZ = std::get<0>(adaptationOrder);
			int newYFromOldXYZ = std::get<1>(adaptationOrder);
			int newZFromOldXYZ = std::get<2>(adaptationOrder);

			int newXSign = std::get<0>(adaptationOrderSign);
			int newYSign = std::get<1>(adaptationOrderSign);
			int newZSign = std::get<2>(adaptationOrderSign);

			double newX = cPoint.Coord(newXFromOldXYZ) * (double)newXSign; //oder SetUpsUnity ndern
			double newY = cPoint.Coord(newYFromOldXYZ) * (double)newYSign;
			double newZ = cPoint.Coord(newZFromOldXYZ) * (double)newZSign;

			const gp_Pnt cAdaptedPoint = gp_Pnt(newX, newY, newZ);

			return cAdaptedPoint;
		}

		const gp_Trsf ExportAdaptionUtilities::
			CoordinateSystemOccToUnityAdapt(const gp_Trsf& crMatrix)
		{
			// in unity we have left handed coordinate system where the y and z-Axis are switched 
			//transformation for the change of y and z axis
			gp_Trsf changeYandZaxis;
			changeYandZaxis.SetValues( 1, 0, 0, 0,
									   0, 0, 1, 0,
									   0, 1, 0, 0 );

			//creation of a right handed cartesian coordinate systems with nullvector as origin
			gp_Ax3 coordinateSystemUnity;
			//change X, Z directions
			coordinateSystemUnity.XReverse(); //reverse the direction of the X-Axis 
			coordinateSystemUnity.ZReverse(); //reverse the direction of Z-Axis
			//change Y, Z axis
			coordinateSystemUnity.Transform(changeYandZaxis); //transformation of the axis change

			gp_Trsf adaptedMatrix = crMatrix;
			adaptedMatrix.SetTransformation(coordinateSystemUnity);

			return adaptedMatrix;
		}
		// Base Attributes Adaption
		const gp_Pnt ExportAdaptionUtilities::
			BaseAttributesOccToUnityAdapt(const gp_Pnt& crPoint)
		{
			const gp_Pnt cCoordinateSystemAdaptedPoint
				= CoordinateSystemOccToUnityAdapt(crPoint);

			const gp_Pnt cLengthDimensionAdaptedPoint
				= LengthDimensionOccToUnityAdapt(cCoordinateSystemAdaptedPoint);

			const gp_Pnt cAdaptedBox = cLengthDimensionAdaptedPoint;

			return cAdaptedBox;
		}

		const gp_Trsf ExportAdaptionUtilities::
			BaseAttributesOccToUnityAdapt(const gp_Trsf& crMatrix)
		{
			const gp_Trsf cUnityCoordSysAdaptedMatrix
				= CoordinateSystemOccToUnityAdapt(crMatrix);

			const gp_Trsf cUnityLengthDimensionAdaptedMatrix
				= LengthDimensionOccToUnityAdapt(cUnityCoordSysAdaptedMatrix);

			return cUnityLengthDimensionAdaptedMatrix;
		}

		/***Public Methods***/

		//Mesh Attributes 
		const gp_Pnt ExportAdaptionUtilities::
			MeshOccToUnityAdapt(const gp_Pnt& cCoordinate) //kann wahrscheinlich weg
		{
			return BaseAttributesOccToUnityAdapt(cCoordinate);
		}

		const int ExportAdaptionUtilities::
			MeshOccToUnityAdapt(const int cIndex)
		{
			const int cFirstIndexAdaptedIndex
				= AdaptationFirstIndex::edGeToUnityAdapt(cIndex);

			const int cAdaptedIndex = cFirstIndexAdaptedIndex;

			return cAdaptedIndex;
		}

		//Model Attributes 
		const gp_Trsf* ExportAdaptionUtilities::
			ModelOccToUnityAdapt(const gp_Trsf& cpTransformation)
		{

			const gp_Trsf cAdaptedMatrix
				= BaseAttributesOccToUnityAdapt(cpTransformation);

			const gp_Trsf* cpAdaptedTransformation
				= new gp_Trsf(cAdaptedMatrix);

			return cpAdaptedTransformation;
		}

		const Bnd_Box ExportAdaptionUtilities::
			ModelOccToUnityAdapt(const Bnd_Box& crBox)
		{
			const gp_Pnt cBoxMinPoint = crBox.CornerMin();
			const gp_Pnt cBoxMaxPoint = crBox.CornerMax();

			const gp_Pnt cAdaptedBoxMinPoint
				= BaseAttributesOccToUnityAdapt(cBoxMinPoint); //hier muss einiges noch angepasst werden damit diese Zuweisung funktioniert
			const gp_Pnt cAdaptedBoxMaxPoint
				= BaseAttributesOccToUnityAdapt(cBoxMaxPoint);

			const Bnd_Box cAdaptedBox(cAdaptedBoxMinPoint, cAdaptedBoxMaxPoint);

			return cAdaptedBox;
		}

	}
}
