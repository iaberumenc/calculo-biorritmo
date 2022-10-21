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

namespace Calculo_Biorritmo.Screens.Generic
{
    /// <summary>
    /// Lógica de interacción para GenericMessage.xaml
    /// </summary>
    public partial class GenericMessage : Window
    {
        public GenericMessage(string Message)
        {
            InitializeComponent();
            InfoText.Text = Message;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
