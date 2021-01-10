using System;

namespace ComputerTheory
{
    public static class TuringUtils
    {
        public static bool Verbose { get; set; }

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Global
        public static void TuringAssert(bool condition, string message = null)
        {
            if (condition == false)
            {
                throw new InvalidTuringMachineConfiguration(message);
            }
        }

        public static void WriteLine(string message)
        {
            if (!Verbose)
            {
                return;
            }
            Console.WriteLine(message);
        }
        
        public static void Write(string message)
        {
            if (!Verbose)
            {
                return;
            }
            Console.Write(message);
        }
    }
}