using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using KMS2_02_LE_03_01.Manager.ApiManager;
using KMS2_02_LE_03_01.Model.WeatherModel;
using KMS2_02_LE_03_01.MVVM;

namespace KMS2_02_LE_03_01.ViewModel
{
    public class WeatherViewModel : ViewModelBase
    {
        private double _scale = 1.0;
        private double _offsetX = 0.0;
        private double _offsetY = 0.0;
        private Point _lastMousePosition;
        private bool _isMouseCaptured = false;

        private string _cityName;
        private double _temperature;

        public string CityName
        {
            get => _cityName;
            set
            {
                _cityName = value;
                OnPropertyChanged(nameof(CityName));
            }
        }

        public double Temperature
        {
            get => _temperature;
            set
            {
                _temperature = value;
                OnPropertyChanged(nameof(Temperature));
            }
        }

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

        private async Task GetWeatherData(double longitude, double latitude)
        {
            string apiKey = "4a7fbcd13ef5dbdacea313d5329b1221"; 
            string url = $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&units=metric&appid={apiKey}";

            var weatherData = await ApiClient.GetDataFromApi<WeatherData>(url);
            if (weatherData != null)
            {
                CityName = weatherData.name;
                Temperature = weatherData.main.temp;

                // Zobrazenie MessageBoxu s názvom mesta a teplotou v Celziach
                MessageBox.Show($"City: {CityName}, Temperature: {Temperature:F2}°C");
            }
        }

        public ICommand MouseDownCommand => new RelayCommand<MouseButtonEventArgs>(OnMouseDown);
        public ICommand MouseMoveCommand => new RelayCommand<MouseEventArgs>(OnMouseMove);
        public ICommand MouseWheelCommand => new RelayCommand<MouseWheelEventArgs>(OnMouseWheel);

        private async  void OnMouseDown(MouseButtonEventArgs e)
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
                MessageBox.Show($"OnMouseDown: Longitude: {coords.X}, Latitude: {coords.Y}");

            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                _lastMousePosition = e.GetPosition(sourceElement);
                _isMouseCaptured = true;
                Mouse.Capture(sourceElement);
            }
        }



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

        private Point ConvertToRealCoordinates(Point pixelPoint)
        {
            // Definovanie hraničných súradníc
            double topLeftLat = 71.106327;
            double topLeftLong = -29.508295;
            double bottomRightLat = 31.771676;
            double bottomRightLong = 55.816731;

            // Veľkosť obrázka (rozmer v pixeloch)
            double imageWidth = 900; // skutočná šírka obrázka
            double imageHeight = 500; // skutočná výška obrázka

            // Prepočet súradníc z hľadiska rozloženia zobrazenia
            double adjustedX = pixelPoint.X * (2500 / imageWidth);
            double adjustedY = pixelPoint.Y * (1770 / imageHeight);

            // Lineárna interpolácia pre Longitude (západ-východ)
            double longitude = topLeftLong + (bottomRightLong - topLeftLong) * (adjustedX / 2500);

            // Lineárna interpolácia pre Latitude (sever-juh)
            double latitude = topLeftLat + (bottomRightLat - topLeftLat) * (adjustedY / 1770);

            // Korekcia Longitude na základe zistených odchýlok
            longitude += 13.195;  // upravenie na základe zistených odchýlok

            // Korekcia Latitude na základe zistených odchýlok
            latitude -= -1.355;  // upravenie na základe zistených odchýlok

            return new Point(longitude, latitude);
        }





    }
}
