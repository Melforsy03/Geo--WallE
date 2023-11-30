using TokensGeo ;
using Lexer;
using ParserGeo ;
namespace Usuario
{
    public class Program
    {
        public static void Main (string [] args)
        {
         string  a = "_ , a = { 90 , 40 , 60 } ; t, _ = {1 ,2 ,3}; sec = intersect(c1 , c2) ; a , rest = {6 , 7 , 8} ; a , b ,_ = { 4 ,5 , 90 } ; a , b , rest = intersect (c1 , c2) ;";

         List<token> m = Tokenizar.TokenizeString(a);

         for (int i = 0; i < m.Count; i++)
         {
            Console.WriteLine(m[i].Value + " - " + m[i].Type);
         }
         Geometrico arbol = new Geometrico("" , TokenTypes.Identifier , null);
         arbol.expression = m ;
         
         arbol.Construir();
         Console.WriteLine("ya");
        
        }
}
}