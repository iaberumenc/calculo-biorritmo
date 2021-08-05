using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows;
using Calculo_Biorritmo.Models;

namespace Calculo_Biorritmo.Connection
{
    class DbClass
    {
        public static string GetConnectionStrings()
        {
            string strConString = ConfigurationManager.ConnectionStrings["conString"].ToString();
            return strConString;
        }

        public static string sql;
        public static SqlConnection con = new SqlConnection();
        public static SqlCommand cmd = new SqlCommand("", con);
        public static SqlDataReader rd;
        public static DataTable dt;
        public static SqlDataAdapter da;

        public static DataView getEmployees()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.ConnectionString = GetConnectionStrings();
                    con.Open();
                }

                if (con.State == ConnectionState.Open)
                {
                    var con = new SqlConnection();
                    con.ConnectionString = ConfigurationManager.ConnectionStrings["connectedString"].ToString();
                    con.Open();

                    var query = $@"
                            SELECT curp as CURP, 
	                        fecha_nacimiento as Fecha_Nacimiento, 
	                        anio as Año,
	                        dias_vividos as Dias_Vividos
                            FROM Employee";

                    var cmd = new SqlCommand(query,con);
                    var data = new SqlDataAdapter(cmd);
                    var dt = new DataTable();
                    data.Fill(dt);
                    return dt.DefaultView;
                }
                return null;
            }
            catch (Exception ex)
            {

                MessageBox.Show("El sistema fallo al conectar a la base de datos." + Environment.NewLine +
                                "Descriptions: " + ex.Message.ToString(), "C# WPF Connect to SQL Server",MessageBoxButton.OK,MessageBoxImage.Error);
                return null;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

        }

        public static void closeConnection()
        {
            try
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            catch (Exception)
            {

                //
            }
        }

        public void GenerateDBIfRequired()
        {
            con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["conString"].ToString();

            var queryDB = $@"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'biorytm')
                BEGIN
                    CREATE DATABASE biorytm;
                END;";

            var queryTable = $@"USE biorytm

                IF NOT EXISTS (SELECT 1
                        FROM sys.tables
                        WHERE name = 'Employee'
                            AND type = 'U')
                BEGIN
                    CREATE TABLE Employee 
	                    (curp varchar(16) primary key, 
	                    fecha_nacimiento date, 
	                    anio int,
	                    mes int,
	                    dia int, 
	                    fecha_accidente date,
	                    anio_acc int,
	                    mes_acc int,
	                    dia_acc int,
	                    dias_vividos int,
	                    residuo_fisico int,
	                    residuo_emocional int,
	                    residuo_intelectual int,
	                    residuo_intuicional int);
                END;";

            var database = new SqlCommand(queryDB, con);
            var table = new SqlCommand(queryTable, con);
            try
            {
                con.Open();
                database.ExecuteNonQuery();
                table.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
    }
}
