// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text.RegularExpressions;
//
// namespace ComputerTheory
// {
//     public class ReversibleTuringMachineCopy
//     {
//         private readonly Tape workingTape;
//         private readonly Tape historyTape;
//         private readonly Tape outputTape;
//
//         private readonly Dictionary<string, State> states = new();
//         private readonly HashSet<char> inputAlphabet = new();
//         private readonly HashSet<char> tapeAlphabet = new();
//         private readonly string input;
//         private State currentState;
//         
//         #region Parsing
//         
//         public ReversibleTuringMachineCopy()
//         {
//             ReadSpacedInformation(int.Parse).GetEnumerator()
//                 .Read(out int statesCount)
//                 .Read(out int inputAlphabetCount)
//                 .Read(out int tapeAlphabetCount)
//                 .Read(out int transitionNumber);
//             
//             TuringAssert(statesCount > 0, "Machine must have at least one state");
//             TuringAssert(inputAlphabetCount > 0, "Machine must have at least one alphabet symbol");
//             TuringAssert(tapeAlphabetCount > 0, "Machine must have at least one tape symbol");
//             TuringAssert(transitionNumber > 0, "Machine must have at least one transition");
//
//             string[] stateNames = ReadSpacedInformation(Convert.ToString).ToArray();
//
//             for (int i = 1; i < stateNames.Length - 1; i++)
//             {
//                 states.Add(stateNames[i]!, new State(stateNames[i]));
//             }
//
//             string first = stateNames.First();
//             string last = stateNames.Last();
//
//             InitialState initialState = new(first);
//             FinalState finalState = new(last);
//             currentState = initialState;
//             states.Add(first, initialState);
//             states.Add(last, finalState);
//             
//             TuringAssert(states.Count == statesCount, "States count is not the same as indicated above");
//
//             foreach (char symbol in ReadSpacedInformation(Convert.ToChar))
//             {
//                 inputAlphabet.Add(symbol);
//             }
//             
//             TuringAssert(inputAlphabet.Count == inputAlphabetCount, "Input alphabet count is not the same as indicated above");
//             
//             foreach (char symbol in ReadSpacedInformation(Convert.ToChar))
//             {
//                 tapeAlphabet.Add(symbol);
//             }
//             
//             TuringAssert(tapeAlphabet.Count == tapeAlphabetCount, "Tape alphabet count is not the same as indicated above");
//
//             for (int i = 0; i < transitionNumber; i++)
//             {
//                 CreateTransition(Console.ReadLine());
//             }
//
//             input = Console.ReadLine();
//             
//             if (input == null)
//             {
//                 throw new InvalidReversibleTuringMachineConfiguration("Transition count is not the same as indicated above or input is not provided");
//             }
//
//             workingTape = new Tape(input);
//             historyTape = new Tape("");
//             outputTape = new Tape("");
//         }
//
//         private void CreateTransition(string code)
//         { 
//             Regex rx = new(@"\((?<fromState>.+),(?<workingTapeInput>.+),(?<historyTapeInput>.+),(?<outputTapeInput>.+)\)\=\((?<toState>.+),(?<workingTapeOutput>.+),(?<historyTapeOutput>.+),(?<outputTapeOutput>.+)\)");
//             Match match = rx.Match(code);
//
//             if (!match.Success)
//             {
//                 throw new InvalidReversibleTuringMachineConfiguration("Transition specification is incorrect"); 
//             }
//             
//             string fromState = match.Groups["fromState"].Value;
//             char workingTapeInput = match.Groups["workingTapeInput"].Value.Single();
//             char historyTapeInput = match.Groups["historyTapeInput"].Value.Single();
//             char outputTapeInput = match.Groups["outputTapeInput"].Value.Single();
//             
//             string toState = match.Groups["toState"].Value;
//             char workingTapeOutput = match.Groups["workingTapeOutput"].Value.Single();
//             char historyTapeOutput = match.Groups["historyTapeOutput"].Value.Single();
//             char outputTapeOutput = match.Groups["outputTapeOutput"].Value.Single();
//
//             CheckOperation(workingTapeInput, workingTapeOutput);
//             CheckOperation(historyTapeInput, historyTapeOutput);
//             CheckOperation(outputTapeInput, outputTapeOutput);
//             
//             MultiTapeTransition multiTapeTransition = new(states[toState],
//                 CreateOperation(workingTapeInput, workingTapeOutput),
//                 CreateOperation(historyTapeInput, historyTapeOutput),
//                 CreateOperation(outputTapeInput, outputTapeOutput));
//             
//             states[fromState].AddTransition(multiTapeTransition);
//         }
//
//         private void CheckOperation(char input, char output)
//         {
//             if (input == '/')
//             {
//                 return;
//             }
//             
//             Console.WriteLine($"Checking input: ({input}) and output: ({output})");
//             TuringAssert(tapeAlphabet.Contains(input), $"Input {input} is not specified in working tape alphabet");
//             TuringAssert(tapeAlphabet.Contains(output), $"Output {output} is not specified in working tape alphabet");
//         }
//
//         private static ITapeOperation CreateOperation(char input, char output)
//         {
//             if (input == '/')
//             {
//                 return new ShiftOperation(ParseDirection(output));
//             }
//
//             return new ReadWriteTapeOperation(input, output);
//         }
//
//         private static ShiftDirection ParseDirection(char symbol)
//         {
//             return symbol switch
//             {
//                 'R' => ShiftDirection.Right,
//                 'L' => ShiftDirection.Left,
//                 'N' => ShiftDirection.Stay,
//                 _ => throw new InvalidReversibleTuringMachineConfiguration(
//                     "Direction could not be parsed. Please use R, L and N for directions")
//             };
//         }
//         
//         private static IEnumerable<T> ReadSpacedInformation<T>(Func<string, T> converter)
//         {
//             string firstLine = Console.ReadLine();
//             if (firstLine == null)
//             {
//                 throw new Exception();
//             }
//
//             foreach (string item in firstLine.Split(' '))
//             {
//                 yield return converter.Invoke(item);
//             }
//         }
//         
//         #endregion
//
//         #region Computing
//         
//         public void Run()
//         {
//             Compute();
//             // CopyOutput();
//             // Retrace();
//         }
//
//         private void Compute()
//         {
//             while (currentState is not FinalState)
//             {
//                 Console.WriteLine(this);
//                 
//                 Console.WriteLine($"Executing state {currentState}");
//                 
//                 if (currentState.DeterministicTransition(ref currentState, workingTape, historyTape, outputTape))
//                 {
//                     Console.WriteLine($"Transitioning to {currentState}");
//                 }
//                 else
//                 {
//                     Halt(CompletionState.Reject);
//                     return;
//                 }
//             }
//             Halt(CompletionState.Accept);
//         }
//
//         private void CopyOutput()
//         {
//             outputTape.CopyFrom(workingTape.ToString());
//         }
//
//         private void Retrace()
//         {
//             foreach (State state in states.Values)
//             {
//                 state.Invert();
//             }
//             while (currentState is not InitialState)
//             {
//                 Console.WriteLine($"Reverting state {currentState}");
//                 if (currentState.DeterministicTransition(ref currentState, workingTape, historyTape, outputTape))
//                 {
//                     Console.WriteLine($"Transitioning to {currentState}");
//                 }
//                 else
//                 {
//                     Halt(CompletionState.Reject);
//                     return;
//                 }
//             }
//             Halt(CompletionState.Accept);
//         }
//         
//         private static void Halt(CompletionState state)
//         {
//             switch (state)
//             {
//                 case CompletionState.Accept:
//                     Console.WriteLine("Input was accepted");
//                     break;
//                 case CompletionState.Reject:
//                     Console.WriteLine("Input was rejected");
//                     break;
//                 default:
//                     throw new ArgumentOutOfRangeException(nameof(state), state, null);
//             }
//         }
//         
//         #endregion
//
//         #region Validation
//
//         public void Validate()
//         {
//             Console.WriteLine($"Working Tape: {workingTape}");
//             Console.WriteLine($"History Tape: {historyTape}");
//             Console.WriteLine($"Output Tape: {outputTape}");
//             TuringAssert(workingTape.ToString() == input);
//             TuringAssert(historyTape.IsBlank());
//         }
//         
//         // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
//         private static void TuringAssert(bool condition, string message = null)
//         {
//             if (condition == false)
//             {
//                 throw new InvalidReversibleTuringMachineConfiguration(message);
//             }
//         }
//
//         #endregion
//         
//         public override string ToString()
//         {
//             return $"Working Tape: {workingTape}\n" +
//                    $"History Tape: {historyTape}\n" +
//                    $"Output Tape: {outputTape}";
//         }
//     }
//
//     internal class InvalidReversibleTuringMachineConfiguration : Exception
//     {
//         public InvalidReversibleTuringMachineConfiguration(string message) 
//             : base ($"Reversible Turing machine has invalid configuration. Error: {message}")
//         {
//         }
//     }
//
//     public enum CompletionState
//     {
//         Accept,
//         Reject
//     }
// }