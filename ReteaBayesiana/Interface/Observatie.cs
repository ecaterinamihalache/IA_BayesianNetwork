using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Interface
{
    public partial class Observatie : Form
    {
        public Observatie()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
