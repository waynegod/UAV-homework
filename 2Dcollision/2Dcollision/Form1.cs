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
//using System.Windows;
using System.Windows.Media.Media3D;//Vector3D 結構(3D向量); (於 PresentationCore.dll)

namespace _2Dcollision
{
    public struct DPoint
    {
        //public int X, Y;
        //public Point p1;
        public int X { get; set; }
        public int Y { get; set; }
        public DPoint(Point point)
        {
            X = point.X;
            Y = point.Y;
        }
        public DPoint(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

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
            DPoint[] point = new DPoint[]
            {
                new DPoint(5,5)
            };
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            graphics0 = pictureBox1.CreateGraphics();
            bitmap0 = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            graphics0 = Graphics.FromImage(bitmap0);
            graphics0.Clear(Color.SkyBlue);
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
            {new  Point (5,5)
            ,new  Point (50,50)
            ,new  Point (50,5)
            ,new  Point (5,50)
            };

            Rectangle rectangle = new Rectangle(100, 100, 200, 200);

            graphics0.FillRectangle (bule, rectangle);
            pictureBox1.Image = bitmap0;
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Console.WriteLine("" +  bitmap0.GetPixel(155, 150));
            button2.BackColor = bitmap0.GetPixel(155, 150);
            Thread ThreadA = new Thread(new ParameterizedThreadStart(Pointing));
            Point point = new Point(50, 50);
            ThreadA.Start(point);
        }

        private void Pointing(object value)
        {
            Point point0 = new Point(0, 250);
            Point point1 = (Point)value;
            bool steep = Math.Abs(point1.Y - point0.Y) > Math.Abs(point1.X -point0.X);
            if (steep)
            {
                int temp = point0.X;
                point0.X = point0.Y;
                point0.Y = temp;

                temp = point1.X;
                point1.X = point1.Y;
                point1.Y = temp;
            }
            if (point0.X > point1.X)
            {
                int temp = point0.X;
                point0.X = point1.X;
                point1.X = temp;

                temp = point0.Y;
                point0.Y = point1.Y;
                point1.Y = temp;
            }///////////
            
        }
        /*
        public void line(int x, int y, int x2, int y2, int color)
        {
            int w = x2 - x;
            int h = y2 - y;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
            int longest = Math.Abs(w);
            int shortest = Math.Abs(h);
            if (!(longest > shortest))
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            int numerator = longest >> 1;
            for (int i = 0; i <= longest; i++)
            {
                putpixel(x, y, color);
                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                }
                else
                {
                    x += dx2;
                    y += dy2;
                }
            }
        }
        */
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Pen line_White = new Pen(Color.White, 3);
            graphics0.DrawLine(line_White, 0, 250, e.X, e.Y);
            pictureBox1.Image = bitmap0;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            bitmap0.Save("123.png", System.Drawing.Imaging.ImageFormat.Png);
        }
        
    }
}
