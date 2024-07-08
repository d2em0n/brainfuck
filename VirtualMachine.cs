using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace func.brainfuck
{
	public class VirtualMachine : IVirtualMachine
	{
		public string Instructions { get; }
		public int InstructionPointer { get; set; }
		public byte[] Memory { get; }
		public int MemoryPointer { get; set; }
		private readonly Dictionary<char, Action<IVirtualMachine>> Commands;
		public VirtualMachine(string program, int memorySize)
		{
			InstructionPointer = 0;
			Memory = new byte[memorySize];
			MemoryPointer = 0;
			Instructions = program;
			Commands = new Dictionary<char, Action<IVirtualMachine>>();
		}

		public void RegisterCommand(char symbol, Action<IVirtualMachine> execute)
		{
			Commands.Add(symbol, execute);
		}

		public void Run()
		{
			while (InstructionPointer < Instructions.Length)
			{
				if (Commands.ContainsKey(Instructions[InstructionPointer]))
					Commands[Instructions[InstructionPointer]]?.Invoke(this);
				InstructionPointer++;
			}
		}
	}
}