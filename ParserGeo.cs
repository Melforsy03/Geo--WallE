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
                variablesLocales.Add(expression[position]);
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
                tokens.Add(auxiliar);
            }
            else  
            {
               
                auxiliar = ParseExpression();

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
          if (leftNode != null && leftNode is OperatorNode && c == ")")
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
        token k = FinIndeX(expression[position] , this);
        if (k != null)
        {
           node = k;
           position++;
        }
        else if( Encuentro(expression[position].Value , this.variablesLocales) != -1 || Encuentro(expression[position].Value , variablesLocales) != -1 )
        {
            Function funcion = new Function(expression[position].Value , TokenTypes.Funcion );
            position++;
            token parametros = new token("," , TokenTypes.Keyword);
            if (expression[position].Value == "(")
            {
                position++;
            }
            for (int i = position; i < expression.Count; i++)
            {
                parametros.tokens.Add(ParseExpression());
                       if (expression[position].Value == ",")
                       {
                        position++;
                        continue;
                       } 
                       if (expression[position].Value == ")")
                       {
                        position++;
                        break;
                       }
                    }
                     funcion.tokens.Add(parametros);
                     
                     if (this.variablesLocales.Contains(funcion))
                     {
                        int j = Encuentro(funcion.Value , Root.variablesLocales);
                        if (funcion.tokens[0].tokens.Count != Root.variablesLocales[j].tokens[0].tokens.Count )
                        {
                            Console.WriteLine("error semantico , la funcion " + funcion.Value  + " no recive " + funcion.tokens[0].tokens .Count + " paramatros");
                            throw new ArgumentException();
                        }
                       funcion = (Function)Root.variablesLocales[j];
                       for (int l = 0; l < funcion.tokens[0].tokens.Count ; l++)
                       {
                       
                        funcion.tokens[0].tokens[l].tokens[0] = parametros.tokens[l];
                       
                       }
                      funcion.globales.Add(Root.variablesLocales[j]);
                      return funcion;
                     }
                    node = funcion;
        }
        else if(position >= 2 && expression[position].Type == TokenTypes.Identifier && expression[position - 2].Type == TokenTypes.Funcion)
        {
            node = expression[position];
            variablesLocales.Add(node);
            position++;
        }
        else if(Encuentro(expression[position].Value , variablesLocales) != - 1)
        {
            node = variablesLocales[Encuentro(expression[position].Value , variablesLocales)];
            position++;
        }
        else
        {
         string c = expression[position].Value;
         if (position > 0 && expression[position].Type == TokenTypes.Identifier && expression[position - 1 ].Value == "let")
        {
            Identificador iden = (Identificador)expression[position];
            position++;
            iden.tokens.Add(ParseExpression());
            Root.variablesLocales.Add(iden);
            node = ParseExpression();
        }
         else if( expression[position+ 1].Value == "=" && TiposFigura(expression[position + 2].Value))
         {
             node = Figura(c);
         }
        else if((expression[position].Type == TokenTypes.Identifier && expression[position + 1].Value == ",") || (expression[position + 1].Value == "=" && expression[position + 2].Value == "{"))
        {
            node = LineSecuence(expression[position].Value);
        }
        else if (expression[position] is Identificador && expression [position + 1].Value == "=" && TipoSecuencia(expression[position + 2].Value))
        {
            node = LineSecuence (expression[position].Value);
        }
        else if (position + 2 < expression.Count - 1 && expression[position] is Identificador && variablesLocales.Any (p => p.Value == expression[position + 2].Value))
        {
            node = LineSecuence(expression[position].Value);
        }
        else if (expression[position].Value == "_" || expression[position].Value == "rest")
        {
            node = LineSecuence(expression[position].Value);
        }
         else if (position > 1 && expression[position].Type == TokenTypes.Funcion && expression[position - 1].Value == "function")
        {
            ParseFuc(expression[position]);
            position++;
            if (position < expression.Count)
            {
                 node = ParseExpression();
            }
        }
        else if(expression[position].Type == TokenTypes.Identifier ) 
        {
            node = expression[position];
            position++;
           
        }
        else if (double.TryParse(c,out double value))
        {
            double val = value;
            node = new TokenNumero (val.ToString() , TokenTypes.Number );
            position++;
        }
        //si el token es un literal 
         
        //si es una coma continue o algunas de estas cosas seguir en lo suyo
         else if (c == "(" || c == "{" || c =="\"" || c == "="  || c == "function" || c == "=>"  )
        {
               position++;
               node = ParseExpression();
        }
        else if(Isfunction(c))
        {
            position++;
            node = ParseFunction(expression[position - 1]);
        }
        else if (c == "if")
        {
            position++;
            node = ParserIFelse();
        }
      
        else if (c == "let")
        {
            position++;
            node = Letin();
        }
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
      IfElseNode ifi = new IfElseNode("if" , TokenTypes.Condicional ,Root );
      ifi.expression = expression;
      ifi.position = position;
      token a = ifi.ParseExpression();
      ifi.tokens.Add(a);
      ifi.position++;
      token b = ifi.Expresiones();
     // ifi.tokens.Add(b);
      if(expression[position].Value == "(")
      {
       position++;
      }
      ifi.position++;
       token c = ifi.Expresiones();
    //   ifi.tokens.Add(c);
       position = ifi.position;
       return ifi;
    }
    private token Figura (string c)
    {
        token fig = new token("nf", TokenTypes.Identifier);
        if(expression[position + 2].Value == "segment") 
        {
         fig = new FigDeDosPunto(c , TokenTypes.Segment , null , null);
        }
        else if (expression[position + 2].Value == "circle")
        {
            fig = new token(c , TokenTypes.Circle );
        }
        else if (expression[position + 2].Value == "point")
        {
            fig = new token(c , TokenTypes.Point);
        }
        else if (expression[position + 2].Value == "line")
        {
            fig = new token(c , TokenTypes.Line);
        }
        position = position + 3 ;
        for (int i = position ; i < expression.Count; i++)
        {
            if (expression[i].Value == "(" || expression[i].Value == "," || expression[i].Value == ";") continue;
           
            if (expression[i].Value == ")")
            {
                position = i + 1 ;
                break ;
            }
            fig.tokens.Add(expression[i]);
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

    public  token FinIndeX (token a , Geometrico Root)
    {
        for (int i = 0 ; i < Root.variablesLocales.Count; i++)
        {
            if (a.Value == Root.variablesLocales[i].Value)
            {
                return Root.variablesLocales[i];
            }
        }
        return null;
    }
  
   public  bool Isfunction(string c)
   {
        return c == "sin" || c == "cos" || c == "tan" || c == "Sqrt"  || c == "^";
  }

public  int FindFun(token a , List<token> variables )
{
    for (int i = 0; i < variables.Count; i++)
    {
        if (a.Value == variables[i].Value)
        {
            return i;
        }
    }
 
    return -1;
}
    
    public   int Encuentro(string a , List<token> b)
  {
    for (int i = 0; i < b.Count; i++)
    {
        if (a == b[i].Value)
        {
            return i;
        }
    }
    return -1;

}
 public  bool IsOperator(string c)
    {
        return c == "+" || c == "-" || c == "*" || c == "/" || c == "%";
    }
    public static bool Tipos (TokenTypes types)
    {
        return types == TokenTypes.Segment || types == TokenTypes.Point ||types == TokenTypes.Circle || types == TokenTypes.Line;
    }
    public static bool TiposFigura(string tipoFigura)
    {
        return tipoFigura == "line" || tipoFigura == "segment" || tipoFigura == "circle" || tipoFigura =="point" || tipoFigura == "measure";
    }
    public static bool TipoSecuencia(string TipoSecuencia)
    {
        return TipoSecuencia == "intersect"|| TipoSecuencia == "randoms" || TipoSecuencia == "samples" || TipoSecuencia == "points";
    }
public  TokenSecuencia LineSecuence(string NombreSecuencia)
{
    TokenSecuencia secuencia = new TokenSecuencia("" , TokenTypes.secuencia);

    for (int i = position; i < expression.Count; i++)
    {
        if (expression[i] is Identificador || expression[i].Value == "rest" || expression[i].Value == "_")
        {
            secuencia.secuencias.Add(new TokenSecuencia(expression[i].Value , TokenTypes.secuencia));
            i ++;
            if (expression[i].Value == ",")continue;
        }
        // si encuentro una secuencia seguido de una asignacion y un tipo de secuencia 
        if (TipoSecuencia(expression[i].Value))
        {
           TokenSecuencia secuenciaHijo = new TokenSecuencia(expression[i].Value ,TokenTypes.secuencia);
          
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
                        if (secuencia.secuencias.Count == 1)
                        {
                            secuenciaHijo.Value = NombreSecuencia;
                            secuenciaHijo.tokens.Add(intersect);
                            if (expression[i].Value == ";")
                            {
                                position = i + 1;
                                return secuenciaHijo;
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
          if (secuencia.secuencias.Count == 1)secuencia.Value = NombreSecuencia;
          position = i + 1 ;
          return secuencia;
        }
    }
    return secuencia;
  }
  public Geometrico DrawFunction()
  {
    Geometrico drawFuncion = new Geometrico("draw " , TokenTypes.Funcion, this)
    
  }
}
}