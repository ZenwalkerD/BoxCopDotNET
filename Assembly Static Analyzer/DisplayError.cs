using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace Zenwalker.BoxCop.AssemblyStaticAnalyzer
{
	/// <summary>
	/// Displays all kinds of error message in Messagebox as well as logs it.
	/// </summary>
	public static class DisplayError
	{
		#region Methods (5) 

		// Public Methods (3) 

		/// <summary>
		/// Displays the error.
		/// </summary>
		/// <param name="ex">The exception object.</param>
		public static void Display(Exception ex)
		{
			ShowMessage(ex.Message);
			LogTraceErrorMessage(string.Format(
				"{0} Message= {1} \n Stacktrace = {2} \n InnerException= {3}",
				DateTime.Now, ex.Message, ex.StackTrace, ex.InnerException));
		}

		/// <summary>
		/// Displays the specified ex.
		/// </summary>
		/// <param name="ex">The ex.</param>
		public static void Display(ReflectionTypeLoadException ex)
		{
			string str = string.Empty;

			ShowMessage(ex.Message);

			if (ex.LoaderExceptions.Length > 0)
				str = ex.LoaderExceptions[0].Message;

			LogTraceErrorMessage(string.Format(
				"{0} Message= {1} \n Stacktrace = {2} \n InnerException= {3}",
				DateTime.Now, ex.Message, ex.StackTrace, str));
		}

		/// <summary>
		/// Displays the specified ex.
		/// </summary>
		/// <param name="ex">The ex.</param>
		public static void Display(BadImageFormatException ex)
		{
			ShowMessage("Selected assembly is either corrupted or its targeted framework version is not supported." +
				" Please select non corrupted assembly targeted for .NET 3.5 or earlier framework versions.");
			LogTraceErrorMessage(string.Format(
				"{0} Message= {1} \n Stacktrace = {2} \n InnerException= {3}",
				DateTime.Now, ex.Message, ex.StackTrace, ex.InnerException));
		}
		// Private Methods (2) 

		/// <summary>
		/// Logs the trace error message.
		/// </summary>
		/// <param name="message">The message.</param>
		private static void LogTraceErrorMessage(string message)
		{
			Trace.TraceError(message);
		}

		/// <summary>
		/// Shows the message.
		/// </summary>
		/// <param name="messageBoxBodyText">The message box body text.</param>
		private static void ShowMessage(string messageBoxBodyText)
		{
			MessageBox.Show(messageBoxBodyText, "Error", 
				MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		#endregion Methods 
	}
}
