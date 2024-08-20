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
        public Nosotros()
        {
            InitializeComponent();
     
        }
        private void Nosotros_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel3.Visible = false;
            panel4.Visible = false;
            panel5.Visible = false;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            cBroker.LimpiarDatos(textBox2);
            cBroker.LimpiarDatos(textBox3);
            cBroker.LimpiarDatos(textBox4);
            cBroker.LimpiarDatos(textBox7);
            cBroker.LimpiarDatos(textBox8);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel3.Visible = true;
            panel1.Visible = false;
            panel4.Visible = false;
            panel5.Visible = false;
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            panel4.Visible = true;
            panel3.Visible = false;    
            panel1.Visible = false; 
            panel5.Visible = false;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            panel5.Visible = true;
            panel4.Visible = false;
            panel3.Visible = false;
            panel1.Visible = false;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            cBroker .LimpiarDatos(textBox9);
            cBroker .LimpiarDatos(textBox10);
            cBroker .LimpiarDatos(textBox11);
        }



        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {
            cBroker.LimpiarDatos(textBox13);
            cBroker.LimpiarDatos(textBox14);
            cBroker .LimpiarDatos(textBox15);
            cBroker .LimpiarDatos(textBox16);
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {
            cBroker.LimpiarDatos(textBox17);
            cBroker.LimpiarDatos(textBox18);
            cBroker.LimpiarDatos (textBox19);
            cBroker.LimpiarDatos(textBox20);
            cBroker.LimpiarDatos(textBox21);
            cBroker.LimpiarDatos(textBox22);
        }

        private void panel4_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void button16_Click(object sender, EventArgs e)
        {
            UserClass userClass = new UserClass();
            userClass.CargarUnaImgen(pictureBox1);
        }
    }
}
