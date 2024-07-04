using GalaSoft.MvvmLight.Command;
using KMS2_02_LE_03_01.Manager.ApiManager;
using KMS2_02_LE_03_01.Model.WeatherModel;
using KMS2_02_LE_03_01.MVVM;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KMS2_02_LE_03_01.ViewModel
{
    /// <summary>
    /// ViewModel for handling and displaying weather data.
    /// </summary>
    public class WeatherViewModel : ViewModelBase
    {
        private double _scale = 1.0;
        private double _offsetX = 0.0;
        private double _offsetY = 0.0;
        private Point _lastMousePosition;
        private bool _isMouseCaptured = false;

        private string _cityName;
        private double _temperature;

        /// <summary>
        /// Gets or sets the name of the city.
        /// </summary>
        public string CityName
        {
            get => _cityName;
            set
            {
                _cityName = value;
                OnPropertyChanged(nameof(CityName));
            }
        }

        /// <summary>
        /// Gets or sets the temperature in the city.
        /// </summary>
        public double Temperature
        {
            get => _temperature;
            set
            {
                _temperature = value;
                OnPropertyChanged(nameof(Temperature));
            }
        }

        /// <summary>
        /// Gets or sets the scale of the image.
        /// </summary>
        public double Scale
        {
            get => _scale;
            set
            {
                if (_scale != value)
                {
                    _scale = value;
                    OnPropertyChanged(nameof(Scale));
                }
            }
        }

        /// <summary>
        /// Gets or sets the horizontal offset of the image.
        /// </summary>
        public double OffsetX
        {
            get => _offsetX;
            set
            {
                if (_offsetX != value)
                {
                    _offsetX = value;
                    OnPropertyChanged(nameof(OffsetX));
                }
            }
        }

        /// <summary>
        /// Gets or sets the vertical offset of the image.
        /// </summary>
        public double OffsetY
        {
            get => _offsetY;
            set
            {
                if (_offsetY != value)
                {
                    _offsetY = value;
                    OnPropertyChanged(nameof(OffsetY));
                }
            }
        }

        /// <summary>
        /// Asynchronously fetches weather data for the given longitude and latitude.
        /// </summary>
        /// <param name="longitude">The longitude of the location.</param>
        /// <param name="latitude">The latitude of the location.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task GetWeatherData(double longitude, double latitude)
        {
            string apiKey = "your_api_key";
            string url = $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&units=metric&appid={apiKey}";

            var weatherData = await ApiClient.GetDataFromApi<WeatherData>(url);
            if (weatherData != null)
            {
                CityName = weatherData.name;
                Temperature = weatherData.main.temp;

                // Displaying a MessageBox with the city name and temperature in Celsius
                MessageBox.Show($"City: {CityName}, Temperature: {Temperature:F2}°C");
            }
        }

        public ICommand MouseDownCommand => new RelayCommand<MouseButtonEventArgs>(OnMouseDown);
        public ICommand MouseMoveCommand => new RelayCommand<MouseEventArgs>(OnMouseMove);
        public ICommand MouseWheelCommand => new RelayCommand<MouseWheelEventArgs>(OnMouseWheel);

        /// <summary>
        /// Handles mouse down events to fetch weather data or initiate image panning.
        /// </summary>
        /// <param name="e">The mouse button event arguments.</param>
        private async void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e == null)
            {
                MessageBox.Show("OnMouseDown: e is null");
                return;
            }

            var sourceElement = e.Source as IInputElement;
            if (sourceElement == null)
            {
                MessageBox.Show("OnMouseDown: sourceElement is null");
                return;
            }

            if (e.ChangedButton == MouseButton.Left)
            {
                var position = e.GetPosition(sourceElement);
                var coords = ConvertToRealCoordinates(position);
                double coordinatX = Math.Round(coords.X, 2);
                double coordinatY = Math.Round(coords.Y, 2);
                await GetWeatherData(coordinatX, coordinatY);
                // MessageBox.Show($"OnMouseDown: Longitude: {coords.X}, Latitude: {coords.Y}");

            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                _lastMousePosition = e.GetPosition(sourceElement);
                _isMouseCaptured = true;
                Mouse.Capture(sourceElement);
            }
        }

        /// <summary>
        /// Handles mouse move events to pan the image if the right mouse button is pressed.
        /// </summary>
        /// <param name="e">The mouse event arguments.</param>
        private void OnMouseMove(MouseEventArgs e)
        {
            if (e == null)
            {
                MessageBox.Show("OnMouseMove: e is null");
                return;
            }

            var sourceElement = e.Source as IInputElement;
            if (sourceElement == null)
            {
                MessageBox.Show("OnMouseMove: sourceElement is null");
                return;
            }

            if (_isMouseCaptured && e.RightButton == MouseButtonState.Pressed)
            {
                var currentPosition = e.GetPosition(sourceElement);
                double newOffsetX = OffsetX + (currentPosition.X - _lastMousePosition.X);
                double newOffsetY = OffsetY + (currentPosition.Y - _lastMousePosition.Y);

                // Calculate boundaries
                var elementWidth = ((FrameworkElement)sourceElement).ActualWidth;
                var elementHeight = ((FrameworkElement)sourceElement).ActualHeight;
                double scaledImageWidth = elementWidth * Scale;
                double scaledImageHeight = elementHeight * Scale;

                double minOffsetX = -(scaledImageWidth - elementWidth);
                double minOffsetY = -(scaledImageHeight - elementHeight);
                double maxOffsetX = 0;
                double maxOffsetY = 0;

                // Clamp offsets to boundaries
                OffsetX = Math.Max(minOffsetX, Math.Min(newOffsetX, maxOffsetX));
                OffsetY = Math.Max(minOffsetY, Math.Min(newOffsetY, maxOffsetY));

                _lastMousePosition = currentPosition;
            }
            else
            {
                _isMouseCaptured = false;
                Mouse.Capture(null);
            }
        }

        /// <summary>
        /// Handles mouse wheel events to zoom the image in and out.
        /// </summary>
        /// <param name="e">The mouse wheel event arguments.</param>
        private void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (e == null)
            {
                MessageBox.Show("OnMouseWheel: e is null");
                return;
            }

            var sourceElement = e.Source as IInputElement;
            if (sourceElement == null)
            {
                MessageBox.Show("OnMouseWheel: sourceElement is null");
                return;
            }

            var position = e.GetPosition(sourceElement);

            double zoomFactor = e.Delta > 0 ? 1.1 : 1 / 1.1;
            double oldScale = Scale;
            Scale *= zoomFactor;

            Scale = Math.Max(Scale, 1); // Define minimum zoom level

            OffsetX = position.X - (position.X - OffsetX) * (Scale / oldScale);
            OffsetY = position.Y - (position.Y - OffsetY) * (Scale / oldScale);

            // Clamp offsets to boundaries after scaling
            var elementWidth = ((FrameworkElement)sourceElement).ActualWidth;
            var elementHeight = ((FrameworkElement)sourceElement).ActualHeight;
            double scaledImageWidth = elementWidth * Scale;
            double scaledImageHeight = elementHeight * Scale;

            double minOffsetX = -(scaledImageWidth - elementWidth);
            double minOffsetY = -(scaledImageHeight - elementHeight);
            double maxOffsetX = 0;
            double maxOffsetY = 0;

            OffsetX = Math.Max(minOffsetX, Math.Min(OffsetX, maxOffsetX));
            OffsetY = Math.Max(minOffsetY, Math.Min(OffsetY, maxOffsetY));
        }

        /// <summary>
        /// Converts pixel coordinates to geographical coordinates.
        /// </summary>
        /// <param name="pixelPoint">The point in pixel coordinates.</param>
        /// <returns>A point representing the geographical coordinates.</returns>
        private Point ConvertToRealCoordinates(Point pixelPoint)
        {
            // Defining boundary coordinates
            double topLeftLat = 71.106327;
            double topLeftLong = -29.508295;
            double bottomRightLat = 31.771676;
            double bottomRightLong = 55.816731;

            // Image dimensions (in pixels)
            double imageWidth = 900; 
            double imageHeight = 500;

            // Adjust coordinates according to the display layout
            double adjustedX = pixelPoint.X * (2500 / imageWidth);
            double adjustedY = pixelPoint.Y * (1770 / imageHeight);

            // Linear interpolation for Longitude (west-east)
            double longitude = topLeftLong + (bottomRightLong - topLeftLong) * (adjustedX / 2500);

            // Linear interpolation for Latitude (north-south)
            double latitude = topLeftLat + (bottomRightLat - topLeftLat) * (adjustedY / 1770);

            // Longitude correction based on observed deviations
            longitude += 13.195;  // adjust based on observed deviations

            // Latitude correction based on observed deviations
            latitude -= -1.355; // adjust based on observed deviations

            return new Point(longitude, latitude);
        }
    }
}
