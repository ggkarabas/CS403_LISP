using System;
using System.Collections.Generic;
using System.Text;

public static class SExprParser
{
    public static SExpr Parse(string input)
    {
        Queue<string> tokens = Tokenize(input);
        return ParseTokens(tokens);
    }

    private static Queue<string> Tokenize(string input)
    {
        Queue<string> tokens = new Queue<string>();
        int i = 0;
        while (i < input.Length)
        {
            if (char.IsWhiteSpace(input[i]))
            {
                i++;
            }
            else if (input[i] == '(' || input[i] == ')')
            {
                tokens.Enqueue(input[i].ToString());
                i++;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                while (i < input.Length && !char.IsWhiteSpace(input[i]) && input[i] != '(' && input[i] != ')')
                {
                    sb.Append(input[i]);
                    i++;
                }
                tokens.Enqueue(sb.ToString());
            }
        }
        return tokens;
    }

    private static SExpr ParseTokens(Queue<string> tokens)
    {
        if (tokens.Count == 0) throw new ArgumentException("Unexpected end of input");

        string token = tokens.Dequeue();
        if (token == "(")
        {
            List<SExpr> elements = new List<SExpr>();
            while (tokens.Peek() != ")")
            {
                elements.Add(ParseTokens(tokens));
            }
            tokens.Dequeue();  
            return new SExpr.List(elements);
        }
        else if (token == ")")
        {
            throw new ArgumentException("Unexpected closing parenthesis");
        }
        else
        {
            return new SExpr.Atom(token);
        }
    }
}
