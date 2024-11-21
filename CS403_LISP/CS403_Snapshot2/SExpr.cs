using System.Collections.Generic;

public abstract class SExpr
{
    public static readonly SExpr Nil = new Atom("nil");
    public static readonly SExpr Truth = new Atom("#t");

    public class Atom : SExpr
    {
        public string Value { get; }
        public Atom(string value) => Value = value;
    }

    public class List : SExpr
    {
        public List<SExpr> Elements { get; }
        public List(List<SExpr> elements) => Elements = elements;
    }

    public static SExpr CreateAtom(string value)
    {
    if (int.TryParse(value, out _))
    {
        return new Atom(value);  // Number
    }
    else
    {
        return new Atom(value);  // Symbol
    }

    }

    public class ConsCell : SExpr
    {
    public SExpr Car { get; }
    public SExpr Cdr { get; }

    public ConsCell(SExpr car, SExpr cdr)
    {
        Car = car;
        Cdr = cdr;
    }
    }




}
