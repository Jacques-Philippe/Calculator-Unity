# Purpose

A unity calculator project showcasing

- a unity build pipeline
- precommit hooks for unity build and code formatting
- precommit hooks for unit and play tests
- localization?

# Functionality

The final solution allows the user to perform basic calculator operations and comes complete with a very robust testing framework. Code formatting tools are also made available.

# Getting started

1. Clone the repo
1. Install Unity and .NET
1. `(Optional)` Use pre-commit hooks  
   These can be initialized by running `dev\init-hooks.ps1`

## On the precommit hooks

Installing the pre-commit hooks ensures that for every commit you make your solution will:

1. Format your code
1. Build the solution
1. Run tests on the solution

# The plan

## UI

UI should be pretty basic, without necessarily being ugly.

### Number buttons

We want ten buttons for numbers, pressing these will add a number to the calculator in the same way you'd expect a regular calculator to add one.
**We expect that**

- given input `1`, `2`, we should get `12`.
- given input `1`, `2` following an operator, we should get `= [previous] [operator] [input]`

### Operator buttons

We'll use four operator buttons, for addition, subtraction, multiplication, and division respectively.  
**We expect that**

- given no number button input, pressing an operator will give us `= 0 [operator]`
- given number button input, pressing an operator will give us `= [input] [operator] `

### Equals button

We'll use a single button for equals, or getting the result of the overall expression.
**We expect that**

- given no number button input, pressing equals gives us `= 0 `
- given number button input, pressing equals gives us `= [input] `
- given a left-hand side, operator, and right-hand side, pressing equals gives us the result of the expression `= [lhs] [operator] [rhs] = [result]`

![the UI](.docs/Full%20UI.png)

### Results

![the results](.docs/ResultsFormat.png)

## State diagram

We have the following state diagram. Note that if a variable is not represented in a state, its value is null.

![the state diagram](.docs/Calculator%20State%20Diagram.png)

# References

- [Figma diagram](https://www.figma.com/file/ErCYBNpRccDCIVZ6I0j3uW/Calculator?node-id=0%3A1); used for UI and state planning
