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
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms.Markers;

namespace UAV
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Scanning()
        {
            
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Bitmap scann = new Bitmap("picture/Path.png");
            pictureBox1.Image = scann;

            /*for (int x = 0; x < scann.Width; x++)
            {
                for (int y = 0; y < scann.Height; y++)
                {
                    Color pixelColor = scann.GetPixel(x, y);
                    if (pixelColor.A == 255)
                    {
                        Color newColor = Color.FromArgb(100, pixelColor.R, pixelColor.B, pixelColor.G);
                        scann.SetPixel(x, y, newColor);
                    }
                }
            }
            pictureBox1.BackColor = Color.Transparent;*/
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Gmap1_load(object sender, EventArgs e)
        {
            Gmap1.MapProvider = GoogleChinaHybridMapProvider.Instance;
            GMaps.Instance.Mode = AccessMode.ServerAndCache;
            Gmap1.Position = new PointLatLng(22.5990179341876, 120.315544009209);
            Gmap1.ShowCenter = false;
            Gmap1.MapScaleInfoEnabled = true;
            Gmap1.CanDragMap = true;
            Gmap1.MarkersEnabled = true;
            Gmap1.RoutesEnabled = true;
            Gmap1.TabIndex = 20;
            Gmap1.GetHashCode();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
