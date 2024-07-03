using KMS2_02_LE_03_01.Manager.ApiManager;
using KMS2_02_LE_03_01.Model.IssModel;
using KMS2_02_LE_03_01.MVVM;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace KMS2_02_LE_03_01.ViewModel
{
    public class IssViewModel : ViewModelBase, IDisposable
    {
        private Point _issPosition;
        private readonly DispatcherTimer _timer;

        public Point IssPosition
        {
            get => _issPosition;
            set
            {
                _issPosition = value;
                OnPropertyChanged(nameof(IssPosition));
            }
        }

        public IssViewModel()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(20)
            };
            _timer.Tick += async (s, args) => await UpdateIssPosition();
            _timer.Start();

            Application.Current.Exit += OnApplicationExit;
        }

        private void OnApplicationExit(object sender, ExitEventArgs e)
        {
            Dispose();
        }

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

        private Point ConvertToRealCoordinates(double longitude, double latitude)
        {
            // Definovanie hraničných súradníc pre mapISS.png
            double topLeftLat = 90.0;
            double topLeftLong = -180.0;
            double bottomRightLat = -90.0;
            double bottomRightLong = 180.0;

            // Veľkosť ContentControl (rozmer v pixeloch)
            double contentWidth = 300; // predpokladaná šírka po odpočítaní marginov
            double contentHeight = 20; // predpokladaná výška po odpočítaní marginov

            // Lineárna interpolácia pre Longitude (západ-východ)
            double x = (longitude - topLeftLong) / (bottomRightLong - topLeftLong) * contentWidth;

            // Lineárna interpolácia pre Latitude (sever-juh)
            double y = (topLeftLat - latitude) / (topLeftLat - bottomRightLat) * contentHeight;

            return new Point(x, y);
        }

        public void Dispose()
        {
            _timer.Stop();
            _timer.Tick -= async (s, args) => await UpdateIssPosition();
            Application.Current.Exit -= OnApplicationExit;
        }
    }
}
