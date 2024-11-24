public abstract class SExpr
{
    public static readonly SExpr Nil = new NilType();
    public static readonly SExpr Truth = new TruthType();

    public class Atom : SExpr
    {
        public string Value { get; }

        public Atom(string value)
        {
            Value = value;
        }
    }

    public class List : SExpr
    {
        public List<SExpr> Elements { get; }

        public List(List<SExpr> elements)
        {
            Elements = elements;
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



    public class UserFunction : SExpr
    {
        public List<string> Parameters { get; set; }
        public SExpr Body { get; set; }
        public Dictionary<string, SExpr> Environment { get; set; }

        public UserFunction()
        {
            Parameters = new List<string>();
            Environment = new Dictionary<string, SExpr>();
            Body = SExpr.Nil; // Initialize Body to avoid nullability warnings
        }
    }

    private class NilType : SExpr
    {
        public override string ToString()
        {
            return "nil";
        }
    }

    private class TruthType : SExpr
    {
        public override string ToString()
        {
            return "#t";
        }
    }

    public static Atom CreateAtom(string value)
    {
        return new Atom(value);
    }
}
