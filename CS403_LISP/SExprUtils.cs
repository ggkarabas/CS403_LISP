// utility functions and predicates for working with SEXPR:

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

    // Utility to get the numeric value from an Atom
    public static int GetNumberValue(SExpr expr)
    {
        if (expr is SExpr.Atom atom && int.TryParse(atom.Value, out int result))
        {
            return result;
        }
        throw new ArgumentException("Expected a numeric atom");
    }

    // Arithmetic operations
    public static SExpr Add(SExpr a, SExpr b)
    {
        int result = GetNumberValue(a) + GetNumberValue(b);
        return new SExpr.Atom(result.ToString());
    }

    public static SExpr Sub(SExpr a, SExpr b)
    {
        int result = GetNumberValue(a) - GetNumberValue(b);
        return new SExpr.Atom(result.ToString());
    }

    public static SExpr Mul(SExpr a, SExpr b)
    {
        int result = GetNumberValue(a) * GetNumberValue(b);
        return new SExpr.Atom(result.ToString());
    }

    public static SExpr Div(SExpr a, SExpr b)
    {
        int divisor = GetNumberValue(b);
        if (divisor == 0)
        {
            // Return "undefined" when dividing by zero
            return new SExpr.Atom("undefined");
        }
        int result = GetNumberValue(a) / divisor;
        return new SExpr.Atom(result.ToString());
    }


    public static SExpr Mod(SExpr a, SExpr b)
    {
        int divisor = GetNumberValue(b);
        if (divisor == 0)
        {
            // Return "undefined" when modulo by zero
            return new SExpr.Atom("undefined");
        }
        int result = GetNumberValue(a) % divisor;
        return new SExpr.Atom(result.ToString());
    }


    // Relational operations
    public static SExpr Lt(SExpr a, SExpr b)
    {
        return GetNumberValue(a) < GetNumberValue(b) ? SExpr.Truth : SExpr.Nil;
    }

    public static SExpr Gt(SExpr a, SExpr b)
    {
        return GetNumberValue(a) > GetNumberValue(b) ? SExpr.Truth : SExpr.Nil;
    }

    public static SExpr Lte(SExpr a, SExpr b)
    {
        return GetNumberValue(a) <= GetNumberValue(b) ? SExpr.Truth : SExpr.Nil;
    }

    public static SExpr Gte(SExpr a, SExpr b)
    {
        return GetNumberValue(a) >= GetNumberValue(b) ? SExpr.Truth : SExpr.Nil;
    }

    // Equality operation
    public static SExpr Eq(SExpr a, SExpr b)
    {
        if (a is SExpr.Atom atomA && b is SExpr.Atom atomB)
        {
            return atomA.Value == atomB.Value ? SExpr.Truth : SExpr.Nil;
        }
        return SExpr.Nil;
    }

    // Logic operation: not
    public static SExpr Not(SExpr expr)
    {
        return expr == SExpr.Nil ? SExpr.Truth : SExpr.Nil;
    }

    public static SExpr Cadr(SExpr list)
    {
    return Car(Cdr(list));
    }

    public static SExpr Caddr(SExpr list)
    {
        return Car(Cdr(Cdr(list)));
    }

}
