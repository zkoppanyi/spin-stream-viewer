using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OSUCalibrator
{
    public partial class SingleImageWnd : Form
    {
        private Image image;

        public SingleImageWnd(Image image)
        {
            InitializeComponent();
            this.image = image;
            pictureBox.Image = image;
            pictureBox.ZoomToFit();
            pictureBox.MouseMove += PictureBox_MouseMove;

        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            //Point pt = pictureBox.GetScaledPoint(e.X, e.Y);
            Point pt = new Point(e.X, e.Y);
            pt = pictureBox.PointToImage(pt);
            tspPixels.Text = "(" + pt.X + ", " + pt.Y + ")";
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void ImageWnd_Load(object sender, EventArgs e)
        {

        }
    }
}
