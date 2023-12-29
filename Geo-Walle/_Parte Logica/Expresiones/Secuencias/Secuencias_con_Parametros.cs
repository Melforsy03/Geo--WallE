using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lexer;

namespace Jerarquia
{
    public abstract class Secuencia_CP : Secuencia
    {
    }
    public class Count : Expression
    {
        public override object Value { get; set; }
        public override ExpressionType Type { get; set; }
        public Expression figura;
        public Count(Expression expression)
        {
            figura = expression;
        }
        public override bool CheckSemantic(List<Errors> errors)
        {
            bool chequeo = figura.CheckSemantic(errors);
            if (figura.Type != ExpressionType.Secuence)
            {
                errors.Add(new Errors(ErrorCode.Semantic, "el argumento pasado no es una secuencia"));
                Type = ExpressionType.ErrorType;
                return false;
            }
            Type = ExpressionType.Number;
            return chequeo;
        }

        public override object Evaluate()
        {
           return ((Secuencia)figura).Count;
        }

        public override void Scope(Scope scope)
        {
           figura.Scope(scope);
        }
    }
    public class Points : Secuencia_CP
    {
        public override object Value { get; set; }
        public override ExpressionType Type { get; set; }
        public double Count { get; set; }
        public Expression Expression { get; set; }

        public Points(Expression Expression)
        {
            this.Expression = Expression;
        }
        public override bool CheckSemantic(List<Errors> errors)
        {
            bool chequeo = Expression.CheckSemantic(errors);
            if(Expression.Type != ExpressionType.ID && Expression.Type != ExpressionType.Line && Expression.Type != ExpressionType.Ray && Expression.Type != ExpressionType.Segment && Expression.Type != ExpressionType.Arc && Expression.Type != ExpressionType.Circle && Expression.Type != ExpressionType.Point)
            {
                Type = ExpressionType.ErrorType;
                errors.Add(new Errors(ErrorCode.Semantic, "el argumento no es una figura"));
                return false;
            }

            Type = ExpressionType.Point;
            return chequeo;
        }

        public override object Evaluate()
        {
            throw new NotImplementedException();
        }

        public override void Scope(Scope scope)
        {
            Expression.Scope(scope);
        }
    }
    public class Intersect : Secuencia_CP
    {
        public override object Value { get; set; }
        public override ExpressionType Type { get; set; }
        public double Count { get; set; }
        public List<Expression> Argumentos { get; set; }

        public Intersect(List<Expression> Argumentos)
        {
            this.Argumentos = Argumentos;
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
                if (Argumentos[i].Type != ExpressionType.ID && Argumentos[i].Type != ExpressionType.Line && Argumentos[i].Type != ExpressionType.Ray && Argumentos[i].Type != ExpressionType.Segment && Argumentos[i].Type != ExpressionType.Arc && Argumentos[i].Type != ExpressionType.Circle && Argumentos[i].Type != ExpressionType.Point)
                {
                    Type = ExpressionType.ErrorType;
                    errors.Add(new Errors(ErrorCode.Semantic, "error en los parametros que se le pasa"));
                    return false;
                }
            }
            Type = ExpressionType.Point;
            return chequeo;
        }

        public override object Evaluate()
        {
            throw new NotImplementedException();
        }

        public override void Scope(Scope scope)
        {
            for (int i = 0; i < Argumentos.Count; i++)
                Argumentos[i].Scope(scope);
        }
    }
}
