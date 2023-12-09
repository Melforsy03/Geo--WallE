using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Geo_Walle
{
    //enum ColorFig { black, green, red, blue }
    public enum FigTye { point, segment, line, ray, circle, arc }

    public class Figuras
    {
        public FigTye figTye;
        public PointP point1;
        public PointP point2;
        Random random = new Random();

        public Figuras(PointP point1, PointP point2, FigTye figType)
        {
            this.point1 = point1;
            this.point2 = point2;
            this.figTye = figType;
        }
        public Figuras(FigTye figType)
        {
            this.figTye = figType;

            point1.x = random.Next(100);
            Thread.Sleep(100);
            point1.y = random.Next(75);
            Thread.Sleep(100);
            point2.x = random.Next(100);
            Thread.Sleep(100);
            point2.y = random.Next(75);

            //point1 = new Point("x");
            //point2 = new Point("y");
        }

        public void Traslate(int eje_x, int eje_y)
        {
            point1.x += eje_x;
            point1.y += eje_y;
            point2.x += eje_x;
            point2.y += eje_y;
        }
    }
    public class Circulo : Figuras
    {
        public int media;

        public Circulo(FigTye figType) : base(figType)
        {
            media = Media();
        }

        public Circulo(PointP point1, PointP point2, FigTye figType, int media) : base(point1, point2, figType)
        {
            this.media = media;
        }

        public int Media()
        {
            return (int)Math.Sqrt(Math.Pow(point2.x - point1.x, 2) + Math.Pow(point2.y - point1.y, 2));
        }
    }
    public class Arco : Circulo
    {
        public PointP point3;

        Random random = new Random();
        public Arco(FigTye figType) : base(figType)
        {
            point1.x = random.Next(300);
            Thread.Sleep(100);
            point1.y = random.Next(75);
            Thread.Sleep(100);
            point2.x = random.Next(500);
            Thread.Sleep(100);
            point2.y = random.Next(75);
            Thread.Sleep(100);
            point3.x = random.Next(500);
            Thread.Sleep(100);
            point3.y = random.Next(75);

            media = Media();
        }
        public Arco(PointP point1, PointP point2, PointP point3, FigTye figType, int media) : base(point1, point2, figType, media)
        {
            this.point3 = point3;
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
    public class PointP
    {
        public int x;
        public int y;
        public string name;
        Random random = new Random();
        public PointP(string name)
        {
            x = random.Next(500);
            Thread.Sleep(100);
            y = random.Next(375);

            this.name = name;
        }

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


