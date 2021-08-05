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
using Calculo_Biorritmo.Connection;
using System.Configuration;
using MediatR;
using Calculo_Biorritmo.ApplicationLayer.UseCases.Employee.CreateEmployee;
using Calculo_Biorritmo.ViewModel;
using Calculo_Biorritmo.Utils.Data;
using Autofac;
using Calculo_Biorritmo.ApplicationLayer.UseCases.Accident;
using Calculo_Biorritmo.Utils.Validators;
using Calculo_Biorritmo.ApplicationLayer.Constants;
using Calculo_Biorritmo.Data;

namespace Calculo_Biorritmo.Screens.Employees
{
    /// <summary>
    /// Lógica de interacción para addEmployee.xaml
    /// </summary>
    public partial class addEmployee : Window
    {
        EmployeesVM vm = new EmployeesVM();
        private IMediator _mediator;
        public addEmployee()
        {
            InitializeComponent();
            init();
        }

        public void init()
        {
            gridForm.DataContext = vm;
            _mediator = DIContainer.container.Resolve<IMediator>();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(vm.curp))
            {
                lblErrorCurp.Content = "El CURP no puede ser vacio";
                lblErrorCurp.Visibility = Visibility.Visible;
                return;
            }

            if (vm.curp.Length != 18)
            {
                lblErrorCurp.Content = "El CURP debe ser a 18 digitos";
                lblErrorCurp.Visibility = Visibility.Visible;
                return;
            }

            if (!InputValidators.validateCURP(vm.curp))
            {
                lblErrorCurp.Content = "Ingresa un CURP valido";
                lblErrorCurp.Visibility = Visibility.Visible;
                return;
            }

            using (var ctx = new EmployeeEntity())
            {
                var result = ctx.employees.Where(x => x.curp == vm.curp).Select(x => x.curp).FirstOrDefault();
                if (result != null)
                {
                    lblErrorCurp.Content = "Ya existe un empleado registrado con ese CURP";
                    lblErrorCurp.Visibility = Visibility.Visible;
                    return;
                }
            }

            vm.fecha_nacimiento = DataCalc.getBirthDate(vm.curp);
            DateTime fecha_nacimiento = DataCalc.getBirthDate(vm.curp);
            int livedDays = DataCalc.daysLived(fecha_nacimiento);

            if(livedDays < 0)
            {
                lblErrorCurp.Content = "Error: La edad maxima 100";
                lblErrorCurp.Visibility = Visibility.Visible;
                return;
            }

            var createCommand = new CreateEmployeeCommand(vm.curp, vm.fecha_nacimiento, tbFechaAccidente.SelectedDate);

            try
            {
                await _mediator.Send(createCommand);
            }
            catch(Exception)
            {
                MessageBox.Show("Ha ocurrido un error al registrar al empleado");
                return;
            }

            if(tbFechaAccidente.SelectedDate != null)
            {
                vm.fecha_accidente = tbFechaAccidente.SelectedDate ?? DateTime.Now;
                var biorritmoFisico = CalcularBiorritmo(livedDays, BiorytmDays.biorritmo_fisico);
                var biorritmoEmocional = CalcularBiorritmo(livedDays, BiorytmDays.biorritmo_emocional);
                var biorritmoIntelectual = CalcularBiorritmo(livedDays, BiorytmDays.biorritmo_intelectual);
                var biorritmoIntuicional = CalcularBiorritmo(livedDays, BiorytmDays.biorritmo_intuicional);

                var registerAccident = new RegisterAccidentCommand(vm.curp, vm.fecha_accidente, biorritmoFisico, biorritmoEmocional, biorritmoIntelectual, biorritmoIntuicional);
                await _mediator.Send(registerAccident);
            }
            var response = $"{vm.curp} registrado con exito";
            MessageBox.Show(response);
            Close();
        }

        private void tbNoReloj_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void tbRfc_KeyDown(object sender, KeyEventArgs e)
        {

        }

        public Double CalcularBiorritmo(int diasVividos, int teoria)
        {
            var dayValue = (2 * Math.PI * (diasVividos)) / teoria;
            var sinValue = Math.Sin(dayValue);
            var roundedValue = Math.Round(sinValue, 9, MidpointRounding.ToEven);
            return roundedValue;
        }

        private void tbId_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (lblErrorCurp.Visibility == Visibility.Visible)
                lblErrorCurp.Visibility = Visibility.Hidden;
        }
    }
}
