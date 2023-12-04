using ParserGeo;
using Lexer;
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
          if(this is Identificador) return ((Identificador)this).Evaluar();
          else if(this is OperatorNode)return ((OperatorNode)this).Evaluar().ToString();
          else if(this is TokenNumero)return ((TokenNumero)this).Evaluar();
          else if(this is Function)return((Function)this).Evaluar();
          else if (this is IfElseNode) return ((IfElseNode)this).Evaluar();
          return Value ;
         }

       }
       public class Identificador : token
       {
        public Identificador (string Value , TokenTypes Type ):base(Value , Type) {}
        public string Evaluar ()
        {
          return tokens[0].Evaluar();
        }
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
         numero = (double. Parse(tokens[0].Evaluar()) < double.Parse(tokens[1].Evaluar()))? 1 : 0 ;
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
      numero = ( double. Parse (tokens[0].Evaluar()) <= double.Parse(tokens[1].Evaluar()))? 1 : 0;
        
     }
     return numero;
  
     }
  }
       public class TokenNumero :  token
       {
         public TokenNumero(string Value , TokenTypes Type) : base (Value , Type){}
         public string Evaluar ()
         {
          return Value;
         }
       }
       public class Function : Geometrico 
       {
         public Function (string Value , TokenTypes Type , Geometrico Root ) : base ( Value , Type , Root){} 
         public string Evaluar ()
         { 
           Function b = new Function(Value , TokenTypes.Funcion , Root);
            if(Root.Value == "") b = (Function)Root.variablesLocales.Find(valor => valor.Value == Value).Clone();
            else b =  b = (Function)((Function)Root.variablesGlobales.Find(valor => valor.Value == Value)).Clone();
            for (int i = 0; i < b.variablesLocales.Count; i++)
            {
              b.variablesLocales[i].tokens.Add(variablesLocales[i]);
            }
            b.variablesGlobales = Root.variablesLocales.Select (p => (token)p.Clone()).ToList();
            if (b != null && b.tokens[0] is IfElseNode )
            {
              return Evaluador(b);
            }
            return tokens[0].Evaluar().ToString();
         }
        //darle nombre a los parametros de la funcion
        private string Evaluador (Function Funcion)
        {
           // Funcion.variablesLocales = CambioVariableParametro(variablesLocales.Select (p => (token)p.Clone()).ToList(),Funcion.variablesLocales);
            Funcion.tokens[0].tokens[0].tokens = Funcion.CambioVariableParametro(Funcion.tokens[0].tokens[0].tokens , Funcion.variablesLocales );
            string valor = Funcion.EvaluadorRecursivo(Funcion , Funcion.variablesLocales , "");
            return valor;
        }
         private List<token> CambioVariableParametro(List<token> variables , List<token> parameter)
         {
          if (variables.Count == 0)
          {
            return variables;
          }
            for(int i = 0 ; i < variables.Count ; i ++)
             {
              if (parameter.Any(p => p.Value == variables[i].Value))
              {
                token a = (token)parameter.Find(p => p.Value == variables[i].Value).Clone();
                variables[i] = (token)a.Clone();
              }
              else if (parameter.Count == 1 && variables.Count == 1 && variables[0].Type == TokenTypes.Number  )
              {
                string Value = variables[0].Value ;
                variables[0] = (token)parameter[0].Clone();
                variables[0].tokens.Add(new TokenNumero(Value , TokenTypes.Number));
              }
              else
              {
                CambioVariableParametro(variables[i].tokens , parameter);
              }
            }
          return variables;
         }
         public string EvaluadorRecursivo (token Parent , List<token> parameter,string h)
         {
          
          if (Parent.tokens.Count > 0 && Parent.tokens[0] is IfElseNode )
          {
             if(((tokenBul)Parent.tokens[0].tokens[0]).Evaluar() == 1 )  return Parent.tokens[0].tokens[1].Evaluar();
            
          }
              for (int i = 0; i < Parent.tokens.Count; i++)
              {
                if (Root.variablesLocales.Any(valor => valor.Value == Parent.tokens[i].Value))
                {
                  List<token> VariblesLocalesDos = new List<token>();
                  VariblesLocalesDos.AddRange(((Function)Parent.tokens[i]).variablesLocales.Select(p => (token)p.Clone()).ToList());
                  Function FuncionEvaluar = (Function)((Function)Root.variablesLocales.Find(valor => valor.Value == Parent.tokens[i].Value )).Clone();
                  FuncionEvaluar.variablesLocales = FuncionEvaluar.CambioVariableParametro(VariblesLocalesDos.Select(p => (token)p.Clone()).ToList() , parameter);
                  FuncionEvaluar.tokens[0].tokens[0].tokens = CambioVariableParametro(FuncionEvaluar.tokens[0].tokens[0].tokens , parameter);
                  token acumulacion = new token (FuncionEvaluar.EvaluadorRecursivo(FuncionEvaluar, FuncionEvaluar.variablesLocales , h) , TokenTypes.Number);
                  Console.WriteLine(acumulacion.Value);
                  token evaluacion = (token)((token)Parent).Clone();
                  evaluacion.tokens[i] = (token)((token)acumulacion).Clone();
                 h = evaluacion.Evaluar().ToString();
                  Console.WriteLine(h);
                  return h;
                 }
                else
                {
                  if (Parent.tokens[i] is Geometrico && Parent is Geometrico)
                  {
                    ((Geometrico)Parent.tokens[i]).variablesGlobales.AddRange(((Geometrico)Parent).variablesGlobales);
                     ((Geometrico)Parent.tokens[i]).variablesGlobales.AddRange(((Geometrico)Parent).variablesLocales);
                    h = EvaluadorRecursivo(((Geometrico)Parent.tokens[i]), parameter , h);
                  }
                  else
                  {
                    h = EvaluadorRecursivo(Parent.tokens[i], parameter, h );
          
                  }
              }
            }
          return h;
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
         return double.Parse(tokens[0].Evaluar()) + double.Parse(tokens[1].Evaluar()) ;
       }
       else if (Value == "-")
       {
        
        return double.Parse(tokens[0].Evaluar()) - double.Parse(tokens[1].Evaluar()) ;  
                  
       }
       else if (Value == "*")
       {
           
       return double.Parse(tokens[0].Evaluar()) * double.Parse(tokens[1].Evaluar()) ;  
         
       }
       else if (Value == "/")
       {
          return double.Parse(tokens[0].Evaluar()) / double.Parse(tokens[1].Evaluar()) ;  
       }
        else if (Value == "%")
       {
           
          return double.Parse(tokens[0].Evaluar()) % double.Parse(tokens[1].Evaluar()) ;  
       }
       else if (Value == "^")
       {
    
        return Math.Pow(double.Parse(tokens[0].Evaluar()) , double.Parse(tokens[1].Evaluar())) ; 
    
             
       }
       else if (Value == "Sqrt")
       {
       
        return Math.Sqrt(double.Parse(tokens[0].Evaluar()));
       
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
