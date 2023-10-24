﻿using ClickTix.Conexion;
using ClickTix.Modelo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClickTix
{
    internal static class Program
    {

        public static Usuario logeado;
        public static Login login ;




        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
       static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ManagerConnection.getInstance();
            if (validateConnection()== true)
            {
                Trace.WriteLine("Conexion a la base de datos establecida con exito");
                Funcion_Controller.validarAsientosaBorrar();
                Application.Run(login = new Login());

            }
            else
            {
                MessageBox.Show("La conexion a la Base de Datos no se pudo establecer, no podrá utilizar ClickTix ");
            }
        }

        private static bool validateConnection()
        {
           try 
            {
                ManagerConnection.OpenConnection();
                ManagerConnection.CloseConnection();
                return true;
            } 
            catch (Exception ex)
            {
                Trace.WriteLine("Error al conectar a la base de datos: " + ex.Message);

                return false;
            }
        }
    }
}
