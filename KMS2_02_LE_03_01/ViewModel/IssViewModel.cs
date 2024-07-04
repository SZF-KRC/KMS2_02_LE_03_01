using KMS2_02_LE_03_01.Manager.ApiManager;
using KMS2_02_LE_03_01.Model.IssModel;
using KMS2_02_LE_03_01.MVVM;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace KMS2_02_LE_03_01.ViewModel
{
    /// <summary>
    /// ViewModel for tracking the International Space Station (ISS) position and updating its location on the map.
    /// </summary>
    public class IssViewModel : ViewModelBase, IDisposable
    {
        private Point _issPosition;
        private readonly DispatcherTimer _timer;

        /// <summary>
        /// Gets or sets the position of the ISS on the map.
        /// </summary>
        public Point IssPosition
        {
            get => _issPosition;
            set
            {
                _issPosition = value;
                OnPropertyChanged(nameof(IssPosition));
            }
        }

        /// <summary>
        /// Initializes a new instance of the IssViewModel class and starts the timer for updating the ISS position.
        /// </summary>
        public IssViewModel()
        {
            FirstUpdate();
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(20)
            };
            _timer.Tick += async (s, args) => await UpdateIssPosition();
            _timer.Start();

            Application.Current.Exit += OnApplicationExit;
        }

        /// <summary>
        /// Handles the application exit event to clean up resources.
        /// </summary>
        private void OnApplicationExit(object sender, ExitEventArgs e)
        {
            Dispose();
        }

        /// <summary>
        /// Asynchronously updates the ISS position by fetching data from the API.
        /// </summary>
        private async Task UpdateIssPosition()
        {
            string url = "http://api.open-notify.org/iss-now.json";
            var data = await ApiClient.GetDataFromApi<IssPositionResponse>(url);
            if (data.iss_position != null)
            {
                //MessageBox.Show($"{data.iss_position.longitude} : {data.iss_position.latitude}");
                var position = ConvertToRealCoordinates(data.iss_position.longitude, data.iss_position.latitude);
                IssPosition = position;
            }
        }

        /// <summary>
        /// Performs the first update of the ISS position immediately after initialization.
        /// </summary>
        private async void FirstUpdate()
        {
           await UpdateIssPosition();
        }

        /// <summary>
        /// Converts the geographical coordinates (longitude, latitude) to a point on the map.
        /// </summary>
        /// <param name="longitude">The longitude of the ISS position.</param>
        /// <param name="latitude">The latitude of the ISS position.</param>
        /// <returns>A point representing the position of the ISS on the map.</returns>
        private Point ConvertToRealCoordinates(double longitude, double latitude)
        {
            // Definovanie hraničných súradníc pre mapISS.png
            double topLeftLat = 90.0;
            double topLeftLong = -180.0;
            double bottomRightLat = -90.0;
            double bottomRightLong = 180.0;

       
            double contentWidth = 300;
            double contentHeight = 20; 

            // Lineárna interpolácia pre Longitude (západ-východ)
            double x = (longitude - topLeftLong) / (bottomRightLong - topLeftLong) * contentWidth;

            // Lineárna interpolácia pre Latitude (sever-juh)
            double y = (topLeftLat - latitude) / (topLeftLat - bottomRightLat) * contentHeight;

            return new Point(x, y);
        }

        /// <summary>
        /// Disposes the resources used by the ViewModel.
        /// </summary>
        public void Dispose()
        {
            _timer.Stop();
            _timer.Tick -= async (s, args) => await UpdateIssPosition();
            Application.Current.Exit -= OnApplicationExit;
        }
    }
}
