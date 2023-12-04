using TokensGeo;
namespace Lexer
{
   public class Tokenizar
   {  
     public static List<token> TokenizeString(string input)
     {
        List<token> tokens = new List<token>();
        string currentToken = "";

        for (int i = 0; i < input.Length; i++)
        {
            char currentChar = input[i];
            if (currentChar == ' ')
            {
                continue;
            }
            if (IsOperator(currentChar.ToString()))
            {
                    tokens.Add(new OperatorNode(currentChar.ToString(), TokenTypes.Operator));
                    currentToken = "";
                    continue;
            }
            else if (IsPunctuation(currentChar.ToString()))
            {
                if (currentChar == '=' && input[i + 1] == '>' )
                {
                    tokens.Add(new token("=>" , TokenTypes.Punctuation));
                    currentToken = "";
                    i++;
                    continue;
                }
                else if (currentChar == '<' && tokens[tokens.Count - 1].Value == "line")
                {
                    tokens[tokens.Count - 1].Type = TokenTypes.Comando;
                }
                else if (currentChar == '(' && tokens[tokens.Count - 1].Value == "line")
                {
                    tokens[tokens.Count - 1].Type = TokenTypes.Funcion;
                    tokens.Add(new token (currentChar.ToString() , TokenTypes.Punctuation));
                }
                else if (currentChar == '<' && input [i + 1] == '=')
                {
                    tokens.Add(new token("<=" , TokenTypes.Punctuation));
                    currentToken = "";
                    i++;
                    continue;
                }
                else if (currentChar == '>' && input [i + 1] == '=')
                {
                    tokens.Add(new token(">=" , TokenTypes.Punctuation));
                    currentToken = "";
                    i++;
                    continue;
                }
                else if (currentChar == '=' && input [i + 1] == '=')
                {
                    tokens.Add(new token("==" , TokenTypes.Punctuation));
                    currentToken = "";
                    i++;
                    continue;
                }
                else
                {
                    tokens.Add(new token(currentChar.ToString(), TokenTypes.Punctuation));
                }
            }
            else
            {
                currentToken += currentChar;
                for (int j = i + 1 ; j < input.Length; j++)
                {
                     if(!IsPunctuation(input[j].ToString()) && input[j] != ' ')
                    {
                       currentToken += input[j];
                       continue;
                    }
                    //si hay un comando compuesto como point sequence o line sequence 
                    else if (IsComando(currentToken) && currentToken == "sequence")
                    {
                        if (tokens[tokens.Count - 1].Value == "point")
                        {
                            tokens[tokens.Count - 1].Value = "point sequence";
                            currentToken = "";
                            i = j ;
                            break ;
                        }
                        else if(tokens[tokens.Count - 1].Value == "line")
                        {
                            tokens[tokens.Count - 1].Value = "line sequence";
                            currentToken = "";
                            i = j ;
                            break ;
                        }
                    }
                    //tokens que son tipo comando 
                    else if(IsComando(currentToken))
                    {
                     tokens.Add(new token (currentToken , TokenTypes.Comando));
                     currentToken = "";
                     i = j;
                     break;
                    }
                    else if(IsKeyword(currentToken))
                    {
                     tokens.Add(new token (currentToken , TokenTypes.Keyword));
                     currentToken = "";
                     i = j;
                     break;
                    }
                    //tokens funciones 
                    else if (Isfunction(currentToken))
                    {
                    tokens.Add(new token(currentToken , TokenTypes.Comando));
                    i = j;
                    currentToken  = "";
                    break;
                    }
                    //si el token es un numero 
                    else if (double.TryParse(currentToken ,out double value ))
                    {
                    tokens.Add(new TokenNumero(currentToken ,TokenTypes.Number));
                    i = j ;
                    currentToken  = ""; 
                    break;
                    }
                     if (tokens.Count > 0 && tokens[tokens.Count - 1].Type == TokenTypes.Comando)
                       {
                            if (tokens[tokens.Count - 1].Value == "point")
                            {
                               tokens[tokens.Count - 1] = new token(currentToken , TokenTypes.Point);
                               currentToken = "";
                               i = j ;
                               break;
                            }
                           else if (tokens[tokens.Count - 1].Value == "line")
                            {
                               tokens[tokens.Count - 1] = new token(currentToken , TokenTypes.Line);
                                currentToken = "";
                               i = j ;
                               break;
                            }
                           else if (tokens[tokens.Count - 1].Value == "segment")
                            {
                               tokens[tokens.Count - 1] = new token(currentToken , TokenTypes.Segment);
                                currentToken = "";
                               i = j ;
                               break;
                            }
                          else if (tokens[tokens.Count - 1].Value == "ray")
                            {
                               tokens[tokens.Count - 1] = new token(currentToken , TokenTypes.Ray);
                                currentToken = "";
                               i = j ;
                               break;
                            }
                           else if (tokens[tokens.Count - 1].Value == "circle")
                            {
                               tokens[tokens.Count - 1] = new token(currentToken , TokenTypes.Circle);
                                currentToken = "";
                               i = j ;
                               break;
                            }
                           else if (tokens[tokens.Count - 1].Value == "point sequence")
                            {
                               tokens[tokens.Count - 1] = new token(currentToken , TokenTypes.point_sequence);
                                currentToken = "";
                               i = j ;
                               break;
                            }
                          else if (tokens[tokens.Count - 1].Value == "line sequence")
                            {
                               tokens[tokens.Count - 1] = new token(currentToken , TokenTypes.line_sequence);
                               currentToken = "";
                               i = j ;
                               break;
                            }
                        else if (tokens[tokens.Count - 1].Value == "color")
                       {
                         tokens[tokens.Count - 1] = new Color(currentToken , TokenTypes.Color);
                         i = j ;
                         currentToken = "";
                         break;
                       }
                       }
                      if (input[j] == ' ' || IsPunctuation(input[j].ToString()))
                    {
                          tokens.Add(new Identificador (currentToken , TokenTypes.Identifier));
                          currentToken = "";
                          i = j ;
                          break;
                    }
    
                }
                if (input[i] != ' ' && IsPunctuation(input[i].ToString()))
                {
                tokens.Add(new token (input[i].ToString()  , TokenTypes.Punctuation));
                continue;
                }
            }
               
        }
      return tokens ;          
     }

      public static bool IsOperator(string c)
     {
        return c == "+" || c == "-" || c == "*" || c == "/" ;
     }
      public  static bool IsPunctuation(string c)
     {
        return c == "." || c == "," || c == ";" || c == ":" || c == "\"" || c == "=>" || c == "=" || c == ">" || c == "<" || c == "<=" || c == "!=" || c == "==" || c == "{" || c == "}" || c== "(" || c == ")";
     }
      public static bool IsComando(string c)
     {
        return c == "let" || c == "in" || c =="point" || c == "line" || c == "segment" || c == "ray" || c == "circle" || c == "sequence" || c == "color" ;
     }
      public  static bool Isfunction(string c)
     {
        return c == "color" || c == "restore" || c == "import" || c == "draw"  || c == "arc" || c == "measure" || c == "intersect" || c == "counts" || c == "randoms" || c == "points" || c == "samples" || c == "underfine" || c == "rest" || c == "_";
     }
     public static bool IsKeyword(string c)
     {
        return c == "if"|| c == "else" || c == "let" || c == "in"|| c == "then" ;
     }
   
   }

    
}