using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhotoSplitter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnBrowser_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtURL.Text = ofd.FileName;

                pictureBox1.Load(ofd.FileName);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

                txtWidth.Text = pictureBox1.Image.Width.ToString();
                txtHeight.Text = pictureBox1.Image.Height.ToString();
            }
        }

        private void btnLine_Click(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
            DrawLine();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            DrawLine();
        }

        private void DrawLine()
        {
            float LineCropWidth = 0;
            float LineCropHeight = 0;

            float overlapWidth = 0;
            float overlapHeight = 0;

            if (pictureBox1.Image == null)
                return;

            if (!float.TryParse(txtLineCropW.Text, out LineCropWidth))
                return;

            if (!float.TryParse(txtLineCropH.Text, out LineCropHeight))
                return;

            if (!float.TryParse(txtOverlapW.Text, out overlapWidth))
                return;

            if (!float.TryParse(txtOverlapH.Text, out overlapHeight))
                return;

            float ImageSizeWidth = pictureBox1.Image.Width;
            float ImageSizeHeight = pictureBox1.Image.Height;

            int displayWidth = pictureBox1.Width;
            int displayHeight = pictureBox1.Height;

            float LineDisplayWidth = (displayWidth * LineCropWidth / ImageSizeWidth);
            float LineDisplayHeight = (displayHeight * LineCropHeight / ImageSizeHeight);

            float offsetWidth = overlapWidth;
            float offsetHeight = overlapHeight;

            using (Graphics g = pictureBox1.CreateGraphics())
            {
                int ix = 1;
                int offsetIx = 0;
                while (((LineDisplayHeight * ix) - (offsetHeight * offsetIx)) < displayHeight)
                {
                    float[] dashValues = { 8, 4, 8, 4 };
                    Pen pBlack = new Pen(Color.Yellow, 1);
                    pBlack.DashPattern = dashValues;

                    offsetIx = (ix - 1);
                    offsetIx = (offsetIx > 0) ? offsetIx : 0;
                    float X1 = 0;
                    float X2 = displayWidth;
                    float Y1 = (ix * LineDisplayHeight) - (offsetHeight * offsetIx);
                    float Y2 = (ix * LineDisplayHeight) - (offsetHeight * offsetIx);
                    g.DrawLine(pBlack, X1, Y1, X2, Y2);

                    Y1 = (float)((ix * LineDisplayHeight) - (offsetHeight * ix));
                    Y2 = (float)((ix * LineDisplayHeight) - (offsetHeight * ix));
                    g.DrawLine(Pens.Red, X1, Y1, X2, Y2);

                    ix++;
                }

                int iy = 1;
                int offsetIy = 0;
                while (((LineDisplayWidth * iy) - (offsetWidth * offsetIy)) < displayWidth)
                {
                    float[] dashValues = { 8, 4, 8, 4 };
                    Pen pBlack = new Pen(Color.Yellow, 1);
                    pBlack.DashPattern = dashValues;

                    offsetIy = (iy - 1);
                    offsetIy = (offsetIy > 0) ? offsetIy : 0;
                    float X1 = (iy * LineDisplayWidth) - (offsetWidth * offsetIy);
                    float X2 = (iy * LineDisplayWidth) - (offsetWidth * offsetIy);
                    float Y1 = 0;
                    float Y2 = displayHeight;
                    g.DrawLine(pBlack, X1, Y1, X2, Y2);

                    X1 = (float)((iy * LineDisplayWidth) - (offsetWidth * iy));
                    X2 = (float)((iy * LineDisplayWidth) - (offsetWidth * iy));
                    g.DrawLine(Pens.Red, X1, Y1, X2, Y2);

                    iy++;
                }
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            float LineCropWidth = 0;
            float LineCropHeight = 0;

            float overlapWidth = 0;
            float overlapHeight = 0;

            if (pictureBox1.Image == null)
                return;

            if (!float.TryParse(txtLineCropW.Text, out LineCropWidth))
                return;

            if (!float.TryParse(txtLineCropH.Text, out LineCropHeight))
                return;

            if (!float.TryParse(txtOverlapW.Text, out overlapWidth))
                return;

            if (!float.TryParse(txtOverlapH.Text, out overlapHeight))
                return;

            float ImageSizeWidth = pictureBox1.Image.Width;
            float ImageSizeHeight = pictureBox1.Image.Height;

            float offsetWidth = overlapWidth;
            float offsetHeight = overlapHeight;

            int iy = 0;
            int Count = 1;
            while (((LineCropHeight * iy) - (offsetHeight * iy)) < ImageSizeHeight)
            {
                int ix = 0;
                while (((LineCropWidth * ix) - (offsetWidth * ix)) < ImageSizeWidth) 
                {
                    Bitmap target = new Bitmap((int)LineCropWidth, (int)LineCropHeight);

                    using (Graphics g = Graphics.FromImage(target))
                    {
                        int startX = (int)(LineCropWidth * ix - offsetWidth * ix);
                        int startY = (int)(LineCropHeight * iy - offsetHeight * iy);

                        if (startX < 0) startX = 0;
                        if (startY < 0) startY = 0;

                        g.DrawImage(pictureBox1.Image,
                                    new Rectangle(0, 0, (int)LineCropWidth, (int)LineCropHeight),
                                    new Rectangle(startX, startY, (int)LineCropWidth, (int)LineCropHeight), 
                                    GraphicsUnit.Pixel);
                    }

                    target.Save("Image" + Count + ".jpg", ImageFormat.Jpeg);
                    Count++;
                    ix++;
                }
                iy++;
            }

        }
    }
}
