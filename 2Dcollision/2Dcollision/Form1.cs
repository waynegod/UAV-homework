using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace _2Dcollision
{
    public partial class Form1 : Form
    {
        Graphics graphics0;
        Bitmap bitmap0;


        public Form1()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            graphics0 = panel1.CreateGraphics();
            graphics0.Clear(Color.Black);
            bitmap0 = new Bitmap(panel1.Width, panel1.Height);
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            Draw();
        }

        public void Draw()
        {
            Pen red = new Pen(Color.Red);
            SolidBrush bule = new SolidBrush(Color.Blue);

            Point[] points = new Point[]
            {new Point (5,5)
            ,new Point (50,50)
            ,new Point (50,5)
            ,new Point (5,50)
            };

            Rectangle rectangle = new Rectangle(100, 100, 200, 200);
            
            //graphics0.FillPolygon(bule, points);
            graphics0.FillRectangle (bule, rectangle);
            
            //graphics0.DrawRectangle(red, 50, 50, 100, 100);
        }

        private void button2_Click(object sender, EventArgs e)
        {


            panel1.DrawToBitmap(bitmap0,new Rectangle(0,0,panel1.Width,panel1.Height));
            Bitmap bitmap1 = new Bitmap(panel1.Width, panel1.Height, graphics0);

            Graphics graph = Graphics.FromImage(bitmap1);

            using (Graphics g = Graphics.FromImage(bitmap0))
            {
                g.DrawLine(new Pen(Color.Red), new Point(1, 1), new Point(50, 50));
            }


            bitmap0.Save("123.png", System.Drawing.Imaging.ImageFormat.Png);
            bitmap1.Save("456.png", System.Drawing.Imaging.ImageFormat.Png);
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            Pen line_White = new Pen(Color.White, 3);
            

            graphics0.DrawLine(line_White, 0, 250, e.X, e.Y);
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            graphics0.Clear(Color.SkyBlue);
        }
    }
}
