using System.Collections.Generic;
using System;
using System.Threading;

namespace Lexer
{
    public class Errors
    {
        public ErrorCode Code { get; private set; }

        public string Argument { get; private set; }

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