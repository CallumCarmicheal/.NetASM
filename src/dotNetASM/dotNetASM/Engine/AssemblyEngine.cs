using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNetASM.Engine {
    class AssemblyEngine {
        // Variables
        private string Name;
        private AssemblyParser parser;
        private Registers registers;


        // Constructor
        public AssemblyEngine(string EngineName) {
            this.parser = new AssemblyParser(this);
            this.registers = new Registers(this);
        }

        // Getter - Setters
        public AssemblyParser getParser() {
            return this.parser;
        }

        public Registers getRegisters() {
            return this.registers;
        }

        // Methods and Functions
        public void ParserLog(string Instruction, string Message, int Level = 0) {
            if (System.Diagnostics.Debugger.IsAttached)
                System.Diagnostics.Debugger.Log(Level, "PARSER", "[" + Instruction + "] - " + Message + "\n");
        }
        
        public void ResetRegisters() {
            registers = new Registers(this);
        }

        public void RunScript(string ScriptText) {
            parser.RunScript(ScriptText);
        }

        public void ThrowParseError(ASMERROR_TYPE TYPE, string STRLine, string Message, int Line, int ArgIndex) {
            // Now how am i gonna do this ...
            // Events maybe
            this.ParserLog("ERROR", TYPE.ToString() + ": [" + STRLine + "]:" + Line + ":" + ArgIndex + " - " + Message);
        }

        public void DoneExecuting() {
            // Create event
            this.ParserLog("DONE", "DONE ASSEMBLY");
        }
    }
}
