using System;

namespace ComputerTheory
{
    public class InvalidTuringMachineConfiguration : Exception
    {
        public InvalidTuringMachineConfiguration(string message) 
            : base ($"Reversible Turing machine has invalid configuration. Error: {message}")
        {
        }
    }
}