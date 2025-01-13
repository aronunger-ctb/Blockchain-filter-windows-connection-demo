using System;
using System.Windows.Forms;

namespace BlockchainFilter
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Program.StartFiddler();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Program.StopFiddler();
        }
    }
}
