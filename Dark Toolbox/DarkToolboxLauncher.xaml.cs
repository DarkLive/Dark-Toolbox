﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Navigation;

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
            else if (((Button)sender).Name.ToString() == "Placeholder")
            {
                MessageBox.Show("Probably");
            }
        }
    }
}
