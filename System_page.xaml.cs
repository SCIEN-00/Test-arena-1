using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Test_arena_1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class System_page : Page
    {
		private I2cDevice I2CScreen;

        public System_page()
        {
            this.InitializeComponent();
            InitI2CScreen();
        }

        private async void InitI2CScreen()
        {
            try
            {
                var settings = new I2cConnectionSettings(0x40) //raspberry official screen i2c address 0x45
                {
                    BusSpeed = I2cBusSpeed.FastMode
                };
                IReadOnlyList<DeviceInformation> devices = await DeviceInformation.FindAllAsync(I2cDevice.GetDeviceSelector());
                I2CScreen = await I2cDevice.FromIdAsync(devices[0].Id, settings);
            }
            catch (Exception e)
            {
                Info_field.Text = "I2C Initialization failed. Exception: " + e.Message;
                return;
            }
        }

        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private void RestartSystem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ShutdownManager.BeginShutdown(ShutdownKind.Restart, TimeSpan.FromSeconds(0));
            }
            catch (Exception ex)
            {
                Info_field.Text = ex.Message;
            }
        }

        private void Screen_brightnes_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            int brightnes = 255 * (int)Screen_brightnes.Value / 100;
            byte[] writeBuff = new byte[] { 0x86, (byte)brightnes }; //backlight address, brightness 0-255. Write the register settings
            try
            {
                Screen_brightnes.Header = "Screen brightnes: " + ((int)Screen_brightnes.Value).ToString() + "%";
                I2CScreen.Write(writeBuff);
            }
            catch (Exception ex)
            {
                Info_field.Text = ex.Message;
            }
		}
	}
}
