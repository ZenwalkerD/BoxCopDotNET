using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Zenwalker.BoxCop.ExportListViewReport;

namespace Zenwalker.BoxCop.AssemblyStaticAnalyzer
{
	/// <summary>
	/// UI for the application.
	/// </summary>
	public partial class BoxCopForm : Form
	{
		#region Fields (3) 

		private const string _csvFilter = "csv files (*.csv)|*.csv";
		private string _stringAssembly;
		private const string _xmlFilter = "xml files (*.xml)|*.xml";

		#endregion Fields 

		#region Constructors (1) 

		/// <summary>
		/// Initializes a new instance of the <see cref="BoxCopForm"/> class.
		/// </summary>
		public BoxCopForm()
		{
			InitializeComponent();
			Trace.Listeners.Add(new TextWriterTraceListener(new FileStream(@"BoxCop.log", FileMode.Append)));
			Trace.AutoFlush = true;
		}

		#endregion Constructors 

		#region Methods (7) 

		// Private Methods (7) 

		/// <summary>
		/// Handles the Click event of the button1 control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void button1_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog fileDialog = new OpenFileDialog();
			fileDialog.Filter = "Assemblies | *.exe;*.dll";
			DialogResult dr = fileDialog.ShowDialog(this);
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                label2.Text = _stringAssembly = fileDialog.FileName;
                listView1.Items.Clear();
            }

		}

		/// <summary>
		/// Handles the Click event of the button2 control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void button2_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			listView1.Items.Clear();
			listView1.Enabled = false;
			try
			{
				AnalyzeAssembly analyzeAssem = new AnalyzeAssembly();

				FillDataGrid(analyzeAssem.Analyze(_stringAssembly));
			}
			catch (FileLoadException ex)
			{
				DisplayError.Display(ex);
			}
			catch (FileNotFoundException ex)
			{
				DisplayError.Display(ex);
			}
			catch (ArgumentNullException ex)
			{
				DisplayError.Display(ex);
			}
			catch (ApplicationException ex)
			{
				DisplayError.Display(ex);
			}
			catch (ReflectionTypeLoadException ex)
			{
				DisplayError.Display(ex);
			}
			catch (BadImageFormatException ex)
			{
				DisplayError.Display(ex);
			}
			finally
			{
				Cursor.Current = Cursors.Default;
				if (listView1.Items.Count > 0) listView1.Enabled = true;
			}
		}

		/// <summary>
		/// Displays the save dialog.
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns></returns>
		private string DisplaySaveDialog(string filter)
		{
			string file = null;

			SaveFileDialog fileDialog = new SaveFileDialog();
			fileDialog.Filter = filter;
			fileDialog.Title = "Save Report";
			fileDialog.CheckPathExists = true;

			fileDialog.FileName = "Report";
			DialogResult dr = fileDialog.ShowDialog(this);

			if (dr == System.Windows.Forms.DialogResult.OK)
				file = fileDialog.FileName;

			return file;
		}

		/// <summary>
		/// Handles the Click event of the exportToCsvToolStripMenuItem control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void exportToCsvToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string savedFile = DisplaySaveDialog(_csvFilter);

			if (!string.IsNullOrEmpty(savedFile))
				ExportOperation.Export(listView1, ExportingOption.Csv, savedFile);
		}

		/// <summary>
		/// Fills the data grid.
		/// </summary>
		/// <param name="analyzedDataList">The analyzed data list.</param>
		private void FillDataGrid(List<AnalyzedData> analyzedDataList)
		{
			int slNo = 1;
			foreach (var item in analyzedDataList)
			{
				var lvItem = new ListViewItem(slNo.ToString());

				lvItem.SubItems.Add(item.moduleClassName);
				lvItem.SubItems.Add(item.methodName);
				lvItem.SubItems.Add(item.sourceLine.ToString());
				lvItem.SubItems.Add(item.sourceColumn.ToString());
				lvItem.SubItems.Add(item.sourceCodeUrl);

				listView1.Items.Add(lvItem);
				slNo++;
			}
			listView1.Update();            
		}

		/// <summary>
		/// Handles the FormClosing event of the Form1 control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.Forms.FormClosingEventArgs"/> instance containing the event data.</param>
		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			Trace.Close();
			this.Dispose();
		}

        /// <summary>
        /// Handles the Click event of the pictureBox1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string helpLink = ConfigurationManager.AppSettings["Help Link"];

            try
            {
                Cursor.Current = Cursors.AppStarting;
                Process.Start(helpLink);
            }
            catch (InvalidOperationException ex)
            {
                DisplayError.Display(ex);
            }
            catch (Win32Exception ex)
            {
                DisplayError.Display(ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

		#endregion Methods 
	}
}
