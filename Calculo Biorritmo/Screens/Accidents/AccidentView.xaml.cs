using Autofac;
using Calculo_Biorritmo.ApplicationLayer.Queries.Accidents.Data;
using Calculo_Biorritmo.Algorytms;
using MediatR;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Calculo_Biorritmo.Screens.Calculate.BiorytmResults;
using Calculo_Biorritmo.ApplicationLayer.Queries.Employees.Data;
using Calculo_Biorritmo.Utils.Data;
using Calculo_Biorritmo.Models;
using Calculo_Biorritmo.Screens.Generic;
using Calculo_Biorritmo.ApplicationLayer.Constants;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Office.Interop.Excel;
using System.Windows.Input;

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
                var genericMessage = new GenericMessage("Ha ocurrido un error" + ex.Message);
                genericMessage.ShowDialog();
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
                var genericMessage = new GenericMessage("El algoritmo no tiene informacion para procesar");
                genericMessage.ShowDialog();
                return;
            }
            AlgorytmInfo algorytmInfo = new AlgorytmInfo();
            algorytmInfo.Show();
        }


        private void btnCheckGraph_Click(object sender, RoutedEventArgs e)
        {
            var accident = (accidentGridItem)empleado.SelectedItem;
            if (accident == null)
            {
                var genericMessage = new GenericMessage("Seleccione un registro");
                genericMessage.ShowDialog();
                return;
            }
            var BirthDay = DataCalc.getBirthDateFromCurp(accident.curp);
            var livingDaysFirstMoth = DataCalc.daysLived(BirthDay, DataCalc.getFirstDayMonth(accident.fecha_accidente));
            _userControl(new EmployeeBiorytm(_userControl, ViewEnum.AccidentViewEnum, DataCalc.daysLived(BirthDay,accident.fecha_accidente).ToString(), livingDaysFirstMoth, accident.fecha_accidente));
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                excel.ScreenUpdating = false;
                Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
                Worksheet sheet1 = (Worksheet)workbook.Sheets[1];
                var AccidentData = (List<accidentGridItem>)empleado.ItemsSource;

                var DataArray = AccidentData.ToArray();
            
                for (int j = 0; j < empleado.Columns.Count; j++)
                {
                    Range myRange = (Range)sheet1.Cells[1, j + 1];
                    sheet1.Cells[1, j + 1].Font.Bold = true;
                    sheet1.Columns[j + 1].ColumnWidth = 20;
                    myRange.Value2 = empleado.Columns[j].Header;
                }

                string[,] FinalArray = new string[empleado.Items.Count, empleado.Columns.Count];


                for (int i = 0; i < empleado.Items.Count; i++)
                {
                    FinalArray[i, 0] = DataArray[i].curp.ToString();
                    FinalArray[i, 1] = DataArray[i].fecha_accidente.ToString();
                    FinalArray[i, 2] = DataArray[i].residuo_fisico.ToString();
                    FinalArray[i, 3] = DataArray[i].residuo_emocional.ToString();
                    FinalArray[i, 4] = DataArray[i].residuo_intelectual.ToString();
                    FinalArray[i, 5] = DataArray[i].residuo_intuicional.ToString();
                }

                int rowCount = FinalArray.GetLength(0);
                int columnCount = FinalArray.GetLength(1);

                Range range = (Range)sheet1.Cells[2, 1];
                range = range.get_Resize(rowCount, columnCount);
                range.set_Value(XlRangeValueDataType.xlRangeValueDefault, FinalArray);

                excel.ScreenUpdating = true;
                excel.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
