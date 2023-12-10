using TokensGeo;
using Lexer;
using ParserGeo;

public class Point : token
{
    public double x;
    public double y;

   token coordenada_x {get ; set ;}
   token coordenada_y {get ; set;}
    public Point(string Value, TokenTypes Type ) : base(Value, Type )
    {
        Random random = new Random(Guid.NewGuid ().GetHashCode());
        x = random.Next(0 , 100);
        Thread.Sleep(100);
        y = random.Next(0 , 100);
    }
    public Point(string Value, TokenTypes Type, token coordenada_x, token coordenada_y) : base(Value, Type )
    {
        this.x = (coordenada_x.Type != TokenTypes.Identifier) ?  int.Parse(coordenada_x.Value) : int.Parse(coordenada_x.tokens[0].Value );
        this.y =  (coordenada_x.Type != TokenTypes.Identifier) ?  int.Parse(coordenada_y.Value) : int.Parse(coordenada_y.tokens[0].Value );
    }
   
    public bool CheckSemantic(List<Errors> errores)
    {
        bool coordenadas = true;
        GeoType = GeoType.FiguraType;
        if (coordenada_y == null && coordenada_x == null)
        {
            return true;
        }
        if(coordenada_x.Type != TokenTypes.Number || (coordenada_x.tokens.Count != 0 && !coordenada_x.CheckSemantic(errores)) || coordenada_x.GeoType != GeoType.NumberType )
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
        }
        if (!coordenadas)
        {
            return false ;
        }
        return true ;
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

