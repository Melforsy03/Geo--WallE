
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
    List<token> m = Tokenizar.TokenizeString("mediatriz (p1 , p2) = let l1 = line(p1 , p2) ; m = measure (p1 , p2) ;c1 = circle (p1 , m) ; c2 = circle (p2, m) ; i1 , i2 = intersect (c1 , c2) ; l2 = line (i1 , i2) ; in l2; puntoMedio (p1 , p2) = let medio ,_ = intersect(line(p1,p2),mediatriz(p1 , p2)) ; in medio ;");
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
