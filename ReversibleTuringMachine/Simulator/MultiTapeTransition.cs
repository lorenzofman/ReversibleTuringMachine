using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ComputerTheory
{
    public class MultiTapeTransition
    {
        public MultiTapeTransition(MultiTapeState final, params ITapeOperation[] tapeOperations)
        {
            Final = final;
            TapeOperations = tapeOperations;
        }

        public MultiTapeState Final { get; }

        private ITapeOperation[] TapeOperations { get; }

        public bool ShouldTransition(IEnumerable<Tape> tapes)
        {
            return TapeOperations.Zip(tapes, Tuple.Create).All(x => x.Item1.ReadConditionMatch(x.Item2));
        }
        
        public void Execute(ref MultiTapeState initial, IEnumerable<Tape> tapes)
        {
            TuringUtils.WriteLine($"From {initial} to `{Final}.");
            TuringUtils.Write("Applying Tape Operations: ");
            int i = 0;
            foreach ((ITapeOperation op, Tape tape) in TapeOperations.Zip(tapes, Tuple.Create))
            {
                op.Execute(tape);
                TuringUtils.Write($"{op} on {tape.Name}. ");

            }
            TuringUtils.WriteLine("");
            initial = Final;
        }
        
    }
}