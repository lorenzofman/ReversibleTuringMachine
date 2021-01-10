Reversible Turing Machine Based on the work of C.H. Bennett's Logical Reversibility of Computation.

This simulator operates in three steps:

1. Parses a regular (ordinary) turing machine (which is by nature irreversibly)
2. Converts the turing machine to an reversible turing machine using Bennett's approach
3. Simulates the reversible machine using a deterministic multi tape simulator which does not know the specifics of the RTM

This project is written in C# 9.0. I recommend using Rider to run it. Rider is available across multiple platforms including Mac.

The application parses the ordinary Turing machine from the standard input. The format is as follow:

```
Initial State
Final State
Number of transitions
Transition #1
Transition #2
...
Transition #N
Input
```

The required number of information is reduced by implying them using the transitions data.

Each transition must be specified as 

```
InitialState FinalState ReadSymbol WriteSymbol ShiftDirection
```

The simulator accepts or rejects the input as the ordinary Turing machine would.

> For an example of ordinary (tested) Turing Machine use [Balanced Parenthesis](ReversibleTuringMachine/InputExamples/BalancedParenthesis.txt)
