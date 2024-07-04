using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace KMS2_02_LE_03_01.Manager.ApiManager
{
    /// <summary>
    /// Provides methods for making HTTP requests to an API and deserializing the response.
    /// </summary>
    public class ApiClient
    {
        /// <summary>
        /// Sends a GET request to the specified URL and deserializes the response into an object of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize the response into.</typeparam>
        /// <param name="url">The URL of the API endpoint.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the deserialized object of type <typeparamref name="T"/>.</returns>
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

