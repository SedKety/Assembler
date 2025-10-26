using Assembler;
using System;

namespace Compiler
{
    internal class Increment : Opcode
    {
        public override byte PaddingBytes => 0;
        private const byte _opcode = 0xFF; // Opcode for inc register 

        private int iterationCount; //The amount of chars already checked
        private int totalChars = 2; //The total amount of characters to check, we need 2(1 = inc, 2 = modR/m)

        public override bool RequireNextChar()
        {
            bool required = iterationCount++ < totalChars;
            if (!required)
            {
                iterationCount = 0; //Reset for next time
            }
            return required;
        }
        public override bool IsValid(char c)
        {
            return c == 'I';
        }

        public override byte OpcodeValue(char name)
        {
            if (RequireNextChar()) // If we are on the first char, return the base opcode
            {
                return _opcode;
            }
            else
            {
                byte opcode = 0xC0; // ModR/M byte for register direct addressing mode
                switch (name) // Get the register from the script
                {
                    case 'A': 
                        return (byte)(opcode + (byte)Registers.EAX);

                    case 'C': 
                        return (byte)(opcode + (byte)Registers.ECX);

                    case 'D': 
                        return (byte)(opcode + (byte)Registers.EDX);

                    case 'B': 
                        return (byte)(opcode + (byte)Registers.EBX);

                    case 'S': // ESP
                        return (byte)(opcode + (byte)Registers.ESP);

                    case 'P': // EBP
                        return (byte)(opcode + (byte)Registers.EBP);

                    case 'I': // ESI
                        return (byte)(opcode + (byte)Registers.ESI);

                    case 'L': // EDI
                        return (byte)(opcode + (byte)Registers.EDI);

                    default:
                        throw new ArgumentException(
                            $"Invalid register specified by the name '{name}'.");
                }
            }

            throw new NotImplementedException();

        }
    }
}
