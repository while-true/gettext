using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GettextLib.ExpressionEvaluator
{
    internal class ExpressionState
    {
        private Dictionary<string, long> variables;

        internal ExpressionState()
        {
            variables = new Dictionary<string, long>();
        }

        public void SetVar(string i, long val)
        {
            if (!variables.ContainsKey(i))
            {
                variables.Add(i, val);
                return;
            }

            variables[i] = val;
        }

        public long GetVar(string i)
        {
            if (!variables.ContainsKey(i))
            {
                throw new Exception(string.Format("No variable {0} set!", i));
            }

            return variables[i];
        }

        public string PrintState()
        {
            var sb = new StringBuilder();
            foreach (var variable in variables)
            {
                sb.AppendFormat("{0}={1}\n", variable.Key, variable.Value);
            }
            return sb.ToString();
        }
    }

    internal class Script : IExpr
    {
        public List<Assignment> Assignments { get; set; } 

        public Script()
        {
            Assignments = new List<Assignment>();
        }

        public string ToPrint()
        {
            return string.Join("; ", Assignments.Select(x => x.ToPrint()));
        }

        public long Execute(ExpressionState state)
        {
            foreach (var assignment in Assignments)
            {
                assignment.Execute(state);
            }

            return 0;
        }
    }

    internal interface IExpr
    {
        string ToPrint();
        long Execute(ExpressionState state);
    }

    internal class Assignment : IExpr
    {
        public string Var { get; set; }
        public Expr Expr { get; set; }

        public Assignment(string @var, Expr expr)
        {
            Var = var;
            Expr = expr;
        }

        public string ToPrint()
        {
            return string.Format("{0} = {1}", Var, Expr.ToPrint());
        }

        public long Execute(ExpressionState state)
        {
            var execute = Expr.Execute(state);
            state.SetVar(Var, execute);
            return execute;
        }
    }

    internal abstract class Expr : IExpr
    {
        public abstract string ToPrint();
        public abstract long Execute(ExpressionState state);
    }

    internal class ExprWrapper : Expr
    {
        private Expr expr;

        public ExprWrapper(Expr expr)
        {
            this.expr = expr;
        }

        public override string ToPrint()
        {
            return string.Format("{0}", expr.ToPrint());
        }

        public override long Execute(ExpressionState state)
        {
            return expr.Execute(state);
        }
    }

    internal abstract class Literal : Expr
    {
        
    }

    internal class LiteralNumber : Literal
    {
        private readonly long num;

        public LiteralNumber(long num)
        {
            this.num = num;
        }

        public override string ToPrint()
        {
            return num.ToString();
        }

        public override long Execute(ExpressionState state)
        {
            return num;
        }
    }

    internal class LiteralVar : Literal
    {
        private readonly string id;

        public LiteralVar(string id)
        {
            if (id == null) throw new ArgumentNullException("id");
            this.id = id;
        }

        public override string ToPrint()
        {
            return id;
        }

        public override long Execute(ExpressionState state)
        {
            return state.GetVar(id);
        }
    }

    internal class ExprTwo : Expr
    {
        private Expr left;
        private Expr right;

        public enum OpEnum
        {
            Equals,
            NotEquals,
            And,
            Minus,
            Modulo,
            Or,
            LessThan,
            GreaterThan,
            LessThanOrEquals,
            GreaterThanOrEquals
        }

        private OpEnum op;

        public ExprTwo(Expr left, Expr right, OpEnum op)
        {
            this.left = left;
            this.right = right;
            this.op = op;
        }

        private string ToPrint(OpEnum o)
        {
            switch (o)
            {
                case OpEnum.Equals:
                    return "==";
                    break;
                case OpEnum.NotEquals:
                    return "!=";
                    break;
                case OpEnum.And:
                    return "&&";
                    break;
                case OpEnum.Minus:
                    return "-";
                    break;
                case OpEnum.Modulo:
                    return "%";
                    break;
                case OpEnum.Or:
                    return "||";
                    break;
                case OpEnum.LessThan:
                    return "<";
                    break;
                case OpEnum.GreaterThan:
                    return ">";
                    break;
                case OpEnum.LessThanOrEquals:
                    return "<=";
                    break;
                case OpEnum.GreaterThanOrEquals:
                    return ">=";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("o");
            }
        }

        public override string ToPrint()
        {
            return string.Format("({0}) {1} ({2})", left.ToPrint(), ToPrint(op), right.ToPrint());
        }

        public override long Execute(ExpressionState state)
        {
            switch (op)
            {
                case OpEnum.Equals:
                    return (left.Execute(state) == right.Execute(state)) ? 1 : 0;
                case OpEnum.NotEquals:
                    return (left.Execute(state) != right.Execute(state)) ? 1 : 0;
                    break;
                case OpEnum.And:
                    return ((left.Execute(state) == 1) && (right.Execute(state) == 1)) ? 1 : 0;
                    break;
                case OpEnum.Minus:
                    return left.Execute(state) - right.Execute(state);
                    break;
                case OpEnum.Modulo:
                    return left.Execute(state) % right.Execute(state);
                    break;
                case OpEnum.Or:
                    return ((left.Execute(state) == 1) || (right.Execute(state) == 1)) ? 1 : 0;
                    break;
                case OpEnum.LessThan:
                    return (left.Execute(state) < right.Execute(state)) ? 1 : 0;
                    break;
                case OpEnum.GreaterThan:
                    return (left.Execute(state) > right.Execute(state)) ? 1 : 0;
                    break;
                case OpEnum.LessThanOrEquals:
                    return (left.Execute(state) <= right.Execute(state)) ? 1 : 0;
                    break;
                case OpEnum.GreaterThanOrEquals:
                    return (left.Execute(state) >= right.Execute(state)) ? 1 : 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    internal class ExprIf : Expr
    {
        private Expr condition;
        private Expr statement1;
        private Expr statement2;

        public ExprIf(Expr condition, Expr statement1, Expr statement2)
        {
            this.condition = condition;
            this.statement1 = statement1;
            this.statement2 = statement2;
        }

        public override string ToPrint()
        {
            return string.Format("{0} ? {1} : {2}", condition.ToPrint(), statement1.ToPrint(), statement2.ToPrint());
        }

        public override long Execute(ExpressionState state)
        {
            return (condition.Execute(state) == 1) ? statement1.Execute(state) : statement2.Execute(state);
        }
    }
}
