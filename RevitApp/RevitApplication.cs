using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RevitApp
{

    internal class RevitApplication : IExternalApplication
    {
        static AddInId addinId = new AddInId(new Guid("2DE669CB-D848-478F-BA3F-0850C46033E4"));
        private readonly string assemblyPath = Assembly.GetExecutingAssembly().Location;
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            string tabName = "CustomTab";
            application.CreateRibbonTab(tabName);
            RibbonPanel ribbonPanel = application.CreateRibbonPanel(tabName, "Автоматизация");
            AddButton(ribbonPanel, "Button 1", assemblyPath, "RevitApp.SumParamPluginCommand", "Подсказка");
            return Result.Succeeded;
        }

        private void AddButton(RibbonPanel ribbonPanel, string buttonName, string path, string linkToCommand, string toolTip)
        {
            PushButtonData buttonData = new PushButtonData(
                buttonName,
                buttonName,
                path,
                linkToCommand);
            PushButton Button = ribbonPanel.AddItem(buttonData) as PushButton;
            Button.ToolTip = toolTip;
        }
    }
}
