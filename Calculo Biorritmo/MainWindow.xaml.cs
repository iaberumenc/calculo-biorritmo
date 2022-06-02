using Calculo_Biorritmo.ApplicationLayer.Constants;
using Calculo_Biorritmo.Interfaces;
using Calculo_Biorritmo.Utils.Data;
using Calculo_Biorritmo.Screens;
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
using Calculo_Biorritmo.Screens.Employees;
using Calculo_Biorritmo.Screens.Calculate;
using System.Data.SqlClient;
using System.Configuration;
using Calculo_Biorritmo.Screens.Home;
using Calculo_Biorritmo.Screens.Accidents;
using System.Data.Entity.Infrastructure;
using Calculo_Biorritmo.Data;
using System.Data.Entity.Migrations;
using Calculo_Biorritmo.Loading;

namespace Calculo_Biorritmo
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            checkFiles();
            InitializeComponent();
            initData();
            LoadEvents();
            ApiStatus();
        }

        public async void ApiStatus()
        {
            var apistatus = await Api.ApiConnection.GetApiStatus();
            if (apistatus)
            {
                lblApiStatus.Content = "Conectado";
                lblApiStatus.Background = new SolidColorBrush(Colors.Green);
            }
        }

        private void LoadEvents()
        {
            this.Deactivated += new EventHandler(LoadBlur);
            this.Activated += new EventHandler(UnloadBlur);
        }

        private void UnloadBlur(object sender, EventArgs e)
        {
            GridBlock.Visibility = Visibility.Collapsed;
        }

        private void LoadBlur(object sender, EventArgs e)
        {
            if (!this.IsFocused)
            {
                GridBlock.Visibility = Visibility.Visible;
            }
        }

        private void checkFiles()
        {
            LoadingView load = new LoadingView();
            load.ShowDialog();
        }

        private void initData()
        {
            try
            {
                DIContainer.container = AutofacRegistrations.Register();
                gridView.Children.Add(new HomeView());
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Employees_Click(object sender, RoutedEventArgs e)
        {
            ResetColors();
            Employees.BorderBrush = Brushes.LightBlue;
            gridView.Children.Clear();
            gridView.Children.Add(new EmployeesView());
        }

        private void Biorritm_Click(object sender, RoutedEventArgs e)
        {
            ResetColors();
            Biorritm.BorderBrush = Brushes.LightBlue;
            gridView.Children.Clear();
            gridView.Children.Add(new CalculateView(ChangeCalculateView));
        }

        private void Main_Click(object sender, RoutedEventArgs e)
        {
            ResetColors();
            Main.BorderBrush = Brushes.LightBlue;
            gridView.Children.Clear();
            gridView.Children.Add(new HomeView());
        }

        private void Accident_Click(object sender, RoutedEventArgs e)
        {
            ResetColors();
            Accident.BorderBrush = Brushes.LightBlue;
            gridView.Children.Clear();
            gridView.Children.Add(new AccidentView(ChangeAccidentsView));
        }

        private void ResetColors()
        {
            Main.BorderBrush = Brushes.Transparent;
            Employees.BorderBrush = Brushes.Transparent;
            Biorritm.BorderBrush = Brushes.Transparent;
            Accident.BorderBrush = Brushes.Transparent;
        }

        public void ChangeCalculateView(UserControl userControl)
        {
            gridView.Children.Clear();
            gridView.Children.Add(userControl);
        }

        public void ChangeAccidentsView(UserControl userControl)
        {
            gridView.Children.Clear();
            gridView.Children.Add(userControl);
        }


    }
}
