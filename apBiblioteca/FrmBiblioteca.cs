﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace apBiblioteca
{
    public partial class FrmBiblioteca : Form
    {
        FrmLivros frmLivros;
        FrmLeitores frmLeitores;
        FrmEmprestimo frmEmprestimo;
        public FrmBiblioteca()
        {
            InitializeComponent();
        }

        private void livrosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frmLivros == null)
                frmLivros = new FrmLivros();
            frmLivros.Show();
        }
        private void leitoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frmLeitores == null)
                frmLeitores = new FrmLeitores();
            frmLeitores.Show();
        }

        private void simToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void empréstimosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frmEmprestimo == null)
                frmEmprestimo = new FrmEmprestimo();
            frmEmprestimo.Show();
        }
    }
}
