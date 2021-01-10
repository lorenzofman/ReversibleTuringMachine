using ComputerTheory;

TuringUtils.Verbose = true;

TuringMachineDefinition ordinaryTuringMachine = new();
ReversibleTuringMachineDefinition reversibleTuringMachine = new(ordinaryTuringMachine);
MultiTapeTuringMachineSimulator simulator = new(reversibleTuringMachine);
simulator.Run();