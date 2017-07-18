using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Tomighty.Windows
{
    public class BaseWindow : Form
    {
        public BaseWindow()
        {
            base.Font = SystemFonts.MessageBoxFont;
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Font Font
        {
            get => base.Font;
            set => base.Font = value;
        }
    }
}
