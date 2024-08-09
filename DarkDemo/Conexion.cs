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
    public partial class Conexion : Form
    {    
        CBroker cBroker = new CBroker();
        public Conexion()
        {
            InitializeComponent();
            cBroker.MostrarBrokers(dataGridView1);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            cBroker.RegistrarBroker(textBox1,textBox2,textBox3,textBox4, dataGridView1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cBroker.LimpiarDatos(textBox1);
            cBroker.LimpiarDatos(textBox2);
            cBroker.LimpiarDatos(textBox3);
            cBroker.LimpiarDatos(textBox4);
        }

        private void Conexion_Load(object sender, EventArgs e)
        {

        }
    }
}
