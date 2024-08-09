using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DarkDemo.Clases
{
    internal class CConexion
    {
        MySqlConnection conex = new MySqlConnection();
        // valores para la cadena de conexion
        static string servidor = "127.0.0.1";
        static string bd = "pecesSena";
        static string usuario = "root";
        static string password = "";
        static string puerto = "3306";
        // cadena para validar la conexion a la base de datos
        string cadenaConexion = "server=" + servidor + ";port=" + puerto + ";user id=" + usuario + ";password=" + password + ";database=" + bd + ";";

        //Realizar la conexion a la base de datos
        public MySqlConnection establecerConexion()
        {
            try
            {
                //preparar la conexion a la base de datos
                conex.ConnectionString = cadenaConexion;
                // Realizar la conexion
                conex.Open();
                //    MessageBox.Show("conexion Exitosa");
            }
            //errores al conectar a la base de datos.
            catch (Exception e)
            {
                MessageBox.Show("no se pudo realizar la conexion" + e.ToString());
            }
            return conex;
        }
        //cerrar la conexion a la base de datos
        public void cerrarConexion()
        {
            conex.Close();
        }
    }
}
