using ParserGeo;
namespace TokensGeo
{
       public enum TokenTypes
       {
          Keyword , Identifier ,Number, Operator, Punctuation ,Point ,Condicional , Funcion , boolean, letIn , Comando , Line , Segment , Ray , Circle , point_sequence , line_sequence , Underfine , secuencia , Arc , Color
       } 
       public interface Evaluacion
       {
        public  string Evaluar ();
       }
       public class token : Evaluacion , ICloneable 
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
    public object Clone ()
    {
      return this.MemberwiseClone();
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
    public class FigDeDosPunto : Geometrico 
    {
          public FigDeDosPunto (string Value , TokenTypes Type , Geometrico root) : base (Value , Type , root)
          {
              this.Value = Value ;
              this.Type = Type ;
              this.Root = Root;
          }
      }
      public class Arco : Geometrico 
      {
        public Arco (string Value , TokenTypes Type , Geometrico root ) : base (Value , Type , root)
        {
          this.Value = Value;
          this.Type = Type;
          this.Root = Root;
        }
      }
      public class Circunferencia : Geometrico
      {
        public Circunferencia (string Value , TokenTypes Type , Geometrico root ) :base (Value ,Type , root)
        {
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
         return tokens[2].Evaluar().ToString();
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
       throw new Exception();
    }
}
 public class TokenSecuencia : token 
 {
    public List <TokenSecuencia> secuencias {get ; set;} 
    public  List <string> FuncionesEjecutar {get ; set ;}

    public token Padre {get ; set ;}
    public TokenSecuencia(string Value , TokenTypes type , token Padre) : base (Value , type )
    {
        secuencias = new List<TokenSecuencia>();
        FuncionesEjecutar = new List<string>();
        this.Padre = Padre;
         
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

public class Color : token 
{
  public Color (string Value , TokenTypes Type) : base (Value , Type ){}

}
}

