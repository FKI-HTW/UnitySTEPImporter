#include "Color.h"

namespace ventus
{
  Color::Color()
  {
    mR = 0.;
    mG = 0.;
    mB = 0.;
    mA = 1.;
  }

  Color::Color( double R, double G, double B, double A)
  {
    mR = R;
    mG = G;
    mB = B;
    mA = A;
  }

  Color::~Color() { }

  void Color::setRGBA(double R, double G, double B, double A)
  {
      mR = R;
      mG = G;
      mB = B;
      mA = A;
  }

} // ventus
