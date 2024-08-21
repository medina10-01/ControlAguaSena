using DarkDemo.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DarkDemo
{
    public partial class Administradores : Form
    {
        CBroker cBroker = new CBroker();

        private Form1 parentForm;
        public Administradores(Form1 form1)
        {
            InitializeComponent();
            parentForm = form1;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }


        private void button3_Click(object sender, EventArgs e)
        {
            parentForm.MostrarLagos();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            parentForm.MostrarFinCosecha();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            parentForm.MostrarReportes();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            UserClass userClass = new UserClass();
            userClass.CargarUnaImgen(pictureBox1);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            ControlDatos controlDatos = new ControlDatos();
            controlDatos.RegistrarResponsable(pictureBox1, textBox9, textBox11, textBox10);
        }
    }
}
