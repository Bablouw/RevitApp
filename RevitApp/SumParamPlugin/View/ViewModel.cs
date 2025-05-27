using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RevitApp;
using RevitApp.SumParamPlugin.View;


namespace RevitApp.SumParamPlugin.View
{   
    public class ParameterSum
    {
        public string Name { get; set; }
        public double Sum { get; set; }
        public string Unit { get; set; }
        
    }
    public class ViewModel :INotifyPropertyChanged
    {
        private ObservableCollection<ParameterSum> _parametersSums;

        public ObservableCollection<ParameterSum> ParametersSums
        {
            get => _parametersSums;
            set
            {
                _parametersSums = value;
                OnPropertyChanged();
            }
        }



        

        public List<string> GetCommonParameters()
        {
            List<ElementId> elementIds = RevitApi.UIDocument.Selection.GetElementIds().ToList();
            List<string> commonParameters = RevitApi.Document.GetElement(elementIds[0]).Parameters.Cast<Parameter>().Select(p => p.Definition.Name).ToList();


            foreach (ElementId elementId in elementIds.Skip(1))
            {
                Element element = RevitApi.Document.GetElement(elementId);
                commonParameters = commonParameters.Intersect(element.Parameters.Cast<Parameter>().Select(p => p.Definition.Name)).ToList();
            }
            return commonParameters;
        }

        public ObservableCollection<ParameterSum> SummParameters()
        {
            _parametersSums = new ObservableCollection<ParameterSum>();
            ParametersSums.Clear();
            List<ElementId> elementIds = RevitApi.UIDocument.Selection.GetElementIds().ToList();
            List<Element> elements = elementIds.Select(id => RevitApi.Document.GetElement(id)).ToList();
            if (elementIds.Count == 0) return _parametersSums;
            List<string> commonParameters = GetCommonParameters().OrderBy(name => name).ToList();
            foreach (string paramName in commonParameters)
            {
                
                Parameter firstParam = elements[0].LookupParameter(paramName);
                if(firstParam == null) continue;
                if (firstParam.StorageType == StorageType.Double)
                {
                    double parSum = 0;
                    ForgeTypeId unitType = firstParam.GetUnitTypeId();
                    foreach (Element element in elements)
                    {
                        Parameter parameter = element.LookupParameter(paramName);
                        if (parameter == null) continue;
                        parSum += parameter.AsDouble();

                    }
                    double converSum = UnitUtils.ConvertFromInternalUnits(parSum, unitType);
                    string unitLabel = LabelUtils.GetLabelForUnit(unitType);
                    _parametersSums.Add(new ParameterSum
                    {
                        Name = paramName,
                        Sum = converSum,
                        Unit = unitLabel
                    });
                }
                else if (firstParam.StorageType == StorageType.Integer)
                {
                    int parSum = 0;
                    foreach (Element element in elements)
                    {
                        Parameter parameter = element.LookupParameter(paramName);
                        if (parameter == null) continue;
                        parSum += parameter.AsInteger();
                    }
                    _parametersSums.Add(new ParameterSum
                    {
                        Name = paramName,
                        Sum = parSum,
                        Unit = ""
                    });
                }
                
            }
            OnPropertyChanged(nameof(ParametersSums));
            return _parametersSums;
        }


        public event EventHandler CloseRequest;
        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler HideRequest;
        private void RaiseHideRequest()
        {
            HideRequest?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler ShowRequest;
        private void RaiseShowRequest()
        {
            ShowRequest?.Invoke(this, EventArgs.Empty);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}

   