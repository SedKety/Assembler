using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler
{
    internal class RegisterMov : Opcode
    {
        public override byte PaddingBytes { get => 4; } //MOV reg, imm32 takes 4 bytes(1 int)  so we return 4 padding bytes
        public override bool OverWritePreviousByte => false;

        private int iterationCount; //The amount of chars already checked
        private int totalChars = 2; //The total amount of characters to check, we need 2(for the mov instruction, and then the register)

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
            if(c == 'M') //First char must be M
            {
                return true;
            }
            return false;
        }

        private const byte opcode = 0xB8; // Opcode for mov into register(+ register index)

        public override byte OpcodeValue(char name)
        {
            if(RequireNextChar()) // If we are on the first char, return the base opcode
            {
                return OpcodeRuleEngine._skipByte;
            }
            else
            {
                switch (name) // Get the register from the script
                {
                    case 'A':
                        return opcode + (byte)Registers.EAX;
                    case 'C':
                        return opcode + (byte)Registers.ECX;
                    case 'D':
                        return opcode + (byte)Registers.EDX;
                    case 'B':
                        return opcode + (byte)Registers.EBX;
                    case 'S':
                        return opcode + (byte)Registers.ESP;
                    case 'P':
                        return opcode + (byte)Registers.EBP;
                    case 'I':
                        return opcode + (byte)Registers.ESI;
                    case 'L':
                        return opcode + (byte)Registers.EDI;
                    default:
                        throw new Exception($"Invalid register specified by the name of: {name}");
                }
            }
        }
    }
}
