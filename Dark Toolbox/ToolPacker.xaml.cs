using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.WindowsAPICodePack.Dialogs;
using Ionic.Zip;

namespace Dark_Toolbox
{
    public partial class ToolPacker
    {
        public ToolPacker()
        {
            InitializeComponent();
            startdatepicker.DisplayDateEnd = DateTime.Now;
            enddatepicker.DisplayDateEnd = DateTime.Now;
        }

        private void picklocationbutton_Click(object sender, RoutedEventArgs e)
        {
            var startloc = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            if (startloc.ShowDialog(this).GetValueOrDefault())
            {
                locationtext.Text = startloc.SelectedPath;
            }
        }

        private void extract_Click(object sender, RoutedEventArgs e)
        {
            var location = new DirectoryInfo(locationtext.Text);
            FileInfo[] files = null;

            if (searchmain.IsChecked == true)
            {
                files = location.GetFiles("*", SearchOption.TopDirectoryOnly);
            }
            else if (searchmainsub.IsChecked == true)
            {
                files = location.GetFiles("*", SearchOption.AllDirectories);
            }

            if (startdatepicker.SelectedDate > enddatepicker.SelectedDate)
            {
                MessageBox.Show("Start date can't be higher than end date.");
            }
            else if (locationtext.Text == "...")
            {
                MessageBox.Show("Location is not selected.");
            }
            else
            {
                int x = 0;
                FileInfo[] extractfile = new FileInfo[files.Length];

                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].CreationTime >= startdatepicker.SelectedDate.Value && files[i].CreationTime <= enddatepicker.SelectedDate.Value)
                    {
                        extractfile[x] = files[i];
                        x++;
                    }
                    else if (files[i].LastWriteTime >= startdatepicker.SelectedDate.Value && files[i].LastWriteTime <= enddatepicker.SelectedDate.Value)
                    {
                        extractfile[x] = files[i];
                        x++;
                    }
                }
                if (x != 0)
                {
                    int r = 0;
                    string extractfilest = "Filename || CreationTime || LastWriteTime \n";
                    while (r <= x - 1)
                    {
                        extractfilest = extractfilest + extractfile[r].Name + " || " + extractfile[r].CreationTime + " || " + extractfile[r].LastWriteTime + "\n";
                        r++;
                    }
                    System.Windows.Forms.MessageBoxManager.Yes = "Extract";
                    System.Windows.Forms.MessageBoxManager.No = "Dont Extract";
                    System.Windows.Forms.MessageBoxManager.Register();
                    MessageBoxResult extractconfirm = MessageBox.Show(extractfilest, "Files Found", MessageBoxButton.YesNo);
                    System.Windows.Forms.MessageBoxManager.Unregister();
                    if (extractconfirm == MessageBoxResult.Yes)
                    {
                        System.Windows.Forms.MessageBoxManager.Yes = "Desktop";
                        System.Windows.Forms.MessageBoxManager.No = "Let Me Choose";
                        System.Windows.Forms.MessageBoxManager.Register();
                        MessageBoxResult extractfolderconfirm = MessageBox.Show("Where would you like to extract?", "Extraction Location", MessageBoxButton.YesNo);
                        System.Windows.Forms.MessageBoxManager.Unregister();

                        string extractloc = "";

                        if (extractfolderconfirm == MessageBoxResult.Yes)
                        {
                            extractloc = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        }
                        else
                        {
                            var endloc = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
                            if (endloc.ShowDialog(this).GetValueOrDefault())
                            {
                                extractloc = endloc.SelectedPath;
                            }
                        }

                        int exnum = 0;
                        if (extractfolder.IsChecked == true)
                        {
                            exnumber:
                            if (Directory.Exists(extractloc + "\\Extract" + exnum))
                            {
                                exnum++;
                                goto exnumber;
                            }
                            else
                            {
                                Directory.CreateDirectory(extractloc + "\\Extract" + exnum);
                            }
                        }

                        ZipFile zip = new ZipFile();

                        r = 0;
                        while (r <= x - 1)
                        {
                            if (extractfolder.IsChecked == true)
                            {
                                extractfile[r].CopyTo(extractloc + "\\Extract" + exnum + "\\" + extractfile[r].Name,true);
                            }
                            else if (extractzip.IsChecked == true)
                            {
                                zip.AddFile(extractfile[r].FullName, "Extract");
                                if (r == x - 1)
                                {
                                    exnum = 0;
                                    exnumbar:
                                    if (File.Exists(extractloc + "\\Extract" + exnum + ".zip"))
                                    {
                                        exnum++;
                                        goto exnumbar;
                                    }
                                    zip.Save(extractloc + "\\Extract" + exnum + ".zip");
                                }
                            }
                            r++;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Extraction Canceled.");
                    }
                }
                else
                {
                    MessageBox.Show("Can't found any files created/modified between the dates selected.");
                }
            }
        }

        private void checkstart(object sender, RoutedEventArgs e)
        {
            if (starttoday.IsChecked == true)
            {
                startdatepicker.SelectedDate = DateTime.Now;
                enddatepicker.SelectedDate = DateTime.Now;
                endtoday.IsChecked = true;
                startdatepicker.IsEnabled = false;
                enddatepicker.IsEnabled = false;
            }
            else
            {
                endtoday.IsChecked = false;
                startdatepicker.IsEnabled = true;
                enddatepicker.IsEnabled = true;
            }
        }

        private void checkend(object sender, RoutedEventArgs e)
        {

            if (endtoday.IsChecked == true)
            {
                enddatepicker.SelectedDate = DateTime.Now;
                enddatepicker.IsEnabled = false;
            }
            else
            {
                enddatepicker.IsEnabled = true;
            }
        }
    }
}
