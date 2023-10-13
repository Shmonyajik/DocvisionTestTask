using System;

using System.Net.Http.Headers;
using System.Net.Http;

using System.Text;
using System.Threading.Tasks;
using System.Windows;

using System.Net.Http.Json;

using System.Text.Json;

namespace LettersRegistration.Client
{
    /// <summary>
    /// Логика взаимодействия для MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow : Window
    {
        public MessageWindow()
        {
            InitializeComponent();
            if(App.Sender!=null)
            {
                SenderTextBox.Text = App.Sender;
            }
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(SenderTextBox.Text) || string.IsNullOrEmpty(AddresseTextBox.Text))
            {
                var errorWindow = new ErrorWindow("Введите отправителя и получателя");
                errorWindow.ShowDialog();
                return;
                
            }
            var message = new Message
            {
                Name = SubjectTextBox.Text != null ? SubjectTextBox.Text : "",
                RegistrationTime = DateTime.Now,
                Sender = SenderTextBox.Text,
                Addressee = AddresseTextBox.Text,
                Content = BodyTextBox.Text != null ? BodyTextBox.Text : ""

            };
            await SendMessage(message);
        }

        public async Task SendMessage(Message message)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5135/"); 
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.PostAsJsonAsync("letters", message); 

                if (response.IsSuccessStatusCode)
                {
                    var successWindow = new SuccessWindow();
                    successWindow.ShowDialog();
                }
                
                else
                {
                    string fullErrorMessage = "Произошла ошибка.";
                   
                    if (response.Content != null)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        if (!string.IsNullOrEmpty(content))
                        {
                           
                            try
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
                                    if(errorResponse.detail != null)
                                    {
                                        errorMessage.AppendLine($"Internal Server Error: {errorResponse.detail}");
                                    }

                                    fullErrorMessage = errorMessage.ToString(); 
                                }
                            }
                            catch(JsonException)
                            {
                                fullErrorMessage = "BadRequest";
                            }
                            catch (Exception ex )
                            {
                                fullErrorMessage = $"Internal Server Error: {ex.Message}";

                            }
                        }
                    }

                    var errorWindow = new ErrorWindow(fullErrorMessage);
                    errorWindow.ShowDialog();
                }
            }
        }
    }
}
