using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lexer;
namespace Jerarquia
{
    public abstract class UnaryExpression : Expression
    {
        public override ExpressionType Type { get; set; }
        public override object Value { get; set; }
        public Expression Arg { get; set; }

        public UnaryExpression(Expression arg)
        {
            Arg = arg;
        }
        public override void Scope(Scope scope)
        {
            Arg.Scope(scope);
        }

        public override bool CheckSemantic(List<Errors> errors)
        {
            bool arg = Arg.CheckSemantic(errors);
            if (Arg.Type != ExpressionType.Number)
            {
                errors.Add(new Errors(ErrorCode.Semantic, ""));
                Type = ExpressionType.ErrorType;
                return false;
            }

            Type = ExpressionType.Number;
            return arg;
        }
    }
    public class Sen : UnaryExpression
    {
        public Sen(Expression arg) : base(arg) { }

        public override object Evaluate()
        {
            Value = Math.Sin(double.Parse(Arg.Evaluate().ToString()));
            return Value;
        }
    }
    public class Cos : UnaryExpression
    {
        public Cos(Expression arg) : base(arg) { }

        public override object Evaluate()
        {
            Value = Math.Cos(double.Parse(Arg.Evaluate().ToString()));
            return Value;
        }
    }
    public class Tan : UnaryExpression
    {
        public Tan(Expression arg) : base(arg) { }

        public override object Evaluate()
        {
            Value = Math.Tan(double.Parse(Arg.Evaluate().ToString()));
            return Value;
        }
    }
    public class Log : UnaryExpression
    {
        public Log(Expression arg) : base(arg) { }

        public override object Evaluate()
        {
            Value = Math.Log(double.Parse(Arg.Evaluate().ToString()));
            return Value;
        }
    }
    public class Squart : UnaryExpression
    {
        public Squart(Expression arg) : base(arg) { }

        public override object Evaluate()
        {
            Value = Math.Sqrt(double.Parse(Arg.Evaluate().ToString()));
            return Value;
        }
    }
}