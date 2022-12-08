using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

namespace AlbanianXrm.EarlyBound.Helpers
{
    internal class FolderPathEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
                dialog.SelectedPath = string.IsNullOrEmpty(value as string) ? OrganizationOptions.Defaults.OutputFolder : value as string;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    return dialog.SelectedPath;
                }
            }
            return value;
        }
    }
}
