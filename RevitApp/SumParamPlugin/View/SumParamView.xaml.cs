using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using System;
using System.Windows;

namespace RevitApp.SumParamPlugin.View
{
    public partial class SumParamView : Window
    {
        private bool _isClosing = false;
        public SumParamView(ViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as ViewModel).SummParameters();
        }
    }
}