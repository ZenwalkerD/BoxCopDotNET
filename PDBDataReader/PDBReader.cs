using System;
using System.Diagnostics.SymbolStore;
using System.Reflection;

namespace Zenwalker.BoxCop.PDBDataReader
{
    /// <summary>
    /// Reads the PDB file of an assembly by using Native APIs.
    /// </summary>
    public class PDBReader
    {
		#region Fields (1) 

        ISymbolReader _symbolReader;

		#endregion Fields 

		#region Constructors (1) 

        /// <summary>
        /// Initializes a new instance of the <see cref="PDBReader"/> class.
        /// </summary>
        /// <param name="symbolReader">The symbol reader.</param>
        public PDBReader(ISymbolReader symbolReader)
        {
            if (symbolReader == null)
                throw new ArgumentNullException("Argument passed is null");

            this._symbolReader = symbolReader;            
        }

		#endregion Constructors 

		#region Methods (1) 

		// Public Methods (1) 

        /// <summary>
        /// Gets the method source from PDB.
        /// </summary>
        /// <param name="methodInfo">The method info.</param>
        /// <returns></returns>
        public PDBMethodSourceData GetMethodSourceFromPDB(MethodInfo methodInfo)
        {
            ISymbolMethod met = _symbolReader.GetMethod(new SymbolToken(methodInfo.MetadataToken));

            int count = met.SequencePointCount;

            ISymbolDocument[] docs = new ISymbolDocument[count];
            int[] offsets = new int[count];
            int[] lines = new int[count];
            int[] columns = new int[count];
            int[] endlines = new int[count];
            int[] endcolumns = new int[count];

            met.GetSequencePoints(offsets, docs, lines, columns, endlines, endcolumns);

            return new PDBMethodSourceData()
            {
                symbolDocs = docs,
                sourceCodeLines = lines,
                ilOpcodeOffsets = offsets,
                souceCodeColumns = columns,
                sourceCodeEndColumns = endcolumns,
                sourceCodeEndLines = endlines
            };
        }

		#endregion Methods 
    }
}
