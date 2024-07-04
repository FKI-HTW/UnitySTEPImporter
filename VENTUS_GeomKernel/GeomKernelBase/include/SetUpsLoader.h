#ifndef _VENTUS_SetUpsLoader_h_
#define _VENTUS_SetUpsLoader_h_

#include <list>
#include <string>

namespace ventus
{
  enum ResultFileLoading
  {
    CREATE_SUCCESSFUL,
    WRONG_FILE_EXTENSION,   // current file extesion is not allowed 
    FILE_NOT_FOUND,         // file not found
    LOADING_EMPTY,
    OCC_ERROR               // developer error
  };

  enum ResultTransfer
  {
    TRANSFER_SUCCESSFUL,              // successful transfer
    TRANSFER_EMPTY,                   // initial state
    TRANSFER_ERROR,                   // transfer was not successful   
    DATA_NOT_IN_READER,               // data was not red from file 
    TRANSFER_DEFAULT_ERROR            // developer error
  };

  struct SetUpsLoader
  {
    static const std::list <std::string> validFileExtensionList;
  };
}
#endif