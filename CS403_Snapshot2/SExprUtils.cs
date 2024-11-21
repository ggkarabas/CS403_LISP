public static class SExprUtils
{
    public static bool ToBoolean(SExpr expr)
    {
        return expr != SExpr.Nil;  
    }

    public static bool IsNil(SExpr expr)
    {
        return expr == SExpr.Nil;
    }

    public static bool IsSymbol(SExpr expr)
    {
        return expr is SExpr.Atom && !int.TryParse(((SExpr.Atom)expr).Value, out _);  
    }

    public static bool IsNumber(SExpr expr)
    {
        return expr is SExpr.Atom && int.TryParse(((SExpr.Atom)expr).Value, out _);  
    }

    public static bool IsList(SExpr expr)
    {
        return expr is SExpr.List;
    }

    public static SExpr Car(SExpr expr)
    {
        if (expr is SExpr.ConsCell cons) 
        {
            return cons.Car;
        }
        throw new ArgumentException("Cannot take car of non-cons cell");
    }

    public static SExpr Cdr(SExpr expr)
    {
        if (expr is SExpr.ConsCell cons)  
        {
            return cons.Cdr;
        }
        throw new ArgumentException("Cannot take cdr of non-cons cell");
    }
}
