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
using Microsoft.Office.Interop.Excel;
using System.Collections.Generic;

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

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                excel.ScreenUpdating = false;
                Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
                Worksheet sheet1 = (Worksheet)workbook.Sheets[1];
                var EmployeeData = (List<employeeGridItem>)empleado.ItemsSource;

                var DataArray = EmployeeData.ToArray();

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
                    FinalArray[i, 1] = DataArray[i].fecha_nacimiento.ToString();
                    FinalArray[i, 2] = DataArray[i].dias_vividos.ToString();
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
