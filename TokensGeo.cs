using Lexer;
using ParserGeo;
namespace TokensGeo
{
  public enum TokenTypes
  {
    Keyword, Identifier, Number, Operator, Punctuation, Point, Condicional,
    Funcion, boolean, letIn, Comando, Line, Segment, Ray, Circle, point_sequence, line_sequence, Underfine, secuencia, Arc , Color
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
          else if(this is OperatorNode)return ((OperatorNode)this).Evaluar().ToString();
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
    
    public Function(string Value, TokenTypes Type , Geometrico Root) : base(Value, Type , Root){}
    
  }
  public class FuncionPointsDos : token
  {
    public Point p1;
    public Point p2;
    private string Value ;
    private Color color;

    public FuncionPointsDos(string Value, TokenTypes Type, token point1, token point2, string name, Color color) : base (Value , Type)
    {
      p1!.x = int.Parse(point1.tokens[0].Value);
      p1.y = int.Parse(point1.tokens[1].Value);

      p2!.x = int.Parse(point2.tokens[0].Value);
      p2.y = int.Parse(point2.tokens[1].Value);

      this.color = color;
    }
    public FuncionPointsDos(string Value, TokenTypes Type, string name, Color color) : base(Value, Type)
    {
      Random random = new Random(1000);
      p1!.x = random.Next(1,100);
      p1.y = random.Next(1,100); 
      p2!.x = random.Next(1,100); 
      p2.y = random.Next(1,100);
      this.color = color;
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
      if (tokens[0]!.GeoType != GeoType.NumberType || tokens[1]!.GeoType != GeoType.NumberType)
      {
        errores.Add(new Errors(ErrorCode.Semantic, "no puede utilizar el operador" + Value + " con estos dos elementos"));
        GeoType = GeoType.ErrorType;
        return false;
      }

      GeoType = GeoType.NumberType;
      return parte_izquierda && parte_derecha;
    }

    public double Evaluar()
    {
      // Evaluar la operación según el operador        
      double numero = 0;

      if (Value == "+")
      {
        numero = double.Parse(tokens[0].Evaluar()) + double.Parse(tokens[1].Evaluar());
      }
      else if (Value == "-")
      {
        numero = double.Parse(tokens[0].Evaluar()) - double.Parse(tokens[1].Evaluar());
      }
      else if (Value == "*")
      {
        numero = double.Parse(tokens[0].Evaluar()) * double.Parse(tokens[1].Evaluar());
      }
      else if (Value == "/")
      {
        numero = double.Parse(tokens[0].Evaluar()) / double.Parse(tokens[1].Evaluar());
      }
      else if (Value == "%")
      {
        numero = double.Parse(tokens[0].Evaluar()) % double.Parse(tokens[1].Evaluar());
      }
      else if (Value == "^")
      {
        numero = int.Parse(tokens[0].Evaluar()) ^ int.Parse(tokens[1].Evaluar());
      }

      return numero;
    }
  }
  //figuras de dos puntos como , el segmento , el rayo , medida  entre dos puntos 
  public class FigDeDosPunto : token
  {
    public token a { get; set; }
    public token b { get; set; }
    public FigDeDosPunto(string Value, TokenTypes Type, token a, token b) : base(Value, Type)
    {
      this.Value = Value;
      this.Type = Type;
      this.a = a;
      this.b = b;
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
    public LetIn(string Value, TokenTypes type, Geometrico Padre) : base(Value, type, Padre)
    {
      this.Value = "let";
      this.Type = TokenTypes.letIn;
    }
    public GeoType geoType { get; set; }
    public override bool CheckSemantic(List<Errors> errores)
    {
      bool expresion_let = false;
      bool expresion_in = false;
      for (int i = 0; i < this.variablesLocales.Count; i++)
      {
        expresion_let = expresion_let && variablesLocales[i].CheckSemantic(errores);
      }
      for (int i = 0; i < this.expression.Count; i++)
      {
        expresion_in = expresion_in && expression[i].CheckSemantic(errores);
      }

      if (!(expresion_let && expresion_in))
      {
        geoType = GeoType.ErrorType;
        return false;
      }

      return true;
    }
  }
  public class IfElseNode : Geometrico
  {
    public IfElseNode(string Value, TokenTypes Type, Geometrico Padre) : base(Value, Type, Padre) { }
    public GeoType GeoType { get; set; }
    public override bool CheckSemantic(List<Errors> errores)
    {
      bool parte_if = tokens[1]!.CheckSemantic(errores);
      bool parte_else = tokens[2]!.CheckSemantic(errores);

      if (tokens[1]!.GeoType != GeoType.NumberType)
      {
        errores.Add(new Errors(ErrorCode.Semantic, "hay error con la expresion if-else"));
        GeoType = GeoType.ErrorType;
        return false;
      }

      GeoType = GeoType.NumberType;
      return parte_if && parte_else;
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
  public class FunctionNode : token
  {
    public FunctionNode(string FunctionName, TokenTypes type) : base(FunctionName, type) { }
    public GeoType GeoType { get; set; }
    public override bool CheckSemantic(List<Errors> errores)
    {
      bool argumento = tokens[0]!.CheckSemantic(errores);

      if (tokens[0]!.GeoType == GeoType.NumberType)
      {
        errores.Add(new Errors(ErrorCode.Semantic, "Le debe pasar a la funcion un nmero"));
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
        if (tokens[0].Type == TokenTypes.Operator)
        {
          numero = Math.Sin(((OperatorNode)tokens[0]).Evaluar());
        }
        else
        {
          numero = Math.Sin(Double.Parse(tokens[0].Evaluar()));
        }
      }
      else if (Value == "cos")
      {
        if (tokens[0].Type == TokenTypes.Operator)
        {
          numero = Math.Cos(((OperatorNode)tokens[0]).Evaluar());
        }
        else
        {
          numero = Math.Cos(Double.Parse(tokens[0].Evaluar()));
        }
      }
      else if (Value == "tan")
      {
        if (tokens[0].Type == TokenTypes.Operator)
        {
          numero = Math.Tan(((OperatorNode)tokens[0]).Evaluar());
        }
        else
        {
          numero = Math.Tan(Double.Parse(tokens[0].Evaluar()));
        }
      }
      else if (Value == "sqrt")
      {
        if (tokens[0].Type == TokenTypes.Operator)
        {
          numero = Math.Sqrt(((OperatorNode)tokens[0]).Evaluar());
        }
        else
        {
          numero = Math.Sqrt(Double.Parse(tokens[0].Evaluar()));
        }
      }
      return numero;
    }
  }
  public class TokenSecuencia : token
  {
    public List<TokenSecuencia> secuencias { get; set; }
    public List<string> FuncionesEjecutar { get; set; }
    public token Padre { get; set; }
    public GeoType geoType { get; set; }

    public TokenSecuencia(string Value, TokenTypes type, token Padre) : base(Value, type)
    {
      secuencias = new List<TokenSecuencia>();
      FuncionesEjecutar = new List<string>();
      this.Padre = Padre;
    }
    public override bool CheckSemantic(List<Errors> errores)
    {
      bool expresion = false;

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
    public IEnumerable<token> Underscore(IEnumerable<token> a)
    {
      return a.Skip(1);
    }
    //devuelve el primer termino de la secuencia
    public token Rest(IEnumerable<token> a)
    {
      IEnumerator<token> c = a.GetEnumerator();
      c.MoveNext();
      return c.Current;
    }
    public IEnumerable<token> Intersect(token a, token b)
    {
      return a.tokens.Intersect(b.tokens);
    }
    public IEnumerable<token> samples()
    {
      token[] a = new token[20];

      Random ran = new Random();

      for (int i = 0; i < a.Length; i++)
      {
        a[i] = new token("p" + i, TokenTypes.Point);
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
}
