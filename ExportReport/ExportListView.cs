using System;
using System.Windows.Forms;

namespace Zenwalker.BoxCop.ExportListViewReport
{
    public class ExportListView
    {
        private ExportStrategy _exportingStrategy;

        public ExportListView(ExportStrategy exportStrategy)
        {
            if (exportStrategy == null)
                throw new ArgumentNullException("ExportStrategy object can not be null");
            _exportingStrategy = exportStrategy;
        }

        public bool Export(ListView listView, string file)
        {
            return _exportingStrategy.Export(listView, file);
        }
    }
}
