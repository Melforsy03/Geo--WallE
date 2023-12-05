using TokensGeo;
using Lexer;
namespace ParserGeo
{
    public class Geometrico : token
    {
        //nombre del arbol
    public  string Value {get ; set ;}
    //tipo del arbol
    public  TokenTypes Type {get ; set ;}
    //variables locales del arbol
    public  List<token> variablesLocales {get ; set ;}
    //variables gloables
    public  List<token> variablesGlobales {get ; set ;}
    // Nodo padre del arbol
    public  Geometrico Root {get ; set ;}
    //lista de tokens a parsear 
    public  List<token> expression {get ;set ;} 
    //posicion con la que se va a recorrer recursivamente
    public int position {get ; set ;}
    // constructor del arbol
   public Geometrico (string Value , TokenTypes Type , Geometrico Root) : base (Value , Type )
   {
      this.Value = Value ; 
      this.Type = Type ;
      variablesLocales = new List<token>();
      variablesGlobales = new List<token>();
      this.Root = Root;
      
   }
   // metodo que me chequea la semantica del arbol
   public  bool CheckSemantic(List<Errors> errores)
   {
        throw new NotImplementedException();
   }
   //metodo que evalua el arbol
   public void Evaluate ()
   {
    
   }
   //construccion del arbol
    public  token Parser()
    {
       return Expresiones();
    }
    public token Expresiones ()
    {
          token auxiliar = null;
          while (position < expression.Count - 1)
          {
        // la funcion Tipos me devuelve True si es tipo point , segmento , circulo , arco ,line o color
            if (Tipos(expression[position].Type))
            {
                //agrega el token a las variables locales 
                this.variablesLocales.Add((token)expression[position].Clone());
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
                // Parsea la condicion if 
                auxiliar = ParserIFelse();
                if(auxiliar != null )this.tokens.Add(auxiliar);
                Expresiones();
            }
            else if(expression[position].Value == "let")
            {
               position++;
               //parsea la estructura Let - in
               auxiliar  = Letin();
               if(auxiliar != null )this.tokens.Add(auxiliar);
               Expresiones();
            }
            else if(expression[position].Value == "draw")
            {
                position++;
                //parsea la funcioon draw 
                auxiliar = DrawFunction();
                this.tokens.Add(auxiliar);
                
            }
            else if(position < expression.Count - 1) 
            {
                auxiliar = ParseExpression();
                if (expression[position].Value == ",")
                {
                    position++;
                }
                if(auxiliar != null)
                {
                    if (Tipos(auxiliar.Type) || auxiliar.Type == TokenTypes.secuencia || auxiliar.Type == TokenTypes.Funcion && !variablesLocales.Any(valor => valor.Value == auxiliar.Value) || auxiliar.Type == TokenTypes.Identifier)
                    {
                        this.variablesLocales.Add(auxiliar);
                    }
                    else
                    {
                          this.tokens.Add(auxiliar);
                    }
                if (expression[position].Value == ";")
                {
                    position++;
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
                FunctionNode operatorNode = new FunctionNode(c,TokenTypes.Operator ,this);
                //crea el hijo izquierdo de la operacion Funcion 
                operatorNode.tokens[0] = ParseTerm();
                position++;
                //crea el hijo derecho de la operacion funcion
                operatorNode.tokens[1] = ParseFunction(operatorNode);
                return operatorNode;
            }
            else if (c == "if")
            {
                position++;
                //retorna la condicion if 
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
            else if (c == ">" || c == "<" || c == "<=" || c == ">=" ||  c == "!=" || c == "==" )
            {
                position++;
                token rigthNode = ParseTerm();
                tokenBul condicion = new tokenBul(c,TokenTypes.Condicional);
                condicion.tokens.Add(leftNode);
                condicion.tokens.Add(rigthNode);
                return condicion;
            }
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
           
        }
       return leftNode;
    }
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
    private token ParseFactor()
    {
        token node  = null ;
        
         string ValorDelToken = expression[position].Value;
         // me devuelve una funcion definida en el lenguaje
         if (expression[position] is Identificador && expression[position + 1].Value == "(")
         {
            position = position + 2;
            return FuncionesGeo(ValorDelToken);
         }
         //devuelve una figura
         else if((expression[position].Type == TokenTypes.Identifier && expression[position + 1].Value == "=" && TiposFigura(expression[position + 2].Value)) || TiposFigura(ValorDelToken) || variablesLocales.Any(p => p.Value == Value))
        {
           return Figura(ValorDelToken);
        }
         //me devuelve una secuencia 
        else if(LineSecuenceTipo())
        {
            return LineSecuence(expression[position].Value);
        }
        //me devuelve un token numero 
        else if (double.TryParse(ValorDelToken,out double value))
        {
            double val = value;
            position++;
            return new TokenNumero (val.ToString() , TokenTypes.Number );
        }
        //me devuelve una funcion sen , coseno , tan , sqrt
        else if(Isfunction(ValorDelToken))
        {
            position++;
            return ParseFunction(expression[position - 1]);
        }
        //me devuelve un token if- else ;
        else if (ValorDelToken == "if")
        {
            position++;
            return ParserIFelse();
        }
        //me devuelve un token let - in;
        else if (ValorDelToken == "let")
        {
            position++;
            return Letin();
        }
        //me devuleve un token draw con los hijos a pintar ;
        else if(ValorDelToken == "draw")
        {
            position++;
            return DrawFunction();
        }
        //retorna un identificador que este igualado a algo
        else if (expression[position].Type == TokenTypes.Identifier && expression[position + 1].Value == "=")
        {
           position = position + 2;
           return Identicadores();
        }
        //devuelve un toquen identificador;
         else if(expression[position].Type == TokenTypes.Identifier ) 
        {
            //si esta en variables locales retorna la variable
          if (variablesLocales.Any (valor => valor.Value == expression[position].Value))
          {
            position++;
            return (token)variablesLocales.Find(valor => valor.Value == expression[position- 1].Value).Clone();
    
          }
          // si esta en variables globales retorna la variable
          else if(variablesGlobales.Any(valor => valor.Value == expression[position].Value))
          {
            position++;
            return (token)variablesGlobales.Find(valor => valor.Value== expression[position- 1].Value ).Clone();
          }
          else
          {
            // si no se encuentra entonces devuelveme el token 
             position++;
             Identificador nodo = new Identificador(expression[position].Value , TokenTypes.Identifier);
             return nodo ;
          }
           
        }
        // si es alguno de los caracteres continua el parseo ;
        else if (ValorDelToken == "(" || ValorDelToken == "{" || ValorDelToken == "="  || ValorDelToken == "=>"  )
        {
               position++;
               return ParseExpression();
        }
     
     return node ;
  }
    //Para recorrer las funciones como sen , cos , tan , arct , cot
    private token ParseFunction(token funcion )
   {
      token a = ParseExpression();
      funcion.tokens.Add(a);
      return funcion;   
   } 
    private  token  ParserIFelse()
    {
      IfElseNode IfElse = new IfElseNode("if" , TokenTypes.Condicional , Root );
      //agrega las variables locales del arbol padre a las variables gloabales (clonadas)
      IfElse.variablesGlobales.AddRange(Root.variablesLocales.Select(x => x));
       //agrega las variables globales del arbol padre a las variaables globales (clonadas)
      IfElse.variablesGlobales.AddRange(Root.variablesGlobales.Select(x => x));
      // elimina las variables repetidas en las variables globales 
      IfElse.variablesGlobales = IfElse.variablesGlobales.Distinct().ToList();
      IfElse.expression = expression;
      IfElse.position = position;
      // parsea la condicion del if else 
      token condicion = IfElse.ParseExpression();
      IfElse.tokens.Add(condicion);
      IfElse.position++;
      //parsea el cuerpo then 
      token then = IfElse.ParseExpression();
      IfElse.tokens.Add(then);
      if(expression[position].Value == "(")position++;
      IfElse.position++;
      //parsea el cuerpo Else
      token Else = IfElse.ParseExpression();
      IfElse.tokens.Add(Else);
      position = IfElse.position;
      return IfElse;
    }
    private token FuncionesGeo(string NombreFuncion)
    {
        Function Funcion = new Function(NombreFuncion , TokenTypes.Funcion , this);
        //agrega las variables locales del arbol padre a las variables gloabales (clonadas)
        Funcion.variablesGlobales.AddRange(variablesGlobales.Select(x => x));
        //agrega las variables gloabales del arbol padre a las variables gloabales (clonadas)
        Funcion.variablesGlobales.AddRange(variablesLocales.Select(x => x));
        //eliminar elementos duplicados 
        Funcion.variablesGlobales = Funcion.variablesGlobales.Distinct().ToList();
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
        // si la funcion sera definida 
        if(expression[Funcion.position].Value == "=") Funcion.position++;
        // si la funcion solo fue llamada 
        else if(expression[Funcion.position].Value != "=")return Funcion;
        Funcion.tokens.Add(Funcion.ParseExpression());
        position = Funcion.position;
        return Funcion;
    }
    //definicion de un figura con nombre 
    private token Figura (string c)
    {
        token fig = new token("", TokenTypes.Identifier);
        if (TiposFigura(c))
        {
          if(c == "line" || c == "segment" ||c == "ray" || c == "measure")
          fig = new FuncionPointsDos(c,ValorNombreFigura(c) );
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
            position = position + 3;
            return fig ;
        }
        else if(TiposFigura(expression[position + 2].Value)) 
        {
          if(expression[position].Value == "line" || expression[position].Value == "segment" ||expression[position].Value == "ray" || expression[position].Value == "measure")
          fig = new FuncionPointsDos(c , ValorNombreFigura(expression[position + 2].Value) );
          else if (expression[position].Value == "circle")
          {
            fig = new Circunferencia(c , TokenTypes.Circle , this);
          }
          else if (expression[position].Value == "arc")
          {
            fig = new Arco (c , TokenTypes.Arc , this);
          }
           position = position + 3 ;
        }
            while(expression[position].Value != ")")
            {
               if (expression[position].Value == "(" || expression[position].Value == ","  || expression[position].Value == ")") position++;
               fig.tokens.Add(ParseExpression());
               while (expression[position].Value != ")")
               {
                    position++;
               }
               if (expression[position].Value == ";")
               {
                 break;
               }
            }
            position++;
            if (fig.Type != TokenTypes.Circle && fig.Type != TokenTypes.Arc)
            {
            if (fig.tokens.Count == 0)
            {
                return new FuncionPointsDos(fig.Value ,fig.Type);
            }
            if(fig.tokens.Count != 0 && fig.tokens[0].Type == TokenTypes.Point && fig.tokens[0].tokens.Count != 0 && fig.tokens[1].tokens.Count != 0)
            {
                return new FuncionPointsDos(fig.Value ,fig.Type , fig.tokens[0],fig.tokens[1]);
            }
            else
            {
              return new FuncionPointsDos(fig.Value , fig.Type , (Point)fig.tokens[0] ,(Point)fig.tokens[1]);
            }
            }
            else if (fig.Type ==TokenTypes.Circle)
            {
                if (fig.tokens.Count == 0)
                {
                    return new Circunferencia(fig.Value, fig.Type ,this);
                }
                if(fig.tokens.Count != 0 && fig.tokens[0].Type == TokenTypes.Point && (fig.tokens[1].Type == TokenTypes.measure || fig.tokens[1].Type == TokenTypes.Number || fig.tokens[1].Type == TokenTypes.Operator) && fig.tokens[0].tokens.Count != 0 )
                {
                    return new Circunferencia(fig.Value , fig.Type , this ,(Point)fig.tokens[0] ,fig.tokens[1]);
                }
            }
        
        return fig;
    }
    private  token Letin()
          {
            //nuevo nodo let
              LetIn let = new LetIn("let" , TokenTypes.Keyword , this);
              let.variablesGlobales.AddRange(let.Root.variablesGlobales.Select (x => x));
              let.variablesGlobales.AddRange(let.Root.variablesLocales.Select (x => x));
              let.variablesGlobales = let.variablesGlobales.Distinct().ToList();
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
    public Geometrico Identicadores ()
    {
           Geometrico nodo = new Geometrico(expression[position - 2].Value , TokenTypes.Identifier , this);
           nodo.expression = expression;
           nodo.position = position;
           if (this.variablesLocales != null)nodo.variablesGlobales.AddRange(nodo.Root.variablesLocales.Select(x => x));
           if(this.variablesGlobales != null)nodo.variablesGlobales.AddRange(this.variablesGlobales.Select(x => x));
           nodo.variablesGlobales = nodo.variablesGlobales.Distinct().ToList();
           nodo.tokens.Add(nodo.ParseExpression());
           return nodo ;
    }
   public  bool Isfunction(string c) 
   {
        return c == "sin" || c == "cos" || c == "tan" || c == "sqrt"  || c == "^";
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
    return ((expression[position].Type == TokenTypes.Identifier && variablesLocales.Any(valor => valor.Value == expression[position].Value) == false && expression[position + 1].Value == "," && variablesGlobales.Any(valor => valor.Value == expression[position].Value) == false) || (expression[position + 1].Value == "=" && expression[position + 2].Value == "{")) || (expression[position] is Identificador && expression [position + 1].Value == "=" && TipoSecuencia(expression[position + 2].Value)) || (position + 2 < expression.Count - 1 && expression[position] is Identificador && variablesLocales.Any (p => p.Value == expression[position + 2].Value)) || (expression[position].Value == "_" || expression[position].Value == "rest") || TipoSecuencia(expression[position].Value) ;
  }
  public Geometrico DrawFunction()
  {
    Geometrico drawFuncion = new Geometrico("draw" , TokenTypes.Funcion, this);
    if (TiposFigura (expression[position].Value))
    {
        drawFuncion.Value = expression[position].Value;
        position++;
    }
    drawFuncion.variablesGlobales.AddRange(drawFuncion.Root.variablesGlobales.Select (x => x));
    drawFuncion.variablesGlobales.AddRange(drawFuncion.Root.variablesLocales.Select (x => x));
    drawFuncion.variablesGlobales = drawFuncion.variablesGlobales.Distinct().ToList();
    drawFuncion.position = position;
    drawFuncion.expression = expression;
    if(expression[drawFuncion.position].Value == "(")drawFuncion.position++;
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
        if (expression[drawFuncion.position].Value == ";")
        {
             position = drawFuncion.position;
            break;
        }
        }
       
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