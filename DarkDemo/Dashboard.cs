using DarkDemo.Clases;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace DarkDemo
{
    public partial class Dashboard : Form
    {
        MqttClient mqttClient;
        //datos para conectarse al broker
        private string  host;
        private string puerto;
        private string topico_ox;
        private string topico_tem;

        // datos del tomados del broker
        private double oxigeno;
        private double temperatura;

 

        public Dashboard(Form1 parentForm)
        {
            InitializeComponent();
            
            ControlDatos controlDatos = new ControlDatos();
            controlDatos.ListarBrokers(comboBox1);
            timer2.Interval = 10000; //cada hora se ejecuta una ves el timer 3600000

            ChartsControl chartsControl = new ChartsControl();  
            chartsControl.InitChart(chart1);
            chartsControl.InitChart(chart2);
        }
        private void Dashboard_Load(object sender, EventArgs e)
        {

        }

        private void conectarAlBroker(string coneccion)
        {
            if (coneccion == "true")
            {
                Task.Run(() =>
                {
                    mqttClient = new MqttClient(host);
                    mqttClient.MqttMsgPublishReceived += MqttClient_MqttMsgPublishReceived;
                    mqttClient.Subscribe(new string[] { topico_ox }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
                    mqttClient.Connect("Application2");
                });
            }
            else
            {
                MessageBox.Show("Usted se a desconectado");
                mqttClient.Disconnect();
                mqttClient = null;
            }
        }

        private void MqttClient_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Message);
            var sensorData = JsonConvert.DeserializeObject<SensorData>(message);

            ChartsControl chart = new ChartsControl();  

            Invoke((MethodInvoker)(() =>
            {
                // Actualiza el primer gráfico con oxígeno disuelto
                chart.UpdateCharts(chart1, sensorData.oxigeno_disuelto);
                chart.updateDataInformation(circularProgressBar1, sensorData.oxigeno_disuelto);
                label2.Text = "Oxigeno: " + sensorData.oxigeno_disuelto.ToString("F2") + "%";
                sendMail sendMail = new sendMail();

                 if (sensorData.oxigeno_disuelto < 70)
                 {    
                     string data = sensorData.oxigeno_disuelto.ToString();
                     string topico = "Oxigeno";
                     sendMail.EnviarVariosMail(data, topico);
                 }

                // Actualiza el segundo gráfico con temperatura
                chart.UpdateCharts(chart2, sensorData.temperatura);
                chart.updateDataInformation(circularProgressBar2, sensorData.temperatura);
                label10.Text = $"Temperatura: {sensorData.temperatura:F2}°C";

               if (sensorData.temperatura < 21)
               {
                  string data = sensorData.temperatura.ToString();
                     string topico = "temperatura";
                     sendMail.EnviarVariosMail(data, topico);
               }
                //Oxigeno y temperatura como valores globales

                oxigeno = sensorData.oxigeno_disuelto;
                temperatura = sensorData.temperatura;


            }));
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox1.Text = "Conectado";
                checkBox1.ForeColor = System.Drawing.Color.Green;

                label12.Text = "Online";
                label12.ForeColor = System.Drawing.Color.Green;

                pictureBox3.Visible = false;
                pictureBox4.Visible = true;

                comboBox1.Enabled = false;

                ControlDatos controlDatos = new ControlDatos();
                controlDatos.DatosNuevaConexion(comboBox1, out host, out puerto,out topico_ox,out topico_tem);

                conectarAlBroker("true");

                timer2.Start();
            }
            else
            {
                checkBox1.Text = "Conectar";
                checkBox1.ForeColor= System.Drawing.Color.Red;

                label12.Text = "Offline";
                label12.ForeColor = System.Drawing.Color.Red;

                pictureBox3.Visible = true;
                pictureBox4.Visible = false;

                comboBox1.Enabled = true;

                conectarAlBroker("false");

                timer2.Stop();
            }
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
          //  MessageBox.Show(oxigeno.ToString(),temperatura.ToString());
          ControlDatos controlDatos = new ControlDatos();
            
            controlDatos.RealizarRegistro(oxigeno, temperatura);
        }

        private void chart1_Click(object sender, EventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
        }

        private void label12_Click(object sender, EventArgs e)
        {
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
        }



        private void panel2_Paint_1(object sender, PaintEventArgs e)
        {
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {
        }

        private void label11_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            ControlDatos controlDatos = new ControlDatos();
            controlDatos.ListarBrokers(comboBox1);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }

    public class SensorData
    {
        public double oxigeno_disuelto { get; set; }
        public double temperatura { get; set; }
    }
}
