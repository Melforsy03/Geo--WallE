using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lexer;

namespace Jerarquia
{
    public class If_them_else : Expression
    {
        public Expression If;
        public Expression Them;
        public Expression Else;

        public If_them_else(Expression If, Expression Them, Expression Else)
        {
            this.If = If;
            this.Them = Them;
            this.Else = Else;
        }
        public override object Value { get; set; }
        public override ExpressionType Type { get; set; }
        public override void Scope(Scope scope)
        {
            If.Scope(scope);
            Them.Scope(scope);
            Else.Scope(scope);
        }
        public override bool CheckSemantic(List<Errors> errors)
        {
            bool IF = If.CheckSemantic(errors);
            bool THEM = Them.CheckSemantic(errors);
            bool ELSE = Else.CheckSemantic(errors);

            if (If.Type != ExpressionType.Number)
            {
                errors.Add(new Errors(ErrorCode.Semantic, ""));
                Type = ExpressionType.ErrorType;
                return false;
            }

            Type = ExpressionType.Number;
            return IF && THEM && ELSE;
        }

        public override object Evaluate()
        {
            return If.Evaluate().ToString() == "1" ? Them.Evaluate() : Else.Evaluate();
        }
    }
}