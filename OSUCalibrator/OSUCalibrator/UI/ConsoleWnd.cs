using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OSUCalibrator
{
    public partial class ConsoleWnd : Form
    {
        public ConsoleWnd()
        {
            InitializeComponent();
        }

        private void ConsoleWnd_Load(object sender, EventArgs e)
        {

        }

        public void Write(String str)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<string>(Write), new object[] { str });
                return;
            }

            textBoxConsole.AppendText(str);
        }

        private void textBoxConsole_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
