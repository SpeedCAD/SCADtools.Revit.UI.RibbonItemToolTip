using Autodesk.Revit.UI;
using Autodesk.Windows;
using SCADtools.Revit.UI;
using System;
using System.Linq;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace SCADtools.RibbonItemToolTipSample
{
    public class ExternalApplication : IExternalApplication
    {
        private static readonly string assemblyName = Assembly.GetExecutingAssembly().Location;
        private static readonly string tabName = "SCADtools";
        private static readonly string panelName = "Sample";

        public Result OnStartup(UIControlledApplication application)
        {
            CreateRibbonTab(application);

            return Result.Succeeded;
        }
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        private static void CreateRibbonTab(UIControlledApplication application)
        {
            application.CreateRibbonTab(tabName);

            Autodesk.Revit.UI.RibbonPanel ribbonPanel = application.CreateRibbonPanel(tabName, panelName);

            PushButtonData pushButtonData = CreatePushButtonData();
            ribbonPanel.AddItem(pushButtonData);

            RibbonTab ribbonTab = ComponentManager.Ribbon.Tabs.First(x => x.Title == tabName);
            RibbonPanelSource ribbonPanelSource = ribbonTab.Panels.Select(x => x.Source).Single(x => x.AutomationName == ribbonPanel.Name);
            Autodesk.Windows.RibbonButton ribbonButton = (Autodesk.Windows.RibbonButton)ribbonPanelSource.
                FindItem("CustomCtrl_%CustomCtrl_%" + tabName + "%" +
                ribbonPanelSource.Name + "%" +
                pushButtonData.Name);

            ribbonButton.ToolTip = new RibbonItemToolTip()
            {
                Title = "Stair Symbol",
                Content = "Insert stair symbology.",
                ExpandedContent = "Allows assigning a stair symbol to represent the starting and ending runs.",
                ExpandedImage = new BitmapImage(new Uri("pack://application:,,,/RibbonItemToolTipSample;component/Images/StairSymbolTooltip.gif"))
            };
        }

        private static PushButtonData CreatePushButtonData()
        {
            string className = "SCADtools.RibbonItemToolTipSample.Sample";
            Uri uriImage = new Uri("pack://application:,,,/RibbonItemToolTipSample;component/Images/struturalplan_symbol_stair_32_light.png");
            PushButtonData pushButtonData = new PushButtonData("PushButtonDataSample", "Sample", assemblyName, className)
            {
                LargeImage = new BitmapImage(uriImage),
                Text = "Stair" + "\r\n" + "Symbol"
            };

            return pushButtonData;
        }
    }
}
