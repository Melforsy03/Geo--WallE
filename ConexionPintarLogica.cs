
using Lexer;
using TokensGeo;
using ParserGeo;
namespace ConexionPinturaLogica
{
    public class ConexionPinturaLogica
    {
    public static void Main()
    {
  
    List<Errors> errors = new List<Errors>();
    List<token> m = Tokenizar.TokenizeString(Console.ReadLine(), errors);
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
