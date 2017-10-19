using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace Dark_Toolbox
{
    public partial class ToolCPUCore
    {
        public ToolCPUCore()
        {
            InitializeComponent();
        }

        int cpucore = 0, cpupoint = 0;
        DispatcherTimer timer = new DispatcherTimer();
        List<string> exclude = new List<string>(50);

        public void SetTimer(object sender, RoutedEventArgs e)
        {
            timer.Tick += timer_Tick;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();

            cpucore = Environment.ProcessorCount;

            if (cpucore == 1)
            {
                cpupoint = 1;
            }
            else if (cpucore == 2)
            {
                cpupoint = 3;
            }
            else if (cpucore == 3)
            {
                cpupoint = 7;
            }
            else if (cpucore == 4)
            {
                cpupoint = 15;
            }
            else if (cpucore == 5)
            {
                cpupoint = 31;
            }
            else if (cpucore == 6)
            {
                cpupoint = 63;
            }
            else if (cpucore == 7)
            {
                cpupoint = 127;
            }
            else if (cpucore == 8)
            {
                cpupoint = 255;
            }
            else
            {
                cpupoint = Convert.ToInt32(Process.GetCurrentProcess().ProcessorAffinity);
            }
        }

        private void onoffswitch_Click(object sender, RoutedEventArgs e)
        {
            if (onoffswitch.IsChecked == true)
            {
                status.Content = "ON"; status.Foreground = Brushes.Green;
                isolateonleft(); isolateonright();
            }
            else
            {
                status.Content = "OFF"; status.Foreground = Brushes.Red;
                isolateoffleft(); isolateoffright();
            }
        }

        private void isolateonright()
        {
            int r = 0;
            while (r < rightlist.Items.Count)
            {
                try
                {
                    Process.GetProcessById(Convert.ToInt16(rightlist.Items[r].ToString().Substring(0, rightlist.Items[r].ToString().IndexOf(" || ")))).ProcessorAffinity = (IntPtr)254;
                    Process.GetProcessById(Convert.ToInt16(rightlist.Items[r].ToString().Substring(0, rightlist.Items[r].ToString().IndexOf(" || ")))).PriorityClass = ProcessPriorityClass.High;
                    r++;
                }
                catch
                {
                    exclude.Add(rightlist.Items[r].ToString());
                    rightlist.Items.Remove(rightlist.Items[r]);
                }
            }
        }

        private void isolateonleft()
        {
            int r = 0;
            while (r < leftlist.Items.Count)
            {
                try
                {
                    Process.GetProcessById(Convert.ToInt16(leftlist.Items[r].ToString().Substring(0, leftlist.Items[r].ToString().IndexOf(" || ")))).ProcessorAffinity = (IntPtr)1;
                    r++;
                }
                catch
                {
                    exclude.Add(leftlist.Items[r].ToString());
                    leftlist.Items.Remove(leftlist.Items[r]);
                }
            }
        }

        private void isolateoffright()
        {
            int r = 0;
            while (r < rightlist.Items.Count)
            {
                try
                {
                    Process.GetProcessById(Convert.ToInt16(rightlist.Items[r].ToString().Substring(0, rightlist.Items[r].ToString().IndexOf(" || ")))).ProcessorAffinity = (IntPtr)255;
                    Process.GetProcessById(Convert.ToInt16(rightlist.Items[r].ToString().Substring(0, rightlist.Items[r].ToString().IndexOf(" || ")))).PriorityClass = ProcessPriorityClass.Normal;
                    r++;
                }
                catch
                {
                    exclude.Add(rightlist.Items[r].ToString());
                    rightlist.Items.Remove(rightlist.Items[r]);
                }
            }
        }

        private void isolateoffleft()
        {
            int r = 0;
            while (r < leftlist.Items.Count)
            {
                try
                {
                    Process.GetProcessById(Convert.ToInt16(leftlist.Items[r].ToString().Substring(0, leftlist.Items[r].ToString().IndexOf(" || ")))).ProcessorAffinity = (IntPtr)255;
                    r++;
                }
                catch
                {
                    exclude.Add(leftlist.Items[r].ToString());
                    leftlist.Items.Remove(leftlist.Items[r]);
                }
            }
        }

        private void itemtoright_Click(object sender, RoutedEventArgs e)
        {
            rightlist.Items.Add(leftlist.SelectedItem);
            leftlist.Items.Remove(leftlist.SelectedItem);
        }

        private void itemtoleft_Click(object sender, RoutedEventArgs e)
        {
            leftlist.Items.Add(rightlist.SelectedItem);
            rightlist.Items.Remove(rightlist.SelectedItem);
        }

        private void clearbutton_Click(object sender, RoutedEventArgs e)
        {
            leftlist.Items.Clear();
            rightlist.Items.Clear();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            int r = 0;
            Process[] allprocess = Process.GetProcesses();
            totalprosses.Content = allprocess.Length;
            while (r < allprocess.Length)
            {
                if (!leftlist.Items.Contains(allprocess[r].Id + " || " + allprocess[r].ProcessName) && !rightlist.Items.Contains(allprocess[r].Id + " || " + allprocess[r].ProcessName) && !exclude.Contains(allprocess[r].Id + " || " + allprocess[r].ProcessName))
                {
                    leftlist.Items.Add(allprocess[r].Id + " || " + allprocess[r].ProcessName);
                }
                r++;
                if (keepfly.IsChecked == true)
                {
                    if (status.Content == "ON")
                    {
                        isolateonleft(); isolateonright();
                    }
                }
            }
        }
    }
}
