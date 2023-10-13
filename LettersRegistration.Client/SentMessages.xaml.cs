using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Text.Json;
using static LettersRegistration.Client.MessageWindow;

namespace LettersRegistration.Client
{
    /// <summary>
    /// Логика взаимодействия для SentMessages.xaml
    /// </summary>
    public partial class SentMessages : Window
    {
        public SentMessages()
        {
            InitializeComponent();

            Loaded += SentMessagesWindow_Loaded;
        }


        private async void SentMessagesWindow_Loaded(object sender, RoutedEventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5135/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync($"letters/{App.Sender}");
                string content = await response.Content.ReadAsStringAsync();
                string fullErrorMessage = "Произошла ошибка.";
                try
                {
                    
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                        {
                            var successWindow = new SuccessWindow("Вы еще не отправляли писем");
                            successWindow.ShowDialog();
                            return;
                        }
                        var messages = JsonSerializer.Deserialize<List<Message>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        SentMessagesListView.ItemsSource = messages;
                        return;

                    }
                    else
                    {
                        
                        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(content);

                        if (errorResponse != null)
                        {
                            StringBuilder errorMessage = new StringBuilder(errorResponse.title);
                            errorMessage.AppendLine($"Status: {errorResponse.status}");
                            if (errorResponse.errors != null)
                            {
                                errorMessage.AppendLine("Validation Errors:");
                                foreach (var validationError in errorResponse.errors)
                                {

                                    errorMessage.AppendLine($"{validationError.Key}: {validationError.Value[0]}");
                                }
                            }
                            if (errorResponse.detail != null)
                            {
                                errorMessage.AppendLine($"Internal Server Error: {errorResponse.detail}");
                            }

                            fullErrorMessage = errorMessage.ToString();
                        }
                    }
                    
                    
                }
                catch (JsonException)
                {
                    fullErrorMessage = "BadRequest";
                }
                catch (Exception ex)
                {
                    fullErrorMessage = $"Internal Server Error: {ex.Message}";
                }

                var errorWindow = new ErrorWindow(fullErrorMessage);
                errorWindow.ShowDialog();

            }

            // Обновление интерфейса на основе загруженных данных
            //UpdateUI(data);
        }


        //private void UpdateUI(DataModel data)
        //{
        //    // Обновите интерфейс на основе загруженных данных
        //}


       
    }
}
