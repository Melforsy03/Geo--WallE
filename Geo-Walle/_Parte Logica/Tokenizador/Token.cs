using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Tokenizador
{
  public enum TokenTypes
  {
    Keyword, Identifier, Number, Operator, Punctuation, Point, Condicional, ErrorType , funcion, Literal , Figura , Arc , Circle , Color
  }
    public class Token
    {
        public string Value {get ; set ;}
        public TokenTypes Type {get ; set ;}

        public Token (string Value , TokenTypes Type)
        {
            this.Value = Value;
            this.Type = Type ;
        }
    
    }
}
