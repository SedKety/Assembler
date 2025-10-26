using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler
{
    internal class Digits : Opcode
    {
        public override byte PaddingBytes => 0;

        public override bool OverWritePreviousByte => true;

        private int iterationCount = 0;

        private char _lastChar;

        //Total bytes to overwrite
        public override int ByteCount()
        {
            return 4; //We need 4 bytes to store an integer
        }

        //Check if the char is a digit
        public override bool IsValid(char c)
        {
            //If it's higher then 0 and lower then 9, it's a digit
            return c >= '0' && c <= '9';

        }

        //Get the opcode value for the digit(literally just the digit lmfao)
        public override byte OpcodeValue(char name)
        {
            _lastChar = name;
            iterationCount = 0; //Reset for overwriting
            return byte.Parse(name.ToString());
        }

        //Overwrite the previous bytes to form a full integer
        public override byte OverwriteIterationByte()
        {
            iterationCount++;
            if(iterationCount == 1) { return OpcodeValue(_lastChar); }
            if (iterationCount == 4)
            {
                iterationCount = 0; //Reset for next time
            }

            return 0x00;
        }
        public override bool RequireNextChar()
        {
            return false;
        }
    }
}
