using Autofac;
using Calculo_Biorritmo.ApplicationLayer.UseCases.Accident;
using Calculo_Biorritmo.Data;
using Calculo_Biorritmo.Models;
using Calculo_Biorritmo.Utils.Data;
using Calculo_Biorritmo.ViewModel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Calculo_Biorritmo.Utils.Validators;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Calculo_Biorritmo.ApplicationLayer.Constants;

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
            //registerall();
        }

        //private async void registerall()
        //{
        //    var accidents = new List<accident>();

        //    using (var ctx = new EmployeeEntity())
        //        accidents = ctx.accidents.ToList();

        //    foreach (var item in accidents)
        //    {
        //        var birthDate = DataCalc.getBirthDate(item.curp);
        //        int dias = DataCalc.daysLived(birthDate, item.fecha_accidente);
        //        var biorritmoFisico = CalcularBiorritmo(dias, BiorytmDays.biorritmo_fisico);
        //        var biorritmoEmocional = CalcularBiorritmo(dias, BiorytmDays.biorritmo_emocional);
        //        var biorritmoIntelectual = CalcularBiorritmo(dias, BiorytmDays.biorritmo_intelectual);
        //        var biorritmoIntuicional = CalcularBiorritmo(dias, BiorytmDays.biorritmo_intuicional);

        //        using (var ctx = new EmployeeEntity())
        //        {
        //            var some = ctx.accidents.Where(x => x.curp == item.curp && x.fecha_accidente == item.fecha_accidente).First();
        //            some.residuo_fisico = biorritmoFisico;
        //            some.residuo_emocional = biorritmoEmocional;
        //            some.residuo_intelectual = biorritmoIntelectual;
        //            some.residuo_intuicional = biorritmoIntuicional;
        //            ctx.SaveChanges();
        //        }

        //    }


        //}

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

            DateTime fecha_nacimiento = DataCalc.getBirthDate(vm.curp);
            int dias = DataCalc.daysLived(fecha_nacimiento);
            var biorritmoFisico = CalcularBiorritmo(dias, 23);
            var biorritmoEmocional = CalcularBiorritmo(dias, 28);
            var biorritmoIntelectual = CalcularBiorritmo(dias, 33);
            var biorritmoIntuicional = CalcularBiorritmo(dias, 38);

            try
            {
                var createCommand = new RegisterAccidentCommand(vm.curp, vm.fecha_accidente, biorritmoFisico, biorritmoEmocional, biorritmoIntelectual, biorritmoIntuicional);
                await _mediator.Send(createCommand);
                MessageBox.Show("Accidente registrado con exito");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ha ocurrido un error al registrar el accidente"+ex.Message);
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
