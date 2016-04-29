using System;
using System.Collections.Generic;
using System.Reflection;
using Zenwalker.BoxCop.ILCodeAnalyzerLib.Interfaces;

namespace Zenwalker.BoxCop.ILCodeAnalyzerLib
{
	/// <summary>
	/// Reads an assembly for all the methods.
	/// </summary>
	public class AssemblyReader
	{
		#region Fields (3) 

		BindingFlags _bindingFlags;
		Assembly _customAssembly;
		List<MethodInfo> _typeInfoList;

		#endregion Fields 

		#region Constructors (2) 

		/// <summary>
		/// Initializes a new instance of the <see cref="AssemblyReader"/> class.
		/// </summary>
		/// <param name="assemLoader">The assem loader.</param>
		public AssemblyReader(IAssemblyLoader assemLoader)
			: this()
		{
			_customAssembly = assemLoader.Load();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AssemblyReader"/> class.
		/// </summary>
		protected AssemblyReader()
		{
			_typeInfoList = new List<MethodInfo>();
			_bindingFlags = BindingFlags.NonPublic | BindingFlags.Public |
				BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;
		}

		#endregion Constructors 

		#region Properties (3) 

		/// <summary>
		/// Gets or sets the custom assembly.
		/// </summary>
		/// <value>
		/// The custom assembly.
		/// </value>
		protected virtual Assembly CustomAssembly
		{
			get
			{
				return _customAssembly;
			}
			set { _customAssembly = value; }
		}

		/// <summary>
		/// Gets or sets the custom type info list.
		/// </summary>
		/// <value>
		/// The custom type info list.
		/// </value>
		protected virtual List<MethodInfo> CustomTypeInfoList
		{
			get
			{
				return _typeInfoList;
			}
			set { _typeInfoList = value; }
		}

		/// <summary>
		/// Gets or sets the reflection binding flags.
		/// </summary>
		/// <value>
		/// The reflection binding flags.
		/// </value>
		protected virtual BindingFlags ReflectionBindingFlags
		{
			get
			{
				return _bindingFlags;
			}
			set { _bindingFlags = value; }
		}

		#endregion Properties 

		#region Methods (3) 

		// Public Methods (1) 

		/// <summary>
		/// Finds all methods in assembly.
		/// </summary>
		/// <returns>Methods List</returns>
		public List<MethodInfo> GetAllMethodsInAssembly()
		{
            Module[] modules = _customAssembly.GetModules();

			foreach (var item in modules)
			{
				FindAllTypesInModule(item);
			}

			return _typeInfoList;
		}
		// Protected Methods (2) 

		/// <summary>
		/// Finds all types for module.
		/// </summary>
		/// <param name="module">The module.</param>
		protected virtual void FindAllTypesInModule(Module module)
		{
			foreach (var item in module.GetTypes())
			{
				if (item.IsClass || item.IsValueType)
					GetMethodsInType(item);
			}
		}

		/// <summary>
		/// Gets the type of the methods in.
		/// </summary>
		/// <param name="type">The type.</param>
		protected virtual void GetMethodsInType(Type type)
		{
			foreach (var item in type.GetMethods(_bindingFlags))
			{
				_typeInfoList.Add(item);
			}
		}

		#endregion Methods 
	}
}
