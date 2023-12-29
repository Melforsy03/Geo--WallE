using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lexer;

namespace Jerarquia
{
    public abstract class Secuencia : Expression
    {
        public int Count { get; set; }
    }
}
