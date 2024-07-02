using KMS2_02_LE_03_01.Model;
using KMS2_02_LE_03_01.MVVM;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
namespace KMS2_02_LE_03_01.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        private object _currentFirstViewModel;
        private object _currentSecondViewModel;

       
        public object CurrentFirstViewModel
        {
            get => _currentFirstViewModel;
            set
            {
                _currentFirstViewModel = value;
                OnPropertyChanged(nameof(CurrentFirstViewModel));
            }
        }

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
        }


        private void OpenWindowWallStreet()
        {
            CurrentFirstViewModel = new WallStreetViewModel();
            CurrentSecondViewModel = new ForexViewModel();
        }

        private void OpenWindowWeather()
        {
            CurrentFirstViewModel = new WeatherViewModel();
            CurrentSecondViewModel = new IssViewModel();
        }
    }
}
