using System;
using System.Drawing;
using System.Windows.Forms;
using DarkDemo.Clases;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;

namespace DarkDemo
{
    public partial class Nosotros : Form
    { 
        CBroker cBroker = new CBroker();

        private Form1 parentForm;
        public Nosotros(Form1 form1)
        {
            InitializeComponent();

            parentForm = form1;
        }
        private void Nosotros_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
       
        }

        private void button9_Click(object sender, EventArgs e)
        {
            
            cBroker.LimpiarDatos(textBox3);
            cBroker.LimpiarDatos(textBox4);
            cBroker.LimpiarDatos(textBox7);
            cBroker.LimpiarDatos(textBox8);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            parentForm.MostrarAdministradores();
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            parentForm.MostrarLagos();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            parentForm.MostrarFinCosecha();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }


        private void button16_Click(object sender, EventArgs e)
        {
            UserClass userClass = new UserClass();
           // userClass.CargarUnaImgen(pictureBox1);
        }
    }
}
