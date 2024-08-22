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
using System.Collections.Generic;

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
            controlDatos.obtenerNombreDeCosecha(comboBox2);
        }
        private void Nosotros_Load(object sender, EventArgs e)
        {

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
                // Validar que textBox1 contiene solo números
                if (!int.TryParse(textBox1.Text, out _))
                {
                    MessageBox.Show("textBox1 debe contener solo números.");
                    return;
                }

                // Validar que textBox2 contiene solo texto
                if (string.IsNullOrWhiteSpace(textBox2.Text) || !IsTextOnly(textBox2.Text))
                {
                    MessageBox.Show("textBox2 debe contener solo texto.");
                    return;
                }

                ComboBoxItem selectedItem = (ComboBoxItem)comboBox1.SelectedItem;
                MessageBox.Show(selectedItem.Id);

                ControlDatos controlDatos = new ControlDatos();
                controlDatos.ParametrosIniciales(dateTimePicker1, textBox1, textBox2, dateTimePicker2, selectedItem);
                textBox1.Text = "";
                textBox2.Text = "";
                controlDatos.obtenerNombreDeLagos(comboBox1);
            }
            else
            {
                MessageBox.Show("Ninguno de los campos puede estar vacío");
                return;
            }
        }

        private bool IsTextOnly(string text)
        {
            foreach (char c in text)
            {
                if (!char.IsLetter(c) && !char.IsWhiteSpace(c))
                {
                    return false;
                }
            }
            return true;
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
            // Validar que textBox14 contenga solo texto
            if (string.IsNullOrWhiteSpace(textBox14.Text) || !IsTextOnly(textBox14.Text))
            {
                MessageBox.Show("textBox14 debe contener solo texto.");
                return;
            }

            // Validar que los demás textBox contengan solo números
            if (!IsNumeric(textBox9.Text) || !IsNumeric(textBox7.Text) || !IsNumeric(textBox8.Text) || !IsNumeric(textBox6.Text) || !IsNumeric(textBox10.Text))
            {
                MessageBox.Show("Los campos de texto deben contener solo números.");
                return;
            }

            // Si todas las validaciones pasan, se llama al método RegistrarUnLago
            ControlDatos controlDatos1 = new ControlDatos();
            controlDatos1.RegistrarUnLago(textBox14, textBox9, textBox7, textBox8, textBox6, textBox10);
            textBox14.Text = "";
            textBox9.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox6.Text = "";
            textBox10.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        { 
            ControlDatos controlDatos = new ControlDatos();
            if (comboBox1.SelectedItem != null && textBox1 != null && textBox2 != null)
            {
                if (!IsNumeric(textBox12.Text) || !IsNumeric(textBox13.Text) || !IsNumeric(textBox15.Text) )
                {
                    MessageBox.Show("Los campos de texto deben contener solo números.");
                    return;
                }
                var selectedItem = (KeyValuePair<string, string>)comboBox2.SelectedItem;
                string idSeleccionado = selectedItem.Value; // Aquí obtienes el id seleccionado
                MessageBox.Show(idSeleccionado);
               
                controlDatos.FinalizarCosecha(idSeleccionado, textBox12, textBox13, textBox15);
                 controlDatos.obtenerNombreDeCosecha(comboBox2);
                textBox12.Text = "";
                textBox13.Text = "";
                textBox15.Text = "";
            }
            else
            {
                MessageBox.Show("Ninguno de los campos puede estar vacío");
                return;
            }  
        }
        private bool IsNumeric(string text)
        {
            return int.TryParse(text, out _);
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new System.Net.Mail.MailAddress(email);
                return mailAddress.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (pictureBox2 != null && textBox3 != null && textBox4 != null && textBox5 != null)
            {
                // Validar que textBox5 contenga solo texto
                if (string.IsNullOrWhiteSpace(textBox5.Text) || !IsTextOnly(textBox5.Text))
                {
                    MessageBox.Show("Nombre debe contener solo texto.");
                    return;
                }

                // Validar que textBox3 contenga solo números
                if (int.TryParse(textBox3.Text, out _))
                {
                    MessageBox.Show("Telefono debe contener solo números.");
                    return;
                }

                // Validar que textBox4 contenga un correo electrónico válido
                if (!IsValidEmail(textBox4.Text))
                {
                    MessageBox.Show("El correo electrónico no es válido.");
                    return;
                }

                // Llamada al método RegistrarResponsable si todas las validaciones pasan
                ControlDatos controlDatos1 = new ControlDatos();
                controlDatos1.RegistrarResponsable(pictureBox2, textBox5, textBox4, textBox3);
                textBox5.Text = "";
                textBox4.Text = "";
                textBox3.Text = "";
                pictureBox2.Image= null;
            }
            else
            {
                MessageBox.Show("Los campos: nombre, imagen, correo y telefono son necesarios.");
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox5.Text = "";
            textBox4.Text = "";
            textBox3.Text = "";
            pictureBox2.Image = null;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            textBox14.Text = "";
            textBox9.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox6.Text = "";
            textBox10.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox12.Text = "";
            textBox13.Text = "";
            textBox15.Text = "";
        }
    }
}
