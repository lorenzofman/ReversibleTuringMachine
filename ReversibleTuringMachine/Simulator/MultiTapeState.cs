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

        public Action Callback { get; set; }
        
        public MultiTapeState(string name, Action callback, bool isFinal)
        {
            Name = name;
            IsFinal = isFinal;
            Callback = callback;
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
                    TuringUtils.Write("Executing transition: ");
                    deterministicTransition.Execute(ref multiTapeState, tapes);
                    return true;
                case 0:
                    TuringUtils.WriteLine("Found zero transitions");
                    return false;
                default:
                    TuringUtils.WriteLine("Found multiple conditions");
                    throw new NonDeterministicTransition(matching);
            }
        }
        
        public override string ToString()
        {
            return Name;
        }

        public void Signal()
        {
            Callback?.Invoke();
        }
    }
}