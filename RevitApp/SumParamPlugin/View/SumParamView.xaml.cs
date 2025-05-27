using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RevitApp.SumParamPlugin;



namespace RevitApp.SumParamPlugin.View
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class SumParamView : Window
    {
        public SumParamView(ViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            this.Closed += (s, e) => RevitApi.Application.DocumentClosed -= OnDocumentClosed;
        }

        private void Button_Click (object sender, RoutedEventArgs e)
        {

            (DataContext as ViewModel).SummParameters();
        }

        private void OnDocumentClosed(object sender, DocumentClosedEventArgs e)
        {
            if (RevitApi.UIApplication?.ActiveUIDocument == null)
            {
               this.Close();
            }
        }


    }
}
