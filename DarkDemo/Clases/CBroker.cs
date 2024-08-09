using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace DarkDemo.Clases
{
    internal class CBroker
    {
        public void LimpiarDatos(TextBox textBox)
        {
            textBox.Text = string.Empty;
        }
        public void RegistrarBroker(TextBox textBox1, TextBox textBox2, TextBox textBox3, TextBox textBox4, DataGridView dataGrid)
        {
            try
            {
                CConexion cConexion = new CConexion();
                string query = "INSERT INTO broker (host, puerto, topico_ox, topico_tem)" +
                               "VALUES (@host, @puerto, @topico_ox, @topico_tem);";
                MySqlCommand mySqlCommand = new MySqlCommand(query, cConexion.establecerConexion());

                mySqlCommand.Parameters.AddWithValue("@host", textBox1.Text);
                mySqlCommand.Parameters.AddWithValue("@puerto", textBox2.Text);
                mySqlCommand.Parameters.AddWithValue("@topico_ox", textBox3.Text);
                mySqlCommand.Parameters.AddWithValue("@topico_tem", textBox4.Text);
                int filasAfectadas = mySqlCommand.ExecuteNonQuery();

                cConexion.cerrarConexion();

                if (filasAfectadas > 0)
                {
                    LimpiarDatos(textBox1);
                    LimpiarDatos(textBox2);
                    LimpiarDatos(textBox3);
                    LimpiarDatos(textBox4);
                }

                MostrarBrokers(dataGrid);
            }
            catch (Exception er)
            {
                MessageBox.Show("Error en el registro: " + er.Message);
            }
        }

        public void MostrarBrokers(DataGridView dataGrid)
        {
            try
            {
                CConexion conexion = new CConexion();
                string query = "SELECT id AS Numero_broker, host, puerto, topico_ox,topico_tem FROM broker";
                MySqlCommand mySqlCommand = new MySqlCommand(query, conexion.establecerConexion());

                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(mySqlCommand);
                DataTable dataTable = new DataTable();

                dataAdapter.Fill(dataTable);

                dataGrid.DataSource = dataTable;

                dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                dataGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                conexion.cerrarConexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al mostrar los datos: " + ex.Message);
            }
        }

    }
}
