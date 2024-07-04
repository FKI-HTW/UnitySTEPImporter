#ifndef ventus_graphic_info_h_
#define ventus_graphic_info_h_

// SYSTEM INCLUDES
#include<memory>

// PROJECT INCLUDES
#include "Color.h"


namespace ventus
{
  using namespace std;

  class GraphicInfo
  {
  public:

    GraphicInfo ();

    GraphicInfo(double R, double G, double B, double A);

    virtual ~GraphicInfo();

    inline const Color& getColor() const;

    inline void setColor (const Color& color);

  private:

    Color mColor;
    // TODO: for future
    // Texture mTexture;

  }; // GraphicInfo

// INLINE METHODS

  const Color& GraphicInfo::getColor() const 
  {
    return mColor;
  }

  void GraphicInfo::setColor (const Color& color)
  {
    mColor = color;
  }

}

#endif // ventus_graphic_info_h_