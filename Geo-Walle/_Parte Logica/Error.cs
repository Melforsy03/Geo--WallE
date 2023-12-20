using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexer
{
    public class Errors
    {
        public ErrorCode Code { get; private set; }

        public string Argument { get; private set; }

        //public CodeLocation Location { get; private set; }

        public Errors(ErrorCode code, string argument)
        {
            this.Code = code;
            this.Argument = argument;
        }
    }
    public enum ErrorCode
    {
        Lexer,
        Sintaxis,
        Semantic,
        Any

    }
}
