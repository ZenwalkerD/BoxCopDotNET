using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using Zenwalker.BoxCop.ILCodeAnalyzerLib;
using Zenwalker.BoxCop.ILCodeAnalyzerLib.Interfaces;
using Zenwalker.BoxCop.PDBDataReader;

namespace Zenwalker.BoxCop.AssemblyStaticAnalyzer
{
	/// <summary>
	/// Analyzes the assembly by investigating its IL instructions.
	/// </summary>
	public class AnalyzeAssembly
	{
		#region Fields (2) 

		List<AnalyzedData> _analyzedData;
		ISymbolReader _symbolreader;

		#endregion Fields 

		#region Constructors (1) 

		/// <summary>
		/// Initializes a new instance of the <see cref="AnalyzeAssembly"/> class.
		/// </summary>
		public AnalyzeAssembly ()
		{
		  _analyzedData = new List<AnalyzedData>();
		}

		#endregion Constructors 

		#region Methods (4) 

		// Public Methods (1) 

		/// <summary>
		/// Analyzes the specified assembly path.
		/// </summary>
		/// <param name="assemblyPath">The assembly path.</param>
		/// <returns></returns>
		public List<AnalyzedData> Analyze(string assemblyPath)
		{
			IAssemblyLoader assem = new AssemblyLoader();
			assem.AssemblyPath = assemblyPath;

			AssemblyReader reader = new AssemblyReader(assem);
			List<MethodInfo> customTypeList = reader.GetAllMethodsInAssembly();

			_symbolreader = PDBDataReader.NativePDBReaderWrapper.GetSymbolReaderForFile(assemblyPath, null);

			if (_symbolreader == null)
				throw new FileLoadException("Symbol file (.pdb) seems to be corrupted or not found." +
					"Please recompile your project again");

			CheckForBoxing(customTypeList);

			return _analyzedData;
		}
		// Private Methods (3) 

		/// <summary>
		/// Checks for boxing.
		/// </summary>
		/// <param name="customTypeList">The custom type list.</param>
		private void CheckForBoxing(List<MethodInfo> customTypeList)
		{
            foreach (var item in customTypeList)
            {
                try
                {
                    //Checking if a method attribute has DLLImport which will be ignored.
                    if (item.GetCustomAttributes(typeof(DllImportAttribute), false).Length > 0)
                        continue;

                    var boxOpCodeList =
                        new MethodBodyAnalyzer().GetMethodBodyInILFormat(item).FindAll(opCodeItem =>
                            opCodeItem.OpCode == OpCodes.Box) as List<ILInstructionFormat>;

                    var pdbSourceData = new PDBReader(_symbolreader).GetMethodSourceFromPDB(item);

                    FetchSourceLines(boxOpCodeList, pdbSourceData, item);
                }
                //It is ok to suppress since for some methods, its source code can not be read or 
                //information can not be read from pdb.
                catch (COMException) { }
                catch (ExternalException) { }
            }
		}

		/// <summary>
		/// Fetches the source lines.
		/// </summary>
		/// <param name="boxOpCodeListngList">The box op code listng list.</param>
		/// <param name="pdbSourceData">The PDB source data.</param>
		/// <param name="methodInfo">The method info.</param>
		private void FetchSourceLines(List<ILInstructionFormat> boxOpCodeListngList, 
			PDBMethodSourceData pdbSourceData, MethodInfo methodInfo)
		{
			foreach (var item in boxOpCodeListngList)
			{
				int? index = GetOffsetIndexFromSourcePDB(item.IlOffset, pdbSourceData.ilOpcodeOffsets);

				if (index != null)
				{
					_analyzedData.Add(new AnalyzedData() {
						 methodName = methodInfo.Name,
						 moduleClassName = methodInfo.DeclaringType.FullName,
						 sourceCodeUrl = pdbSourceData.symbolDocs[0].URL,
						 sourceColumn = (Int32)pdbSourceData.souceCodeColumns.GetValue(index.Value),
						 sourceLine = (Int32)pdbSourceData.sourceCodeLines.GetValue(index.Value)
					});
				}
			}
		}

		/// <summary>
		/// Gets the offset index from source PDB.
		/// </summary>
		/// <param name="ilOffset">The il offset.</param>
		/// <param name="pdbOpCodeOffSetArray">The PDB op code off set array.</param>
		/// <returns></returns>
		private int? GetOffsetIndexFromSourcePDB(int ilOffset, int[] pdbOpCodeOffSetArray)
		{
			int? indexValue = null;
			
			//The offset from method is compared with the pdb offset. Offset for opcode i.e Box in this case
			//is searched in the pdb opcode array. PDb offset array last item will always be end of method line.
			//Hence always index -1 value is chosen as the correct/matching index of the required opcode.
			for (int index = 0; index < pdbOpCodeOffSetArray.Length; index++)
			{
				if (ilOffset >= pdbOpCodeOffSetArray[index] &&
					ilOffset <= pdbOpCodeOffSetArray[index + 1])
				{
					indexValue = index;
					break;
				}
			}

			return indexValue;
		}

		#endregion Methods 
	}
}
