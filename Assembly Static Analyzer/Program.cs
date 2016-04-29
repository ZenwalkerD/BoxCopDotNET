using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;

namespace Zenwalker.BoxCop.AssemblyStaticAnalyzer
{
	static class Program
	{
		#region Methods (2) 

		// Private Methods (2) 

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			bool createdNew = true;
			using (Mutex mutex = new Mutex(true, "BoxCop", out createdNew))
			{
				if (createdNew)
				{
					Application.EnableVisualStyles();
					Application.SetCompatibleTextRenderingDefault(false);
					Application.Run(new BoxCopForm());
				}
				else
				{
					Process current = Process.GetCurrentProcess();
					foreach (Process process in Process.GetProcessesByName(current.ProcessName))
					{
						if (process.Id != current.Id)
						{
							SetForegroundWindow(process.MainWindowHandle);
							break;
						}
					}        
				}
			}
		}

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool SetForegroundWindow(IntPtr hWnd);

		#endregion Methods 
	}
}
