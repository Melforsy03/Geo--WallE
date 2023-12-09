using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Geo_Walle
{
    public partial class Inicio : Form
    {
        string[] instrucciones;
        string[] informe;

        public Inicio()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (GeoW ventana_GW = new GeoW())
                ventana_GW.ShowDialog();

        }

        private void Inicio_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string rutaPDF = @". \.GW.pdf";
            
        }

        private void btn_info_Click(object sender, EventArgs e)
        {
            Process p = new Process();
        }
    }
}
