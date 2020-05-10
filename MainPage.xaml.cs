using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Web.Http;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Test_arena_1
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		int NumOfImage = 1;
		public MainPage()
		{
			this.InitializeComponent();
			backround_checkerAsync(null, null);

			DispatcherTimer Timer1 = new DispatcherTimer();
			Timer1.Tick += Timer_Tick;
			Timer1.Interval = new TimeSpan(0, 0, 1);
			Timer1.Start();

			DispatcherTimer Timer2 = new DispatcherTimer();
			Timer2.Tick += backround_checkerAsync;
			Timer2.Interval = new TimeSpan(24, 0, 0);
			Timer2.Start();
		}

		private void Timer_Tick(object sender, object e)
		{
			DateAndTime.Text = DateTime.Now.ToString("HH:mm:ss") + "\n" + DateTime.Now.ToString("dd.MM.yyy");
		}

		private async void backround_checkerAsync(object sender, object e)
		{
			string strRawJSONString = await getJSONString().ConfigureAwait(true); //sama mis strJSONString
			string lstBingImageURLs = NumOfImage.ToString(); // see on uusima pildi puhul alati 0
			JsonObject jsonObject;
			bool boolParsed = JsonObject.TryParse(strRawJSONString, out jsonObject);

			lstBingImageURLs = jsonObject["images"].GetArray()[NumOfImage-1].GetObject()["url"].GetString();

			var bingURL = "https://www.bing.com" + lstBingImageURLs;
			BitmapSource imgbingImageSource = new BitmapImage(new Uri(bingURL));
			backround.ImageSource = imgbingImageSource;
		}
		string strJSONString;
		public async Task<string> getJSONString()
		{
			// We can specify the region we want for the Bing Image of the Day.
			string strRegion = "en-ww";
			string strBingImageURL = string.Format("http://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n={0}&mkt={1}", NumOfImage, strRegion);

			// Use Windows.Web.Http Namespace as System.Net.Http will be deprecated in future versions.
			HttpClient client = new HttpClient();

			// Using an Async call makes sure the app is responsive during the time the response is fetched.
			// GetAsync sends a Async GET request to the Specified URI.
			HttpResponseMessage response = await client.GetAsync(new Uri(strBingImageURL));

			// Content property get or sets the content of a HTTP response message.
			// ReadAsStringAsync is a method of the HttpContent which asynchronously reads the content of the HTTP Response and returns as a string.
			strJSONString = await response.Content.ReadAsStringAsync();

			return strJSONString;
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
