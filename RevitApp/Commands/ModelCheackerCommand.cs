using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using RevitApp.SumParamPlugin.View;
using System;
using System.Collections.Generic;
using System.Configuration.Assemblies;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;



namespace RevitApp
{
    [Autodesk.Revit.Attributes.TransactionAttribute(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class ModelCheackerCommand : IExternalCommand
    {
        static AddInId addinId = new AddInId(new Guid("2DE669CB-D848-478F-BA3F-0850C46033E4"));
        private SumParamView _view;
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            if (RevitApi.UIApplication == null)
            {
                RevitApi.Initialize(commandData);
            }
            var viewModel = new ViewModel();

        }
        private void OnDocumentClosed(object sender, DocumentClosedEventArgs e)
        {
            _view.Close();
            _view = null;
            RevitApi.Application.DocumentClosed -= OnDocumentClosed;
        }
    }
}
