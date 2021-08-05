using Calculo_Biorritmo.Algorytms;
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
using System.Windows.Shapes;

namespace Calculo_Biorritmo.Screens.Accidents
{
    /// <summary>
    /// Lógica de interacción para AlgorytmInfo.xaml
    /// </summary>
    public partial class AlgorytmInfo : Window
    {
        public AlgorytmInfo()
        {
            InitializeComponent();
            init();
        }

        public void init()
        {
            var critics = AccidentAlgorytm.checkCritics();
            var avgs = AccidentAlgorytm.startAlgorytm();

            try
            {
                lblTotalAccidents.Content = critics[0].ToString();

                lblTotalCriticFisic.Content = critics[1].ToString();
                lblTotalCriticFisicPercent.Content = "(" + ((critics[1] * 100) / critics[0]).ToString() + "%)";

                lblTotalCriticEmotional.Content = critics[2].ToString();
                lblTotalCriticEmotionalPercent.Content = "(" + ((critics[2] * 100) / critics[0]).ToString() + "%)";

                lblTotalCriticIntuitional.Content = critics[3].ToString();
                lblTotalCriticIntuitionalPercent.Content = "(" + ((critics[3] * 100) / critics[0]).ToString() + "%)";

                lblTotalCriticIntelectual.Content = critics[4].ToString();
                lblTotalCriticIntelectualPercent.Content = "(" + ((critics[4] * 100) / critics[0]).ToString() + "%)";

                lblTotalAllCriticsAccidents.Content = critics[5].ToString();
                lblTotalAllCriticsAccidentsPercent.Content = "(" + ((critics[5] * 100) / critics[0]).ToString() + "%)";

                lblTotalCriticAccidents.Content = critics[6].ToString();
                lblTotalCriticAccidentsPercent.Content = "(" + ((critics[6] * 100) / critics[0]).ToString() + "%)";

                lblAvgFisic.Content = avgs.biorritmoFisico.ToString();
                lblAvgFisicTotal.Content = "(" + avgs.totalBiorritmoFisico.ToString() + ")";
                lblAvgEmotional.Content = avgs.biorritmoEmocional.ToString();
                lblAvgEmotionalTotal.Content = "(" + avgs.totalBiorritmoEmocional.ToString() + ")";
                lblAvgIntelectual.Content = avgs.biorritmoIntelectual.ToString();
                lblAvgIntelectualTotal.Content = "(" + avgs.totalBiorritmoIntelectual.ToString() + ")";
                lblAvgIntuitional.Content = avgs.biorritmoIntuicional.ToString();
                lblAvgIntuitionalTotal.Content = "(" + avgs.totalBiorritmoIntuicional.ToString() + ")";
            }
            catch (Exception)
            {
                MessageBox.Show("No se cuenta con la suficiente informacion");
                Close();
            }
            
        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
