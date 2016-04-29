using System;
using System.IO;
using System.Windows.Forms;
using Zenwalker.BoxCop.ExportListViewReport;

namespace Zenwalker.BoxCop.AssemblyStaticAnalyzer
{
    public static class ExportOperation
    {
		#region Methods (2) 

		// Public Methods (1) 

        public static void Export(ListView listView, ExportingOption exportOption, string file)
        {
            try
            {
                ExportStrategy exportStrategy = GetExportStrategyObject(exportOption);

                ExportListView exportListView = new ExportListView(exportStrategy);

                exportListView.Export(listView, file);

                MessageBox.Show("Exporting Done", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (ArgumentException ex)
            {
                DisplayError.Display(ex);
            }
            catch (IOException ex)
            {
                DisplayError.Display(ex);
            }
        }
		// Private Methods (1) 

        private static ExportStrategy GetExportStrategyObject(ExportingOption exportOption)
        {
            ExportStrategy obj;

            switch(exportOption)
            {
                case ExportingOption.Csv:
                    obj = new ExportCsv();
                    break;
                default: throw new ArgumentException("Invalid export type specified = " + exportOption);
            }

            return obj;
        }

		#endregion Methods 
    }
}
