using System;

public class Program
{
    public static void Main()
    {
        Console.WriteLine("Running tests...");

        // Run tests for Parser
        TestParser();

        // Run tests for Printer
        TestPrinter();

        // Run tests for Sprint 2
        TestSprint2();

        Console.WriteLine("All tests completed.");
    }

    private static void TestParser()
    {
        Console.WriteLine("Testing Parser...");

        // Test 1: Single Atom
        string input1 = "42";
        var result1 = SExprParser.Parse(input1);
        Check(result1 is SExpr.Atom && ((SExpr.Atom)result1).Value == "42", "Test 1: Parse single atom");

        // Test 2: Simple List
        string input2 = "(+ 1 2 3)";
        var result2 = SExprParser.Parse(input2);
        Check(result2 is SExpr.List && ((SExpr.List)result2).Elements.Count == 4, "Test 2: Parse simple list");

        // Test 3: Nested List
        string input3 = "(define x (10 20))";
        var result3 = SExprParser.Parse(input3);
        var list3 = result3 as SExpr.List;
        Check(list3 != null && list3.Elements.Count == 3, "Test 3: Parse nested list");

        Console.WriteLine("Parser Tests Completed.\n");
    }

    private static void TestPrinter()
    {
        Console.WriteLine("Testing Printer...");

        // Test 1: Print single atom
        var atom1 = new SExpr.Atom("42");
        Check(SExprPrinter.Print(atom1) == "42", "Test 1: Print single atom");

        // Test 2: Print simple list
        var simpleList = new SExpr.List(new System.Collections.Generic.List<SExpr>
        {
            new SExpr.Atom("+"),
            new SExpr.Atom("1"),
            new SExpr.Atom("2"),
            new SExpr.Atom("3")
        });
        Check(SExprPrinter.Print(simpleList) == "(+ 1 2 3)", "Test 2: Print simple list");

        // Test 3: Print nested list
        var nestedList = new SExpr.List(new System.Collections.Generic.List<SExpr>
        {
            new SExpr.Atom("define"),
            new SExpr.Atom("x"),
            new SExpr.List(new System.Collections.Generic.List<SExpr>
            {
                new SExpr.Atom("10"),
                new SExpr.Atom("20")
            })
        });
        Check(SExprPrinter.Print(nestedList) == "(define x (10 20))", "Test 3: Print nested list");

        Console.WriteLine("Printer Tests Completed.\n");
    }

    private static void TestSprint2()
    {
        // Test Nil and Truth
        Console.WriteLine(SExpr.Nil);   // should print nil
        Console.WriteLine(SExpr.Truth); // should print #t

        // Test Atom (Symbol and Number)
        var symbolAtom = SExpr.CreateAtom("symbol");
        Console.WriteLine(SExprPrinter.Print(symbolAtom));  // should print symbol

        var numberAtom = SExpr.CreateAtom("411");
        Console.WriteLine(SExprPrinter.Print(numberAtom));  // should print 411

        // Test Cons Cells
        var consCell = new SExpr.ConsCell(
            SExpr.CreateAtom("one"),
            new SExpr.ConsCell(
                SExpr.CreateAtom("two"),
                new SExpr.ConsCell(SExpr.CreateAtom("three"), SExpr.Nil)
            )
        );
        Console.WriteLine(SExprPrinter.Print(consCell));  // should print (one two three)
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
