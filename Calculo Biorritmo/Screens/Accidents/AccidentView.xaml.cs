using Autofac;
using Calculo_Biorritmo.ApplicationLayer.Queries.Accidents.Data;
using Calculo_Biorritmo.Algorytms;
using MediatR;
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
using Calculo_Biorritmo.Screens.Calculate.BiorytmResults;
using Calculo_Biorritmo.ViewModel;

namespace Calculo_Biorritmo.Screens.Accidents
{
    /// <summary>
    /// Lógica de interacción para AccidentView.xaml
    /// </summary>
    public partial class AccidentView : UserControl
    {
        public Action<UserControl> _userControl;
        private IMediator _mediator;
        public AccidentView(Action<UserControl> action)
        {
            InitializeComponent();
            _userControl = action;
            init();
            AccidentAlgorytm.startAlgorytm();
            AccidentAlgorytm.checkCritics();
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
                var response = await _mediator.Send(new GetAccidentDataGridCommand()
                {
                    curp = tbBuscar.Text,
                });

                empleado.ItemsSource = response.data;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha ocurrido un error" +ex.Message);
            }
        }

        private void btnAddAccident_Click(object sender, RoutedEventArgs e)
        {
            var addNewEmployee = new addAccident();
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

        private void btnCheckNeuralInfo_Click(object sender, RoutedEventArgs e)
        {
            if(empleado.Items.Count == 0)
            {
                MessageBox.Show("El algoritmo no tiene informacion para procesar");
                return;
            }
            AlgorytmInfo algorytmInfo = new AlgorytmInfo();
            algorytmInfo.Show();
        }

        //private void btnCheckGraph_Click(object sender, RoutedEventArgs e)
        //{
        //    //_userControl(new EmployeeBiorytm(_userControl,"",0));
        //}
    }
}
