/*using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using OxyPlot.Wpf;

namespace Calculo_Biorritmo.Front
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            OxyPlot.WindowsForms.PlotView pv = new PlotView();
            pv.Location = new Point(0, 0);
            pv.Size = new Size(500, 250);
            this.Controls.Add(pv);

            pv.Model = new PlotModel { Title = "Biorritmo" };

            FunctionSeries fs = new FunctionSeries();
            fs.Points.Add(new DataPoint(0, 0));
            fs.Points.Add(new DataPoint(10, 10));
            pv.Model.Series.Add(fs);

            pv.Model.Series.Add(new FunctionSeries(Math.Sin, 0, 10, 0.1,"sin(x)"));

            'Irving Rene Gonzalez Eugenio


        }
    }
}
*/