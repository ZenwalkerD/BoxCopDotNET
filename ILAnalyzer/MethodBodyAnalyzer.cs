using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;

namespace Zenwalker.BoxCop.ILCodeAnalyzerLib
{
	/// <summary>
	/// Analyzes the method body code IL instructions
	/// </summary>
	public class MethodBodyAnalyzer
	{
		#region Fields (2) 

		protected byte[] _ilInstructions;
		protected List<ILInstructionFormat> _instructionFormatList;

		#endregion Fields 

		#region Constructors (1) 

		/// <summary>
		/// Initializes a new instance of the <see cref="MethodBodyAnalyzer"/> class.
		/// </summary>
		public MethodBodyAnalyzer()
		{
			_instructionFormatList = new List<ILInstructionFormat>();
		}

		#endregion Constructors 

		#region Methods (4) 

		// Public Methods (1) 

		/// <summary>
		/// Gets the method body in IL format.
		/// </summary>
		/// <param name="methodInfo">The method info.</param>
		/// <returns></returns>
		public List<ILInstructionFormat> GetMethodBodyInILFormat(MethodInfo methodInfo)
		{
			ReadMethodBody(methodInfo);
			
			//If method is empty, then only nop and ret IL instructions will be present in that method.
			//In such case, no use converting the byte array to IL code.
			if (_ilInstructions != null && _ilInstructions.Length != 2 && _ilInstructions[1] != 42)
				ConvertByteInstructionsToILInstructions(methodInfo);

			return _instructionFormatList;
		}
		// Protected Methods (3) 

		/// <summary>
		/// Converts the byte instructions to IL instructions.
		/// </summary>
		/// <param name="methodInfo">The method info.</param>
		protected virtual void ConvertByteInstructionsToILInstructions(MethodInfo methodInfo)
		{
			OpCode opCode = OpCodes.Nop;//Default set to Nop instruction.

			for (int position = 0; position < _ilInstructions.Length;)
			{
				//many opcode values are in -ve range, hence converting to unsigned values.
				ushort ilByteValue = _ilInstructions[position++];

				if (ilByteValue != 254)//Some opcodes are doublebyte type and their byte value is 254
				{
					OpCodesBuilder.Instance.OneByteOpCodeDictionary.TryGetValue((int)ilByteValue, out opCode);
				}
				else
				{
					//In IL byte array, double byte opcodes are identified as 254 value. 
					//Once it is identified, next byte value in this array represents the actual opcodes.
					ilByteValue = _ilInstructions[position++];
					OpCodesBuilder.Instance.TwoByteOpCodeDictionary.TryGetValue((int)ilByteValue, out opCode);
				}
				//Since position is already incremented to next value, the actual offset lies in the previous value.
				//If you see dissaassembled IL via ILSPY or similar tool, on left hand side of IL op codes, there
				//lies their offset.
				_instructionFormatList.Add(new ILInstructionFormat() { OpCode = opCode, IlOffset = (position - 1) });

				MovePositionToNextOpcode(opCode, ref position);
			}
		}

		/// <summary>
		/// Moves the position to next opcode.
		/// </summary>
		/// <param name="opCode">The op code.</param>
		/// <param name="position">The position.</param>
		protected virtual void MovePositionToNextOpcode(OpCode opCode, ref int position)
		{
			//Based on the operand type, the position is reset.
			switch(opCode.OperandType)
			{
				case OperandType.InlineBrTarget :                    
				case OperandType.InlineField :
				case OperandType.InlineMethod :
				case OperandType.InlineSig:
				case OperandType.InlineTok:
				case OperandType.InlineType:
				case OperandType.InlineI:
				case OperandType.InlineString:
				case OperandType.ShortInlineR:
					position += 4;
					break;
				case OperandType.InlineSwitch:
					position += (4 * 4);
					break;
				case OperandType.InlineI8:
				case OperandType.InlineR:
					position += 8;
					break;
				case OperandType.InlineVar:
					position += 2;
					break;
				case OperandType.ShortInlineBrTarget:
				case OperandType.ShortInlineI:
				case OperandType.ShortInlineVar:
					position++;
					break;
				case OperandType.InlineNone:
					break;
				default:
					throw new ApplicationException("Invalid OpCode type. Assembly is not compatible",
						new Exception("OperadType = " + opCode.OperandType + " OpCode= " + opCode.Name));
			}
		}

		/// <summary>
		/// Reads the method body.
		/// </summary>
		/// <param name="methodInfo">The method info.</param>
		protected void ReadMethodBody(MethodInfo methodInfo)
		{
            try
            {
                if (methodInfo != null)
                {
                    MethodBody methodBody = methodInfo.GetMethodBody();

                    if (methodBody != null)
                    {
                        _ilInstructions = methodBody.GetILAsByteArray();
                    }
                }
            }
            catch(FileNotFoundException ex)
            {
                Trace.WriteLine("MethodInfo = " + methodInfo.Module + "." + 
                    methodInfo.Name+ "  " + ex.Message + "      " + ex.StackTrace);
            }
		}

		#endregion Methods 
	}
}
