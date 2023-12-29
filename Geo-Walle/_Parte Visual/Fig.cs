using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Jerarquia;

namespace Geo_Walle
{ 
    public abstract class Figura
    {
        public string color;
    }
    public abstract class Figura_Sin_Punto : Figura
    {
        public PointP point1;
        public PointP point2;

        public Figura_Sin_Punto(PointP point1, PointP point2)
        {
            this.point1 = point1;
            this.point2 = point2;
        }
    }
    public class Linea : Figura_Sin_Punto
    {
        public Linea(PointP point1, PointP point2) : base(point1, point2) { }

        public virtual void Traslate(int eje_x, int eje_y)
        {
            point1.x += eje_x;
            point1.y += eje_y;
            point2.x += eje_x;
            point2.y += eje_y;
        }
    }
    public class Rayo : Figura_Sin_Punto
    {
        public Rayo(PointP point1, PointP point2) : base(point1, point2) { }

        public virtual void Traslate(int eje_x, int eje_y)
        {
            point1.x += eje_x;
            point1.y += eje_y;
            point2.x += eje_x;
            point2.y += eje_y;
        }
    }
    public class Segmento : Figura_Sin_Punto
    {
        public Segmento(PointP point1, PointP point2) : base(point1, point2) { }

        public virtual void Traslate(int eje_x, int eje_y)
        {
            point1.x += eje_x;
            point1.y += eje_y;
            point2.x += eje_x;
            point2.y += eje_y;
        }
    }
    public class Circulo : Figura_Sin_Punto
    {
        public int media;

        public Circulo(PointP point1, PointP point2, int media) : base(point1, point2)
        {
            this.media = media;
        }

        public virtual void Traslate(int eje_x, int eje_y)
        {
            point1.x += eje_x;
            point1.y += eje_y;
            point2.x += eje_x;
            point2.y += eje_y;
        }
    }
    public class Arco : Figura_Sin_Punto
    {
        public PointP point3;
        public int media;

        public Arco(PointP point1, PointP point2, PointP point3, int media) : base(point1, point2)
        {
            this.point3 = point3;
            this.media = media;
        }
        public void Traslate(int eje_x, int eje_y)
        {
            point1.x += eje_x;
            point1.y += eje_y;
            point2.x += eje_x;
            point2.y += eje_y;
            point3.x += eje_x;
            point3.y += eje_y;
        }
    }
    public class PointP : Figura
    {
        public int x;
        public int y;
        public string name;

        public PointP(string name, int x, int y)
        {
            this.name = name;
            this.x = x;
            this.y = y;
        }
        public void Traslate(int eje_x, int eje_y)
        {
            x += eje_x;
            y += eje_y;
        }
    }
}


