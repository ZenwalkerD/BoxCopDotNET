using System.Windows.Forms;

namespace Zenwalker.BoxCop.ExportListViewReport
{
    public abstract class ExportStrategy
    {
        public abstract bool Export(ListView listView, string file);
    }
}
