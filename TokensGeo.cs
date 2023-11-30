using ParserGeo;
namespace TokensGeo
{
       public enum TokenTypes
       {
          Keyword , Identifier ,Number, Operator, Punctuation ,Point ,Condicional , Funcion , boolean, letIn , Comando , Line , Segment , Ray , Circle , point_sequence , line_sequence , Underfine , secuencia
       } 
       public interface Evaluacion
       {
        public  string Evaluar ();
       }
       public class token : Evaluacion 
       {
          public string Value { get ; set;}

          public TokenTypes Type {get ; set ;}

          public List <token> tokens {get ; set ;}

          public token (string Value , TokenTypes Type)
          {
            this.Value = Value ;
            this.Type = Type ;
            tokens = new List<token>();
          }

         public string Evaluar ()
         {
            return Value;
         }

       }
       public class Identificador : token
       {
        public Identificador (string Value , TokenTypes Type ):base(Value , Type) {}
       }
       public class  tokenBul : token
     {
      public tokenBul(string Value , TokenTypes type) : base(Value , type){}

       public int Evaluar()
     {
        int numero = 0;
    /*   if (Value == "&&")
     {
      
        numero = ((tokenBul)tokens[0]).Evaluar() && ((tokenBul)tokens[1]).Evaluar() ? 1 : 0 ;
    }
    else if (Value == "||")
    {
       
       numero = (((tokenBul)tokens[0]).Evaluar() || ((tokenBul)tokens[1]).Evaluar()) ? 1 :0;
        
    }*/
     if (Value == "!=")
    {

         numero = double. Parse(tokens[0].Evaluar()) != double.Parse(tokens[1].Evaluar())? 1 : 0;
       
    }
    else if (Value == ">")
    {
         numero = double. Parse(tokens[0].Evaluar()) > double.Parse(tokens[1].Evaluar())? 1 : 0;  
    }
    else if (Value == "<" )
    {
         numero = double. Parse(tokens[0].Evaluar()) < double.Parse(tokens[1].Evaluar())? 1 : 0 ;
    }
    else if (Value == "==" )
    {
     
       numero = double. Parse (tokens[0].Evaluar()) == double.Parse(tokens[1].Evaluar()) ? 1 : 0;
     
    }
    else if (Value == ">=" )
    {
     
      numero = double. Parse (tokens[0].Evaluar()) >= double.Parse(tokens[1].Evaluar()) ? 1 : 0;
      
    }
    else if (Value == "<=" )
    {
        numero = double. Parse (tokens[0].Evaluar()) <= double.Parse(tokens[1].Evaluar())? 1 : 0;  
     }
     return numero ;
  
     }
  }
       public class TokenNumero :  token
       {
         public TokenNumero(string Value , TokenTypes Type) : base (Value , Type){}
       }
       public class Function : token 
       {
        public List<token> globales {get ; set ;}
         public Function (string Value , TokenTypes Type) : base ( Value , Type) 
         {
             globales = new List<token>();
         }
       }
       public class FuncionPointsDos : Function
       {
          public FuncionPointsDos (string Value , TokenTypes Type , token point1 , token point2) : base (Value , Type)
          {

          }
       }
       public class OperatorNode : token
       {
          public OperatorNode(string Value  , TokenTypes Type ) : base ( Value , Type){}
         
         public double Evaluar()
    {
        // Evaluar la operación según el operador        
            
       if (Value == "+")
       {
        
            try
            {
                return double.Parse(tokens[0].Evaluar()) + double.Parse(tokens[1].Evaluar()) ;  
            }
            catch (System.Exception)
            {
                
                Console.WriteLine(" error en ejecucuion ,el operador" +  Value + " no puede operar con estos elementos");
            }

       }
       else if (Value == "-")
       {
         try
            {
                return double.Parse(tokens[0].Evaluar()) - double.Parse(tokens[1].Evaluar()) ;  
            }
            catch (System.Exception)
            {
                
                Console.WriteLine(" error en ejecucuion ,el operador" +  Value + " no puede operar con estos elementos");
            }           
            
       }
       else if (Value == "*")
       {
            try
            {
                return double.Parse(tokens[0].Evaluar()) * double.Parse(tokens[1].Evaluar()) ;  
            }
            catch (System.Exception)
            {
                
                Console.WriteLine(" error en ejecucuion ,el operador " + Value + " no puede operar con estos elementos");
            }
       }
       else if (Value == "/")
       {
            try
            {
                return double.Parse(tokens[0].Evaluar()) / double.Parse(tokens[1].Evaluar()) ;  
            }
            catch (System.Exception)
            {
                Console.WriteLine(" error en ejecucuion ,el operador " + Value + " no puede operar con estos elementos");
            }
        
       }
        else if (Value == "%")
       {
            try
            {
                return double.Parse(tokens[0].Evaluar()) % double.Parse(tokens[1].Evaluar()) ;  
            }
            catch (System.Exception)
            {
                Console.WriteLine(" error en ejecucuion ,el operador " + Value + " no puede operar con estos elementos");
            }
        
       }
       else if (Value == "^")
       {
      try
      {
        return Math.Pow(double.Parse(tokens[0].Evaluar()) , double.Parse(tokens[1].Evaluar())) ; 
      }
      catch (System.Exception)
      {
        
         Console.WriteLine("error en ejecucuion , el operador " + Value + "no opera con esos elementos");
       }
             
       }
       else if (Value == "Sqrt")
       {
        try
        {
        return Math.Sqrt(double.Parse(tokens[0].Evaluar()));
        }
        catch (System.Exception)
        {
            
            throw;
        }
       }
     throw new ArgumentException (" error en ejecucuion ,no se pudo ejecutar esta operacion");
}
       }
       //figuras de dos puntos como , el segmento , el rayo , medida  entre dos puntos 
    public class FigDeDosPunto : token 
       {
          public  token a {get ;set;}
          public token b {get ; set ;}
          public FigDeDosPunto (string Value , TokenTypes Type , token a , token b) : base (Value , Type)
          {
              this.Value = Value ;
              this.Type = Type ;
              this.a = a ;
              this.b = b ;
          }
       }
    public class LetIn : Geometrico
      {
       public LetIn (string Value , TokenTypes type , Geometrico Padre) : base(Value , type , Padre)
       {
        this.Value = "let";
        this.Type = TokenTypes.letIn;
       } 
   }
   public class IfElseNode : Geometrico
   {
      public IfElseNode(string Value ,  TokenTypes Type , Geometrico Padre) : base(Value , Type , Padre){}
      public string Evaluar()
     {
        if (((tokenBul)tokens[0]).Evaluar() == 1)
        {
          return tokens[1].Evaluar().ToString();
        }
        else
        {
            try
            {
                   return tokens[2].Evaluar().ToString();
            }
            catch (System.Exception)
            {
                Console.WriteLine("error en ejcucion en la funcion if - else ");
                throw;
            }
          

        }

    }
  }
   public class FunctionNode : token
{
    public FunctionNode (string FunctionName , TokenTypes type ):base (FunctionName , type){}
   
    public double Evaluar()
    {
        // Evaluar la función según el nombre
       if (Value == "sin")
       {
                if (tokens[0].Type == TokenTypes.Operator)
                {
                return Math.Sin(((OperatorNode)tokens[0]).Evaluar());
                }
                else
                {
                    return Math.Sin(Double.Parse(tokens[0].Evaluar()));
                }
       }
       else if (Value == "cos")
       {
         if (tokens[0].Type == TokenTypes.Operator)
                {
                return Math.Cos(((OperatorNode)tokens[0]).Evaluar());
                }
                else
                {
                    return Math.Cos(Double.Parse(tokens[0].Evaluar()));
                }
       }
         else if (Value == "tan")
       {
         if (tokens[0].Type == TokenTypes.Operator)
                {
                return Math.Tan(((OperatorNode)tokens[0]).Evaluar());
                }
                else
                {
                    return Math.Tan(Double.Parse(tokens[0].Evaluar()));
                }
       }
        else if (Value == "sqrt")
       {
         if (tokens[0].Type == TokenTypes.Operator)
                {
                return Math.Sqrt(((OperatorNode)tokens[0]).Evaluar());
                }
                else
                {
                    return Math.Sqrt(Double.Parse(tokens[0].Evaluar()));
                }
       }   
                throw new InvalidOperationException("Función no válida");
    }
}
 public class TokenSecuencia : token 
 {
    public List <TokenSecuencia> secuencias {get ; set;} 
    public  List <string> FuncionesEjecutar {get ; set ;}
    public TokenSecuencia(string Value , TokenTypes type) : base (Value , type )
    {
        secuencias = new List<TokenSecuencia>();
        FuncionesEjecutar = new List<string>();
         
    }
      public IEnumerable<token> Underscore (IEnumerable <token> a )
      {
            return a.Skip(1);
      }
        //devuelve el primer termino de la secuencia
        public token Rest (IEnumerable<token> a ) 
        {
              IEnumerator<token> c = a.GetEnumerator() ;
              c.MoveNext();
              return c.Current;
        }
        public IEnumerable<token> Intersect (token a , token b)
        {
            return a.tokens.Intersect(b.tokens);
        }
        public IEnumerable<token> samples ()
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
       public  IEnumerable <int> randoms ()
       {
         Random ran = new Random ();
          for (int i = 0; i < 21 ; i++)
          {
            yield return ran.Next(1 ,  101);
          }
       }
     

 }

}
