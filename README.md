# Lisp Interpreter

This project is a simple Lisp interpreter implemented in C#. It supports basic Lisp expressions, arithmetic and logic operations, conditional statements, and user-defined functions. The interpreter evaluates S-expressions and can handle both global variables and locally scoped variables through function calls.

## Features

### 1. **Basic Arithmetic Operations**
   - Supported operations:
     - Addition: `(add x y)`
     - Subtraction: `(sub x y)`
     - Multiplication: `(mul x y)`
     - Division: `(div x y)`
     - Modulo: `(mod x y)`

### 2. **Logic Operations**
   - Supported operations:
     - `and`: `(and e1 e2)`
     - `or`: `(or e1 e2)`
     - `not`: `(not e)`

### 3. **Conditionals**
   - **if**: `(if condition true-branch false-branch)`
   - **cond**: 
     ```lisp
     (cond
       (condition1 result1)
       (condition2 result2)
       ...
     )
     ```

### 4. **User-Defined Functions**
   - Define a function: `(fn fname (arg0 arg1 ... argn) body)`
   - Call a function: `(fname arg0 arg1 ... argn)`
   - Local environments are supported via an environment stack, enabling correct scoping of variables within functions.

### 5. **S-Expression Parsing and Evaluation**
   - This interpreter parses and evaluates S-expressions, supporting symbols, lists, and basic quoting with `quote`.

## Example Usage

### Define and Use a Function

```lisp
(fn add1 (x) (add x 1))  ; Defines a function `add1` that adds 1 to its argument
(add1 5)                 ; Returns 6
