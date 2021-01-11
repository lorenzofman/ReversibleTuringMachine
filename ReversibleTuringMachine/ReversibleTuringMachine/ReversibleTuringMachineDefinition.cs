using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComputerTheory
{
    public class ReversibleTuringMachineDefinition : IMultiTapeTuringMachineDefinition
    {
        #region Reversible Turing Machine

        /// <summary>
        /// Collection with multi tape states
        /// </summary>
        private readonly Dictionary<string, MultiTapeState> states = new();

        /// <summary>
        /// The initial state
        /// </summary>
        public MultiTapeState Initial { get; }

        /// <summary>
        /// Tapes collection
        /// </summary>
        public IReadOnlyList<Tape> Tapes { get; }

        /// <summary>
        /// Creates a new reversible turing machine from an ordinary turing machine
        /// </summary>
        public ReversibleTuringMachineDefinition(TuringMachineDefinition tm)
        {
            Tapes = new Tape[] {new(tm.Input, "Working Tape"), new("", "History Tape"), new("", "Output Tape")};

            CreateMultiTapeTransitionsFromTransitions(tm);

            CreateCopyOutputStates(tm);

            CreateInvertedStatesFromOrdinary(tm);
            
            Initial = states.Single(st => st.Key == tm.Initial.Name).Value;
        }

        private MultiTapeState GetOrCreateState(string name, bool isFinal = false, Action callback = null)
        {
            if (states.ContainsKey(name))
            {
                MultiTapeState state = states[name];
                state.Callback += callback;
                TuringUtils.TuringAssert(isFinal == state.IsFinal, $"{state} was created before and it is not final.");
                return state;
            }
            else
            {
                MultiTapeState state = new(name, callback, isFinal);
                states.Add(name, state);
                return state;
            }
        }

        #endregion

        #region TM -> RTM

        /// <summary>
        /// Converts original TM states to reversible ones. Also convert
        /// transitions from quintuples to quadruples with history writing  
        /// </summary>
        private void CreateMultiTapeTransitionsFromTransitions(TuringMachineDefinition tm)
        {
            foreach (Transition transition in tm.Transitions)
            {
                MultiTapeState initial = GetOrCreateState($"{transition.InitialState.Name}");
                MultiTapeState final = GetOrCreateState($"{transition.FinalState.Name}");

                MultiTapeState mth = GetOrCreateState($"{transition.Id}'");

                initial.AddTransition(new MultiTapeTransition(
                    mth,
                    new ReadWriteTapeOperation(transition.Read, transition.Write),
                    new ShiftOperation(ShiftDirection.Right),
                    new NullOperation()));

                mth.AddTransition(new MultiTapeTransition(
                    final,
                    new ShiftOperation(transition.Direction),
                    new ReadWriteTapeOperation(Tape.Blank, transition.Id),
                    new NullOperation()));
            }

            MultiTapeState multiFinal = GetOrCreateState($"{tm.Final.Name}", false, OnCompute);

            multiFinal.AddTransition(new MultiTapeTransition(GetOrCreateState("Copy Output"),
                new NullOperation(),
                new NullOperation(),
                new NullOperation()));
        }

        #endregion

        #region Copy Output

        private void CreateCopyOutputStates(TuringMachineDefinition tm)
        {
            MultiTapeState copyOutput = GetOrCreateState("Copy Output");
            MultiTapeState seekStart = GetOrCreateState("Seek Start");

            MultiTapeState next = GetOrCreateState($"{tm.Final.Name} Inverted", false, OnCopyOutput);
            foreach (char symbol in tm.TapeSymbols)
            {
                CreateCopyOutputTransition(copyOutput, copyOutput, symbol, ShiftDirection.Right);
                CreateHeadRepositioningTransition(seekStart, seekStart, symbol, ShiftDirection.Left);
            }

            CreateCopyOutputTransition(copyOutput, seekStart, Tape.Blank, ShiftDirection.Left);
            CreateHeadRepositioningTransition(seekStart, next, Tape.Blank, ShiftDirection.Right);

        }

        private void CreateHeadRepositioningTransition(MultiTapeState initial, MultiTapeState next, char symbol,
            ShiftDirection direction)
        {
            MultiTapeState tapeHeadRollbackShift = GetOrCreateState($"{initial.Name} {symbol}'");

            initial.AddTransition(new MultiTapeTransition(tapeHeadRollbackShift,
                new ReadWriteTapeOperation(symbol, symbol),
                new NullOperation(),
                new NullOperation()));
            tapeHeadRollbackShift.AddTransition(new MultiTapeTransition(next,
                new ShiftOperation(direction),
                new NullOperation(),
                new ShiftOperation(direction)));
        }

        private void CreateCopyOutputTransition(MultiTapeState initial, MultiTapeState next, char symbol,
            ShiftDirection direction)
        {
            MultiTapeState copyOutputShift = GetOrCreateState($"Copy Output {symbol}");
            initial.AddTransition(new MultiTapeTransition(copyOutputShift,
                new ReadWriteTapeOperation(symbol, symbol),
                new NullOperation(),
                new ReadWriteTapeOperation(Tape.Blank, symbol)));
            copyOutputShift.AddTransition(new MultiTapeTransition(next,
                new ShiftOperation(direction),
                new NullOperation(),
                new ShiftOperation(direction)));
        }

        #endregion

        #region Retrace

        private void CreateInvertedStatesFromOrdinary(TuringMachineDefinition tm)
        {
            foreach (Transition transition in tm.Transitions)
            {
                MultiTapeState initial = GetOrCreateState($"{transition.InitialState.Name} Inverted", tm.Initial == transition.InitialState);
                MultiTapeState final = GetOrCreateState($"{transition.FinalState.Name} Inverted", tm.Initial == transition.FinalState);

                MultiTapeState mth = GetOrCreateState($"{transition.Id} Inverted'");

                final.AddTransition(new MultiTapeTransition(
                    mth,
                    new ShiftOperation(transition.Direction.Invert()),
                    new ReadWriteTapeOperation(transition.Id, Tape.Blank),
                    new NullOperation()));

                mth.AddTransition(new MultiTapeTransition(
                    initial,
                    new ReadWriteTapeOperation(transition.Write, transition.Read),
                    new ShiftOperation(ShiftDirection.Left),
                    new NullOperation()));
            }

            GetOrCreateState($"{tm.Initial.Name} Inverted", true, OnRetrace);
        }
        #endregion
        
        private void OnCompute()
        {
            Console.WriteLine("Machine state after computing");
            Console.WriteLine(this);
            TuringUtils.TuringAssert(Tapes[0].SeeksBegin, "Compute must finish with working tape head seeking start");
            TuringUtils.TuringAssert(Tapes[2].IsBlank, "Compute must not write to output tape");
        }

        private void OnCopyOutput()
        {
            Console.WriteLine("Machine state after copying output");
            Console.WriteLine(this);
            TuringUtils.TuringAssert(Tapes[0].SeeksBegin, "Copy output must finish with working tape head seeking start");
            TuringUtils.TuringAssert(Tapes[2].SeeksBegin, "Copy output must not write to output tape");
        }

        private void OnRetrace()
        {
            Console.WriteLine("Machine state after retracing");
            Console.WriteLine(this);
        }
        
        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine("Machine state: ");
            foreach (Tape tape in Tapes)
            {
                sb.AppendLine($"\t{tape}");
            }

            return sb.ToString();
        }
    }
}

