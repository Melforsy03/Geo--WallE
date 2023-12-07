
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
    List<token> m = Tokenizar.TokenizeString("point p1 ; point p2 ; point p3 ; point p4 ; point p5 ; sequence = {p1 , p2 , p3} + {undefined} + {p4 , p5} ;");
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
