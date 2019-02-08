using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    using System.Threading;

    public partial class MainForm : Form
    {

        public MainForm()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var form2 = new BackgroundWorkerTestForm())
            {
                form2.ShowDialog();
                //form2.ShowDialog();
            }
        }
    }
}
