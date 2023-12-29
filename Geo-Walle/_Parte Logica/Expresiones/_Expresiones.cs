using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lexer;

namespace Jerarquia
{
    public abstract class Expression
    {
        public abstract object Value { get; set; }
        public abstract ExpressionType Type { get; set; }
        public abstract void Scope(Scope scope);
        public abstract bool CheckSemantic(List<Errors> errors);
        public abstract object Evaluate();
    }

    public enum ExpressionType
    {
        Text,
        Number,
        ID,
        Aritmetic,
        ErrorType,
        AnyType,
        Line,
        Ray,
        Segment,
        Point,
        Circle,
        Arc,
        Secuence
    }
}

