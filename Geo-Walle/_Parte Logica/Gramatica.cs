using TokensGeo;
using Lexer;
using System.Linq;
namespace ParserGeo;

 public partial class Geometrico 
 {
     private token ParseFunction()
   {
    FunctionNode funcion = new FunctionNode(expression[position].Value , TokenTypes.Funcion);
    position++;
    if (expression[position].Value != "(")
    throw new ArgumentException("esperabamos un parentesis de apertura");
    parentesis.Push("(");
    position++;
    funcion.tokens.Add(ParseExpression());
    if (expression[position].Value != ")")
    throw new ArgumentException("esperabamos un parentesis de cierre");
    parentesis.Pop();
    position++;
    return funcion; 
   } 
    private token Identificador(string c)
  {
     if (variablesLocales.Any(valor => valor.Value == c))
            {
                position++;
                return (token)variablesLocales.Find(valor => valor.Value == c).Clone();
            }
            else if (expression[position + 1].Value == "=")
            {
                token temporal = new Identificador(expression[position].Value , TokenTypes.Identifier);
                position++;
                temporal.tokens.Add(ParseExpression());
                return temporal;
            }
            else if (variablesGlobales.Any(valor => valor.Value == c))
            {
                position++;
                return (token)variablesGlobales.Find(valor => valor.Value == c).Clone();
            }
            else
            {
                position++;
                return (token)expression[position-1].Clone();
            }
  }
    private  token  ParserIFelse()
    {
      IfElseNode IfElse = new IfElseNode("if" , TokenTypes.boolean);
      // parsea la condicion del if else 
      position ++;
      if (expression[position].Value != "(")
      throw new ArgumentException("esperabamos un (");
      else
      {
        parentesis.Push("(");
        position++;
      }
      token condicion = ParseExpression();
      if (condicion == null)
      throw new ArgumentException("la condicion del if no puede ser nula");
      IfElse.tokens.Add(condicion);
       if (expression[position].Value != ")")
       {
            throw new ArgumentException("esperabamos un (");
        }
        else
        {
            parentesis.Pop();
            position++;
        }
      //parsea el cuerpo then 
      token then = ParseExpression();
       if (then == null)
      throw new ArgumentException("la expresion then  del if no puede ser nula");
      IfElse.tokens.Add(then);
      //parsea el cuerpo Else
      if (expression[position].Value != "else")
      {
        throw new ArgumentException("esperabamos la palabra clave else");
      }
      position++;
      token Else = ParseExpression();
       if (Else == null)
      throw new ArgumentException("la expresion else del if no puede ser nula");
      if ( position > expression.Count - 1 )
       {
            throw new ArgumentException("esperabamos un ;");
       }
       if (position < expression.Count && expression[position].Value == ")" && parentesis.Count == 0)
       {
            throw new ArgumentException("esperabamos un (");
       }
      IfElse.tokens.Add(Else);
      
      return IfElse;
    }
    private token FuncionesGeo()
    {
        string NombreFuncion = "";
         if(expression[position].Value == "function")
        {
            position++;
        }
        if (expression[position].Type == TokenTypes.Identifier )
        {
            NombreFuncion = expression[position].Value;
            position++;
        }
        if(expression[position].Value != "(")
        {
            throw new ArgumentException("esprabamos un parentesis de apertura despues declarada la funcion");
        }
        else
        {
            parentesis.Push("(");
            position++;
        }
        Function Funcion = new Function(NombreFuncion , TokenTypes.Funcion , this);
        //agrega las variables locales del arbol padre a las variables gloabales (clonadas)
        Funcion.variablesGlobales.AddRange(variablesGlobales.Select(x => x));
        //agrega las variables gloabales del arbol padre a las variables gloabales (clonadas)
        Funcion.variablesGlobales.AddRange(variablesLocales.Select(x => x));
        //eliminar elementos duplicados 
        Funcion.variablesGlobales = Funcion.variablesGlobales.Distinct().ToList();
        Funcion.position = position;
        Funcion.expression = expression;
        Funcion.parentesis = parentesis;
        while (expression[Funcion.position].Value != ")")
        {
         if (expression[Funcion.position].Value != ")")
        Funcion.variablesLocales.Add((token)expression[Funcion.position].Clone());

        Funcion.position++;
        if ( Funcion.position > expression.Count - 1 )
        {
         throw new ArgumentException("esperabamos un ;");
        }
        if(expression[Funcion.position].Value == ",")
        Funcion.position++;

        else if (expression[Funcion.position].Value == ";")
        {
            position = Funcion.position;
            return Funcion ;
        }
        else if(expression[Funcion.position].Value == ")")
        {
            Funcion.parentesis.Pop();
            Funcion.position++;
            break;
        }
        else
        {
            throw new ArgumentException("esperabamos un )");
        }
        }
        // si la funcion sera definida 
        if(expression[Funcion.position].Value == "=") 
        Funcion.position++;
        // si la funcion solo fue llamada 
        else if(expression[Funcion.position].Value != "=")return Funcion;
        Funcion.tokens.Add(Funcion.ParseExpression());
        position = Funcion.position;
        parentesis = Funcion.parentesis;
        return Funcion;
    }
    //definicion de un figura con nombre 
    private token Figura (string nombreFigura)
    {
        token fig = new token("", TokenTypes.Identifier);
        if (TiposFigura(nombreFigura))
        {
          if(nombreFigura == "line" || nombreFigura == "segment" ||nombreFigura == "ray" || nombreFigura == "measure" )
          {
           if (expression[position + 1].Type == TokenTypes.Identifier)
           {
           fig = new FuncionPointsDos(expression[position + 1].Value,ValorNombreFigura(nombreFigura), color.Peek());
           position+=2;
           }
            else
            {
             fig = new FuncionPointsDos(expression[position].Value,ValorNombreFigura(nombreFigura), color.Peek());
             position++;
            }
           
          }
          else if (nombreFigura == "circle")
          {
            if (expression[position + 1].Type == TokenTypes.Identifier)
            {
            fig = new Circunferencia (expression[position + 1].Value , ValorNombreFigura(nombreFigura) , color.Peek());
             position += 2;
            }
            else
            {
                fig = new Circunferencia(expression[position].Value,ValorNombreFigura(nombreFigura) , color.Peek());
                position++;
            }
          }
          else if ( nombreFigura == "arc")
          {
            if (expression[position + 1].Type == TokenTypes.Identifier)
            {
            fig = new Arco(expression[position + 1].Value ,ValorNombreFigura(nombreFigura) , color.Peek());
             position += 2;
            }
            else
            {
             fig = new Arco(expression[position ].Value ,ValorNombreFigura(nombreFigura), color.Peek() );
             position++;
            }
          }
          else if(nombreFigura == "point")
          {
            if (expression[position + 1].Type == TokenTypes.Identifier)
            {
            fig = new Point (expression[position + 1].Value , ValorNombreFigura(nombreFigura) );
            position += 2;
            }
            else
            {
             fig = new Point (expression[position].Value , ValorNombreFigura(nombreFigura) );
             position++;
            }
          }
        }
        if (expression[position].Value == "(")
        {
        parentesis.Push("(");
        position++;
        }
        if (expression[position].Value == ";")return fig ;
        while(expression[position].Value != ")")
            {
               if (expression[position].Value == "(" || expression[position].Value == ","  ) position++;
               if(expression[position].Value == "(")parentesis.Push("(");
               if (variablesLocales.Any (valor => valor.Value == expression[position].Value))
               {
                fig.tokens.Add((token)variablesLocales.Find (valor => valor.Value == expression[position].Value).Clone());
                position++;
               }
               else if (variablesGlobales.Any(valor => valor.Value == expression[position].Value))
               {
                fig.tokens.Add((token)variablesGlobales.Find(valor => valor.Value == expression[position].Value).Clone());
                position++;
               }
               else
               {
                   fig.tokens.Add(ParseExpression());
                   position++;
               }
               if (expression[position].Value == ")")
               {
                    parentesis.Pop();
                    position++;
                    break;
               }
               if (expression[position].Value == ";")return fig;
            }
            if (position > expression.Count - 1)
            {
                throw new ArgumentException("esperabamos un punto y coma");
            }
           if(expression[position].Value != ";") return fig ;
           if (fig.Type != TokenTypes.Circle && fig.Type != TokenTypes.Arc && fig.Type != TokenTypes.Point)
            {
            if (fig.tokens.Count == 0)
            {
                return fig;
            }
            if(fig.tokens.Count != 0 )
            {
                if (fig.tokens.Count == 1)
                {
                    return fig.tokens[0];
                }
                if(fig.tokens[0].Type != TokenTypes.Identifier && fig.tokens[0].Type != TokenTypes.Point|| fig.tokens[1].Type != TokenTypes.Identifier && fig.tokens [1].Type != TokenTypes.Point )
                {
                 throw new ArgumentException ("los parametros pasados al  punto no son validos");
                }
                else
                {
                   return new FuncionPointsDos(fig.Value ,fig.Type , fig.tokens[0],fig.tokens[1] , color.Peek());
                }
            }
            }
           else if (fig.Type ==TokenTypes.Circle)
            {
                if (fig.tokens.Count == 0)
                {
                    return fig;
                }
                if(fig.tokens.Count != 0  )
                {
                    if(fig.tokens[0].Type != TokenTypes.Identifier && fig.tokens[0].Type != TokenTypes.Point ||  fig.tokens[1].Type != TokenTypes.Identifier && fig.tokens [1].Type != TokenTypes.Number )
                    {
                       throw new ArgumentException ("los parametros pasados a la circunferencia no son validos");

                    }
                    else
                    {
                    return new Circunferencia(fig.Value , fig.Type , fig.tokens[0] ,fig.tokens[1] , color.Peek());
                    }
                }
            }
           else if(fig.Type == TokenTypes.Point)
            {
                if(fig.tokens.Count == 0 )
                {
                    return fig;
                }
                if(fig.tokens.Count != 0)
                {
                    if(fig.tokens[0].Type != TokenTypes.Identifier && fig.tokens[0].Type != TokenTypes.Number ||  fig.tokens[1].Type != TokenTypes.Identifier && fig.tokens [1].Type != TokenTypes.Number )
                    {
                       throw new ArgumentException ("los parametros pasados al  punto no son validos");
                    }
                    else
                    {
                    return new Point (fig.Value , fig.Type , fig.tokens[0] , fig.tokens[1] );
                    }
                }
            }
           else if(fig.Type == TokenTypes.measure)
            {
                 if(fig.tokens.Count == 0 )
                {
                    return fig;
                }
                if(fig.tokens.Count != 0)
                {
                    if(fig.tokens[0].Type != TokenTypes.Identifier && fig.tokens[0].Type != TokenTypes.Point ||  fig.tokens[1].Type != TokenTypes.Identifier && fig.tokens [1].Type != TokenTypes.Point )
                    {
                      throw  new ArgumentException ("los parametros pasados a la medida  no son  validos");
                        
                    }
                    else
                    {
                     return new Measure (fig.Value , fig.Type , fig.tokens[0] , fig.tokens[1], color.Peek());
                    }
                    
                }
            }
                else if (fig.Type == TokenTypes.Arc)
                {
                    if (fig.tokens.Count == 0)
                    {
                        return fig ;
                    }
                    else
                    {
                    if(fig.tokens[0].Type != TokenTypes.Identifier && fig.tokens[0].Type != TokenTypes.Point ||  fig.tokens[1].Type != TokenTypes.Identifier && fig.tokens [1].Type != TokenTypes.Point || fig.tokens[2].Type != TokenTypes.Identifier && fig.tokens[2].Type != TokenTypes.Point || fig.tokens[3].Type != TokenTypes.Identifier && fig.tokens[3].Type != TokenTypes.Number )
                    {
                        new ArgumentException ("los parametros pasados al arco no son validos");
                    }
                    else
                    {
                        try
                        {
                            return new Arco (fig.Value , fig.Type , fig.tokens[0] , fig.tokens[1] , fig.tokens[2] , fig.tokens[3] , color.Peek());                            
                        }
                        catch (System.Exception)
                        {
                            
                        throw new ArgumentException("los parametros pasados no son validos");
                        }
                        
                    }
                    }
                }
            
        
        return fig;
    }
    private token Letin()
    {
            //nuevo nodo let
              LetIn let = new LetIn("let" , TokenTypes.Keyword , this);
              let.variablesGlobales.AddRange(let.Root.variablesGlobales.Select (x => x));
              let.variablesGlobales.AddRange(let.Root.variablesLocales.Select (x => x));
              let.variablesGlobales = let.variablesGlobales.Distinct().ToList();
              let.expression = expression;
              let.position = position ;
              while(let.position < expression.Count - 1)
              {
                    if (expression[let.position] is Identificador)
                    {
                        if (expression[let.position + 1].Value != ",")
                        {
                         Identificador nodo = (Identificador)expression[let.position];
                         let.position++;
                         nodo.tokens.Add(let.ParseExpression());
                        let.variablesLocales.Add(nodo);
                        }
                        else
                        {
                            let.LineSecuence(expression[let.position].Value);
                        }
                    }
                   
                       if (expression[let.position].Value == ";")
                       {
                        let.position++;
                        if (expression[let.position].Value == "in")
                        {
                            break;
                        }
                        continue;
                       } 
                    
                    if(let.position > expression.Count - 1 && expression[let.position - 1].Value == ";" )
                    {
                        throw new ArgumentException("error en let - in ,esperabamos la instruccion in");
                    }
               }
                if (expression[let.position].Value == "in")
                {
                  let.position++;
                  let.tokens.Add(let.ParseExpression());
                  if(let.position > expression.Count - 1)
                {
                throw new ArgumentException("esperabamos un ;");
                }
               
                }
                if(expression[let.position].Value == ";")
                 position = let.position++;
                  position = let.position;    
                
                 return let ;
        }
    private token Identificadores ()
    {
          if (variablesLocales.Any (valor => valor.Value == expression[position].Value))
          {
            position++;
            return (token)variablesLocales.Find(valor => valor.Value == expression[position- 1].Value).Clone();
          }
          // si esta en variables globales retorna la variable
          else if(variablesGlobales.Any(valor => valor.Value == expression[position].Value))
          {
            position++;
            return (token)variablesGlobales.Find(valor => valor.Value== expression[position - 1].Value ).Clone();
          }
          else
          {
            // si no se encuentra entonces devuelveme el token 
              Identificador nodo = new Identificador(expression[position].Value , TokenTypes.Identifier);
              position++;
              nodo.tokens.Add(ParseExpression());
              return nodo;
          }
        
    }
    private bool Isfunction(string c) 
    {
        return c == "sin" || c == "cos" || c == "tan" || c == "sqrt"  || c == "^";
    }
    private  bool IsOperator(string c)
    {
        return c == "+" || c == "-" || c == "*" || c == "/" || c == "%";
    }
    private  bool Tipos (TokenTypes types)
    {
        return types == TokenTypes.Segment || types == TokenTypes.Point ||types == TokenTypes.Circle || types == TokenTypes.Line || types == TokenTypes.Color || types == TokenTypes.Arc;
    }
    private  bool TiposFigura(string tipoFigura)
    {
        return tipoFigura == "line" || tipoFigura == "segment" || tipoFigura == "circle" ||tipoFigura =="point"|| tipoFigura == "measure" || tipoFigura == "arc" || tipoFigura == "ray";
    }
    private bool TipoSecuencia(string TipoSecuencia)
    {
        return TipoSecuencia == "intersect"|| TipoSecuencia == "randoms" || TipoSecuencia == "samples" || TipoSecuencia == "points";
    }
  private token LineSecuence(string NombreSecuencia)
  {
     TokenSecuencia secuencia = new TokenSecuencia("" , TokenTypes.secuencia );
   

     if (TipoSecuencia(NombreSecuencia))
     {
        secuencia.Value = NombreSecuencia;
        position++;
        while (expression[position].Value != ")")
        {
            if (expression[position].Value == "(") 
            {
                parentesis.Push("(");
                position++;
            }
            
        if (variablesLocales.Any(valor => valor.Value == expression[position].Value ) && expression[position].Value != ")")
        {
            secuencia.tokens.Add((token)variablesLocales.Find(valor => valor.Value == expression[position].Value).Clone());
            position++;
            if (expression[position].Value == "(")
            {
            while (expression[position].Value != ")")
            {
                position++;
                if (expression[position].Value == "," && secuencia.tokens[secuencia.tokens.Count- 1].Type != TokenTypes.Funcion)
                {
                    break;
                }
            }
            }
        }
        else if (variablesGlobales.Any(valor => valor.Value == expression[position].Value ) && expression[position].Value != ")")
        {
             secuencia.tokens.Add((token)variablesGlobales.Find(valor => valor.Value == expression[position].Value).Clone());
             position++;
            if (expression[position].Value == "(")
            {
             while (expression[position].Value != ")" )
            {
                if (expression[position].Value == "," && secuencia.tokens[secuencia.tokens.Count- 1].Type != TokenTypes.Funcion)
                {
                    break;
                }
                position++;
            }
            }
        }
        else
        {
            if(expression[position].Value != ")") secuencia.tokens.Add(ParseTerm());
        }
         if (expression[position].Value == ",")position++;
         if (expression[position].Value == ")")position++;
        if (expression[position].Value == ";")
        {
         break;
        }
             if (position > expression.Count - 1 || expression[position].Value == ";")
             {
                if (parentesis.Count != 0)
                {
                   if(parentesis.Count < 0 ) throw new ArgumentException( "error esperabamos un corchete de apertura ");

                   if(parentesis.Count > 0) throw new ArgumentException("error esperabamos un corchete de clausura");
                }
                if (position > expression.Count - 1)
                {
                    throw new ArgumentException("esperabamos un ;");
                }
                break ;
             }
         }
             return  ComandosDIntercepcion (NombreSecuencia ,secuencia.tokens[0] , secuencia.tokens[1]);
        
         return secuencia;
        
    }
    else if (NombreSecuencia == "point sequence" || NombreSecuencia == "line sequence")
    {
        position++;
        secuencia.Type = ValorNombreFigura(NombreSecuencia);
    }
      if (expression[position].Type == TokenTypes.Identifier)
    {
        
            
        while (expression[position].Value  != "=")
        {
             if (expression[position].Value == ",")
            {
                position++;
            }
            secuencia.secuencias.Add((token)expression[position].Clone());
            position++;
            if (expression[position].Value == "=" || expression[position].Value == ";")
            {
                break;
            }

        }
         if (expression[position].Value != "=")
        {
            throw new ArgumentException( "esperabamos un =");
        
        }
         position++;
       if (expression[position].Value == "{")
        {
            while (expression[position].Value != "}")
            {
                secuencia.tokens.Add(ParseTerm());
                if (expression[position].Value == ";")
                {
                    break;
                }
            }
            
        }
        else if(TipoSecuencia(expression[position].Value) || expression[position].Type == TokenTypes.Identifier)
        {
            secuencia.tokens.Add(ParseTerm());
        }
        
        if (secuencia.tokens.Count == 1 && secuencia.tokens[0].Type == TokenTypes.Operator )
        {
        secuencia.tokens[0] = CalculoSecuencia(secuencia.tokens[0].tokens[0] , secuencia.tokens[0].tokens[1]);
        }
        if(secuencia.secuencias.Count > 1)
        {
        for (int i = 0; i < secuencia.secuencias.Count; i++)
        {
          if(i < secuencia.tokens[0].tokens.Count )
           {   
            if( secuencia.secuencias[0].Value == "_" && secuencia.secuencias.Count == 2 && tokens[0].tokens.Count < secuencia.secuencias.Count)
            {
                 secuencia.secuencias[i] = new Underfine("vacio" , TokenTypes.Underfine);
                 variablesLocales.Add(secuencia.secuencias[i]);
            }
            else if (secuencia.secuencias[i].Value == "rest")
            {
                 secuencia.secuencias[i].tokens = secuencia.tokens[0].tokens.GetRange(i ,secuencia.tokens[0].tokens.Count - 1);
                 variablesLocales.Add(secuencia.secuencias[i]);
            }
            else if(secuencia.secuencias[i].Value != "_")
            {
                 secuencia.secuencias[i].tokens.Add(secuencia.tokens[0].tokens[i]);
                 variablesLocales.Add(secuencia.secuencias[i]);
            }
            }
              else if (secuencia.tokens[0].tokens.Count == 1 &&secuencia.secuencias.Count > 2 && i == secuencia.secuencias.Count - 2 && secuencia.secuencias[secuencia.secuencias.Count - 1].Value == "_")
            {
                 secuencia.secuencias[i] = new Underfine("vacio" , TokenTypes.Underfine);
                 variablesLocales.Add(secuencia.secuencias[i]);
            }
            else if (secuencia.secuencias[i].Value != "_")
            {
                variablesLocales.Add(secuencia.secuencias[i]);
            }
        }
      }
      else
      {
        secuencia.Value = secuencia.secuencias[0].Value;
        secuencia.tokens = secuencia.tokens[0].tokens;
        variablesLocales.Add(secuencia);
      }
    }
    
     return secuencia;
    }
    private bool LineSecuenceTipo ()
   {
    return ((expression[position].Type == TokenTypes.Identifier && variablesLocales.Any(valor => valor.Value == expression[position].Value) == false && variablesGlobales.Any(valor => valor.Value == expression[position].Value) == false) || position + 2 < expression.Count - 1 &&  expression[position + 2].Value == "{") || TipoSecuencia(expression[position].Value) || expression[position].Value == "point sequence" || expression[position].Value  == "line sequence";
  }
   private  token DrawFunction()
  {
    token drawFuncion = new token("draw" , TokenTypes.Comando);
    
    if (TiposFigura (expression[position].Value))
    {
        drawFuncion.Value = expression[position].Value;
        position++;
    }
    if(expression[position].Value == "(")
    {
        position++;
        parentesis.Push("(");
    }
    while (expression[position].Value != ";")
    {
        drawFuncion.tokens.Add(ParseExpression());
        
        if (expression[position].Value == ")")
        {
          while(expression[position].Value == ")" )
        {
            parentesis.Pop();
            position++;
            if (expression[position].Value == ";")
            {
                break; 
            }
        }
        }
        if(expression[position].Value == ",") position++;
        if (position > expression.Count - 1)
        {
            throw new ArgumentException("esperabamos un punto y coma");
        }
        else if (expression[position].Value == ";")
        {
            if (parentesis.Count < 0)
            {
               throw new ArgumentException ("en uno de los draw esperabamos un corchet de apertura");
            }
            else if (parentesis.Count > 0 )
            {
                throw new ArgumentException("en uno de los draw esperabamos un corchet de cierre");
            }
              break;
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
    if(c == "measure")
    {
        return TokenTypes.measure;
    }
    if(c == "point sequence")
    {
        return TokenTypes.point_sequence;
    }
    if (c == "line sequence")
    {
        return TokenTypes.line_sequence;
    }
    return TokenTypes.Point;
  }
    private token SecuenciaFinita()
  {
    corchetes.Push("}");
    position++;
    token secuencia = new token("contenedor" , TokenTypes.secuencia);
    List<token> componentes = new List<token>();
    int posibleVacio = 20;
    
    while (expression[position].Value != "}")
    {
        if (variablesLocales.Any(valor => valor.Value == expression[position].Value ))
        {
            componentes.Add((token)variablesLocales.Find(valor => valor.Value == expression[position].Value).Clone());
            
        }
        else if (variablesGlobales.Any(valor => valor.Value == expression[position].Value ))
        {
             componentes.Add((token)variablesGlobales.Find(valor => valor.Value == expression[position].Value).Clone());
             
        }
        else
        {
         if (expression[position].Value != "}")componentes.Add((token)expression[position].Clone());
             
        }
        posibleVacio--;
        position++;
        if (expression[position].Value == ",")
        {
            position++;
            continue;
        }
        if(expression[position].Value == "}")
        {
            corchetes.Pop();
            break ;
        }
       
        if (posibleVacio < 0)
        {
            return new Underfine("infinito" , TokenTypes.Underfine);
        }
             if (position > expression.Count - 1)
                {
                    throw new ArgumentException( "se esperaba un corchete de cierre y un ; en la secuencia ");
                }
                else if (expression[position].Value == ";" && corchetes.Count> 0)
                {
                    throw new ArgumentException ("se esperaba un corchete de cierre en la secuencia");
                }
                if (expression[position].Value == ";" && corchetes.Count < 0)
                {
                   throw new ArgumentException( "se esperaba un corchete de cierre en la secuencia");
                }
    }
    position++;
    secuencia.tokens = componentes;
    return secuencia;
  }
    private token CalculoSecuencia(token secuencia1 , token secuencia2)
    {
      if (secuencia1.tokens[0] is Underfine || secuencia2.tokens[0] is Underfine || secuencia2.tokens.Count > 25)
      {
        return secuencia1;
      }
      if (secuencia1.Value == "+" )
      {
        secuencia1 = CalculoSecuencia(secuencia1.tokens[0] , secuencia1.tokens[1]);
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
    private token ComandosDIntercepcion(string nombre , token a , token b)
    {
         IEnumerable<Point>tokensA = new List<Point>();
         IEnumerable<Point> tokensB = new List<Point>();
        TokenSecuencia secuencial = new TokenSecuencia("secuencia" , TokenTypes.secuencia );
        if(nombre == "intersect")
        {
            if(a.Type == TokenTypes.Identifier && b .Type == TokenTypes.Identifier)
            {
                tokensA = ((Figura)a.tokens[0]).puntosFigura;
                tokensB =  ((Figura)b.tokens[0]).puntosFigura;
                foreach (var itemB in tokensB)
                {
                    foreach (var itemA in tokensA)
                    {
                        if(itemB.x == itemA.x && itemB.y == itemA.y)
                        {
                        if (secuencial.tokens.Count > 0)
                        {  
                        foreach (var itemC in secuencial.tokens)
                        {
                            if (itemB.x != ((Point)itemC).x && itemB.y != ((Point)itemC).y )
                            {
                                secuencial.tokens.Add(itemB);
                                break;
                            }
                        }
                        }
                        else
                        {
                            secuencial.tokens.Add(itemB);
                        }
                        }
                    }
                }
                
               if(secuencial.tokens.Count > 2) return new Underfine("", TokenTypes.Underfine);
               return secuencial;
            }
            else if (a.Type != TokenTypes.Identifier  && b .Type != TokenTypes.Identifier)
            {
                tokensA = ((Figura)a).puntosFigura;
                tokensB = ((Figura)b).puntosFigura;
                //secuencial.tokens = tokensA.Intersect(tokensB).ToList();
                if(secuencial.tokens.Count > 2) return new Underfine("", TokenTypes.Underfine);
            }
            else if(b.Type == TokenTypes.Identifier && a.Type != TokenTypes.Identifier)
            {
                   tokensA = ((Figura)a).puntosFigura;
                   tokensB = ((Figura)b.tokens[0]).puntosFigura;
                  // secuencial.tokens = tokensA.Intersect(tokensB).ToList();
                   if(secuencial.tokens.Count > 2) return new Underfine("", TokenTypes.Underfine);
            }
            else
            {
               tokensA = ((Figura)a.tokens[0]).puntosFigura;
               tokensB = ((Figura)b.tokens[0]).puntosFigura;
              // secuencial.tokens = tokensA.Intersect(tokensB).ToList();
               if(secuencial.tokens.Count > 2) return new Underfine("", TokenTypes.Underfine);
            }
           // secuencial.puntosFigura = evaluados;
            return secuencial;
        }
        else 
        {
            List <token> evaluados = (List<token>)randoms();
            secuencial.tokens = evaluados;
            return secuencial;
        }
       
    }
    private  IEnumerable<int> randoms()
    {
      Random ran = new Random();
      for (int i = 0; i < 21; i++)
      {
        yield return ran.Next(1, 101);
      }
    }
    
    private TokenNumero Count ()
    {
        TokenNumero  count = new TokenNumero("" , TokenTypes.Number);
        if(expression[position].Value == "(")
        {
            parentesis.Push("(");
            position++;
        }
        if (variablesLocales.Any(valor => valor.Value == expression[position].Value))
        {
            token auxiliar = variablesLocales.Find(valor => valor.Value == expression[position].Value);
            if (auxiliar.Type == TokenTypes.Identifier)
            {   
                count = new TokenNumero((auxiliar.tokens[0]).tokens.Count.ToString() , TokenTypes.Number);
                position++;
            }
            else
            {
                count = new TokenNumero(auxiliar.tokens.Count.ToString() , TokenTypes.Number);
                position++;
            }
        }
        else if (variablesGlobales.Any(valor => valor.Value == expression[position].Value))
        {
            token auxiliar = variablesGlobales.Find(valor => valor.Value == expression[position].Value);
            if (auxiliar.Type == TokenTypes.Identifier)
            {
                count= new TokenNumero(auxiliar.tokens[0].tokens.Count.ToString() , TokenTypes.Number);
                position++;
            }
            else
            {
                count = new TokenNumero(auxiliar.tokens.Count.ToString() , TokenTypes.Number);
                position++;
            }
        }
        else
        {
            throw new ArgumentException("esta secuencia no es valida para la funcion count");
        }
        if (position > expression.Count - 1)
        {
            throw new ArgumentException("esperabamos un parentesis de cierre y un punto y coma");
        }
        if(expression[position].Value == ")")
        {
           parentesis.Pop();
           position++;
           if(expression[position].Value == ";")
           {
             return count;
           }
           else
           {
            throw new ArgumentException("esperabamos un punto y coma");
           }
        }
        else
        {
            throw new ArgumentException("esperabamos un parentesis de cierre");
        }
       
       
    }
    private TokenSecuencia Samples()
    {
        TokenSecuencia  auxiliar = new TokenSecuencia ("" , TokenTypes.secuencia);
        if(expression[position].Value == "(")
        {
            parentesis.Push("(");
            position++;
        }
        if (variablesLocales.Any(valor => valor.Value == expression[position].Value))
        {
            token auxiliar2 = variablesLocales.Find(valor => valor.Value == expression[position].Value);
              
                auxiliar.Value = auxiliar2.Value;
                int contador = 0;
                Random rnd =new Random();
                for (int i = 0; i < 20 ; i++)
                {
                  if(contador <auxiliar2.tokens.Count - 1)
                  {
                    int indiceAleatorio = rnd.Next(auxiliar2.tokens.Count);
                     auxiliar.tokens.Add(auxiliar2.tokens[indiceAleatorio]);
                     contador++;
                  }
                
                }

             position++;
        }
        else if (variablesGlobales.Any(valor => valor.Value == expression[position].Value))
        {
            token auxiliar2 = variablesGlobales.Find(valor => valor.Value == expression[position].Value);
              
                auxiliar.Value = auxiliar2.Value;
                int contador = 0;
                Random rnd =new Random();
                for (int i = 0; i < 20 ; i++)
                {
                  if(contador <auxiliar2.tokens.Count - 1)
                  {
                    int indiceAleatorio = rnd.Next(auxiliar2.tokens.Count);
                     auxiliar.tokens.Add(auxiliar2.tokens[indiceAleatorio]);
                     contador++;
                  }
                
                }

             position++;
        }
         if(expression[position].Value == ")")
        {
           parentesis.Pop();
           position++;
           if(expression[position].Value == ";")
           {
             return auxiliar;
           }
           else
           {
            throw new ArgumentException("esperabamos un punto y coma");
           }
        }
        else 
        {
            throw new ArgumentException("esperabamos un parentesis de cierre");
        }
        
     }
    private bool aumentoContador()
   {
    return position < expression.Count - 1 && expression[position].Value != ";" && !IsOperator(expression[position].Value) && !Isfunction(expression[position].Value) && expression[position].Value != "in" && expression[position].Value != "else" && expression[position].Value == "," && expression[position].Value != ";" ;
   }
    private void Import ()
    {
        string rutaDelArchivo = expression[position].Value;
        string contenido = "";
        if (File.Exists(rutaDelArchivo))
        {
         contenido =  File.ReadAllText (rutaDelArchivo);
        }
        else
        {
            throw new ArgumentException("el archivo no existe");
        }
        List<token> importaciones = Tokenizar.TokenizeString(contenido, errors);
        Geometrico alfa = new Geometrico ("" , TokenTypes.Identifier , null);
        alfa.expression = importaciones;
        alfa.Parser();
        variablesLocales.AddRange(alfa.variablesLocales.Select(x => x));
        position++;
         if (expression[position].Value != ";")
         throw new ArgumentException("esperabamos un ;");
    }
 }
