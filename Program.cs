using TokensGeo ;
using Lexer;
using ParserGeo ;
namespace Usuario
{
    public class Program
    {
        public static void Main (string [] args)
        {
         string  a = "Fib (n) =  if n <= 1 then 1 else Fib (n - 1) + Fib (n - 2) ; a = 1 + Fib (3);";

         List<token> m = Tokenizar.TokenizeString(a);

         for (int i = 0; i < m.Count; i++)
         {
            Console.WriteLine(m[i].Value + " - " + m[i].Type);
         }
         Geometrico arbol = new Geometrico("" , TokenTypes.Identifier , null);
         arbol.expression = m ;
         
         arbol.Parser();
        arbol.Evaluate();
         Console.WriteLine();
        
        }
 }
}