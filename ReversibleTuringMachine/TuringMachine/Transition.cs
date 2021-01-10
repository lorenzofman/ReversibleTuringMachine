using ComputerTheory;

public readonly struct Transition
{
    public State InitialState { get; }
    public State FinalState { get; }
    public char Read { get; }
    public char Write { get; }
    public ShiftDirection Direction { get; }
    public char Id { get; }

    public Transition(State initialState, State finalState, char read, char write, ShiftDirection shiftDirection, char id)
    {
        Read = read;
        Write = write;
        Direction = shiftDirection;
        InitialState = initialState;
        FinalState = finalState;
        Id = id;
    }
}