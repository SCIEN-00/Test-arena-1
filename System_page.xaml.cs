using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        public System_page()
        {
            this.InitializeComponent();
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
    }
}
