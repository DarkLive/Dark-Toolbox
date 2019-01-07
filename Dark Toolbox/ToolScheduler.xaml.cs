using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Dark_Toolbox
{
    public partial class ToolScheduler
    {
        DispatcherTimer timer = new DispatcherTimer();
        OpenFileDialog filebrowser = new OpenFileDialog();
        Process process = new Process();
        ProcessStartInfo startInfo = new ProcessStartInfo();
        public string[,] que = new string[10, 4];
        public int quecount = 0;

        public ToolScheduler()
        {
            InitializeComponent();
            timer.Tick += timer_Tick;
            timer.Interval = TimeSpan.FromMinutes(1);
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            filebrowser.Filter = "ALL Files (*.*)|*.*|EXE Files (*.exe)|*.exe|BAT Files (*.bat)|*.bat";
            softwarelocklabel.Visibility = Visibility.Hidden; softwareloc.Visibility = Visibility.Hidden;
        }

        private void schedulethis_Click(object sender, RoutedEventArgs e)
        {
            if (triggertimepicker.SelectedDate.Value.ToShortDateString() != null || triggertimepicker.SelectedDate.Value.ToShortDateString() != "")
            {
                if (triggertypepicker.SelectionBoxItem.ToString() != null || triggertypepicker.SelectionBoxItem.ToString() != "")
                {
                    string pickedtime = triggertimepicker.SelectedDate.Value.ToShortDateString() + " | " + triggertimepicker.SelectedDate.Value.ToShortTimeString() + " | " + triggertypepicker.SelectionBoxItem.ToString() + " | " + softwareloc.Text.Substring(softwareloc.Text.LastIndexOf("\\") + 1);
                    que[quecount, 0] = triggertimepicker.SelectedDate.Value.ToShortDateString(); que[quecount, 1] = triggertimepicker.SelectedDate.Value.ToShortTimeString(); que[quecount, 2] = triggertypepicker.SelectionBoxItem.ToString(); que[quecount, 3] = softwareloc.Text;
                    knowntriggers.Items.Add(pickedtime.ToString());
                    quecount++;
                    timer.Start();
                    if (quecount == 10)
                    {
                        schedulethis.IsEnabled = false;
                    }
                }
                else
                {
                    MessageBox.Show("You need to pick a trigger type before scheduling any action");
                }
            }
            else
            {
                MessageBox.Show("You need to pick a trigger date and time before scheduling any action");
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            for (int i =0; i<quecount;i++)
            {
                if (DateTime.Now.ToShortDateString() == que[i, 0])
                {
                    if (DateTime.Now.ToShortTimeString() == que[i, 1])
                    {
                        switch (que[i, 2])
                        {
                            case "Start Software":
                                startInfo.FileName = "cmd.exe";
                                startInfo.Arguments = "/C START " + que[i,3];
                                process.StartInfo = startInfo;
                                process.Start();
                                break;
                            case "Close Software":
                                startInfo.FileName = "cmd.exe";
                                startInfo.Arguments = "/C TASKKILL /IM " + que[i, 3].Substring(que[i, 3].LastIndexOf("\\") + 1) + " /F";
                                startInfo.Verb = "runas";
                                process.StartInfo = startInfo;
                                process.Start();
                                break;
                            case "Lock Computer":
                                startInfo.FileName = "cmd.exe";
                                startInfo.Arguments = "/C Rundll32.exe User32.dll,LockWorkStation";
                                startInfo.Verb = "runas";
                                process.StartInfo = startInfo;
                                process.Start();
                                break;
                            case "Sleep Computer":
                                startInfo.FileName = "cmd.exe";
                                startInfo.Arguments = "/C rundll32.exe powrprof.dll,SetSuspendState 0,1,0";
                                process.StartInfo = startInfo;
                                process.Start();
                                break;
                            case "Hibernate Computer":
                                startInfo.FileName = "cmd.exe";
                                startInfo.Arguments = "/C rundll32.exe PowrProf.dll,SetSuspendState";
                                startInfo.Verb = "runas";
                                process.StartInfo = startInfo;
                                process.Start();
                                break;
                            case "Restart Computer":
                                startInfo.FileName = "cmd.exe";
                                startInfo.Arguments = "/C Shutdown.exe -r -t 00";
                                process.StartInfo = startInfo;
                                process.Start();
                                break;
                            case "Shutdown Computer":
                                startInfo.FileName = "cmd.exe";
                                startInfo.Arguments = "/C Shutdown.exe -s -t 00";
                                process.StartInfo = startInfo;
                                process.Start();
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        private void comboselect(object sender, RoutedEventArgs e)
        {
            if(((ComboBoxItem)sender).Content.ToString()=="Start Software")
            {
                Nullable<bool> filepicked = filebrowser.ShowDialog();
                if (filepicked == true)
                {
                    string filename = filebrowser.FileName;
                    softwareloc.Text = filename;
                    softwarelocklabel.Visibility = Visibility.Visible; softwareloc.Visibility = Visibility.Visible;
                }
            }
            else if (((ComboBoxItem)sender).Content.ToString() == "Close Software")
            {
                Nullable<bool> filepicked = filebrowser.ShowDialog();
                if (filepicked == true)
                {
                    string filename = filebrowser.FileName;
                    softwareloc.Text = filename;
                    softwarelocklabel.Visibility = Visibility.Visible; softwareloc.Visibility = Visibility.Visible;
                }
            }
            else
            {
                softwareloc.Text = ""; softwarelocklabel.Visibility = Visibility.Hidden; softwareloc.Visibility = Visibility.Hidden;
            }
        }
    }
}
