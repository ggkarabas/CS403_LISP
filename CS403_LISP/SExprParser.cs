// Implements an S-Expression parser:

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
        int length = input.Length;

        while (i < length)
        {
            if (char.IsWhiteSpace(input[i]))
            {
                i++;
            }
            else if (input[i] == ';')
            {
                while (i < length && input[i] != '\n')
                {
                    i++;
                }
            }
            else if (input[i] == '(' || input[i] == ')')
            {
                tokens.Enqueue(input[i].ToString());
                i++;
            }
            else if (input[i] == '"')
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(input[i]); // Append the opening quote
                i++;
                while (i < length)
                {
                    if (input[i] == '"')
                    {
                        sb.Append(input[i]); // Append the closing quote
                        i++;
                        break;
                    }
                    else if (input[i] == '\\' && i + 1 < length)
                    {
                        // Handle escape sequences
                        sb.Append(input[i]);
                        i++;
                        sb.Append(input[i]);
                        i++;
                    }
                    else
                    {
                        sb.Append(input[i]);
                        i++;
                    }
                }
                tokens.Enqueue(sb.ToString());
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                while (i < length && !char.IsWhiteSpace(input[i]) && input[i] != '(' && input[i] != ')' && input[i] != ';')
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
            while (tokens.Count > 0 && tokens.Peek() != ")")
            {
                elements.Add(ParseTokens(tokens));
            }
            if (tokens.Count == 0)
            {
                throw new ArgumentException("Missing closing parenthesis");
            }
            tokens.Dequeue();  // Remove the ')'
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
