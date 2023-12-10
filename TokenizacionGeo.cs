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
                     if(!IsPunctuation(input[j].ToString()) && input[j] != ' ' || IsOperator(input[j].ToString()) || input[j] == '\'' && input[j + 1] == 'r' || input[j] == 'r' && input[j - 1] == '\'')
                    {
                       currentToken += input[j];
                       continue;
                    }
                    //si hay un comando compuesto como point sequence o line sequence 
                    else if (tokens.Count > 0 && IsComando(currentToken) && currentToken == "sequence")
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
                    else if (currentToken == "undefined") 
                    {
                        tokens.Add(new Underfine("undefined", TokenTypes.Underfine));
                        currentToken ="";
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
                    tokens.Add(new Function(currentToken , TokenTypes.Funcion , null));
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
                      if (input[j] == ' ')
                    {
                         tokens.Add(new Identificador (currentToken , TokenTypes.Identifier));
                         currentToken = "";
                          i = j ;
                          break;
                    }
                    if(IsPunctuation(input[j].ToString()))
                    {
                         tokens.Add(new Identificador (currentToken , TokenTypes.Identifier));
                         currentToken = "";
                          i = j ;
                          break;
                    }
                }
                if (input[i] == '\'' && input[i + 1] == 'r')
                {
                    i ++;
                    currentToken = "";
                    continue;

                }
                if (input[i] != ' '  && IsPunctuation(input[i].ToString()))
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
        return c == "let" || c == "in" || c =="point" || c == "line" || c == "segment" || c == "ray" || c == "circle" || c == "draw";
     }
      public  static bool Isfunction(string c)
     {
        return c == "color" || c == "restore" || c == "import" || c == "arc" || c == "measure" || c == "intersect" || c == "counts" || c == "randoms" || c == "points" || c == "samples" || c == "underfine" ;
     }
     public static bool IsKeyword(string c)
     {
        return c == "if"|| c == "else" || c == "let" || c == "in"|| c == "then" ;
     }
   
   }

    
}