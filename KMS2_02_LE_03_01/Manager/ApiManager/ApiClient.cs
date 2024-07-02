using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace KMS2_02_LE_03_01.Manager.ApiManager
{
    public class ApiClient
    {
        public static async Task<T> GetDataFromApi<T>(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("KMS2_02_LE_03_01/1.0");

                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        T result = JsonConvert.DeserializeObject<T>(responseBody);
                        return result;
                    }
                    else
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Error: {response.StatusCode}, {error}");
                    }
                }
            }
            catch (HttpRequestException ex) { MessageBox.Show($"HttpRequestException: {ex.Message}"); }
            catch (Exception ex) { MessageBox.Show($"Exception: {ex.Message}"); }
            return default(T);
        }
    }
}

