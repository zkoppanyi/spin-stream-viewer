using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OSUCalibrator.UI
{
    public partial class TimeSelectorWnd : Form
    {
        public DateTime SelectedTime { get; private set; }

        public TimeSelectorWnd()
        {
            InitializeComponent();
            SelectedTime = DateTime.Now;
        }

        public TimeSelectorWnd(DateTime defaultTime) : this()
        {
            SelectedTime = defaultTime;
        }


        private void TimeSelectorWnd_Load(object sender, EventArgs e)
        {
            populateFromTime(SelectedTime);
        }

        private void populateFromTime(DateTime time)
        {
            txtYear.Text = time.Year.ToString();
            txtMonth.Text = time.Month.ToString();
            txtDay.Text = time.Day.ToString();
            txtHour.Text = time.Hour.ToString();
            txtMinute.Text = time.Minute.ToString();
            txtSecond.Text = time.Second.ToString() + "." + time.Millisecond.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int year = Convert.ToInt32(txtYear.Text);
                int month = Convert.ToInt32(txtMonth.Text);
                int day = Convert.ToInt32(txtDay.Text);
                int hour = Convert.ToInt32(txtHour.Text);
                int minute = Convert.ToInt32(txtMinute.Text);
                int sec = (int)Math.Floor(Convert.ToDouble(txtSecond.Text));
                int milisec = Convert.ToInt32((Convert.ToDouble(txtSecond.Text) - sec) * 1000);

                SelectedTime = new DateTime(year, month, day, hour, minute, sec, milisec);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid date/time format: " + ex.Message, "Invalid date/time format!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
