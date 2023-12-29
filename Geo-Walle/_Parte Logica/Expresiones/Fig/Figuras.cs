using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jerarquia
{
    public abstract class Figuras : Expression
    {
        public override object Value { get; set; }
        public override ExpressionType Type { get; set; }
        public virtual void Traslate(int x, int y) { }
    }
}
