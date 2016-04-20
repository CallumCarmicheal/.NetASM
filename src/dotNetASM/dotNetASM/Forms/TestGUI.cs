using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScintillaNET;

namespace dotNetASM.Forms {
    public partial class TestGUI : Form {
        private int maxLineNumberCharLength;

        public bool UseHexMargin = false;
        Engine.AssemblyEngine asm = new Engine.AssemblyEngine("Test");

        public TestGUI() {
            InitializeComponent();


            /* Styling */ {
                scintilla.StyleResetDefault();
                scintilla.Styles[Style.Default].Font = "Consolas";
                scintilla.Styles[Style.Default].Size = 10;
                scintilla.Styles[Style.Default].BackColor = Color.FromKnownColor(KnownColor.Control);
                scintilla.StyleClearAll();

                scintilla.Styles[ScintillaNET.Style.Asm.Comment].ForeColor = Color.Green;
                scintilla.Styles[ScintillaNET.Style.Asm.String].ForeColor = Color.FromArgb(0xFF6F00);

                // Text: Blue, Background: White
                scintilla.Styles[ScintillaNET.Style.Asm.CpuInstruction].ForeColor = Color.FromArgb(0x303F9F);

                // Text: Green, Background: Grey
                scintilla.Styles[ScintillaNET.Style.Asm.Register].BackColor = Color.FromArgb(0x455A64);
                scintilla.Styles[ScintillaNET.Style.Asm.Register].ForeColor = Color.FromArgb(0x00C853);

                // Text: Blue, Background: X
                scintilla.Styles[ScintillaNET.Style.Asm.DirectiveOperand].ForeColor = Color.FromArgb(0x1A237E);

                // Text: Red, Background: X
                scintilla.Styles[ScintillaNET.Style.Asm.Identifier].ForeColor = Color.FromArgb(0xF44336);

                scintilla.Lexer = ScintillaNET.Lexer.Asm;
            }

            /* Key Words */ {
                string keywords_0 = "NOP END PUSH POP JMP CMP MOV ADD MSG DEC INC SUB INT ";
                string keywords_2 = "EAX AX AH AL EBX BX BH BL EXC CX CH CL EDX DX DH DL ESI EDI ESP ESI ";
                string keywords_4 = "$ % ";
                scintilla.SetKeywords(0, keywords_0 + keywords_0.ToLower());
                scintilla.SetKeywords(2, keywords_2 + keywords_2.ToLower());
                scintilla.SetKeywords(4, keywords_4 + keywords_4.ToLower());
            }

            /* Other */ {
                // Margin
                scintilla.Margins[0].Type = MarginType.RightText;
                scintilla.Margins[0].Width = 35;

                // Scroll Width
                scintilla.ScrollWidth = scintilla.Width - 40;

                // Margin
                UpdateLineNumbers(0);
            }

            /* Custom Instructions */ {
                asm.CustomInstructions += Asm_CustomInstructions;
            }
        }

        private void Asm_CustomInstructions(Engine.AssemblyEngine eng, Engine.CIEvent e) {
            // If another Event has found the instruction
            // Just ignore it
            if (e.Found) return;

            // 0 - F is reserved, Please follow this rule
            // It allows extra functionality
            int BASE_ADDR = 0xF + 1; // A

            // ADDRESS OF FUNC - Description
            // BASE_ADDR + 0x0 = Message Hello
            // BASE_ADDR + 0x1 = Display ESI register

            int ADDR_HELLO = BASE_ADDR + 0x0; // A
            int ADDR_ESI   = BASE_ADDR + 0x1; // B

            // ENTRY + Our Function Address
            if (e.Instruction == ADDR_HELLO) {
                MessageBox.Show("Hello");
                e.Found = true; // We found the instruction so cancel the evt
            } else if (e.Instruction == ADDR_ESI) { // B
                MessageBox.Show(eng.getRegisters().ESI.Data + "");
                e.Found = true; // We found the instruction so cancel the evt
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            /*var txt = richTextBox1.Text.Split('\n');

            var operation = Regex.Matches(txt[0], @"[\""].+?[\""]|[^ ]+")
                               .Cast<Match>()
                               .Select(m => m.Value)
                               .ToArray();

            MessageBox.Show(string.Join("\n", operation));*/

            // First EVER TEST
            // LETS WATCH IT BE REALLY BAD!
            asm.getParser().RunScript(scintilla.Text);
        }

        private void UpdateLineNumbers(int startingAtLine) {
            // Starting at the specified line index, update each
            // subsequent line margin text with a hex line number.
            for (int i = startingAtLine; i < scintilla.Lines.Count; i++) {
                scintilla.Lines[i].MarginStyle = Style.LineNumber;

                if(UseHexMargin)
                    scintilla.Lines[i].MarginText = "" + i.ToString("X2");
                else scintilla.Lines[i].MarginText = "" + i;
            }
        }
        
        private void scintilla_Insert(object sender, ModificationEventArgs e) {
            // Only update line numbers if the number of lines changed
            if (e.LinesAdded != 0) 
                UpdateLineNumbers(scintilla.LineFromPosition(e.Position));
        }

        private void scintilla_Delete(object sender, ModificationEventArgs e) {
            // Only update line numbers if the number of lines changed
            if (e.LinesAdded != 0)
                UpdateLineNumbers(scintilla.LineFromPosition(e.Position));
        }

        private void scintilla_Resize(object sender, EventArgs e) {
            scintilla.ScrollWidth = scintilla.Width - 40;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            UseHexMargin = checkBox1.Checked;
            UpdateLineNumbers(0);
        }

        private void button2_Click(object sender, EventArgs e) {

            openFileDialog1.DefaultExt = "*.asm";
            openFileDialog1.FileName = "SUPER SECRET HACKING DEVICE.asm";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.Filter = "Assembly Files (*.asm)|*.asm";
            openFileDialog1.Title = "Please open the file you wish to use to hack the world * I mean run";
            var res = openFileDialog1.ShowDialog();

            if(res == DialogResult.OK) {
                string file = openFileDialog1.FileName;
                scintilla.Text = System.IO.File.ReadAllText(file);
            }
        }
    }
}
