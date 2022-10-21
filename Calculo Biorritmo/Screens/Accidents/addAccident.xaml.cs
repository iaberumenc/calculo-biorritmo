using Autofac;
using Calculo_Biorritmo.ApplicationLayer.UseCases.Accident;
using Calculo_Biorritmo.Data;
using Calculo_Biorritmo.Models;
using Calculo_Biorritmo.Utils.Data;
using Calculo_Biorritmo.ViewModel;
using MediatR;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Calculo_Biorritmo.Utils.Validators;
using Calculo_Biorritmo.Screens.Generic;

namespace Calculo_Biorritmo.Screens.Accidents
{
    /// <summary>
    /// Lógica de interacción para addAccident.xaml
    /// </summary>
    public partial class addAccident : Window
    {
        AccidentsVM vm = new AccidentsVM();
        private IMediator _mediator;
        public addAccident()
        {
            InitializeComponent();
            init();
        }

        public void init()
        {
            gridForm.DataContext = vm;
            vm.fecha_accidente = DateTime.Now;
            _mediator = DIContainer.container.Resolve<IMediator>();
        }


        private async void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            bool errors = false;
            if (string.IsNullOrEmpty(vm.curp))
            {
                lblErrorCurp.Content = "El CURP no puede ser vacio";
                lblErrorCurp.Visibility = Visibility.Visible;
                errors = true;
            }

            if (vm.curp.Length != 18)
            {
                lblErrorCurp.Content = "El CURP debe ser a 18 digitos";
                lblErrorCurp.Visibility = Visibility.Visible;
                errors = true;
            }

            if (tbFechaAccidente.SelectedDate == null)
            {
                lblErrorDate.Content = "La fecha del accidente no puede ser vacia";
                lblErrorDate.Visibility = Visibility.Visible;
                errors = true;
            }

            if (!InputValidators.validateCURP(vm.curp))
            {
                lblErrorCurp.Content = "Ingresa un CURP valido";
                lblErrorCurp.Visibility = Visibility.Visible;
                errors = true;
            }

            if (errors)
                return;

            employee empleado = new employee();

            using (var ctx = new EmployeeEntity())
                empleado = ctx.employees.Where(x => x.curp == vm.curp).FirstOrDefault();

            if(empleado == null)
            {
                lblErrorCurp.Content = "No hay un empleado registrado con ese CURP";
                lblErrorCurp.Visibility = Visibility.Visible;
                return;
            }

            DateTime fecha_nacimiento = DataCalc.getBirthDateFromCurp(vm.curp);
            int dias = DataCalc.daysLived(fecha_nacimiento);
            var biorritmoFisico = CalcularBiorritmo(dias, 23);
            var biorritmoEmocional = CalcularBiorritmo(dias, 28);
            var biorritmoIntelectual = CalcularBiorritmo(dias, 33);
            var biorritmoIntuicional = CalcularBiorritmo(dias, 38);

            try
            {
                var createCommand = new RegisterAccidentCommand(vm.curp, vm.fecha_accidente, biorritmoFisico, biorritmoEmocional, biorritmoIntelectual, biorritmoIntuicional);
                await _mediator.Send(createCommand);
                var genericMessage = new GenericMessage("Accidente registrado con exito");
                genericMessage.ShowDialog();
            }
            catch(Exception ex)
            {
                var genericMessage = new GenericMessage("Ha ocurrido un error al registrar el accidente" + ex.Message);
                genericMessage.ShowDialog();
                return;
            }
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
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

        private void tbFechaAccidente_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lblErrorDate.Visibility == Visibility.Visible)
                lblErrorDate.Visibility = Visibility.Hidden;
        }
    }
}
