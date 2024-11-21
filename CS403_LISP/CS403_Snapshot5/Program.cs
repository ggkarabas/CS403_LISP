using System;

public class Program
{

    public static void Main()
    {
        Console.WriteLine("Running tests...");

        TestParser();
        TestPrinter();
        TestSprint2();
        TestSprint3();
        TestSprint4();
        TestSprint5();

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

        Console.WriteLine(SExpr.Nil);   // should print nil
        Console.WriteLine(SExpr.Truth); // should print #t

        var symbolAtom = SExpr.CreateAtom("symbol");
        Console.WriteLine(SExprPrinter.Print(symbolAtom));  // should print symbol

        var numberAtom = SExpr.CreateAtom("411");
        Console.WriteLine(SExprPrinter.Print(numberAtom));  // should print 411

        var consCell = new SExpr.ConsCell(
            SExpr.CreateAtom("one"),
            new SExpr.ConsCell(
                SExpr.CreateAtom("two"),
                new SExpr.ConsCell(SExpr.CreateAtom("three"), SExpr.Nil)
            )
        );
        Console.WriteLine(SExprPrinter.Print(consCell));  // should print (one two three)
    }

        public static void TestSprint3()
    {

    Console.WriteLine(SExprPrinter.Print(SExprUtils.Add(SExpr.CreateAtom("2"), SExpr.CreateAtom("3"))));  // 5
    Console.WriteLine(SExprPrinter.Print(SExprUtils.Sub(SExpr.CreateAtom("3"), SExpr.CreateAtom("2"))));  // 1
    Console.WriteLine(SExprPrinter.Print(SExprUtils.Mul(SExpr.CreateAtom("2"), SExpr.CreateAtom("3"))));  // 6
    Console.WriteLine(SExprPrinter.Print(SExprUtils.Div(SExpr.CreateAtom("6"), SExpr.CreateAtom("3"))));  // 2
    Console.WriteLine(SExprPrinter.Print(SExprUtils.Mod(SExpr.CreateAtom("5"), SExpr.CreateAtom("3"))));  // 2


    Console.WriteLine(SExprPrinter.Print(SExprUtils.Lt(SExpr.CreateAtom("2"), SExpr.CreateAtom("3"))));   // #t
    Console.WriteLine(SExprPrinter.Print(SExprUtils.Gt(SExpr.CreateAtom("3"), SExpr.CreateAtom("2"))));   // #t
    Console.WriteLine(SExprPrinter.Print(SExprUtils.Lte(SExpr.CreateAtom("2"), SExpr.CreateAtom("2"))));  // #t
    Console.WriteLine(SExprPrinter.Print(SExprUtils.Gte(SExpr.CreateAtom("3"), SExpr.CreateAtom("2"))));  // #t

    // Equality Test
    Console.WriteLine(SExprPrinter.Print(SExprUtils.Eq(SExpr.CreateAtom("2"), SExpr.CreateAtom("2"))));   // #t
    Console.WriteLine(SExprPrinter.Print(SExprUtils.Eq(SExpr.CreateAtom("2"), SExpr.CreateAtom("3"))));   // nil

    // Logic Test
    Console.WriteLine(SExprPrinter.Print(SExprUtils.Not(SExpr.Nil)));  // #t
    Console.WriteLine(SExprPrinter.Print(SExprUtils.Not(SExpr.Truth)));  // nil
    }

    public static void TestSprint4()
    {
    // Test nil and numbers
    Console.WriteLine(SExprPrinter.Print(SExprEvaluator.Eval(SExpr.Nil)));  // should print nil
    Console.WriteLine(SExprPrinter.Print(SExprEvaluator.Eval(SExpr.CreateAtom("42"))));  // should print 42


    SExprEvaluator.Set(new SExpr.Atom("x"), SExpr.CreateAtom("10"));
    Console.WriteLine(SExprPrinter.Print(SExprEvaluator.Eval(new SExpr.Atom("x"))));  // should print 10


    var quoteExpr = new SExpr.List(new System.Collections.Generic.List<SExpr>
    {
        new SExpr.Atom("quote"),
        new SExpr.Atom("hello")
    });
    Console.WriteLine(SExprPrinter.Print(SExprEvaluator.Eval(quoteExpr)));  // should print hello


    var setExpr = new SExpr.List(new System.Collections.Generic.List<SExpr>
    {
        new SExpr.Atom("set"),
        new SExpr.Atom("y"),
        new SExpr.Atom("5")
    });
    Console.WriteLine(SExprPrinter.Print(SExprEvaluator.Eval(setExpr)));  // Should print 5
    Console.WriteLine(SExprPrinter.Print(SExprEvaluator.Eval(new SExpr.Atom("y"))));  // Should print 5

    // Test add
    var addExpr = new SExpr.List(new System.Collections.Generic.List<SExpr>
    {
        new SExpr.Atom("add"),
        new SExpr.Atom("3"),
        new SExpr.Atom("4")
    });
    Console.WriteLine(SExprPrinter.Print(SExprEvaluator.Eval(addExpr)));  // should print 7
    }

    public static void TestSprint5()
{

    var andExpr = new SExpr.List(new System.Collections.Generic.List<SExpr>
    {
        new SExpr.Atom("and"),
        new SExpr.Atom("1"),
        new SExpr.Atom("nil")
    });
    Console.WriteLine(SExprPrinter.Print(SExprEvaluator.Eval(andExpr)));  // should print nil

    var andExpr2 = new SExpr.List(new System.Collections.Generic.List<SExpr>
    {
        new SExpr.Atom("and"),
        new SExpr.Atom("1"),
        new SExpr.Atom("2")
    });
    Console.WriteLine(SExprPrinter.Print(SExprEvaluator.Eval(andExpr2)));  // should print 2


    var orExpr = new SExpr.List(new System.Collections.Generic.List<SExpr>
    {
        new SExpr.Atom("or"),
        new SExpr.Atom("nil"),
        new SExpr.Atom("2")
    });
    Console.WriteLine(SExprPrinter.Print(SExprEvaluator.Eval(orExpr)));  // should print #t

    var orExpr2 = new SExpr.List(new System.Collections.Generic.List<SExpr>
    {
        new SExpr.Atom("or"),
        new SExpr.Atom("1"),
        new SExpr.Atom("2")
    });
    Console.WriteLine(SExprPrinter.Print(SExprEvaluator.Eval(orExpr2)));  // should print #t


    var ifExpr = new SExpr.List(new System.Collections.Generic.List<SExpr>
    {
        new SExpr.Atom("if"),
        new SExpr.Atom("1"),
        new SExpr.Atom("2"),
        new SExpr.Atom("3")
    });
    Console.WriteLine(SExprPrinter.Print(SExprEvaluator.Eval(ifExpr)));  // should print 2

    var ifExpr2 = new SExpr.List(new System.Collections.Generic.List<SExpr>
    {
        new SExpr.Atom("if"),
        new SExpr.Atom("nil"),
        new SExpr.Atom("2"),
        new SExpr.Atom("3")
    });
    Console.WriteLine(SExprPrinter.Print(SExprEvaluator.Eval(ifExpr2)));  // should print 3


    var condExpr = new SExpr.List(new System.Collections.Generic.List<SExpr>
    {
        new SExpr.Atom("cond"),
        new SExpr.List(new System.Collections.Generic.List<SExpr> { new SExpr.Atom("nil"), new SExpr.Atom("1") }),
        new SExpr.List(new System.Collections.Generic.List<SExpr> { new SExpr.Atom("1"), new SExpr.Atom("2") })
    });
    Console.WriteLine(SExprPrinter.Print(SExprEvaluator.Eval(condExpr)));  // should print 2
}



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
