using TokensGeo;
using Lexer;
namespace ParserGeo;

public partial class Geometrico
{
       public bool CheckSemantic(List<Errors> errores)
   {
       bool chek = true;
        foreach (var item in variablesLocales)
        {
             item.CheckSemantic(errors);
        }
        foreach (var item in tokens)
        {
            item.CheckSemantic(errors);
        }
        ErroresVariablesLocales();
        InferenciaSobrecarga(tokens);
        if(errors.Count == 0)return true;
        return false; 
   }
   //metodo que evalua el arbol
   public  void  Evaluate ()
   {
      
   }
    private void InferenciaSobrecarga(List <token> tokens)
   {
    if (tokens.Count == 0)return ;
       foreach (var item in tokens)
       {
            if (item is Function)
            {
                bool sinError = true ;
                Function auxiliar = (Function)Valido((Function)item);
                if(auxiliar != null)
                {
                    for (int i = 0; i < auxiliar.variablesLocales.Count; i++)
                    {
                        if(auxiliar.variablesLocales[i].TypeReturn != ((Function)item).variablesLocales[i].TypeReturn && auxiliar.variablesLocales[i].TypeReturn != TokenTypes.Identifier && auxiliar.variablesLocales[i].TypeReturn != TokenTypes.comparacion )
                        {
                            sinError = false ;
                            errors.Add(new Errors(ErrorCode.Semantic , "En la funcion " + auxiliar.Value + " el parametro " + ((Function)auxiliar).variablesLocales[i].Value + " recibe un tipo " + ((Function)auxiliar).variablesLocales[i].TypeReturn));
                        }
                        else if(auxiliar.TypeReturn == TokenTypes.Identifier&&  item.TypeReturn != TokenTypes.Number && item.TypeReturn !=TokenTypes.Literal&& item.TypeReturn != TokenTypes.Identifier)
                        {
                            sinError = false ;
                            errors.Add(new Errors(ErrorCode.Semantic , "En la funcion " + auxiliar.Value + " el parametro " + ((Function)auxiliar).variablesLocales[i].Value + " recibe un tipo Number o Literal "));
                        }
                    }
                
                 if (auxiliar.TypeReturn != TokenTypes.Identifier) 
                 item.TypeReturn = auxiliar.TypeReturn;
                 if (sinError)
                 {
                    ((Function)item).FuncionGuardada = auxiliar.FuncionGuardada;
                 }
                }
            }
            else
            {
                InferenciaSobrecarga(item.tokens);
            }
        }
           
   }
   private Function Valido (Function item)
   {
     List <  Function> FuncionesGuardadas = new List <Function> ();
        for ( int i =  0 ; i < variablesLocales.Count ; i ++)
        {
            if (variablesLocales[i].Value == item.Value && variablesLocales[i] is Function && ((Function)variablesLocales[i]).variablesLocales.Count == item.variablesLocales.Count )
            {
                ((Function)variablesLocales[i]).FuncionGuardada = i ;
                FuncionesGuardadas.Add((Function)variablesLocales[i]);
            }
        }
        if (FuncionesGuardadas.Count == 1)
        return FuncionesGuardadas[0];
        else if (FuncionesGuardadas.Count > 1)
        {
            bool correctos = true;
            foreach (Function items in FuncionesGuardadas)
            {
                correctos = true;
                for (int i = 0; i < item.variablesLocales.Count; i++)
                {
                    if(items.variablesLocales[i].TypeReturn != TokenTypes.Identifier && items.variablesLocales[i].TypeReturn != item.variablesLocales[i].TypeReturn )
                    {
                        correctos = false;
                        break;
                    }
                    else if(items.variablesLocales[i].TypeReturn == TokenTypes.Identifier && item.variablesLocales[i].TypeReturn != TokenTypes.Literal && item.variablesLocales[i].TypeReturn != TokenTypes.Number && item.variablesLocales[i].TypeReturn != TokenTypes.Identifier)
                    {
                        correctos = false;
                        break ;
                    }
                }
                 if (correctos)
                {
                    if (items.TypeReturn != TokenTypes.Identifier) 
                    item.TypeReturn = items.TypeReturn;
                    return items;
                }
                
            }
            if (!correctos)
            {
                errors.Add(new Errors (ErrorCode.Semantic , "no existe una funcion en este contexto , q reciba parametros con esos tipos"));
                return null;
            }
        }
        else 
        {
             errors.Add(new Errors (ErrorCode.Semantic , "no existe la funcion "+ item.Value + " en este contexto que cumpla con los parametros"));
            
        }
    return null;
   }
   private void ErroresVariablesLocales()
   {
    bool Sobrecarga = true ;
    
    for (int i = 0; i < variablesLocales.Count; i++)
    {   
        
        for (int j = i + 1; j < variablesLocales.Count; j++)
        {
            if (variablesLocales[i] is Function && variablesLocales[j] is Function && ((Function)variablesLocales[i]).variablesLocales.Count == ((Function)variablesLocales[j]).variablesLocales.Count)
            {
                int count = ((Function)variablesLocales[i]).variablesLocales.Count;
                int count1 = ((Function)variablesLocales[j]).variablesLocales.Count;
                for (int k = 0; k < ((Function)variablesLocales[i]).variablesLocales.Count ; k++)
                {
                    if (((Function)variablesLocales[i]).variablesLocales[k].TypeReturn != ((Function)variablesLocales[j]).variablesLocales[k].TypeReturn)
                    {
                        Sobrecarga = false;
                        break;
                    }
                }
                if(Sobrecarga && count == count1)
                {
                    errors.Add(new Errors (ErrorCode.Semantic ,"la funcion " + variablesLocales[i].Value + " fue definida mas de una vez en este contexto"));
                }
            }
        }
    }
   }
 
}