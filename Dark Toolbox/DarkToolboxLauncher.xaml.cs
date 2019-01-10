using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using MahAppsMetroThemes;

namespace Dark_Toolbox {
    public partial class DarkToolboxLauncher {
        public DarkToolboxLauncher() {
            InitializeComponent();
        }

        private void Navigate(object sender, RoutedEventArgs e) {
            if ( ( (Button)sender ).Name.ToString() == "EventScheduler" ) { ToolScheduler NewPAGE = new ToolScheduler(); NewPAGE.Show(); }
            else if ( ( (Button)sender ).Name.ToString() == "UpdatePacker" ) { ToolPacker NewPAGE = new ToolPacker(); NewPAGE.Show(); }
            else if ( ( (Button)sender ).Name.ToString() == "GammaModifier" ) { ToolGamma NewPAGE = new ToolGamma(); NewPAGE.Show(); }
            else if ( ( (Button)sender ).Name.ToString() == "CPUOptimizer" ) { ToolCPUCore NewPAGE = new ToolCPUCore(); NewPAGE.Show(); }
            else if ( ( (Button)sender ).Name.ToString() == "BulkRenamer" ) { ToolRenamer NewPAGE = new ToolRenamer(); NewPAGE.Show(); }
            else if ( ( (Button)sender ).Name.ToString() == "StartupManager" ) { ToolStartup NewPAGE = new ToolStartup(); NewPAGE.Show(); }
            else MessageBox.Show("Contact me thru GitHub if you have a feature request");
        }

        private MetroWindow accentThemeWindow;

        private void ChangeAppStyleButtonClick(object sender, RoutedEventArgs e) {
            if ( accentThemeWindow != null ) { accentThemeWindow.Activate(); return; }
            accentThemeWindow = new AccentStyleWindow();
            accentThemeWindow.Owner = this;
            accentThemeWindow.Closed += (o, args) => accentThemeWindow = null;
            accentThemeWindow.Left = this.Left + this.ActualWidth / 2.0;
            accentThemeWindow.Top = this.Top + this.ActualHeight / 2.0;
            accentThemeWindow.Show();
        }
    }
}
