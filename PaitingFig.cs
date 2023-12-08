using TokensGeo;
using Lexer;
using ParserGeo;
public enum Color { black, red, blue, green }

public class Point : token
{
    public int x;
    public int y;

    token coordenada_x {get ; set ;}
    token coordenada_y {get ; set ;}


    public Point(string Value, TokenTypes Type ) : base(Value, Type )
    {
        coordenada_x = null;
        coordenada_y = null;
        Random random = new Random(100);
        x = random.Next(0 , 100);
        Thread.Sleep(100);
        y = random.Next(0 , 100);
    }
    public Point(string Value, TokenTypes Type, token coordenada_x, token coordenada_y) : base(Value, Type )
    {
        this.coordenada_x = coordenada_x;
        this.coordenada_y = coordenada_y;
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
    public Line(string Value, TokenTypes Type) : base(Value, Type) { }

    public Line(string Value, TokenTypes Type, token point1, token point2) : base(Value, Type, point1, point2)
    {
        puntosFigura = Puntos_Recta((Point)point1 ,(Point)point2);
        puntosFigura.Add((Point)point1);
        puntosFigura.Add((Point)point2);
    }
     public List<Point> Puntos_Recta(Point p1, Point p2)
        {
            List<Point> result = new List<Point>();
            for (int x = p1.x; x <= p2.x; x++)
            {
                int y = ((p2.y - p1.y) / (p2.x - p1.x)) * (x - p1.x) + p1.y;
                result.Add(new Point("",TokenTypes.Point , new TokenNumero(x.ToString() , TokenTypes.Number),new TokenNumero( y.ToString() , TokenTypes.Number)));
            }
            return result;
        }
}
public class Segment : FuncionPointsDos
{
    public Segment(string Value, TokenTypes Type) : base(Value, Type) 
    { 
    }

    public Segment(string Value, TokenTypes Type, token point1, token point2) : base(Value, Type, point1, point2) { }
}
public class Ray : FuncionPointsDos
{
    public Ray(string Value, TokenTypes Type) : base(Value, Type) 
    {
        puntosFigura = Puntos_Rayo(p1 ,p2);
        puntosFigura.Add(p1);
        puntosFigura.Add(p2);
    }   

    public Ray(string Value, TokenTypes Type, token point1, token point2) : base(Value, Type, point1, point2) 
    {
         puntosFigura = Puntos_Rayo((Point)point1 ,(Point)point2);
        puntosFigura.Add((Point)point1);
        puntosFigura.Add((Point)point2);
     }

       public List<Point> Puntos_Rayo(Point p1, Point p2)
        {
            List<Point> result = new List<Point>();
            double pendiente_m = (p2.y - p1.y) / (p2.x - p1.x);
            for (int x = p1.x; x <= p2.x; x++)
            {
                int y = (int)pendiente_m * (x - p2.x) + p2.y;
                result.Add(new Point("",TokenTypes.Point ,new TokenNumero(x.ToString(),TokenTypes.Number) , new TokenNumero (y.ToString(), TokenTypes.Number)));
            }
            return result;
        }
}
public class Measure : FuncionPointsDos
{
    public Measure (string Value , TokenTypes Type) : base (Value , Type){}

    public Measure(string Value, TokenTypes Type, token point1, token point2) : base(Value, Type, point1, point2) { }

}

