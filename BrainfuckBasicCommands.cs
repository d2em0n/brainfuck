using System;
using System.Collections.Generic;
using System.Linq;

namespace func.brainfuck
{
    public class BrainfuckBasicCommands
    {
        private static void RegisterSaveSymbols(IVirtualMachine vm)
        {
            var intervals = new Dictionary<byte, byte>()
            {
                { 48 , 58},
                { 65, 91 },
                { 97, 123}
            };
            foreach (var start in intervals.Keys)
            {
                var end = intervals[start];
                for (byte i = start; i < end; i++)
                {
                    var j = i;
                    vm.RegisterCommand((char)i, b => b.Memory[b.MemoryPointer] = j);
                }
            }
        }

        public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
        {
            vm.RegisterCommand('.', b => { write((char)b.Memory[b.MemoryPointer]); });
            vm.RegisterCommand('+', b => { unchecked { b.Memory[b.MemoryPointer]++; } });
            vm.RegisterCommand('-', b => { unchecked { b.Memory[b.MemoryPointer]--; } });
            vm.RegisterCommand('>', b => b.MemoryPointer =
                (b.MemoryPointer + 1 == b.Memory.Length) ? 0 : b.MemoryPointer + 1);
            vm.RegisterCommand('<', b => b.MemoryPointer =
                (b.MemoryPointer - 1 < 0) ? b.Memory.Length - 1 : b.MemoryPointer - 1);
            vm.RegisterCommand(',', b => b.Memory[b.MemoryPointer] = (byte)read());
            RegisterSaveSymbols(vm);
        }
    }
}