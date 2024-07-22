#include "CheckPathString.h"

// SYSTEM INCLUDES
#include <fstream>

// PROJECT INCLUDES
#include "SetUpsLoader.h"

namespace ventus
{
  bool CheckPathString::
    checkFileExtension( std::string filepath )
  {
    int filepathStringSize = (int)filepath.size();
    for ( std::string extensionString : SetUpsLoader::validFileExtensionList )
    {
      int extensionStringSize = (int)extensionString.size();
      if ( 0 == filepath.substr( filepathStringSize - extensionStringSize,
        extensionStringSize ).compare( extensionString ) )
      {
        return true;
      }
    }
    return false;
  }

  bool CheckPathString::
    checkExistFile( std::string filepath )
  {
    std::ifstream infile( filepath );
    return infile.good();
  }

}