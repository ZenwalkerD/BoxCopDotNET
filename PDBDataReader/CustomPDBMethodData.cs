using System.Diagnostics.SymbolStore;

namespace Zenwalker.BoxCop.PDBDataReader
{
    /// <summary>
    /// Holds all the PDB information for a method.
    /// </summary>
    public class PDBMethodSourceData
    {
		#region Fields (6) 

        /// <summary>
        /// Offset information for each of the IL instruction set
        /// </summary>
        public int[] ilOpcodeOffsets;
        /// <summary>
        /// Column information in source code
        /// </summary>
        public int[] souceCodeColumns;
        /// <summary>
        /// Column end information in source code
        /// </summary>
        public int[] sourceCodeEndColumns;
        /// <summary>
        /// End line information in source code
        /// </summary>
        public int[] sourceCodeEndLines;
        /// <summary>
        /// All line information in source code
        /// </summary>
        public int[] sourceCodeLines;
        /// <summary>
        /// Soure code path and name
        /// </summary>
        public ISymbolDocument[] symbolDocs;

		#endregion Fields 
    }
}
