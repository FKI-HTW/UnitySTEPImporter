#ifndef _VENTUS_CheckPathString_h_
#define _VENTUS_CheckPathString_h_

#include <string>

namespace ventus
{
  class CheckPathString
  {
  public:
    static bool checkFileExtension( std::string filepath );
  
    static bool checkExistFile( std::string filepath );
  };
}

#endif // !_VENTUS_CheckPathString_h_
