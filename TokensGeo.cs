using Lexer;
using ParserGeo;
namespace TokensGeo
{
  public enum TokenTypes
  {
    Keyword, Identifier, Number, Operator, Punctuation, Point, Condicional,
    Funcion, boolean, letIn, Comando, Line, Segment, Ray, Circle, point_sequence, line_sequence, Underfine, secuencia, Arc , Color, figura , measure
  }
  public interface Evaluacion
  {
    public string Evaluar();
    public bool CheckSemantic(List<Errors> errores);
  }
  public class token : Evaluacion
  {
    public string Value { get; set; }

    public TokenTypes Type { get; set; }
    public GeoType GeoType { get; set; }

    public List<token> tokens { get; set; }

    public token(string Value, TokenTypes Type)
    {
      this.Value = Value;
      this.Type = Type;
      tokens = new List<token>();
    }
    public object Clone ()
    {
      return this.MemberwiseClone();
    }
       public string Evaluar ()
         {
          if(this is Identificador) return ((Identificador)this).Evaluar();
          else if(this is OperatorNode)return ((OperatorNode)this).Evaluar();
          else if(this is TokenNumero)return ((TokenNumero)this).Evaluar();
          else if(this is Function)return((Function)this).Evaluar();
          else if (this is IfElseNode) return ((IfElseNode)this).Evaluar();
          return Value ;
         }
    public virtual bool CheckSemantic(List<Errors> errores)
    {
      bool var = false;
      for (int i = 0; i < tokens.Count(); i++)
      {
        var = var && tokens[i].CheckSemantic(errores);
      }
      return var;
    }
  }
  public class Identificador : token
  {
    public GeoType geoType { get; set; }
    public Identificador(string Value, TokenTypes Type) : base(Value, Type) { }

    public override bool CheckSemantic(List<Errors> errores)
    {
      geoType = GeoType.IdentificadorType;
      return true;
    }
    public string Evaluar ()
        {
          return tokens[0].Evaluar();
        }
  }
  public class tokenBul : token
  {
    public tokenBul(string Value, TokenTypes Type) : base(Value, Type) { }

    public GeoType GeoType { get; set; }
    public override bool CheckSemantic(List<Errors> errores)
    {
      bool parte_izquierda = tokens[0]!.CheckSemantic(errores);
      bool parte_derecha = tokens[1]!.CheckSemantic(errores);
      if (tokens[0]!.GeoType != GeoType.NumberType || tokens[1]!.GeoType != GeoType.NumberType)
      {
        errores.Add(new Errors(ErrorCode.Semantic, "no puede utilizar el operador" + Value + " con estos dos elementos"));
        GeoType = GeoType.ErrorType;
        return false;
      }

      GeoType = GeoType.NumberType;
      return parte_izquierda && parte_derecha;
    }

    public int Evaluar()
    {
      int numero = 0;

      if (Value == "!=")
      {
        numero = double.Parse(tokens[0].Evaluar()) != double.Parse(tokens[1].Evaluar()) ? 1 : 0;
      }
      else if (Value == ">")
      {
        numero = double.Parse(tokens[0].Evaluar()) > double.Parse(tokens[1].Evaluar()) ? 1 : 0;
      }
      else if (Value == "<")
      {
        numero = double.Parse(tokens[0].Evaluar()) < double.Parse(tokens[1].Evaluar()) ? 1 : 0;
      }
      else if (Value == "==")
      {
        numero = double.Parse(tokens[0].Evaluar()) == double.Parse(tokens[1].Evaluar()) ? 1 : 0;
      }
      else if (Value == ">=")
      {
        numero = double.Parse(tokens[0].Evaluar()) >= double.Parse(tokens[1].Evaluar()) ? 1 : 0;
      }
      else if (Value == "<=")
      {
        numero = double.Parse(tokens[0].Evaluar()) <= double.Parse(tokens[1].Evaluar()) ? 1 : 0;
      }

      return numero;
    }
  }

  public class TokenNumero : token
  {
    public GeoType geoType { get; set; }
    public TokenNumero(string Value, TokenTypes Type) : base(Value, Type) { }
    public override bool CheckSemantic(List<Errors> errores)
    {
      geoType = GeoType.NumberType;
      return true;
    }
     public string Evaluar ()
         {
          return Value;
         }
  }
  public class Function : Geometrico
  {
    List<token> parametros {get ; set ;}
    public Function(string Value, TokenTypes Type , Geometrico Root) : base(Value, Type , Root)
    {
      parametros = new List<token>();
    }
    public string Evaluate()
    {
      if (variablesLocales.Any(valor => valor .Value ==  Value))
      {
        Function actual = (Function)variablesLocales.Find(valor => valor.Value == Value).Clone();
        CambioDevariable(actual.parametros);
        return actual.tokens[0].Evaluar();
      }
       if (tokens[0].Type ==  TokenTypes.figura)
       {
         tokens[0].Evaluar();
       }
      return "";
    }
    private void CambioDevariable(List<token> tokenes)
    {
      for (int i = 0; i < tokens.Count; i++)
      {
        tokens[i].tokens.Add((token)variablesLocales[i].Clone());
      }
    }
    private List<token> CambioDevariable2(List<token> cambio)
    {
      if (cambio.Count == 0)
      {
        return cambio ;
      }
      for (int i = 0; i < cambio.Count; i++)
      {
        if (parametros.Any(valor => valor.Value == cambio[i].Value))
        {
          cambio[i] = (token)parametros.Find(valor => valor.Value == Value);
        }
        else
        {
          CambioDevariable2(cambio[i].tokens);
        }
      }
      return cambio;
    }
    
  }
  public class FuncionPointsDos : Figura
  {
    public Point p1 = new Point("punto1" , TokenTypes.Point );
    public Point p2 = new Point ("punto2", TokenTypes.Point);
    public string Value ;

     public List<Point> puntosFigura {get ; set;}
   
    public GeoType GeoType ;

    public FuncionPointsDos(string Value, TokenTypes Type, Point point1, Point point2) : base (Value , Type)
    {
     this.p1 = point1;
     this.p2 = point2;
  
    }
    public FuncionPointsDos(string Value, TokenTypes Type) : base(Value, Type)
    {
      Random random = new Random(100);
      p1!.x = random.Next(1,100);
      Thread.Sleep(100);
      p1.y = random.Next(1,100); 
      Thread.Sleep(100);
      p2!.x = random.Next(1,100);
      Thread.Sleep(100);
      p2.y = random.Next(1,100);
      Thread.Sleep(100);
      
    }
    token punto1;
    token punto2;
    public FuncionPointsDos(string Value, TokenTypes Type , token punto1, token punto2) : base(Value, Type)
    {
      this.Value = Value;
      this.Type = Type;
      this.punto1 = punto1;
      this.punto2= punto2;
    }
    public bool CheckSemantic(List<Errors> errores)
    {
          bool puntos = true ;
          GeoType = GeoType.FiguraType;
          if(!p1.CheckSemantic(errores) || p1 == null)
          {
           errores.Add(new Errors(ErrorCode.Semantic, "hay error en " + Value + " , el punto uno no fue declarado correctamente"));
           puntos = false ;
           GeoType = GeoType.ErrorType;
           }
           if(!p2.CheckSemantic(errores) || p2 == null)
           {
                errores.Add(new Errors(ErrorCode.Semantic, "hay error en " + Value + " , el punto dos no fue declarado correctamente"));
                puntos = false ;
                GeoType = GeoType.ErrorType;  
           }
     
     return puntos ;
    }
   
  }
  public class OperatorNode : token
  {
    public OperatorNode(string Value, TokenTypes Type) : base(Value, Type) { }

    public GeoType GeoType { get; set; }
    public override bool CheckSemantic(List<Errors> errores)
    {
      bool parte_izquierda = tokens[0]!.CheckSemantic(errores);
      bool parte_derecha = tokens[1]!.CheckSemantic(errores);
      if (tokens[0]!.GeoType != tokens[1]!.GeoType)
      {
        errores.Add(new Errors(ErrorCode.Semantic, "no puede utilizar el operador" + Value + " con estos dos elementos"));
        GeoType = GeoType.ErrorType;
        return false;
      }

      GeoType = GeoType.NumberType;
      return parte_izquierda && parte_derecha;
    }

    public string Evaluar()
    {
    
      // Evaluar la operación según el operador   
      if (Value == "+")
      {
      
        return  (double.Parse(tokens[0].Evaluar()) + double.Parse(tokens[1].Evaluar())).ToString();
       
      }
      else if (Value == "-")
      {
           return  (double.Parse(tokens[0].Evaluar()) - double.Parse(tokens[1].Evaluar())).ToString();
      }
      else if (Value == "*")
      {
           return (double.Parse(tokens[0].Evaluar()) * double.Parse(tokens[1].Evaluar())).ToString();
  
      }
      else if (Value == "/")
      {
           return  (double.Parse(tokens[0].Evaluar()) / double.Parse(tokens[1].Evaluar())).ToString();
        
      }
      else if (Value == "%")
      {
           return  (double.Parse(tokens[0].Evaluar()) % double.Parse(tokens[1].Evaluar())).ToString();
    
      }
      else if (Value == "^")
      {
           return  (int.Parse(tokens[0].Evaluar()) ^ int.Parse(tokens[1].Evaluar())).ToString();

      }

      return "";
    }
    private token CalculoSecuencia(token secuencia1 , token secuencia2)
    {
      if (secuencia1 is Underfine || secuencia2 is Underfine || secuencia2.tokens.Count > 25)
      {
        return secuencia1;
      }
      else
      {
       foreach (var item in secuencia2.tokens)
      {
        if (!secuencia1.tokens.Any(obj => obj.Value == item.Value ))
        {
          secuencia1.tokens.Add(item);
        }
      }
      }
      return secuencia1;
    }
  }
   public class Arco : Figura
      {
        public Point p1 = new Point("punto1" , TokenTypes.Point ) ;
        public Point p2 = new Point ("punto2", TokenTypes.Point);
        public Point p3 = new Point ("punto3", TokenTypes.Point);
        public token medida = new Measure("medida", TokenTypes.measure);
         public GeoType GeoTyper {get ; set ;}
         public List<Point> PuntosFigura {get ; set ;}
        public Arco (string Value , TokenTypes Type , Geometrico root ) : base (Value , Type )
        {
        Random random = new Random(100);
         p1!.x = random.Next(1,100);
         Thread.Sleep(100);
         p1.y = random.Next(1,100); 
         Thread.Sleep(100);
         p2!.x = random.Next(1,100);
         Thread.Sleep(100);
         p2.y = random.Next(1,100);
         Thread.Sleep(100);
         p3!.x = random.Next(1 , 100);
         Thread.Sleep(100);
         p3!.y = random.Next(1 , 100);
         medida.Value = random.Next(1 , 25).ToString();
         PuntosFigura = Puntos_Arco(p1 , p2 ,p3 ,int.Parse(medida.Value));
        }
        
        public Arco (string Value , TokenTypes Type , Geometrico root , Point p1 , Point p2 ,Point p3 , token medida) :base (Value , Type)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
        }
         bool puntos = true;


       public bool CheckSemantic(List<Errors> errores)
       {
        GeoType = GeoType.FiguraType;
          for (int i = 0; i < tokens.Count - 1; i++)
         {
           if (tokens[i].Type != TokenTypes.Point) 
           {
           puntos = false;
           errores.Add(new Errors(ErrorCode.Semantic, "hay error en los parametros del arco, el parametro " + i + "deben ser un punto"));
           break;
           }
         }
         if(tokens[3].Type != TokenTypes.measure || tokens[3].Type != TokenTypes.Number || (tokens[3].Type == TokenTypes.Identifier && tokens[3].tokens[0].Type == TokenTypes.Number)) puntos = false;
         if (!puntos)
          {
            GeoType = GeoType.ErrorType;
            errores.Add(new Errors(ErrorCode.Semantic, "hay error en los parametros del arco , se esperaba una medida"));
            return false;
          }
          return true;
      }
       public List<Point> Puntos_Arco(Point p1, Point p2,Point p3, int measure)
        {
            int radio = measure;
            double d = Math.Sqrt(Math.Pow(p3.x - p2.x, 2) + Math.Pow(p3.y - p2.y, 2));
            double angulo = 2 * Math.Asin(d / 2 * radio);
            double angulo1 = angulo / (30 - 1);
            List<Point> result = new List<Point>();
            for (int i = 1; i < 20; i++)
            {
                int x = p1.x + radio * (int)Math.Cos(angulo1 / 2 + i * angulo);
                int y = p1.y + radio * (int)Math.Sin(angulo1 / 2 + i * angulo);
                result.Add(new Point("",TokenTypes.Point , new TokenNumero(x.ToString(),TokenTypes.Number ),new TokenNumero(y.ToString() , TokenTypes.Number)));
            }

            return result;
        }
    }
    public class Circunferencia : Figura
      {
        public Point p1 = new Point("punto", TokenTypes.Point );

        public token punto1 = new Point("punto", TokenTypes.Point );

        public Measure medida = new Measure("medida", TokenTypes.measure);
        
        public  List<Point> PuntosFigura {get ; set ;}
        public Circunferencia (string Value , TokenTypes Type ) :base (Value ,Type )
        {
          Random random = new Random(100);
          p1!.x = random.Next(1,100);
          Thread.Sleep(100);
          p1.y = random.Next(1,100); 
          Thread.Sleep(100);
          medida.Value = random.Next(1,20).ToString();
          PuntosFigura = Puntos_Ciscunferencia(p1 , medida.p2 , int.Parse(medida.Value));
          PuntosFigura.Add(p1);
          PuntosFigura.Add(medida.p2);
        }
        public Circunferencia (string Value , TokenTypes Type ,Geometrico root , token p1 , token medida ) : base (Value , Type )
        {
          this.punto1 = p1;
          this.medida = (Measure)medida;
          PuntosFigura = Puntos_Ciscunferencia((Point)p1 , ((Measure)medida).p2 , int.Parse(medida.Value));
          PuntosFigura.Add((Point)p1);
          PuntosFigura.Add(((Measure)medida).p2);
        }
        bool puntos = true;
        public  GeoType GeoType {get ; set ;}
        public bool CheckSemantic(List<Errors> errores)
        {
         if (tokens.Count > 0)
         {
            if (tokens.Count != 2)
            {
             errores.Add(new Errors(ErrorCode.Semantic, "la circunferencia recive dos paramros"));
             GeoType = GeoType.ErrorType;
             puntos = false ;
            }
            else
            {
              
             if(tokens[0].Type != TokenTypes.Point) 
            {
             errores.Add(new Errors(ErrorCode.Semantic, "hay error en los parametros de la circunferencia , el primer parametro deben ser un punto"));
             GeoType = GeoType.ErrorType;
             puntos = false ;
             }
            if(tokens[1].Type != TokenTypes.Number || tokens[1].Type != TokenTypes.measure)
           {
            errores.Add(new Errors(ErrorCode.Semantic, "hay error en los parametros de la circunferencia , el segundo parametro debe ser un valor numerico o una madiana "));
            GeoType = GeoType.ErrorType;
            puntos = false ;
           }
          }
          if (!puntos)return false;
         }
          return true;
        }
         public List<Point> Puntos_Ciscunferencia(Point p1, Point p2, int measure)
        {
            int radio = measure;
            List<Point> result = new List<Point>();

            for (int i = 1; i < 20; i++)
            {
                int x = p1.x + radio;
                int y = p1.y;
                result.Add(new Point("",TokenTypes.Point, new TokenNumero(x.ToString(),TokenTypes.Number),new TokenNumero(y.ToString() , TokenTypes.Number)));
            }
            return result;
        }
      
      }
  public class LetIn : Geometrico
  {
    public LetIn(string Value, TokenTypes type, Geometrico Padre) : base(Value, type, Padre)
    {
      this.Value = "let";
      this.Type = TokenTypes.letIn;
    }
    public GeoType geoType { get; set; }
    public  bool CheckSemantic(List<Errors> errores)
    {
      bool expresion_let = false;
      bool expresion_in = false;
      for (int i = 0; i < this.variablesLocales.Count; i++)
      {
        expresion_let = expresion_let && variablesLocales[i].CheckSemantic(errores);
      }
      for (int i = 0; i < this.variablesGlobales.Count; i++)
      {
        expresion_in = expresion_in && variablesGlobales[i].CheckSemantic(errores);
      }

      if (!expresion_let)
      {
        errores.Add(new Errors(ErrorCode.Semantic, "hay error en la expresion let - in , error en la expresion let "));
        geoType = GeoType.ErrorType;
      }
      if(!expresion_in)
      {
        errores.Add(new Errors(ErrorCode.Semantic, "hay error en la expresion let - in , error en la expresion let "));
        geoType = GeoType.ErrorType;
      }
      if (!(expresion_in && expresion_let))
      {
        return false ;
      }
      return true;
    }
    public token Evaluate ()
    {
       if (tokens[0].Type == TokenTypes.figura)
       {
        tokens[0].Evaluar();
        return null ;
       }
       else
       {
        return tokens[0];
       }
    }
  }
  public class IfElseNode : Geometrico
  {
    public IfElseNode(string Value, TokenTypes Type, Geometrico Padre) : base(Value, Type, Padre) { }
    public GeoType GeoType { get; set; }
    
    public bool CheckSemantic(List<Errors> errores)
    {
         GeoType = GeoType.NumberType;
      bool condicion = tokens[0]!.CheckSemantic(errores);
      bool parte_if = tokens[1]!.CheckSemantic(errores);
      bool parte_else = tokens[2]!.CheckSemantic(errores);
     
      if (!parte_if || tokens[1] == null||tokens[1].tokens.Count == 0)
      {
        errores.Add(new Errors(ErrorCode.Semantic, "hay error en la expresion if-else , error en la expresion condicional"));
        GeoType = GeoType.ErrorType;
        parte_if = false;
      }
      if(!parte_else || tokens[2] == null || tokens[2].tokens.Count == 0)
      {
        errores.Add(new Errors(ErrorCode.Semantic, "hay error en la expresion if-else , error en la expresion then"));
        GeoType = GeoType.ErrorType;
        condicion = false ;
      }
      if(!condicion || tokens[0] == null ||tokens[0].tokens.Count == 0)
      {
        errores.Add(new Errors(ErrorCode.Semantic, "hay error en la expresion if-else , error en la expresion else "));
        GeoType = GeoType.ErrorType;
        parte_else = false ;
      }


      return parte_if && parte_else && condicion;
    }
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
  public class FunctionNode : Geometrico
  {
    public FunctionNode(string FunctionName, TokenTypes type , Geometrico root) : base(FunctionName, type , root) { }
    public GeoType GeoType { get; set;}
    public  bool CheckSemantic(List<Errors> errores)
    {
      bool argumento = tokens[0]!.CheckSemantic(errores);
    
      if (tokens[0].GeoType != GeoType.NumberType)
      {
        errores.Add(new Errors(ErrorCode.Semantic, "Le debe pasar a la funcion un numero"));
        GeoType = GeoType.ErrorType;
        return false;
      }
    
      GeoType = GeoType.NumberType;
      return argumento;
    }
    public double Evaluar()
    {
      double numero = 0;
      // Evaluar la función según el nombre
      if (Value == "sin")
      {
        numero = Math.Sin(Double.Parse(tokens[0].Evaluar()));
      }
      else if (Value == "cos")
      {
        numero = Math.Cos(Double.Parse(tokens[0].Evaluar()));
      }
      else if (Value == "tan")
      {
       numero = Math.Tan(Double.Parse(tokens[0].Evaluar()));
      }
      else if (Value == "sqrt")
      {
        numero = Math.Sqrt(Double.Parse(tokens[0].Evaluar()));
      }
      return numero;
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
   
    public IEnumerable<token> Intersect(token a, token b)
    {
      IEnumerable<token> tokensA = a.tokens;
      IEnumerable<token> tokensB = b.tokens;
      return tokensA.Intersect(tokensB);
    }
    public IEnumerable<token> samples()
    {
      token[] a = new token[20];

      Random ran = new Random();

      for (int i = 0; i < a.Length; i++)
      {
        a[i] = new Point("p" + i, TokenTypes.Point );
      }
      for (int i = 0; i < a.Length; i++)
      {
        yield return a[ran.Next(a.Length)];
      }
    }
    //devuelve una secuencia de valores positivos 
    public IEnumerable<int> randoms()
    {
      Random ran = new Random();
      for (int i = 0; i < 21; i++)
      {
        yield return ran.Next(1, 101);
      }
    }
  }
  public class Underfine : token
  {
    public Underfine(string Value , TokenTypes Type):base(Value , Type)
    {
      Value = "Underfine";
      Type = TokenTypes.Underfine;
    }
  }
  public class Figura :token
  {
    public List<Point> puntosFigura {get ; set ;}

    public Figura (string Value , TokenTypes Type ) : base(Value , Type)
    {
      puntosFigura = new List<Point>();
    }
  }
}
