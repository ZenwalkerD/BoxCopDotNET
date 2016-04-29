using System.Reflection.Emit;

namespace Zenwalker.BoxCop.ILCodeAnalyzerLib
{
	/// <summary>
	/// Instruction format structure.
	/// </summary>
	public class ILInstructionFormat
	{
		#region Fields (2) 

				private int _ilOffset;
		private OpCode _opCode;

		#endregion Fields 

		#region Properties (2) 

		/// <summary>
		/// Gets or sets the il offset.
		/// </summary>
		/// <value>
		/// The il offset.
		/// </value>
		public int IlOffset
		{
			get
			{
				return _ilOffset;
			}
			set
			{
				_ilOffset = value;
			}
		}

		/// <summary>
		/// Gets or sets the op code.
		/// </summary>
		/// <value>
		/// The op code.
		/// </value>
		public OpCode OpCode
		{
			get
			{
				return _opCode;
			}
			set
			{
				_opCode = value;
			}
		}

		#endregion Properties 
	}
}
