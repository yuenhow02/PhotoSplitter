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
                while ((LineDisplayHeight * ix) < displayHeight)
                {
                    float[] dashValues = { 8, 4, 8, 4 };
                    Pen pBlack = new Pen(Color.Gray, 1);
                    pBlack.DashPattern = dashValues;
                    g.DrawLine(pBlack,0, (ix * LineDisplayHeight), displayWidth, (ix * LineDisplayHeight));

                    g.DrawLine(Pens.Red, 0, (float)((ix * LineDisplayHeight) - offsetHeight), 
                                displayWidth, (float)((ix * LineDisplayHeight)) - offsetHeight);
                    g.DrawLine(Pens.Red, 0, (float)((ix * LineDisplayHeight) + offsetHeight), 
                                displayWidth, (float)((ix * LineDisplayHeight)) + offsetHeight);
                    ix++;
                }

                int iy = 1;
                while ((LineDisplayWidth * iy) < displayWidth)
                {
                    float[] dashValues = { 8, 4, 8, 4 };
                    Pen pBlack = new Pen(Color.Gray, 1);
                    pBlack.DashPattern = dashValues;
                    g.DrawLine(pBlack, (iy * LineDisplayWidth), 0, (iy * LineDisplayWidth), displayHeight);

                    g.DrawLine(Pens.Red, (float)((iy * LineDisplayWidth) - offsetWidth), 0,
                                (float)((iy * LineDisplayWidth) - offsetWidth), displayHeight);
                    g.DrawLine(Pens.Red, (float)((iy * LineDisplayWidth) + offsetWidth), 0,
                                (float)((iy * LineDisplayWidth) + offsetWidth), displayHeight);
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
            while ((LineCropHeight * iy) < ImageSizeHeight)
            {
                int ix = 0;
                while ((LineCropWidth * ix) < ImageSizeWidth) 
                {
                    Bitmap target = new Bitmap((int)LineCropWidth, (int)LineCropHeight);

                    using (Graphics g = Graphics.FromImage(target))
                    {
                        int startX = (int)(LineCropWidth * ix - offsetWidth);
                        int startY = (int)(LineCropHeight * iy - offsetHeight);

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
