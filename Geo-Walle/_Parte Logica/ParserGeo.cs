using TokensGeo;
using Lexer;
namespace ParserGeo
{
    public partial class Geometrico : token
    {
        //nombre del arbol
        public string Value { get; set; }
        //tipo del arbol
        public TokenTypes Type { get; set; }
        //variables locales del arbol
        public List<token> variablesLocales { get; set; }
        private Stack<string> corchetes { get; set; }
        private Stack<string> parentesis { get; set; }
        //variables gloables
        public List<token> variablesGlobales { get; set; }
        // Nodo padre del arbol
        public Geometrico Root { get; set; }
        //lista de tokens a parsear 
        public List<token> expression { get; set; }
        //color con el que se va a pintar
        public Stack<string> color { get; set; }
        //posicion con la que se va a recorrer recursivamente
        private int position { get; set; }

        private List<Errors> errors { get; set; }

        // constructor del arbol
        public Geometrico(string Value, TokenTypes Type, Geometrico Root) : base(Value, Type)
        {
            this.Value = Value;
            this.Type = Type;
            variablesLocales = new List<token>();
            variablesGlobales = new List<token>();
            this.Root = Root;
            this.color = new Stack<string>();
            color.Push("black");
            parentesis = new Stack<string>();
            corchetes = new Stack<string>();
            errors = new List<Errors>();

        }
        // metodo que me chequea la semantica del arbol

        //construccion del arbol
        public token Parser()
        {
            return Expresiones();
        }
        private token Expresiones()
        {
            token auxiliar = null;
            while (position < expression.Count - 1)
            {
                if (expression[position].Value == "import")
                {
                    position++;
                    Import();
                    position++;
                }
                // la funcion Tipos me devuelve True si es tipo point , segmento , circulo , arco ,line o color
                else if (TiposFigura(expression[position].Value))
                {
                    variablesLocales.Add(ParseTerm());
                    if (expression[position].Value == ";") position++;
                    Expresiones();

                }
                if (position >= expression.Count - 1)
                {
                    return Root;
                }

                else if (expression[position].Value == "if")
                {
                    // Parsea la condicion if 
                    auxiliar = ParserIFelse();
                    if (auxiliar != null) this.tokens.Add(auxiliar);
                    Expresiones();
                }
                else if (expression[position].Value == "let")
                {
                    position++;
                    //parsea la estructura Let - in
                    auxiliar = Letin();
                    if (auxiliar != null) this.tokens.Add(auxiliar);
                    Expresiones();
                }
                else if (expression[position].Value == "draw")
                {
                    position++;
                    //parsea la funcioon draw 
                    auxiliar = DrawFunction();
                    this.tokens.Add(auxiliar);
                    if (expression[position].Value == ";")
                    {
                        position++;
                    }

                }
                else
                {
                    auxiliar = ParseExpression();
                    if (auxiliar != null && (Tipos(auxiliar.Type) || auxiliar.Type == TokenTypes.Funcion || auxiliar.Type == TokenTypes.Identifier))
                    {
                        //agrega el token a las variables locales 
                        this.variablesLocales.Add((token)auxiliar.Clone());
                        position++;

                        Expresiones();
                    }
                    else if (auxiliar != null && auxiliar.Type != TokenTypes.secuencia)
                    {
                        this.tokens.Add(auxiliar);
                    }
                    if (position < expression.Count - 1 && expression[position].Value == ";")
                    {
                        position++;
                    }
                    else
                    {
                        Expresiones();
                    }
                }
            }
            return Root;
        }
        //parsea las expresiones 
        private token ParseExpression()
        {
            token leftNode = ParseTerm();
            if (position == expression.Count)
            {
                return leftNode;
            }
            int bucle = 20;
            while (position < expression.Count)
            {
                bucle--;
                if (bucle == 0) return leftNode;
                string c = expression[position].Value;

                if (c == ";" || c == "," || c == "in" || c == ")" || c == "else" || c == "=>")
                {
                    return leftNode;
                }
                //si encuentra una funcion
                if (Isfunction(c))
                {

                    position++;
                    FunctionNode operatorNode = new FunctionNode(c, TokenTypes.Operator);
                    operatorNode.tokens.Add(ParseExpression());
                    if (position < expression.Count && (expression[position].Value == ")" || expression[position].Value == ";"))
                        return operatorNode;
                    if (position > expression.Count - 1)
                        return operatorNode;
                    c = expression[position].Value;
                    if (aumentoContador() && (c != ">" || c != "<" || c != "<=" || c != ">=" || c != "!=" || c != "==" || c != "&&" || c != "||"))
                        position++;
                    leftNode = operatorNode;
                }
                else if (IsOperator(c))
                {
                    OperatorNode operatorNode = new OperatorNode(c, TokenTypes.Operator);
                    operatorNode.tokens.Add(leftNode);
                    position++;
                    operatorNode.tokens.Add(ParseExpression());
                    if (position < expression.Count && (expression[position].Value == ")" || expression[position].Value == ";"))
                        return operatorNode;
                    if (position > expression.Count - 1)
                        return operatorNode;
                    if (position < expression.Count)
                    {
                        c = expression[position].Value;
                        if (aumentoContador() && (c != ">" || c != "<" || c != "<=" || c != ">=" || c != "!=" || c != "==" || c != "&&" || c != "||"))
                            position++;
                    }
                    leftNode = operatorNode;

                }
                else if (c == ">" || c == "<" || c == "<=" || c == ">=" || c == "!=" || c == "==")
                {
                    position++;
                    token rigthNode = ParseExpression();
                    tokenBul condicion = new tokenBul(c, TokenTypes.boolean);
                    condicion.tokens.Add(leftNode);
                    condicion.tokens.Add(rigthNode);
                    if (position < expression.Count && (expression[position].Value == ")" || expression[position].Value == ";"))
                        return condicion;
                    if (position > expression.Count - 1)
                        return condicion;
                    c = expression[position].Value;
                    if (aumentoContador() && (c != ">" || c != "<" || c != "<=" || c != ">=" || c != "!=" || c != "==" || c != "&&" || c != "||"))
                        position++;
                    leftNode = condicion;
                }
                else if (c == "&&" || c == "||")
                {
                    position++;
                    tokenBul condicion = new tokenBul(c, TokenTypes.boolean);
                    token rigthNode = ParseExpression();
                    condicion.tokens.Add(leftNode);
                    condicion.tokens.Add(rigthNode);
                    if (position < expression.Count && (expression[position].Value == ")" || expression[position].Value == ";"))
                        return condicion;
                    if (position > expression.Count - 1)
                        return condicion;
                    c = expression[position].Value;
                    if (aumentoContador() && (c != ">" || c != "<" || c != "<=" || c != ">=" || c != "!=" || c != "==" || c != "&&" || c != "||"))
                        position++;
                    leftNode = condicion;

                }

            }
            return leftNode;
        }
        private token ParseTerm()
        {
            token leftNode = ParseFactor();

            while (position < expression.Count)
            {
                string c = expression[position].Value;

                if (c == "*" || c == "/" || c == "%")
                {
                    OperatorNode operatorNode = new OperatorNode(c, TokenTypes.Operator);
                    position++;
                    token rightNode = ParseFactor();
                    operatorNode.tokens.Add(leftNode);
                    operatorNode.tokens.Add(rightNode);
                    if (position < expression.Count && (expression[position].Value == ")" || expression[position].Value == ";"))
                        return operatorNode;
                    if (position > expression.Count - 1)
                        return operatorNode;
                    c = expression[position].Value;
                    if (aumentoContador() && (c != ">" || c != "<" || c != "<=" || c != ">=" || c != "!=" || c != "==" || c != "&&" || c != "||"))
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
        private token ParseFactor()
        {
            token node = null;

            string ValorDelToken = expression[position].Value;
            if (expression[position] is Identificador && variablesLocales.Any(valor => valor.Value == expression[position].Value) && expression[position + 1].Value == "=")
            {
                throw new ArgumentException("esta variables ya fue definida en este contexto");
            }
            else if (position < expression.Count - 1 && Tokenizar.colores(expression[position].Value))
            {
                color.Push(expression[position].Value);
                position++;
                if (expression[position].Value == ";")
                {
                    return node;
                }
                else
                {
                    throw new ArgumentException("esperabamos un punto y coma despues del color");
                }

            }
            else if (position < expression.Count - 1 && expression[position].Value == "restore")
            {
                color.Pop();
                position++;
                if (expression[position].Value == ";")
                {

                    return node;
                }
                else
                {
                    throw new ArgumentException("esperabamos un punto y coma despues de restaurar el color");
                }
            }
            else if (ValorDelToken == "count")
            {
                return Count();
            }
            else if (ValorDelToken == "samples")
            {
                return Samples();
            }
            // me devuelve una funcion definida en el lenguaje
            if (expression[position] is Identificador && expression[position + 1].Value == "(")
            {
                return FuncionesGeo();
            }
            //devuelve una figura
            else if (TiposFigura(ValorDelToken) || variablesLocales.Any(valor => valor.Value == expression[position].Value))
            {
                return Figura(ValorDelToken);
            }
            else if (expression[position].Type == TokenTypes.Identifier && expression[position + 1].Value == "=" && expression[position + 2].Value != "{")
            {
                return Identificadores();
            }
            else if (LineSecuenceTipo())
            {
                return LineSecuence(expression[position].Value);
            }
            else if (expression[position].Type == TokenTypes.Underfine)
            {
                position++;
                return (token)expression[position - 1].Clone();
            }
            //me devuelve un token numero 
            else if (double.TryParse(ValorDelToken, out double value))
            {
                position++;
                return new TokenNumero(value.ToString(), TokenTypes.Number);
            }
            //me devuelve una funcion sen , coseno , tan , sqrt
            else if (Isfunction(ValorDelToken))
            {
                return ParseFunction();
            }
            //me devuelve un token if- else ;
            else if (ValorDelToken == "if")
            {
                return ParserIFelse();
            }
            //me devuelve un token let - in;
            else if (ValorDelToken == "let")
            {
                return Letin();
            }
            //me devuleve un token draw con los hijos a pintar ;
            else if (ValorDelToken == "draw")
            {
                position++;
                return DrawFunction();
            }

            else if (ValorDelToken == "{")
            {

                return SecuenciaFinita();
            }
            // si es alguno de los caracteres continua el parseo ;
            else if (ValorDelToken == "(" || ValorDelToken == "=" || ValorDelToken == "=>")
            {
                if (ValorDelToken == "(") parentesis.Push("(");
                position++;
                node = ParseExpression();
                if (expression[position].Value == ")" && parentesis.Count > 0)
                    parentesis.Pop();
            }

            return node;
        }
    }
}
