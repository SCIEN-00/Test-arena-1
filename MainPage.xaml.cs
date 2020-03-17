using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Test_arena_1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            DispatcherTimer Timer = new DispatcherTimer();
            Timer.Tick += Timer_Tick;
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            DateAndTime.Text = DateTime.Now.ToString("HH:mm:ss") + "\n" + DateTime.Now.ToString("dd.MM.yyy");
        }

        private void System_Tapped(object sender, RoutedEventArgs e)
        {
            if (System_b.IsSelected)
            {
                SystemSettingsPage_win.Visibility = Visibility.Collapsed;
                System_b.IsSelected = false;
            }
            else
            {
                GpioTesting_win.Visibility = Visibility.Collapsed;
                GPIO_b.IsSelected = false;
                SystemSettingsPage_win.Visibility = Visibility.Visible;
                System_b.IsSelected = true;
                LiveCharts_win.Visibility = Visibility.Collapsed;
                LiveCharts_b.IsSelected = false;
                DataPlotting_win.Visibility = Visibility.Collapsed;
                Data_plotting_b.IsSelected = false;
            }
        }

        private void GPIO_Tapped(object sender, RoutedEventArgs e)
        {
            if (GPIO_b.IsSelected)
            {
                GpioTesting_win.Visibility = Visibility.Collapsed;
                GPIO_b.IsSelected = false;
            }
            else
            {
                GpioTesting_win.Visibility = Visibility.Visible;
                GPIO_b.IsSelected = true;
                SystemSettingsPage_win.Visibility = Visibility.Collapsed;
                System_b.IsSelected = false;
                LiveCharts_win.Visibility = Visibility.Collapsed;
                LiveCharts_b.IsSelected = false;
                DataPlotting_win.Visibility = Visibility.Collapsed;
                Data_plotting_b.IsSelected = false;
            }
        }
    }
}
