using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace dotNetASM.Engine {
    struct CMPResults {
        public int  X;
        public int  Y;

        public int  TTL;
        public bool isValid;
    }

    class AssemblyParser {
        AssemblyEngine eng;

        public AssemblyParser(AssemblyEngine engine) {
            this.eng = engine;
        }

        // TEMPORARY VALUES
        public int              tmpStack;
        public int              CurrentLine = 0;
        public bool             Executing   = false;
        public bool             tmpB        = false;
        public CMPResults       cmpValue    = default(CMPResults);

        public bool ScriptContainsDuplicateLabels(string Script) {
            List<string> labels = new List<string>();


            string[] Lines = Script.Split('\n');

            for (int x = 0; x < Lines.Length - 1; x++) {
                string line = Lines[x].Trim();
                string instruction = line.Split(';')[0].Trim();

                // Is Label
                if (instruction.EndsWith(":")) {
                    var label = instruction.Replace(":", "");
                    if (labels.Contains(label))
                        return true;
                    else labels.Add(label);
                }
            } return false;
        }

        public int FindLabelLocation(string Script, string Label) {
            if (ScriptContainsDuplicateLabels(Script))
                return -1;

            string[] Lines = Script.Split('\n');
            for (int x = 0; x < Lines.Length-1; x++) {
                string line = Lines[x].Trim();
                string instruction = line.Split(';')[0].Trim();
                
                // Is Label
                if(instruction.EndsWith(":")) {
                    var label = instruction.Replace(":", "");
                    if (label == Label)
                        return x+1;
                }
            } return -1;
        }

        public bool argumentIsString(string x) {
            return (x.StartsWith("\"") && x.EndsWith("\""));
        }
        
        public void RunScript(string Script) {
            string[] Lines = Script.Split('\n');

            // Setup Parser
            Executing = true;
            tmpStack = 0;
            CurrentLine = 0;

            // Setup default CMP
            cmpValue = default(CMPResults);
            cmpValue.isValid = false;
            cmpValue.TTL = 2;

            if (ScriptContainsDuplicateLabels(Script)) {
                eng.ParserLog("SCRIPT", "Script contains multiple same labels!", 2);
                Executing = false;
            }

            do {
                if (CurrentLine + 1 >= Lines.Length) {
                    Executing = false;
                    continue;
                }

                string currentOperation = Lines[CurrentLine].Trim();

                // Check for whitespace
                if(string.IsNullOrWhiteSpace(currentOperation) || string.IsNullOrEmpty(currentOperation)) {
                    CurrentLine++;
                    continue;
                }

                // Check for comment
                if (currentOperation.StartsWith(";")) {
                    CurrentLine++;
                    continue;
                }

                // Check for label
                if (currentOperation.EndsWith(":")) {
                    CurrentLine++;
                    continue;
                }

                currentOperation = currentOperation.Split(';')[0].Replace('\'', '\"').Trim();

                var operation = Regex.Matches(currentOperation, @"[\""].+?[\""]|[^ ]+")
                                .Cast<Match>()
                                .Select(m => m.Value)
                                .ToArray();

                string instruction = operation[0];
                eng.ParserLog("Exec", currentOperation + " @ " + CurrentLine + " -  Started", 0);

                if (instruction.EndsWith(":")) {
                    // This is a GOTO Label
                    CurrentLine++;
                    continue;
                }

                // FORCE ALL CHARS TO UPPER
                for(int x = 0; x < operation.Length-1; x++) {
                    // Skip hex location
                    if (operation[x].Contains("0x"))
                        continue;

                    // Skip numerals
                    if (operation[x].IsNumeric(
                        System.Globalization.NumberStyles.Integer) 
                        || operation[x].IsNumeric(System.Globalization.NumberStyles.Number))

                        return;

                    operation[x] = operation[x].ToUpper();
                }

                int iSize = operation.Length;
                if (iSize == 1) {
                    // Single Instruction's EG. NOP

                    switch (operation[0]) {
                    case "NOP":
                        CurrentLine++;
                        continue;


                    /* FAKE COMMANDS | NOT REAL ASM COMMANDS BUT ADDED BECAUSE IT MAKES MY LIFE EASIER */
                    // Usage
                    //      END
                    case "END":
                        Executing = false;
                        break;
                    }
                } else {
                    // Everything Else

                    switch (operation[0]) {
                    case "PUSH":
                        var stackVal = operation[1];

                        if (eng.getRegisters().isRegister(stackVal)) {
                            int value = eng.getRegisters().getRegister(stackVal, ref tmpB).Data;
                            tmpStack = value;
                            eng.ParserLog(currentOperation, "Set stack value to Register[" + stackVal + "] = " + value);
                        } else  if (!stackVal.IsNumeric()) {
                            eng.ThrowParseError(ASMERROR_TYPE.PARSE_ERROR, currentOperation, "PUSH expecting Int/Hex/Register, GOT " + stackVal, CurrentLine, 1);
                            break;
                        } else {
                            int value = (int)new System.ComponentModel.Int32Converter().ConvertFromString(stackVal);
                            tmpStack = value;
                            eng.ParserLog(currentOperation, "Set stack value to " + value);
                        }

                        break;
                    case "POP":
                        string REG = operation[1];
                        if (!eng.getRegisters().isRegister(REG)) {
                            Executing = false;
                            eng.ThrowParseError(ASMERROR_TYPE.PARSE_ERROR, currentOperation, "POP expecting Register, GOT " + REG, CurrentLine, 1);
                            break;
                        }

                        // Set the register
                        eng.getRegisters().SetRegister(REG, tmpStack);

                        break;
                    case "JMP":
                        string loc = operation[1];

                        if (!loc.IsNumeric()) {
                            // Find Name/Label EG. Blah
                            int label = FindLabelLocation(Script, loc);

                            if (label != -1) {
                                CurrentLine = label-1;
                                eng.ParserLog(currentOperation, "Changing line to " + loc + "@" + label + "+1");
                            } else {
                                Executing = false;
                                eng.ThrowParseError(ASMERROR_TYPE.PARSE_ERROR, currentOperation, "JMP expecting Label, NONE FOUND", CurrentLine, 1);
                                break;
                            }
                        } else if (loc.IsNumeric()) {
                            // File Location, Line EG. 5
                            // Set tot he name loction because the current line is incremented
                            CurrentLine = (int)new System.ComponentModel.Int32Converter().ConvertFromString(loc) - 2; // Evaluated as 4 -2 = 3 (Line 4)  (-2 because Index at 0 and 1 is added at the end of the parse)
                            eng.ParserLog(currentOperation, "Changing line to " + CurrentLine);
                        } else if (loc.StartsWith("0x")) {
                            // Binary File Location (COMPILED ONLY) EG. 0x3F
                            // IGNORED FOR NOW
                            eng.ParserLog(currentOperation, "Ignoring hex jmp (BINARY JUMP)");
                        }
                        break;

                    case "CMP":
                        string arg = operation[1];
                        if (operation.Length - 1 == 2)
                            arg += "," + operation[2];
                        arg = arg.Replace(" ", "");

                        var values = arg.Trim().Split(',');
                        values[0] = values[0].Trim();
                        values[1] = values[1].Trim();
                        if (values.Length == 3)
                            values[1] = values[2].Trim();


                        int[] dec = new int[2];

                        // EAX,0xF
                        // EAX,3
                        // EAX,EDX
                        // 0xF,3
                        // -------
                        // 0xF,EAX
                        // 3,EAX
                        // EDX,EAX
                        // 3,0xF

                        if (eng.getRegisters().isRegister(values[0])) // use tmpB because we already know its a valid register
                            dec[0] = eng.getRegisters().getRegister(values[0], ref tmpB).Data;
                        else
                            dec[0] = (int)new System.ComponentModel.Int32Converter().ConvertFromString(values[0]);

                        if (eng.getRegisters().isRegister(values[0])) // use tmpB because we already know its a valid register
                            dec[1] = eng.getRegisters().getRegister(values[0], ref tmpB).Data;
                        else
                            dec[1] = (int)new System.ComponentModel.Int32Converter().ConvertFromString(values[0]);

                        // Setup our tmp CMP buffer
                        cmpValue.X = dec[0];
                        cmpValue.Y = dec[1];
                        cmpValue.isValid = true;
                        cmpValue.TTL = 2;

                        eng.ParserLog(currentOperation, string.Format("cmp {0}, {1}", cmpValue.X, cmpValue.Y));
                        break;

                    case "MOV":
                        string arg_ = operation[1];
                        if (operation.Length - 1 == 2)
                            arg_ += "," + operation[2];
                        arg_ = arg_.Replace(" ", "");

                        var v = arg_.Trim().Split(',');
                        v[0] = v[0].Trim();
                        v[1] = v[1].Trim();

                        if (v.Length == 3)
                            v[1] = v[2].Trim();

                        Registers reg = eng.getRegisters(); // Easier to debug
                        

                        if (eng.getRegisters().isRegister(v[2])) {
                            // mov EAX, EDX
                            // [0] EAX
                            // [1] EDX
                            eng.getRegisters().CopyRegister(v[0], v[1]);


                            string output = "Copying Register[";
                            output += v[1];
                            output += "]=(";
                            output += "" + eng.getRegisters().getRegister(v[1], ref tmpB).Data;
                            output += ")";
                            output += " value to Register[";
                            output += v[0];
                            output += "]";

                            eng.ParserLog(currentOperation, output);
                        } else {
                            int __SetValue = (int)new System.ComponentModel.Int32Converter().ConvertFromString(operation[2]);
                            
                            eng.getRegisters().SetRegister(v[0], __SetValue);
                            int registerValue = eng.getRegisters().getRegister(v[0], ref tmpB).Data;
                            eng.ParserLog(currentOperation, string.Format("Setting Register[{0}] value to {1}", v[0], registerValue));
                        }
                        break;

                    case "ADD":
                        // add x, y
                        // add ESI, EDX
                        // add ESI, 3
                        // add ESI, 0x3f

                        string arg__ = operation[1];
                        if (operation.Length - 1 == 2)
                            arg__ += "," + operation[2];
                        arg__ = arg__.Replace(" ", "");

                        var v_ = arg__.Trim().Split(',');
                        v_[0] = v_[0].Trim();
                        v_[1] = v_[1].Trim();

                        if (v_.Length == 3)
                            v_[1] = v_[2].Trim();

                        // ARG 0 HAS TO BE A REGISTER
                        if(!eng.getRegisters().isRegister(v_[0])) {
                            Executing = false;
                            eng.ThrowParseError(ASMERROR_TYPE.PARSE_ERROR, currentOperation, "ADD expecting Register, GOT " + v_[0], CurrentLine, 1);
                            break;
                        }

                        int valueToAdd = 0;
                        bool isRegister = eng.getRegisters().isRegister(v_[1]);

                        if (!isRegister)
                            valueToAdd = (int)new System.ComponentModel.Int32Converter().ConvertFromString(v_[1]);
                        else valueToAdd = eng.getRegisters().getRegister(v_[1], ref tmpB).Data;

                        // Add val to register
                        eng.getRegisters().AddToRegister(v_[0], valueToAdd);

                        int newvalue = eng.getRegisters().getRegister(v_[0], ref tmpB).Data;
                        if (!isRegister)
                            eng.ParserLog(currentOperation, string.Format("Added {1} to Register[{0}] : New Value = {2}", v_[0], valueToAdd, newvalue));
                        else eng.ParserLog(currentOperation, string.Format("Added Register[{0}]={1} to Register[{2}] : New value = {3}", v_[1], valueToAdd, v_[0], newvalue));
                        break;


                    /* FAKE COMMANDS | NOT REAL ASM COMMANDS BUT ADDED BECAUSE IT MAKES MY LIFE EASIER */

                    // Args = 0;
                    // Usage
                    //      MSG 
                    //          title,              message
                    //          register/value,     register/value
                    //          ----------------------------------
                    //          message
                    //          register/value
                    case "MSG":
                        string val0 = "";
                        string val1 = "";

                        string _arg_ = "";
                        var _v = new string[3];

                        bool isBypass = false;

                        if (operation.Length != 4) {
                            _arg_ = operation[1];
                            if (operation.Length - 1 == 2)
                                _arg_ += "," + operation[2];
                            _arg_ = _arg_.Replace(", ", ",");

                            _v = _arg_.Trim().Split(',');
                            _v[0] = _v[0].Trim();
                            _v[1] = _v[1].Trim();

                            if (_v.Length == 3)
                                _v[1] = _v[2].Trim();
                        } else {
                            val1 = operation[1].Replace("\"", "");
                            val0 = operation[3].Replace("\"", "");

                            isBypass = true;
                        }

                        if (eng.getRegisters().isRegister(_v[0]))
                            val0 = "" + eng.getRegisters().getRegister(_v[0], ref tmpB).Data;

                        if (operation.Length - 1 == 2 || isBypass) {
                            if(!isBypass)
                                if (eng.getRegisters().isRegister(_v[1]))
                                    val1 = "" + eng.getRegisters().getRegister(_v[1], ref tmpB).Data;
                                else
                                    val1 = operation[2];

                            System.Windows.Forms.MessageBox.Show(val0, val1);
                        } else {
                            System.Windows.Forms.MessageBox.Show(val0);
                        }
                        break;

                    default:
                        break;
                    }
                }

                // Setup CMP TTL
                if (cmpValue.isValid)
                    if (cmpValue.TTL != 0)
                        cmpValue.TTL--;

                if (cmpValue.TTL == 0)
                    cmpValue.isValid = false;

                CurrentLine++;
                if (CurrentLine + 1 >= Lines.Length) // wrong operator.....
                    Executing = false;
            } while (Executing);

            tmpStack = 0;
            CurrentLine = 0;

            eng.DoneExecuting();
        }
    }
}
