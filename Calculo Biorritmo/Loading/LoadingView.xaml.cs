using Calculo_Biorritmo.Api;
using Calculo_Biorritmo.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Calculo_Biorritmo.Loading
{
    /// <summary>
    /// Lógica de interacción para LoadingView.xaml
    /// </summary>
    public partial class LoadingView : Window
    {
        public LoadingView()
        {
            InitializeComponent();
            init();
        }

        private async void init()
        {

            var t = await ApplyMigrations();
            Console.WriteLine(t);
        }

        private async Task<bool> ApplyMigrations()
        {
            try
            {
                progress.Value = 20;
                await Task.Delay(1000);
                var dbInfo = new DbConnectionInfo(ConfigurationManager.ConnectionStrings["BiorytmDb"].ToString(), "System.Data.SqlClient");
                var config = new Calculo_Biorritmo.Migrations.Configuration();
                config.MigrationsAssembly = typeof(EmployeeEntity).Assembly;
                config.MigrationsNamespace = "Calculo_Biorritmo.Migrations";
                config.ContextKey = "Calculo_Biorritmo.Migrations.Configuration";
                config.TargetDatabase = dbInfo;

                status.Content = "Conectando con el servidor de bases datos";
                progress.Value = 40;
                await Task.Delay(10);

                var migrator = new DbMigrator(config);
                migrator.Configuration.TargetDatabase = dbInfo;

                status.Content = "Verificando actualizaciones";
                progress.Value = 60;
                await Task.Delay(10);

                status.Content = "Verificando datos en el API";
                progress.Value = 80;
                await ApiConnection.RefreshEmployeesFromApiAsync();
                await ApiConnection.RefreshAccidentsFromApiAsync();
                await ApiConnection.checkPendingSync();
                await Task.Delay(10);

                var migrations = migrator.GetPendingMigrations();

                status.Content = "Cargando sistema";
                progress.Value = 90;
                await Task.Delay(10);

                if (!migrations.Any())
                    Close();
                else
                    migrator.Update();

                return true;
            }
            catch (SqlException ex)
            {
                MessageBox.Show("La computadora cliente no cuenta con una base de datos local o no se encontro la instancia .SQLEXPRESS. Favor de instalar SQL Express");
                Application.Current.Shutdown();
                return false;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
                return false;
            }
        }
    }
}
