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

    public class ViewModel: INotifyPropertyChanged
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
        
        public ObservableCollection<ParameterSum> SummParameters()
       {
            _parametersSums = new ObservableCollection<ParameterSum>();
            ParametersSums.Clear();
           List<Element> elements = GetSelectedElements();
           if (elements.Count == 0)
               return _parametersSums;
           List<string> commonParameters = GetCommonParameters().OrderBy(name => name).ToList();
           foreach (string paramName in commonParameters)
           {
                ParameterSum sum = CalculateParameterSum(elements, paramName);
                if (sum != null)
                {
                    _parametersSums.Add(sum);
                }
           }
           OnPropertyChanged(nameof(ParametersSums));
           return _parametersSums;
       }

        public event EventHandler CloseRequest;
        public event EventHandler HideRequest;
        public event EventHandler ShowRequest;
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }

        private void RaiseHideRequest()
        {
            HideRequest?.Invoke(this, EventArgs.Empty);
        }

        private void RaiseShowRequest()
        {
            ShowRequest?.Invoke(this, EventArgs.Empty);
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private List<Element> GetSelectedElements()
        {
            List<ElementId> elementIds = RevitApi.UIDocument.Selection.GetElementIds().ToList();
            List<Element> elements = elementIds.Select(id => RevitApi.Document.GetElement(id)).ToList();
            return elements;
        }

        private List<string> GetCommonParameters()
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

        private ParameterSum CalculateParameterSum(List<Element> elements, string paramName)
        {
            Parameter firstParam = elements[0].LookupParameter(paramName);
            if (firstParam == null)
                return null;
            if (firstParam.StorageType == StorageType.Double)
            {
                return SumDoubleParameter(elements, paramName);
            }
            else if (firstParam.StorageType == StorageType.Integer)
            {
                return SumIntegerParameter(elements, paramName);
            }
            else return null;
        }

        private ParameterSum SumDoubleParameter (List<Element> elements, string paramName)
        {
            double parSum = 0;
            ForgeTypeId unitType = null;
            foreach (Element element in elements)
            {
                Parameter parameter = element.LookupParameter(paramName);
                if (parameter == null)
                    continue;
                unitType = parameter.GetUnitTypeId();
                parSum += parameter.AsDouble();
            }
            double converSum = UnitUtils.ConvertFromInternalUnits(parSum, unitType);
            string unitLabel = LabelUtils.GetLabelForUnit(unitType);
            return new ParameterSum
            {
                Name = paramName,
                Sum = converSum,
                Unit = unitLabel
            };
        }

        private ParameterSum SumIntegerParameter(List<Element> elements, string paramName)
        {
            int parSum = 0;
            foreach (Element element in elements)
            {
                Parameter parameter = element.LookupParameter(paramName);
                if (parameter == null) continue;
                parSum += parameter.AsInteger();
            }
            return new ParameterSum
            {
                Name = paramName,
                Sum = parSum,
                Unit = ""
            };
        }
    }
}

   