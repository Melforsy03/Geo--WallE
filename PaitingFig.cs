using TokensGeo;
using Lexer;
using ParserGeo;

 public class Figura :token
  {
    public List<Point> puntosFigura = new List<Point> ();

    public Point p1 {get ; set ;}

    public Point p2 {get ; set ;}

    public Point p3 {get; set ;}

    public string color {get ; set ;}
    public Figura (string Value , TokenTypes Type  , string color) : base(Value , Type)
    {
      p1 = new Point("" , TokenTypes.Point);
      p2 = new Point("", TokenTypes.Point);
      p3 = new Point("", TokenTypes.Point);
      this.color = color ;
    }
    
  }
 public class FuncionPointsDos : Figura
  {
   
      public GeoType GeoType ;

    public FuncionPointsDos(string Value, TokenTypes Type, token point1, token point2 , string color) : base (Value , Type , color)
    {
     this.p1 = (point1.Type != TokenTypes.Identifier) ? (Point)point1 : (Point)(point1.tokens[0]) ;
     this.p2 = (point2.Type != TokenTypes.Identifier) ? (Point)point2 : (Point)(point2.tokens[0]) ;
      puntosFigura = Puntos_Recta(p1 , p2);
      puntosFigura.Add(p1);
      puntosFigura.Add(p2);
    }
    public FuncionPointsDos(string Value, TokenTypes Type , string color) : base(Value, Type , color)
    {
      puntosFigura = Puntos_Recta(p1 , p2);
    }
    public FuncionPointsDos Clone (FuncionPointsDos clonar)
    {
        FuncionPointsDos clon = new FuncionPointsDos(this.Value , this.Type ,this.p1, this.p2, this.color);

        foreach (var item in clonar.puntosFigura)
        {
            clon.puntosFigura.Add(item.Clone());
        }
        return clon;
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
                if ( y > 0)
                result.Add(new Point("",TokenTypes.Point , new TokenNumero(x.ToString() , TokenTypes.Number),new TokenNumero( y.ToString() , TokenTypes.Number)));
            }
            return result;
        }
        public token Evaluar()
        {
            return this;
        }
        public FuncionPointsDos Clone ()
        {
            FuncionPointsDos clone = new FuncionPointsDos(this.Value , this.Type , this.p1, this.p2 , this.color);
            foreach (var item in puntosFigura)
            {
                clone.puntosFigura.Add(item.Clone());
            }
            return clone;
        }
  }
public class Point : token
{
    public double x;
    public double y;

    public Point(string Value, TokenTypes Type ) : base(Value, Type )
    {
        Random random = new Random(Guid.NewGuid ().GetHashCode());
        x = random.Next(0 , 100);
        Thread.Sleep(100);
        y = random.Next(0 , 100);
    }
    public Point(string Value, TokenTypes Type, token coordenada_x, token coordenada_y) : base(Value, Type )
    {
        try
        {
         this.x = (coordenada_x.Type != TokenTypes.Identifier) ?  int.Parse(coordenada_x.Value) : int.Parse(coordenada_x.tokens[0].Value );
         this.y =  (coordenada_x.Type != TokenTypes.Identifier) ?  int.Parse(coordenada_y.Value) : int.Parse(coordenada_y.tokens[0].Value );
        }
        catch (System.Exception)
        {
            throw new ArgumentException("el punto " + Value + " recive dos numeros");
        }
    }
   
    public bool CheckSemantic(List<Errors> errores)
    {
        bool coordenadas = true;
        GeoType = GeoType.FiguraType;
       /* if (coordenada_y == null && coordenada_x == null)
        {
            return true;
        }*/
       /* if(coordenada_x.Type != TokenTypes.Number || (coordenada_x.tokens.Count != 0 && !coordenada_x.CheckSemantic(errores)) || coordenada_x.GeoType != GeoType.NumberType )
        {
           errores.Add(new Errors(ErrorCode.Semantic,"hay error en " + Value + " , el coordenada uno no fue declarado correctamente"));
           coordenadas = false ;
           GeoType = GeoType.ErrorType;
        }
        if (coordenada_y.Type != TokenTypes.Number || (coordenada_y.tokens.Count != 0 && !coordenada_y.CheckSemantic(errores)) ||  coordenada_y.GeoType != GeoType.NumberType) 
        {
           errores.Add(new Errors(ErrorCode.Semantic, "hay error en " + Value + " , el coordenada dos no fue declarado correctamente"));
           coordenadas = false ;
           GeoType = GeoType.ErrorType;
        }*/
        if (!coordenadas)
        {
            return false ;
        }
        return true ;
    }
    public Point Clone ()
    {
        TokenNumero equis = new TokenNumero(x.ToString() , TokenTypes.Number);
        TokenNumero ye = new TokenNumero(y.ToString() , TokenTypes.Number);
        Point clone = new Point (this.Value , this.Type , equis, ye);
        return clone ;
    }
    public Point Evaluar()
    {
        return this;
    }
}
public class Line : FuncionPointsDos
{
    public Line(string Value, TokenTypes Type , string color) : base(Value, Type , color) { }

    public Line(string Value, TokenTypes Type, token point1, token point2 ,string color) : base(Value, Type, point1, point2 , color)
    {
        puntosFigura = Puntos_Recta((Point)point1 ,(Point)point2);
        puntosFigura.Add((Point)point1);
        puntosFigura.Add((Point)point2);
    }
     
}
public class Segment : FuncionPointsDos
{
    public Segment(string Value, TokenTypes Type , string color) : base(Value, Type , color) 
    { 
    }

    public Segment(string Value, TokenTypes Type, token point1, token point2 , string color) : base(Value, Type, point1, point2 , color) { }
}
public class Ray : FuncionPointsDos
{
    public Ray(string Value, TokenTypes Type , string color) : base(Value, Type, color) 
    {
        puntosFigura = Puntos_Rayo(p1 ,p2);
        puntosFigura.Add(p1);
        puntosFigura.Add(p2);
    }   

    public Ray(string Value, TokenTypes Type, token point1, token point2 , string color) : base(Value, Type, point1, point2 , color) 
    {
        puntosFigura = Puntos_Rayo((Point)point1 ,(Point)point2);
        puntosFigura.Add((Point)point1);
        puntosFigura.Add((Point)point2);
     }

       public List<Point> Puntos_Rayo(Point p1, Point p2)
        {
            List<Point> result = new List<Point>();
            double pendiente_m = (p2.y - p1.y) / (p2.x - p1.x);
            for (double x = p1.x; x <= p2.x; x++)
            {
                double y = pendiente_m * (x - p2.x) + p2.y;
                if ( y > 0)
                result.Add(new Point("",TokenTypes.Point ,new TokenNumero(x.ToString(),TokenTypes.Number) , new TokenNumero (y.ToString(), TokenTypes.Number)));
            }
            return result;
        }
}
  public class Measure : FuncionPointsDos
   {
    
    public Measure (string Value , TokenTypes Type , string color) : base (Value , Type , color)
    {
        this.Value = CalculoMedida().ToString();
    }
    public Measure(string Value ,TokenTypes Type, token point1, token point2 , string color) : base(Value, Type, point1, point2 , color) 
    {
        this.Value = CalculoMedida().ToString();
    }
    private double CalculoMedida ()
    {
        return Math.Sqrt(Math.Pow(p2.x - p1.x , 2) + Math.Pow(p2.y - p1.y , 2));
    }

   }
  public class Arco : FuncionPointsDos
      {
        public double  medida {get ; set ;}
        public GeoType GeoTyper {get ; set ;}
        
        public Arco (string Value , TokenTypes Type , string color ) : base (Value , Type , color )
        {
         Random random = new Random(Guid.NewGuid().GetHashCode());
         medida = random.Next(1 , 25);
         puntosFigura = Puntos_Arco(p1 , p2 ,p3 ,medida);
         puntosFigura.Add(p2);
         puntosFigura.Add(p3);
        }
        
        public Arco (string Value , TokenTypes Type , token point1 ,token point2 , token point3 , token medida , string color) :base (Value , Type , color)
        {
           this.p1 = (point1.Type != TokenTypes.Identifier) ? (Point)point1 : (Point)(point1.tokens[0]) ;
           this.p2 = (point2.Type != TokenTypes.Identifier) ? (Point)point2 : (Point)(point2.tokens[0]) ;
           this.p3 = (point1.Type != TokenTypes.Identifier) ? (Point)point3 : (Point)(point3.tokens[0]) ;
           this.medida = (medida.Type != TokenTypes.Identifier || medida.Type is Measure) ? double.Parse(medida.Value) : double.Parse(medida.tokens[0].Value);
           puntosFigura = Puntos_Arco(p1 , p2 ,p3 , this.medida);
            puntosFigura.Add(p2);
            puntosFigura.Add(p3);
            
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
            double angulo = Math.Atan2(p2.y - p1.y , p2.x - p1.x);
            
            List<Point> result = new List<Point>();
            for (int i = 1; i < 20; i++)
            {
                double x = p1.x + radio * (int)Math.Cos(angulo);
                double y = p1.y + radio * (int)Math.Sin(angulo);
                if (x > 0 && y > 0)
                result.Add(new Point("",TokenTypes.Point , new TokenNumero(x.ToString(),TokenTypes.Number ),new TokenNumero(y.ToString() , TokenTypes.Number)));
            }

            return result;
        }
        public  Arco  Clone()
        {
            TokenNumero medida = new TokenNumero(this.medida.ToString(), TokenTypes.Number);
            Arco clone = new Arco (this.Value , this.Type , this.p1, this.p2 , this.p3 , medida , this.color);
            foreach (var item in puntosFigura)
            {
                clone.puntosFigura.Add(item.Clone());
            }
            return clone ;
        }
        public Arco Evaluar()
        {
            return this ;
        }
    }
      public class Circunferencia : Figura
      {
        
        public double medida {get ; set ;}
        public Circunferencia (string Value , TokenTypes Type , string color) :base (Value ,Type , color)
        {
          Random random = new Random(100);
          medida = random.Next(1,20);
          Puntos_Ciscunferencia( (Point)p1 , this.medida);
          
        
        }
        public Circunferencia (string Value , TokenTypes Type , token p1 , token medida , string color ) : base (Value , Type , color)
        {
          this.p1 = (p1.Type != TokenTypes.Identifier)? (Point)p1 : (Point)p1.tokens[0];
          this.medida = (medida.Type != TokenTypes.Identifier) ? double.Parse(medida.Value) : double.Parse(p1.tokens[0].Value);
           Puntos_Ciscunferencia( (Point)p1 , this.medida);
          
        }
        bool puntos = true;
        public  GeoType GeoType {get ; set ;}
        public bool CheckSemantic(List<Errors> errores)
        {
         if (tokens.Count > 0)
         {
            if (tokens.Count != 2)
            {
             errores.Add(new Errors(ErrorCode.Semantic, "la circunferencia recive dos parametros"));
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
         public void Puntos_Ciscunferencia(Point p1, double measure)
        {
            double radio = measure;
            
            for (double x = p1.x ; x < 60; x++)
            {
                double y = Math.Ceiling(p1.y + Math.Sqrt(Math.Pow(measure - (x - p1.x) , 2 )));
                if( y > 0)
                puntosFigura.Add(new Point("",TokenTypes.Point, new TokenNumero(x.ToString(),TokenTypes.Number),new TokenNumero(y.ToString() , TokenTypes.Number)));
            }
            
        }
        public Circunferencia Clone ()
        {
            TokenNumero  med = new TokenNumero(medida.ToString(), TokenTypes.Number);
            Circunferencia clone = new Circunferencia(this.Value , this.Type , this.p1 ,med , this.color);

            foreach (var item in puntosFigura)
            {
                clone.puntosFigura.Add(item.Clone());
            }
            return clone ;
        }
      public Circunferencia Evaluar()
      {
        return this;
      }
    }

