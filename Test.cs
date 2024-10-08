using System;

public class Test
{
    public static void Main()
    {
        // Run your test cases here
        TestParser();
        TestPrinter();
    }

    // Function to test the SExprParser
    private static void TestParser()
    {
        Console.WriteLine("Running Parser Tests...");

        // Test Case 1: Single Atom
        string input1 = "42";
        var result1 = SExprParser.Parse(input1);
        Check(result1 is SExpr.Atom && ((SExpr.Atom)result1).Value == "42", "Test Case 1: Parse Single Atom");

        // Test Case 2: Simple List
        string input2 = "(+ 1 2 3)";
        var result2 = SExprParser.Parse(input2);
        Check(result2 is SExpr.List && ((SExpr.List)result2).Elements.Count == 4, "Test Case 2: Parse Simple List");

        // Test Case 3: Nested List
        string input3 = "(define x (10 20))";
        var result3 = SExprParser.Parse(input3);
        Check(result3 is SExpr.List && ((SExpr.List)result3).Elements.Count == 3, "Test Case 3: Parse Nested List");

        Console.WriteLine("Parser Tests Completed.\n");
    }

    // Function to test the SExprPrinter
    private static void TestPrinter()
    {
        Console.WriteLine("Running Printer Tests...");

        // Test Case 1: Atom
        var expr1 = new SExpr.Atom("42");
        Check(SExprPrinter.Print(expr1) == "42", "Test Case 1: Print Single Atom");

        // Test Case 2: Simple List
        var expr2 = new SExpr.List(new System.Collections.Generic.List<SExpr>
        {
            new SExpr.Atom("+"),
            new SExpr.Atom("1"),
            new SExpr.Atom("2"),
            new SExpr.Atom("3")
        });
        Check(SExprPrinter.Print(expr2) == "(+ 1 2 3)", "Test Case 2: Print Simple List");

        // Test Case 3: Nested List
        var expr3 = new SExpr.List(new System.Collections.Generic.List<SExpr>
        {
            new SExpr.Atom("define"),
            new SExpr.Atom("x"),
            new SExpr.List(new System.Collections.Generic.List<SExpr>
            {
                new SExpr.Atom("10"),
                new SExpr.Atom("20")
            })
        });
        Check(SExprPrinter.Print(expr3) == "(define x (10 20))", "Test Case 3: Print Nested List");

        Console.WriteLine("Printer Tests Completed.\n");
    }

    // Helper function to check and display results
    private static void Check(bool condition, string testName)
    {
        if (condition)
        {
            Console.WriteLine($"[PASS] {testName}");
        }
        else
        {
            Console.WriteLine($"[FAIL] {testName}");
        }
    }
}
