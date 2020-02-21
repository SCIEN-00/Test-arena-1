﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Enumeration;
using Windows.Devices.Gpio;
using Windows.Devices.I2c;
using Windows.Devices.Spi;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class GPIO_page : Page
    {
        private I2cDevice SHT30_sensor;
        private DispatcherTimer SHT30_sensor_timer;

        public GPIO_page()
        {
            this.InitializeComponent();
        }

        private async void GPIO_Switch_Toggled(object sender, RoutedEventArgs e)
        {
            GpioPin White_LED;

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
            var settings = new I2cConnectionSettings(0x44); // Arduino address
            settings.BusSpeed = I2cBusSpeed.StandardMode;
            string aqs = I2cDevice.GetDeviceSelector("I2C1");
            var dis = await DeviceInformation.FindAllAsync(aqs);
            SHT30_sensor = await I2cDevice.FromIdAsync(dis[0].Id, settings);

            //string i2cDeviceSelector = I2cDevice.GetDeviceSelector();
            //try
            //{
            //    await I2cController.GetDefaultAsync();
            //}
            //catch (Exception ex)
            //{
            //    GPIO_Info.Visibility = Visibility.Visible;
            //    GPIO_Info.Text += "\n" + ex.Source;
            //}

            //IReadOnlyList<DeviceInformation> devices = await DeviceInformation.FindAllAsync(i2cDeviceSelector);
            //var SHT30_sensor_settings = new I2cConnectionSettings(0x44); //SHT30 default I2C address is 0x44
            //try
            //{
            //    SHT30_sensor = await I2cDevice.FromIdAsync(devices[0].Id, SHT30_sensor_settings);
            //}
            //catch (Exception ex)
            //{
            //    GPIO_Info.Visibility = Visibility.Visible;
            //    GPIO_Info.Text += "\n" + ex.Source;
            //}

            SHT30_sensor_timer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(1000) };
            SHT30_sensor_timer.Tick += SHT30_sensor_tick;
        }

        private void SPI_Switch_Toggled(object sender, RoutedEventArgs e)
        {
            if(SPI_enable.IsOn)
            {

                GPIO_Info.Text = "No SPI devices connected.";
                SPI_enable.IsOn = false;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>")]
        private void SHT30_sensor_tick(object sender, object e)
        {
            var command = new byte[1];
            var HumidityData = new byte[2];
            var TemperatureData = new byte[2];

            command[0] = 0xE5; //Read humidity
            SHT30_sensor.WriteRead(command, HumidityData);

            command[0] = 0xE3; //Read temperature
            SHT30_sensor.WriteRead(command, TemperatureData);

            // Calculate and report the humidity.
            var rawHumidityReading = HumidityData[0] << 8 | HumidityData[1];
            var humidityRatio = rawHumidityReading / (float)65536;
            double humidity = -6 + (125 * humidityRatio);
            CurrentHumidity.Text = humidity.ToString();

            // Calculate and report the temperature.
            var rawTempReading = TemperatureData[0] << 8 | TemperatureData[1];
            var tempRatio = rawTempReading / (float)65536;
            double temperature = (-46.85 + (175.72 * tempRatio)) * 9 / 5 + 32;
            CurrentTemp.Text = temperature.ToString();
        }
    }
}