using AlbanianXrm.EarlyBound.Properties;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

namespace AlbanianXrm.EarlyBound.Helpers
{
    internal class FilePathEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            var organizationOptions = context.Instance as OrganizationOptions;
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.ValidateNames = false;
                dialog.CheckFileExists = false;
                dialog.CheckPathExists = true;
                dialog.Filter = organizationOptions.Language == LanguageEnum.CS ? Resources.FILTER_C_SHARP : Resources.FILTER_VISUAL_BASIC;

                dialog.FileName = value as string;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    return dialog.FileName;
                }
            }
            return value;
        }
    }
}
