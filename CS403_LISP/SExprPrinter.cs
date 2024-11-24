using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class SExprPrinter
{
    public static string Print(SExpr expr)
    {
        if (expr == SExpr.Nil)
        {
            return "nil";
        }
        else if (expr == SExpr.Truth)
        {
            return "#t";
        }
        else if (expr is SExpr.Atom atom)
        {
            return atom.Value;
        }
        else if (expr is SExpr.List list)
        {
            var elements = list.Elements.Select(Print);
            return $"({string.Join(" ", elements)})";
        }
        else if (expr is SExpr.ConsCell cons)
        {
            return PrintConsCell(cons);
        }
        else if (expr is SExpr.UserFunction func)
        {
            return $"<fn:{string.Join(" ", func.Parameters)}>";
        }
        else
        {
            throw new Exception("Unknown SExpr type");
        }
    }

    private static string PrintConsCell(SExpr expr)
    {
        var sb = new StringBuilder();
        sb.Append('(');
        PrintConsCellRecursive(expr, sb);
        sb.Append(')');
        return sb.ToString();
    }

    private static void PrintConsCellRecursive(SExpr expr, StringBuilder sb)
    {
        if (expr is SExpr.ConsCell cons)
        {
            sb.Append(Print(cons.Car));
            if (cons.Cdr != SExpr.Nil)
            {
                if (cons.Cdr is SExpr.ConsCell)
                {
                    sb.Append(' ');
                    PrintConsCellRecursive(cons.Cdr, sb);
                }
                else
                {
                    sb.Append(" . ");
                    sb.Append(Print(cons.Cdr));
                }
            }
        }
        else
        {
            sb.Append(Print(expr));
        }
    }
}
