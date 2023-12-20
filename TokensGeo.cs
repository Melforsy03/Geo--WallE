using Lexer;
using ParserGeo;
namespace TokensGeo
{
  public enum TokenTypes
  {
    Keyword, Identifier, Number, Operator, Punctuation, Point, Condicional, ErrorType,
    Funcion, boolean, letIn, Comando, Line, Segment, Ray, Circle, point_sequence, line_sequence, Underfine, secuencia, Arc , Color, figura , measure , Literal , comparacion
  }
  public interface Evaluacion
  {
     token Evaluar();
     bool CheckSemantic(List<Errors> errores);
  }
  public class token : Evaluacion
  {
    public string Value { get; set; }

    public TokenTypes Type { get; set; }
    public GeoType GeoType { get; set; }
    public TokenTypes TypeReturn {get ; set ;}

    public List<token> tokens { get; set; }

    public token(string Value, TokenTypes Type)
    {
      this.Value = Value;
      this.Type = Type;
      tokens = new List<token>();
    }
   
      public object Clone ()
    {
     if(this is LetIn) return ((LetIn)this).Clone();
     else if(this is Print)return ((Print)this).Clone();
     else if (this is OperatorNode)return ((OperatorNode)this).Clone();
     else if (this is Identificador) return ((Identificador)this).Clone();
     else if (this is TokenNumero)return ((TokenNumero)this).Clone();
     else if (this is tokenBul) return ((tokenBul)this).Clone();
     else if(this is IfElseNode) return ((IfElseNode)this).Clone();
     else if (this is tokenLiteral)return ((tokenLiteral)this).Clone();
     else if (this is FunctionNode)return ((FunctionNode)this).Clone();
     else if (this is Point)return ((Point)this).Clone ();
     else if (this is FuncionPointsDos) return ((FuncionPointsDos)this).Clone();
     else if (this is Arco )return ((Arco)this).Clone();
     else if (this is Circunferencia) return ((Circunferencia)this).Clone();
     else if (this is TokenSecuencia) return ((TokenSecuencia)this).Clone ();
     else 
     {
      return ((Function)this).Clone();
     }
    }
   
   public token Evaluar()
    {
     if(this is LetIn) return ((LetIn)this).Evaluar();
     else if(this is Print)return ((Print)this).Evaluar();
     else if (this is OperatorNode)return ((OperatorNode)this).Evaluar();
     else if (this is Identificador) return ((Identificador)this).Evaluar();
     else if (this is TokenNumero)return ((TokenNumero)this).Evaluar();
     else if (this is tokenBul) return ((tokenBul)this).Evaluar();
     else if(this is IfElseNode) return ((IfElseNode)this).Evaluar();
     else if (this is tokenLiteral)return ((tokenLiteral)this).Evaluar();
     else if (this is FunctionNode)return ((FunctionNode)this).Evaluar();
     else
     {
      return ((Function)this).Evaluar();
     }
    }
  
   public bool CheckSemantic(List<Errors> errors)
   {
     if(this is LetIn) return ((LetIn)this).CheckSemantic(errors);
     else if(this is Print)return ((Print)this).CheckSemantic(errors);
     else if (this is OperatorNode)return ((OperatorNode)this).CheckSemantic(errors);
     else if (this is Identificador) return ((Identificador)this).CheckSemantic(errors);
     else if (this is TokenNumero)return ((TokenNumero)this).CheckSemantic(errors);
     else if (this is tokenBul) return ((tokenBul)this).CheckSemantic(errors);
     else if(this is IfElseNode) return ((IfElseNode)this).CheckSemantic(errors);
     else if (this is tokenLiteral)return ((tokenLiteral)this).CheckSemantic(errors);
     else if (this is FunctionNode)return ((FunctionNode)this).CheckSemantic(errors);
     else if (this is FuncionPointsDos)return ((FuncionPointsDos)this).CheckSemantic(errors);
     else if (this is Arco)return ((Arco)this).CheckSemantic(errors);
     else if (this is Circunferencia) return ((Circunferencia)this).CheckSemantic(errors);
     else
     {
      return ((Function)this).CheckSemantic(errors);
     }
   }
  
  }
   public class Print : token 
 {
   public Print(string Value , TokenTypes type ) : base (Value,type){}

   public token Evaluar()
   {
    if (tokens.Count == 0)
    {
        return null;
    }
    return tokens[0].Evaluar(); 
   }
    public Print Clone ()
   {
    Print objeto = new Print(this.Value , this.Type);
    foreach (var item in this.tokens)
    {
      objeto.tokens.Add((token)item.Clone());
    }
    return objeto;
  }
  public bool CheckSemantic(List<Errors> errors)
  {
    bool error = tokens[0].CheckSemantic(errors);
    if (!error)
    {
       errors.Add(new Errors(ErrorCode.Semantic , "error en la funcion Print"));
       TypeReturn = TokenTypes.ErrorType;
       return false ;
    }
    TypeReturn = tokens[0].TypeReturn;
    return true;
    
  }
 }
  public class Identificador : token  ,ICloneable
 {
    
    //public int position{get ;set;}
    public Identificador(string Value , TokenTypes type) :base(Value , type ){}
    public token Evaluar ()
    {
      try
      {
         return tokens[0].Evaluar();
      }
      catch(Exception x)
      {
        throw new ArgumentException("la variable " + Value +" no fue definida");
      }
      
    }

   public Identificador Clone ()
  {
    Identificador objeto = new Identificador(this.Value , this.Type);
    objeto.TypeReturn = this.TypeReturn;
    foreach (var item in this.tokens)
    {
      if (item != null)
      objeto.tokens.Add((token)item.Clone());
      else
      {
        throw new ArgumentException("la varibale " + Value + " no fue definida correctamente");
      }
    }
    return objeto;
  }
  public bool CheckSemantic(List<Errors> errors)
  {
      bool check = false ;
   
      if (tokens.Count > 0)
      {
      check = tokens[0].CheckSemantic(errors);
    
      if (!check)
      {
      errors.Add(new Errors (ErrorCode.Semantic , "error en la variable " + Value ));
      TypeReturn = TokenTypes.ErrorType;
      return false ;
      }
       if (tokens[0].Value == "true" || tokens[0].Value == "true ")
       TypeReturn = TokenTypes.boolean;
       else
       {
       TypeReturn = tokens[0].TypeReturn;
       }
      }
    else {TypeReturn = TokenTypes.Identifier;}
    return true ;
  }
 }
  public class tokenBul : token
  {
    public tokenBul(string Value , TokenTypes type) : base(Value , type){}

     public token Evaluar()
    {
      token evaluado ;
     try
     {
      
     if (Value == "&&")
     {
      if (tokens[0].Evaluar().Equals(tokens[1].Evaluar()))
      return new TokenNumero("1", TokenTypes.Number);
      else
      {
         return new TokenNumero("0", TokenTypes.Number);
      }
     }
     else if (Value == "||")
     {
     return new TokenNumero("1", TokenTypes.Number);
     }
     else if (Value == "!=")
     {
         if (tokens[0].Evaluar().Equals(tokens[1].Evaluar()))
         return new TokenNumero ("0" , TokenTypes.Number);
         else 
         {
         return new TokenNumero ("1" , TokenTypes.Number);
         }
     }
     else if (Value == ">")
     {
     return evaluado = (double.Parse(tokens[0].Evaluar().Value) > double.Parse(tokens[1].Evaluar().Value )) ? new TokenNumero("1", TokenTypes.Number) : new TokenNumero("0" , TokenTypes.Number);
     }
     else if (Value == "<" )
     {
     return evaluado = (double.Parse(tokens[0].Evaluar().Value) < double.Parse(tokens[1].Evaluar().Value )) ? new TokenNumero("1", TokenTypes.Number) : new TokenNumero("0" , TokenTypes.Number);
     }
     else if (Value == "==" )
     {
         if (tokens[0].Evaluar().Equals(tokens[1].Evaluar()))
         return new TokenNumero ("1" , TokenTypes.Number);
         else 
         {
         return new TokenNumero ("0" , TokenTypes.Number);
         }
    
     }
     else if (Value == ">=" )
     {
     return evaluado = (double.Parse(tokens[0].Evaluar().Value) >= double.Parse(tokens[1].Evaluar().Value )) ? new TokenNumero("1", TokenTypes.Number) : new TokenNumero("0" , TokenTypes.Number);
     }
     else  
     {
      return evaluado = (double.Parse(tokens[0].Evaluar().Value) <= double.Parse(tokens[1].Evaluar().Value )) ? new TokenNumero("1", TokenTypes.Number) : new TokenNumero("0" , TokenTypes.Number);
     }
    }
     catch (System.Exception)
     {
      throw new ArgumentException("el operador " + Value + " no puede trabajar con elementos de tipo distinto");
     }
    return evaluado;
    }
    public tokenBul Clone ()
   {
    tokenBul objeto = new tokenBul(this.Value , this.Type);
    foreach (var item in this.tokens)
    {
      objeto.tokens.Add((token)item.Clone());
    }
    return objeto;
   }
   public bool CheckSemantic(List<Errors> errors)
   {
    bool check = false ;
    bool check1 = false;
   
    if(tokens.Count != 2 )
    {
        errors.Add(new Errors(ErrorCode.Semantic , "error en la operacion " + Value + " parametros incorrectos" ));
        return false ;
    }
    if (tokens[0].Type == TokenTypes.Number || tokens[0].Type == TokenTypes.Literal)
    {
      check = true ;
      tokens[0].TypeReturn = tokens[0].Type;
    }
    else if(tokens[0].Value == "true" || tokens[0].Value == "false")
    {
      check = true ;
      tokens[0].TypeReturn = TokenTypes.boolean;
    }
    else 
    {
    check = tokens[0].CheckSemantic(errors);
    }
     if(!check)
    {
      errors.Add(new Errors(ErrorCode.Semantic , "error en la operacion " + Value ));
    }
    if (tokens[1].Type == TokenTypes.Number || tokens[1].Type == TokenTypes.Literal)
    {
      check1 = true ;
      tokens[1].TypeReturn = tokens[1].Type;
    }
    else if(tokens[1].Value == "true" || tokens[1].Value == "false")
    {
      check1 = true ;
      tokens[1].TypeReturn = TokenTypes.boolean;
    }
    else
    {
      check1 = tokens[1].CheckSemantic(errors);
    }
     if(!check1)
    {
      errors.Add(new Errors(ErrorCode.Semantic , "error en la operacion " + Value ));
      
    }
    if(tokens[0].Type == TokenTypes.Identifier)
    {
      tokens[0].TypeReturn = tokens[1].TypeReturn;
    }
     if(tokens[1].Type == TokenTypes.Identifier)
    {
      tokens[1].TypeReturn = tokens[0].TypeReturn;
    }
    if (Value != "&&" && Value != "||" && Value != "!=" && Value != "==" && tokens[0].TypeReturn != tokens[1].TypeReturn )
    {
      errors.Add(new Errors(ErrorCode.Semantic , "el operador " + Value + " solo opera con typos numeros"));
      return false ;
    }
    if(check && check1)
    {
      if(Value == "==" && Value == "!=")
      TypeReturn = TokenTypes.comparacion;
      TypeReturn = TokenTypes.boolean;
      return true;
    }
    else
    {
      TypeReturn = TokenTypes.ErrorType;
      return false ;
    }
   }
  }
  public class tokenLiteral : token 
 {
    public tokenLiteral(string value , TokenTypes types) :base(value ,types ){}

    public token Evaluar()
    {
        return this;
    }
    public tokenLiteral Clone ()
   {
    tokenLiteral objeto = new tokenLiteral(this.Value , this.Type);
    
    return objeto;
   }
   public bool CheckSemantic(List<Errors> errors)
   {
    TypeReturn = TokenTypes.Literal;
    return true ;
   }
 }
  public class TokenNumero : token
  {
    public TokenNumero (string Value , TokenTypes type) : base(Value , type){}

    public token Evaluar()
    {
        return this;
    }
   public TokenNumero Clone ()
   {
    TokenNumero objeto = new TokenNumero(this.Value , this.Type);
    return objeto;
   }
   public bool CheckSemantic(List<Errors> errors)
   {
    TypeReturn = TokenTypes.Number;
    return true ;
   }
  }
  public class Function : Geometrico
  {
     public int FuncionGuardada {get ; set ;}
    public Function(String Value , TokenTypes Type , Geometrico root) : base(Value , Type , root)
    {
      FuncionGuardada = 0;
    }
    public Function Clone ()
    {
      Function objeto = new Function(this.Value , this.Type , this.Root);
      objeto.FuncionGuardada = this.FuncionGuardada;
      objeto.tokens = new List<token>();
      foreach (token item in this.tokens)
      {
        objeto.tokens.Add((token)item.Clone());
      }
      objeto.variablesLocales = new List<token>();
      foreach (var item in this.variablesLocales)
      {
        objeto.variablesLocales.Add((token)item.Clone());
      }
      objeto.variablesGlobales = new List<token>();
      foreach (var item in this.variablesGlobales)
      {
        objeto.variablesGlobales.Add((token)item.Clone());
      }
     
      return objeto;
    }
    public token Evaluar()
    {
      if (Root.variablesLocales.Any(valor => valor .Value ==  Value))
      {
        Function actual = (Function)Root.variablesLocales[FuncionGuardada].Clone();
        actual.variablesGlobales = new List<token>(Root.variablesLocales);
        actual.tokens = CambioDPadre(actual.tokens , actual);
        for (int i = 0; i < variablesLocales.Count; i++)
        {
          actual.variablesLocales[i].tokens.Add(variablesLocales[i]);
        }
        actual.CambioDevariable2(actual.tokens , actual.variablesLocales);
        return actual.tokens[0].Evaluar();
      }
      else 
      {
        Function actual = (Function) Root.variablesGlobales[FuncionGuardada].Clone();
         actual.tokens = CambioDPadre(actual.tokens , actual);
         actual.variablesGlobales.Add((Function)Root.variablesGlobales.Find(valor => valor.Value ==  Value).Clone());

        for (int i = 0; i < variablesLocales.Count; i++)
        {
         actual.variablesLocales[i].tokens.Add(actual.CambioDevariable(Root.variablesLocales[i] , variablesLocales[i]).tokens[0]);
        }
         actual.CambioDevariable2(actual.tokens, actual.variablesLocales);
        return actual.tokens[0].Evaluar();
      }
     
    }
    private token CambioDevariable(token token , token varible)
    {
      if (varible.tokens.Count == 0)
      {
        token.tokens = new List<token>();
        token.tokens.Add(varible);
        return token;
      }
      token auxiliar = (token)varible.Clone();
      for (int i = 0; i < varible.tokens.Count; i++)
      {
        if(varible.tokens[i].Value == token.Value)
        {
          auxiliar.tokens[i] = (token)token.Clone();
        }
      }
        token auxiliar2 = (token)token.Clone();
        auxiliar2.tokens = new List<token>();
        auxiliar2.tokens.Add(new token (auxiliar.Evaluar().Value, TokenTypes.Number));
        return auxiliar2;

    }
    private List<token> CambioDevariable2(List<token> cambio, List<token> parametros)
    {
      if (cambio.Count == 0)
      {
        return cambio ;
      }
      for (int i = 0; i < cambio.Count; i++)
      {
        if (parametros.Any(valor => valor.Value == cambio[i].Value))
        {
          cambio[i].tokens = new List<token>();
          cambio[i].tokens.Add((token)parametros.Find(valor=> valor.Value == cambio[i].Value).tokens[0].Clone());
        }
        else
        {
          CambioDevariable2(cambio[i].tokens, parametros);
        }
      }
      return cambio;
    }
   private List<token>  CambioDPadre (List<token> hijos ,Function actual)
    {
      for (int i = 0; i < hijos.Count; i++)
      {
        if (hijos[i].Value == actual.Value)
        {
          ((Function)hijos[i]).Root = actual;
        }
        else
        {
          CambioDPadre(hijos[i].tokens,actual);
        }
      }
      return hijos ;
    }
    public bool CheckSemantic(List<Errors> errors)
    {
      bool cuerpo = true;
      bool variablesL = true;
    
      if(tokens.Count > 0)
      {
      cuerpo = tokens[0].CheckSemantic(errors);
      if(!cuerpo)
      {
        errors.Add(new Errors(ErrorCode.Semantic , "error en la funcion " + Value ));
      }
      }
      foreach (var item in variablesLocales)
      {
        variablesL = variablesL && item.CheckSemantic(errors);
        if (!variablesL)
        {
          errors.Add(new Errors(ErrorCode.Semantic , "error en la funcion " + Value + "error en las variables locales"));
        }
      }
     inferencia(tokens);
      
    if(cuerpo && variablesL)
    {
      if(tokens.Count > 0 && tokens[0].TypeReturn != TokenTypes.Keyword && !(tokens[0] is IfElseNode) )
      TypeReturn = tokens[0].TypeReturn;
      else
      {
      TypeReturn = TokenTypes.Identifier;
      }
      return true ;

    }
    else
    {
      TypeReturn = TokenTypes.ErrorType;
      return false ;
    }

   }  
   void inferencia (List<token> tokens)
   {
   if(tokens.Count == 0)return ;
   foreach (var item1 in variablesLocales)
      {
        foreach (var item in tokens)
       {
         if(item1.Value == item.Value && item.TypeReturn != TokenTypes.Keyword)
         {
           item1.TypeReturn = item.TypeReturn;
           break;
         }
         else
         {
           inferencia(item.tokens);
         }
       }
      }
      
   }
  }
 
  public class OperatorNode : token
  {
    public OperatorNode (string Operator , TokenTypes type) : base(Operator , type){}

    public token Evaluar()
    {
      if (tokens[0].Type == TokenTypes.Identifier && tokens[0].tokens.Count == 0)
      {
        throw new ArgumentException("la variable " + tokens[0].Value + " no fue declarada");
      }
      if (tokens[1].Type == TokenTypes.Identifier && tokens[1].tokens.Count == 0)
      {
        throw new ArgumentException("la variable " + tokens[1].Value + " no fue declarada");
      }
      try
      {
       if (Value == "+")
       {
       
        try
        {
          
         return new TokenNumero((double.Parse(tokens[0].Value) + double.Parse(tokens[1].Value)).ToString() , TokenTypes.Number);
        
         }
         catch (System.Exception)
          {
           
           if(!double.TryParse(tokens[0].Value ,out double value ) && !double.TryParse(tokens[1].Value ,out double value2 ))
            {
             return new tokenLiteral (value + "" + value2 , TokenTypes.Literal);
            }
          throw new ArgumentException("no es posible hacer la operacion " + " con estos elementos");
        }
       }
       else if (Value == "-")
       {
        return new TokenNumero ((double.Parse(tokens[0].Evaluar().Value) - double.Parse(tokens[1].Evaluar().Value)).ToString() , TokenTypes.Number);   
       }
       else if (Value == "*")
       {
        return new TokenNumero ((double.Parse(tokens[0].Evaluar().Value) * double.Parse(tokens[1].Evaluar().Value)).ToString() , TokenTypes.Number);  
       }
       else if (Value == "/")
       {
          return new TokenNumero ((double.Parse(tokens[0].Evaluar().Value) / double.Parse(tokens[1].Evaluar().Value)).ToString() , TokenTypes.Number);  
       }
        else if (Value == "%")
       {
          return new TokenNumero ((double.Parse(tokens[0].Evaluar().Value) % double.Parse(tokens[1].Evaluar().Value)).ToString() , TokenTypes.Number); 
       }
       else 
       {
         return new TokenNumero (Math.Pow(double.Parse(tokens[0].Evaluar().Value) , double.Parse(tokens[1].Evaluar().Value)).ToString() , TokenTypes.Number); 
       }
      }
      catch (System.Exception)
      {
        throw new ArgumentException("el operador " + Value + " no puede operar con estos elementos");
        
      }
    }
    public OperatorNode Clone ()
   {
    OperatorNode objeto = new OperatorNode(this.Value , this.Type);
    foreach (var item in this.tokens)
    {
      objeto.tokens.Add((token)item.Clone());
    }
    return objeto;
   }
   public bool CheckSemantic(List<Errors> errors)
   {
    bool check = false ;
    bool check1 = false;
     if(tokens.Count != 2 || tokens[0] is null || tokens[1] is null )
    {
        errors.Add(new Errors(ErrorCode.Semantic , "error en la operacion " + Value + " parametros incorrectos" ));
        TypeReturn = TokenTypes.ErrorType;
        return false;
    }
     check = tokens[0].CheckSemantic(errors);
     if(!check)
     {
       errors.Add(new Errors(ErrorCode.Semantic , "error en la operacion " + Value ));
       TypeReturn = TokenTypes.ErrorType;
     }
     check1 = tokens[1].CheckSemantic(errors);
     if(!check1)
     {
       errors.Add(new Errors(ErrorCode.Semantic , "error en la operacion " + Value ));
       TypeReturn = TokenTypes.ErrorType;
     }
    if(tokens[0].Type == TokenTypes.Identifier)
    {
      tokens[0].TypeReturn = tokens[1].TypeReturn;
    }
    else if (tokens[1].Type == TokenTypes.Identifier)
    {
      tokens[1].TypeReturn = tokens[0].TypeReturn;
    }
    if((check && check1))
    {
      if (Value != "+")TypeReturn = TokenTypes.Number;
      else 
      {
        TypeReturn = tokens[1].TypeReturn;
      }
      return true ;
    }
    else
    {
         TypeReturn = TokenTypes.ErrorType;
         return false;
    }
   }
  }
  
  public class LetIn : Geometrico
  {
    public LetIn(string Value, TokenTypes type, Geometrico Padre) : base(Value, type, Padre)
    {
      this.Value = "let";
      this.Type = TokenTypes.letIn;
    }
  
    public  bool CheckSemantic(List<Errors> errores)
    {
      bool expresion_let = true;
      bool expresion_in = true;
      for (int i = 0; i < variablesLocales.Count; i++)
      {
        expresion_let = expresion_let && variablesLocales[i].CheckSemantic(errores);
      }

      for (int i = 0; i < tokens.Count; i++)
      {
        expresion_in = expresion_in && tokens[i].CheckSemantic(errores);
      }
      if (!expresion_let)
      {
        errores.Add(new Errors(ErrorCode.Semantic, "hay error en la expresion let - in , error en la expresion let "));
        TypeReturn = TokenTypes.ErrorType;
      }
      if(!expresion_in)
      {
        errores.Add(new Errors(ErrorCode.Semantic, "hay error en la expresion let - in , error en la expresion let "));
        TypeReturn = TokenTypes.ErrorType;
      }
      foreach (var item in tokens)
        {
            if(variablesGlobales.Any(valor => valor.Value == item.Value) && item is Function)
            {
                Function auxiliar = (Function)variablesGlobales.Find(valor => valor.Value == item.Value);

                if(auxiliar.variablesLocales.Count != ((Function)item).variablesLocales.Count)
                {
                    errores.Add(new Errors(ErrorCode.Semantic , "la funcion " + item.Value + " no cuenta con esa cantidad de parametros"));
                }
                else
                {
                    for (int i = 0; i < auxiliar.variablesLocales.Count; i++)
                    {
                        if(auxiliar.variablesLocales[i].TypeReturn != ((Function)item).variablesLocales[i].TypeReturn)
                        {
                            errores.Add(new Errors(ErrorCode.Semantic , "En la funcion " + auxiliar.Value + " el parametro " + ((Function)item).variablesLocales[i].Value + " recive un tipo incorrecto"));
                        }
                    }
                }
                
            }
    }
      if (!(expresion_in && expresion_let))
      {
        return false ;
      }
      return true;
    }
    public LetIn Clone ()
    {
      LetIn objeto = new LetIn(this.Value , this.Type , this.Root);
      objeto.tokens = new List<token>();
      foreach (token item in this.tokens)
      {
        objeto.tokens.Add((token)item.Clone());
      }
      objeto.variablesLocales = new List<token>();
      foreach (var item in this.variablesLocales)
      {
        objeto.variablesLocales.Add((token)item.Clone());
      }
      objeto.variablesGlobales = new List<token>();
      foreach (var item in this.variablesGlobales)
      {
        objeto.variablesGlobales.Add((token)item.Clone());
      }
      
      return objeto;
    }
    public token Evaluar ()
    {
      return tokens[0].Evaluar();
    } 
  }
  public class IfElseNode : token
  {
    public IfElseNode(string Value ,  TokenTypes Type ) : base(Value , Type ){}
  
    public token Evaluar()
    {
        if (tokens[0].Evaluar().Value == "1")
        {
        return tokens[1].Evaluar();
        }
        else
        {
         return tokens[2].Evaluar();
        }

    }
    public IfElseNode Clone ()
   {
     IfElseNode objeto = new IfElseNode(this.Value , this.Type);
     foreach (var item in this.tokens)
     {
      objeto.tokens.Add((token)item.Clone());
     }
     return objeto;
   }
   public bool CheckSemantic(List<Errors> errors)
   {
    bool condicion = false;
    bool then = false;
    bool elsse = false ;
    condicion = tokens[0].CheckSemantic(errors);
    if(!condicion)
    {
        errors.Add(new Errors(ErrorCode.Semantic , "error en la condicion de la expresion if - else"));
         TypeReturn = TokenTypes.ErrorType;
         
    }
      then = tokens[1].CheckSemantic(errors);
     if(!then)
    {
      errors.Add(new Errors(ErrorCode.Semantic , "error en la instruccion then de la expresion if - else"));
         TypeReturn = TokenTypes.ErrorType;
    }
      elsse = tokens[2].CheckSemantic(errors);
     if(!elsse)
    {
      errors.Add(new Errors(ErrorCode.Semantic , "error en la instruccion else de la expresion if - else"));
         TypeReturn = TokenTypes.ErrorType;
    }
    if(condicion && then && elsse)
    {
      TypeReturn = tokens[0].TypeReturn;
      return true;
    }
    else
    {
      return false;
    }
   
   }
  }
  public class FunctionNode :token
  {
    
    public FunctionNode (string FunctionName , TokenTypes type ):base (FunctionName , type){}
    
    public TokenNumero Evaluar()
    {
        // Evaluar la función según el nombre
       if (Value == "sin")
       {
        return new TokenNumero (Math.Sin(Double.Parse((tokens[0].Evaluar()).Value)).ToString() , TokenTypes.Number);
       }
       else if (Value == "cos")
       {
        return new TokenNumero (Math.Cos(Double.Parse((tokens[0].Evaluar()).Value)).ToString()  , TokenTypes.Number);
       }
         else if (Value == "tan")
       {
        return new TokenNumero (Math.Tan(Double.Parse((tokens[0].Evaluar()).Value)).ToString() , TokenTypes.Number);
       }
        else 
       {
       return new TokenNumero (Math.Sqrt(Double.Parse((tokens[0].Evaluar()).Value)).ToString() , TokenTypes.Number);
       }
    }
    public FunctionNode Clone ()
    {
        FunctionNode objeto = new FunctionNode(this.Value , this.Type);
        foreach (var item in this.tokens)
        {
          objeto.tokens.Add((token)item.Clone());
        }
        return objeto;
    }
    public bool CheckSemantic(List<Errors> errors)
    {
      bool check = tokens[0].CheckSemantic(errors);
      if(!check || tokens[0].TypeReturn != TokenTypes.Number)
      {
        TypeReturn = TokenTypes.ErrorType;
        return false ;
      }
      else
      {
        TypeReturn = TokenTypes.Number;
        return true ;
      }
    }
  }
  public class TokenSecuencia :token
  {
    public GeoType geoType { get; set; }
    public List<token> secuencias{get ; set ;}
    public TokenSecuencia(string Value, TokenTypes type) : base(Value, type )
    {
       secuencias = new List<token>();
    }
    public bool CheckSemantic(List<Errors> errores)
    {
      bool expresion = false;
      geoType = GeoType.SecuenciaType;
      for (int i = 0; i < this.tokens.Count; i++)
      {
        expresion = expresion && tokens[i].CheckSemantic(errores);
      }
      if (!expresion)
      {
        geoType = GeoType.ErrorType;
        return false;
      }
      return true;
    }
    public token Evaluar()
    {
      return this;
    }
    public TokenSecuencia Clone ()
    {
      TokenSecuencia clone = new TokenSecuencia(this.Value , this.Type);
      foreach (token item in this.tokens)
      {
        clone.tokens.Add((token)item.Clone());
      }
      foreach (token item in this.secuencias)
      {
        clone.secuencias.Add((token)item.Clone());
      }
      return clone;
    }
  }
  public class Underfine : token
  {
    public Underfine(string Value , TokenTypes Type):base(Value , Type)
    {
      Value = "Underfine";
      Type = TokenTypes.Underfine;
    }
    public bool CheckSemantic()
    {
      return true;
    }
    public Underfine Clone ()
    {
      return new Underfine(this.Value , this.Type);
    }
    public Underfine Evaluar()
    {
      return this;
    }
  }
}

