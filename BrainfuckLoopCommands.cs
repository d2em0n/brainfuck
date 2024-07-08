using System.Collections.Generic;
using System.Reflection;

namespace func.brainfuck
{
	public class BrainfuckLoopCommands
	{
		private static void AnalyzeLoops(IVirtualMachine vm,
            Dictionary<int, int> openBrackets, Dictionary<int, int> endBrackets)
		{
            var instructions = vm.Instructions;            
            var stack = new Stack<int>();
            for (int i = 0; i < instructions.Length; i++)
            {
                if (instructions[i] == '[')
                {
                    stack.Push(i);
                    openBrackets.Add(i, 0);
                }
                if (instructions[i] == ']')
                {
                    var openBracketIndex = stack.Pop();
                    openBrackets[openBracketIndex] = i;
                    endBrackets.Add(i, openBracketIndex);
                }
            }
        }

		public static void RegisterTo(IVirtualMachine vm)
		{
            var openBrackets = new Dictionary<int, int>();
            var endBrackets = new Dictionary<int, int>();
            AnalyzeLoops (vm, openBrackets, endBrackets);

            vm.RegisterCommand('[', b => 
			{
				if (b.Memory[b.MemoryPointer] == 0)
					b.InstructionPointer = openBrackets[b.InstructionPointer];
			});
			vm.RegisterCommand(']', b => 
			{ 
				if (b.Memory[b.MemoryPointer] != 0)
					b.InstructionPointer = endBrackets[b.InstructionPointer];
			});
		}
	}
}