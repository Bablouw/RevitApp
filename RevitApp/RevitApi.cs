using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitApp
{
    public static class RevitApi
    {
        public static UIApplication UIApplication { get; set; }
        public static UIDocument UIDocument { get => UIApplication.ActiveUIDocument; }
        public static Document Document { get => UIDocument.Document; }
        public static Autodesk.Revit.ApplicationServices.Application Application { get; set; }
        public static void Initialize(ExternalCommandData commandData)
        {
            UIApplication = commandData.Application;
            Application = commandData.Application.Application;
        }
    }
}
