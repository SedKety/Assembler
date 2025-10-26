using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler
{
    internal abstract class Opcode
    {
        public abstract bool IsValid(char c); //Whether the char is valid for this opcode

        public abstract bool RequireNextChar(); //If the opcode require more then one byte

        public abstract byte OpcodeValue(char name); //The corrosponding opcode

        public abstract byte PaddingBytes { get; }



        //--------------------- Overwrite Previous Byte Methods/Fields ---------------------{
        public virtual bool OverWritePreviousByte { get; }//Whether to overwrite previous data

        public virtual int ByteCount() { return 1; } //Amount of bytes the opcode overrides
        
        public virtual byte OverwriteIterationByte() { return 0x00; } //Method to decide what char to use when overwriting previous bytes
        
    }
}
