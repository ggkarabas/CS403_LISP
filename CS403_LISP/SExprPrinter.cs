using System;
using System.Text;

public static class SExprPrinter
{
    public static string Print(SExpr expr)
    {
        switch (expr)
        {
            case SExpr.Atom atom:
                return atom.Value;
            case SExpr.List list:
                StringBuilder sb = new StringBuilder();
                sb.Append("(");
                for (int i = 0; i < list.Elements.Count; i++)
                {
                    if (i > 0) sb.Append(" ");
                    sb.Append(Print(list.Elements[i]));
                }
                sb.Append(")");
                return sb.ToString();
            case SExpr.ConsCell cons:
                StringBuilder consSb = new StringBuilder();
                consSb.Append("(");
                consSb.Append(Print(cons.Car));
                var cdr = cons.Cdr;
                if (cdr != SExpr.Nil)
                {
                    consSb.Append(" . ");
                    consSb.Append(Print(cdr));
                }
                consSb.Append(")");
                return consSb.ToString();
            default:
                throw new ArgumentException("Unknown S-Expression type");
        }
    }
}
