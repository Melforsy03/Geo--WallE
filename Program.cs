using TokensGeo ;
using Lexer;
using ParserGeo ;
namespace Usuario
{
    public class Program
    {
        public static void Main (string [] args)
        {
         string  a = "draw (p1 , p2) ;";

         List<token> m = Tokenizar.TokenizeString(a);

         for (int i = 0; i < m.Count; i++)
         {
            Console.WriteLine(m[i].Value + " - " + m[i].Type);
         }
         Geometrico arbol = new Geometrico("" , TokenTypes.Identifier , null);
         arbol.expression = m ;
         
         arbol.Parser();
         Console.WriteLine("ya");
        
        }
}
}