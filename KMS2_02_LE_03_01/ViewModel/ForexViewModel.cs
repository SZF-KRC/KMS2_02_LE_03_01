using KMS2_02_LE_03_01.Manager.ApiManager;
using KMS2_02_LE_03_01.Model;
using KMS2_02_LE_03_01.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace KMS2_02_LE_03_01.ViewModel
{
    public class ForexViewModel : ViewModelBase
    {
        private readonly string url = "https://api.freecurrencyapi.com/v1/latest?apikey=fca_live_v6WZltHUZdfLgvNCaZGEF8yD3AIriHLZyvI2WL4y&currencies=CZK,USD,CAD,GBP,JPY,AUD,NZD,CHF,NOK,SEK,DKK,HKD,SGD,MXN,ZAR,INR,BRL&base_currency=EUR";


        private ObservableCollection<string> currencyPairs;
        public ObservableCollection<string> CurrencyPairs
        {
            get { return currencyPairs; }
            set { currencyPairs = value; OnPropertyChanged(); }
        }

        public ForexViewModel()
        {
            LoadData();
        }

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
