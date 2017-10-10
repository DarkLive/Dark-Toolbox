using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace Dark_Toolbox
{
    public partial class ToolGamma
    {
        public ToolGamma()
        {
            InitializeComponent();
        }

        bool isloadfinished = false;

        private void isloaddone(object sender, RoutedEventArgs e)
        {
            isloadfinished = true;
            statcombined.Content = SliderCombined.Value; statred.Content = SliderRed.Value; statblu.Content = SliderBlue.Value; statgre.Content = SliderGreen.Value;
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("gdi32.dll")]
        public static extern bool SetDeviceGammaRamp(IntPtr hDC, ref RAMP lpRamp);

        [DllImport("gdi32.dll")]
        public static extern bool GetDeviceGammaRamp(IntPtr hDC, ref RAMP lpRamp);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct RAMP
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public UInt16[] Red;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public UInt16[] Green;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public UInt16[] Blue;
        }

        public static void SetGamma(int gamma)
        {
            if (gamma <= 256 && gamma >= 1)
            {
                RAMP ramp = new RAMP();
                ramp.Red = new ushort[256];
                ramp.Green = new ushort[256];
                ramp.Blue = new ushort[256];
                for (int i = 1; i < 256; i++)
                {
                    int iArrayValue = i * (gamma + 128);

                    if (iArrayValue > 65535) iArrayValue = 65535;

                    ramp.Red[i] = ramp.Blue[i] = ramp.Green[i] = (ushort)iArrayValue;//HERE
                }
                SetDeviceGammaRamp(GetDC(IntPtr.Zero), ref ramp);
            }
        }

        public static void SetGammaColor(int red, int blue, int green)
        {
            if (red <= 256 && red >= 1)
            {
                if (blue <= 256 && blue >= 1)
                {
                    if (green <= 256 && green >= 1)
                    {
                        RAMP ramp = new RAMP();
                        ramp.Red = new ushort[256];
                        ramp.Green = new ushort[256];
                        ramp.Blue = new ushort[256];
                        for (int i = 1; i < 256; i++)
                        {
                            int RedValue = i * (red + 128);
                            int BluValue = i * (blue + 128);
                            int GreValue = i * (green + 128);

                            if (RedValue > 65535) RedValue = 65535;
                            if (BluValue > 65535) BluValue = 65535;
                            if (GreValue > 65535) GreValue = 65535;

                            ramp.Red[i] = (ushort)RedValue;
                            ramp.Blue[i] = (ushort)BluValue;
                            ramp.Green[i] = (ushort)GreValue;
                        }
                        SetDeviceGammaRamp(GetDC(IntPtr.Zero), ref ramp);
                    }
                }
            }
        }      

        private void Reset(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Tag.ToString() == "ResetCombined")
            {
                SliderCombined.Value = 128;
            }
            else if (((Button)sender).Tag.ToString() == "ResetRed")
            {
                SliderRed.Value = 128;
            }
            else if (((Button)sender).Tag.ToString() == "ResetBlu")
            {
                SliderBlue.Value = 128;
            }
            else if (((Button)sender).Tag.ToString() == "ResetGre")
            {
                SliderGreen.Value = 128;
            }
        }

        private void SliderMove(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (isloadfinished == true)
            {
                if (((Slider)sender).Name == "SliderCombined")
                {
                    SetGamma(Convert.ToByte(((Slider)sender).Value));
                    statcombined.Content = Convert.ToByte(((Slider)sender).Value);
                }
                else 
                {
                    SetGammaColor(Convert.ToByte(SliderRed.Value), Convert.ToByte(SliderBlue.Value), Convert.ToByte(SliderGreen.Value));
                    statred.Content = ((Slider)sender).Value.ToString();
                }
            }
        }
    }
}
