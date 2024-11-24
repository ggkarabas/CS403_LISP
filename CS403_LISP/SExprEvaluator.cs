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
        "lt", "gt", "lte", "gte", "eq", "not", "and", "or"
    };


    private static readonly Dictionary<string, string> SymbolicOperatorAliases = new Dictionary<string, string>
    {
        { "+", "add" },
        { "-", "sub" },
        { "*", "mul" },
        { "/", "div" }
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
            else
            {
                return Lookup(expr, env);
            }
        }

        if (SExprUtils.IsList(expr))
        {
            var list = expr as SExpr.List;
            var firstElem = list.Elements[0];

            if (firstElem is SExpr.Atom opAtom)
            {
                // Resolve symbolic aliases
                var op = SymbolicOperatorAliases.ContainsKey(opAtom.Value)
                    ? SymbolicOperatorAliases[opAtom.Value]
                    : opAtom.Value;

                switch (op)
                {

                    case "and":
                        foreach (var arg in list.Elements.Skip(1))
                        {
                            var result = Eval(arg, env);
                            if (result == SExpr.Nil)
                            {
                                return SExpr.Nil;
                            }
                        }
                        return SExpr.Truth;

                    case "or":
                        foreach (var arg in list.Elements.Skip(1))
                        {
                            var result = Eval(arg, env);
                            if (result != SExpr.Nil)
                            {
                                return SExpr.Truth;
                            }
                        }
                        return SExpr.Nil;


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
                        if (funcName == null || paramElements == null)
                            throw new ArgumentException("Invalid function definition");

                        var parameters = new List<string>();
                        foreach (var param in paramElements)
                        {
                            if (param is SExpr.Atom paramAtom)
                            {
                                parameters.Add(paramAtom.Value);
                            }
                            else
                            {
                                throw new ArgumentException("Function parameters must be atoms");
                            }
                        }
                        var body = list.Elements[3];

                        var function = new SExpr.UserFunction
                        {
                            Parameters = parameters,
                            Body = body,
                            Environment = new Dictionary<string, SExpr>(env)
                        };

                        function.Environment[funcName] = function;

                        env[funcName] = function;
                        return new SExpr.Atom(funcName);

                    case "if":
                        var condition = Eval(list.Elements[1], env);
                        if (SExprUtils.ToBoolean(condition))
                        {
                            return Eval(list.Elements[2], env);
                        }
                        else
                        {
                            return Eval(list.Elements[3], env);
                        }

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
            else
            {
                throw new ArgumentException("Invalid function call");
            }
        }

        throw new ArgumentException("Undefined behavior");
    }

    private static bool IsBuiltInFunction(string op)
    {
        return BuiltInFunctions.Contains(op);
    }

    private static SExpr ApplyBuiltInFunction(string op, List<SExpr> args)
    {
        switch (op)
        {
            case "add":
                return SExprUtils.Add(args[0], args[1]);
            case "sub":
                return SExprUtils.Sub(args[0], args[1]);
            case "mul":
                return SExprUtils.Mul(args[0], args[1]);
            case "div":
                return SExprUtils.Div(args[0], args[1]);
            case "mod":
                return SExprUtils.Mod(args[0], args[1]);
            case "lt":
                return SExprUtils.Lt(args[0], args[1]);
            case "gt":
                return SExprUtils.Gt(args[0], args[1]);
            case "lte":
                return SExprUtils.Lte(args[0], args[1]);
            case "gte":
                return SExprUtils.Gte(args[0], args[1]);
            case "eq":
                return SExprUtils.Eq(args[0], args[1]);
            case "not":
                return SExprUtils.Not(args[0]);
            default:
                throw new ArgumentException($"Unknown built-in function: {op}");
        }
    }

    public static SExpr EvalUserFunction(SExpr.UserFunction func, List<SExpr> args)
    {
        if (func.Parameters.Count != args.Count)
            throw new ArgumentException("Argument count mismatch");

        var localEnv = new Dictionary<string, SExpr>(func.Environment);

        for (int i = 0; i < func.Parameters.Count; i++)
        {
            localEnv[func.Parameters[i]] = args[i];
        }

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
