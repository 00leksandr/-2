using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PR17
{
    public partial class Form1 : Form
    {
        private readonly HttpClient _httpClient;
        private System.Windows.Forms.Timer _timer;

        public Form1()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 3000;
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private async void Timer_Tick(object sender, EventArgs e)
        {
            string imageUrl = await GetRandomDogImageUrl();
            pictureBox1.LoadAsync(imageUrl);
        }

        private async Task<string> GetRandomDogImageUrl()
        {
            string apiUrl = "https://dog.ceo/api/breeds/image/random";
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();

            string jsonResponse = await response.Content.ReadAsStringAsync();
            DogImageResponse dogImageResponse = JsonConvert.DeserializeObject<DogImageResponse>(jsonResponse);

            return dogImageResponse.Message;
        }

        private class DogImageResponse
        {
            public string Message { get; set; }
            public string Status { get; set; }
        }
    }
}