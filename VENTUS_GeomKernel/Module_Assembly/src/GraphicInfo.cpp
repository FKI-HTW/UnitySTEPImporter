#include "GraphicInfo.h" 

namespace ventus
{
  GraphicInfo::GraphicInfo() 
  { 
  }

  GraphicInfo::GraphicInfo(double R, double G, double B, double A)
      : mColor(R, G, B, A)
  {
  }

  GraphicInfo::~GraphicInfo() { }

} // ventus