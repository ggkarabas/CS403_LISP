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
            default:
                throw new ArgumentException("Unknown S-Expression type");
        }
    }
}
