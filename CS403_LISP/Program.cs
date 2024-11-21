// Contains testing functions for the code:

using System;

public class Program
{
    public static void Main()
    {
        Console.WriteLine("Running tests...\n");

        TestParser();
        TestPrinter();
        TestSprint2();
        TestSprint3();
        TestSprint4();
        TestSprint5();
        TestUserDefinedFunctions();

        Console.WriteLine("All tests completed.");
    }

    private static void TestParser()
    {
        Console.WriteLine("Testing Parser...\n");

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

        // Additional Tests

        // Test 4: Empty List
        string input4 = "()";
        var result4 = SExprParser.Parse(input4);
        Check(result4 is SExpr.List && ((SExpr.List)result4).Elements.Count == 0, "Test 4: Parse empty list");

        // Test 5: Deeply Nested Lists
        string input5 = "(a (b (c (d))))";
        var result5 = SExprParser.Parse(input5);
        Check(result5 is SExpr.List && ((SExpr.List)result5).Elements.Count == 2, "Test 5: Parse deeply nested lists");

        // Test 6: Multiple Lists
        string input6 = "(1 2) (3 4)";
        var result6 = SExprParser.Parse(input6);
        Check(result6 is SExpr.List && ((SExpr.List)result6).Elements.Count == 2, "Test 6: Parse multiple lists (should parse first list)");

        // Test 7: Atoms with special characters
        string input7 = "+-*/";
        var result7 = SExprParser.Parse(input7);
        Check(result7 is SExpr.Atom && ((SExpr.Atom)result7).Value == "+-*/", "Test 7: Parse atom with special characters");

        // Test 8: String Literals (if supported)
        string input8 = "\"Hello, World!\"";
        var result8 = SExprParser.Parse(input8);
        Check(result8 is SExpr.Atom && ((SExpr.Atom)result8).Value == "\"Hello, World!\"", "Test 8: Parse string literal");

        // Test 9: Incorrect Syntax
        string input9 = "(+ 1 2";
        try
        {
            var result9 = SExprParser.Parse(input9);
            Check(false, "Test 9: Should fail on incomplete input");
        }
        catch (Exception)
        {
            Check(true, "Test 9: Correctly failed on incomplete input");
        }

        // Test 10: Comments (if supported)
        string input10 = "(+ 1 2) ; this is a comment";
        var result10 = SExprParser.Parse(input10);
        Check(result10 is SExpr.List && ((SExpr.List)result10).Elements.Count == 3, "Test 10: Parse list with comment");

        Console.WriteLine("Parser Tests Completed.\n");
    }

    private static void TestPrinter()
    {
        Console.WriteLine("Testing Printer...\n");

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

        // Additional Tests

        // Test 4: Print empty list
        var emptyList = new SExpr.List(new System.Collections.Generic.List<SExpr>());
        Check(SExprPrinter.Print(emptyList) == "()", "Test 4: Print empty list");

        // Test 5: Print deeply nested lists
        var deepNestedList = new SExpr.List(new System.Collections.Generic.List<SExpr>
        {
            new SExpr.Atom("a"),
            new SExpr.List(new System.Collections.Generic.List<SExpr>
            {
                new SExpr.Atom("b"),
                new SExpr.List(new System.Collections.Generic.List<SExpr>
                {
                    new SExpr.Atom("c"),
                    new SExpr.List(new System.Collections.Generic.List<SExpr>
                    {
                        new SExpr.Atom("d")
                    })
                })
            })
        });
        Check(SExprPrinter.Print(deepNestedList) == "(a (b (c (d))))", "Test 5: Print deeply nested list");

        // Test 6: Print atoms with special characters
        var specialAtom = new SExpr.Atom("+-*/");
        Check(SExprPrinter.Print(specialAtom) == "+-*/", "Test 6: Print atom with special characters");

        // Test 7: Print string literals (if supported)
        var stringAtom = new SExpr.Atom("\"Hello, World!\"");
        Check(SExprPrinter.Print(stringAtom) == "\"Hello, World!\"", "Test 7: Print string literal");

        Console.WriteLine("Printer Tests Completed.\n");
    }

    private static void TestSprint2()
    {
        Console.WriteLine("Testing Sprint 2...\n");

        // Test Nil and Truth
        Check(SExprPrinter.Print(SExpr.Nil) == "nil", "Test 1: Print nil");
        Check(SExprPrinter.Print(SExpr.Truth) == "#t", "Test 2: Print truth");

        // Test Atom (Symbol and Number)
        var symbolAtom = SExpr.CreateAtom("symbol");
        Check(SExprPrinter.Print(symbolAtom) == "symbol", "Test 3: Print symbol atom");

        var numberAtom = SExpr.CreateAtom("411");
        Check(SExprPrinter.Print(numberAtom) == "411", "Test 4: Print number atom");

        // Test Cons Cells
        var consCell = new SExpr.ConsCell(
            SExpr.CreateAtom("one"),
            new SExpr.ConsCell(
                SExpr.CreateAtom("two"),
                new SExpr.ConsCell(SExpr.CreateAtom("three"), SExpr.Nil)
            )
        );
        Check(SExprPrinter.Print(consCell) == "(one two three)", "Test 5: Print cons cell");

        // Additional Tests

        // Test 6: Nested Cons Cells
        var nestedConsCell = new SExpr.ConsCell(
            SExpr.CreateAtom("a"),
            new SExpr.ConsCell(
                new SExpr.ConsCell(
                    SExpr.CreateAtom("b"),
                    SExpr.Nil
                ),
                SExpr.Nil
            )
        );
        Check(SExprPrinter.Print(nestedConsCell) == "(a (b))", "Test 6: Print nested cons cells");

        Console.WriteLine("Sprint 2 Tests Completed.\n");
    }

    private static void TestSprint3()
    {
        Console.WriteLine("Testing Sprint 3...\n");

        // Arithmetic Tests
        Check(SExprPrinter.Print(SExprUtils.Add(SExpr.CreateAtom("2"), SExpr.CreateAtom("3"))) == "5", "Test 1: Addition");
        Check(SExprPrinter.Print(SExprUtils.Sub(SExpr.CreateAtom("3"), SExpr.CreateAtom("2"))) == "1", "Test 2: Subtraction");
        Check(SExprPrinter.Print(SExprUtils.Mul(SExpr.CreateAtom("2"), SExpr.CreateAtom("3"))) == "6", "Test 3: Multiplication");
        Check(SExprPrinter.Print(SExprUtils.Div(SExpr.CreateAtom("6"), SExpr.CreateAtom("3"))) == "2", "Test 4: Division");
        Check(SExprPrinter.Print(SExprUtils.Mod(SExpr.CreateAtom("5"), SExpr.CreateAtom("3"))) == "2", "Test 5: Modulo");

        // Additional Arithmetic Tests
        Check(SExprPrinter.Print(SExprUtils.Add(SExpr.CreateAtom("-1"), SExpr.CreateAtom("1"))) == "0", "Test 6: Addition with negative numbers");
        Check(SExprPrinter.Print(SExprUtils.Div(SExpr.CreateAtom("1"), SExpr.CreateAtom("0"))) == "undefined", "Test 7: Division by zero");

        // Relational Tests
        Check(SExprPrinter.Print(SExprUtils.Lt(SExpr.CreateAtom("2"), SExpr.CreateAtom("3"))) == "#t", "Test 8: Less than");
        Check(SExprPrinter.Print(SExprUtils.Gt(SExpr.CreateAtom("3"), SExpr.CreateAtom("2"))) == "#t", "Test 9: Greater than");
        Check(SExprPrinter.Print(SExprUtils.Lte(SExpr.CreateAtom("2"), SExpr.CreateAtom("2"))) == "#t", "Test 10: Less than or equal");
        Check(SExprPrinter.Print(SExprUtils.Gte(SExpr.CreateAtom("3"), SExpr.CreateAtom("2"))) == "#t", "Test 11: Greater than or equal");

        // Equality Test
        Check(SExprPrinter.Print(SExprUtils.Eq(SExpr.CreateAtom("2"), SExpr.CreateAtom("2"))) == "#t", "Test 12: Equality true");
        Check(SExprPrinter.Print(SExprUtils.Eq(SExpr.CreateAtom("2"), SExpr.CreateAtom("3"))) == "nil", "Test 13: Equality false");

        // Logic Test
        Check(SExprPrinter.Print(SExprUtils.Not(SExpr.Nil)) == "#t", "Test 14: Not nil");
        Check(SExprPrinter.Print(SExprUtils.Not(SExpr.Truth)) == "nil", "Test 15: Not truth");

        Console.WriteLine("Sprint 3 Tests Completed.\n");
    }

    private static void TestSprint4()
    {
        Console.WriteLine("Testing Sprint 4...\n");

        // Test nil and numbers
        Check(SExprPrinter.Print(SExprEvaluator.Eval(SExpr.Nil)) == "nil", "Test 1: Eval nil");
        Check(SExprPrinter.Print(SExprEvaluator.Eval(SExpr.CreateAtom("42"))) == "42", "Test 2: Eval number");

        // Test symbols and set/lookup
        SExprEvaluator.Set("x", SExpr.CreateAtom("10"));
        Check(SExprPrinter.Print(SExprEvaluator.Eval(new SExpr.Atom("x"))) == "10", "Test 3: Eval symbol x");

        // Test quote
        var quoteExpr = new SExpr.List(new System.Collections.Generic.List<SExpr>
        {
            new SExpr.Atom("quote"),
            new SExpr.Atom("hello")
        });
        Check(SExprPrinter.Print(SExprEvaluator.Eval(quoteExpr)) == "hello", "Test 4: Eval quote");

        // Test set and lookup
        var setExpr = new SExpr.List(new System.Collections.Generic.List<SExpr>
        {
            new SExpr.Atom("set"),
            new SExpr.Atom("y"),
            new SExpr.Atom("5")
        });
        Check(SExprPrinter.Print(SExprEvaluator.Eval(setExpr)) == "5", "Test 5: Set y to 5");
        Check(SExprPrinter.Print(SExprEvaluator.Eval(new SExpr.Atom("y"))) == "5", "Test 6: Eval symbol y");

        // Test add
        var addExpr = new SExpr.List(new System.Collections.Generic.List<SExpr>
        {
            new SExpr.Atom("add"),
            new SExpr.Atom("3"),
            new SExpr.Atom("4")
        });
        Check(SExprPrinter.Print(SExprEvaluator.Eval(addExpr)) == "7", "Test 7: Eval add 3 4");

        // Additional Tests

        // Test undefined symbol
        try
        {
            var undefinedSymbol = new SExpr.Atom("z");
            SExprEvaluator.Eval(undefinedSymbol);
            Check(false, "Test 8: Should fail on undefined symbol");
        }
        catch (Exception)
        {
            Check(true, "Test 8: Correctly failed on undefined symbol");
        }

        Console.WriteLine("Sprint 4 Tests Completed.\n");
    }

    private static void TestSprint5()
    {
        Console.WriteLine("Testing Sprint 5...\n");

        // Test "and" (short-circuit)
        var andExpr = new SExpr.List(new System.Collections.Generic.List<SExpr>
        {
            new SExpr.Atom("and"),
            new SExpr.Atom("1"),
            new SExpr.Atom("nil")
        });
        Check(SExprPrinter.Print(SExprEvaluator.Eval(andExpr)) == "nil", "Test 1: And with nil");

        var andExpr2 = new SExpr.List(new System.Collections.Generic.List<SExpr>
        {
            new SExpr.Atom("and"),
            new SExpr.Atom("1"),
            new SExpr.Atom("2")
        });
        Check(SExprPrinter.Print(SExprEvaluator.Eval(andExpr2)) == "2", "Test 2: And with all true");

        // Test "or"
        var orExpr = new SExpr.List(new System.Collections.Generic.List<SExpr>
        {
            new SExpr.Atom("or"),
            new SExpr.Atom("nil"),
            new SExpr.Atom("2")
        });
        Check(SExprPrinter.Print(SExprEvaluator.Eval(orExpr)) == "2", "Test 3: Or with second true");

        var orExpr2 = new SExpr.List(new System.Collections.Generic.List<SExpr>
        {
            new SExpr.Atom("or"),
            new SExpr.Atom("1"),
            new SExpr.Atom("2")
        });
        Check(SExprPrinter.Print(SExprEvaluator.Eval(orExpr2)) == "1", "Test 4: Or with first true");

        // Test "if"
        var ifExpr = new SExpr.List(new System.Collections.Generic.List<SExpr>
        {
            new SExpr.Atom("if"),
            new SExpr.Atom("1"),
            new SExpr.Atom("2"),
            new SExpr.Atom("3")
        });
        Check(SExprPrinter.Print(SExprEvaluator.Eval(ifExpr)) == "2", "Test 5: If condition true");

        var ifExpr2 = new SExpr.List(new System.Collections.Generic.List<SExpr>
        {
            new SExpr.Atom("if"),
            new SExpr.Atom("nil"),
            new SExpr.Atom("2"),
            new SExpr.Atom("3")
        });
        Check(SExprPrinter.Print(SExprEvaluator.Eval(ifExpr2)) == "3", "Test 6: If condition false");

        // Test "cond"
        var condExpr = new SExpr.List(new System.Collections.Generic.List<SExpr>
        {
            new SExpr.Atom("cond"),
            new SExpr.List(new System.Collections.Generic.List<SExpr> { new SExpr.Atom("nil"), new SExpr.Atom("1") }),
            new SExpr.List(new System.Collections.Generic.List<SExpr> { new SExpr.Atom("1"), new SExpr.Atom("2") })
        });
        Check(SExprPrinter.Print(SExprEvaluator.Eval(condExpr)) == "2", "Test 7: Cond evaluates to 2");

        // Additional Tests

        // Test "cond" with multiple conditions
        var condExpr2 = new SExpr.List(new System.Collections.Generic.List<SExpr>
        {
            new SExpr.Atom("cond"),
            new SExpr.List(new System.Collections.Generic.List<SExpr> { new SExpr.Atom("nil"), new SExpr.Atom("1") }),
            new SExpr.List(new System.Collections.Generic.List<SExpr> { new SExpr.Atom("nil"), new SExpr.Atom("2") }),
            new SExpr.List(new System.Collections.Generic.List<SExpr> { new SExpr.Atom("3"), new SExpr.Atom("3") })
        });
        Check(SExprPrinter.Print(SExprEvaluator.Eval(condExpr2)) == "3", "Test 8: Cond evaluates to 3");

        Console.WriteLine("Sprint 5 Tests Completed.\n");
    }

    private static void TestUserDefinedFunctions()
    {
        Console.WriteLine("Testing User-Defined Functions...\n");

        var defineFn = new SExpr.List(new System.Collections.Generic.List<SExpr>
        {
            new SExpr.Atom("fn"),
            new SExpr.Atom("add1"),
            new SExpr.List(new System.Collections.Generic.List<SExpr> { new SExpr.Atom("x") }),
            new SExpr.List(new System.Collections.Generic.List<SExpr> { new SExpr.Atom("add"), new SExpr.Atom("x"), new SExpr.Atom("1") })
        });
        Check(SExprPrinter.Print(SExprEvaluator.Eval(defineFn)) == "add1", "Test 1: Define function add1");

        var callFn = new SExpr.List(new System.Collections.Generic.List<SExpr>
        {
            new SExpr.Atom("add1"),
            new SExpr.Atom("5")
        });
        Check(SExprPrinter.Print(SExprEvaluator.Eval(callFn)) == "6", "Test 2: Call function add1(5)");

        // Additional Tests

        // Test recursive function
        var defineFact = new SExpr.List(new System.Collections.Generic.List<SExpr>
        {
            new SExpr.Atom("fn"),
            new SExpr.Atom("fact"),
            new SExpr.List(new System.Collections.Generic.List<SExpr> { new SExpr.Atom("n") }),
            new SExpr.List(new System.Collections.Generic.List<SExpr>
            {
                new SExpr.Atom("if"),
                new SExpr.List(new System.Collections.Generic.List<SExpr> { new SExpr.Atom("lte"), new SExpr.Atom("n"), new SExpr.Atom("1") }),
                new SExpr.Atom("1"),
                new SExpr.List(new System.Collections.Generic.List<SExpr>
                {
                    new SExpr.Atom("mul"),
                    new SExpr.Atom("n"),
                    new SExpr.List(new System.Collections.Generic.List<SExpr>
                    {
                        new SExpr.Atom("fact"),
                        new SExpr.List(new System.Collections.Generic.List<SExpr>
                        {
                            new SExpr.Atom("sub"),
                            new SExpr.Atom("n"),
                            new SExpr.Atom("1")
                        })
                    })
                })
            })
        });
        Check(SExprPrinter.Print(SExprEvaluator.Eval(defineFact)) == "fact", "Test 3: Define recursive function fact");

        var callFact = new SExpr.List(new System.Collections.Generic.List<SExpr>
        {
            new SExpr.Atom("fact"),
            new SExpr.Atom("5")
        });
        Check(SExprPrinter.Print(SExprEvaluator.Eval(callFact)) == "120", "Test 4: Call function fact(5)");

        Console.WriteLine("User-Defined Functions Tests Completed.\n");
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
