using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace ComputerTheory
{
    public class MultiTapeState
    {
        private readonly List<MultiTapeTransition> transitions = new();
        
        public string Name { get; }
        public bool IsFinal { get; }
        
        public MultiTapeState(string name, bool isFinal = false)
        {
            Name = name;
            IsFinal = isFinal;
        }
        
        public void AddTransition(MultiTapeTransition tapeOperation)
        {
            transitions.Add(tapeOperation);
        }

        public bool DeterministicTransition(ref MultiTapeState multiTapeState, IEnumerable<Tape> tapes)
        {
            ImmutableArray<MultiTapeTransition> matching = transitions.Where(x => x.ShouldTransition(tapes)).ToImmutableArray();

            switch (matching.Length)
            {
                case 1:
                    MultiTapeTransition deterministicTransition = matching.First();
                    Console.Write("Executing transition: ");
                    deterministicTransition.Execute(ref multiTapeState, tapes);
                    return true;
                case 0:
                    Console.WriteLine("Found zero transitions");
                    return false;
                default:
                    Console.WriteLine("Found multiple conditions");
                    throw new NonDeterministicTransition(matching);
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}