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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
using Calculo_Biorritmo.Data;
using MediatR;
using Autofac;
using Calculo_Biorritmo.ApplicationLayer.Queries.Employees.Data;
using Calculo_Biorritmo.Api;

namespace Calculo_Biorritmo.Screens.Employees
{
    /// <summary>
    /// Lógica de interacción para EmployeesView.xaml
    /// </summary>
    public partial class EmployeesView : UserControl
    {
        private IMediator _mediator;
        public EmployeesView()
        {
            InitializeComponent();
            init();
            
        }

        public async void init()
        {
            _mediator = DIContainer.container.Resolve<IMediator>();
            await updateTable();
        }

        private async Task updateTable()
        {
            try
            {
                var response = await _mediator.Send(new GetEmployeeDataGridCommand()
                {
                    curp = tbBuscar.Text,
                    
                });

                empleado.ItemsSource = response.data;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha ocurrido un error");
            }
        }

        private void btnAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            var addNewEmployee = new addEmployee();
            addNewEmployee.ShowDialog();
            init();
        }

        private void empleado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            await updateTable();
        }
    }
}
