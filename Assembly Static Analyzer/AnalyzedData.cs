
namespace Zenwalker.BoxCop.AssemblyStaticAnalyzer
{
	/// <summary>
	/// Class holds final analyzed information as result.
	/// </summary>
	public class AnalyzedData
	{
		#region Fields (5) 

        /// <summary>
        /// Method name
        /// </summary>
		public string methodName;
        /// <summary>
        /// Class Name to which a method belongs
        /// </summary>
		public string moduleClassName;
        /// <summary>
        /// Path and file name of the source code.
        /// </summary>
		public string sourceCodeUrl;
        /// <summary>
        /// Column location in the source code.
        /// </summary>
		public int sourceColumn;
        /// <summary>
        /// Line number in the source code.
        /// </summary>
		public int sourceLine;

		#endregion Fields 
	}
}
