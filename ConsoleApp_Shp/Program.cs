using DotSpatial.Data;
using DotSpatial.Projections;
using DotSpatial.Topology;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp_Shp
{
    class Program
    {
        static void Main(string[] args)
        {
            FeatureSet featureSet = new FeatureSet(DotSpatial.Topology.FeatureType.Polygon);    //設定Shapefile幾何要素
            featureSet.Projection = ProjectionInfo.FromEpsgCode(4326);  //設定坐標投影系統
            featureSet.DataTable.Columns.Add(new System.Data.DataColumn("序號", typeof(int)));    //設定屬性表
            featureSet.DataTable.Columns.Add(new System.Data.DataColumn("名稱", typeof(string)));    //設定屬性表

            //寫入Feature
            string wkt = "POLYGON((120.501320843584 23.643673036339,120.501491708153 23.6444211261058,120.501498926896 23.6444147291785,120.501524821969 23.6443917685627,120.501717159325 23.6442212029981,120.501677888785 23.644054697763,120.501579964979 23.6436249183856,120.501320843584 23.643673036339))";
            Feature feature = WktToFeature(wkt);
            IFeature iFeature= featureSet.AddFeature(feature);
            iFeature.DataRow["序號"] = 1;
            iFeature.DataRow["名稱"] = "多邊形測試";

            featureSet.SaveAs("newFile.shp", true);
        }

        /// <summary>wkt 轉換成 Feature</summary>
        /// <param name="wkt"></param>
        /// <returns></returns>
        static Feature WktToFeature(string wkt)
        {
            Feature feature = new Feature();

            if (wkt.StartsWith("POLYGON"))
            {
                Polygon polygon = WktToPolygon(wkt);
                feature = new Feature(polygon);
            }

            return feature;
        }

        /// <summary>wkt 轉換成 POLYGON</summary>
        /// <param name="wkt">wkt格式字串</param>
        /// <returns>Polygon</returns>
        static Polygon WktToPolygon(string wkt)
        {
            string pointString_string = wkt.Replace("POLYGON((", "").Replace("))", "");
            string[] pointString_array = pointString_string.Split(',');

            List<Coordinate> coordinateList = new List<Coordinate>();
            foreach (string pointString in pointString_array)
            {
                string[] pointArr = pointString.Split(' ');
                double x = Convert.ToDouble(pointArr[0]);
                double y = Convert.ToDouble(pointArr[1]);
                Coordinate coordinate = new Coordinate(x, y);
                coordinateList.Add(coordinate);
            }
            Coordinate[] coordinates = coordinateList.ToArray();

            Polygon polygon = new Polygon(coordinates);
            return polygon;
        }
    }
}
