using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Windows;
using System.IO;
using UIFramework;

namespace RevitApp.ModelChecker.Model
{
    public class ModelPreparer
    {
        public bool PrepareModelForOpening(string filePath, out string errorMessage)
        {
            bool isFileExist = System.IO.File.Exists(filePath);
            if (isFileExist)
            {
                ModelPath modelPath = ModelPathUtils.ConvertUserVisiblePathToModelPath(filePath);
                try
                {
                    var trmData = new TransmissionData(TransmissionData.ReadTransmissionData(modelPath));
                    if (trmData != null)
                    {
                        ICollection<ElementId> extFiles = trmData.GetAllExternalFileReferenceIds();
                        foreach (ElementId extFile in extFiles)
                        {
                            ExternalFileReference desiredRefData = trmData.GetDesiredReferenceData(extFile);
                            switch (desiredRefData.ExternalFileReferenceType)
                            {
                                case ExternalFileReferenceType.RevitLink:
                                case ExternalFileReferenceType.CADLink:
                                case ExternalFileReferenceType.DWFMarkup:
                                    trmData.SetDesiredReferenceData(extFile, desiredRefData.GetAbsolutePath(), desiredRefData.PathType, false);
                                    continue;
                                default: continue;
                            }
                        }
                        trmData.IsTransmitted = true;
                        TransmissionData.WriteTransmissionData(modelPath, trmData);
                    }
                    else
                    {
                        errorMessage = "trmData = null";
                        return false; 
                    }
                    errorMessage = "все ок";
                    return true;
                }
                catch (Exception ex)
                {
                    errorMessage = "Файл недоступен, ошибка:" + ex;
                    return false;
                }
            }
            else
            {
                errorMessage = "Файл не существует ";
                return false;
            }
           
        }
    }
}