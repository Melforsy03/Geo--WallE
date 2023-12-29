using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tokenizador;
using Lexer;

namespace Jerarquia
{
    //clase de numeros 
    public class Number : Expression
    {
        public Number(double value)
        {
            Value = value;
        }
        public override object Value { get; set; }
        public override ExpressionType Type { get; set; }

        public override bool CheckSemantic(List<Errors> errors)
        {
            Type = ExpressionType.Number;
            return true;
        }

        public override object Evaluate()
        {
            return Value;
        }

        public override void Scope(Scope scope) { }
    }
    //clase de literales
    public class Text : Expression
    {
        public override object Value { get; set; }
        public override ExpressionType Type { get; set; }

        public override bool CheckSemantic(List<Errors> errors)
        {
            Type = ExpressionType.Text;
            return true;
        }

        public override object Evaluate()
        {
            return Value;
        }

        public override void Scope(Scope scope) { }
    }
    public class ID : Expression
    {
        public ID(Token id, Expression value)
        {
            this.id = id;
            Value = value;
        }
        public Token id;
        public override object Value { get; set; }
        private Scope scope;
        public override ExpressionType Type { get; set; }

        public override void Scope(Scope scope)
        {
            if (scope.Parent == null)
            {
                return;
            }

            foreach (var item in scope.Parent.Type.Keys)
            {
                if (item.Value == id.Value)
                {
                    this.scope = scope.Parent;
                    return;
                }
            }

            Scope(scope.Parent);
            this.scope = scope;
        }
        public override bool CheckSemantic(List<Errors> errors)
        {
            foreach (var item in scope.Type.Keys)
            {
                if (item.Value == id.Value)
                {
                    Type = scope.Type[item];
                    return true;
                }
            }

            errors.Add(new Errors(ErrorCode.Semantic, "Esta variable no esta declarada"));
            return false;
        }

        public override object Evaluate()
        {
            foreach (var item in scope.Value.Keys)
            {
                if (item.Value == id.Value)
                {
                    Value = scope.Value[item];
                    return Value;
                }

            }
            throw new Exception();
        }
    }

}

