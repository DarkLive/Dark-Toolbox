using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace Dark_Toolbox {
    public partial class ToolStartup {
        public ToolStartup() {
            InitializeComponent();
        }

        OpenFileDialog filebrowser = new OpenFileDialog();

        private void seppo_Loaded(object sender, RoutedEventArgs e) {

            if ( File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + $@"\DarkStartup.bat") ) {
                string line;
                StreamReader file = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + $@"\DarkStartup.bat");
                while ( ( line = file.ReadLine() ) != null ) {
                    if ( line == "echo off" ) { /* Echo Off */ }
                    else if ( line.StartsWith("start") ) setactions.Items.Add($"Start Software: {line.Replace("start \"\" ", "")}");
                    else if ( line.StartsWith("taskkill") ) setactions.Items.Add($"Stop Software: {line.Replace("taskkill /f /im ", "")}");
                    else if ( line.StartsWith("net start") ) setactions.Items.Add($"Start Service: {line.Replace("net start ", "")}");
                    else if ( line.StartsWith("net stop") ) setactions.Items.Add($"Stop Service: {line.Replace("net stop ", "")}");
                    else if ( line.StartsWith("sleep") ) setactions.Items.Add($"Wait {line.Replace("sleep ", "")} Second");
                    else setactions.Items.Add("UNSUPPORTED ACTION");
                }
                file.Close();
                actionlist.IsEnabled = true;
                addbutton.IsEnabled = true;
                installuninstallbutton.Content = "Uninstall";
            }
            else installuninstallbutton.Content = "Install";
        }

        private void comboselect(object sender, RoutedEventArgs e) {
            if ( ( (ComboBoxItem)sender ).Content.ToString() == "Start Software" ) {
                Nullable<bool> filepicked = filebrowser.ShowDialog();
                if ( filepicked == true ) {
                    string filename = filebrowser.FileName;
                    additionallabel.Visibility = Visibility.Visible; additionaltextbox.Visibility = Visibility.Visible;
                    additionallabel.Content = "Additional arguements for software(e.g. -arg):";
                }
            }
            else if ( ( (ComboBoxItem)sender ).Content.ToString() == "Close Software" ) {
                additionallabel.Visibility = Visibility.Visible; additionaltextbox.Visibility = Visibility.Visible;
                additionallabel.Content = "Proccess name of executable(e.g. cmd.exe):";
            }
            else if ( ( (ComboBoxItem)sender ).Content.ToString() == "Start Service" ) {
                additionallabel.Visibility = Visibility.Visible; additionaltextbox.Visibility = Visibility.Visible;
                additionallabel.Content = "Service Name(e.g. Superfetch):";
            }
            else if ( ( (ComboBoxItem)sender ).Content.ToString() == "Close Service" ) {
                additionallabel.Visibility = Visibility.Visible; additionaltextbox.Visibility = Visibility.Visible;
                additionallabel.Content = "Service Name(e.g. Superfetch):";
            }
            else if ( ( (ComboBoxItem)sender ).Content.ToString() == "Wait \"x\" Seconds" ) {
                additionallabel.Visibility = Visibility.Visible; additionaltextbox.Visibility = Visibility.Visible;
                additionallabel.Content = "How Long?(e.g. 5):";
            }
            else MessageBox.Show("Something Is Wrong...");
        }

        private void installuninstallbutton_Click(object sender, RoutedEventArgs e) {
            if ( installuninstallbutton.Content == "Install" ) {
                File.Create(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + $@"\DarkStartup.bat").Close();
                installuninstallbutton.Content = "Uninstall";
                actionlist.IsEnabled = true;
                addbutton.IsEnabled = true;
                using ( StreamWriter writer = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + $@"\DarkStartup.bat") ) {
                    writer.WriteLine("echo off");
                    writer.Close();
                }
            }
            else {
                File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + $@"\DarkStartup.bat");
                installuninstallbutton.Content = "Install";
                actionlist.IsEnabled = false;
                addbutton.IsEnabled = false;
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e) {
            using ( StreamWriter writer = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + $@"\DarkStartup.bat", true) ) {
                if ( actionlist.SelectionBoxItem.ToString() == "Start Software" ) {
                    setactions.Items.Add($"Start Software: {filebrowser.FileName} {additionaltextbox.Text}");
                    writer.WriteLine($"start \"\" \"{filebrowser.FileName} {additionaltextbox.Text}\"");
                }
                else if ( actionlist.SelectionBoxItem.ToString() == "Close Software" ) {
                    setactions.Items.Add($"Close Software: {additionaltextbox.Text}");
                    writer.WriteLine($"taskkill /f /im \"{additionaltextbox.Text}\"");
                }
                else if ( actionlist.SelectionBoxItem.ToString() == "Start Service" ) {
                    setactions.Items.Add($"Start Service: {additionaltextbox.Text}");
                    writer.WriteLine($"net start \"{additionaltextbox.Text}\"");
                }
                else if ( actionlist.SelectionBoxItem.ToString() == "Close Service" ) {
                    setactions.Items.Add($"Stop Service: {additionaltextbox.Text}");
                    writer.WriteLine($"net stop \"{additionaltextbox.Text}\"");
                }
                else if ( actionlist.SelectionBoxItem.ToString() == "Wait \"x\" Seconds" ) {
                    int number = 0;
                    if ( int.TryParse(additionaltextbox.Text, out number) ) {
                        setactions.Items.Add($"Wait {int.Parse(additionaltextbox.Text)} Second");
                        writer.WriteLine($"sleep {int.Parse(additionaltextbox.Text)}");
                    }
                    else MessageBox.Show("Please check the number you entered.");
                }
                else MessageBox.Show("Something Is Wrong...");

                writer.Close();
                additionaltextbox.Text = "";
            }
        }
    }
}
