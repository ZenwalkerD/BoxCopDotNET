
using System.Reflection;
namespace Zenwalker.BoxCop.ILCodeAnalyzerLib.Interfaces
{
    /// <summary>
    /// Contract for loading an assembly
    /// </summary>
	public interface IAssemblyLoader
	{
		#region Data Members (1) 

        /// <summary>
        /// Gets or sets the assembly path.
        /// </summary>
        /// <value>
        /// The assembly path.
        /// </value>
		string AssemblyPath { set; get; }

		#endregion Data Members 

		#region Operations (1) 

        /// <summary>
        /// Loads this instance.
        /// </summary>
        /// <returns>The assembly object</returns>
		Assembly Load();

		#endregion Operations 
	}
}
