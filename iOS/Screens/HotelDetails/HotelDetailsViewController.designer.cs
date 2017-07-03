// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace iHere.iOS.HotelDetails
{
    [Register ("HotelDetailsViewController")]
    partial class HotelDetailsViewController
    {
        [Outlet]
        iHere.iOS.iHereButton btnBook { get; set; }
        
        void ReleaseDesignerOutlets ()
        {
            if (btnBook != null) {
                btnBook.Dispose ();
                btnBook = null;
            }
        }
    }
}
