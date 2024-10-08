using System;

public class Program
{
    public static void Repl()
    {
        Console.WriteLine("S-Expression REPL. Enter an S-Expression:");
        while (true)
        {
            Console.Write("> ");
            string input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) continue;

            try
            {
                SExpr expr = SExprParser.Parse(input);
                SExpr result = SExprEvaluator.Eval(expr);
                string output = SExprPrinter.Print(result);
                Console.WriteLine(output);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }
    }

    public static void Main()
    {
        Repl();
    }
}
