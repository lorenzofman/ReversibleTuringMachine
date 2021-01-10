using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerTheory
{
    public class NonDeterministicTransition : Exception
    {
        private readonly IEnumerable<MultiTapeTransition> matchingTransitions;

        public NonDeterministicTransition(IEnumerable<MultiTapeTransition> matchingTransitions)
        {
            this.matchingTransitions = matchingTransitions;
        }

        public override string ToString()
        {
            return $"{base.ToString()}\n {MatchingTransitionsLog()}";
        }

        private string MatchingTransitionsLog()
        {
            StringBuilder builder = new();
            
            foreach (MultiTapeTransition multiTapeTransition in matchingTransitions)
            {
                builder.AppendLine(multiTapeTransition.ToString());
            }

            return builder.ToString();
        }
    }
}