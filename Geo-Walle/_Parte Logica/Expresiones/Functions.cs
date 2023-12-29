using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo_Walle;
using Lexer;
using Tokenizador;

namespace Jerarquia
{
    public class LetInExpression : Expression
    {
        List<Expression> Let;
        Expression In;

        public LetInExpression(List<Expression> let, Expression In)
        {
            this.Let = let;
            this.In = In;
        }
        public override object Value { get; set; }
        public override ExpressionType Type { get; set; }

        public override void Scope(Scope scope)
        {
            Scope L = new Scope(scope, new Dictionary<Token, object>(), new Dictionary<Token, ExpressionType>());
            foreach (var item in Let)
            {
                item.Scope(L);
            }

            Scope i = new Scope(L, new Dictionary<Token, object>(), new Dictionary<Token, ExpressionType>());
            In.Scope(i);
        }

        public override bool CheckSemantic(List<Errors> errors)
        {
            bool IN = In.CheckSemantic(errors);
            foreach (var item in Let)
            {
                if (!item.CheckSemantic(errors) || !IN)
                {
                    errors.Add(new Errors(ErrorCode.Semantic, "Error en la expresion Let-in"));
                    Type = ExpressionType.ErrorType;
                    return false;
                }
            }
            return true;
        }

        public override object Evaluate()
        {
            foreach (var item in Let)
                item.Evaluate();

            return In.Evaluate();
        }
    }

    public class AssingExpression : Expression
    {
        public AssingExpression(Token id, Expression value)
        {
            this.id = id;
            this.value = value;
        }

        public Token id;
        public Expression value;
        Scope scope;
        public override object Value { get; set; }
        public override ExpressionType Type { get; set; }

        public override void Scope(Scope scope)
        {
            scope.Type.Add(id, ExpressionType.AnyType);
            value.Scope(scope);
            this.scope = scope;
        }

        public override bool CheckSemantic(List<Errors> errors)
        {
            foreach (var item in scope.Type.Keys)
            {
                if (item.Value == id.Value)
                {
                    value.CheckSemantic(errors);
                    scope.Type[item] = value.Type;
                    Type = scope.Type[item];
                    return true;
                }
            }

            errors.Add(new Errors(ErrorCode.Semantic, "problemas en la asignacion de valores"));
            return false;
        }

        public override object Evaluate()
        {
            foreach (var item in scope.Type.Keys)
            {
                if (item.Value == id.Value)
                {
                    object result = value.Evaluate();
                    scope.Value.Add(id, result);
                    return result;
                }
            }

            throw new Exception();
        }
    }
    public class Draw : Expression
    {
        public Expression draw;
        public string Color;
        public Draw(Expression draw, string color)
        {
            this.draw = draw;
            this.Color = color; 
        }
        public override object Value { get; set; }
        public override ExpressionType Type { get; set; }

        public override void Scope(Scope scope)
        {
            draw.Scope(scope);
        }

        public override bool CheckSemantic(List<Errors> errors)
        {
            bool Draw = draw.CheckSemantic(errors);

            if(draw.Type == ExpressionType.Line || draw.Type == ExpressionType.Ray || draw.Type == ExpressionType.Segment || draw.Type == ExpressionType.Arc || draw.Type == ExpressionType.Circle || draw.Type == ExpressionType.Point || draw.Type == ExpressionType.Secuence)
                return true;

            errors.Add(new Errors(ErrorCode.Semantic, "no se puede pintar algo que no sea figura o secuencia."));
            Type = ExpressionType.ErrorType;
            return false;
        }
        public override object Evaluate()
        {
            Figura figura = (Figura)draw.Evaluate();
            figura.color = Color;
            //figura.Pintar(Color);
            Walle.InvoKEvent(figura,Color);
            return figura;
        }
    }
    public class Function : Expression
    {
        public string Name;
        public List<Token> Arguments;
        public Expression Instructions;
        public Scope scope = new Scope(Context.General, new Dictionary<Token, object>(), new Dictionary<Token, ExpressionType>());

        public Function(string Name, List<Token> Arguments, Expression Instructions)
        {
            this.Name = Name;
            this.Arguments = Arguments;
            this.Instructions = Instructions;
        }
        public override object Value { get; set; }
        public override ExpressionType Type { get; set; }

        public override void Scope(Scope scope)
        {
            foreach (var item in Arguments)
                scope.Type.Add(item, ExpressionType.AnyType);

            Scope s = new Scope(scope, new Dictionary<Token, object>(), new Dictionary<Token, ExpressionType>());
            Instructions.Scope(s);
            this.scope = scope;
        }

        public override bool CheckSemantic(List<Errors> errors)
        {
            Type = ExpressionType.AnyType;
            return true;
        }

        public override object Evaluate()
        {
            throw new NotImplementedException();
        }
    }

    public class FuncCall : Expression
    {
        public FuncCall(List<Expression> expressions, Function f)
        {
            exp = expressions;
            func = f;
        }
        public override object Value { get; set; }
        public override ExpressionType Type { get; set; }
        Function func;
        List<Expression> exp;

        public override bool CheckSemantic(List<Errors> errors)
        {
            if (func.Arguments.Count == exp.Count)
            {
                foreach (var item in exp)
                    item.CheckSemantic(errors);

                Type = ExpressionType.AnyType;
                return true;
            }

            Type = ExpressionType.ErrorType;
            return false;
        }

        public override object Evaluate()
        {
            Dictionary<Token, object> dicc = func.scope.Value;
            Dictionary<Token, object> Arguments = new Dictionary<Token, object>();
            int index = 0;

            foreach (var item in exp)
            {
                Arguments.Add(func.Arguments[index], item.Evaluate());
                index++;
            }

            func.scope.Value = Arguments;
            object result = func.Instructions.Evaluate();
            Value = result;
            func.scope.Value = dicc;

            return result;
        }

        public override void Scope(Scope scope)
        {
            foreach (var item in exp)
            {
                item.Scope(scope);
            }
            return;
        }
    }
}

