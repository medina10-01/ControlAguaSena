using DarkDemo.Clases;
using iTextSharp.text.pdf;
using iTextSharp.text;
using MySql.Data.MySqlClient;
using Mysqlx.Cursor;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Image = iTextSharp.text.Image;
using Rectangle = iTextSharp.text.Rectangle;
using Font = iTextSharp.text.Font;
using OxyPlot.Series;
using OxyPlot;
using PageSize = iTextSharp.text.PageSize;
using Element = iTextSharp.text.Element;
using System.Data.SqlClient;

namespace DarkDemo
{
    internal class ControlDatos
    {
        DataGridView tablareportes2;
        public void RealizarRegistro(double oxigeno, double temperatura)
        {
            try
            {

                // redondear datos
                double oxigenoRound = Math.Round(oxigeno, 2);
                double temperaturaRound = Math.Round(temperatura, 2);

                //  MessageBox.Show(oxigeno.ToString());
                DateTime dateTime = DateTime.Now;
                string fechaFormateada = dateTime.ToString("yyyy-MM-dd"); 

                CConexion cConexion = new CConexion();
                String query = "INSERT INTO lecturas (oxigeno, temperatura, fecha)" +
                    "VALUES (@oxigeno, @temperatura, @fecha);";
                MySqlCommand mySqlCommand = new MySqlCommand(query, cConexion.establecerConexion());

                mySqlCommand.Parameters.AddWithValue("@oxigeno", oxigenoRound);
                mySqlCommand.Parameters.AddWithValue("@temperatura", temperaturaRound);
                mySqlCommand.Parameters.AddWithValue("@fecha", fechaFormateada);
                int filasAfectadas = mySqlCommand.ExecuteNonQuery();

                cConexion.cerrarConexion();


            }
            catch (Exception ex) {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        public void listarBrokers(System.Windows.Forms.ComboBox comboBox)
        {
            try
            {
                if (comboBox == null)
                {
                    throw new ArgumentNullException(nameof(comboBox), "El ComboBox no puede ser nulo.");
                }

                // Crear una conexión
                CConexion conexion = new CConexion();
                string query = "SELECT Id FROM broker ORDER BY Id DESC"; // Ordenar por Id en orden descendente

                // Crear el comando y adaptador
                MySqlCommand mySqlCommand = new MySqlCommand(query, conexion.establecerConexion());
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(mySqlCommand);
                DataTable dataTable = new DataTable();

                // Llenar el DataTable con los datos del DataAdapter
                dataAdapter.Fill(dataTable);

                // Limpiar el ComboBox antes de llenarlo
                comboBox.Items.Clear();

                // Llenar el ComboBox con los datos
                foreach (DataRow row in dataTable.Rows)
                {
                    comboBox.Items.Add(row["Id"].ToString());
                }

                if (comboBox.Items.Count > 0)
                {
                    comboBox.SelectedIndex = 0; // Selecciona el primer ítem
                }

                // Cerrar la conexión
                conexion.cerrarConexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se mostraron datos. Error: " + ex.ToString());
            }
        }



        public void DatosNuevaConexion(System.Windows.Forms.ComboBox id, out string host, out string puerto, out string topico_ox, out string topico_tem)
        {
            host = null;
            puerto = null;
            topico_ox = null;
            topico_tem = null;

            try
            {
                CConexion cConexion = new CConexion();
                string query = "SELECT host, puerto, topico_ox, topico_tem FROM broker WHERE id = @id";

                // Crear el comando y adaptador
                MySqlCommand mySqlCommand = new MySqlCommand(query, cConexion.establecerConexion());
                mySqlCommand.Parameters.AddWithValue("@id", id.Text);

                // Ejecutar el comando y obtener el lector de datos
                MySqlDataReader dataReader = mySqlCommand.ExecuteReader();

                // Verificar si hay datos
                if (dataReader.Read())
                {
                    host = dataReader["host"].ToString();
                    puerto = dataReader["puerto"].ToString();
                    topico_ox = dataReader["topico_ox"].ToString();
                    topico_tem = dataReader["topico_tem"].ToString();
                }
                else
                {
                    MessageBox.Show("No se encontraron datos para el ID proporcionado.");
                }

                // Cerrar el lector y la conexión
                dataReader.Close();
                cConexion.cerrarConexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se realizó la conexión al Broker: " + ex.Message);
            }
        }

        public void MostrarRegistros(DataGridView tablaReportes)
        {
            try
            {
                CConexion objetoConeccion = new CConexion();
                String query = "SELECT    fecha,   ROUND(AVG(oxigeno), 2) AS promedio_oxigeno,    ROUND(AVG(temperatura), 2) AS promedio_temperatura FROM    lecturas GROUP BY     fecha ORDER BY     fecha DESC";
        
                tablaReportes.DataSource = null;
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, objetoConeccion.establecerConexion());
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
        
                tablaReportes.DataSource = dataTable;
        
                // Establecer el estilo del encabezado
                tablaReportes.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(231, 154, 37); // RGB para naranja
                tablaReportes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; // Texto blanco para los encabezados
                tablaReportes.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold); // Fuente de los encabezados
                tablaReportes.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // Centrar el texto
                tablaReportes.EnableHeadersVisualStyles = false; // Deshabilitar los estilos visuales predeterminados de los encabezados
        
                // Agregar columna de botones
                if (tablaReportes.Columns["Eliminar"] == null)
                {
                    DataGridViewButtonColumn btnColumnEliminar = new DataGridViewButtonColumn
                    {
                        Name = "PDFToMail",
                        HeaderText = "Email",
                        Text = "Enviar a Email",
                        UseColumnTextForButtonValue = true,
                        DefaultCellStyle = new DataGridViewCellStyle
                        {
                            BackColor = Color.White,
        
                        }
                    };
        
                    DataGridViewButtonColumn btnColumnPDF = new DataGridViewButtonColumn
                    {
                        Name = "PDF",
                        HeaderText = "PDF",
                        Text = "PDF",
                        UseColumnTextForButtonValue = true,
                        DefaultCellStyle = new DataGridViewCellStyle
                        {
                            BackColor = Color.White,
                            ForeColor = Color.White
                        }
                    };
        
                    tablaReportes.Columns.Add(btnColumnEliminar);
                    tablaReportes.Columns.Add(btnColumnPDF);
                }
        
                // Configurar las columnas para que llenen el espacio disponible
                tablaReportes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        
                // Configurar el evento CellContentClick para manejar clics en los botones
                tablaReportes.CellContentClick += TablaReportes_CellContentClick;
        
                objetoConeccion.cerrarConexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se mostraron datos Error:" + ex.ToString());
            }
        }
        public void PrimerRegistro(DateTimePicker dateTimePicker)
        {
            try
            {
                CConexion cConexion = new CConexion();
                String query = "SELECT MIN(fecha) AS fecha FROM lecturas";

                // Abre la conexión
                MySqlConnection connection = cConexion.establecerConexion();

                // Crear comando para ejecutar la consulta
                MySqlCommand command = new MySqlCommand(query, connection);

                // Ejecutar la consulta y obtener el resultado
                object result = command.ExecuteScalar();

                if (result != DBNull.Value)
                {
                    // Convertir el resultado a DateTime y establecerlo en el DateTimePicker
                    DateTime fecha = Convert.ToDateTime(result);
                    dateTimePicker.Value = fecha;
                }
                else
                {
                    MessageBox.Show("No se encontró ninguna fecha en la base de datos.");
                }

                // Cerrar la conexión
                cConexion.cerrarConexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void TablaReportes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridView dgv = sender as DataGridView;

                // Obtener el valor de la celda como cadena
                var fechaValue = dgv.Rows[e.RowIndex].Cells["fecha"].Value?.ToString();

                // Convertir la cadena a un objeto DateTime
                DateTime fecha;
                if (DateTime.TryParse(fechaValue, out fecha))
                {
                    // Formatear la fecha en el formato "yyyy/MM/dd"
                    var formattedDate = fecha.ToString("yyyy/MM/dd");

                     if (dgv.Columns[e.ColumnIndex].Name == "PDFToMail")
                     {
                        DataTable dataTable = SeleccionarProceso(formattedDate);
                        if (dataTable != null)
                        
                        {
                            string fechaFormateada = fecha.ToString("yyyy-MM-dd");
                            string pdfPath = Path.Combine(
                                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                                @"Downloads\Sena\PDF\" + fechaFormateada + ".pdf"
                            );
                            GenerarPDF(dataTable, pdfPath);
                            sendMail sendMail = new sendMail();
                            string destino = "mstiven748@gmail.com";
                            string asunto = "Reporte de lagos en la fecha: "+fechaFormateada;
                            // Este ejemplo asume que ya tienes un control WebBrowser en tu formulario

                            byte[] bites = File.ReadAllBytes(pdfPath);

                            string cuerpo = "Pdf del reporte:";

                            sendMail.mailSender(destino, asunto, cuerpo, bites, fechaFormateada);
                        }
                        // EliminarProceso(procesoValue);
                       // MessageBox.Show($"Eliminar clicked for proceso: {formattedDate}");
                     }
                     else if (dgv.Columns[e.ColumnIndex].Name == "PDF")
                     {
                        DataTable dataTable = SeleccionarProceso(formattedDate);

                        if (dataTable != null)
                        {
                            // Si SeleccionarProceso fue exitoso y devolvió datos, generar el PDF
                            string fechaFormateada = fecha.ToString("yyyy-MM-dd");
                            string pdfPath = Path.Combine(
                                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                                @"Downloads\Sena\PDF\" + fechaFormateada + ".pdf"
                            );

                            GenerarPDF(dataTable, pdfPath);
                        }
                        else
                        {
                            MessageBox.Show("No se generó el PDF debido a que no se encontraron datos.");
                        }
                    }
                }
                else
                {
                    // Manejar el caso en que la conversión falle (opcional)
                    MessageBox.Show("La fecha no es válida.");
                }
            }
        }

        public DataTable SeleccionarProceso(string fecha)
        {
            try
            {
                if (string.IsNullOrEmpty(fecha))
                {
                    MessageBox.Show("La fecha no puede estar vacía");
                    return null;
                }

                CConexion objetoConexion = new CConexion();
                String query = "SELECT fecha, oxigeno, temperatura " +
                               "FROM lecturas " +
                               "WHERE fecha = @fecha ";

                using (MySqlConnection connection = objetoConexion.establecerConexion())
                {
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                    adapter.SelectCommand.Parameters.AddWithValue("@fecha", fecha);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count == 0)
                    {
                        MessageBox.Show("No se encontraron datos para la fecha especificada.");
                        return null;
                    }

                    return dataTable; // Devolver el DataTable si tiene datos
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se mostraron datos. Error: " + ex.ToString());
                return null;
            }
        }

        public void GenerarPDF(DataTable dataTable, string filePath)
        {
            if (dataTable.Rows.Count > 0)
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    Document pdfDoc = new Document(PageSize.A4);

                    // Configurar márgenes (izquierda, derecha, arriba, abajo)
                    pdfDoc.SetMargins(36, 36, 72, 72); // Ajusta estos valores según tus necesidades

                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);

                    // Agregar el encabezado y pie de página
                    writer.PageEvent = new PDFHeaderFooter();

                    pdfDoc.Open();
                    BaseColor customColor = new BaseColor(16, 135, 52);
                    Font fontTitle = FontFactory.GetFont(FontFactory.HELVETICA, 16, Font.BOLD, customColor);

                    string fechaFormateada = DateTime.Parse(dataTable.Rows[0]["fecha"].ToString()).ToString("yyyy/MM/dd");

                    // Agregar título
                    Paragraph title = new Paragraph($"Reporte de Datos - {fechaFormateada}", fontTitle)
                    {
                        Alignment = Element.ALIGN_CENTER,
                    };
                    pdfDoc.Add(title);
                    pdfDoc.Add(new Paragraph(" ")); // Agregar una línea vacía

                    // Agregar promedios de oxígeno y temperatura
                    Font font = FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, BaseColor.BLACK);
                    Paragraph paragraph1 = new Paragraph($"oxígeno: {dataTable.Rows[0]["oxigeno"]}", font);
                    Paragraph paragraph2 = new Paragraph($"temperatura: {dataTable.Rows[0]["temperatura"]}", font);
                    paragraph1.Alignment = Element.ALIGN_CENTER;
                    paragraph2.Alignment = Element.ALIGN_CENTER;
                    pdfDoc.Add(paragraph1);
                    pdfDoc.Add(paragraph2);

                    pdfDoc.Add(new Paragraph(" ")); // Agregar una línea vacía

                    // Crear tabla con los datos
                    PdfPTable pdfTable = new PdfPTable(dataTable.Columns.Count);

                    // Agregar encabezados
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(column.ColumnName))
                        {
                            BackgroundColor = new BaseColor(255, 165, 0), // Color naranja
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        };
                        pdfTable.AddCell(cell);
                    }

                    // Agregar filas de datos
                    foreach (DataRow row in dataTable.Rows)
                    {
                        foreach (var cell in row.ItemArray)
                        {
                            pdfTable.AddCell(cell.ToString());
                        }
                    }
                    pdfDoc.Add(pdfTable);

                    // Si necesitas agregar gráficos u otros elementos, aquí puedes hacerlo

                    pdfDoc.Close();
                }

                MessageBox.Show("PDF generado correctamente en: " + filePath);
            }
            else
            {
                MessageBox.Show("No hay datos para generar el PDF.");
            }
        }

        // Clase para el encabezado y pie de página personalizados
        public class PDFHeaderFooter : PdfPageEventHelper
        {
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                // Crear el encabezado con una altura de 80 píxeles
                PdfPTable headerTable = new PdfPTable(2);
                headerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;

                // Agregar imagen al encabezado
                string headerImagePath = @"C:\Users\brayan\Downloads\Sena\Imagenes\logo.png";
                Image headerImage = Image.GetInstance(headerImagePath);
                headerImage.ScaleAbsolute(80, 60);
                PdfPCell imageCell = new PdfPCell(headerImage)
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_MIDDLE // Alinear verticalmente al medio
                };
                headerTable.AddCell(imageCell);

                // Agregar texto al encabezado
                PdfPCell textCell = new PdfPCell(new Phrase("Produccion Agropecuaria Ecologica"))
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    VerticalAlignment = Element.ALIGN_MIDDLE // Alinear verticalmente al medio
                };
                headerTable.AddCell(textCell);

                // Ajustar la posición del encabezado
                headerTable.WriteSelectedRows(0, -1, document.LeftMargin, writer.PageSize.GetTop(document.TopMargin) + 60, writer.DirectContent);

                // Crear el pie de página con una altura de 80 píxeles
                PdfPTable footerTable = new PdfPTable(1);
                footerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;

                // Agregar imagen al pie de página
                string footerImagePath = @"C:\Users\brayan\Downloads\Sena\Imagenes\footer_image.png";
                Image footerImage = Image.GetInstance(footerImagePath);
                footerImage.ScaleAbsolute(159, 40);
                PdfPCell footerImageCell = new PdfPCell(footerImage)
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE // Alinear verticalmente al medio
                };
                footerTable.AddCell(footerImageCell);

                // Ajustar la posición del pie de página
                footerTable.WriteSelectedRows(0, -1, document.LeftMargin, writer.PageSize.GetBottom(document.BottomMargin) + 0, writer.DirectContent);
            }
        }

        public void FiltrarPorFechas(DateTimePicker inicio, DateTimePicker fin, DataGridView dataGridView)
        {
            try
            {
                // Abrir la conexión a la base de datos
                CConexion cConexion = new CConexion();
                MySqlConnection connection = cConexion.establecerConexion();

                // Construir la consulta con parámetros para evitar SQL injection
                String query = "SELECT fecha, ROUND(AVG(oxigeno), 2) AS promedio_oxigeno, ROUND(AVG(temperatura), 2) AS promedio_temperatura FROM lecturas WHERE fecha BETWEEN @inicio AND @fin GROUP BY fecha";

                // Crear el comando SQL y asignar los parámetros
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@inicio", inicio.Value.Date);
                command.Parameters.AddWithValue("@fin", fin.Value.Date);

                // Ejecutar la consulta y llenar un DataTable con los resultados
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Asignar el DataTable al DataGridView
                dataGridView.DataSource = dataTable;

                // Cerrar la conexión a la base de datos
                cConexion.cerrarConexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se mostraron datos. Error: " + ex.ToString());
            }
        }

        //lisar el nombre de los lagos
        public void obtenerNombreDeLagos(System.Windows.Forms.ComboBox comboBox)
        {
            try
            {
                CConexion conexion = new CConexion();
                String query = "SELECT id, name FROM lagos";

             

                // Ejecutamos la consulta
                MySqlCommand command = new MySqlCommand(query, conexion.establecerConexion());
                MySqlDataReader reader = command.ExecuteReader();

                // Limpiamos los elementos existentes en el ComboBox
                comboBox.Items.Clear();

                // Llenamos el ComboBox con los resultados de la consulta
                while (reader.Read())
                {
                    comboBox.Items.Add(new ComboBoxItem(reader["name"].ToString(), reader["id"].ToString()));
                }
                if (comboBox.Items.Count > 0)
                {
                    comboBox.SelectedIndex = 0; // Selecciona el primer elemento del ComboBox
                }

                // Cerramos la conexión y el lector
                reader.Close();
                conexion.cerrarConexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la consulta: " + ex.Message);
            }
        }

        public class ComboBoxItem
        {
            public string Name { get; set; }
            public string Id { get; set; }

            public ComboBoxItem(string name, string id)
            {
                Name = name;
                Id = id;
            }

            public override string ToString()
            {
                return Name; // Esto es lo que se mostrará en el ComboBox
            }
        }
        // registrar parametros Iniciales
        public void ParametrosIniciales(DateTimePicker inicio, System.Windows.Forms.TextBox densidad, System.Windows.Forms.TextBox especie, DateTimePicker finEstimado, ComboBoxItem idLago)
        {
            try
            {
                CConexion conexion = new CConexion();

                // Definir la consulta SQL
                string query = "INSERT INTO cosecha (fecha_inicio, densidad_ciembra, especie, fecha_fin_estimada, lago_id) " +
                               "VALUES (@fecha_inicio, @densidad_ciembra, @especie, @fecha_fin_estimada, @lago_id);";

                // Crear el comando SQL
                MySqlCommand command = new MySqlCommand(query, conexion.establecerConexion()); // Asumiendo que la propiedad 'Conexion' es de tipo SqlConnection

                // Añadir parámetros al comando
                command.Parameters.AddWithValue("@fecha_inicio", inicio.Value);
                command.Parameters.AddWithValue("@densidad_ciembra", densidad.Text);
                command.Parameters.AddWithValue("@especie", especie.Text);
                command.Parameters.AddWithValue("@fecha_fin_estimada", finEstimado.Value);
                command.Parameters.AddWithValue("@lago_id", idLago.Id); // Asumiendo que 'SelectedValue' es la propiedad correcta

                // Ejecutar el comando
                command.ExecuteNonQuery();

                conexion.cerrarConexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en registrar: " + ex.Message);
            }
        }

    }
}
