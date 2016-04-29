using System.IO;
using System.Reflection;
using Zenwalker.BoxCop.ILCodeAnalyzerLib.Interfaces;

namespace Zenwalker.BoxCop.ILCodeAnalyzerLib
{
	/// <summary>
	/// Loads the assembly into memory.
	/// </summary>
	public sealed class AssemblyLoader : IAssemblyLoader
	{
		#region Fields (1) 

		private string _assemblyPath;

		#endregion Fields 

		#region Properties (1) 

		/// <summary>
		/// Gets or sets the assembly path.
		/// </summary>
		/// <value>
		/// The assembly path.
		/// </value>
		public string AssemblyPath
		{
			get
			{
				return _assemblyPath;   
			}
			set
			{
				_assemblyPath = value;
			}
		}

		#endregion Properties 

		#region Methods (1) 

		// Public Methods (1) 

		/// <summary>
		/// Loads assembly into memory.
		/// </summary>
		/// <returns>
		/// The assembly object
		/// </returns>
		public Assembly Load()
		{
			if (string.IsNullOrEmpty(_assemblyPath))
				throw new FileNotFoundException("Please select an assembly.");
						
			Assembly assembly = Assembly.LoadFrom(_assemblyPath);
			FileInfo file = new FileInfo(assembly.Location);

			var pdbPath = Path.Combine(file.Directory.FullName, 
				string.Concat(assembly.GetName().Name, ".pdb"));
			
			if (!File.Exists(pdbPath))
				throw new FileNotFoundException(
                    "Pdb information for selected assembly not found. Please copy correct pdb files "+
                    "or rebuild your source code to get pdb files.");

			return assembly;
		}

		#endregion Methods 
	}
}
