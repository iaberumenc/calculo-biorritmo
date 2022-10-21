using Autofac;
using MediatR;
using Calculo_Biorritmo.ApplicationLayer.Queries.Employees.Data;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Calculo_Biorritmo.Screens.Calculate.BiorytmResults;
using Calculo_Biorritmo.ViewModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using Calculo_Biorritmo.Utils.Data;
using Calculo_Biorritmo.Screens.Generic;
using Calculo_Biorritmo.ApplicationLayer.Constants;

namespace Calculo_Biorritmo.Screens.Employees
{
    /// <summary>
    /// Lógica de interacción para EmployeesView.xaml
    /// </summary>
    public partial class EmployeesView : UserControl
    {
        public Action<UserControl> _userControl;
        private IMediator _mediator;
        public EmployeesView(Action<UserControl> action)
        {
            InitializeComponent();
            _userControl = action;
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
                var genericErrorMessage = new GenericMessage("Ha ocurrido un error");
                genericErrorMessage.ShowDialog();
            }
        }

        private void btnAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            var addNewEmployee = new addEmployee(null);
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

        private void btnCheckGraph_Click(object sender, RoutedEventArgs e)
        {
            var employee = (employeeGridItem)empleado.SelectedItem;
            if(employee == null)
            {
                var genericMessage = new GenericMessage("Seleccione un registro");
                genericMessage.ShowDialog();
                return;
            }
            var livingDaysFirstMoth = DataCalc.daysLived(employee.fecha_nacimiento, DataCalc.getFirstDayMonth());
            _userControl(new EmployeeBiorytm(_userControl, ViewEnum.EmployeesViewEnum , employee.dias_vividos.ToString(), livingDaysFirstMoth, DateTime.Now));
        }
    }
}
