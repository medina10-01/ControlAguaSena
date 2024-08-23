using DarkDemo.Clases;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Windows.Forms.DataVisualization.Charting;
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
        public void RealizarRegistro(double oxigeno, double temperatura)
        {
            try
            {
                double oxigenoRound = Math.Round(oxigeno, 2);
                double temperaturaRound = Math.Round(temperatura, 2);

                CConexion cConexion = new CConexion();
                String query = "INSERT INTO lecturas (oxigeno, temperatura, fecha)" +
                    "VALUES (@oxigeno, @temperatura, @fecha);";
                MySqlCommand mySqlCommand = new MySqlCommand(query, cConexion.establecerConexion());

                mySqlCommand.Parameters.AddWithValue("@oxigeno", oxigenoRound);
                mySqlCommand.Parameters.AddWithValue("@temperatura", temperaturaRound);
                mySqlCommand.Parameters.AddWithValue("@fecha", DateTime.Now); // Añadido parámetro para la fecha

                int filasAfectadas = mySqlCommand.ExecuteNonQuery();

                cConexion.cerrarConexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar en lecturas: " + ex.Message);
            }
        }

        public void ListarBrokers(System.Windows.Forms.ComboBox comboBox)
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
                String query = "SELECT DATE(fecha) AS fecha, " +
                               "ROUND(AVG(oxigeno), 2) AS promedio_oxigeno, " +
                               "ROUND(AVG(temperatura), 2) AS promedio_temperatura " +
                               "FROM lecturas " +
                               "GROUP BY DATE(fecha) " +
                               "ORDER BY fecha DESC";

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
                if (tablaReportes.Columns["PDFToMail"] == null)
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
                            ForeColor = Color.Black
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
                MessageBox.Show("No se mostraron datos. Error: " + ex.ToString());
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
                            string asunto = "Reporte de lagos en la fecha: " + fechaFormateada;
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
                               "WHERE DATE(fecha) = @fecha ";

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

                    // Definir colores
                    BaseColor headerColor = new BaseColor(255, 255, 255); 
                    BaseColor titleColor = new BaseColor(0, 170, 0); // Azul claro para el título
                    BaseColor dataColor = BaseColor.BLACK; // Negro para los datos
                    BaseColor tableHeaderColor = new BaseColor(20, 182, 20); 
                    BaseColor tableBackgroundColor = new BaseColor(230, 230, 230); // Gris claro para el fondo de la tabla

                    // Definir fuentes
                    Font fontTitle = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16, Font.BOLD, titleColor);
                    Font fontHeader = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, Font.BOLD, BaseColor.WHITE);
                    Font fontData = FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.NORMAL, dataColor);

                    // Calcular máximo, mínimo y promedio de oxígeno y temperatura
                    double maxOxigeno = dataTable.AsEnumerable().Max(row => row.Field<double>("oxigeno"));
                    double minOxigeno = dataTable.AsEnumerable().Min(row => row.Field<double>("oxigeno"));
                    double avgOxigeno = dataTable.AsEnumerable().Average(row => row.Field<double>("oxigeno"));

                    // Capturar las fechas correspondientes al máximo y mínimo de oxígeno
                    DateTime maxFechaOxigeno = dataTable.AsEnumerable()
                        .Where(row => row.Field<double>("oxigeno") == maxOxigeno)
                        .Select(row => row.Field<DateTime>("fecha"))
                        .FirstOrDefault();
                    DateTime minFechaOxigeno = dataTable.AsEnumerable()
                        .Where(row => row.Field<double>("oxigeno") == minOxigeno)
                        .Select(row => row.Field<DateTime>("fecha"))
                        .FirstOrDefault();

                    double maxTemperatura = dataTable.AsEnumerable().Max(row => row.Field<double>("temperatura"));
                    double minTemperatura = dataTable.AsEnumerable().Min(row => row.Field<double>("temperatura"));
                    double avgTemperatura = dataTable.AsEnumerable().Average(row => row.Field<double>("temperatura"));

                    // Capturar las fechas correspondientes al máximo y mínimo de temperatura
                    DateTime maxFechaTemperatura = dataTable.AsEnumerable()
                        .Where(row => row.Field<double>("temperatura") == maxTemperatura)
                        .Select(row => row.Field<DateTime>("fecha"))
                        .FirstOrDefault();
                    DateTime minFechaTemperatura = dataTable.AsEnumerable()
                        .Where(row => row.Field<double>("temperatura") == minTemperatura)
                        .Select(row => row.Field<DateTime>("fecha"))
                        .FirstOrDefault();

                    // Formatear las fechas
                    string fechaFormateadaMaxOxigeno = maxFechaOxigeno.ToString("yyyy/MM/dd HH:mm:ss");
                    string fechaFormateadaMinOxigeno = minFechaOxigeno.ToString("yyyy/MM/dd HH:mm:ss");
                    string fechaFormateadaMaxTemperatura = maxFechaTemperatura.ToString("yyyy/MM/dd HH:mm:ss");
                    string fechaFormateadaMinTemperatura = minFechaTemperatura.ToString("yyyy/MM/dd HH:mm:ss");

                    // Crear tabla para oxígeno y temperatura
                    PdfPTable table = new PdfPTable(4); // Crear una tabla con 4 columnas

                    // Definir anchos de columna
                    float[] widths = new float[] { 2f, 2f, 2f, 2f };
                    table.SetWidths(widths);

                    // Agregar la celda del título
                    PdfPCell cellTitle = new PdfPCell(new Phrase("Datos de Oxígeno y Temperatura", fontTitle));
                    cellTitle.Colspan = 4; // Hacer que el título ocupe cuatro columnas
                    cellTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellTitle.BackgroundColor = headerColor;
                    cellTitle.Border = Rectangle.NO_BORDER;
                    table.AddCell(cellTitle);

                    // Agregar la fecha en la primera fila
                    string fechaFormateada = DateTime.Parse(dataTable.Rows[0]["fecha"].ToString()).ToString("yyyy/MM/dd");
                    PdfPCell cellFecha = new PdfPCell(new Phrase($"Fecha: {fechaFormateada}", fontData));
                    cellFecha.Colspan = 4; // Hacer que la fecha ocupe cuatro columnas
                    cellFecha.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellFecha.BackgroundColor = headerColor;
                    cellFecha.Border = Rectangle.NO_BORDER;
                    table.AddCell(cellFecha);

                    // Agregar los títulos de las columnas
                    string[] headers = { "Tipo", "Valor", "Fecha", "Promedio" };
                    foreach (string header in headers)
                    {
                        PdfPCell cellHeader = new PdfPCell(new Phrase(header, fontHeader));
                        cellHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                        cellHeader.BackgroundColor = tableHeaderColor;
                        table.AddCell(cellHeader);
                    }

                    // Agregar los datos de oxígeno
                    table.AddCell(new Phrase("Oxígeno Máx:", fontData));
                    table.AddCell(new Phrase($"{maxOxigeno}", fontData));
                    table.AddCell(new Phrase($"Fecha: {fechaFormateadaMaxOxigeno}", fontData));
                    table.AddCell(new Phrase($"Prom: {avgOxigeno}", fontData));

                    table.AddCell(new Phrase("Oxígeno Mín:", fontData));
                    table.AddCell(new Phrase($"{minOxigeno}", fontData));
                    table.AddCell(new Phrase($"Fecha: {fechaFormateadaMinOxigeno}", fontData));
                    table.AddCell(new Phrase($"", fontData));

                    // Agregar los datos de temperatura
                    table.AddCell(new Phrase("Temperatura Máx:", fontData));
                    table.AddCell(new Phrase($"{maxTemperatura}", fontData));
                    table.AddCell(new Phrase($"Fecha: {fechaFormateadaMaxTemperatura}", fontData));
                    table.AddCell(new Phrase($"Prom: {avgTemperatura}", fontData));

                    table.AddCell(new Phrase("Temperatura Mín:", fontData));
                    table.AddCell(new Phrase($"{minTemperatura}", fontData));
                    table.AddCell(new Phrase($"Fecha: {fechaFormateadaMinTemperatura}", fontData));
                    table.AddCell(new Phrase($"", fontData));

                    // Agregar la tabla al documento
                    pdfDoc.Add(table);



                    // Crear gráficos
                    string chartPath = Path.Combine(Path.GetTempPath(), "chart.png");
                    CreateChart(dataTable, chartPath);

                    Paragraph paragraph = new Paragraph();
                    pdfDoc.Add(paragraph);
                    Paragraph paragraph1 = new Paragraph();
                    pdfDoc.Add(paragraph1);
                    Paragraph paragraph2 = new Paragraph("Grafica de datos");
                    paragraph2.Alignment = Element.ALIGN_CENTER;
                    paragraph2.IndentationLeft = 20f;
                    paragraph2.IndentationRight = 30f;
                    paragraph2.SpacingAfter = 10f;
                    paragraph2.SpacingBefore = 20f;
                    pdfDoc.Add(paragraph2);
                    // Agregar gráficos al documento PDF
                    if (File.Exists(chartPath))
                    {
                        Image chartImage = Image.GetInstance(chartPath);
                        chartImage.ScaleToFit(500f, 300f); // Ajustar el tamaño de la imagen
                        chartImage.Alignment = Element.ALIGN_CENTER;
                        pdfDoc.Add(chartImage);
                    }

                    pdfDoc.Close();
                }

                MessageBox.Show("PDF generado correctamente en: " + filePath);
            }
            else
            {
                MessageBox.Show("No hay datos para generar el PDF.");
            }
        }


        private void CreateChart(DataTable dataTable, string filePath)
        {
            var chart = new Chart();
            chart.Width = 800;
            chart.Height = 600;
            var chartArea = new ChartArea();
            chart.ChartAreas.Add(chartArea);

            // Crear leyenda para mostrar el nombre de las series
            var legend = new Legend();
            chart.Legends.Add(legend);

            // Crear series para oxígeno y temperatura
            var seriesOxygen = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "Oxígeno",
                Color = Color.Green,
                ChartType = SeriesChartType.Line
            };
            var seriesTemperature = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "Temperatura",
                Color = Color.Orange,
                ChartType = SeriesChartType.Line
            };
            chart.Series.Add(seriesOxygen);
            chart.Series.Add(seriesTemperature);

            // Agregar los puntos a las series
            foreach (DataRow row in dataTable.Rows)
            {
                DateTime fecha = DateTime.Parse(row["fecha"].ToString());
                double oxigeno = Convert.ToDouble(row["oxigeno"]);
                double temperatura = Convert.ToDouble(row["temperatura"]);

                // Puedes usar el valor X como un índice si no deseas mostrar tiempos específicos
                int index = seriesOxygen.Points.Count;
                seriesOxygen.Points.Add(new System.Windows.Forms.DataVisualization.Charting.DataPoint
                {
                    XValue = index,
                    YValues = new[] { oxigeno }
                });
                seriesTemperature.Points.Add(new System.Windows.Forms.DataVisualization.Charting.DataPoint
                {
                    XValue = index,
                    YValues = new[] { temperatura }
                });
            }

            // Configurar el gráfico para mostrar un solo texto en el eje X
            chart.ChartAreas[0].AxisX.LabelStyle.Format = ""; // Deja vacío para no mostrar números
            chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
            chart.ChartAreas[0].AxisX.Title = "Datos Capturados";

            // Opcional: Configurar el gráfico para ocultar las líneas del eje X
            chart.ChartAreas[0].AxisX.LineWidth = 0;
            chart.ChartAreas[0].AxisX.MajorTickMark.Enabled = false;
            chart.ChartAreas[0].AxisX.MinorTickMark.Enabled = false;

            // Guardar el gráfico como imagen
            chart.SaveImage(filePath, ChartImageFormat.Png);
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
                PdfPCell textCell = new PdfPCell(new Phrase("producción piscícola"))
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

        public void RegistrarResponsable(PictureBox pictureBox, System.Windows.Forms.TextBox textBox, System.Windows.Forms.TextBox textBox1, System.Windows.Forms.TextBox textBox2)
        {
            try
            {
                CConexion conexion = new CConexion();
                string rutaImagen;

                // Guardar la imagen y obtener la ruta
                if (GuardarImagen(pictureBox, out rutaImagen))
                {
                    // Extraer el nombre de la imagen de la ruta completa
                    string nombreImagen = Path.GetFileName(rutaImagen);

                    // Ahora que la imagen está guardada, podemos realizar la consulta SQL
                    string query = "INSERT INTO users (name, mail, cellPhone, UserImage) " +
                                   "VALUES (@name, @mail, @cellPhone, @UserImage);";

                    using (MySqlConnection conn = conexion.establecerConexion())
                    {
                        using (MySqlCommand command = new MySqlCommand(query, conn))
                        {
                            command.Parameters.AddWithValue("@name", textBox.Text);
                            command.Parameters.AddWithValue("@mail", textBox1.Text);
                            command.Parameters.AddWithValue("@cellPhone", textBox2.Text);

                            // Guardar solo el nombre de la imagen en la base de datos
                            command.Parameters.AddWithValue("@UserImage", nombreImagen);

                            command.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Registro exitoso.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error: " + ex.Message);
            }
        }


        public bool GuardarImagen(PictureBox pictureBox, out string rutaImagen)
        {
            try
            {
                // Especificar la ruta donde se guardará la imagen
                string rutaDirectorio = @"C:\Users\brayan\Downloads\Sena\Imagenes\ImagenesDeUsuario";

                // Crear el directorio si no existe
                if (!Directory.Exists(rutaDirectorio))
                {
                    Directory.CreateDirectory(rutaDirectorio);
                }

                // Generar un nombre único para la imagen
                string nombreImagen = Guid.NewGuid().ToString() + ".jpg";
                rutaImagen = Path.Combine(rutaDirectorio, nombreImagen);

                // Guardar la imagen en el directorio
                pictureBox.Image.Save(rutaImagen, System.Drawing.Imaging.ImageFormat.Jpeg);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar la imagen: " + ex.Message);
                rutaImagen = null;
                return false;
            }
        }


        public void RegistrarUnLago(System.Windows.Forms.TextBox nombre, System.Windows.Forms.TextBox largo, System.Windows.Forms.TextBox ancho, System.Windows.Forms.TextBox area, System.Windows.Forms.TextBox profundidad, System.Windows.Forms.TextBox estimados)
        {
            try
            {
                CConexion cConexion = new CConexion();
                string query = "INSERT INTO lagos (name, largo, ancho, area, profundidad, PesEstimados)" +
                               " VALUES (@name, @largo, @ancho, @area, @profundidad, @PesEstimados);";
                MySqlCommand mySqlCommand = new MySqlCommand(query, cConexion.establecerConexion());

                // Asignar valores a los parámetros
                mySqlCommand.Parameters.AddWithValue("@name", nombre.Text);
                mySqlCommand.Parameters.AddWithValue("@largo", largo.Text);
                mySqlCommand.Parameters.AddWithValue("@ancho", ancho.Text);
                mySqlCommand.Parameters.AddWithValue("@area", area.Text);
                mySqlCommand.Parameters.AddWithValue("@profundidad", profundidad.Text);
                mySqlCommand.Parameters.AddWithValue("@PesEstimados", estimados.Text);

                // Ejecutar el comando
                mySqlCommand.ExecuteNonQuery();

                // Cerrar la conexión
                cConexion.cerrarConexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en el registro: " + ex.Message);
            }
        }

       public void FinalizarCosecha(string cosecha, System.Windows.Forms.TextBox produccion, System.Windows.Forms.TextBox valorVenta, System.Windows.Forms.TextBox totalProduccion)
        {   
            try
            {
                double produccionValue = double.Parse(produccion.Text);
                double valorVentaValue = double.Parse(valorVenta.Text);
                double totalProduccionValue = double.Parse(totalProduccion.Text);
        
                // Crear una nueva conexión
                CConexion conexion = new CConexion();
      
        
                // Intentar inactivar la cosecha
                bool cosechaInactivada = InactivarCosecha(cosecha);
        
                if (cosechaInactivada)
                {
                    // Preparar la consulta de inserción en fin_cosecha
                    String query = "INSERT INTO fin_cosecha (cosecha_id, produccionKG, valor_venta, totalProduccion, fecha_fin) " +
                        "VALUES (@cosecha_id, @produccionKG, @valor_venta, @totalProduccion, @fecha_fin);";

                    MySqlCommand mySqlCommand = new MySqlCommand(query, conexion.establecerConexion());
        
                    mySqlCommand.Parameters.AddWithValue("@cosecha_id", cosecha);
                    mySqlCommand.Parameters.AddWithValue("@produccionKG", produccionValue);
                    mySqlCommand.Parameters.AddWithValue("@valor_venta", valorVentaValue);
                    mySqlCommand.Parameters.AddWithValue("@totalProduccion", totalProduccionValue);
                    mySqlCommand.Parameters.AddWithValue("@fecha_fin", DateTime.Now);
        
                    // Ejecutar la inserción
                    mySqlCommand.ExecuteNonQuery();

                    conexion.cerrarConexion();
                    MessageBox.Show("La cosecha se ha finalizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No se pudo inactivar la cosecha.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en finalizar: " + ex.Message);
            }
           
        }
        
        public bool InactivarCosecha(string comboBoxItem)
        {
            try
            {

                CConexion conexion = new CConexion();
                String query = "UPDATE cosecha SET estado = 'Cosechado' WHERE id = @id;";
                MySqlCommand mySqlCommand = new MySqlCommand(query, conexion.establecerConexion());
        
                mySqlCommand.Parameters.AddWithValue("@id", comboBoxItem);
        
                int rowsAffected = mySqlCommand.ExecuteNonQuery();
                conexion.cerrarConexion();
                // Retornar true si se actualizó al menos una fila
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al inactivar la cosecha: " + ex.Message);
                return false;
            }
        }


        public void obtenerNombreDeCosecha(System.Windows.Forms.ComboBox comboBox)
        {
            try
            {
                CConexion conexion = new CConexion();
                string query = "SELECT id, fecha_inicio FROM cosecha WHERE estado = 'Activo'";

                // Ejecutamos la consulta
                MySqlCommand command = new MySqlCommand(query, conexion.establecerConexion());
                MySqlDataReader reader = command.ExecuteReader();

                // Limpiamos los elementos existentes en el ComboBox
                comboBox.Items.Clear();

                // Llenamos el ComboBox con los resultados de la consulta
                while (reader.Read())
                {
                    DateTime fechaInicio = Convert.ToDateTime(reader["fecha_inicio"]);
                    string fechaFormateada = fechaInicio.ToString("yyyy-MM-dd"); // Formato de la fecha

                    // Agregamos un KeyValuePair con la fecha formateada y el id
                    comboBox.Items.Add(new KeyValuePair<string, string>(fechaFormateada, reader["id"].ToString()));
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

        public void RegistrarUnaAlerta()
        {
            try
            {

            }catch (Exception ex)
            {
                MessageBox.Show("");
            }
        }

    }
}
