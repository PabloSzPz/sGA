using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace GSA {
    class LogTextBox {
        private RichTextBox _txtBox;
        private Paragraph _pgph;

        public LogTextBox(RichTextBox txtBox) {
            _txtBox = txtBox;
            _pgph = new Paragraph();
            _txtBox.Document.Blocks.Add(_pgph);
        }

        public void Write(string text, Brush color) {
            WriteLog(text, color);
        }
        public void Write(string text) {
            Brush brush = Brushes.White;
            WriteLog(text, brush);
        }

        private void WriteLog(string text, Brush color) {
            Bold myrun = new Bold();
            myrun.Inlines.Add(text);
            myrun.Foreground = color;

            _pgph.Inlines.Add(myrun);
            _pgph.Inlines.Add(Environment.NewLine);
        }
    }
}
