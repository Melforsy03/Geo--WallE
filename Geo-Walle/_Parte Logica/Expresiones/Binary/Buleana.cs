using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jerarquia
{
    public class AndExpression : BinaryExpression
    {
        public AndExpression(Expression left, Expression right) : base(left, right) { }

        public override object Value { get; set; }
        public override ExpressionType Type { get; set; }

        public override object Evaluate()
        {
            Value = (left.Evaluate().ToString() == "1") && (right.Evaluate().ToString() == "1") ? 1 : 0;
            return Value;
        }
    }
    public class OrExpression : BinaryExpression
    {
        public OrExpression(Expression left, Expression right) : base(left, right) { }

        public override object Value { get; set; }
        public override ExpressionType Type { get; set; }

        public override object Evaluate()
        {
            Value = (left.Evaluate().ToString() == "1") || (right.Evaluate().ToString() == "1") ? 1 : 0;
            return Value;
        }
    }

    public class NotExpresion : UnaryExpression
    {
        public NotExpresion(Expression arg) : base(arg) { }
        public override ExpressionType Type { get; set; }

        public override object Evaluate()
        {
            return Arg.Evaluate().ToString() == "1" ? "0" : "1";
        }
    }

    public class DistintExpression : BinaryExpression
    {
        public DistintExpression(Expression left, Expression right) : base(left, right) { }

        public override object Value { get; set; }
        public override ExpressionType Type { get; set; }

        public override object Evaluate()
        {
            Value = (double)(Convert.ToInt32(right.Value) == Convert.ToInt32(left.Value) ? 1 : 0);
            return Value;
        }
    }
    public class MenorExpression : BinaryExpression
    {
        public MenorExpression(Expression left, Expression right) : base(left, right) { }

        public override object Value { get; set; }
        public override ExpressionType Type { get; set; }

        public override object Evaluate()
        {
            Value = (double)(Convert.ToInt32(left.Evaluate().ToString()) < Convert.ToInt32(right.Evaluate().ToString()) ? 1 : 0);
            return Value;
        }
    }
    public class MayorExpression : BinaryExpression
    {
        public MayorExpression(Expression left, Expression right) : base(left, right) { }

        public override object Value { get; set; }
        public override ExpressionType Type { get; set; }

        public override object Evaluate()
        {
            Value = (double)(Convert.ToInt32(left.Evaluate().ToString()) > Convert.ToInt32(right.Evaluate().ToString()) ? 1 : 0);
            return Value;
        }
    }
    public class MenorEqualExpression : BinaryExpression
    {
        public MenorEqualExpression(Expression left, Expression right) : base(left, right) { }

        public override object Value { get; set; }
        public override ExpressionType Type { get; set; }

        public override object Evaluate()
        {
            Value = (double)(Convert.ToInt32(left.Evaluate().ToString()) <= Convert.ToInt32(right.Evaluate().ToString()) ? 1 : 0);
            return Value;
        }
    }
    public class MayorEqualExpression : BinaryExpression
    {
        public MayorEqualExpression(Expression left, Expression right) : base(left, right) { }

        public override object Value { get; set; }
        public override ExpressionType Type { get; set; }

        public override object Evaluate()
        {
            Value = (double)(Convert.ToInt32(left.Evaluate().ToString()) >= Convert.ToInt32(right.Evaluate().ToString()) ? 1 : 0);
            return Value;
        }
    }
}