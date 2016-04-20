using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNetASM.Engine {

    class CIEvent : EventArgs {
        public int Instruction;
        public bool Found = false;

        public CIEvent(int ino) {
            this.Instruction = ino;
        } public CIEvent(BitVector32 ino) {
            this.Instruction = ino.Data;
        }

        
    }

    class AssemblyEngine {
        // Variables
        private string Name;
        private AssemblyParser parser;
        private Registers registers;

        public event CustomInstruction CustomInstructions;
        public CIEvent lastEvent = null;
        public delegate void CustomInstruction(AssemblyEngine eng, CIEvent e);

        public bool CIContainsEvents() {
            return (CustomInstructions != null);
        }

        public void ThowICEvent(CIEvent evt) {
            if (!CIContainsEvents()) return;
            this.lastEvent = evt;
            CustomInstructions(this, evt);
        }

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
