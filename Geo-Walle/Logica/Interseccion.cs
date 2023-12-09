using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geo_Walle.Logica
{
    public class Interseccion
    {
        public List<Point> Puntos_Recta(Point p1, Point p2)
        {
            List<Point> result = new List<Point>();
            for (int x = p1.x; x <= p2.x; x++)
            {
                int y = ((p2.y - p1.y) / (p2.x - p1.x)) * (x - p1.x) + p1.y;
                result.Add(new Point("", x, y));
            }
            return result;
        }
        public List<Point> Puntos_Rayo(Point p1, Point p2)
        {
            List<Point> result = new List<Point>();
            double pendiente_m = (p2.y - p1.y) / (p2.x - p1.x);
            for (int x = p1.x; x <= p2.x; x++)
            {
                int y = (int)pendiente_m * (x - p2.x) + p2.y;
                result.Add(new Point("", x, y));
            }
            return result;
        }
        public List<Point> Puntos_Linea(Point p1, Point p2)
        {
            List<Point> result = new List<Point>();
            for (int x = p1.x; x <= p2.x; x++)
            {
                int y = ((p2.y - p1.y) / (p2.x - p1.x)) * (x - p1.x) + p1.y;
                result.Add(new Point("", x, y));
            }
            return result;
        }
        public List<Point> Puntos_Ciscunferencia(Point p1, Point p2, int measure)
        {
            int radio = measure;
            List<Point> result = new List<Point>();

            for (int i = 1; i < 20; i++)
            {
                int x = p1.x + radio;
                int y = p1.y;
                result.Add(new Point("", x, y));
            }

            return result;
        }
        public List<Point> Puntos_Arco(Point p1, Point p2,Point p3, int measure)
        {
            int radio = measure;
            double d = Math.Sqrt(Math.Pow(p3.x - p2.x, 2) + Math.Pow(p3.y - p2.y, 2));
            double angulo = 2 * Math.Asin(d / 2 * radio);
            double angulo1 = angulo / (30 - 1);
            List<Point> result = new List<Point>();

            for (int i = 1; i < 20; i++)
            {
                int x = p1.x + radio * (int)Math.Cos(angulo1 / 2 + i * angulo);
                int y = p1.y + radio * (int)Math.Sin(angulo1 / 2 + i * angulo);
                result.Add(new Point("", x, y));
            }

            return result;
        }
    }
}
