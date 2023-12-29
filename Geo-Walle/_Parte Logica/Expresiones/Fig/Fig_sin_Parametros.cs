using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lexer;
using Geo_Walle;

namespace Jerarquia
{
    public abstract class Fig_sin_Parametros : Figuras
    {

    }
    public class PointSP : Fig_sin_Parametros
    {
        public List<Expression> Argumentos;
        public PointSP()
        {
            Argumentos = new List<Expression>();
            Random random = new Random(Guid.NewGuid().GetHashCode());
            int x = random.Next(100, 300);
            Thread.Sleep(100);
            int y = random.Next(100, 300);
            Thread.Sleep(100);
            Argumentos.Add(new Number(x));
            Argumentos.Add(new Number(y));
        }

        public override void Scope(Scope scope)
        {
            for (int i = 0; i < Argumentos.Count; i++)
            {
                Argumentos[i].Scope(scope);
            }
        }

        public override bool CheckSemantic(List<Errors> errors)
        {
            for (int i = 0; i < Argumentos.Count; i++)
                Argumentos[i].CheckSemantic(errors);

            Type = ExpressionType.Point;
            return true;
        }

        public override object Evaluate()
        {
            return new PointP("", Convert.ToInt32(Argumentos[0].Value), Convert.ToInt32(Argumentos[1].Value));
        }
    }
    public class LineSP : Fig_sin_Parametros
    {
        public List<Expression> Argumentos;

        public LineSP()
        {
            Argumentos = new List<Expression>();
            PointSP p1 = new PointSP();
            PointSP p2 = new PointSP();
            Argumentos.Add(p1);
            Argumentos.Add(p2);
        }

        public override void Scope(Scope scope)
        {
            for (int i = 0; i < Argumentos.Count; i++)
                Argumentos[i].Scope(scope);
        }

        public override bool CheckSemantic(List<Errors> errors)
        {
            for (int i = 0; i < Argumentos.Count; i++)
                Argumentos[i].CheckSemantic(errors);

            Type = ExpressionType.Line;
            return true;
        }

        public override object Evaluate()
        {
            PointP p1 = new PointP("", Convert.ToInt32(((PointSP)Argumentos[0]).Argumentos[0].Value), Convert.ToInt32(((PointSP)Argumentos[0]).Argumentos[1].Value));
            PointP p2 = new PointP("", Convert.ToInt32(((PointSP)Argumentos[1]).Argumentos[0].Value), Convert.ToInt32(((PointSP)Argumentos[1]).Argumentos[1].Value));

            return new Linea(p1, p2);
        }
    }
    public class SegmentSP : Fig_sin_Parametros
    {
        public List<Expression> Argumentos;

        public SegmentSP()
        {
            Argumentos = new List<Expression>();
            PointSP p1 = new PointSP();
            PointSP p2 = new PointSP();
            Argumentos.Add(p1);
            Argumentos.Add(p2);
        }
        public override void Scope(Scope scope)
        {
            for (int i = 0; i < Argumentos.Count; i++)
                Argumentos[i].Scope(scope);
        }

        public override bool CheckSemantic(List<Errors> errors)
        {
            for (int i = 0; i < Argumentos.Count; i++)
                Argumentos[i].CheckSemantic(errors);

            Type = ExpressionType.Point;
            return true;
        }

        public override object Evaluate()
        {
            PointP p1 = new PointP("", Convert.ToInt32(((PointSP)Argumentos[0]).Argumentos[0].Value), Convert.ToInt32(((PointSP)Argumentos[0]).Argumentos[1].Value));
            PointP p2 = new PointP("", Convert.ToInt32(((PointSP)Argumentos[1]).Argumentos[0].Value), Convert.ToInt32(((PointSP)Argumentos[1]).Argumentos[1].Value));

            return new Segmento(p1, p2);
        }
    }
    public class RaySP : Fig_sin_Parametros
    {
        public List<Expression> Argumentos;

        public RaySP()
        {
            Argumentos = new List<Expression>();
            PointSP p1 = new PointSP();
            PointSP p2 = new PointSP();
            Argumentos.Add(p1);
            Argumentos.Add(p2);
        }

        public override void Scope(Scope scope)
        {
            for (int i = 0; i < Argumentos.Count; i++)
                Argumentos[i].Scope(scope);
        }

        public override bool CheckSemantic(List<Errors> errors)
        {
            for (int i = 0; i < Argumentos.Count; i++)
                Argumentos[i].CheckSemantic(errors);

            Type = ExpressionType.Point;
            return true;
        }

        public override object Evaluate()
        {
            PointP p1 = new PointP("", Convert.ToInt32(((PointSP)Argumentos[0]).Argumentos[0].Value), Convert.ToInt32(((PointSP)Argumentos[0]).Argumentos[1].Value));
            PointP p2 = new PointP("", Convert.ToInt32(((PointSP)Argumentos[1]).Argumentos[0].Value), Convert.ToInt32(((PointSP)Argumentos[1]).Argumentos[1].Value));

            return new Rayo(p1, p2);
        }

        public class ArcSP : Fig_sin_Parametros
        {
            public List<Expression> Argumentos;

            public ArcSP()
            {
                Argumentos = new List<Expression>();
                PointSP p1 = new PointSP();
                PointSP p2 = new PointSP();
                PointSP p3 = new PointSP();
                Random random = new Random(Guid.NewGuid().GetHashCode());
                int medida = random.Next(10, 100);
                Argumentos.Add(p1);
                Argumentos.Add(p2);
                Argumentos.Add(p3);
                Argumentos.Add(new Number(medida));
            }

            public override void Scope(Scope scope)
            {
                for (int i = 0; i < Argumentos.Count; i++)
                    Argumentos[i].Scope(scope);
            }

            public override bool CheckSemantic(List<Errors> errors)
            {
                bool chequeo = false;
                for (int i = 0; i < Argumentos.Count; i++)
                {
                    chequeo = Argumentos[i].CheckSemantic(errors) && chequeo;
                }

                Type = ExpressionType.Arc;
                return chequeo;
            }

            public override object Evaluate()
            {
                PointP p1 = new PointP("", Convert.ToInt32(((PointSP)Argumentos[0]).Argumentos[0].Value), Convert.ToInt32(((PointSP)Argumentos[0]).Argumentos[1].Value));
                PointP p2 = new PointP("", Convert.ToInt32(((PointSP)Argumentos[1]).Argumentos[0].Value), Convert.ToInt32(((PointSP)Argumentos[1]).Argumentos[1].Value));
                PointP p3 = new PointP("", Convert.ToInt32(((PointSP)Argumentos[2]).Argumentos[0].Value), Convert.ToInt32(((PointSP)Argumentos[2]).Argumentos[1].Value));
                int medida = Convert.ToInt32(Argumentos[3].Value);

                return new Arco(p1, p2, p3, medida);
            }
        }
        public class CircleSP : Fig_sin_Parametros
        {
            public List<Expression> Argumentos;
            public CircleSP()
            {
                Argumentos = new List<Expression>();
                PointSP p1 = new PointSP();
                PointSP p2 = new PointSP();
                double medida = Math.Sqrt(Math.Pow(Convert.ToInt32(p2.Argumentos[0].Evaluate()) - Convert.ToInt32(p1.Argumentos[0].Evaluate()), 2) + Math.Pow(Convert.ToInt32(p2.Argumentos[1].Evaluate()) - Convert.ToInt32(p1.Argumentos[1].Evaluate()), 2));
                Argumentos.Add(p1);
                Argumentos.Add(new Number((int)medida));
                Argumentos.Add(p2);
            }

            public override void Scope(Scope scope)
            {
                for (int i = 0; i < Argumentos.Count; i++)
                    Argumentos[i].Scope(scope);
            }

            public override bool CheckSemantic(List<Errors> errors)
            {
                bool chequeo = false;
                for (int i = 0; i < Argumentos.Count; i++)
                {
                    chequeo = Argumentos[i].CheckSemantic(errors) && chequeo;
                }

                Type = ExpressionType.Circle;
                return chequeo;
            }

            public override object Evaluate()
            {
                PointP p1 = new PointP("", Convert.ToInt32(((PointSP)Argumentos[0]).Argumentos[0].Value), Convert.ToInt32(((PointSP)Argumentos[0]).Argumentos[1].Value));
                PointP p2 = new PointP("", Convert.ToInt32(((PointSP)Argumentos[2]).Argumentos[0].Value), Convert.ToInt32(((PointSP)Argumentos[2]).Argumentos[1].Value));
                int medida = Convert.ToInt32(Argumentos[1].Value);

                return new Circulo(p1, p2, medida);
            }
        }
    }
}