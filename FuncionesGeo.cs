using TokensGeo;
namespace Funciones 
{
    public class FuncionesDeSecuencia
    {
        //devuelve la secuecia original sin el primer elemento
        public static IEnumerable<token> Underscore (IEnumerable <token> a )
        {
            return a.Skip(1);
        }
        //devuelve el primer termino de la secuencia
        public static token Rest (IEnumerable<token> a ) 
        {
              IEnumerator<token> c = a.GetEnumerator() ;
              c.MoveNext();
              return c.Current;
        }
        public static IEnumerable<token> Intersect (token a , token b)
        {
            return a.tokens.Intersect(b.tokens);
        }
    
        //metodo que devuelve un lista de IEnumerable con puntos aleatorios  de la figura 
        public static IEnumerable <token> points (token figura)
        {
            var random = new Random();
            var ListaDesordenada = new List<token>(figura.tokens);
            for (int i = 0; i < ListaDesordenada.Count; i++)
            {
                int RandomIndex = random.Next(ListaDesordenada.Count);
                token temp = ListaDesordenada[i];
                ListaDesordenada[i] = ListaDesordenada[RandomIndex];
                ListaDesordenada[RandomIndex] = temp ;
            } 
            //si la lista tiene mas de un elemento o mas
            if (ListaDesordenada.Count > 1 )
            {
                 return ListaDesordenada.GetRange(0 , ListaDesordenada.Count - 1 );
            }
            else
            {
                //si no , entonces devolvera una lista vacia 
                return ListaDesordenada.GetRange(0 , 0);
            }
          
        }
       //secuencia de puntos aleatorios 
       public static IEnumerable<token> samples ()
        {
        token [] a = new token[20];

        Random ran = new Random();

        for (int i = 0; i < a.Length; i++)
        {
            a[i] = new token("p" + i  , TokenTypes.Point);
        }
        for (int i = 0; i < a.Length; i++)
        {
            yield return a[ran.Next(a.Length)];
        }
        }
       //devuelve una secuencia de valores positivos 
       public static IEnumerable <int> randoms ()
       {
         Random ran = new Random ();

          for (int i = 0; i < 21 ; i++)
          {
            yield return ran.Next(1 ,  101);
          }
       }
       public static int count (IEnumerable <token> secuencia)
       {
          int coun =  (secuencia.Count() > int.MaxValue)? 0 : secuencia.Count();
          return coun;
       } 
    }
}