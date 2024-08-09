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
    public partial class MailSender : Form
    {
        public MailSender()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {
            CBroker cBroker = new CBroker();
            cBroker.LimpiarDatos(textBox1);
            cBroker.LimpiarDatos(textBox2);
            cBroker.LimpiarDatos(textBox3);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            sendMail sendMail = new sendMail();
            sendMail.SendMail(textBox1,textBox2,textBox3);
        }
    }
}
