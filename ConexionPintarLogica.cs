
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
    List<token> m = Tokenizar.TokenizeString("mediatriz (p1 , p2) = ");
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
