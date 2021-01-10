namespace ComputerTheory
{
    public interface ITapeOperation
    {
        void Execute(Tape tape);
        bool ReadConditionMatch(Tape tape);
    }
}