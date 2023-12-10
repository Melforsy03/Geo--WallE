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
    //public Stack <Color> colors {get ; set ;}
    public List<Errors> errores {get ; set ;}
    // constructor del arbol
   public Geometrico (string Value , TokenTypes Type , Geometrico Root) : base (Value , Type )
   {
      errores= new List<Errors>();
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
            if (TiposFigura(expression[position].Value))
            {
                variablesLocales.Add(ParseTerm());
                if(expression[position].Value == ";")position++;
                Expresiones();

            }
            else if ( expression[position].Value == "if" )
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
            else  
            {
                auxiliar = ParseExpression();
            if (Tipos(auxiliar.Type) || auxiliar.Type == TokenTypes.Funcion || auxiliar.Type == TokenTypes.Identifier)
            {
                //agrega el token a las variables locales 
                this.variablesLocales.Add((token)auxiliar.Clone());
                position++;
                
                Expresiones();
            }
              else if(auxiliar != null || auxiliar.Type != TokenTypes.secuencia)
                {
                  this.tokens.Add(auxiliar);
                }
                if (position < expression.Count - 1 && expression[position].Value == ";")
                {
                    position++;
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
            else if (c == "(" )
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
            else if ( c == ";"  || c== ",")
            {
                return leftNode;
            }
            else if (c== "in")
            {
                return leftNode;
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
         if(expression[position] is Identificador && variablesLocales.Any(valor => valor.Value == expression[position].Value) && expression[position + 1].Value == "=" )
         {
            new ArgumentException("esta variables ya fue definida en este contexto");
         }
         // me devuelve una funcion definida en el lenguaje
         if (expression[position] is Identificador && expression[position + 1].Value == "(")
         {
            position = position + 2;
            return FuncionesGeo(ValorDelToken);
         }
         //devuelve una figura
         else if( TiposFigura(ValorDelToken) || variablesLocales.Any(valor => valor.Value == expression[position].Value))
        {
           return Figura(ValorDelToken);
        }
         else if (expression[position].Type == TokenTypes.Identifier && expression[position + 1].Value == "=" && expression[position + 2].Value != "{")
        {
           position ++;;
           return Identificadores();
        }
         else if(LineSecuenceTipo())
        {
           return LineSecuence(expression[position].Value);
        }
        else if (expression[position].Type == TokenTypes.Underfine)
       {
        position++;
        return (token)expression[position - 1].Clone();
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
            return (token)variablesGlobales.Find(valor => valor.Value== expression[position - 1].Value ).Clone();
          }
          else
          {
            // si no se encuentra entonces devuelveme el token 
             position++;
             Identificador nodo = new Identificador(expression[position].Value , TokenTypes.Identifier);
             return nodo ;
          }
        }
        else if (ValorDelToken == "{")
        {
            position++;
            return SecuenciaFinita();
        }
        // si es alguno de los caracteres continua el parseo ;
        else if (ValorDelToken == "(" || ValorDelToken == "="  || ValorDelToken == "=>"  )
        {
               position++;
               return ParseExpression();
        }
        else if (expression[position].Type == TokenTypes.Identifier && !variablesGlobales.Any(nombre => nombre.Value == ValorDelToken) && !variablesLocales.Any(nombre => nombre.Value == ValorDelToken))
       {
         position++;
         errores.Add(new Errors(ErrorCode.Semantic , "esta varariable no esta definida en este contexto"));
         return expression[position - 1];
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
        if(expression[Funcion.position].Value == ",")Funcion.position++;
       
         if (expression[Funcion.position].Value != ")")Funcion.variablesLocales.Add((token)expression[position].Clone());
        
        Funcion.position++;
        if (expression[Funcion.position].Value == ";")
        {
            position = Funcion.position;
            return Funcion ;
        }
        if(expression[Funcion.position].Value == ")")
        {
            Funcion.position++;
            break;
        }
        
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
    private token Figura (string nombreFigura)
    {
        token fig = new token("", TokenTypes.Identifier);
        if (TiposFigura(nombreFigura))
        {
          if(nombreFigura == "line" || nombreFigura == "segment" ||nombreFigura == "ray" || nombreFigura == "measure" )
          {
           if (expression[position + 1].Type == TokenTypes.Identifier)
           {
           fig = new FuncionPointsDos(expression[position + 1].Value,ValorNombreFigura(nombreFigura) );
           position+=2;
           }
            else
            {
             fig = new FuncionPointsDos(expression[position].Value,ValorNombreFigura(nombreFigura));
             position++;
            }
           
          }
          else if (nombreFigura == "circle")
          {
            if (expression[position + 1].Type == TokenTypes.Identifier)
            {
            fig = new Circunferencia (expression[position + 1].Value , ValorNombreFigura(nombreFigura) );
             position += 2;
            }
            else
            {
                fig = new Circunferencia(expression[position].Value,ValorNombreFigura(nombreFigura));
                position++;
            }
          }
          else if ( nombreFigura == "arc")
          {
            if (expression[position + 1].Type == TokenTypes.Identifier)
            {
            fig = new Arco(expression[position + 1].Value ,ValorNombreFigura(nombreFigura) , this);
             position += 2;
            }
            else
            {
             fig = new Arco(expression[position ].Value ,ValorNombreFigura(nombreFigura) , this);
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
        if (expression[position].Value == ";")return fig ;
        while(expression[position].Value != ")")
            {
               if (expression[position].Value == "(" || expression[position].Value == ","  || expression[position].Value == ")") position++;
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
                   fig.tokens.Add((token)expression[position].Clone());
                   position++;
               }
               
               if (expression[position].Value == ")")
               {
              
                    position++;
                    break;
               
               }
               if (expression[position].Value == ";")
               {
                 break;
               }
            }
        if(expression[position].Value != ";") position++;
        if (fig.Type != TokenTypes.Circle && fig.Type != TokenTypes.Arc && fig.Type != TokenTypes.Point)
            {
            if (fig.tokens.Count == 0)
            {
                return fig;
            }
            if(fig.tokens.Count != 0 )
            {
                if(fig.tokens[0].Type != TokenTypes.Identifier && fig.tokens[0].Type != TokenTypes.Point|| fig.tokens[1].Type != TokenTypes.Identifier && fig.tokens [1].Type != TokenTypes.Point )
                {
                  new ArgumentException ("los parametros pasados al  punto no son validos");
                }
                else
                {
                   return new FuncionPointsDos(fig.Value ,fig.Type , fig.tokens[0],fig.tokens[1]);
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
                        new ArgumentException ("los parametros pasados a la circunferencia no son validos");

                    }
                    else
                    {
                    return new Circunferencia(fig.Value , fig.Type , fig.tokens[0] ,fig.tokens[1]);
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
                        new ArgumentException ("los parametros pasados al  punto no son validos");
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
                        new ArgumentException ("los parametros pasados a la medida  no son  validos");
                        
                    }
                    else
                    {
                     return new Measure (fig.Value , fig.Type , fig.tokens[0] , fig.tokens[1] );
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
                        return new Arco (fig.Value , fig.Type , tokens[0] , tokens[1] , tokens[2] , tokens[3]);
                    }
                    }
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
                    if (expression[let.position].Value == ")") let.position++;
                    
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
    public token Identificadores ()
    {
           token nodo = new Identificador(expression[position - 1].Value , TokenTypes.Identifier );
           nodo.tokens.Add(ParseExpression());
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
        return tipoFigura == "line" || tipoFigura == "segment" || tipoFigura == "circle" ||tipoFigura =="point"|| tipoFigura == "measure" || tipoFigura == "arc";
    }
    public static bool TipoSecuencia(string TipoSecuencia)
    {
        return TipoSecuencia == "intersect"|| TipoSecuencia == "randoms" || TipoSecuencia == "samples" || TipoSecuencia == "points";
    }
    public  TokenSecuencia LineSecuence(string NombreSecuencia)
   {
    TokenSecuencia secuencia = new TokenSecuencia("" , TokenTypes.secuencia );
    int parentesis = 0 ;
    int corchetes = 0;

    if (TipoSecuencia(NombreSecuencia))
    {
        secuencia.Value = NombreSecuencia;
        position++;
        while (expression[position].Value != ")")
        {
            if (expression[position].Value == "(") 
            {
                parentesis++;
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
                if (parentesis!= 0)
                {
                   if(parentesis < 0 ) errores.Add(new Errors(ErrorCode.Semantic , "error esperabamos un corchete de apertura "));

                   if(parentesis > 0) errores.Add(new Errors(ErrorCode.Semantic ,"error esperabamos un corchete de clausura"));
                }
                if (position > expression.Count - 1)
                {
                    errores.Add(new Errors(ErrorCode.Semantic , "esperabamos un ;"));
                }
                break ;
             }
             secuencia.tokens[0] = ComandosDIntercepcion (NombreSecuencia ,(Figura)secuencia.tokens[0].tokens[0] , (Figura)secuencia.tokens[0].tokens[1]);
        }
         return secuencia;
        
    }
    else if (NombreSecuencia == "point sequence" || NombreSecuencia == "line sequence")
    {
        position++;
        secuencia.Type = ValorNombreFigura(NombreSecuencia);
    }
      if (expression[position].Type == TokenTypes.Identifier)
    {
        //secuencia.Value = expression[position].Value;
            
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
            errores.Add(new Errors(ErrorCode.Semantic , "esperabamos un ="));
            return secuencia;
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
  public bool LineSecuenceTipo ()
  {
    return ((expression[position].Type == TokenTypes.Identifier && variablesLocales.Any(valor => valor.Value == expression[position].Value) == false && variablesGlobales.Any(valor => valor.Value == expression[position].Value) == false) || position + 2 < expression.Count - 1 &&  expression[position + 2].Value == "{") || TipoSecuencia(expression[position].Value) || expression[position].Value == "point sequence" || expression[position].Value  == "line sequence";
  }
  public token DrawFunction()
  {
    token drawFuncion = new token("draw" , TokenTypes.Comando);
    int corchete = 0;
    if (TiposFigura (expression[position].Value))
    {
        drawFuncion.Value = expression[position].Value;
        position++;
    }
    
    if(expression[position].Value == "(")
    {
        position++;
        corchete++;
    }
    while (expression[position].Value != ";")
    {
        drawFuncion.tokens.Add(ParseTerm());
        
        if (expression[position].Value == ")")
        {
          while(expression[position].Value == ")" )
        {
            corchete--;
            position++;
            if (expression[position].Value == ";")
            {
                break; 
            }
        }
        }
        if(expression[position].Value == ",") position++;
        else if (expression[position].Value == ";")
        {
            if (corchete < 0)
            {
                new ArgumentException ("en uno de los draw esperabamos un corchet de apertura");
            }
            else if (corchete > 0 )
            {
                new ArgumentException("en uno de los draw esperabamos un corchet de cierre");
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
  public token SecuenciaFinita()
  {
    token secuencia = new token("contenedor" , TokenTypes.secuencia);
    List<token> componentes = new List<token>();
    int posibleVacio = 20;
    int corchetes = 1;
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
        if (expression[position].Value == ",")position++;
        if (expression[position].Value == "}")corchetes--;
        if(expression[position].Value == "}")
        {
            break ;
        }
        if (expression[position].Value == ";" )
        {
            errores.Add(new Errors(ErrorCode.Semantic , "esperabamos un corchete de cierre"));
            break;
        }
        if (posibleVacio < 0)
        {
            return new Underfine("infinito" , TokenTypes.Underfine);
        }
             if (position > expression.Count - 1)
                {
                    errores.Add(new Errors(ErrorCode.Semantic , "se esperaba un corchete de cierre y un ; en la secuencia "));
                }
                else if (expression[position].Value == ";" && corchetes> 0)
                {
                    errores.Add(new Errors(ErrorCode.Semantic , "se esperaba un corchete de cierre en la secuencia"));
                    break;
                    
                }
                if (expression[position].Value == ";" && corchetes <0)
                {
                   errores.Add(new Errors(ErrorCode.Semantic , "se esperaba un corchete de cierre en la secuencia"));
                   break;
                   
                }
    }
    if(expression[position].Value != ";")position++;
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
    private  token ComandosDIntercepcion(string nombre , Figura a , Figura b)
    {
         List<token>tokensA = new List<token>();
            List<token> tokensB = new List<token>();
        Figura secuencial = new Figura("secuencia" , TokenTypes.secuencia);
        if(nombre == "intersect")
        {
            if(a.Type == TokenTypes.Identifier)
            {
                tokensA = CumpleCondicion((Figura)a.tokens[0] , b).tokens;
            }
            else
            {
                tokensA = CumpleCondicion(a, b).tokens;
            }
             if(b.Type == TokenTypes.Identifier)
            {
                   //tokensB = (List<Point>)((Figura)b.tokens[0]).puntosFigura;;
            }
            else
            {
              //  tokensB = (List<Point>)b.puntosFigura;
            }
            
           // List<Point> evaluados = tokensA.Intersect(tokensB).ToList();
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
     private IEnumerable<int> randoms()
    {
      Random ran = new Random();
      for (int i = 0; i < 21; i++)
      {
        yield return ran.Next(1, 101);
      }
    }
    private token CumpleCondicion(Figura A , Figura B)
    {
        Figura valor = new Figura("",TokenTypes.secuencia);
        List<Point>interseccion = new List<Point>();
        int limite = 2;
        double menor = (((FuncionPointsDos)A).p1.x < ((FuncionPointsDos)B).p2.x) ?((FuncionPointsDos)A).p1.x : ((FuncionPointsDos)B).p2.x ;
        double mayor = (((FuncionPointsDos)A).p1.x > ((FuncionPointsDos)B).p2.x ) ?((FuncionPointsDos)A).p1.x : ((FuncionPointsDos)B).p2.x ;
        for (double J = menor ; J < mayor; J++)
        {
            if (A is FuncionPointsDos && B is FuncionPointsDos)
            {
                double valor1 =(A.p2.y -  A.p1.y) / (A.p2.x -  A.p1.x )* (J -  A.p1.x) +  A.p1.y;
                double valor2 =  B.p2.y -  B.p1.y / ( B.p2.x -  (B.p1.x)) * (J -  B.p1.x) +  B.p1.y;
                if(limite > 0 && valor1 == valor2)
                {
                    Point punto = new Point("", TokenTypes.Point);
                    punto.x = J;
                    punto.y = valor1;
                    interseccion.Add(punto);
                    limite --;
                }
                else
                {
                    return new Underfine("infinito" , TokenTypes.Underfine);
                }
            }
            if (A is FuncionPointsDos && B is Arco)
            {   
                 double d = Math.Sqrt(Math.Pow(((Arco)B).p3.x - ((Arco)B).p2.x, 2) + Math.Pow(((Arco)B).p3.y - ((Arco)B).p2.y, 2));
                 double angulo = 2 * Math.Asin(d / 2 * int.Parse(((Arco)B).medida.Value));
                 double angulo1 = angulo / (30 - 1);
                 double x1 = B.p1.x + int.Parse(((Arco)B).medida.Value) * (int)Math.Cos(angulo1 / 2 + J * angulo);
                 double y = B.p1.x + int.Parse(((Arco)B).medida.Value) * (int)Math.Sin(angulo1 / 2 + J * angulo);
                 double valor2 =  (A.p2.y -  ((FuncionPointsDos)A).p1.y) / (A.p2.x -  A.p1.x) * (J -  A.p1.x) +  ((FuncionPointsDos)A).p1.y;  
                 double  valor1 =  ((x1 - y) / (x1 - y)) * (J - y) + y;  
                 if (limite > 0 && valor1 == valor2)
                 {
                    Point punto = new Point("", TokenTypes.Point);
                    punto.x = J;
                    punto.y = valor1;
                    interseccion.Add(punto);
                    limite --;
                 }
                 else
                 {
                    return new Underfine("infinito" , TokenTypes.Underfine);
                 }           
            }
            if (A is Arco && B is FuncionPointsDos)
            {
                double d = Math.Sqrt(Math.Pow(((Arco)A).p3.x - ((Arco)A).p2.x, 2) + Math.Pow(((Arco)A).p3.y - ((Arco)A).p2.y, 2));
                 double angulo = 2 * Math.Asin(d / 2 * int.Parse(((Arco)A).medida.Value));
                 double angulo1 = angulo / (30 - 1);
                 double x1 = ((Arco)A).p1.x + double.Parse(((Arco)A).medida.Value) * (int)Math.Cos(angulo1 / 2 + J * angulo);
                 double y = ((Arco)A).p1.x + double.Parse(((Arco)A).medida.Value) * (int)Math.Sin(angulo1 / 2 + J * angulo);
                 double valor2 =  ((((FuncionPointsDos)B).p2.y -  ((FuncionPointsDos)B).p1.y) / ( ((FuncionPointsDos)B).p2.x -  ((FuncionPointsDos)B).p1.x)) * (J -  ((FuncionPointsDos)B).p1.x) +  ((FuncionPointsDos)B).p1.y;  
                 double valor1 =  ((x1 - y) / (x1 - y)) * (J - y) + y;  
                 if (limite > 0 && valor1 == valor2)
                 {
                    Point punto = new Point("", TokenTypes.Point);
                    punto.x = J;
                    punto.y = valor1;
                    interseccion.Add(punto);
                    limite --;
            }
            
        }
            if (A is Arco && B is Arco)
            {
                 double d = Math.Sqrt(Math.Pow(((Arco)B).p3.x - ((Arco)B).p2.x, 2) + Math.Pow(((Arco)B).p3.y - ((Arco)B).p2.y, 2));
                 double angulo = 2 * Math.Asin(d / 2 * int.Parse(((Arco)B).medida.Value));
                 double angulo1 = angulo / (30 - 1);
                 double d1 = Math.Sqrt(Math.Pow(((Arco)A).p3.x - ((Arco)A).p2.x, 2) + Math.Pow(((Arco)A).p3.y - ((Arco)A).p2.y, 2));
                 double angulo2 = 2 * Math.Asin(d / 2 * int.Parse(((Arco)A).medida.Value));
                 double angulo3= angulo2 / (30 - 1);
                 if (angulo1 == angulo3)
                 {
                    Point punto = new Point("", TokenTypes.Point);
                    punto.x = J;
                    punto.y = angulo1;
                    interseccion.Add(punto);
                    limite --;
                 }
            }
           /* if(A is Circunferencia && B is Circunferencia)
            {
                double valor1 = Math.Pow(A.p1.x - J , 2) + Math.Pow(A.p.y - )
            }*/
            valor.puntosFigura = interseccion;
        }
        return valor;
   
}
}
}