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
		public MainPage()
		{
			this.InitializeComponent();
			DispatcherTimer Timer1 = new DispatcherTimer();
			Timer1.Tick += Timer_Tick;
			Timer1.Interval = new TimeSpan(0, 0, 1);
			Timer1.Start();
			DispatcherTimer Timer2 = new DispatcherTimer();
			Timer2.Tick += backround_checkerAsync;
			Timer2.Interval = new TimeSpan(0, 1, 0);
			Timer2.Start();
		}

		private void Timer_Tick(object sender, object e)
		{
			DateAndTime.Text = DateTime.Now.ToString("HH:mm:ss") + "\n" + DateTime.Now.ToString("dd.MM.yyy");
		}

		private async void backround_checkerAsync(object sender, object e)
		{
			int _numOfImages = 0; //Convert.ToInt32(e.NewValue);
			string strRawJSONString = await getJSONString().ConfigureAwait(true); //sama mis strJSONString
			string _lstBingImageURLs = _numOfImages.ToString(); // see on uusima pildi puhul alati 0
			JsonObject jsonObject;
			bool boolParsed = JsonObject.TryParse(strRawJSONString, out jsonObject);

			_lstBingImageURLs = jsonObject["images"].GetArray()[_numOfImages].GetObject()["url"].GetString();

			var bingURL = "https://www.bing.com" + _lstBingImageURLs;
			BitmapSource imgbingImageSource = new BitmapImage(new Uri(bingURL));
			Image imgbingImage = new Image();
			imgbingImage.Source = imgbingImageSource;
			backround.ImageSource = imgbingImageSource;
		}
		string strJSONString;
		public async Task<string> getJSONString()
		{
			// We can specify the region we want for the Bing Image of the Day.
			string strRegion = "en-ww";
			//string strBingImageURL = string.Format("http://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n={0}&mkt={1}", _numOfImages, strRegion);
			string strBingImageURL = string.Format("http://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1&mkt=en-ww");

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

		public List<string> parseJSONString(int _numOfImages, string _strRawJSONString)
		{
			List<string> _lstBingImageURLs = new List<string>(_numOfImages);
			// JsonObject class implements the IMap interface, which helps in manipulating the name/value pairs like a dictionary.
			JsonObject jsonObject;

			// JsonObject.TryParse parses the JSON string into a JSON value, which returns a boolean value, indicating success or failure.
			// TryParse is an added safe measure to avoid an execption while parsing.
			bool boolParsed = JsonObject.TryParse(_strRawJSONString, out jsonObject);
			if (boolParsed)
			{
				for (int i = 0; i < _numOfImages; i++)
				{
					// The retrieval structure depends upon JSON string, the base key in our case is "images".
					// If you retrieve more than one image, "images" key will have more than one array values.
					// Each Array value has a key/value pair "url", which we retrieve and conver it to a string.
					_lstBingImageURLs.Add(jsonObject["images"].GetArray()[i].GetObject()["url"].GetString());
				}
			}

			// Return the list containing URLs.
			return _lstBingImageURLs;
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
