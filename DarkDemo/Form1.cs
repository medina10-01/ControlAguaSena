using DarkDemo.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml.Linq;

namespace DarkDemo
{
    public partial class Form1 : Form
    {
        private bool mouseDown;
        private Point lastLocation;

        //formularios hijos
        private Dashboard dashboard;
        private Reportes reportes;
        private Nosotros nosotros;
        private Contactos contactos;
        private Conexion conexion;
        private MailSender mailSender;

        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(100, 100);
            InicializarFormularios();
            UserClass userClass = new UserClass();
            userClass.MirarUsuario(pictureBox4, label6);
        }

        private void InicializarFormularios()
        {
            dashboard = new Dashboard(this) { MdiParent = this, FormBorderStyle = FormBorderStyle.None, TopLevel = false, Dock = DockStyle.Fill };
            reportes = new Reportes() { MdiParent = this, FormBorderStyle = FormBorderStyle.None, TopLevel = false, Dock = DockStyle.Fill };
            nosotros = new Nosotros(this) { MdiParent = this, FormBorderStyle = FormBorderStyle.None, TopLevel = false, Dock = DockStyle.Fill };
            contactos = new Contactos() { MdiParent = this, FormBorderStyle = FormBorderStyle.None, TopLevel = false, Dock = DockStyle.Fill };
            conexion = new Conexion() { MdiParent = this, FormBorderStyle = FormBorderStyle.None, TopLevel = false, Dock = DockStyle.Fill };
            mailSender = new MailSender() { MdiParent = this, FormBorderStyle= FormBorderStyle.None, TopLevel= false, Dock = DockStyle.Fill };
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            panel3.MouseDown += inicio_MouseDown;
            panel3.MouseMove += inicio_MouseMove;
            panel3.MouseUp += inicio_MouseUp;
        }

        private void inicio_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDown = true;
                lastLocation = e.Location;
            }
        }
        // Evento para mover el formulario mientras se arrastra
        private void inicio_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                Point delta = Point.Subtract(Cursor.Position, new Size(lastLocation));
                this.Location = Point.Add(this.Location, new Size(delta));
            }
        }

        private void inicio_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDown = false;
            }
        }
       

        private void pictureBox1_Click_2(object sender, EventArgs e)
        {
            
        }


        private void MostrarFormulario(Form formulario)
        {
            toolStripContainer1.ContentPanel.Controls.Clear(); // Limpiar el ContentPanel

            foreach (Form frm in this.MdiChildren)
            {
                frm.Hide(); // Ocultar todos los formularios hijos
            }

            toolStripContainer1.ContentPanel.Controls.Add(formulario); // Agregar el formulario deseado
            formulario.Show(); // Mostrar el formulario deseado
        }

        private void button9_Click(object sender, EventArgs e)
        {
            MostrarFormulario(dashboard);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MostrarFormulario(nosotros);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MostrarFormulario(reportes);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MostrarFormulario(contactos);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MostrarFormulario(conexion);
        }
        
        private void button1_Click_1(object sender, EventArgs e)
        {
            MostrarFormulario(mailSender);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

         private void toolStripContainer1_ContentPanel_Load(object sender, EventArgs e)
        {
            
            button9_Click(sender, e);
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            this.MouseDown += inicio_MouseDown;
            this.MouseMove += inicio_MouseMove;
            this.MouseUp += inicio_MouseUp;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
