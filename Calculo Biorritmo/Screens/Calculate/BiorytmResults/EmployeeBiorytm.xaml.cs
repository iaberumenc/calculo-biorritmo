using Calculo_Biorritmo.Algorytms;
using Calculo_Biorritmo.ApplicationLayer.Constants;
using Calculo_Biorritmo.Utils.Data;
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

namespace Calculo_Biorritmo.Screens.Calculate.BiorytmResults
{
    /// <summary>
    /// Lógica de interacción para EmployeeBiorytm.xaml
    /// </summary>
    public partial class EmployeeBiorytm : UserControl
    {
        Random r = new Random();
        Generadora generador;
        private string _livingDays;
        private int _livingDaysFirstDayMonth;
        public EmployeeBiorytm(string livingDays,int livingDaysFirstDayMonth)
        {
            InitializeComponent();
            generador = new Generadora();
            _livingDays = livingDays;
            _livingDaysFirstDayMonth = livingDaysFirstDayMonth;
            init();
        }

        public void init()
        {
            generador.CalcularBiorritmo(_livingDaysFirstDayMonth, BiorytmDays.biorritmo_fisico);
            //Tabla.ItemsSource = null;
            //Tabla.ItemsSource = generador.Puntos;
            PlotModel model = new PlotModel();
            LinearAxis ejeX = new LinearAxis();
            ejeX.Minimum = double.Parse("1");
            ejeX.Maximum = double.Parse("30");
            ejeX.Position = AxisPosition.Bottom;

            LinearAxis ejeY = new LinearAxis();
            ejeY.Minimum = generador.Puntos.Min(p => p.Y);
            ejeY.Maximum = generador.Puntos.Max(p => p.Y);
            ejeY.Position = AxisPosition.Left;

            model.Axes.Add(ejeX);
            model.Axes.Add(ejeY);
            model.Title = "Biorritmo";
            LineSeries Fisico = new LineSeries();
            foreach (var item in generador.Puntos)
            {
                Fisico.Points.Add(new DataPoint(item.X, item.Y));
            }
            Fisico.Title = "Fisico";
            Fisico.Color = OxyColor.FromRgb(byte.Parse(r.Next(0, 255).ToString()), byte.Parse(r.Next(0, 255).ToString()), byte.Parse(r.Next(0, 255).ToString()));

            generador.CalcularBiorritmo(_livingDaysFirstDayMonth, BiorytmDays.biorritmo_emocional);
            LineSeries Emocional = new LineSeries();
            foreach (var item in generador.Puntos)
            {
                Emocional.Points.Add(new DataPoint(item.X, item.Y));
            }
            Emocional.Title = "Emocional";
            Emocional.Color = OxyColor.FromRgb(byte.Parse(r.Next(0, 255).ToString()), byte.Parse(r.Next(0, 255).ToString()), byte.Parse(r.Next(0, 255).ToString()));

            generador.CalcularBiorritmo(_livingDaysFirstDayMonth, BiorytmDays.biorritmo_intelectual);
            LineSeries Intelectual = new LineSeries();
            foreach (var item in generador.Puntos)
            {
                Intelectual.Points.Add(new DataPoint(item.X, item.Y));
            }
            Intelectual.Title = "Intelectual";
            Intelectual.Color = OxyColor.FromRgb(byte.Parse(r.Next(0, 255).ToString()), byte.Parse(r.Next(0, 255).ToString()), byte.Parse(r.Next(0, 255).ToString()));

            generador.CalcularBiorritmo(_livingDaysFirstDayMonth, BiorytmDays.biorritmo_intuicional);
            LineSeries Intuicional = new LineSeries();
            foreach (var item in generador.Puntos)
            {
                Intuicional.Points.Add(new DataPoint(item.X, item.Y));
            }
            Intuicional.Title = "Intuicional";
            Intuicional.Color = OxyColor.FromRgb(byte.Parse(r.Next(0, 255).ToString()), byte.Parse(r.Next(0, 255).ToString()), byte.Parse(r.Next(0, 255).ToString()));

            LineSeries Cero = new LineSeries();
            Cero.Points.Add(new DataPoint(0, 0));
            Cero.Points.Add(new DataPoint(30, 0));
            Cero.Color = OxyColor.FromRgb(0, 0, 0);

            model.Series.Add(Fisico);
            model.Series.Add(Emocional);
            model.Series.Add(Intelectual);
            model.Series.Add(Intuicional);
            model.Series.Add(Cero);
            asd.Model = model;

            var fisic = DataCalc.CalculateBiorritm(Convert.ToInt32(_livingDays), BiorytmDays.biorritmo_fisico);
            var emotional = DataCalc.CalculateBiorritm(Convert.ToInt32(_livingDays), BiorytmDays.biorritmo_emocional);
            var intelectual = DataCalc.CalculateBiorritm(Convert.ToInt32(_livingDays), BiorytmDays.biorritmo_intelectual);
            var intuitional = DataCalc.CalculateBiorritm(Convert.ToInt32(_livingDays), BiorytmDays.biorritmo_intuicional);

            lblFisicBiorytm.Content = "Fisico: " + fisic[1];
            lblEmotionalBiorytm.Content = "Emocional: " + emotional[1];
            lblIntelectualBiorytm.Content = "Intelectual: " + intelectual[1];
            lblIntuitionalBiorytm.Content = "Intuicional: " + intuitional[1];

            if(AccidentAlgorytm.calculateCritics(fisic) == null)
            {
                lblFisicCritic.Content = "Fisico: OK...";
            }
            else
            {
                lblFisicCritic.Content = "Fisico: CRITICO";
                lblFisicCritic.Foreground = new SolidColorBrush(Colors.Red);
            }
            if (AccidentAlgorytm.calculateCritics(emotional) == null)
            {
                lblEmotionalCritic.Content = "Emocional: OK...";
            }
            else
            {
                lblEmotionalCritic.Content = "Emocional: CRITICO";
                lblEmotionalCritic.Foreground = new SolidColorBrush(Colors.Red);
            }
            if (AccidentAlgorytm.calculateCritics(intelectual) == null)
            {
                lblIntelectualCritic.Content = "Intelectual: OK...";
            }
            else
            {
                lblIntelectualCritic.Content = "Intelectual: CRITICO";
                lblIntelectualCritic.Foreground = new SolidColorBrush(Colors.Red);
            }
            if (AccidentAlgorytm.calculateCritics(intuitional) == null)
            {
                lblIntuitionalCritic.Content = "Intuicional: OK...";
            }
            else
            {
                lblIntuitionalCritic.Content = "Intuicional: CRITICO";
                lblIntuitionalCritic.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        private void BtnRegresar_Click(object sender, RoutedEventArgs e)
        {
            //_userControl(new CalculateView(tbDiasVividos.Text));
        }

    }
}
