namespace ComputerTheory
{
    /// <summary>
    /// This is just a syntatic abstraction over operations. It could be easily
    /// exchanged with a read write (with same symbol) or a stay shift. It was
    /// created to provide better readability
    /// </summary>
    public struct NullOperation : ITapeOperation
    {
        public void Execute(Tape tape)
        {
            
        }

        public bool ReadConditionMatch(Tape tape)
        {
            return true;
        }

        public override string ToString()
        {
            return "Null operation";
        }
    }
}