using System.Collections.Generic;
namespace Tokenizador
{
    /// <summary>
    /// metodo para tokenizar el input
    /// </summary> <summary>
    /// 
    /// </summary>
    public class lexer
    {/// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <returns> returna una lista de tokens </returns>
        public static List<Token> TokenizeString(string input)
        {
            List<Token> tokens = new List<Token>();
            string currentToken = "";

            for (int i = 0; i < input.Length; i++)
            {
                char currentChar = input[i];
                if (currentChar == ' ')
                {
                    continue;
                }
                else if (IsOperator(currentChar.ToString()))
                {
                    tokens.Add(new Token(currentChar.ToString(), TokenTypes.Operator));
                    currentToken = "";
                    continue;
                }
                else if (IsPunctuation(currentChar.ToString()))
                {
                    if (currentChar == '=' && input[i + 1] == '>')
                    {
                        tokens.Add(new Token("=>", TokenTypes.Punctuation));
                        currentToken = "";
                        i++;
                        continue;
                    }
                    else if (currentChar == '<' && tokens[tokens.Count - 1].Value == "line")
                    {
                        tokens[tokens.Count - 1].Type = TokenTypes.Keyword;
                    }
                    else if (currentChar == '(' && tokens[tokens.Count - 1].Value == "line")
                    {
                        tokens[tokens.Count - 1].Type = TokenTypes.funcion;
                        tokens.Add(new Token(currentChar.ToString(), TokenTypes.Punctuation));
                    }
                    else if (currentChar == '<' && input[i + 1] == '=')
                    {
                        tokens.Add(new Token("<=", TokenTypes.Punctuation));
                        currentToken = "";
                        i++;
                        continue;
                    }
                    else if (currentChar == '>' && input[i + 1] == '=')
                    {
                        tokens.Add(new Token(">=", TokenTypes.Punctuation));
                        currentToken = "";
                        i++;
                        continue;
                    }
                    else if (currentChar == '=' && input[i + 1] == '=')
                    {
                        tokens.Add(new Token("==", TokenTypes.Punctuation));
                        currentToken = "";
                        i++;
                        continue;
                    }
                    else
                    {
                        tokens.Add(new Token(currentChar.ToString(), TokenTypes.Punctuation));
                    }
                }
                else
                {
                    currentToken += currentChar;
                    for (int j = i + 1; j < input.Length; j++)
                    {
                        if (currentToken != "\r\n" && input[j] != '\r' && input[j] != '\t' && input[j] != '\n' && (!IsPunctuation(input[j].ToString()) && input[j] != ' ' || IsOperator(input[j].ToString())))
                        {
                            currentToken += input[j];
                            continue;
                        }
                        //si hay un comando compuesto como point sequence o line sequence 
                        else if (tokens.Count > 0 && IsKeyword(currentToken) && currentToken == "sequence")
                        {
                            if (tokens[tokens.Count - 1].Value == "point")
                            {
                                tokens[tokens.Count - 1].Value = "point sequence";
                                currentToken = "";
                                i = j;
                                break;
                            }
                            else if (tokens[tokens.Count - 1].Value == "line")
                            {
                                tokens[tokens.Count - 1].Value = "line sequence";
                                currentToken = "";
                                i = j;
                                break;
                            }
                        }
                        else if (colores(currentToken) && tokens[tokens.Count - 1].Value == "color")
                        {
                            tokens[tokens.Count - 1] = new Token(currentToken, TokenTypes.Color);
                            currentToken = "";
                            i = j;
                            break;
                        }
                        //tokens que son tipo comando 
                        else if (IsKeyword(currentToken))
                        {
                            tokens.Add(new Token(currentToken, TokenTypes.Keyword));
                            currentToken = "";
                            i = j;
                            break;
                        }
                        //tokens funciones 
                        else if (Isfunction(currentToken))
                        {
                            tokens.Add(new Token(currentToken, TokenTypes.funcion));
                            i = j;
                            currentToken = "";
                            break;
                        }
                        //si el token es un numero 
                        else if (double.TryParse(currentToken, out double value))
                        {
                            tokens.Add(new Token(currentToken, TokenTypes.Number));
                            i = j;
                            currentToken = "";
                            break;
                        }
                        if (input[j] == ' ')
                        {
                            tokens.Add(new Token(currentToken, TokenTypes.Identifier));
                            currentToken = "";
                            i = j;
                            break;
                        }
                        if (IsPunctuation(input[j].ToString()))
                        {
                            tokens.Add(new Token (currentToken, TokenTypes.Identifier));
                            currentToken = "";
                            i = j;
                            break;
                        }
                        if (currentToken == "\r\n" || currentToken == "\r" || currentToken == "\n" || currentToken == "\t")
                        {
                            currentToken = "";
                            j--;
                            continue;
                        }
                    }

                    if (input[i] != ' ' && IsPunctuation(input[i].ToString()))
                    {
                        tokens.Add(new Token(input[i].ToString(), TokenTypes.Punctuation));
                        continue;
                    }
                }

            }
            return tokens;
        }

       /// <summary>
        /// 
        /// </summary>
        /// <param name="c">  representa la palabra a evaluar </param>
        /// <returns> returna si lo cumple o no </returns> <summary>
        public static bool IsOperator(string c)
        {
            return c == "+" || c == "-" || c == "*" || c == "/" || c == "^" || c == "%";
        }
       
        public static bool IsPunctuation(string c)
        {
            return  c == "," || c == ";" || c == ":" || c == "\"" || c == "=>" || c == "=" || c == ">" || c == "<" || c == "<=" || c == "!=" || c == "==" || c == "{" || c == "}" || c == "(" || c == ")";
        }
       
        public static bool Isfunction(string c)
        {
            return c == "sen" || c == "cos" || c == "tan" || c == "sqrt" ;
        }
        public static bool IsKeyword(string c)
        {
            return c == "if" || c == "else" || c == "let" || c == "in" || c == "then" ||  c == "let" || c == "in" || c == "point" || c == "line" || c == "segment" || c == "ray" || c == "circle" || c == "draw" ||
             c == "color" || c == "restore" || c == "import" || c == "arc" || c == "measure" || c == "intersect" || c == "counts" || c == "randoms" || c == "points" || c == "samples" ;
        }
        public static bool colores(string color)
        {
            return color == "red" || color == "green" || color == "yellow" || color == "orange" || color == "blue";
        }
    }


 }