using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace Erstes_Projekt
{
    public partial class MainWindow : Window
    {
        private const string apiKey = "0878a04b922d20431d7ad5242467b279"; // Dein API-Schlüssel
        private const string apiUrl = "https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=metric";

        public MainWindow()
        {
            InitializeComponent();
            SetPlaceholderText();
        }

        private async void GetWeatherButton_Click(object sender, RoutedEventArgs e)
        {
            string city = cityTextBox.Text;
            if (!string.IsNullOrEmpty(city) && city != "Stadt eingeben...")
            {
                var weather = await GetWeatherDataAsync(city);
                if (weather != null)
                {
                    DisplayTemperature(weather);
                }
                else
                {
                    MessageBox.Show("Wetterdaten konnten nicht abgerufen werden.");
                }
            }
            else
            {
                MessageBox.Show("Bitte eine Stadt eingeben.");
            }
        }

        private async Task<WeatherData> GetWeatherDataAsync(string city)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = string.Format(apiUrl, city, apiKey);
                var response = await client.GetStringAsync(url);
                var weather = JsonConvert.DeserializeObject<WeatherData>(response);
                return weather;
            }
        }

        private void DisplayTemperature(WeatherData weather)
        {
            // Temperatur ohne Dezimalstellen anzeigen, mit Math.Floor auf die ganze Zahl abrunden
            int temp = (int)Math.Floor(weather.Main.Temp);
            temperatureText.Text = $"{temp}°C";
        }

        private void SetPlaceholderText()
        {
            // Standardmäßig den Platzhaltertext setzen
            cityTextBox.Text = "Stadt eingeben...";
            cityTextBox.Foreground = System.Windows.Media.Brushes.Gray;
        }

        private void CityTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            // Wenn das Textfeld den Fokus erhält, Platzhaltertext löschen
            if (cityTextBox.Text == "Stadt eingeben...")
            {
                cityTextBox.Text = "";
                cityTextBox.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void CityTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // Wenn das Textfeld den Fokus verliert und leer ist, Platzhaltertext wieder anzeigen
            if (string.IsNullOrWhiteSpace(cityTextBox.Text))
            {
                SetPlaceholderText();
            }
        }
    }

    // Modell für die Wetterdaten
    public class WeatherData
    {
        public MainInfo Main { get; set; }
    }

    public class MainInfo
    {
        public double Temp { get; set; }
    }
}
