using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DarkDemo.Clases
{
    internal class UserClass
    {
        public void MirarUsuario(PictureBox pictureBox, Label label)
        {
            try
            {
                CConexion cConexion = new CConexion();
                String query = "SELECT name, UserImage FROM users LIMIT 1";

                using (MySqlConnection connection = cConexion.establecerConexion())
                {
                    MySqlCommand mySqlCommand = new MySqlCommand(query, connection);
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string userName = reader["name"].ToString();
                            string imageName = reader["UserImage"].ToString();

                            // Especificar la ruta completa de la imagen
                            string imagePath = System.IO.Path.Combine(@"C:\Users\brayan\Downloads\Sena\Imagenes\ImagenesDeUsuario", imageName);

                            // Verificar si el archivo existe
                            if (System.IO.File.Exists(imagePath))
                            {
                                // Cargar la imagen en el PictureBox
                                pictureBox.Image = Image.FromFile(imagePath);

                                // Ajustar la imagen al tamaño del contenedor
                                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

                                // Asignar el nombre del usuario al Label
                                label.Text = userName;
                            }
                            else
                            {
                                MessageBox.Show("La imagen no se encontró en la ruta especificada.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("No se encontró ningún usuario en la base de datos.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la imagen del usuario: " + ex.Message);
            }
        }

        public void CargarUnaImgen(PictureBox pictureBox1)
        {
            // Crear y configurar el OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "C:\\";
            openFileDialog.Filter = "Image Files (*.bmp;*.jpg;*.jpeg;*.png)|*.bmp;*.jpg;*.jpeg;*.png";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            // Mostrar el diálogo y verificar si el usuario seleccionó un archivo
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Obtener la ruta del archivo seleccionado
                string filePath = openFileDialog.FileName;

                // Cargar la imagen en el PictureBox
                pictureBox1.Image = new Bitmap(filePath);
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

                // Opcional: guardar la imagen en una ubicación específica o en una base de datos
                // Ejemplo: pictureBox.Image.Save("ruta_de_guardado");
            }
        }
    }
}
