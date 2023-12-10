using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
     string Evaluar();
     bool CheckSemantic(List<Errors> errores);
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
      bool parte_izquierda = tokens[0].CheckSemantic(errores);
      bool parte_derecha = tokens[1].CheckSemantic(errores);
      if (tokens[0].GeoType != GeoType.NumberType || tokens[1].GeoType != GeoType.NumberType)
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
   
     public List<Point> puntosFigura {get ; set;}
   
    public GeoType GeoType ;

    public FuncionPointsDos(string Value, TokenTypes Type, token point1, token point2 , string color) : base (Value , Type , color)
    {
     this.p1 = (point1.Type != TokenTypes.Identifier) ? (Point)point1 : (Point)(point1.tokens[0]) ;
     this.p2 = (point2.Type != TokenTypes.Identifier) ? (Point)point2 : (Point)(point2.tokens[0]) ;
    }
    public FuncionPointsDos(string Value, TokenTypes Type , string color) : base(Value, Type , color)
    {
      puntosFigura = Puntos_Recta(p1 , p2);
      
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
    public List<Point> Puntos_Recta(Point p1, Point p2)
        {
            List<Point> result = new List<Point>();
            double menos = (p1.x < p2.x) ? p1.x : p2.x;
            double mayor = (p1.x > p2.x) ? p1.x : p2.x;
             for (double x = menos; x <= mayor; x++)
            {
                double y = Math.Ceiling((((p2.y - p1.y) /(p2.x - p1.x) + 1)  * (x - p1.x) + p1.y));
                result.Add(new Point("",TokenTypes.Point , new TokenNumero(x.ToString() , TokenTypes.Number),new TokenNumero( y.ToString() , TokenTypes.Number)));
            }
            return result;
        }
   
  }
  public class OperatorNode : token
  {
    public OperatorNode(string Value, TokenTypes Type) : base(Value, Type) { }

    public GeoType GeoType { get; set; }
    public override bool CheckSemantic(List<Errors> errores)
    {
      bool parte_izquierda = tokens[0].CheckSemantic(errores);
      bool parte_derecha = tokens[1].CheckSemantic(errores);
      if (tokens[0].GeoType != tokens[1].GeoType)
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
  public class Arco : FuncionPointsDos
      {
        public double  medida {get ; set ;}
        public GeoType GeoTyper {get ; set ;}
        public List<Point> PuntosFigura {get ; set ;}
        public Arco (string Value , TokenTypes Type , string color ) : base (Value , Type , color )
        {
         Random random = new Random(Guid.NewGuid().GetHashCode());
         medida = random.Next(1 , 25);
         //PuntosFigura = Puntos_Arco(p1 , p2 ,p3 ,medida);
        }
        
        public Arco (string Value , TokenTypes Type , token point1 ,token point2 , token point3 , token medida , string color) :base (Value , Type , color)
        {
           this.p1 = (point1.Type != TokenTypes.Identifier) ? (Point)point1 : (Point)(point1.tokens[0]) ;
           this.p2 = (point2.Type != TokenTypes.Identifier) ? (Point)point2 : (Point)(point2.tokens[0]) ;
           this.p3 = (point1.Type != TokenTypes.Identifier) ? (Point)point3 : (Point)(point3.tokens[0]) ;
           this.medida = (medida.Type != TokenTypes.Identifier) ? int.Parse(medida.Value) : int.Parse(medida.tokens[0].Value);
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
       public List<Point> Puntos_Arco(Point p1, Point p2,Point p3, double measure)
        {
            double radio = measure;
            double d = Math.Sqrt(Math.Pow(p3.x - p2.x, 2) + Math.Pow(p3.y - p2.y, 2));
            double angulo = 2 * Math.Asin(d / 2 * radio);
            double angulo1 = angulo / (30 - 1);
            List<Point> result = new List<Point>();
            for (int i = 1; i < 20; i++)
            {
                double x = p1.x + radio * (int)Math.Cos(angulo1 / 2 + i * angulo);
                double y = p1.y + radio * (int)Math.Sin(angulo1 / 2 + i * angulo);
                result.Add(new Point("",TokenTypes.Point , new TokenNumero(x.ToString(),TokenTypes.Number ),new TokenNumero(y.ToString() , TokenTypes.Number)));
            }

            return result;
        }
    }
  public class Circunferencia : Figura
      {
        public Point p1 {get ; set ;}

        public int medida {get ; set ;}
        

        public Circunferencia (string Value , TokenTypes Type , string color) :base (Value ,Type , color)
        {
          Random random = new Random(100);
          
          medida = random.Next(1,20);
          //PuntosFigura = Puntos_Ciscunferencia(p1 , medida.p2 , int.Parse(medida.Value));
          //PuntosFigura.Add(p1);
          //PuntosFigura.Add(medida.p2);
        }
        public Circunferencia (string Value , TokenTypes Type , token p1 , token medida , string color ) : base (Value , Type , color)
        {
          this.p1 = (p1.Type != TokenTypes.Identifier)? (Point)p1 : (Point)p1.tokens[0];
          this.medida = (medida.Type != TokenTypes.Identifier) ? int.Parse(medida.Value) : int.Parse(p1.tokens[0].Value);
         Puntos_Ciscunferencia( (Point)p1 , this.medida);
          puntosFigura.Add((Point)p1);
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
         public void Puntos_Ciscunferencia(Point p1, int measure)
        {
            int radio = measure;
            
            for (double x = p1.x ; x < 60; x++)
            {
                double y = Math.Ceiling(p1.y + Math.Sqrt(Math.Pow(measure - (x - p1.x) , 2 )));
                puntosFigura.Add(new Point("",TokenTypes.Point, new TokenNumero(x.ToString(),TokenTypes.Number),new TokenNumero(y.ToString() , TokenTypes.Number)));
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
      bool condicion = tokens[0].CheckSemantic(errores);
      bool parte_if = tokens[1].CheckSemantic(errores);
      bool parte_else = tokens[2].CheckSemantic(errores);
     
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
      bool argumento = tokens[0].CheckSemantic(errores);
    
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

    public Point p1 {get ; set ;}

    public Point p2 {get ; set ;}

    public Point p3 {get; set ;}

    public string color {get ; set ;}
    public Figura (string Value , TokenTypes Type  , string color) : base(Value , Type)
    {
      p1 = new Point("" , TokenTypes.Point);
      p2 = new Point("", TokenTypes.Point);
      p3 = new Point("", TokenTypes.Point);
      puntosFigura = new List<Point>();
      this.color = color ;
    }
  }
}
