﻿using ClickTix.Empleado.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClickTix.Empleado
{
    public partial class Index_User : Form
    {
        public Index_User()
        {
            InitializeComponent();
        }

        private void Index_User_Load(object sender, EventArgs e)
        {
            BUTACAS_UC butacas = new BUTACAS_UC();
            Index_User.addUserControl(butacas);
        }

        public static void addUserControl(UserControl uc)
        {
            uc.Dock = DockStyle.Fill;
            panel2.Controls.Clear();
            panel2.Controls.Add(uc);
            uc.BringToFront();
        }
    }
}