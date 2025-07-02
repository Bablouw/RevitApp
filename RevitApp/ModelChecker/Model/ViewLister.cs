using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace RevitApp.ModelChecker.Model
{
    public class ViewLister
    {
        private Document PreparedDocument;

        public List<string> GetViewNames(Document doc)
        {
            if(doc == null)
            {
                return new List<string>();
            }
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(View));
            List<View> views = collector.Cast<View>().ToList();
            views = views.Where(v => !v.IsTemplate).ToList();
            List<string> viewNames = views.Select(v => v.Name).ToList();
            return viewNames;
        }
    }
}
