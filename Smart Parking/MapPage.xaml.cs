using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Smart_Parking
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapPage : Page
    {
        public MapPage()
        {
            this.InitializeComponent();
            
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
            Frame.Navigate(typeof(MainPage));
        }

        private void buttonRoad_Click(Object sender, RoutedEventArgs e)
        {
            map1.Style = MapStyle.Road;
        }

        private void buttonAerial_Click(Object sender, RoutedEventArgs e)
        {
            map1.Style = MapStyle.Aerial;
        }

        private void buttonHybrid_Click(Object sender, RoutedEventArgs e)
        {
            map1.Style = MapStyle.AerialWithRoads;
        }

        private void buttonTerrain_Click(Object sender, RoutedEventArgs e)
        {
            map1.Style = MapStyle.Terrain;
        }

        private void map1_Loaded(Object sender, RoutedEventArgs e)
        {
            ZoomToSarit();
        }

        private void buttonSarit_Click(object sender, RoutedEventArgs e)
        {
            ZoomToSarit();
        }
        private async void ZoomToSarit()
        {
            var sarit = new Geopoint(new BasicGeoposition() { Latitude = -1.261199, Longitude = 36.801981});
            await map1.TrySetViewAsync(sarit, 11.4086892086054, 0, 0, MapAnimationKind.Bow);
        }

        private async void buttonYou_Click(object sender, RoutedEventArgs e)
        {
            var gl = new Geolocator() { DesiredAccuracy = PositionAccuracy.High };
            var location = await gl.GetGeopositionAsync(TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(5));

            var pin = new MapIcon()
            {
                Location = location.Coordinate.Point,
                Title = "You are here!",
                Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/pin.png")),
                NormalizedAnchorPoint = new Point() { X = 0.32, Y = 0.78 },
            };
            map1.MapElements.Add(pin);
            await map1.TrySetViewAsync(location.Coordinate.Point, 16, 0, 0, MapAnimationKind.Bow);
        }

        private async void buttonYaya_Click(object sender, RoutedEventArgs e)
        {
            var yaya = new Geopoint(new BasicGeoposition() { Latitude = -1.292949, Longitude = 36.787766 });
            var youPin = CreatePin();
            map1.Children.Add(youPin);
            MapControl.SetLocation(youPin, yaya);
            MapControl.SetNormalizedAnchorPoint(youPin, new Point(0.0, 1.0));
            await map1.TrySetViewAsync(yaya, 15, 0, 0, MapAnimationKind.Bow);
        }

        private async void buttonGalleria_Click(object sender, RoutedEventArgs e)
        {
            var galleria = new Geopoint(new BasicGeoposition() { Latitude = -1.342902 , Longitude = 36.766198 });
            await map1.TrySetViewAsync(galleria, 16.5, 0, 0, MapAnimationKind.Bow);
        }


        private async void Landmarks_Checked(object sender, RoutedEventArgs e)
        {
            await map1.TrySetViewAsync(map1.Center, map1.ZoomLevel, 340, 35, MapAnimationKind.Bow);
            map1.LandmarksVisible = true;
        }

        private async void Landmarks_Unchecked(object sender, RoutedEventArgs e)
        {
            map1.LandmarksVisible = false;
            await map1.TrySetViewAsync(map1.Center, map1.ZoomLevel, 0, 0, MapAnimationKind.Bow);
        }

        private void Pedestrian_Checked(object sender, RoutedEventArgs e)
        {
            map1.PedestrianFeaturesVisible = true;
        }

        private void Pedestrian_Unchecked(object sender, RoutedEventArgs e)
        {
            map1.PedestrianFeaturesVisible = false;
        }

        private void Dark_Checked(object sender, RoutedEventArgs e)
        {
            map1.ColorScheme = MapColorScheme.Dark;
        }

        private void Dark_Unchecked(object sender, RoutedEventArgs e)
        {
            map1.ColorScheme = MapColorScheme.Light;
        }

        private void Traffic_Checked(object sender, RoutedEventArgs e)
        {
            map1.TrafficFlowVisible = true;
        }

        private void Traffic_Unchecked(object sender, RoutedEventArgs e)
        {
            map1.TrafficFlowVisible = false;
        }

        private void TileSource_Checked(object sender, RoutedEventArgs e)
        {
            var httpsource = new HttpMapTileDataSource("http://a.tile.openstreetmap.org/{zoomlevel}/{x}/{y}.png");
            var ts = new MapTileSource(httpsource)
            {
                Layer = MapTileLayer.BackgroundReplacement
            };
            map1.Style = MapStyle.None;
            map1.TileSources.Add(ts);
        }

        private void TileSource_Unchecked(object sender, RoutedEventArgs e)
        {
            map1.TileSources.Clear();
            map1.Style = MapStyle.Road;
        }

        private void CustTileSource_Checked(object sender, RoutedEventArgs e)
        {
            var customSource = new CustomMapTileDataSource();
            customSource.BitmapRequested += async (o, args) =>
            {
                var deferral = args.Request.GetDeferral();
                args.Request.PixelData = await CreateBitmapAsStreamAsync();
                deferral.Complete();
            };
            var ts = new MapTileSource(customSource)
            {
                Layer = MapTileLayer.BackgroundOverlay,
                IsTransparencyEnabled = true,
            };
            map1.TileSources.Add(ts);
        }


        private void CustTileSource_Unchecked(object sender, RoutedEventArgs e)
        {
            map1.TileSources.Clear();
            map1.Style = MapStyle.Road;
        }

        private async System.Threading.Tasks.Task<IRandomAccessStreamReference> CreateBitmapAsStreamAsync()
        {
            const int pixelHeight = 256;
            const int pixelWidth = 256;
            const int bpp = 4;

            var bytes = new byte[pixelHeight * pixelWidth * bpp];

            for (var y = 0; y < pixelHeight; y++)
            {
                for (var x = 0; x < pixelWidth; x++)
                {
                    var pixelIndex = y * pixelWidth + x;
                    var byteIndex = pixelIndex * bpp;

                    if (x % 64 > 16)
                    {
                        bytes[byteIndex] = 0x20;        // Red
                        bytes[byteIndex + 1] = 0x20;    // Green
                        bytes[byteIndex + 2] = 0x80;    // Blue
                        bytes[byteIndex + 3] = 0x00;    // Alpha (0xff = fully opaque)
                    }
                    else
                    {
                        bytes[byteIndex + 3] = 0xff;    // Alpha (0xff = fully opaque)
                    }
                }
            }

            // Create RandomAccessStream from byte array
            var randomAccessStream =
                new InMemoryRandomAccessStream();
            var outputStream = randomAccessStream.GetOutputStreamAt(0);
            var writer = new DataWriter(outputStream);
            writer.WriteBytes(bytes);
            await writer.StoreAsync();
            await writer.FlushAsync();
            return RandomAccessStreamReference.CreateFromStream(randomAccessStream);
        }

        private DependencyObject CreatePin()
        {
            //Creating a Grid element.
            var myGrid = new Grid();
            myGrid.RowDefinitions.Add(new RowDefinition());
            myGrid.RowDefinitions.Add(new RowDefinition());
            myGrid.Background = new SolidColorBrush(Colors.Transparent);

            //Creating a Rectangle
            var myRectangle = new Rectangle { Fill = new SolidColorBrush(Colors.Black), Height = 20, Width = 20 };
            myRectangle.SetValue(Grid.RowProperty, 0);
            myRectangle.SetValue(Grid.ColumnProperty, 0);

            //Adding the Rectangle to the Grid
            myGrid.Children.Add(myRectangle);

            //Creating a Polygon
            var myPolygon = new Polygon()
            {
                Points = new PointCollection() { new Point(2, 0), new Point(22, 0), new Point(2, 40) },
                Stroke = new SolidColorBrush(Colors.Black),
                Fill = new SolidColorBrush(Colors.Black)
            };
            myPolygon.SetValue(Grid.RowProperty, 1);
            myPolygon.SetValue(Grid.ColumnProperty, 0);

            //Adding the Polygon to the Grid
            myGrid.Children.Add(myPolygon);
            return myGrid;
        }
    }
}
