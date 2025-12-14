using System;
using System.Collections.Generic;
using System.IO;

namespace Assembler
{
    internal class Assembler
    {

        static byte[] program;

        public static void Assemble(BinaryWriter writer, int entrypoint, string script)
        {
            OpcodeRuleEngine ruleEngine = new OpcodeRuleEngine();
            program = ruleEngine.Translate(File.ReadAllText(script));

            writer.Seek(entrypoint, SeekOrigin.Begin);

            for (int i = 0; i < program.Length; i++)
            {
                writer.Write(program[i]);
            }
        }
        public static void CleanSlate(BinaryWriter writer, int entrypoint)
        {
            writer.Seek(entrypoint, SeekOrigin.Begin);
            for (int i = 0; i < 128; i++)
            {
                writer.Write((byte)0);
            }
        }
    }
}
