using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GettextLib.ExpressionEvaluator
{
    internal class Script : IOutput
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
    }

    internal interface IOutput
    {
        string ToPrint();
    }

    internal class Assignment : IOutput
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
            return string.Format("{0} = {1};", Var, Expr.ToPrint());
        }
    }

    internal abstract class Expr : IOutput
    {
        public abstract string ToPrint();
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
            return string.Format("({0})", expr.ToPrint());
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
            return string.Format("{0} {1} {2}", left.ToPrint(), ToPrint(op), right.ToPrint());
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
            return string.Format("({0}) ? ({1}) : ({2})", condition.ToPrint(), statement1.ToPrint(), statement2.ToPrint());
        }
    }
}
