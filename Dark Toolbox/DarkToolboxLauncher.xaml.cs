using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualBasic;

namespace Dark_Toolbox
{
    public partial class DarkToolboxLauncher 
    {
        public DarkToolboxLauncher()
        {
            InitializeComponent();
        }

        private void Navigate(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Name.ToString() == "EventScheduler")
            {
                ToolScheduler NewPAGE = new ToolScheduler();
                NewPAGE.Show();
            }
            else if (((Button)sender).Name.ToString() == "UpdatePacker")
            {
                ToolPacker NewPAGE = new ToolPacker();
                NewPAGE.Show();
            }
            else if (((Button)sender).Name.ToString() == "GammaModifier")
            {
                ToolGamma NewPAGE = new ToolGamma();
                NewPAGE.Show();
            }
            else if (((Button)sender).Name.ToString() == "CPUOptimizer")
            {
                ToolCPUCore NewPAGE = new ToolCPUCore();
                NewPAGE.Show();
            }
            else if (((Button)sender).Name.ToString() == "BulkRenamer")
            {
                ToolRenamer NewPAGE = new ToolRenamer();
                NewPAGE.Show();
            }
            else
            {
                MessageBox.Show("Probably");
            }
        }
    }
}
