using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Zenwalker.BoxCop.ILCodeAnalyzerLib
{
	/// <summary>
	/// Builds the basic structure of all the MSIL opcodes.
	/// </summary>
	public sealed class OpCodesBuilder
	{
		#region Fields (3) 

		private static readonly OpCodesBuilder _instance = new OpCodesBuilder();
		private readonly IDictionary<int, OpCode> oneByteOpcodesDictionary;
 //32 bit opcodes
		private readonly IDictionary<int, OpCode> twoByteOpcodesDictionary;

		#endregion Fields 

		#region Constructors (2) 

//64 bit opcodes viz Int64, etc.
		/// <summary>
		/// Initializes the <see cref="OpCodesBuilder"/> class.
		/// </summary>
		static OpCodesBuilder()
		{
			
		}

		/// <summary>
		/// Prevents a default instance of the <see cref="OpCodesBuilder"/> class from being created.
		/// </summary>
		private OpCodesBuilder()
		{
			oneByteOpcodesDictionary = new Dictionary<int, OpCode>();
			twoByteOpcodesDictionary = new Dictionary<int, OpCode>();
			LoadDefaultOpcodes();
		}

		#endregion Constructors 

		#region Properties (3) 

		/// <summary>
		/// Gets the instance.
		/// </summary>
		public static OpCodesBuilder Instance
		{            
			get
			{
				return _instance;
			}
		}

		/// <summary>
		/// Gets the one byte op code dictionary.
		/// </summary>
		public IDictionary<int, OpCode> OneByteOpCodeDictionary { get { return oneByteOpcodesDictionary; } }

		/// <summary>
		/// Gets the two byte op code dictionary.
		/// </summary>
		public IDictionary<int, OpCode> TwoByteOpCodeDictionary { get { return twoByteOpcodesDictionary; } }

		#endregion Properties 

		#region Methods (1) 

		// Private Methods (1) 

		/// <summary>
		/// Loads the default opcodes.
		/// </summary>
		private void LoadDefaultOpcodes()
		{
            FieldInfo[] frameworkOpCodeFieldInfo = typeof(OpCodes).GetFields();

			foreach (var item in frameworkOpCodeFieldInfo)
			{
				if (item.FieldType == typeof(OpCode))
				{
					var opCodeValue = (OpCode)item.GetValue(null);
                    ushort opCodeShortValue = (ushort)opCodeValue.Value;

                    if (opCodeShortValue < 0x100)
					{
                        oneByteOpcodesDictionary.Add((int)opCodeShortValue, opCodeValue);
					}
                    else twoByteOpcodesDictionary.Add((opCodeShortValue & 0xff), opCodeValue);
				}
			}
		}

		#endregion Methods 
	}
}
