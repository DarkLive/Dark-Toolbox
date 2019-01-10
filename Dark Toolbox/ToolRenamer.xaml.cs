using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Dark_Toolbox {
    public partial class ToolRenamer {
        public ToolRenamer() {
            InitializeComponent();
        }

        private void picklocationbutton_Click(object sender, RoutedEventArgs e) {
            var startloc = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            if ( startloc.ShowDialog(this).GetValueOrDefault() ) locationtext.Text = startloc.SelectedPath;
        }

        private void radiocheck(object sender, RoutedEventArgs e) {
            if ( ( (RadioButton)sender ).Name.ToString() == "radrename" ) {
                filenamelabel.Content = "Output Name:";
                filenametip.Content = "* will determine the increasing number location.";
                selecfilenameout.Visibility = Visibility.Visible;
                selecfilenametag.Visibility = Visibility.Hidden;
            }
            else if ( ( (RadioButton)sender ).Name.ToString() == "radretag" ) {
                filenamelabel.Content = "Tag Type:";
                selecfilenameout.Visibility = Visibility.Hidden;
                selecfilenametag.Visibility = Visibility.Visible;
            }
        }

        private void start_Click(object sender, RoutedEventArgs e) {
            if ( locationtext.Text == "..." || locationtext.Text == "" ) MessageBox.Show("Can't found the selected location.");
            else {
                if ( radrename.IsChecked == true ) {
                    string tempqua = "";
                    string filesfound = "Old File Name || New File Name\n";

                    int r = 0, re = 0;
                    var location = new DirectoryInfo(locationtext.Text);
                    FileInfo[] files = location.GetFiles("*", SearchOption.TopDirectoryOnly);
                    FileInfo[] fileextrac = new FileInfo[files.Length];

                    if ( selecextension.Text == "" || selecextension.Text == null ) {
                        MessageBox.Show("File extension can't be empty.");
                    }
                    else if ( selecfilenameout.Text == "" || selecfilenameout.Text == null ) {
                        MessageBox.Show("Output filename can't be empty.");
                    }
                    else if ( !selecfilenameout.Text.Contains("*") ) {
                        MessageBox.Show("Output File Name must include * tag before running the rename action.");
                    }
                    else {
                        while ( r < files.Length ) {
                            if ( files[r].Extension.ToString().Contains(selecextension.Text) ) {
                                fileextrac[re] = files[r];
                                tempqua = selecfilenameout.Text.Replace("*", re.ToString());
                                filesfound = $"{filesfound} {files[r].Name.ToString()} || {tempqua}\n";
                                re++;
                            }
                            r++;
                        }

                        System.Windows.Forms.MessageBoxManager.Yes = "Confirm";
                        System.Windows.Forms.MessageBoxManager.No = "No";
                        System.Windows.Forms.MessageBoxManager.Register();
                        MessageBoxResult extractconfirm = MessageBox.Show(filesfound, "Files Found", MessageBoxButton.YesNo);
                        System.Windows.Forms.MessageBoxManager.Unregister();
                        if ( extractconfirm == MessageBoxResult.Yes ) {
                            r = 0;
                            while ( r < re ) {
                                File.Move(fileextrac[r].FullName, fileextrac[r].FullName.Replace(fileextrac[r].Name, selecfilenameout.Text.Replace("*", ( r + 1 ).ToString()) + fileextrac[r].Extension));
                                r++;
                            }
                            MessageBoxResult endconfirmation = MessageBox.Show("Would you like to open location?", "Action is completed", MessageBoxButton.YesNo);
                            if ( extractconfirm == MessageBoxResult.Yes ) Process.Start($@"{fileextrac[r - 1].Directory}");
                        }
                    }
                }
                else if ( radretag.IsChecked == true ) {
                    string filesfound = "Old File Name || New File Name\n";

                    int r = 0, re = 0;
                    var location = new DirectoryInfo(locationtext.Text);
                    FileInfo[] files = location.GetFiles("*", SearchOption.TopDirectoryOnly);
                    FileInfo[] fileextrac = new FileInfo[files.Length];

                    if ( selecextension.Text == "" || selecextension.Text == null ) {
                        MessageBox.Show("File extension can't be empty.");
                    }
                    else if ( selecfilenametag.Text == "" || selecfilenametag.Text == null ) {
                        MessageBox.Show("Tag type can't be empty.");
                    }
                    else {
                        while ( r < files.Length ) {
                            if ( ( files[r].Name.ToString().Contains(selecextension.Text) ) ) {
                                if ( selecfilenametag.Text == "[TAG]" ) {
                                    if ( files[r].Name.Contains("[") && files[r].Name.Contains("]") ) {
                                        fileextrac[re] = files[r];
                                        filesfound = $"{filesfound} {files[r].Name.ToString()} || {Regex.Replace(files[r].Name, "\\((.*?)\\)", "")}\n";
                                        re++;
                                    }
                                }
                                else if ( selecfilenametag.Text == "(TAG)" ) {
                                    if ( files[r].Name.Contains("(") && files[r].Name.Contains(")") ) {
                                        fileextrac[re] = files[r];
                                        filesfound = $"{filesfound} {files[r].Name.ToString()} || {Regex.Replace(files[r].Name, "\\((.*?)\\)", "")}\n";
                                        re++;
                                    }
                                }
                            }
                            r++;
                        }

                        System.Windows.Forms.MessageBoxManager.Yes = "Confirm";
                        System.Windows.Forms.MessageBoxManager.No = "No";
                        System.Windows.Forms.MessageBoxManager.Register();
                        MessageBoxResult extractconfirm = MessageBox.Show(filesfound, "Files Found", MessageBoxButton.YesNo);
                        System.Windows.Forms.MessageBoxManager.Unregister();

                        if ( extractconfirm == MessageBoxResult.Yes ) {
                            r = 0;
                            if ( selecfilenametag.Text == "[TAG]" ) {
                                while ( r < re ) {
                                    if ( !File.Exists(fileextrac[r].FullName.Replace(fileextrac[r].Name, Regex.Replace(fileextrac[r].Name, "\\[(.*?)\\]", ""))) ) File.Move(fileextrac[r].FullName, ( fileextrac[r].FullName.Replace(fileextrac[r].Name, Regex.Replace(fileextrac[r].Name, "\\[(.*?)\\]", "")) ));
                                    else {
                                        System.Windows.Forms.MessageBoxManager.Yes = "Manually Naming";
                                        System.Windows.Forms.MessageBoxManager.No = "Skip";
                                        System.Windows.Forms.MessageBoxManager.Register();
                                        MessageBoxResult alreadyhave = MessageBox.Show($"There is a file({fileextrac[r].Name}) with the same name on the directory would you like to chose a name for this file manually or skip", "Duplicate Detected", MessageBoxButton.YesNo);
                                        System.Windows.Forms.MessageBoxManager.Unregister();
                                        if ( alreadyhave == MessageBoxResult.Yes ) {
                                            var alreadyloc = new Ookii.Dialogs.Wpf.VistaSaveFileDialog();
                                            if ( alreadyloc.ShowDialog(this).GetValueOrDefault() ) {
                                                File.Move(fileextrac[r].FullName, alreadyloc.FileName + fileextrac[r].Extension);
                                            }
                                        }
                                    }
                                    r++;
                                }
                                MessageBoxResult endconfirmation = MessageBox.Show("Would you like to open location.", "Action is completed", MessageBoxButton.YesNo);
                                if ( extractconfirm == MessageBoxResult.Yes ) Process.Start(@"" + fileextrac[r - 1].Directory);
                            }
                            else if ( selecfilenametag.Text == "(TAG)" ) {
                                while ( r < re ) {
                                    if ( !File.Exists(fileextrac[r].FullName.Replace(fileextrac[r].Name, Regex.Replace(fileextrac[r].Name, "\\((.*?)\\)", ""))) ) File.Move(fileextrac[r].FullName, ( fileextrac[r].FullName.Replace(fileextrac[r].Name, Regex.Replace(fileextrac[r].Name, "\\((.*?)\\)", "")) ));
                                    else {
                                        System.Windows.Forms.MessageBoxManager.Yes = "Manually Naming";
                                        System.Windows.Forms.MessageBoxManager.No = "Skip";
                                        System.Windows.Forms.MessageBoxManager.Register();
                                        MessageBoxResult alreadyhave = MessageBox.Show($"There is a file({fileextrac[r].Name}) with the same name on the directory would you like to chose a name for this file manually or skip", "Duplicate Detected", MessageBoxButton.YesNo);
                                        System.Windows.Forms.MessageBoxManager.Unregister();
                                        if ( alreadyhave == MessageBoxResult.Yes ) {
                                            var alreadyloc = new Ookii.Dialogs.Wpf.VistaSaveFileDialog();
                                            if ( alreadyloc.ShowDialog(this).GetValueOrDefault() ) File.Move(fileextrac[r].FullName, alreadyloc.FileName + fileextrac[r].Extension);
                                        }
                                    }
                                    r++;
                                }
                                MessageBoxResult endconfirmation = MessageBox.Show("Would you like to open location?", "Action is completed", MessageBoxButton.YesNo);
                                if ( extractconfirm == MessageBoxResult.Yes ) Process.Start($@"{fileextrac[r - 1].Directory}");
                            }
                        }
                    }
                }
            }
        }
    }
}
