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

namespace dotNetASM.Forms {
    public partial class TestGUI : Form {
        public TestGUI() {
            InitializeComponent();
        }

        Engine.AssemblyEngine asm = new Engine.AssemblyEngine("Test");
        private void button1_Click(object sender, EventArgs e) {
            /*var txt = richTextBox1.Text.Split('\n');

            var operation = Regex.Matches(txt[0], @"[\""].+?[\""]|[^ ]+")
                               .Cast<Match>()
                               .Select(m => m.Value)
                               .ToArray();

            MessageBox.Show(string.Join("\n", operation));*/

            // First EVER TEST
            // LETS WATCH IT BE REALLY BAD!
            asm.getParser().RunScript(richTextBox1.Text);
        }
    }
}
