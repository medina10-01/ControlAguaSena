using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace DarkDemo.Clases
{
    internal class ChartsControl
    {

        public void InitChart( Chart chart , int newValue = 1)
        {
            // Añade el nuevo punto al gráfico
            if (chart.Series[0].Points.Count == 60)
            {
                // Si hay 60 puntos, elimina el punto más antiguo
                chart.Series[0].Points.RemoveAt(0);
            }

            // Añade el nuevo punto al final
            chart.Series[0].Points.AddXY(chart.Series[0].Points.Count + 1, newValue);

            // Configura el rango del eje X para que muestre del 1 al 60
            chart.ChartAreas[0].AxisX.Minimum = 0;
            chart.ChartAreas[0].AxisX.Maximum = 60;

            // Ajusta los puntos en el eje X para que estén alineados desde 1 hasta el número actual de puntos
            for (int i = 0; i < chart.Series[0].Points.Count; i++)
            {
                chart.Series[0].Points[i].XValue = i + 1;
            }

            // Configura el intervalo del eje X para mostrar números en intervalos de 5
            chart.ChartAreas[0].AxisX.Interval = 5; // Intervalo de 5 unidades
            chart.ChartAreas[0].AxisX.LabelStyle.Format = "0"; // Muestra solo enteros

            // Personaliza las líneas de cuadrícula y los bordes
            var chartArea = chart.ChartAreas[0];

            // Mostrar solo las líneas del eje Y
            chartArea.AxisX.MajorGrid.Enabled = false; // Desactiva las líneas de cuadrícula del eje X
            chartArea.AxisX.MinorGrid.Enabled = false; // Desactiva las líneas de cuadrícula menores del eje X
            chartArea.AxisY.MajorGrid.Enabled = true; // Activa las líneas de cuadrícula del eje Y
            chartArea.AxisY.MinorGrid.Enabled = false; // Activa las líneas de cuadrícula menores del eje Y

            // Asegúrate de que las líneas de los ejes estén visibles
            chartArea.AxisX.LineColor = System.Drawing.Color.Black; // Color de la línea del eje X
            chartArea.AxisY.LineColor = System.Drawing.Color.Black; // Color de la línea del eje Y

            // Opcional: Ajustar la visibilidad de las líneas de los ejes
            chartArea.AxisX.LineWidth = 2; // Ancho de la línea del eje X
            chartArea.AxisY.LineWidth = 2; // Ancho de la línea del eje Y
        }
        public void UpdateCharts( Chart chart, double newValue)
        {
            // Añade el nuevo punto al gráfico
            if (chart.Series[0].Points.Count == 60)
            {
                // Si hay 60 puntos, elimina el punto más antiguo
                chart.Series[0].Points.RemoveAt(0);
            }

            // Añade el nuevo punto al final
            chart.Series[0].Points.AddXY(chart.Series[0].Points.Count + 1, newValue);

            // Configura el rango del eje X para que muestre del 1 al 60
            chart.ChartAreas[0].AxisX.Minimum = 0;
            chart.ChartAreas[0].AxisX.Maximum = 60;

            // Ajusta los puntos en el eje X para que estén alineados desde 1 hasta el número actual de puntos
            for (int i = 0; i < chart.Series[0].Points.Count; i++)
            {
                chart.Series[0].Points[i].XValue = i + 1;
            }

            // Configura el intervalo del eje X para mostrar números en intervalos de 5
            chart.ChartAreas[0].AxisX.Interval = 5; // Intervalo de 5 unidades
            chart.ChartAreas[0].AxisX.LabelStyle.Format = "0"; // Muestra solo enteros

            // Personaliza las líneas de cuadrícula y los bordes
            var chartArea = chart.ChartAreas[0];

            // Mostrar solo las líneas del eje Y
            chartArea.AxisX.MajorGrid.Enabled = false; // Desactiva las líneas de cuadrícula del eje X
            chartArea.AxisX.MinorGrid.Enabled = false; // Desactiva las líneas de cuadrícula menores del eje X
            chartArea.AxisY.MajorGrid.Enabled = true; // Activa las líneas de cuadrícula del eje Y
            chartArea.AxisY.MinorGrid.Enabled = false; // Activa las líneas de cuadrícula menores del eje Y

            // Asegúrate de que las líneas de los ejes estén visibles
            chartArea.AxisX.LineColor = System.Drawing.Color.Black; // Color de la línea del eje X
            chartArea.AxisY.LineColor = System.Drawing.Color.Black; // Color de la línea del eje Y

            // Opcional: Ajustar la visibilidad de las líneas de los ejes
            chartArea.AxisX.LineWidth = 2; // Ancho de la línea del eje X
            chartArea.AxisY.LineWidth = 2; // Ancho de la línea del eje Y
        }

        public void updateDataInformation(CircularProgressBar.CircularProgressBar circularProgressBar, double newValue)
        {
            if (circularProgressBar != null)
            {
                // Asegúrate de que el nuevo valor esté dentro del rango permitido (0-100)
                int value = (int)Math.Min(Math.Max(newValue, 0), 100);
                double valueDouble = Math.Round(Math.Min(Math.Max(newValue, 0), 100), 2);

               

                // Actualiza el valor de progreso en el CircularProgressBar
                circularProgressBar.Value = value;

                // Actualiza el texto que se muestra en el centro del CircularProgressBar
                circularProgressBar.Text = $"{valueDouble}%";
            }

        }

    }
}
