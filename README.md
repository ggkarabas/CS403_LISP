
# Lisp Interpreter

This project is a simple Lisp interpreter implemented in C#. It supports basic Lisp expressions, arithmetic and logic operations, conditionals, and user-defined functions. The interpreter evaluates S-expressions, handling both global and locally scoped variables.

## Authors

Project completed by:
- Gabrielle Karabas
- Cooper Olson
- Avery Fernandez
- Gabe Gros

## Output Information

The output.txt file will include the output for the successful test run. 

## Features

### 1. **Basic Arithmetic Operations**
   - Supported operations:
     - **Addition**: `(add x y)`
     - **Subtraction**: `(sub x y)`
     - **Multiplication**: `(mul x y)`
     - **Division**: `(div x y)`
     - **Modulo**: `(mod x y)`

### 2. **Logic Operations**
   - Supported operations:
     - **And**: `(and e1 e2)`
     - **Or**: `(or e1 e2)`
     - **Not**: `(not e)`

### 3. **Conditionals**
   - **If Statement**: `(if condition true-branch false-branch)`
   - **Cond Statement**: 
     ```lisp
     (cond
       (condition1 result1)
       (condition2 result2)
       ...
     )
     ```

### 4. **User-Defined Functions**
   - **Define a function**: `(fn fname (arg0 arg1 ... argn) body)`
   - **Call a function**: `(fname arg0 arg1 ... argn)`
   - Local environments are supported via an environment stack, ensuring correct scoping within functions.

### 5. **S-Expression Parsing and Evaluation**
   - This interpreter parses and evaluates S-expressions, handling:
     - **Symbols**
     - **Lists**
     - **Quoting**: `(quote expr)` or `'(expr)`

### 6. **Variables**
   - **Set a variable**: `(set name value)`
   - **Retrieve a variable**: Refer to the variable name directly (e.g., `name`).

### 7. **Error Handling**
   - Handles malformed S-expressions and runtime errors such as division by zero.

## Example Usage

### Arithmetic and Logic
```lisp
(add 2 3)             ; Returns 5
(sub 7 4)             ; Returns 3
(mul 3 4)             ; Returns 12
(div 8 2)             ; Returns 4
(and 1 nil)           ; Returns nil
(not nil)             ; Returns #t
```

### Conditional Statements
```lisp
(if (lt 3 5) 1 0)     ; Returns 1
(cond
  ((lt 3 2) "No")
  ((eq 3 3) "Yes"))    ; Returns "Yes"
```

### Functions
```lisp
(fn square (x) (mul x x))  ; Defines a function `square`
(square 4)                 ; Returns 16

(fn fib (n)
  (if (lt n 2) 
      1 
      (add (fib (sub n 1)) (fib (sub n 2)))))  ; Defines Fibonacci
(fib 10)                   ; Returns 55
```

### Variables
```lisp
(set x 10)          ; Sets `x` to 10
(add x 5)           ; Returns 15
```

## How to Build and Run

### Prerequisites
- .NET SDK v7.0
  - [Download Here](https://dotnet.microsoft.com/en-us/download/dotnet/7.0/runtime?cid=getdotnetcore&os=windows&arch=x64)

### Build
1. Clone this repository:
   ```bash
   git clone https://github.com/ggkarabas/CS403_LISP.git
   cd CS403_LISP/CS403_LISP
   ```
2. Build the project:
   ```bash
   dotnet build
   ```

### Run
#### Windows
1. Navigate to the `publish` folder for the Windows build:
   ```bash
   cd /path/to/CS403_LISP/bin/Release/net7.0/win-x64/publish/
   ```
2. Execute the program:
   ```bash
   ./CS403_LISP.exe
   ```

#### Linux
1. Navigate to the `publish` folder for the Linux build:
   ```bash
   cd /path/to/CS403_LISP/bin/Release/net7.0/linux-x64/publish/
   ```
2. Make the binary executable:
   ```bash
   chmod +x CS403_LISP
   ```
3. Run the binary:
   ```bash
   ./CS403_LISP
   ```

### Testing
- The `Program.cs` file includes predefined tests for core functionality. To run the tests:
   ```bash
   dotnet run
   ```

## File Structure
- `SExpr.cs`: Defines the S-expression data structures.
- `SExprParser.cs`: Implements the parser for S-expressions.
- `SExprPrinter.cs`: Converts S-expressions back into string form.
- `SExprUtils.cs`: Provides utility functions for working with S-expressions.
- `SExprEvaluator.cs`: Implements the evaluation logic.
- `Program.cs`: Includes test cases and a main entry point for running the interpreter.
