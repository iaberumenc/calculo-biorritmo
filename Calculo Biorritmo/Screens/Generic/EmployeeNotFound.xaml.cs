using Calculo_Biorritmo.Screens.Calculate;
using Calculo_Biorritmo.Screens.Employees;
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
    /// Lógica de interacción para EmployeeNotFound.xaml
    /// </summary>
    public partial class EmployeeNotFound : Window
    {
        private string _curp;
        public EmployeeNotFound(string curp)
        {
            InitializeComponent();
            _curp = curp;
        }

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).DialogResult = true;
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            var addNewEmployee = new addEmployee(_curp);
            Window.GetWindow(this).DialogResult = false;
            Close();
            addNewEmployee.ShowDialog();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).DialogResult = false;
            Close();
        }
    }
}
