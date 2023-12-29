using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lexer;

namespace Jerarquia
{
    public abstract class BinaryExpression : Expression
    {
        public Expression right { get; set; }
        public Expression left { get; set; }
        public Expression operacion { get; set; }

        public BinaryExpression(Expression left, Expression right)
        {
            this.left = left;
            this.right = right;
        }
        public override void Scope(Scope scope)
        {
            left.Scope(scope);
            right.Scope(scope);
        }
        public override bool CheckSemantic(List<Errors> errors)
        {
            bool Right = right.CheckSemantic(errors);
            bool Left = left.CheckSemantic(errors);

            if (right.Type != ExpressionType.Number || left.Type != ExpressionType.Number)
            {
                errors.Add(new Errors(ErrorCode.Semantic, "no se puede operar con objetos q no son de tipo numero"));
                Type = ExpressionType.ErrorType;
                return false;
            }

            Type = ExpressionType.Number;
            return Right && Left;
        }
    }
}