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
    public partial class PropertiesWnd : Form
    {

        public object Object { get; private set; }

        /// <summary>
        /// Does the dialog show a delete button
        /// </summary>
        public bool IsDeleteAble { get; private set; }

        /// <summary>
        /// Custom dilog result to delete the object
        /// </summary>
        public bool IsDeleteObject { get; private set; }

        public PropertiesWnd(object obj, bool isDelete)
        {
            InitializeComponent();
            this.Object = obj;
            IsDeleteAble = isDelete;
            IsDeleteObject = false;

            if (IsDeleteAble == false)
            {
                btnDelete.Visible = false;
            }
        }

        public PropertiesWnd(object obj) : this(obj, true)
        {

        }

        private void PropertiesWnd_Load(object sender, EventArgs e)
        {
            propertyGrid.SelectedObject = this.Object;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.IsDeleteObject = true;
            this.Close();
        }
    }
}
