﻿
using Lexer;
using TokensGeo;
using ParserGeo;
namespace ConexionPinturaLogica
{
    public class ConexionPinturaLogica
    {
    public static void Main()
    {
    //string input = ""
    List<token> m = Tokenizar.TokenizeString("point p3 ; point p4 ;circle  p1 = ( p3 , measure (p3, p4)); draw (p1) ;");
    for (int i = 0; i < m.Count; i++)
    {
        Console.WriteLine(m[i].Value + " - "+ m[i].Type);
    }
    Geometrico arbol = new Geometrico("" , TokenTypes.Identifier , null);
    arbol.expression = m ;
    arbol.Parser();
    // List<Errors> errores = new();

    }
}
}
