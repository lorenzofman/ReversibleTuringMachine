using System;

namespace ComputerTheory
{
    public readonly struct ShiftOperation : ITapeOperation
    {
        private readonly ShiftDirection direction;
        
        public ShiftOperation(ShiftDirection direction)
        {
            this.direction = direction;
        }

        public void Execute(Tape tape)
        {
            switch (direction)
            {
                case ShiftDirection.Left:
                    tape.ShiftLeft();
                    break;
                case ShiftDirection.Right:
                    tape.ShiftRight();
                    break;
                case ShiftDirection.Stay:
                    // Nothing
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool ReadConditionMatch(Tape tape)
        {
            return true;
        }

        public override string ToString()
        {
            return direction switch
            {
                ShiftDirection.Left => "Shifts left",
                ShiftDirection.Right => "Shifts right",
                ShiftDirection.Stay => "Stays",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
    
    public enum ShiftDirection
    {
        Left,
        Right,
        Stay
    }
    
    
}