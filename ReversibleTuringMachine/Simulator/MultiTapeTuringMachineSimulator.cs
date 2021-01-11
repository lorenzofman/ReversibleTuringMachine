using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerTheory
{
    public class MultiTapeTuringMachineSimulator
    {
        private readonly IMultiTapeTuringMachineDefinition multiTapeTuringMachineDefinition;

        public MultiTapeTuringMachineSimulator(IMultiTapeTuringMachineDefinition multiTapeTuringMachineDefinition)
        {
            this.multiTapeTuringMachineDefinition = multiTapeTuringMachineDefinition;
        }

        public void Run()
        {
            MultiTapeState currentState = multiTapeTuringMachineDefinition.Initial;

            do
            {
                currentState.Signal();
            } while (currentState.DeterministicTransition(ref currentState, multiTapeTuringMachineDefinition.Tapes));
            
            currentState.Signal();
            
            Halt(currentState.IsFinal ? CompletionState.Accept : CompletionState.Reject);
        }

        private static void Halt(CompletionState state)
        {
            switch (state)
            {
                case CompletionState.Accept:
                    Console.WriteLine("Input was accepted");
                    break;
                case CompletionState.Reject:
                    Console.WriteLine("Input was rejected");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private enum CompletionState
        {
            Accept,
            Reject
        }
    }

    public interface IMultiTapeTuringMachineDefinition
    {
        MultiTapeState Initial { get; }
        IReadOnlyList<Tape> Tapes { get; }
    }
}