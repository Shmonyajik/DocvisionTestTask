using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LettersRegistration.Client
{
    /// <summary>
    /// Логика взаимодействия для SuccessWindow.xaml
    /// </summary>
    public partial class SuccessWindow : Window
    {
        public SuccessWindow()
        {
            InitializeComponent();
        }
        public SuccessWindow(string message)
        {
            InitializeComponent();
            SuccessTextBlock.Text = message;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }
    }
}
