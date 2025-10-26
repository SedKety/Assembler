using System;
using System.Collections.Generic;
using System.IO;

namespace Assembler
{
    internal class Program
    {
        private const int EntryPoint = 0x00000200;
        private const string ExePath = @"C:\Users\nobem\Documents\Github\Assembler\Assembler\bin\Debug\CurTestFile.exe";
        private const string Script = @"C:\Users\nobem\Documents\Github\Assembler\Assembler\Code.txt";

        static byte[] program =
{
            0x3B, // ModR/M byte for CMP  = MODE, REGISTER, MEMORY
            0xC3, // ModR/M byte indicating both operands are registers (EAX and EBX)

            0xEB, //Jmp short
            0xFC, //Jump back 4 bytes to create an infinite loop
        };
        static void Main()
        {
            BinaryWriter writer = new BinaryWriter(File.Open(ExePath, FileMode.Open, FileAccess.Write));

            //Wipe the .exe first
            Assembler.CleanSlate(writer, EntryPoint);

            //Assemble the script into the .exe
            Assembler.Assemble(writer, EntryPoint, Script);
        }


    }
}
