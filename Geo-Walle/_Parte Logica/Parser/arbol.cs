using System;
using System.Collections.Generic;
using System.IO;
using Jerarquia;
using Tokenizador;
using static Jerarquia.RaySP;

namespace Arbol
{
 public  class Parser 
 {/// <summary>
 /// posicion por la que se mueve el arbol 
 /// </summary>
    private int position;
    /// <summary>
    /// lista de Tokens a parsear
    /// </summary> <summary>

    public List<Token> expression {get; set;}
    /// <summary>
    /// colores a asignar
    /// </summary> <summary>
    private Stack <string> colores {get ;set ;}
    /// <summary>
    /// hijos de el arbol 
    /// </summary> <summary>
  
    List <Expression> children {get ; set;}
    /// <summary>
    /// constructor del token 
    /// </summary>
    /// <param name="tokens"></param> <summary>
    /// lista de tokens 
    /// </summary>
    /// <param name="tokens"></param>
    public Parser(List <Token> tokens)
    {
        this.expression = tokens;
        colores = new Stack<string>();
        children = new List<Expression>();
    }
  
 
  /// <summary>
  /// Parser
  /// </summary>
  /// <returns>retorna la lista de hijos ya parseados </returns>/
   public  List<Expression>  ParseOne()
   {
    while (position < expression.Count - 1)
    {
           if (expression[position].Value == "import")
            {
             Import();
             if (expression[position].Value == ";")
             position++;
             continue;
            }
            else
            {
                Expression hijo = Parse ();
                if (hijo != null)
                children.Add(hijo);
                if (expression[position].Value == ";")
                 position++;
            }
    }
    return children;
   }
   /// <summary>
   /// primer nivel de parseo por prioridad 
   /// </summary>
   /// <returns>returna una Expression </returns>
   private Expression Parse ()
   {
    return ParseExpressionAndOr();
   }
   /// <summary>
   /// segundo nivel de parseo por prioridad 
   /// </summary>
   /// <returns>retorna una Expression </returns>
   private Expression ParseExpressionAndOr()
   {
        Expression leftNode = ParserExpressionBoolean();
        string c = expression[position].Value;
        if (expression[position].Type == TokenTypes.Color)
        {
            colores.Push(expression[position].Value);
            position ++;
        }
        if (expression[position].Value ==  "restore")
        {
            if (colores.Count > 0)
            colores.Pop();
            position ++;
        }
         if(c == "&&" || c == "||")
            {
                position++;
                Expression rigthNode = ParseExpression();
                BinaryExpression condicion = BinaryNode(c , leftNode , rigthNode);
                if (position < expression.Count &&( expression[position].Value == ")" || expression[position].Value == ";") )
                return condicion;
                if (position > expression.Count- 1)
                return condicion;
                c = expression[position].Value;
                if (aumentoContador() && (c != ">" || c != "<" || c != "<=" || c != ">=" ||  c != "!=" || c != "==" || c != "&&" || c != "||"))
                position++;
                leftNode = condicion;
            }
            return leftNode;
   }
   /// <summary>
   /// tercer nivel de parseo por prioridad
   /// </summary>
   /// <returns>retorna una Expression</returns>
   private Expression ParserExpressionBoolean()
   {
    Expression leftNode = ParseExpression ();
    string c = expression[position].Value;
        if (c == ">" || c == "<" || c == "<=" || c == ">=" ||  c == "!=" || c == "==" )
            {
                position++;
                Expression rigthNode = ParseExpression();
                BinaryExpression condicion = BinaryNode (c , leftNode , rigthNode) ;
               if (position < expression.Count && (expression[position].Value == ")" || expression[position].Value == ";") )
                return condicion;
                if (position > expression.Count - 1)
                return condicion;
                c = expression[position].Value ; 
                if(aumentoContador() &&( c != ">" || c != "<" || c != "<=" || c != ">=" ||  c != "!=" || c != "==" || c != "&&" || c != "||"))
                position++;
                return condicion;
            }
            return leftNode ;
   }
   /// <summary>
   /// cuarto nivel de parseo
   /// </summary>
   /// <returns>retorna una Expression</returns>
  private Expression ParseExpression()
  {
        Expression leftNode = ParseTerm();
        if (position > expression.Count- 1)
        {
           return leftNode ;
        }
        int bucle = 20 ;
        while (position < expression.Count)
        {
            bucle --;
            if(bucle == 0)return leftNode;
            string c = expression[position].Value;
            if (c == ";" || c == "," || c == "in" || c == ")" || c == "else" || c == "=>")
            {
              return leftNode;
            }
         
            //si encuentra un operador de estos
            else if (IsOperator(c))
            {
                 position++;
                 Expression rigth = ParseExpression();
                 BinaryExpression operatorNode = BinaryNode(c , leftNode , rigth);
                if ( position < expression.Count  && (expression[position].Value == ")" || expression[position].Value == ";"))
                return operatorNode;
                if (position > expression.Count -1 )
                return operatorNode;
                if (position < expression.Count )
                {
                c = expression[position].Value;
                if (aumentoContador() && (c != ">" || c != "<" || c != "<=" || c != ">=" ||  c != "!=" || c != "==" || c != "&&" || c != "||" ))
                position++;
                }
                leftNode = operatorNode;
              
            }
            else if (c == "(")
            {
                return ParseTerm();
            }
        }
       return leftNode;
  }
    /// <summary>
    /// quinto nivel de parseo
    /// </summary>
    /// <returns> retorna una Expression</returns>
  private Expression ParseTerm()
  {
        Expression leftNode = ParseFactor();
        
        while (position < expression.Count)
        {
            string c = expression[position].Value;

            if (c == "*" || c == "/" || c == "%")
            {
                position++;
                Expression rightNode = ParseFactor();
                BinaryExpression operatorNode = BinaryNode( c ,leftNode , rightNode);
                if (position < expression.Count &&( expression[position].Value == ")" || expression[position].Value == ";"))
                return operatorNode;
                if(position > expression.Count - 1)
                return operatorNode;
                c = expression[position].Value;
                if (aumentoContador() && (c != ">" || c != "<" || c != "<=" || c != ">=" ||  c != "!=" || c != "==" || c != "&&" || c != "||"))
                position++;
                leftNode = operatorNode;
            }
             
            else
            {
                break;
            }
        }

        return leftNode;
  }
  /// <summary>
  /// sexto y ultimo nivel de parseo
  /// </summary>
  /// <returns> retorna la Expression mas primitiva que puede tener el lenguaje en dependecia de las expresiones a parsear</returns> <summary>

  private Expression ParseFactor()
   {
        Expression node  = null ;
        if(position > expression.Count - 1 ) return node ;
        ///valor del token 
        string c  = expression[position].Value ; 
        //continuar el parseo
        if (c == "(" || c == "{" || c == "="   || c == "=>"  )
        {
            position++;
    
            node = ParseExpression();
        }
        else if (expression[position].Value == "draw")
        {
            position++;
            return DrawFunction();
        }
        else if (c == "randoms")
        {
            return new Ramdoms ();
        }
        else if (c == "samples")
        {
            return new Samples();
        }
        else if (c == "intersect")
        {
            return Intersect();
        }
        else if (c == "count" || c == "points")
        {
            return SecuenciaConteo(c);
        }
        else if (position + 1 < expression.Count - 1 && (c == "function" || expression[position].Type ==  TokenTypes.Identifier && expression[position+1].Value == "("))
        {
           return FuncionesGeo();
        }
        else if (expression[position].Type == TokenTypes.Identifier)
        {
          return Identificador();
        }
         else if (double.TryParse(c,out double value))
        {
            position++;
            return new Number (value);
        }
        else if (c == "if")
        {
            return parserIFelse();
        }
        else if (c == "let")
        {
            position++;
            return Letin();
        }
        else if (expression[position].Type == TokenTypes.Literal)
        {
            position++;
            return new Text();
        }
        else if (Isfunction(c))
        {
            return ParseFunction(c);
        }
        if (TiposFigura(c))
        {
            return Asignacion(c);
        }
      return node ;
   }
   /// <summary>
   /// devuelve una expression funcion 
   /// </summary>
   /// <param name="nombre"> nombre de la funcion llamada </param>
   /// <returns></returns>
  private  Expression ParseFunction( string nombre )
  {
    position++;
    if (expression[position].Value != "(")
    {
    throw new ArgumentException("esperabamos un parentesis de apertura");
    }
    position++;
    Expression hijo = ParseExpression();
    if (expression[position].Value != ")")
    {
        
    throw new ArgumentException("esperabamos un parentesis de cierre");
    }
    position++;

    if (nombre == "sen")
    {
         return new Sen (hijo);
    }
    else if (nombre == "cos")
    {
        return new Cos(hijo);
    }
    else if (nombre == "tan")
    {
        return new Tan(hijo);
    }
    else if (nombre == "log")
    {
        return new Log(hijo);
    }
    else
    {
        return new Squart(hijo);
    }
     
  }
  /// <summary>
  ///
  /// </summary>
  /// <param name="nombre">nombre del tipo de secuencia a retornar </param>
  /// <returns></returns>
  /// <exception cref="ArgumentException"></exception> <summary>
  /// 
  /// </summary>
  /// <param name="nombre"></param>
  /// <returns></returns>
  private Expression SecuenciaConteo (string nombre)
  {
    position ++;
    if (expression[position].Value != "(")
    throw new ArgumentException("esperabamos un parentesis de apertura");
    Expression argumento = Parse ();
    if (nombre == "count")
    return new Count(argumento);
    else
    return new Points(argumento);
  }
  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
  private Expression Intersect ()
  {
    position ++;
    if (expression[position].Value != "(")
    throw new ArgumentException("esperabamos un parentesis de apertura");
    List<Expression> argumento = new List<Expression>();
    while (expression[position].Value != ")")
    {
        if (expression[position].Value == ",")position++;
        argumento.Add(Parse());
        if (expression [position].Value != "," && expression[position].Value != ")")
        throw new ArgumentException("esperabamos un parentesis de cierre");
    }
    position++;
    if (argumento.Count != 2)
    throw new ArgumentException("la secuencia intersect trabaja con dos elementos" );
    return new Intersect(argumento);
  }
  /// <summary>
  /// Parsea un identificador 
  /// </summary>
  /// <returns>retorna una expresion tipo ID</returns>
  private Expression Identificador()
  {
    if (expression[position + 1].Value == "=")
    {
        position += 2;
        return new AssingExpression(expression[position - 2] , Parse());
    }
    else
    position++;
    return new ID( expression[position-1] , null);
  }
  /// <summary>
  /// Parsea la funcion If -else 
  /// </summary>
  /// <returns>retorna una expresion tipo If - else </returns>
  /// <exception cref="ArgumentException"></exception> <summary>
  /// la excepciones lanzadas en este metodo se asegurar del correcto parseo de la expresion
  /// </summary>
  /// <returns></returns>
  private Expression parserIFelse()
   {
       position++;
      if (expression[position].Value != "(")
      throw new ArgumentException("esperabamos un (");
      else
      {
        position++;
      }
      Expression condition = Parse();
      if (condition == null)
      throw new ArgumentException("la condicion del if no puede ser nula");
       if (expression[position].Value != ")")
       {
            throw new ArgumentException("esperabamos un (");
        }
        else
        {
            position++;
        }
      //parsea el cuerpo then 
      Expression then = ParseExpression();
       if (then == null)
      throw new ArgumentException("la expresion then  del if no puede ser nula");
      //parsea el cuerpo Else
      if (expression[position].Value != "else")
      {
        throw new ArgumentException("esperabamos la palabra clave else");
      }
      position++;
      Expression Else = ParseExpression();
       if (Else == null)
      throw new ArgumentException("la expresion else del if no puede ser nula");
      if ( position > expression.Count - 1 )
       {
            throw new ArgumentException("esperabamos un ;");
       }
       if (position < expression.Count && expression[position].Value == ")" )
       {
            throw new ArgumentException("esperabamos un (");
       }
        return new If_them_else(condition , then , Else);
     
   }
   /// <summary>
   /// se encarga de parsear las funciones declaradas en el lenguaje 
   /// </summary>
   /// <returns>retorna una expresion tipo funcion</returns>
   /// <exception cref="ArgumentException"></exception> <summary>
   /// la excepciones lanzadas en este metodo se asegurar del correcto parseo de la expresion
   /// </summary>
   /// <returns></returns>
  private Expression FuncionesGeo()
    {
        string Name = "";
         if(expression[position].Value == "function")
        {
            position++;
        }
        if (expression[position].Type == TokenTypes.Identifier )
        {
            Name = expression[position].Value;
            position++;
        }
        if(expression[position].Value != "(")
        {
            throw new ArgumentException("esprabamos un parentesis de apertura despues declarada la funcion");
        }
        else
        {
            position++;
        }
        List <Token> Argument = new List <Token> ();
        while (expression[position].Value != ")")
        {
            if (expression[position].Type != TokenTypes.Identifier && expression[position].Value !=  ")" && expression[position].Value != ",")
            {
                throw new ArgumentException("esperabamos un parentesis de cierre");
            }
            else if ( expression[position].Value == ",")
            {
                position++;
            }
            else
            {
                Argument.Add(expression[position]);
                position++;
            }
        }

        position++;
        Expression body = ParseExpression();
        if (expression[position].Value != ";")
        throw new ArgumentException("esperabamos un punto y coma");
        
        Function function = new Function (Name, Argument , body);
        return function;
    }
    /// <summary>
    /// parseo de la funcion Draw
    /// </summary>
    /// <returns>retorna una expresion</returns>
  private  Expression DrawFunction()
   {
   
    if(expression[position].Value != "(")
    {
        throw new ArgumentException("esperabamos un parentesis de apertura");
    }
    position++;

    Expression Argument = ParseExpression();

    if (expression[position].Value != ")")
    {
        throw new ArgumentException("esperabamos un parentesis de cierre");
    }
    position++;

    return new Draw(Argument,colores.Peek());

   }
   /// <summary>
   /// Parsea la funcion Let-in
   /// </summary>
   /// <returns> retorna una expresion tipo let - in </returns>
   /// <exception cref="ArgumentException"></exception> <summary>
   /// la excepciones lanzadas en este metodo se asegurar del correcto parseo de la expresion
   /// </summary>
   /// <returns></returns>
  private Expression Letin()
  {
        List <Expression> let = new List<Expression>();
        while(expression[position].Value != "in")
        {
             let.Add(ParseExpression()) ;
             if (expression[position].Value != ";")
             throw new ArgumentException("esperabamos un ;");
             position++;
        }
        position++;
        Expression In = ParseExpression();
        if (expression[position].Value != ";")
        {
          throw new ArgumentException("esperabamos un ;");
        }
         return new LetInExpression(let , In );
  }
  /// <summary>
  /// Parsea las figurar
  /// </summary>
  /// <param name="figura"> nombre de la figura a crear </param>
  /// <returns>una expresion tipo figura</returns>
  /// <exception cref="ArgumentException"></exception> <summary>
  /// la excepciones lanzadas en este metodo se asegurar del correcto parseo de la expresion
  
  private Expression Figura (string figura)
   { 
    if (expression[position].Value != "(")
    throw new ArgumentException("esperabamos un parentesis de apertura");
    position++;
    List<Expression> argument = new List<Expression>();
    while (expression[position].Value != ")")
    {
        if(expression[position].Value == ",")position++;
        argument.Add(ParseExpressionAndOr());
        if(expression[position].Value != ")" && expression[position].Value != ",")
        throw new ArgumentException("esperabamos un parentesis de cierre");
    }
    position++;
    if(figura == "line")
    return new Line(argument);
    if(figura == "arc")
    return new Arc(argument);
    if(figura == "segment")
    return new Segment(argument);
    if (figura == "circle")
    return new Circle(argument);
    if (figura == "ray")
    return new Ray(argument);
    else
    {
        return new Point(argument);
    }
   }
/// <summary>
/// Parsea una asignacion (=)
/// </summary>
/// <param name="figuraNombre">nombre de la figura a contruir y asignar un identificador</param>
/// <returns></returns>
/// <exception cref="ArgumentException"></exception> <summary>
/// la excepciones lanzadas en este metodo se asegurar del correcto parseo de la expresion

  private Expression Asignacion(string figuraNombre)
   {
    position++;
     if (expression[position].Type != TokenTypes.Identifier && expression[position].Value != "(")
     {
        throw new ArgumentException("esperabamos un (");
     }
     Expression figura ;
     if (expression[position].Type == TokenTypes.Identifier && expression[position + 1].Value == "(")
     {
        Token auxiliar = expression[position];
         position++;
         figura = Figura(figuraNombre);
         return new AssingExpression(auxiliar , figura);
     }
     else if (expression[position].Type == TokenTypes.Identifier && expression[position + 1].Value == ";" )
     {
        if(figuraNombre == "line")
        figura = new LineSP();
        else if(figuraNombre == "arc")
        figura = new ArcSP();
        else if(figuraNombre == "segment")
        figura = new SegmentSP();
        else if (figuraNombre == "circle")
        figura = new CircleSP();
        else if (figuraNombre == "ray")
         figura =  new RaySP();
        else
        {
        figura = new PointSP();
        }
        position++;
        return new AssingExpression(expression[position - 1] , figura);
     }
     else
     {
       return new AssingExpression(null , Figura(figuraNombre));
     }
   }
   /// <summary>
   ///  
   /// </summary>
   /// <param name="operador">simbolo del operador para el cual se debe crear la expresion binaria</param>
   /// <param name="left">argumento de la expresion a contruir</param>
   /// <param name="rigth">argumento de la expresion a contruir</param>
   /// <returns></returns>  devuelve un Expresion binaria <summary>
 
   /// <returns></returns>
  private BinaryExpression BinaryNode (string operador , Expression left , Expression rigth)
  {
    if (operador == "+")
    return new Add(left , rigth);
    else if (operador == "-")
    return new Sub (left , rigth);
    else if (operador == "*")
    return new Start(left , rigth);
    else if (operador == "/")
    return new Div (left, rigth);
    else if (operador == "^")
    return new Pow (left , rigth);
    else if (operador == "%")
    return new Mod(left, rigth);
    else if (operador == ">")
    return new MayorExpression( left, rigth);
    else if (operador == "<")
    return new MenorExpression(left , rigth);
    else if (operador == ">=")
    return new MayorEqualExpression(left , rigth);
    else if (operador == "==")
    return new DistintExpression(left , rigth);
    else if (operador == "||")
    return new OrExpression(left , rigth);
    else
    {
        return new AndExpression(left, rigth);
    }
    
  }
  /// <summary>
  /// importa codigos previamente guardados en un archivo 
  /// </summary> <summary>
 
  private void Import ()
  {
        position++;
        string nombreArchivo = expression[position].Value + ".txt";
         string directorioActual = Environment.CurrentDirectory;
        // Construye la ruta completa del archivo
        string rutaArchivo = Path.Combine(directorioActual,"content");
        string rutaOficial = Path.Combine (rutaArchivo , nombreArchivo);
        string contenido = File.ReadAllText(rutaOficial);
        List<Token> importaciones = lexer.TokenizeString(contenido);
        Parser import = new Parser (importaciones);
        List<Expression> importacion = new List<Expression>();
        importacion = import.ParseOne();
        children.AddRange(importacion);
        position++;

  }
  private bool aumentoContador()
   {
    return position < expression.Count - 1 && expression[position].Value != ";" && !IsOperator(expression[position].Value) && !Isfunction(expression[position].Value) && expression[position].Value != "in" && expression[position].Value != "else" && expression[position].Value == "," && expression[position].Value != ";" ;
   }
  private bool Isfunction(string c) 
    {
        return c == "sin" || c == "cos" || c == "tan" || c == "sqrt"  || c == "^";
    }
  private  bool IsOperator(string c)
    {
        return c == "+" || c == "-" || c == "*" || c == "/" || c == "%";
    }
  private  bool TiposFigura(string tipoFigura)
    {
        return tipoFigura == "line" || tipoFigura == "segment" || tipoFigura == "circle" ||tipoFigura =="point"|| tipoFigura == "measure" || tipoFigura == "arc" || tipoFigura == "ray";
    }
  private bool TipoSecuencia(string TipoSecuencia)
    {
        return TipoSecuencia == "intersect"|| TipoSecuencia == "randoms" || TipoSecuencia == "samples" || TipoSecuencia == "points";
    }
 
 }
}

