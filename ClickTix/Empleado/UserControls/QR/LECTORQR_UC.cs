﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using AForge.Video.DirectShow;
using AForge.Video;

using System.Windows.Forms;
using System.Timers;
using ZXing;
using AForge.Controls;
using iTextSharp.text.xml;
using System.Windows.Forms.VisualStyles;
using System.Diagnostics;
using ClickTix.Modelo;
using MySqlX.XDevAPI.Common;
using MySql.Data.MySqlClient;
using ClickTix.Conexion;

namespace ClickTix.Empleado.UserControls
{
    public partial class LECTORQR_UC : UserControl
    {

        FilterInfoCollection dispositivos;
        VideoCaptureDevice fuenteVideo;




        public LECTORQR_UC()
        {
            InitializeComponent();
        }

        private void LECTORQR_UC_Load(object sender, EventArgs e)
        {
            dispositivos = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if (dispositivos.Count>0)
            {
                foreach (FilterInfo dispositivo in dispositivos)
                {
                    comboBox1.Items.Add(dispositivo.Name);
                }
                comboBox1.SelectedIndex = 0;
            }
            else
            {
                comboBox1.Items.Clear();
                comboBox1.Enabled = false;
                pictureBox1.Enabled = false;
                button1.Enabled = false;
                error.Visible = true;

            }


        }


        private void title_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

 

        private void LECTORQR_UC_Leave(object sender, EventArgs e)
        {
            if (fuenteVideo != null)
            {
                if (fuenteVideo.IsRunning)
                {
                    fuenteVideo.Stop();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fuenteVideo = new VideoCaptureDevice(dispositivos[comboBox1.SelectedIndex].MonikerString);
            fuenteVideo.NewFrame += CaptureDevice_NewFrame;
            fuenteVideo.Start(); 
            timer1.Start();
        }

        private void CaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void LECTORQR_UC_ControlRemoved(object sender, ControlEventArgs e)
        {
            if (fuenteVideo != null)
            {
                if (fuenteVideo.IsRunning)
                {
                    fuenteVideo.Stop();
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                BarcodeReader barcodeReader = new BarcodeReader();
                ZXing.Result result = barcodeReader.Decode((Bitmap)pictureBox1.Image);
                if (result != null)
                {
                    timer1.Stop();
                    textBox1.Text = result.ToString();
                    int idTicket = int.Parse(textBox1.Text);
                    if (fuenteVideo.IsRunning)
                    {
                        fuenteVideo.Stop();
                    }
                    Trace.WriteLine("EL ID TICKET ES:" + idTicket);
                    TICKET_UC ticket = new TICKET_UC(idTicket);
                    Index_User.addUserControlUsuario(ticket);

                 
                }

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fuenteVideo != null)
            {
                if (fuenteVideo.IsRunning)
                {
                    fuenteVideo.Stop();
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("El campo donde se ingresa el Nro de Ticket está vacío, por favor ingrese un valor.");
            }
            else if (!int.TryParse(textBox2.Text, out int idTicket))
            {
                MessageBox.Show("El valor ingresado no es un número, por favor ingrese un valor del tipo numerico.");
            }

            else
            {
                int idTicketInput = int.Parse(textBox2.Text);
                if (validarExistenciaTicket(idTicketInput))
                {
                    Trace.WriteLine("EL ID TICKET ES: " + idTicketInput);
                    TICKET_UC ticket = new TICKET_UC(idTicketInput);
                    Index_User.addUserControlUsuario(ticket);
                }
                else
                {
                    MessageBox.Show("No se encontró un ticket con ese Nro de Ticket.");
                }

            }
        }

        private bool validarExistenciaTicket(int id) {

                try
                {
                    ManagerConnection.OpenConnection();

                    string query = "SELECT id FROM ticket where id=@id";

                    MySqlCommand command = new MySqlCommand(query, ManagerConnection.getInstance());

                    command.Parameters.AddWithValue("@id", id);

                    object resultado = command.ExecuteScalar();

                    if (resultado == null || resultado == DBNull.Value)
                    {
                    ManagerConnection.CloseConnection();
                    return false;
                    }
                ManagerConnection.CloseConnection();
                return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                ManagerConnection.CloseConnection();
                return false;
                }
            
        }

        private void back_pelicula_Click(object sender, EventArgs e)
        {
            MENU_UC menuUser = new MENU_UC();
            Index_User.addUserControlUsuario(menuUser);
        }
    }
}
