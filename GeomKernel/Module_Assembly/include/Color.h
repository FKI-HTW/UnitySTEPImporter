#ifndef ventus_Color_h_
#define ventus_Color_h_

// SYSTEM INCLUDES
#include <memory>

namespace ventus
{
  using namespace std;

  class Color
  {
  public:   

    Color();

    Color(double R, double G, double B, double A);

    virtual ~Color();

    inline double getR () const;

    inline double getG () const;

    inline double getB () const;

    inline double getA () const;
    
    void setRGBA(double R, double G, double B, double A);

  private:

      double mR, mG, mB, mA;

  };// Color

// INLINE METHODS

  double Color::getR () const
  {
    return mR;
  }

  double Color::getG () const
  {
    return mG;
  }

  double Color::getB () const
  {
    return mB;
  }

  double Color::getA () const
  {
    return mA;
  }

} 

#endif // ventus_Color_h_