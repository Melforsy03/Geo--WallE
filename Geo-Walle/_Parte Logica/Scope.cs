using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tokenizador;
using Jerarquia;

namespace Lexer
{
    public class Scope
    {
        public Scope Parent;
        public Dictionary<Token, object> Value;
        public Dictionary<Token, ExpressionType> Type;

        public Scope(Scope parent, Dictionary<Token, object> Value, Dictionary<Token, ExpressionType> Type)
        {
            Parent = parent;
            this.Value = Value;
            this.Type = Type;
        }
    }
}
