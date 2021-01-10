using System;

namespace ComputerTheory
{
    public static class InversionExtensions
    {
        public static ShiftDirection Invert(this ShiftDirection direction)
        {
            return direction switch
            {
                ShiftDirection.Left => ShiftDirection.Right,
                ShiftDirection.Right => ShiftDirection.Left,
                ShiftDirection.Stay => ShiftDirection.Stay,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }
    }
}