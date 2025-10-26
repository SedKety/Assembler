using Compiler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assembler
{ 
    internal class OpcodeRuleEngine
    {
        private List<Opcode> opcodes = new List<Opcode>();
        private bool _initialized = false;

        private bool _commentsAllowed = true;
        public const char _commentChar = '#';

        public const int _skipByte = 0x69;

        private int pc = 0; //Program counter(at what character we are currently at)
        private int bc = 0; //Byte counter(at what byte we are currently at)

        private void Initialize()
        {
            opcodes.Add(new RegisterMov());
            opcodes.Add(new Digits());
            opcodes.Add(new Increment());
            _initialized = true;
        }

        public byte[] Translate(string script)
        {
            if (!_initialized) { Initialize(); }
            List<byte> machineCode = new List<byte>();


            if (_commentsAllowed)
            {
                Console.WriteLine("Comments: \n");
            }

            //Loops over every character in the script
            while (pc < script.Length)
            {
                //Gets the character at the current program counter
                char c = script[pc];
               
                //Check for whitespaces, skip them if found
                if(char.IsWhiteSpace(c))
                {
                    pc++;
                    continue; 
                }

                //Looks for comments
                if(_commentsAllowed && c == _commentChar)
                {
                    //Skip to the end of the line
                    while (pc < script.Length && script[pc] != '\n')
                    {
                        Console.Write(script[pc]);
                        pc++;
                    }
                    Console.Write('\n');
                    continue;
                }

                // Find an opcode which is valid for the current character
                var opcode = opcodes.FirstOrDefault(o => o.IsValid(c));
                if (opcode == null)
                {
                    throw new Exception($"No opcode for: {c}");
                }

                // Get the base opcode for the current character
                byte opcodeValue = opcode.OpcodeValue(c);

                //Add the opcode to the machine code if it's not a skip byte
                if (opcodeValue != _skipByte)
                {
                    if (opcode.OverWritePreviousByte)
                    {
                        // Overwrite the previous N bytes, starting from the most recent written bytes
                        for (int i = opcode.ByteCount(); i > 0; i--)
                        {
                            int targetIndex = bc - i; // overwrite existing bytes that were just written
                            if (targetIndex >= 0 && targetIndex < machineCode.Count)
                            {
                                machineCode[targetIndex] = opcode.OverwriteIterationByte();
                            }
                        }

                        // Now advance bc by the number of bytes logically "processed"
                        bc += 1;
                    }
                    else
                    {
                        AddToMachineCode(ref machineCode, opcodeValue);
                    }

                }

                // Handle multichar opcodes
                if (opcode.RequireNextChar())
                {
                    pc++; // Move to the next character
                    if (pc >= script.Length)
                    {
                        throw new Exception("Tried a multilayered opcode but the script ended before it could be completed");
                    }
                    char nextChar = script[pc];
                    byte nextOpcodeValue = opcode.OpcodeValue(nextChar);
                    AddToMachineCode(ref machineCode, nextOpcodeValue);
                }

                //Add padding bytes if necessary
                AddPaddingBytes(ref machineCode, opcode.PaddingBytes);

                pc++; // Move to the next instruction
            }


            for(int i = 0; i < machineCode.Count; i++)
            {
                var prntStr = $"Byte {i}: 0x{machineCode[i]:X2}";
                if (machineCode[i] == 0x00) { prntStr = "Empty"; }
                Console.WriteLine(prntStr);
            }
            return machineCode.ToArray();
        }
        private void AddToMachineCode(ref List<byte> machineCode, byte value)
        {
            bc++;
            machineCode.Add(value);
        }
        //Adds padding bytes(integer = 4 bytes, but we can only use bytes so instead of math we just pad them lol)
        public void AddPaddingBytes(ref List<byte> machineCode, int paddingBytes)
        {
            for (int i = 0; i < paddingBytes; i++)
            {
                AddToMachineCode(ref machineCode, 0x00); // Add padding bytes
            }
        }
    }
}
