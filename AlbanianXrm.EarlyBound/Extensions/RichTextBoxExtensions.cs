using System;
using System.Drawing;
using System.Windows.Forms;

namespace AlbanianXrm.EarlyBound.Extensions
{
    public static class RichTextBoxExtensions
    {
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            if (box == null) throw new ArgumentNullException(nameof(box));

            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }
    }
}
