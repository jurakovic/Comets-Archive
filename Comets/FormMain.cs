﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Comets
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void menuItemImport_Click(object sender, EventArgs e)
        {
            FormImport formImport = new FormImport();
            formImport.ShowDialog();
        }
    }
}
