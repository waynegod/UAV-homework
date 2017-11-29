using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms.Markers;
using System.Diagnostics;

namespace UAV
{
    public class SETmarkers
    {
        
        public static GMarkerGoogle GMarkerGoogle()
        {
            GMarkerGoogle target01 = new GMarkerGoogle(new PointLatLng(), GMarkerGoogleType.blue);

            return target01;
        }

        public int abc()
        {
            int ss = 0;
            return ss;
        }
    }
}