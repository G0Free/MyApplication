using MyApplication.Model;
using Newtonsoft.Json;

namespace MyApplication
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            RefreshClicked(null,null);
        }

        private void RefreshClicked(object sender, EventArgs e)
        {
            //Download weatherinfo here          
            string weatherInfo = "";
            Task<string> weatherInfTask = GetWeatherInfoAsnyc();
            weatherInfTask.Wait();

            if (weatherInfTask.Status == TaskStatus.RanToCompletion)
            {
                weatherInfo = weatherInfTask.Result;
            }
            else if (weatherInfTask.IsFaulted)
            {                
                weatherInfo = "";
            }
            
            var currentWeather = JsonConvert.DeserializeObject<WeatherInfo>(weatherInfo);

            TemperatureValue.Text = currentWeather.CurrentWeather.Temperature.ToString() + " °C";
            TimeValue.Text = currentWeather.CurrentWeather.Time.ToString();
            WindSpeedValue.Text = currentWeather.CurrentWeather.Windspeed.ToString() + " km/h";
            WindDirectionValue.Text = currentWeather.CurrentWeather.Winddirection.ToString() + "°";

            if (currentWeather.CurrentWeather.Weathercode == 0)
            {
                Image.Source = "sunny.svg";
                WeatherText.Text = "Sunny";
            }
            else if (0 < currentWeather.CurrentWeather.Weathercode && currentWeather.CurrentWeather.Weathercode < 4)
            {
                Image.Source = "cloudy.svg";
                WeatherText.Text = "Cloudy";
            }
            else if (47 < currentWeather.CurrentWeather.Weathercode && currentWeather.CurrentWeather.Weathercode < 70)
            {
                Image.Source = "rain.svg";
                WeatherText.Text = "Rain";
            }
            else
            {
                Image.Source = "skull.svg";
                WeatherText.Text = "RIP";
            }




            Arrow.Rotation = (double)currentWeather.CurrentWeather.Winddirection;

            //SemanticScreenReader.Announce(TemperatureValue.Text);
            //SemanticScreenReader.Announce(TimeValue.Text);
            //SemanticScreenReader.Announce(WindSpeedValue.Text);
            //SemanticScreenReader.Announce(WindDirectionValue.Text);
            //SemanticScreenReader.Announce(Arrow.Rotation.ToString());
           
            
        }

        private static async Task<string> GetWeatherInfoAsnyc()
        {
            // Create an instance of HttpClient
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Specify the URL of the JSON file you want to download
                    string url = "https://api.open-meteo.com/v1/forecast?latitude=47.4984&longitude=19.0404&current_weather=true&timezone=Europe%2FBerlin";
                    
                    // Send an HTTP GET request to the URL and get the response
                    var response = client.GetAsync(url).Result;

                    // Check if the request was successful (status code 200)
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the JSON content as a string
                        string jsonContent = await response.Content.ReadAsStringAsync();

                        return jsonContent;
                    }
                    else
                    {
                        throw new HttpRequestException(response.StatusCode.ToString());                      
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"HTTP Request Error: {e.Message}");
                }
                return null;
            }
        }

        private void SettingClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}