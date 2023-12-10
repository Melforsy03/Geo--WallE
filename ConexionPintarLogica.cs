
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
    List<Errors> errors = new List<Errors>();
    List<token> m = Tokenizar.TokenizeString("point p1 ; point p2 ; line l (p1 , p2) ; draw (l);" , errors);
    for (int i = 0; i < m.Count; i++)
    {
        Console.WriteLine(m[i].Value + " - "+ m[i].Type);
    }
    Geometrico arbol = new Geometrico("" , TokenTypes.Identifier , null);
    arbol.expression = m ;
    arbol.Parser();
   Console.WriteLine(((Point) arbol.tokens[0].tokens[0]).x);
    // List<Errors> errores = new();

    }
}
}
