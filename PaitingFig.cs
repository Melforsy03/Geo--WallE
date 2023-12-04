using TokensGeo;

public enum Color { black, red, blue, green }

public class Point : FuncionPointsDos
{
    public int x;
    public int y;

    public Point(string Value, TokenTypes Type, string name, Color color) : base(Value, Type, name, color)
    {
        Random random = new Random();
        x = random.Next();
        y = random.Next();
    }

    public Point(string Value, TokenTypes Type, token coordenada_x, token coordenada_y, string name, Color color) : base(Value, Type, name, color)
    {
        x = int.Parse(coordenada_x.tokens[0].Value);
        y = int.Parse(coordenada_y.tokens[0].Value);
    }
}
public class Line : FuncionPointsDos
{
    public Line(string Value, TokenTypes Type, string name, Color color) : base(Value, Type, name, color) { }

    public Line(string Value, TokenTypes Type, token point1, token point2, string name, Color color) : base(Value, Type, point1, point2, name, color)
    {
    }
}
public class Segment : FuncionPointsDos
{
    public Segment(string Value, TokenTypes Type, string name, Color color) : base(Value, Type, name, color) { }

    public Segment(string Value, TokenTypes Type, token point1, token point2, string name, Color color) : base(Value, Type, point1, point2, name, color) { }
}
public class Ray : FuncionPointsDos
{
    public Ray(string Value, TokenTypes Type, string name, Color color) : base(Value, Type, name, color) { }

    public Ray(string Value, TokenTypes Type, token point1, token point2, string name, Color color) : base(Value, Type, point1, point2, name, color) { }
}
public class Circle : FuncionPointsDos
{
    public Circle(string Value, TokenTypes Type, string name, Color color) : base(Value, Type, name, color) { }

    public Circle(string Value, TokenTypes Type, token point1, token point2, string name, Color color) : base(Value, Type, point1, point2, name, color) { }
}
public class Arc : FuncionPointsDos
{
    public Arc(string Value, TokenTypes Type, string name, Color color) : base(Value, Type, name, color) { }

    public Arc(string Value, TokenTypes Type, token point1, token point2, string name, Color color) : base(Value, Type, point1, point2, name, color) { }
}