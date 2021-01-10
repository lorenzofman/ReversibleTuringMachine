using System;
using System.Collections.Generic;
using System.Linq;

namespace ComputerTheory
{
    public class TuringMachineDefinition
    {
        public string Input { get; }
        public List<Transition> Transitions { get; } = new();
        public HashSet<char> TapeSymbols { get; } = new();
        public State Initial { get; }
        public State Final { get; }

        private readonly Dictionary<string, State> states = new();
        
        private char transitionUniqueId = 'a';

        public TuringMachineDefinition()
        {
            string initialName = AskConsoleLine("Initial State: ");
            string finalName = AskConsoleLine("Accept State: ");
            TuringUtils.TuringAssert(initialName != finalName, "Initial state cannot be final");
            Initial = GetOrCreateState(initialName);
            Final = GetOrCreateState(finalName);
            
            int transitionsCount = AskConsoleInteger("Number of transitions: ");

            const int charRange = char.MaxValue - char.MinValue;
            TuringUtils.TuringAssert(transitionsCount < charRange, $"Transition count must be lower than {charRange}");

            for (int i = 0; i < transitionsCount; i++)
            {
                string line = AskConsoleLine($"Transition #{i + 1}: ");

                try
                {
                    Transitions.Add(ParseTransition(line));
                }
                catch (InvalidTuringMachineConfiguration e)
                {
                    Console.WriteLine($"Could not parse this transition ({line}). Reason: {e.Message}");
                }
                catch (Exception)
                {
                    Console.WriteLine($"Could not parse this transition ({line}). Reason: Generic parsing error");
                }
            }

            Input = AskConsoleLine("Input: ");
            
            ScanTapeSymbols();
        }

        private void ScanTapeSymbols()
        {
            foreach (char c in Input)
            {
                TapeSymbols.Add(c);
            }
            foreach (Transition transition in Transitions)
            {
                TapeSymbols.Add(transition.Read);
                TapeSymbols.Add(transition.Write);
            }

            TapeSymbols.Remove(Tape.Blank);
        }

        private Transition ParseTransition(string line)
        {
            string[] values = line.Split(' ');
            string initial = values[0];
            string final = values[1];
            string read = values[2];
            string write = values[3];
            ShiftDirection direction = ParseDirection(values[4]);
            return new Transition(GetOrCreateState(initial), GetOrCreateState(final), read.Single(), 
                write.Single(), direction, transitionUniqueId++);
        }

        private static ShiftDirection ParseDirection(string value)
        {
            return value switch
            {
                "L" => ShiftDirection.Left,
                "R" => ShiftDirection.Right,
                "S" => ShiftDirection.Stay,
                _ => throw new InvalidTuringMachineConfiguration(
                    $"Could not parse direction {value}. Please use L (Left), R (Right) and S (Stay)")
            };
        }

        private State GetOrCreateState(string name)
        {
            if (states.ContainsKey(name))
            {
                return states[name];
            }

            State state = new(name);
            states.Add(name, state);
            return state;
        }

        private static int AskConsoleInteger(string message)
        {
            Console.WriteLine(message);
            if (int.TryParse(Console.ReadLine(), out int i))
            {
                return i;
            }

            throw new InvalidTuringMachineConfiguration("Incorrect value of transitions");
        }
        
        private static string AskConsoleLine(string message)
        {
            Console.WriteLine(message); return Console.ReadLine();
        }
    }
}