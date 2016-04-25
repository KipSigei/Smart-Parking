using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Smart_Parking
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class billing : Page
    {

        private DispatcherTimer timer;
        private int minutes, seconds, hours;

        public billing()
        {
            this.InitializeComponent();
            DrawerLayout.InitializeDrawerLayout();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            this.timer = new DispatcherTimer();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (DrawerLayout.IsDrawerOpen)
            {
                DrawerLayout.CloseDrawer();
                e.Handled = true;
            }
            else
            {
                Application.Current.Exit();
            }
        }

        private void DrawerIcon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (DrawerLayout.IsDrawerOpen)
                DrawerLayout.CloseDrawer();
            else
                DrawerLayout.OpenDrawer();
        }


        private void Item1_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var grid = sender as Grid;
            if (grid != null)
            {
                string menuItem = grid.Name;
                switch (menuItem)
                {
                    case "Item1":
                        Frame.Navigate(typeof(MainPage));
                        break;
                }
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (!this.timer.IsEnabled)
            {
                this.StartTimer();
            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            this.StopTimer();
            this.ResetTimer();
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            if (this.timer.IsEnabled)
            {
                this.StopTimer();
            }
        }

        private void timer_Tick(object sender, object e)
        {
            //Just change the text  here

            ++this.seconds;
            if (this.seconds == 60)
            {
                this.minutes++;
                this.seconds = 0;
            }
            if (this.minutes == 60)
            {
                this.hours++;
                this.minutes = 0;
            }

            this.StopwatchText.Text = string.Format("{0} : {1} : {2}", hours, minutes, seconds);
        }



       

        private void StartTimer()
        {
            this.timer.Interval = new TimeSpan(0, 0, 1);
            this.timer.Tick += timer_Tick;
            this.timer.Start();
        }

        private void textBlock1_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void StopTimer()
        {
            this.timer.Tick -= timer_Tick;
            this.timer.Stop();
        }

        private void ResetTimer()
        {
            this.minutes = this.seconds = this.hours = 0;
            this.StopwatchText.Text = "0 : 0 : 0";
        }
    }
}
