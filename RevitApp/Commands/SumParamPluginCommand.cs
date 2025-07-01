using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using RevitApp.SumParamPlugin.View;
using System;



namespace RevitApp
{
    [Autodesk.Revit.Attributes.TransactionAttribute(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class SumParamPluginCommand : IExternalCommand
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
            _view = new SumParamView(viewModel);
            _view.Show();
            RevitApi.Application.DocumentClosed += OnDocumentClosed;
            return Result.Succeeded;
        }
        private void OnDocumentClosed(object sender, DocumentClosedEventArgs e)
        {
            _view.Close();
            _view = null;
            RevitApi.Application.DocumentClosed -= OnDocumentClosed;
        }
    }
}
