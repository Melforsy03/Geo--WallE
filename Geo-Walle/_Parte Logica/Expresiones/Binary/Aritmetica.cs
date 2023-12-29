using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jerarquia
{
    public class Add : BinaryExpression
    {
        public override object Value { get; set; }
        public override ExpressionType Type { get; set; }
        public Add(Expression left, Expression right) : base(left, right) { }
        public override object Evaluate()
        {
            Value = (double)(Convert.ToInt32(left.Evaluate()) + Convert.ToInt32(right.Evaluate()));
            return Value;
        }
    }
    public class Sub : BinaryExpression
    {
        public override object Value { get; set; }
        public override ExpressionType Type { get; set; }
        public Sub(Expression left, Expression right) : base(left, right) { }
        public override object Evaluate()
        {
            Value = (double)(Convert.ToInt32(left.Evaluate()) - Convert.ToInt32(right.Evaluate()));
            return Value;
        }
    }
    public class Start : BinaryExpression
    {
        public override object Value { get; set; }
        public override ExpressionType Type { get; set; }
        public Start(Expression left, Expression right) : base(left, right) { }
        public override object Evaluate()
        {
            Value = (double)(Convert.ToInt32(left.Evaluate()) * Convert.ToInt32(right.Evaluate()));
            return Value;
        }

    }
    public class Div : BinaryExpression
    {
        public override object Value { get; set; }
        public override ExpressionType Type { get; set; }
        public Div(Expression left, Expression right) : base(left, right) { }
        public override object Evaluate()
        {
            Value = (double)(Convert.ToInt32(left.Evaluate()) / Convert.ToInt32(right.Evaluate()));
            return Value;
        }
    }
    public class Pow : BinaryExpression
    {
        public override object Value { get; set; }
        public override ExpressionType Type { get; set; }
        public Pow(Expression left, Expression right) : base(left, right) { }
        public override object Evaluate()
        {
            Value = (double)(Convert.ToInt32(left.Evaluate()) ^ Convert.ToInt32(right.Evaluate()));
            return Value;
        }
    }
    public class Mod : BinaryExpression
    {
        public override object Value { get; set; }
        public override ExpressionType Type { get; set; }
        public Mod(Expression left, Expression right) : base(left, right) { }
        public override object Evaluate()
        {
            Value = (double)(Convert.ToInt32(left.Evaluate()) % Convert.ToInt32(right.Evaluate()));
            return Value;
        }
    }
}
