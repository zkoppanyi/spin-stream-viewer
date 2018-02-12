using SharpVelodyne;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpVelodyneTest
{
    public partial class Simple3dViewerWnd : Form
    {
        VelodyneReader reader;

        public Simple3dViewerWnd(VelodyneReader reader)
        {
            InitializeComponent();
            this.reader = reader;
            viewer.PointNumForPreview = 10000 * 2;
        }

        private void Simple3dViewer_Load(object sender, EventArgs e)
        {
            reader.ReadNextFrame();
            List<VelodynePoint> pts = reader.ReadNextFrame();
            viewer.ClearAndAddNewPoints(pts);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            List<VelodynePoint> pts = reader.ReadNextFrame();
            viewer.ClearAndAddNewPoints(pts, SimpleVelodyneViewerRenderingMode.Manual);
            viewer.RenderPreview();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer.Start();
                
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer.Stop();
            viewer.Render();
        }
    }
}
