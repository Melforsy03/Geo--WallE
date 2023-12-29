using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lexer;
using Tokenizador;
using Geo_Walle;

namespace Jerarquia
{
    public abstract class Fig_con_Parametros : Figuras
    {
    }
    public class Measure : Expression
    {
        public override object Value { get; set; }
        public override ExpressionType Type { get; set; }
        public List<Expression> Argumentos;
        public Measure(List<Expression> Argumentos)
        {
            this.Argumentos = Argumentos;
        }
        
        public override bool CheckSemantic(List<Errors> errors)
        {
            bool chequeo = true;
            for (int i = 0; i < Argumentos.Count; i++)
            {
                chequeo = Argumentos[i].CheckSemantic(errors) && chequeo;
            }
            for (int i = 0; i < 2; i++)
            {
                if (Argumentos[i].Type != ExpressionType.ID && Argumentos[i].Type != ExpressionType.Point)
                {
                    Type = ExpressionType.ErrorType;
                    errors.Add(new Errors(ErrorCode.Semantic, "error en los parametros que se le pasa a la figura"));
                    return false;
                }
            }
            Type = ExpressionType.Number;
            return chequeo;
        }

        public override object Evaluate()
        {
            Dictionary<Expression, object> Valores = new Dictionary<Expression, object>();

            foreach (var item in Argumentos)
            {
                Valores.Add(item, item.Evaluate());
            }

            int medida = Convert.ToInt32(Math.Sqrt(Math.Pow(((PointP)Valores[Argumentos[1]]).x - ((PointP)Valores[Argumentos[0]]).x, 2) + Math.Pow(((PointP)Valores[Argumentos[1]]).y - ((PointP)Valores[Argumentos[0]]).y, 2)));
            return new Number(medida);
        }

        public override void Scope(Scope scope)
        {
            for (int i = 0; i < Argumentos.Count; i++)
                Argumentos[i].Scope(scope);
        }
    }
    public class Point : Fig_con_Parametros
    {
        public List<Expression> Argumentos;
        public Dictionary<Expression, object> Valores;
        public Scope scope = new Scope(Context.General, new Dictionary<Token, object>(), new Dictionary<Token, ExpressionType>());
        public Point(List<Expression> Argumentos)
        {
            this.Argumentos = Argumentos;
        }
        public override object Evaluate()
        {
            Dictionary<Expression, object> Valores = new Dictionary<Expression, object>();
            int index = 0;

            foreach (var item in Argumentos)
            {
                Valores.Add(item, item.Evaluate());
                index++;
            }
            return new PointP("", Convert.ToInt32(Valores[Argumentos[0]]), Convert.ToInt32(Valores[Argumentos[1]]));
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
            bool chequeo = false;
            for (int i = 0; i < Argumentos.Count; i++)
            {
                chequeo = Argumentos[i].CheckSemantic(errors) && chequeo;
            }
            for (int i = 0; i < 2; i++)
            {
                if (Argumentos[i].Type != ExpressionType.Number && Argumentos[i].Type != ExpressionType.ID)
                {
                    Type = ExpressionType.ErrorType;
                    errors.Add(new Errors(ErrorCode.Semantic, "error en los parametros que se le pasa a la figura"));
                    return false;
                }
            }
            Type = ExpressionType.Point;
            return chequeo;
        }
    }
    public class Line : Fig_con_Parametros
    {
        public List<Expression> Argumentos;
        public Dictionary<Expression, object> Valores;

        public Line(List<Expression> Argumentos)
        {
            this.Argumentos = Argumentos;
        }

        public override object Evaluate()
        {
            Dictionary<Expression, object> Valores = new Dictionary<Expression, object>();

            foreach (var item in Argumentos)
            {
                Valores.Add(item, item.Evaluate());
            }

            return new Linea((PointP)Argumentos[0].Value, (PointP)Argumentos[1].Value);
        }
        public override void Scope(Scope scope)
        {
            for (int i = 0; i < Argumentos.Count; i++)
                Argumentos[i].Scope(scope);
        }

        public override bool CheckSemantic(List<Errors> errors)
        {
            bool chequeo = true;
            for (int i = 0; i < Argumentos.Count; i++)
            {
                chequeo = Argumentos[i].CheckSemantic(errors) && chequeo;
            }
            for (int i = 0; i < 2; i++)
            {
                if (Argumentos[i].Type != ExpressionType.ID && Argumentos[i].Type != ExpressionType.Point)
                {
                    Type = ExpressionType.ErrorType;
                    errors.Add(new Errors(ErrorCode.Semantic, "error en los parametros que se le pasa a la figura"));
                    return false;
                }
            }
            Type = ExpressionType.Line;
            return chequeo;
        }
    }
    public class Segment : Fig_con_Parametros
    {
        public List<Expression> Argumentos;
        public Dictionary<Expression, object> Valores;

        public Segment(List<Expression> Argumentos)
        {
            this.Argumentos = Argumentos;
        }

        public override object Evaluate()
        {
            Dictionary<Expression, object> Valores = new Dictionary<Expression, object>();
            int index = 0;

            foreach (var item in Argumentos)
            {
                Valores.Add(item, item.Evaluate());
                index++;
            }

            return new Segmento((PointP)Argumentos[0].Value, (PointP)Argumentos[1].Value);
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
            for (int i = 0; i < 2; i++)
            {
                if (Argumentos[i].Type != ExpressionType.ID && Argumentos[i].Type != ExpressionType.Point)
                {
                    Type = ExpressionType.ErrorType;
                    errors.Add(new Errors(ErrorCode.Semantic, "error en los parametros que se le pasa a la figura"));
                    return false;
                }
            }
            Type = ExpressionType.Segment;
            return chequeo;
        }
    }
    public class Ray : Fig_con_Parametros
    {
        public List<Expression> Argumentos;
        public Dictionary<Expression, object> Valores;

        public Ray(List<Expression> Argumentos)
        {
            this.Argumentos = Argumentos;
        }

        public override object Evaluate()
        {
            Dictionary<Expression, object> Valores = new Dictionary<Expression, object>();
            int index = 0;

            foreach (var item in Argumentos)
            {
                Valores.Add(item, item.Evaluate());
                index++;
            }

            return new Rayo((PointP)Argumentos[0].Value, (PointP)Argumentos[1].Value);
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
            for (int i = 0; i < 2; i++)
            {
                if (Argumentos[i].Type != ExpressionType.ID && Argumentos[i].Type != ExpressionType.Point)
                {
                    Type = ExpressionType.ErrorType;
                    errors.Add(new Errors(ErrorCode.Semantic, "error en los parametros que se le pasa a la figura"));
                    return false;
                }
            }
            Type = ExpressionType.Ray;
            return chequeo;
        }
    }

    public class Arc : Fig_con_Parametros
    {
        public List<Expression> Argumentos;
        public Dictionary<Expression, object> Valores;

        public Arc(List<Expression> Argumentos)
        {
            this.Argumentos = Argumentos;
        }

        public override object Evaluate()
        {
            Dictionary<Expression, object> Valores = new Dictionary<Expression, object>();
            int index = 0;

            foreach (var item in Argumentos)
            {
                Valores.Add(item, item.Evaluate());
                index++;
            }
            int medida = Convert.ToInt32(Valores[Argumentos[3]]);
            return new Arco((PointP)Argumentos[0].Value, (PointP)Argumentos[1].Value, (PointP)Argumentos[2].Value, medida);
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
            if (Argumentos[3].Type != ExpressionType.ID && Argumentos[3].Type != ExpressionType.Number)
            {
                Type = ExpressionType.ErrorType;
                errors.Add(new Errors(ErrorCode.Semantic, "error en los parametros que se le pasa a la figura"));
                return false;
            }
            if (Argumentos[1].Type != ExpressionType.Point)
            {
                Type = ExpressionType.ErrorType;
                errors.Add(new Errors(ErrorCode.Semantic, "error en los parametros que se le pasa a la figura"));
                return false;
            }

            Type = ExpressionType.Arc;
            return chequeo;
        }
    }
    public class Circle : Fig_con_Parametros
    {
        public List<Expression> Argumentos;
        public Dictionary<Expression, object> Valores;
        public Scope scope = new Scope(Context.General, new Dictionary<Token, object>(), new Dictionary<Token, ExpressionType>());
        public Circle(List<Expression> Argumentos)
        {
            this.Argumentos = Argumentos;
        }
        public override object Evaluate()
        {
            Dictionary<Expression, object> Valores = new Dictionary<Expression, object>();
            int index = 0;

            foreach (var item in Argumentos)
            {
                Valores.Add(item, item.Evaluate());
                index++;
            }

            int medida = Convert.ToInt32(Valores[Argumentos[1]]);
            PointP pointCentral = (PointP)Argumentos[0].Value;
            double angulo = Math.PI / 4;
            int x = Convert.ToInt32(pointCentral.x + medida * Math.Cos(angulo));
            int y = Convert.ToInt32(pointCentral.y + medida * Math.Sin(angulo));

            return new Circulo(pointCentral, new PointP("", x, y), medida);
        }

        public override void Scope(Scope scope)
        {
            for (int i = 0; i < Argumentos.Count; i++)
                Argumentos[i].Scope(scope);
            this.scope = scope;
        }

        public override bool CheckSemantic(List<Errors> errors)
        {
            bool chequeo = false;
            for (int i = 0; i < Argumentos.Count; i++)
            {
                chequeo = Argumentos[i].CheckSemantic(errors) && chequeo;
            }
            if (Argumentos[1].Type != ExpressionType.ID && Argumentos[1].Type != ExpressionType.Number)
            {
                Type = ExpressionType.ErrorType;
                errors.Add(new Errors(ErrorCode.Semantic, "error en los parametros que se le pasa a la figura"));
                return false;
            }
            if (Argumentos[0].Type != ExpressionType.Point)
            {
                Type = ExpressionType.ErrorType;
                errors.Add(new Errors(ErrorCode.Semantic, "error en los parametros que se le pasa a la figura"));
                return false;
            }

            Type = ExpressionType.Circle;
            return chequeo;
        }
    }
}
