using System.Collections.Generic;

public abstract class SExpr
{
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
}
