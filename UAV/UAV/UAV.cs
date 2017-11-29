using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms.Markers;
using MaterialSkin;
using MaterialSkin.Controls;
using System.Diagnostics;



namespace UAV
{
    public partial class UAV : Form
    {
        Scanning scing = new Scanning();
        GMapOverlay Main_marker = new GMapOverlay("Main_marker");
        
        GMapOverlay Attack_marker = new GMapOverlay("Attack_marker");
        GMapOverlay Target_marker = new GMapOverlay("Target_marker");
        Bitmap base_icon = Bitmap.FromFile(@"picture\base.PNG") as Bitmap;
        Bitmap target = Bitmap.FromFile(@"picture\UAV_red.PNG") as Bitmap;
        Bitmap attack = Bitmap.FromFile(@"picture\UAV_white.PNG") as Bitmap;
        //新增圖片及圖層
        PointLatLng[] setpoint = new PointLatLng[20];
        public UAV()
        {
            InitializeComponent();
            
            setpoint[0].Lat = Position.home_lat;
            setpoint[0].Lng = Position.home_lng;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tabControl1.ItemSize = new Size(0, 1);//設定 tabContro 行列數
        }
        
        private void Scanning(object sender, EventArgs e)//雷達顯示
        {
            T_lat.Text = "" + Math.Round(Gmap1.Position.Lat, 4);
            T_lng.Text = "" + Math.Round(Gmap1.Position.Lng, 4);
            Point set = new Point
            {
                X = panel1.Location.X + Gmap1.Width - scing.Width,
                Y = panel1.Location.Y + tableLayoutPanel1.Height + 3
            };
            set = PointToScreen(set);
            scing.StartPosition = FormStartPosition.Manual;
            scing.Location = new Point(set.X, set.Y);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Gmap1_Load(object sender, EventArgs e)//地圖載入設定
        {
            GMaps.Instance.Mode = AccessMode.ServerAndCache;
            Gmap1.MapProvider = GMapProviders.GoogleChinaHybridMap;
            Gmap1.Position = new PointLatLng(Position.home_lat, Position.home_lng);
            Gmap1.ShowCenter = true;//中央十字
            Gmap1.MapScaleInfoEnabled = true;//比例尺
            Gmap1.CanDragMap = true;//允許製圖
            Gmap1.MarkersEnabled = true;//允許加入標籤
            Gmap1.RoutesEnabled = true;//允許加入路線
            Gmap1.TabIndex = 2;
            GMarkerGoogle base_m = new GMarkerGoogle(new PointLatLng(Position.home_lat, Position.home_lng),base_icon);
            Main_marker.Markers.Add(base_m);
            Gmap1.Overlays.Add(Main_marker);
        }

        int SET_control = 0;//功能指數
        int SET_count = 1;//計數
        private void B_target_Click(object sender, EventArgs e) { SET_control = 1; Target_marker.Markers.Clear(); }//前往目標
        private void B_route_Click(object sender, EventArgs e) { SET_control = 2; Target_marker.Markers.Clear(); }//行走路線
        private void B_back_Click(object sender, EventArgs e) { SET_control = 3; Target_marker.Markers.Clear(); }//返航
        //功能選定

        private void Gmap1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && SET_control != 0)
            {
                try
                {
                    setpoint[SET_count].Lat = Gmap1.FromLocalToLatLng(e.X, e.Y).Lat;
                    setpoint[SET_count].Lng = Gmap1.FromLocalToLatLng(e.X, e.Y).Lng;
                    GMarkerGoogle target_m = SETmarkers.GMarkerGoogle();

                    Gmap1.Overlays.Add(Target_marker);
                    switch (SET_control)
                    {
                        case 1://前往目標
                            if (SET_control != 3)
                            {
                                Target_marker.Markers.Clear();
                                target_m.Position = new PointLatLng(setpoint[SET_count].Lat, setpoint[SET_count].Lng);
                                Target_marker.Markers.Add(target_m);

                                Planning(setpoint[0], setpoint[1]);
                            }
                            break;
                        case 2://規畫路線
                            GMapOverlay routes = new GMapOverlay("routes");
                            Gmap1.Overlays.Add(routes);
                            List<PointLatLng> Point1 = new List<PointLatLng>();
                            Point1.Add(new PointLatLng(setpoint[SET_count - 1].Lat, setpoint[SET_count - 1].Lng));
                            Point1.Add(new PointLatLng(setpoint[SET_count].Lat, setpoint[SET_count].Lng));
                            GMapRoute route = new GMapRoute(Point1, "running");
                            route.Stroke = new Pen(Color.Red, 3);
                            routes.Routes.Add(route);
                            SET_count++;

                            break;
                        case 3:
                            Debug.WriteLine("{0}" + Main_marker.Id);

                            break;
                    }
                }
                catch{}
            }
        }
        async void Running()
        {

        }

        async void Planning(PointLatLng target_0 , PointLatLng target_1)
        {
            GMarkerGoogle target_A = new GMarkerGoogle(new PointLatLng(), attack);
            GMarkerGoogle MM = new GMarkerGoogle(new PointLatLng(), target);
            int con = 0;
            double x0, y0, x1, y1;
            
            x0 = Math.Round(target_0.Lat, 6) * 1000000;
            y0 = Math.Round(target_0.Lng, 6) * 1000000;
            x1 = Math.Round(target_1.Lat, 6) * 1000000;
            y1 = Math.Round(target_1.Lng, 6) * 1000000;
            
            double dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            double dy = Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            double err = (dx > dy ? dx : -dy) / 2, e2;
            Target_marker.Markers.Add(target_A);
            
            for (; ;)
            {
                if (con == 5)
                {
                    target_A.Position = new PointLatLng(x0 / 1000000, y0 / 1000000);
                    await Task.Delay(30);
                    T_label1.Text = "目標1\n" + "緯度：" + x1 / 1000000 + "\n經度：" + y1 / 1000000;
                    T_lat.Text = "" + x0 / 1000000;
                    T_lng.Text = "" + y0 / 1000000;
                    con = 0;
                }
                 //bitmap.SetPixel(x0, y0, color);
                if (x0 == x1 && y0 == y1 || SET_control == 3) break;
                e2 = err;
                if (e2 > -dx) { err -= dy; x0 += sx; }
                if (e2 < dy) { err += dx; y0 += sy; }
                con++;
            }
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            if (scing.Visible == false)
            {
                Scan_b.Text = "雷達ON";
                scing.Visible = true;
            }
            else
            {
                Scan_b.Text = "雷達OFF";
                scing.Visible = false;
            }
            scing.TopMost = true;
            scing.Opacity = 0.5;
            Scanning(sender, e);
        }
        
        private void Button1_Click(object sender, EventArgs e) { Switch_button(1); }//頁面1
        private void Button2_Click(object sender, EventArgs e) { Switch_button(2); }//頁面2
        private void Button3_Click(object sender, EventArgs e) { Switch_button(3); }//頁面3
        private void Button4_Click(object sender, EventArgs e) { Switch_button(4); }//頁面4

        private void UAV_FormClosed(object sender, FormClosedEventArgs e)//程式結束時避免卡住
        {
            Application.Exit();
        }

        private void Switch_button(int SW)//選單切換
        {
            button1.FlatAppearance.BorderColor = Color.FromArgb(55, 71, 79);
            button2.FlatAppearance.BorderColor = Color.FromArgb(55, 71, 79);
            button3.FlatAppearance.BorderColor = Color.FromArgb(55, 71, 79);
            button4.FlatAppearance.BorderColor = Color.FromArgb(55, 71, 79);
            switch (SW)
            {
                case 1:
                    button1.FlatAppearance.BorderColor = Color.Blue;
                    tabControl1.SelectedTab = tabPage1;
                    break;
                case 2:
                    button2.FlatAppearance.BorderColor = Color.Blue;
                    tabControl1.SelectedTab = tabPage2;
                    break;
                case 3:
                    button3.FlatAppearance.BorderColor = Color.Blue;
                    tabControl1.SelectedTab = tabPage3;
                    break;
                case 4:
                    button4.FlatAppearance.BorderColor = Color.Blue;
                    tabControl1.SelectedTab = tabPage4;
                    break;                
            }
        }
    }
    public class Position //原點(全域變數)
    {
        public const double home_lat = 22.649585;
        public const double home_lng = 120.328600;
    }
}
/*async void Run()
{
    double[] plat = {
        22.652225,
        22.651864,
        22.651003,
        22.650167,
        22.650369,
        22.650745,
        22.650136,
        22.648938,
        22.648448,
        22.649171};
    double[] plng = {
        120.324869,
        120.326097,
        120.327106,
        120.328163,
        120.328882,
        120.32937,
        120.329746,
        120.329585,
        120.328528,
        120.327723};
    double x0, y0, x1, y1, x2, y2;
    double dxx, sxx, dyy, syy, errr, ee2;
    x2 = Math.Round(22.649585, 5) * 100000;
    y2 = Math.Round(120.328600, 5) * 100000;
    GMarkerGoogle MM = new GMarkerGoogle(new PointLatLng(), target);
    GMarkerGoogle AA = new GMarkerGoogle(new PointLatLng(), attack);

    Gmap1.Overlays.Add(Attack_marker);
    Attack_marker.Markers.Add(MM);
    Attack_marker.Markers.Add(AA);

    for (int p = 5; p < 9; p++)
    {
        x0 = Math.Round(plat[p], 5) * 100000;
        y0 = Math.Round(plng[p], 5) * 100000;
        x1 = Math.Round(plat[p + 1], 5) * 100000;
        y1 = Math.Round(plng[p + 1], 5) * 100000;

        double dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
        double dy = Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
        double err = (dx > dy ? dx : -dy) / 2, e2;

        while (true)
        {
            if (x0 == x1 && y0 == y1) break;
            e2 = err;
            if (e2 > -dx) { err -= dy; x0 += sx; }
            if (e2 < dy) { err += dx; y0 += sy; }
            MM.Position = new PointLatLng(x0 / 100000, y0 / 100000);
            if (p >= 5)
            {   
                dxx = Math.Abs(x0 - x2);
                sxx = x2 < x0 ? 1 : -1;
                dyy = Math.Abs(y0 - y2);
                syy = y2 < y0 ? 1 : -1;
                errr = (dxx > dyy ? dxx : -dyy) / 2;
                ee2 = errr;
                if (ee2 > -dxx) { errr = dyy; x2 += sxx; }
                if (ee2 < dyy) { errr += dxx; y2 += syy; }
                AA.Position = new PointLatLng(x2 / 100000, y2 / 100000);
                if (p >= 8)
                {
                    Attack_marker.Markers.Clear();
                    Attack_marker.Markers.Add(AA);
                    await Task.Delay(1000);
                    goto AX;
                }
            }
            await Task.Delay(1);
        }

    }
    AX:
    Attack_marker.Markers.Clear();
}*/
