// implements the evaluation logic for SEXPR:

public static class SExprEvaluator
{
    private static List<SExpr> symbolList = new List<SExpr>(); 
    private static List<SExpr> valueList = new List<SExpr>(); 

    // lists for user-defined functions
    private static List<SExpr> functionList = new List<SExpr>();  
    private static List<SExpr> functionArgsList = new List<SExpr>(); 
    private static List<SExpr> functionBodyList = new List<SExpr>(); 

    private static Stack<(List<SExpr> symbols, List<SExpr> values)> envStack = new Stack<(List<SExpr>, List<SExpr>)>();

    public static SExpr Eval(SExpr expr)
    {
        if (expr == SExpr.Nil || SExprUtils.IsNumber(expr))
        {
            return expr;
        }

        if (SExprUtils.IsSymbol(expr))
        {
            return Lookup(expr);
        }

        if (SExprUtils.IsList(expr))
        {
            var list = expr as SExpr.List;
            var function = Eval(list.Elements[0]);

            // Handle "quote"
            if (SExprUtils.Eq(function, new SExpr.Atom("quote")) == SExpr.Truth)
            {
                return list.Elements[1];  
            }

            // Handle "set"
            if (SExprUtils.Eq(function, new SExpr.Atom("set")) == SExpr.Truth)
            {
                var name = list.Elements[1];
                var value = Eval(list.Elements[2]);
                Set(name, value);
                return value;
            }

            if (SExprUtils.Eq(function, new SExpr.Atom("fn")) == SExpr.Truth)
            {
                var fname = list.Elements[1];  
                var args = list.Elements[2];   
                var body = list.Elements[3];  

                SetFunction(fname, args, body);
                return fname;  // Return the function name as confirmation
            }

            if (SExprUtils.Eq(function, new SExpr.Atom("add")) == SExpr.Truth)
            {
                return SExprUtils.Add(Eval(list.Elements[1]), Eval(list.Elements[2]));
            }
            if (SExprUtils.Eq(function, new SExpr.Atom("sub")) == SExpr.Truth)
            {
                return SExprUtils.Sub(Eval(list.Elements[1]), Eval(list.Elements[2]));
            }
            if (SExprUtils.Eq(function, new SExpr.Atom("mul")) == SExpr.Truth)
            {
                return SExprUtils.Mul(Eval(list.Elements[1]), Eval(list.Elements[2]));
            }
            if (SExprUtils.Eq(function, new SExpr.Atom("div")) == SExpr.Truth)
            {
                return SExprUtils.Div(Eval(list.Elements[1]), Eval(list.Elements[2]));
            }
            if (SExprUtils.Eq(function, new SExpr.Atom("mod")) == SExpr.Truth)
            {
                return SExprUtils.Mod(Eval(list.Elements[1]), Eval(list.Elements[2]));
            }

            if (SExprUtils.Eq(function, new SExpr.Atom("and")) == SExpr.Truth)
            {
                return Eval(list.Elements[1]) == SExpr.Nil ? SExpr.Nil : Eval(list.Elements[2]);
            }
            if (SExprUtils.Eq(function, new SExpr.Atom("or")) == SExpr.Truth)
            {
                return Eval(list.Elements[1]) != SExpr.Nil ? SExpr.Truth : Eval(list.Elements[2]);
            }
            if (SExprUtils.Eq(function, new SExpr.Atom("not")) == SExpr.Truth)
            {
                return SExprUtils.Not(Eval(list.Elements[1]));
            }

            // Handle "if"
            if (SExprUtils.Eq(function, new SExpr.Atom("if")) == SExpr.Truth)
            {
                var condition = Eval(list.Elements[1]);
                if (condition != SExpr.Nil)
                {
                    return Eval(list.Elements[2]);
                }
                else
                {
                    return Eval(list.Elements[3]);
                }
            }

            // Handle "cond"
            if (SExprUtils.Eq(function, new SExpr.Atom("cond")) == SExpr.Truth)
            {
                for (int i = 1; i < list.Elements.Count; i++)
                {
                    var clause = list.Elements[i] as SExpr.List;
                    var condition = Eval(clause.Elements[0]);
                    if (condition != SExpr.Nil)
                    {
                        return Eval(clause.Elements[1]);
                    }
                }
                return SExpr.Nil;
            }

            if (IsFunction(function))
            {
                return EvalUserFunction(function, list.Elements.Skip(1).ToList());
            }
        }

        throw new ArgumentException("Undefined behavior");
    }

    // Lookup for variables and function arguments
    public static SExpr Lookup(SExpr symbol)
    {
        if (envStack.Count > 0)
        {
            var (symbols, values) = envStack.Peek();
            for (int i = 0; i < symbols.Count; i++)
            {
                if (SExprUtils.Eq(symbols[i], symbol) == SExpr.Truth)
                {
                    return values[i];
                }
            }
        }

        for (int i = 0; i < symbolList.Count; i++)
        {
            if (SExprUtils.Eq(symbolList[i], symbol) == SExpr.Truth)
            {
                return valueList[i];
            }
        }

        return symbol;  
    }

    public static void Set(SExpr symbol, SExpr value)
    {
        symbolList.Add(symbol);
        valueList.Add(value);
    }

    public static void SetFunction(SExpr fname, SExpr args, SExpr body)
    {
        functionList.Add(fname);
        functionArgsList.Add(args);
        functionBodyList.Add(body);
    }

    public static bool IsFunction(SExpr symbol)
    {
        return functionList.Any(f => SExprUtils.Eq(f, symbol) == SExpr.Truth);
    }

    // Evaluate a user defined function call
    public static SExpr EvalUserFunction(SExpr function, List<SExpr> actualArgs)
    {
        var index = functionList.FindIndex(f => SExprUtils.Eq(f, function) == SExpr.Truth);
        var formalArgs = functionArgsList[index] as SExpr.List;
        var body = functionBodyList[index];

        var values = actualArgs.Select(arg => Eval(arg)).ToList();

        envStack.Push((formalArgs.Elements, values));

        var result = Eval(body);

        envStack.Pop();

        return result;
    }
}
