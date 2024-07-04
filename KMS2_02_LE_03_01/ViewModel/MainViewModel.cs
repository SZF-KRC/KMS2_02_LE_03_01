using KMS2_02_LE_03_01.MVVM;
using System.Windows.Input;
namespace KMS2_02_LE_03_01.ViewModel
{
    /// <summary>
    /// The main ViewModel that manages different views and their corresponding view models.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {

        private object _currentFirstViewModel;
        private object _currentSecondViewModel;

        /// <summary>
        /// Gets or sets the current view model for the primary content area.
        /// </summary>
        public object CurrentFirstViewModel
        {
            get => _currentFirstViewModel;
            set
            {
                _currentFirstViewModel = value;
                OnPropertyChanged(nameof(CurrentFirstViewModel));
            }
        }

        /// <summary>
        /// Gets or sets the current view model for the secondary content area.
        /// </summary>
        public object CurrentSecondViewModel
        {
            get => _currentSecondViewModel;
            set
            {
                _currentSecondViewModel = value;
                OnPropertyChanged(nameof(CurrentSecondViewModel));
            }
        }
        public ICommand WallStreetCommand { get; }
        public ICommand WeatherCommand { get; }

        public MainViewModel()
        {
            WallStreetCommand = new CustomRelayCommand(OpenWindowWallStreet);
            WeatherCommand = new CustomRelayCommand(OpenWindowWeather);
            CurrentFirstViewModel = new WelcomeViewModel();
        }

        /// <summary>
        /// Opens the Wall Street view and sets the corresponding view models.
        /// </summary>
        private void OpenWindowWallStreet()
        {
            CurrentFirstViewModel = new WallStreetViewModel();
            CurrentSecondViewModel = new ForexViewModel();
        }

        /// <summary>
        /// Opens the Weather view and sets the corresponding view models.
        /// </summary>
        private void OpenWindowWeather()
        {
            CurrentFirstViewModel = new WeatherViewModel();
            CurrentSecondViewModel = new IssViewModel();
        }
    }
}
