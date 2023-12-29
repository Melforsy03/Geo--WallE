using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lexer;
using Tokenizador;

namespace Jerarquia
{
    public static class Context
    {
        public static List<Function> funcs = new List<Function>();
        public static Scope General = new Scope(null, new Dictionary<Token, object>(), new Dictionary<Token, ExpressionType>());
        public static bool IsDeclare_Func;
    }
}
