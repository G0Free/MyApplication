using MyApplication.Model;
using Newtonsoft.Json;

namespace MyApplication
{
    public partial class MainPage : ContentPage
    {
        string temp;
        public MainPage()
        {
            InitializeComponent();
        }

        private void RefreshClicked(object sender, EventArgs e)
        {
            temp = "tmp";

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

            WeatherInfo currentWeather;
            //Convert from json to object
            if (weatherInfo != "")
            {
                currentWeather = JsonConvert.DeserializeObject<WeatherInfo>(weatherInfo);
            }
            else
            {
                currentWeather = new WeatherInfo();
                currentWeather.CurrentWeather.Temperature = 0.0;                    
            }
            temp = currentWeather.CurrentWeather.Temperature.ToString();

            TemperatureValue.Text = temp;
            SemanticScreenReader.Announce(TemperatureValue.Text);
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
                    ;
                    // Send an HTTP GET request to the URL and get the response
                    HttpResponseMessage response = await client.GetAsync(url);
                    //TODO: Solve this stuck
                    // Check if the request was successful (status code 200)
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the JSON content as a string
                        string jsonContent = await response.Content.ReadAsStringAsync();

                        // Now, you can work with the downloaded JSON data (e.g., parse it or save it to a file)
                        return jsonContent;
                    }
                    else
                    {
                        throw new HttpRequestException(response.StatusCode.ToString());
                        //Console.WriteLine($"HTTP Error: {response.StatusCode}");
                        return null;
                    }
                    return null;
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"HTTP Request Error: {e.Message}");
                }
                return null;
            }
        }
    }
}