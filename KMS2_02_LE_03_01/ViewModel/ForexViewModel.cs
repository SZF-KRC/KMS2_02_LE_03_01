using KMS2_02_LE_03_01.Manager.ApiManager;
using KMS2_02_LE_03_01.Model;
using KMS2_02_LE_03_01.MVVM;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace KMS2_02_LE_03_01.ViewModel
{
    /// <summary>
    /// ViewModel for handling and displaying Forex data.
    /// </summary>
    public class ForexViewModel : ViewModelBase
    {
        private readonly string url = "https://api.freecurrencyapi.com/here_your_API";


        private ObservableCollection<string> currencyPairs;

        /// <summary>
        /// Gets or sets the collection of currency pairs.
        /// </summary>
        public ObservableCollection<string> CurrencyPairs
        {
            get { return currencyPairs; }
            set { currencyPairs = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Initializes a new instance of the ForexViewModel class.
        /// </summary>
        public ForexViewModel()
        {
            LoadData();
        }

        /// <summary>
        /// Loads Forex data asynchronously from the API.
        /// </summary>
        public async void LoadData()
        {
            var data = await ApiClient.GetDataFromApi<Forex>(url);
            if (data != null && data.Data != null)
            {
                var randomPairs = data.Data.Select(kvp => $"{kvp.Key}: {kvp.Value.ToString("F3", CultureInfo.InvariantCulture)}").OrderBy(x => Guid.NewGuid()).ToList();
                CurrencyPairs = new ObservableCollection<string>(randomPairs);
            }
        }
    }
}
