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
    public partial class Lagos : Form
    {
        CBroker cBroker = new CBroker();

        private Form1 parentForm;
        public Lagos( Form1 form1)
        {
            InitializeComponent();
            parentForm = form1;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            parentForm.MostrarAdministradores();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            parentForm.MostrarFinCosecha();
        }
    }
}
