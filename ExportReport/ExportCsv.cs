using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Zenwalker.BoxCop.ExportListViewReport
{
    public class ExportCsv : ExportStrategy
    { 
        public override bool Export(ListView listView, string file)
        {
            bool isExported = false;

            StringBuilder sb = new StringBuilder();

            foreach (ColumnHeader ch in listView.Columns)
            {
                sb.Append(ch.Text + ",");
            }
            sb.AppendLine();

            foreach (ListViewItem lvi in listView.Items)
            {
                foreach (ListViewItem.ListViewSubItem lvs in lvi.SubItems)
                {
                    if (lvs.Text.Trim() == string.Empty)
                        sb.Append(" ,");
                    else
                        sb.Append(lvs.Text + ",");
                }
                sb.AppendLine();
            }

            using (StreamWriter sw = new StreamWriter(file))
            {
                sw.Write(sb.ToString());
                isExported = true;
            }

            return isExported;
        }
    }
}
