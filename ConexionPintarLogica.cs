
using Lexer;
using TokensGeo;
using ParserGeo;
namespace ConexionPinturaLogica
{
    public class ConexionPinturaLogica
    {
    public static void Menu()
    {
    //string input = ""
    List<token> m = Tokenizar.TokenizeString("point p1 ; point p2 ; draw(p1;p2)");
    Geometrico arbol = new Geometrico("" , TokenTypes.Identifier , null);
     arbol.expression = m ;
     arbol.Parser();
     List<Errors> errores = new();

     if (arbol.CheckSemantic(errores))
     {

     }
     else
     {
        
     }
    }
}
}
