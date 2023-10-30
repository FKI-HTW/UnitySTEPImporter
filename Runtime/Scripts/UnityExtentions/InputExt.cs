using UnityEngine;

namespace VENTUS.UnitySTEPImporter.UnityExtentions
{
    public static class InputExt {
	    public static bool MouseIsMoved() {
            return (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) ? true : false;
        }
    }
}