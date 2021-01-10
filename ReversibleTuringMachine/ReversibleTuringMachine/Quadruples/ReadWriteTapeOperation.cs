namespace ComputerTheory
{
    public readonly struct ReadWriteTapeOperation : ITapeOperation
    {
        private readonly char read;
        
        private readonly char write;
        
        public ReadWriteTapeOperation(char read, char write)
        {
            this.read = read;
            this.write = write;
        }

        public void Execute(Tape tape)
        {
            tape.Write(write);
        }

        public bool ReadConditionMatch(Tape tape)
        {
            return tape.Read() == read;
        }

        public ITapeOperation Invert()
        {
            return new ReadWriteTapeOperation(write, read);
        }

        public override string ToString()
        {
            return $"Reads {ConvertSymbol(read)} and writes {ConvertSymbol(write)}";
        }

        private static string ConvertSymbol(char c)
        {
            return c == Tape.Blank ? "Blank" : $"{c}";
        }
    }
}