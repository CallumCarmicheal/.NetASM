using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNetASM.Engine {
    class AssemblyParser {
        AssemblyEngine eng;

        public AssemblyParser(AssemblyEngine engine) {
            this.eng = engine;
        }

        // TEMPORARY VALUES
        public int tmpStack;
        public int CurrentLine = 0;
        public bool Executing = false;

        public void RunScript(string Script) {

            string[] Lines = Script.Split('\n');

            do {
                string instruction = Lines[CurrentLine].Trim();

                if (instruction.EndsWith(":")) {
                    // This is a GOTO Label
                    CurrentLine++;
                    continue;
                }

                string removeComment = instruction.Split(';')[0];
                string[] operation = removeComment.Trim().Split(' ');

                for(int x = 0; x < operation.Length-1; x++) {
                    // Skip hex location
                    if (operation[x].Contains("0x"))
                        continue;

                    // Skip numerals
                    if (operation[x].IsNumeric(System.Globalization.NumberStyles.HexNumber) || operation[x].IsNumeric(System.Globalization.NumberStyles.Number))
                        return;

                    operation[x] = operation[x].ToUpper();
                }

                int iSize = operation.Length;

                if (iSize == 1) {
                    // Single Instruction's EG. NOP

                    switch (operation[0]) {
                    case "NOP":
                        break;
                    }
                } else {
                    // Everything Else

                    switch (operation[0]) {
                    case "PUSH":
                        var stackVal = operation[1];

                        if(!stackVal.IsNumeric()) {
                            eng.ThrowParseError(ASMERROR_TYPE.PARSE_ERROR, removeComment, "PUSH expecting Int32, GOT " + stackVal, CurrentLine, 1);
                        }
                        
                        int value = (int)new System.ComponentModel.Int32Converter().ConvertFromString(stackVal);
                        tmpStack = value;

                        break;
                    case "POP": 
                        string REG = operation[1];
                        if (!eng.getRegisters().isRegister(REG)) {
                            Executing = false;
                            // Stop Execution
                            eng.ThrowParseError(ASMERROR_TYPE.PARSE_ERROR, removeComment, "POP expecting Register, GOT " + REG, CurrentLine, 1);
                        }

                        // Set the register
                        eng.getRegisters().SetRegister(REG, tmpStack);

                        break;


                    case "JMP":

                        bool isGOTOSymbol = false;  // EG. blah
                        bool isLine = false;        // EG. 0
                        bool isOffset = false;      // EG. 0x{8} this will only be used when compiled, NOTE THAT



                        break;
                    }

                }

                CurrentLine++;
                if (CurrentLine + 1 <= Lines.Length)
                    Executing = false;
            } while (Executing);

            tmpStack = 0;
            CurrentLine = 0;

            eng.DoneExecuting();
        }
    }
}
