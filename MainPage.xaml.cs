using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using lifeManagementApp.ViewModels;

namespace lifeManagementApp
{
    public partial class MainPage : ContentPage
    {
        private const string API_KEY = "72d3ff66e001449ad5dfb77aed8ca7b7";
        private const string CITY_URL = "http://api.openweathermap.org/geo/1.0/direct";
        private const string WEATHER_URL = "https://api.openweathermap.org/data/2.5/weather";

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();

        }

        // Weather API logic remains the same
        private async void OnGetWeatherClicked(object sender, EventArgs e)
        {
            string city = CityEntry.Text;

            if (string.IsNullOrEmpty(city))
            {
                await DisplayAlert("Error", "Please enter a city name", "OK");
                return;
            }

            try
            {
                string city_url = $"{CITY_URL}?q={city}&limit=1&appid={API_KEY}";
                HttpClient client = new HttpClient();
                string response_city = await client.GetStringAsync(city_url);

                var cityData = JArray.Parse(response_city);
                if (cityData.Count == 0)
                {
                    await DisplayAlert("Error", "City not found", "OK");
                    return;
                }

                string lat = cityData[0]["lat"].ToString();
                string lon = cityData[0]["lon"].ToString();

                string weather_url = $"{WEATHER_URL}?lat={lat}&lon={lon}&appid={API_KEY}&units=metric";
                string response_weather = await client.GetStringAsync(weather_url);

                var weatherData = JObject.Parse(response_weather);

                if (weatherData["main"] == null)
                {
                    await DisplayAlert("Error", "Weather data not found", "OK");
                    return;
                }

                string temperature = weatherData["main"]["temp"].ToString();
                string min_temperature = weatherData["main"]["temp_min"].ToString();
                string max_temperature = weatherData["main"]["temp_max"].ToString();
                string main_weather = weatherData["weather"][0]["main"].ToString();
                string wind = weatherData["wind"]["speed"].ToString();

                TemperatureLabel.Text = $"Temperature: {temperature}°C\nMin temperature: {min_temperature}°C\nMax temperature: {max_temperature}°C\nGeneral weather: {main_weather}\nWind speed: {wind}";

                if (float.Parse(wind) > 4)
                {
                    await DisplayAlert("Warning", "Wind Alert!!!", "OK");
                }
            }
            catch (HttpRequestException httpEx)
            {
                await DisplayAlert("Error", $"Network error: {httpEx.Message}", "OK");
            }
            catch (JsonReaderException jsonEx)
            {
                await DisplayAlert("Error", $"Data format error: {jsonEx.Message}", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        
    }
}
