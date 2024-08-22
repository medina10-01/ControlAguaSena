using System;
using System.Drawing;
using System.Windows.Forms;
using DarkDemo.Clases;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using static DarkDemo.ControlDatos;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
            ControlDatos controlDatos = new ControlDatos();
            controlDatos.obtenerNombreDeLagos(comboBox1);
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

      // private void button8_Click(object sender, EventArgs e)
      // {
      //     if (comboBox1.SelectedItem != null && textBox4 != null && textBox3 != null)
      //     {
      //         ComboBoxItem selectedItem = (ComboBoxItem)comboBox1.SelectedItem;
      //         MessageBox.Show(selectedItem.Id);
      //
      //         ControlDatos controlDatos = new ControlDatos();
      //         controlDatos.ParametrosIniciales(dateTimePicker1, textBox4, textBox3, dateTimePicker2, selectedItem);
      //     }
      //     else {
      //         MessageBox.Show("Ninguno de los campos puede estar vacío");
      //         return;
      //     }
      // }

        private void Parametros_Click(object sender, EventArgs e)
        {
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null && textBox1 != null && textBox2 != null)
            {
                ComboBoxItem selectedItem = (ComboBoxItem)comboBox1.SelectedItem;
                MessageBox.Show(selectedItem.Id);
                            ControlDatos controlDatos = new ControlDatos();
                controlDatos.ParametrosIniciales(dateTimePicker1, textBox1, textBox2, dateTimePicker2, selectedItem);
            }
            else {
                MessageBox.Show("Ninguno de los campos puede estar vacío");
                return;
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            UserClass userClass = new UserClass();
            userClass.CargarUnaImgen(pictureBox2);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            ControlDatos controlDatos1 = new ControlDatos();
            controlDatos1.RegistrarUnLago(textBox14,textBox9,textBox7,textBox8, textBox6,textBox10);
        }
    }
}
