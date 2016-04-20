using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNetASM.Engine {
    class Registers {
        AssemblyEngine engine;

        /* Notes for my Dumb self!
               {} = Register
               E{}X is the full 32-bit value
               {}X is the lower 16-bits
               {}L is the lower 8 bits
               {}H is the bits 8 through 15 (zero-based)

           SHORTCUT TO LOWER REGISTERS
            else {
                    // HONESTLY NOT A CLUE HOW TO DO THESE
                    // TODO: CREATE THE SUB RANGE REGISTERS

                    // THE AX, BX, STUFF HERE

                    switch(Register) {
                    // A Registers
                    case "AX":
                        break;
                    case "AH":
                        break;
                    case "AL":
                        break;

                    // B Registers
                    case "BX":
                        break;
                    case "BH":
                        break;
                    case "BL":
                        break;

                    // C Registers
                    case "CX":
                        break;
                    case "CH":
                        break;
                    case "CL":
                        break;

                    // D Registers
                    case "DX":
                        break;
                    case "DH":
                        break;
                    case "DL":
                        break;
                    }
                }


            SHORTCUT TO 32BIT REGISTERS
            switch (Register) {
                case "EAX":
                    
                    break;
                case "EBX":
                    break;
                case "ECX":
                    break;
                case "EDX":
                    break;
                case "ESI":
                    break;
                case "EDI":
                    break;
                case "ESP":
                    break;
                case "EBP":
                    break;
                }
            */


        // 32bit
        public BitVector32 EAX;
        public BitVector32 EBX;
        public BitVector32 ECX;
        public BitVector32 EDX;
        public BitVector32 ESI;
        public BitVector32 EDI;
        public BitVector32 ESP;
        public BitVector32 EBP;
        
        public Registers(AssemblyEngine engine) {
            this.engine = engine;
        }

        public void ResetRegisters() {
            EAX = new BitVector32();
            EBX = new BitVector32();
            ECX = new BitVector32();
            EDX = new BitVector32();
            EDI = new BitVector32();
            ESP = new BitVector32();
            EBP = new BitVector32();
        }


       
        // Split register is like EAH, EAL
        public void SetRegister(string Register, int Value) {
            // Expecting a 2-3 char string
            var len = Register.Length;

            // Create BV32 for the full 32bit Registers
            BitVector32 bits = new BitVector32(Value);

            if (len == 3) {
                // DO THE 32Bit STUFF HERE
                switch(Register) {
                case "EAX":
                    EAX = bits;
                    break;
                case "EBX":
                    EBX = bits;
                    break;
                case "ECX":
                    ECX = bits;
                    break;
                case "EDX":
                    EDX = bits;
                    break;
                case "ESI":
                    ESI = bits;
                    break;
                case "EDI":
                    EDI = bits;
                    break;
                case "ESP":
                    ESP = bits;
                    break;
                case "EBP":
                    EBP = bits;
                    break;
                }
            } 
        }

        public void SetRegister(string Register, BitVector32 bits) {
            // Expecting a 2-3 char string
            var len = Register.Length;

            if (len == 3) {
                // DO THE 32Bit STUFF HERE
                switch (Register) {
                case "EAX":
                    EAX = bits;
                    break;
                case "EBX":
                    EBX = bits;
                    break;
                case "ECX":
                    ECX = bits;
                    break;
                case "EDX":
                    EDX = bits;
                    break;
                case "ESI":
                    ESI = bits;
                    break;
                case "EDI":
                    EDI = bits;
                    break;
                case "ESP":
                    ESP = bits;
                    break;
                case "EBP":
                    EBP = bits;
                    break;
                }
            }
        }

        public BitVector32 getRegister(string Register, ref bool valid) {
            var len = Register.Length;

            valid = true;
            if (len == 3) {
                // DO THE 32Bit STUFF HERE
                switch (Register) {
                case "EAX":
                    return EAX;
                case "EBX":
                    return EBX;
                case "ECX":
                    return ECX;
                case "EDX":
                    return EDX;
                case "ESI":
                    return ESI;
                case "EDI":
                    return EDI;
                case "ESP":
                    return ESP;
                case "EBP":
                    return EBP;
                }
            }

            valid = false;
            // Invalid Register
            return default(BitVector32);
        }

        public void AddToRegister(string Register, int Value) {
            // Expecting a 2-3 char string
            var len = Register.Length;

            bool useless = false;
            BitVector32 bits;

            if (len == 3) {
                // DO THE 32Bit STUFF HERE
                switch (Register) {
                case "EAX":
                    bits = new BitVector32(getRegister(Register, ref useless).Data + Value);
                    EAX = bits;
                    break;
                case "EBX":
                    bits = new BitVector32(getRegister(Register, ref useless).Data + Value);
                    EBX = bits;
                    break;
                case "ECX":
                    bits = new BitVector32(getRegister(Register, ref useless).Data + Value);
                    ECX = bits;
                    break;
                case "EDX":
                    bits = new BitVector32(getRegister(Register, ref useless).Data + Value);
                    EDX = bits;
                    break;
                case "ESI":
                    bits = new BitVector32(getRegister(Register, ref useless).Data + Value);
                    ESI = bits;
                    break;
                case "EDI":
                    bits = new BitVector32(getRegister(Register, ref useless).Data + Value);
                    EDI = bits;
                    break;
                case "ESP":
                    bits = new BitVector32(getRegister(Register, ref useless).Data + Value);
                    ESP = bits;
                    break;
                case "EBP":
                    bits = new BitVector32(getRegister(Register, ref useless).Data + Value);
                    EBP = bits;
                    break;
                }
            }
        }

        public void AddRegisters(string Register, string RegisterToAdd) {
            int len1 = Register.Length,
                len2 = RegisterToAdd.Length;
            BitVector32 output;            

            int Data = 0;

            // Cant add
            if (len2 < len1)
                return;
            
            // HANDLE 32 BIT REGISTERS
            if(len1 == 3) {
                bool exists = false;
                var vec = getRegister(Register, ref exists);

                if (exists)
                    Data = vec.Data;
                else return;
            } if(len2 == 3) {
                bool exists = false;
                var vec = getRegister(Register, ref exists);

                if (exists)
                    Data += vec.Data;
                else return;
            }

            output = new BitVector32(Data);
            SetRegister(Register, output);
        }


        public void SubRegister(string Register, int Value) {
            Value *= -1;
            AddToRegister(Register, Value);
        }


        public void SubRegisters(string Register, int Register2) {
            bool useless = false;
            var register = getRegister(Register, ref useless);
            AddToRegister(Register, register.Data * -1);
        }

        public void CopyRegister(string destination, string value) {
            bool valid = false;
            BitVector32 val = getRegister(value, ref valid);
            if (!valid)
                return;

            SetRegister(destination, val);
        }

        public bool isRegister(string str) {
            string[] array;
            array = new string[] {
                "EAX",      "EBX",                "ECX",      "EDX",                "ESI",      "EDI",                "ESP",      "EBP"
            };

            if (str == null)
                return false;
            return array.Contains(str.ToUpper());
        }
    }
}
