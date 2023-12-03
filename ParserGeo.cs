using TokensGeo;

namespace ParserGeo
{
    public class Geometrico : token
    {
    public  string Value {get ; set ;}
    public  TokenTypes Type {get ; set ;}
    public  List<token> variablesLocales {get ; set ;}
    public  List<token> variablesGlobales {get ; set ;}
    public  Geometrico Root {get ; set ;}
    public  List<token> expression {get ;set ;} 
    public int position {get ; set ;}
   public Geometrico (string Value , TokenTypes Type , Geometrico Root) : base (Value , Type )
   {
      this.Value = Value ; 
      this.Type = Type ;
      variablesLocales = new List<token>();
      variablesGlobales = new List<token>();
      this.Root = Root;
   }
    public token Construir()
     {
        return Parser ();
     }
    
    public  token Parser()
    {
       return Expresiones();
    }
    public token Expresiones ()
    {
          token auxiliar = null;
          while (position < expression.Count - 1)
          {
            
            if (Tipos(expression[position].Type))
            {
                this.variablesLocales.Add(expression[position]);
                position++;
                if (expression[position].Value == ";")
                {
                    position++;
                }
                Expresiones();
            }
           
             if ( expression[position].Value == "if" )
            {
                position++;
                auxiliar = ParserIFelse();
                if(auxiliar != null )this.tokens.Add(auxiliar);
                Expresiones();
            }
            else if(expression[position].Value == "let")
            {
               position++;
               auxiliar  = Letin();
               if(auxiliar != null )this.tokens.Add(auxiliar);
               Expresiones();
            }
            else if(expression[position].Value == "draw")
            {
                position++;
                auxiliar = DrawFunction();
                this.tokens.Add(auxiliar);
            }
            else  
            {
                auxiliar = ParseExpression();
                if (expression[position].Value == ",")
                {
                    position++;
                }

                if(auxiliar != null)
                {
                    if (Tipos(auxiliar.Type) || auxiliar.Type == TokenTypes.secuencia)
                    {
                        this.variablesLocales.Add(auxiliar);
                    }
                    else
                    {
                          this.tokens.Add(auxiliar);
                    }
                }
                
            }
          }
            return Root;
    }
//parsea las expresiones 
    private  token ParseExpression()
     {
        token leftNode = ParseTerm();
        if (position == expression.Count)
        {
           return leftNode ;
        }
        
        while (position < expression.Count)
        {
            string c = expression[position].Value;
          
          if (leftNode != null && leftNode is Function && c == ")" )
          {
            if (position + 1 < expression.Count)
            {
                position++;
               c = expression[position].Value ;
            }
          }
          
            //si encuentra una funcion
             if(Isfunction(c))
            {
                int p = position;
                FunctionNode operatorNode = new FunctionNode(c,TokenTypes.Operator );
                operatorNode.tokens[0] = ParseTerm();
                position++;
                operatorNode.tokens[1] = ParseFunction(operatorNode);
                return operatorNode;
  
            }
            else if (c == "if")
            {
                position++;
                return ParserIFelse();
            }
           
            else if (c == "+" || c == "-" || c == "%")
            {
                 OperatorNode operatorNode = new OperatorNode( c,TokenTypes.Operator );
                 operatorNode.tokens.Add(leftNode);
                 position++;
                 operatorNode.tokens.Add(ParseTerm());
                 return operatorNode;
                
            }   
            else if (c == "(")
            {
                return ParseTerm();
            }
            //si encuentra un operador de estos 
            else if (c == ">" || c == "<" || c == "<=" || c == ">=" ||  c == "!=" || c == "==" )
            {
                position++;
                token rigthNode = ParseTerm();
                tokenBul condicion = new tokenBul(c,TokenTypes.Condicional);
                condicion.tokens.Add(leftNode);
                condicion.tokens.Add(rigthNode);
                return condicion;
            }
            //si encuentra un operador de estos 
            else if(c == "&&" || c == "||" )
            {
                position++;
                tokenBul condicion = new tokenBul(c,TokenTypes.Condicional);
                token rigthNode = ParseTerm();
                condicion.tokens.Add((tokenBul)leftNode);
                condicion.tokens.Add((tokenBul)rigthNode);
                return condicion;

            }
            else if (c == "let")
            {
                position++;
                return Letin();
            }
            else
            {
                break;
            }
        }
    return leftNode;
    }
    //si encuentra un operador de estos
    private  token ParseTerm()
    {
        token leftNode = ParseFactor();

        while (position < expression.Count)
        {
            string c = expression[position].Value;

            if (IsOperator(c))
            {
                OperatorNode operatorNode = new OperatorNode(c , TokenTypes.Operator);
                position++;
                token rightNode = ParseFactor();
                 operatorNode.tokens.Add(leftNode);
                operatorNode.tokens.Add(rightNode);
                leftNode = operatorNode;
            }
            else
            {
                break;
            }
        }

        return leftNode;
    }

//este metodo devuelve un numero o en caso de ser un parentesis de apertura entra a la sub cadena 
    private token ParseFactor()
    {
        token node  = null ;
        //esto es por si se encuentra una variable que almacene su valor 
         string c = expression[position].Value;
         //me devuelve una figura 
         if (expression[position] is Identificador && expression[position + 1].Value == "(")
         {
           position = position + 2;
            return FuncionesGeo(c);
         }
         else if((expression[position].Type == TokenTypes.Identifier && expression[position + 1].Value == "=" && TiposFigura(expression[position + 2].Value)) || TiposFigura(c) || variablesLocales.Any(p => p.Value == Value))
        {
             return Figura(c);
         }
         //me devuelve una secuencia 
        else if(LineSecuenceTipo())
        {
            return LineSecuence(expression[position].Value);
        }
        //me devuelve un token numero 
        else if (double.TryParse(c,out double value))
        {
            double val = value;
            position++;
            return new TokenNumero (val.ToString() , TokenTypes.Number );
        }
        //me devuelve una funcion sen , coseno , tan , sqrt
        else if(Isfunction(c))
        {
            position++;
            return ParseFunction(expression[position - 1]);
        }
        //me devuelve un token if- else ;
        else if (c == "if")
        {
            position++;
            return ParserIFelse();
        }
        //me devuelve un token let ;
        else if (c == "let")
        {
            position++;
            return Letin();
        }
        //me devuleve un token draw con los hijos a pintar ;
        else if( c == "draw")
        {
            position++;
            return DrawFunction();
        }
        //devuelve un toquen identificador ;
         else if(expression[position].Type == TokenTypes.Identifier ) 
        {
            position++;
            return expression[position - 1];
        }
        // si es alguno de los caracteres continua el parseo ;
        else if (c == "(" || c == "{" || c == "="  || c == "=>"  )
        {
               position++;
               return ParseExpression();
        }
     
     return node ;
  }
    //Para recorrer las funciones
    private token ParseFunction(token funcion )
{
    token a = ParseExpression();
    funcion.tokens.Add(a);
    return funcion;
            
} 
    private  token  ParserIFelse()
    {
      IfElseNode ifi = new IfElseNode("if" , TokenTypes.Condicional , Root );
      ifi.expression = expression;
      ifi.position = position;
      token a = ifi.ParseExpression();
      ifi.tokens.Add(a);
      ifi.position++;
      token b = ifi.ParseExpression();
      ifi.tokens.Add(b);
      if(expression[position].Value == "(")
      {
       position++;
      }
      ifi.position++;
      token c = ifi.ParseExpression();
      ifi.tokens.Add(c);
      position = ifi.position;
    return ifi;
    }
    private token FuncionesGeo(string NombreFuncion)
    {
        Geometrico Funcion = new Geometrico(NombreFuncion , TokenTypes.Funcion , this);
        Funcion.variablesGlobales.AddRange(variablesGlobales);
        Funcion.variablesGlobales.AddRange(variablesLocales);
        Funcion.position = position;
        Funcion.expression = expression;
         while (expression[Funcion.position].Value != ")")
        {
            Funcion.variablesLocales.Add(Funcion.ParseExpression());
        }
         Funcion.position ++;
        if (expression[Funcion.position].Value == ";")
        {
            position = Funcion.position;
            return Funcion ;
        }
        position = Funcion.position;
        if(expression[Funcion.position].Value == "=") Funcion.position++;
        else if(expression[Funcion.position].Value != "=")return Funcion;
        Funcion.tokens.Add(Funcion.ParseExpression());
        position = Funcion.position;
        return Funcion ;
    }
    //definicion de un figura con nombre 
    private token Figura (string c)
    {
        token fig = new token("", TokenTypes.Identifier);
        if (TiposFigura(c))
        {
         if(c == "line" || c == "segment" ||c == "ray" || c == "measure")
          fig = new FigDeDosPunto(c , ValorNombreFigura(c) ,this);
          else if (c == "circle")
          {
            fig = new Circunferencia (c , ValorNombreFigura(c) , this);
          }
          else if ( c == "arc")
          {
            fig = new Arco(c ,ValorNombreFigura(c) , this);
          }
        }
        else if (variablesLocales.Any (p => p.Value == expression[position + 2].Value ))
        {
            token VariableEncontrada = variablesLocales.Find(p => p.Value == expression[position + 2].Value );
            fig = (token)VariableEncontrada.Clone();
            fig.Value = c ;
            position = position + 3 ;
            return fig ;
        }
        else if(TiposFigura(expression[position + 2].Value)) 
        {
          if(expression[position].Value == "line" || expression[position].Value == "segment" ||expression[position].Value == "ray" || expression[position].Value == "measure")
          fig = new FigDeDosPunto(c , ValorNombreFigura(expression[position + 2].Value) ,this);
          else if (expression[position].Value == "circle")
          {
            fig = new Circunferencia (c , TokenTypes.Circle , this);
          }
          else if (expression[position].Value == "arc")
          {
            fig = new Arco (c , TokenTypes.Arc , this);
          }
           position = position + 3 ;
        }
        for (int i = position ; i < expression.Count; i++)
        {
            if (expression[i].Value == "(" || expression[i].Value == "," || expression[i].Value == ";") continue;
           
            if (expression[i].Value == ")")
            {
                position = i + 1 ;
                break ;
            }
            fig.tokens.Add(expression[position]);
        }
        return fig;
    }
    private  token Letin()
          {
            //nuevo nodo let
              LetIn let = new LetIn("let" , TokenTypes.Keyword , this);
              let.variablesGlobales.AddRange(let.Root.variablesGlobales);
              let.variablesGlobales.AddRange(let.Root.variablesLocales);
              let.expression = expression;
              let.position = position ;
               for (int i = let.position; i < expression.Count; let.position++)
               {
                    if (expression[let.position] is Identificador)
                    {
                       Identificador nodo = (Identificador)expression[let.position];
                       let.position++;
                       nodo.tokens.Add(let.ParseExpression());
                       let.variablesLocales.Add(nodo);
                       if (expression[let.position].Value == ";")
                       {
                        continue;
                       }
                    } 
                       if (expression[let.position].Value == "in")break;
                       
               }
                if (expression[let.position].Value == "in")
                {
                  let.position++;
                  let.tokens.Add(let.ParseExpression()); 
                  position = let.position;    
                }
                 return let ;
            }
    public void ParseFuc(token b)
    {
      variablesLocales.Add(expression[position]);
      Function iden = (Function)b;
      position++;
      if (expression[position].Value == "(")
      {
        position++;
      }
        token coma = new token("," , TokenTypes.Keyword);
        for (int i = position; i < expression.Count; i++)
         {
            if (expression[position] is Identificador)
             {
                Identificador nodo = (Identificador)expression[position];
                position++;
                nodo.tokens.Add(ParseExpression());
                coma.tokens.Add(nodo);
                if (expression[position].Value == ",")
                {
                 position++;
                 continue;
                } 
                if (expression[position].Value == ")")
                {
                 break;
                }
              } 
            }
             if(expression[position].Value == ")")
            {
              position++;
            }
            iden.tokens.Add(coma);
            iden.tokens.Add(ParseExpression());
            Root.variablesLocales.Add(iden);
            
    }
  
   public  bool Isfunction(string c) 
   {
        return c == "sin" || c == "cos" || c == "tan" || c == "Sqrt"  || c == "^";
  }

 public  bool IsOperator(string c)
    {
        return c == "+" || c == "-" || c == "*" || c == "/" || c == "%";
    }
    public static bool Tipos (TokenTypes types)
    {
        return types == TokenTypes.Segment || types == TokenTypes.Point ||types == TokenTypes.Circle || types == TokenTypes.Line || types == TokenTypes.Color || types == TokenTypes.Arc;
    }
    public static bool TiposFigura(string tipoFigura)
    {
        return tipoFigura == "line" || tipoFigura == "segment" || tipoFigura == "circle" || tipoFigura =="point" || tipoFigura == "measure";
    }
    public static bool TipoSecuencia(string TipoSecuencia)
    {
        return TipoSecuencia == "intersect"|| TipoSecuencia == "randoms" || TipoSecuencia == "samples" || TipoSecuencia == "points";
    }
    public  Geometrico LineSecuence(string NombreSecuencia)
   {
    Geometrico secuencia = new Geometrico("" , TokenTypes.secuencia , this );

    for (int i = position; i < expression.Count; i++)
    {
        if (expression[i] is Identificador || expression[i].Value == "rest" || expression[i].Value == "_")
        {
            secuencia.variablesLocales.Add(new TokenSecuencia(expression[i].Value , TokenTypes.secuencia , this));
            i ++;
            if (expression[i].Value == ",")continue;
        }
        // si encuentro una secuencia seguido de una asignacion y un tipo de secuencia 
        if (TipoSecuencia(expression[i].Value))
        {
           Geometrico secuenciaHijo = new Geometrico(expression[i].Value ,TokenTypes.secuencia , this);
          
           if (expression[i].Value != "intersect")
           {
            secuencia.tokens.Add(expression[i]);
            i = i + 3 ;
           }
            if (expression[i].Value == "intersect")
            {
                 Geometrico intersect = new Geometrico("intersect" ,TokenTypes.secuencia , this);
                for (int k = i + 1 ; k < expression.Count; k++)
                {
                    if(expression[k] is Identificador )
                    {
                        intersect.tokens.Add(expression[k]);
                    }
                    else if(expression[k].Value == ")")
                    {
                        i = k + 1;
                        if (secuencia.variablesLocales.Count == 1 || secuencia.variablesLocales.Count == 0)
                        {
                            secuenciaHijo.Value = NombreSecuencia;
                            secuenciaHijo.tokens.Add(intersect);
                            if (expression[i].Value == ";" && secuencia.variablesLocales.Count == 1 )
                            {
                                position = i + 1;
                                return  secuenciaHijo;
                            }
                            else if (secuencia.variablesLocales.Count == 0)
                            {
                                position = i ;
                                return intersect;
                            }
                        }
                        else
                        {
                            secuencia.tokens.Add(intersect);
                            if (expression[i].Value == ";")
                            {
                                position = i + 1;
                                return secuencia;
                            }
                           
                        }
                        break;
                    }
                }     
           }
        }
        if (expression[i].Value == "{")
        {
            for (int j = i + 1; j < expression.Count; j++)
            {
                if (expression[j].Value != "}" && expression[j].Value != ",")
                {
                    secuencia.tokens.Add(expression[j]);
                }
                else if (expression[j].Value =="}")
                {
                     i = j + 1;
                    break;
                }
               
            }
        }
        if (expression[i].Value == ")")
        {
            position = i + 1;
            i++;
        }
       if (expression[i].Value == ";" )
        {
          if (secuencia.variablesLocales.Count == 1)secuencia.Value = NombreSecuencia;
          position = i + 1 ;
          return secuencia;
        }
        position = i ;
    }
    return secuencia;
  }
  public bool LineSecuenceTipo ()
  {
    return ((expression[position].Type == TokenTypes.Identifier && expression[position + 1].Value == ",") || (expression[position + 1].Value == "=" && expression[position + 2].Value == "{")) || (expression[position] is Identificador && expression [position + 1].Value == "=" && TipoSecuencia(expression[position + 2].Value)) || (position + 2 < expression.Count - 1 && expression[position] is Identificador && variablesLocales.Any (p => p.Value == expression[position + 2].Value)) || (expression[position].Value == "_" || expression[position].Value == "rest") || TipoSecuencia(expression[position].Value) ;
  }
  public Geometrico DrawFunction()
  {
    Geometrico drawFuncion = new Geometrico("draw" , TokenTypes.Funcion, this);
    if (TiposFigura (expression[position].Value))
    {
        drawFuncion.Value = expression[position].Value;
        position++;
    }
    drawFuncion.variablesGlobales.AddRange(this.variablesLocales);
    drawFuncion.variablesLocales.AddRange(this.variablesLocales);
    drawFuncion.position = position;
    drawFuncion.expression = expression;
    while (expression[drawFuncion.position].Value != ";")
    {
        drawFuncion.tokens.Add(drawFuncion.ParseExpression());
        if(expression[drawFuncion.position].Value == ",")
        {
            drawFuncion.position++;
        }
        else if (expression[drawFuncion.position].Value == ")")
        {
             while(expression[position].Value != ")" )
        {
            drawFuncion.position++;
            if (expression[drawFuncion.position].Value == ";")
            {
                break; 
            }
        }
        }
        position = drawFuncion.position; 
    } 
    return drawFuncion;
  }
  private TokenTypes ValorNombreFigura (string c )
  {
    if(c == "line")
    {
        return TokenTypes.Line;
    }
    if (c == "segment")
    {
        return TokenTypes.Segment;
    }
    if (c == "circle")
    {
        return TokenTypes.Circle;
    }
    if (c == "ray")
    {
        return TokenTypes.Ray;
    }
    if (c == "point")
    {
       return TokenTypes.Point;
    }
    if (c == "arc")
    {
        return TokenTypes.Arc;
    }
    return TokenTypes.Point;
  }
}
}