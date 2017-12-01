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

        private static void Swap<T>(ref T lhs, ref T rhs) { T temp; temp = lhs; lhs = rhs; rhs = temp; }

        public Form1()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

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
            SolidBrush bule = new SolidBrush(Color.Blue);

            Rectangle rectangle1 = new Rectangle(100, 100, 200, 200);
            Rectangle rectangle2 = new Rectangle(50, 50, 300, 100);
            graphics0.FillRectangle(bule, rectangle1);
            graphics0.FillRectangle(bule, rectangle2);
            pictureBox1.Image = bitmap0;
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        //private void Pointing(object value)
        private void Pointing(Point point0, Point point1)
        {
            int x0 = point0.X;
            int y0 = point0.Y;
            int x1 = point1.X;
            int y1 = point1.Y;
            int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = (dx > dy ? dx : -dy) / 2, e2;

            while(true)
            {
                plot(x0, y0);
                if (x0 == x1 && y0 == y1) break;
                e2 = err;
                if (e2 > -dx) { err -= dy; x0 += sx; }
                if (e2 < dy) { err += dx; y0 += sy; }
                if (bitmap0.GetPixel(x0 + sx, y0) == Color.FromArgb(0, 0, 255) || bitmap0.GetPixel(x0, y0 + sy) == Color.FromArgb(0, 0, 255))
                {
                    pictureBox1.Image = bitmap0;
                    Turn_bacK(x0, y0, x1, y1);

                    return;
                }
            }
            pictureBox1.Image = bitmap0;
        }

        private void Turn_bacK(int x0, int y0, int x1, int y1)
        {
            int ctrl = 0;
            Point point0;
            Point point1;
            if (bitmap0.GetPixel(x0 + 1, y0) == Color.FromArgb(0, 0, 255))
                ctrl = 1;
            if (bitmap0.GetPixel(x0, y0 + 1) == Color.FromArgb(0, 0, 255))
                ctrl = 2;
            if (bitmap0.GetPixel(x0 - 1, y0) == Color.FromArgb(0, 0, 255))
                ctrl = 3;
            if (bitmap0.GetPixel(x0, y0 - 1) == Color.FromArgb(0, 0, 255))
                ctrl = 4;
            int c = 0;
            switch (ctrl)
            {
                case 1:
                    while (bitmap0.GetPixel(x0 + 1, y0) == Color.FromArgb(0, 0, 255))
                    {
                        y0++;
                        Console.WriteLine("1 : " + x0 + "," + y0);
                    }                        
                    break;
                case 2:
                    while (bitmap0.GetPixel(x0, y0 + 1) == Color.FromArgb(0, 0, 255))
                    {
                        x0++;
                        Console.WriteLine("2 : " + x0 + "," + y0);
                    }
                    break;
                case 3:
                    while (bitmap0.GetPixel(x0 - 1, y0) == Color.FromArgb(0, 0, 255))
                    {
                        y0--;
                        Console.WriteLine("3 : " + x0 + "," + y0);
                    }
                    break;
                case 4:
                    while (bitmap0.GetPixel(x0, y0 - 1) == Color.FromArgb(0, 0, 255))
                    {
                        x0--;
                        Console.WriteLine("4 : " + x0 + "," + y0);
                    }
                    break;

            }


            point0 = new Point(x0, y0);
            point1 = new Point(x1, y1);
            Pointing(point0, point1);
        }

        private void plot(int x, int y)
        {
            bitmap0.SetPixel(x, y, Color.White);
            pictureBox1.Image = bitmap0;
        }


        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            /*Thread ThreadA = new Thread(new ParameterizedThreadStart(Pointing));
            Point point = new Point(e.X, e.Y);
            ThreadA.Start(point);*/
            Point point0 = new Point(600,400);
            Point point1 = new Point(e.X, e.Y);
            Pointing(point0, point1);

        }

        private void button5_Click(object sender, EventArgs e)
        {
            //bitmap0.Save("123.png", System.Drawing.Imaging.ImageFormat.Png);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bitmap0.SetPixel(187, 49, Color.Red);
        }
    }
}


/*
 async void Pointing(Point point0, Point point1)
        {
            int x0 = point0.X;
            int y0 = point0.Y;
            int x1 = point1.X;
            int y1 = point1.Y;
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            
            if (steep){
                Swap<int>(ref x0, ref y0);
                Swap<int>(ref x1, ref y1);}
            if (x0 > x1){
                Swap<int>(ref x0, ref x1);
                Swap<int>(ref y0, ref y1);}

            int dX = x1 - x0;
            int dY = Math.Abs(y1 - y0);
            int err = dX / 2;
            int ystep = y0 < y1 ? 1 : -1;
            int y = y0;
            for (int x = x0; x <= x1; ++x)
            {
                if (steep)
                    plot(y, x);
                else
                    plot(x, y);
                Console.WriteLine("" +  x +","+ y);

                //if (!(steep ? plot(y, x) : plot(x, y))) break;
                err = err - dY;
                if (err < 0)
                {
                    y += ystep;
                    err += dX;
                }
                await Task.Delay(1);
                pictureBox1.Image = bitmap0;
            }
     */
//https://rosettacode.org/wiki/Bitmap/Bresenham%27s_line_algorithm