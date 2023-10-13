
using System.Windows;


namespace LettersRegistration.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(App.Sender))
            {
                SenderNameInput.Text = App.Sender;
            }
        }

        private void SaveNameButton_Click(object sender, RoutedEventArgs e)
        {
            App.Sender = SenderNameInput.Text;
        }

        private void SendMessageMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var messageWindow = new MessageWindow();
            messageWindow.ShowDialog();
        }
        private void ViewSentMessagesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(App.Sender))
            {
                var errorWindow = new ErrorWindow("Пожалуйста введите имя пользователя для отображения писем");
                errorWindow.ShowDialog();
                return;
            }
            var sentMessagesWindow = new SentMessages();
            sentMessagesWindow.ShowDialog();
        }
        

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

       
       
    }
}
