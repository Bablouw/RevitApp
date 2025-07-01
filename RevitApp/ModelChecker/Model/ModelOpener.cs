using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RevitApp.ModelChecker.Model
{
    public class ModelOpener
    {
        private readonly ModelPreparer _preparer;

        public ModelOpener() 
        {
            _preparer = new ModelPreparer();
        }
        public Document OpenModel(string filePath, Autodesk.Revit.ApplicationServices.Application app, out string errorMessage) 
        {
            errorMessage = null;
            bool isModelPrepared = _preparer.PrepareModelForOpening(filePath, out string prepareError);
            if(isModelPrepared == false)
            {
                errorMessage = prepareError;
                return null;
            }
            ModelPath modelPath = ModelPathUtils.ConvertUserVisiblePathToModelPath(filePath);
            OpenOptions options = new OpenOptions();
            OpenOptions tempOptions = new OpenOptions();
            tempOptions.DetachFromCentralOption = DetachFromCentralOption.DetachAndPreserveWorksets;
            Document tempDoc = app.OpenDocumentFile(modelPath, tempOptions);
            options.DetachFromCentralOption = DetachFromCentralOption.DetachAndPreserveWorksets;
            WorksetConfiguration configuration = new WorksetConfiguration();
            configuration.Open(GetWorksetIdsToOpen(tempDoc));
            tempDoc.Close(false);
        }
        private IList<WorksetId> GetWorksetIdsToOpen (Document doc)
        {
            if (doc == null)
            {
                return new List<WorksetId>();
            }
            IList<WorksetId> currentWorksets = new List<WorksetId>();
            FilteredWorksetCollector collector = new FilteredWorksetCollector(doc);
            currentWorksets = collector
                .ToList()
                .Where(ws => !ws.Name.StartsWith("04",StringComparison.OrdinalIgnoreCase))
                .Select(ws => ws.Id)
                .ToList();
            return currentWorksets;
        }
    }
}
