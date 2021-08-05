using Autofac;
using Calculo_Biorritmo.Algorytms;
using Calculo_Biorritmo.ApplicationLayer.Queries.Employees.Data;
using Calculo_Biorritmo.Data;
using Calculo_Biorritmo.Extensions.ContextExtensions;
using Calculo_Biorritmo.Screens.Calculate.BiorytmResults;
using Calculo_Biorritmo.Utils.Data;
using Calculo_Biorritmo.ViewModel;
using MediatR;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
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

namespace Calculo_Biorritmo.Screens.Calculate
{
    /// <summary>
    /// Lógica de interacción para CalculateView.xaml
    /// </summary>
    public partial class CalculateView : UserControl
    {
        
        
        EmployeesVM vm = new EmployeesVM();
        private IMediator _mediator;
        private int dias;
        private DateTime _fechaNacimiento;
        public Action<UserControl> _userControl;

        public CalculateView(Action<UserControl> action)
        {
            InitializeComponent();
            init();
            
            _userControl = action;
        }

        public void init()
        {
            _mediator = DIContainer.container.Resolve<IMediator>();
            mainGrid.DataContext = vm;
        }

        private void tbId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnSearch_Click(null, null);
        }

        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbCurp.Text))
                return;

            var response = await _mediator.Send(new GetEmployeeDataGridCommand()
            {
                curp = tbCurp.Text
            });


            if (!response.data.Any())
            {
                MessageBox.Show("No se encontro un empleado registrado con ese CURP");
                return;
            }

            using (var ctx = new EmployeeEntity())
                tbAccidentes.Text = ctx.accidents.totalAccidentsByCurp(tbCurp.Text).ToString();
            
            _fechaNacimiento = response.data.Select(x => x.fecha_nacimiento).First();
            tbDiasVividos.Text = Utils.Data.DataCalc.daysLived(_fechaNacimiento).ToString();
            tbFechaNacimiento.Text = _fechaNacimiento.ToString("dd/MM/yyyy");
            btnCalculate.IsEnabled = true;
        }

        private void btnClean_Click(object sender, RoutedEventArgs e)
        {
            tbCurp.Text = "";
            tbAccidentes.Text = "";
            tbDiasVividos.Text = "";
            tbFechaNacimiento.Text = "";
            btnCalculate.IsEnabled = false;
        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            var livingDaysFirstMoth = DataCalc.daysLived(_fechaNacimiento, DataCalc.getFirstDayMonth());
            _userControl(new EmployeeBiorytm(tbDiasVividos.Text, livingDaysFirstMoth));

            //dias = int.Parse(tbDiasVividos.Text);
            //var biorritmoFisico = CalcularBiorritmo(dias,23);
            //var biorritmoEmocional = CalcularBiorritmo(dias, 28);
            //var biorritmoIntelectual = CalcularBiorritmo(dias, 33);
            //var biorritmoIntuicional = CalcularBiorritmo(dias, 38);

            //var results = new Results(tbAccidentes.Text,_fechaNacimiento.ToString(),tbCurp.Text,biorritmoFisico,biorritmoEmocional,biorritmoIntelectual,biorritmoIntuicional);
            //results.ShowDialog(); 


        }


    }
}
