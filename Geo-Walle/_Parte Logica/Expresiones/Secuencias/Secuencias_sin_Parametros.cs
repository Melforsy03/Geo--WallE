using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo_Walle;
using Lexer;

namespace Jerarquia
{
    public abstract class Secuencia_SP : Secuencia
    {
    }
    public class Ramdoms : Secuencia_SP
    {
        public override object Value { get; set; }
        public override ExpressionType Type { get; set; }
        public IEnumerable<Expression> Argumentos;
        public double Count { get; set; }
        public Ramdoms()
        {

        }

        public override bool CheckSemantic(List<Errors> errors)
        {
            Type = ExpressionType.Number;
            return true;
        }

        public override object Evaluate()
        {
            Random random = new Random();
            List<double> numbers = new List<double>();
            for (int i = 0; i < 100; i++) 
            {
                double valorAleatorio = random.NextDouble(); 
                numbers.Add(valorAleatorio);
            }
            return numbers;
        }

        public override void Scope(Scope scope) { }
    }
    public class Samples : Secuencia_SP
    {
        public override object Value { get; set; }
        public override ExpressionType Type { get; set; }
        public IEnumerable<Expression> Argumentos;
        public double Count { get; set; }
        public Samples()
        {
           
        }
        public override bool CheckSemantic(List<Errors> errors)
        {
            Type = ExpressionType.Point;
            return true;
        }

        public override object Evaluate()
        {
            Random random = new Random();
            List<PointP> numbers = new List<PointP>();
            for (int i = 0; i < 100; i++)
            {
                PointSP point = new PointSP();
                PointP p = (PointP)point.Evaluate();
                numbers.Add(p);
            }
            return numbers;
        }

        public override void Scope(Scope scope) { }
    }
}
