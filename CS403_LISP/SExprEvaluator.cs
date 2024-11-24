// Implements the evaluation logic for S-Expressions:

using System;
using System.Collections.Generic;
using System.Linq;

public static class SExprEvaluator
{
    private static Dictionary<string, SExpr> GlobalEnvironment = new Dictionary<string, SExpr>();

    private static readonly HashSet<string> BuiltInFunctions = new HashSet<string>
    {
        "add", "sub", "mul", "div", "mod",
        "lt", "gt", "lte", "gte", "eq", "not",
        "cons", "car", "cdr", "quote", "set", "define", "fn", "and", "or", "if", "cond"
    };

    private static readonly Dictionary<string, string> SymbolicOperatorAliases = new Dictionary<string, string>
    {
        { "+", "add" },
        { "-", "sub" },
        { "*", "mul" },
        { "/", "div" },
        { ">", "gt" },
        { "<", "lt" },
        { "<=", "lte" },
        { ">=", "gte" },
        
    };

    public static SExpr Eval(SExpr expr, Dictionary<string, SExpr>? env = null)
    {
        env = env ?? GlobalEnvironment;

        if (expr == SExpr.Nil || expr == SExpr.Truth || SExprUtils.IsNumber(expr))
        {
            return expr;
        }

        if (expr is SExpr.Atom atom)
        {
            if (atom.Value == "nil")
            {
                return SExpr.Nil;
            }
            else if (atom.Value == "#t")
            {
                return SExpr.Truth;
            }
            else if (env.ContainsKey(atom.Value))
            {
                return env[atom.Value];
            }
            else
            {
                throw new ArgumentException($"Undefined symbol: {atom.Value}");
            }
        }

        if (SExprUtils.IsList(expr))
        {
            var list = expr as SExpr.List;

            // Treat lists as data if the first element is not a symbol or a valid function/operator
            if (list.Elements.Count == 0 || !(list.Elements[0] is SExpr.Atom opAtom) || SExprUtils.IsNumber(opAtom))
            {
                return new SExpr.List(list.Elements.Select(element => Eval(element, env)).ToList());
            }


            // Handle function calls and special forms
            var op = SymbolicOperatorAliases.ContainsKey(opAtom.Value)
                ? SymbolicOperatorAliases[opAtom.Value]
                : opAtom.Value;

            switch (op)
            {
                case "quote":
                    return list.Elements[1];

                case "define":
                    var varName = (list.Elements[1] as SExpr.Atom)?.Value;
                    if (string.IsNullOrEmpty(varName))
                        throw new ArgumentException("Invalid variable name in define");

                    var varValue = Eval(list.Elements[2], env);
                    env[varName] = varValue;
                    return varValue;

                case "set":
                    var name = (list.Elements[1] as SExpr.Atom)?.Value;
                    if (string.IsNullOrEmpty(name))
                        throw new ArgumentException("Invalid variable name in set");

                    var value = Eval(list.Elements[2], env);
                    env[name] = value;
                    return value;
                case "fn":
                    var funcName = (list.Elements[1] as SExpr.Atom)?.Value;
                    var paramElements = (list.Elements[2] as SExpr.List)?.Elements;

                    if (string.IsNullOrEmpty(funcName) || paramElements == null)
                        throw new ArgumentException("Invalid function definition");

                    var parameters = paramElements.Select(param => (param as SExpr.Atom)?.Value ?? throw new ArgumentException("Function parameters must be atoms")).ToList();

                    var body = list.Elements[3];
                    var function = new SExpr.UserFunction
                    {
                        Parameters = parameters,
                        Body = body,
                        Environment = new Dictionary<string, SExpr>(env)
                    };

                    // Add the function itself to its environment for recursive calls
                    function.Environment[funcName] = function;

                    // Store the function in the global environment
                    env[funcName] = function;

                    return new SExpr.Atom(funcName);

                case "and":
                    SExpr lastAndResult = SExpr.Truth;
                    foreach (var arg in list.Elements.Skip(1))
                    {
                        lastAndResult = Eval(arg, env);
                        if (lastAndResult == SExpr.Nil)
                        {
                            return SExpr.Nil;
                        }
                    }
                    return lastAndResult; // Return the last truthy value if all are true


                case "or":
                    foreach (var arg in list.Elements.Skip(1))
                    {
                        var result = Eval(arg, env);
                        if (result != SExpr.Nil)
                        {
                            return result;
                        }
                    }
                    return SExpr.Nil;

                case "if":
                    var condition = Eval(list.Elements[1], env);
                    return SExprUtils.ToBoolean(condition)
                        ? Eval(list.Elements[2], env)
                        : Eval(list.Elements[3], env);

                    
                case "eval":
                    var evalArg = Eval(list.Elements[1], env);
                    return Eval(evalArg, env);
                    

                case "cond":
                    foreach (var clause in list.Elements.Skip(1))
                    {
                        if (clause is SExpr.List clauseList && clauseList.Elements.Count == 2)
                        {
                            var testExpr = clauseList.Elements[0];
                            var resultExpr = clauseList.Elements[1];

                            if (SExprUtils.ToBoolean(Eval(testExpr, env)))
                            {
                                return Eval(resultExpr, env);
                            }
                        }
                        else
                        {
                            throw new ArgumentException("Invalid cond clause");
                        }
                    }
                    return SExpr.Nil;

                case "cons":
                    var carPart = Eval(list.Elements[1], env);
                    var cdrPart = Eval(list.Elements[2], env);
                    return new SExpr.ConsCell(carPart, cdrPart);

                case "car":
                    var carArg = Eval(list.Elements[1], env);
                    if (carArg is SExpr.List carList && carList.Elements.Count > 0)
                    {
                        return carList.Elements[0];
                    }
                    else if (carArg is SExpr.ConsCell consCar)
                    {
                        return consCar.Car;
                    }
                    throw new ArgumentException("car requires a non-empty list or cons cell");

                case "cdr":
                    var cdrArg = Eval(list.Elements[1], env);
                    if (cdrArg is SExpr.List cdrList && cdrList.Elements.Count > 0)
                    {
                        return new SExpr.List(cdrList.Elements.Skip(1).ToList());
                    }
                    else if (cdrArg is SExpr.ConsCell consCdr)
                    {
                        return consCdr.Cdr;
                    }
                    throw new ArgumentException("cdr requires a list or cons cell");

                default:
                    if (IsBuiltInFunction(op))
                    {
                        var args = list.Elements.Skip(1).Select(arg => Eval(arg, env)).ToList();
                        return ApplyBuiltInFunction(op, args);
                    }
                    else if (env.ContainsKey(op))
                    {
                        var func = env[op];
                        if (func is SExpr.UserFunction userFunc)
                        {
                            var args = list.Elements.Skip(1).Select(arg => Eval(arg, env)).ToList();
                            return EvalUserFunction(userFunc, args);
                        }
                        else
                        {
                            throw new ArgumentException($"Symbol {op} is not a function");
                        }
                    }
                    else
                    {
                        throw new ArgumentException($"Undefined or unsupported function: {op}");
                    }
            }
        }

        throw new ArgumentException("Invalid function call");
    }

    private static bool IsBuiltInFunction(string op)
    {
        return BuiltInFunctions.Contains(op);
    }

    private static SExpr ApplyBuiltInFunction(string op, List<SExpr> args)
    {
        switch (op)
        {
            case "add": return SExprUtils.Add(args[0], args[1]);
            case "sub": return SExprUtils.Sub(args[0], args[1]);
            case "mul": return SExprUtils.Mul(args[0], args[1]);
            case "div": return SExprUtils.Div(args[0], args[1]);
            case "mod": return SExprUtils.Mod(args[0], args[1]);
            case "lt": return SExprUtils.Lt(args[0], args[1]);
            case "gt": return SExprUtils.Gt(args[0], args[1]);
            case "lte": return SExprUtils.Lte(args[0], args[1]);
            case "gte": return SExprUtils.Gte(args[0], args[1]);
            case "eq": return SExprUtils.Eq(args[0], args[1]);
            case "not": return SExprUtils.Not(args[0]);
            default:
                throw new ArgumentException($"Unknown built-in function: {op}");
        }
    }

    public static SExpr EvalUserFunction(SExpr.UserFunction func, List<SExpr> args)
    {
        if (func.Parameters.Count != args.Count)
            throw new ArgumentException($"Function {func} expected {func.Parameters.Count} arguments but got {args.Count}");

        // Create a local environment for the function
        var localEnv = new Dictionary<string, SExpr>(func.Environment);

        // Map arguments to parameters in the local environment
        for (int i = 0; i < func.Parameters.Count; i++)
        {
            localEnv[func.Parameters[i]] = args[i];
        }

        // Evaluate the function body in the local environment
        return Eval(func.Body, localEnv);
    }



    public static SExpr Lookup(SExpr expr, Dictionary<string, SExpr> env)
    {
        if (expr is SExpr.Atom atom)
        {
            var name = atom!.Value;
            if (env.ContainsKey(name))
            {
                return env[name];
            }
            else
            {
                throw new ArgumentException($"Undefined symbol: {name}");
            }
        }
        else
        {
            throw new ArgumentException("Invalid symbol");
        }
    }

    public static void Set(string name, SExpr value)
    {
        GlobalEnvironment[name] = value;
    }
}
