using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Enumeration;
using Windows.Devices.Gpio;
using Windows.Devices.I2c;
using Windows.Devices.Spi;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Point = Windows.Foundation.Point;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Test_arena_1
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class GPIO_page : Page
	{
		DispatcherTimer SHT30_sensor_timer = new DispatcherTimer();
		I2cDevice SHT30_sensor_1;
		const byte SHT30_I2C_addr = 0x45; //Default I2C address of SHT30 sensor
		Byte[] SensorData = new byte[] { 0, 0, 0, 0, 0, 0 }, command = { 0x2C, 0x06 }; //Send measurement command(0x2C, 0x06)
		int i = 1;

		GpioPin White_LED;
		public GPIO_page()
		{
			this.InitializeComponent();
		}

		private async void GPIO_Switch_Toggled(object sender, RoutedEventArgs e)
		{
			try
			{
				await GpioController.GetDefaultAsync();
			}
			catch (Exception ex)
			{
				GPIO_Info.Visibility = Visibility.Visible;
				GPIO_Info.Text += "\n" + ex.Source;
			}
		}

		private async void I2C_Switch_Toggled(object sender, RoutedEventArgs e)
		{
			if (I2C_enable.IsOn == true)
			{
				try
				{
					var settings = new I2cConnectionSettings(SHT30_I2C_addr)
					{
						BusSpeed = I2cBusSpeed.FastMode
					};
					IReadOnlyList<DeviceInformation> devices = await DeviceInformation.FindAllAsync(I2cDevice.GetDeviceSelector());
					SHT30_sensor_1 = await I2cDevice.FromIdAsync(devices[0].Id, settings);
					if (SHT30_sensor_1 == null)
					{
						GPIO_Info.Visibility = Visibility.Visible;
						GPIO_Info.Text = "Slave address {0} on I2C Controller {1} is currently in use by another application. Please ensure that no other applications are using I2C." + settings.SlaveAddress + devices[0].Id;
						return;
					}

					SHT30_sensor_timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(1000) };
					SHT30_sensor_timer.Tick += SHT30_sensor_tick;
					SHT30_sensor_timer.Start();

					CurrentHumidity.Visibility = Visibility.Visible;
					CurrentTemp.Visibility = Visibility.Visible;
				}
				catch (Exception ex)
				{
					GPIO_Info.Visibility = Visibility.Visible;
					GPIO_Info.Text += "\n" + ex.Source;
				}
			}
			else
			{
				SHT30_sensor_1.Dispose();
				SHT30_sensor_timer.Stop();
			}
		}

		private void SPI_Switch_Toggled(object sender, RoutedEventArgs e)
		{
			if (SPI_enable.IsOn)
			{

				GPIO_Info.Text = "No SPI devices connected.";
				SPI_enable.IsOn = false;
			}
		}

		private void SHT30_sensor_tick(object sender, object e)
		{
			try
			{
				SHT30_sensor_1.Write(command);
			}
			catch(Exception ex)
			{
				GPIO_Info.Visibility = Visibility.Visible;
				GPIO_Info.Text += "\n" + ex;
				SHT30_sensor_1.Dispose();
				return;
			}
			
			try
			{
				SHT30_sensor_1.Read(SensorData);
			}
			catch (Exception ex)
			{
				GPIO_Info.Visibility = Visibility.Visible;
				GPIO_Info.Text += "\n" + ex;
				SHT30_sensor_1.Dispose();
				return;
			}

			// Calculate and report the temperature.
			var rawTempReading = SensorData[0] << 8 | SensorData[1];
			double temperature = (35 * (float)rawTempReading / 13107 - 45);
			CurrentTemp.Text = "Temperature: " + Math.Round(temperature, 1, MidpointRounding.AwayFromZero).ToString() + " ℃";

			// Calculate and report the humidity.
			var rawHumidityReading = SensorData[3] << 8 | SensorData[4];
			double humidity = 20* (float)rawHumidityReading/13107;
			CurrentHumidity.Text = "Humidity: " + Math.Round(humidity, 1, MidpointRounding.AwayFromZero).ToString() + " %";
		}
	}
}
