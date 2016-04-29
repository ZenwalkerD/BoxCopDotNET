using System;
using System.Diagnostics.SymbolStore;
using System.Runtime.InteropServices;
using Zenwalker.BoxCop.PDBDataReader.Interfaces;

namespace Zenwalker.BoxCop.PDBDataReader
{
    /// <summary>
    /// Native PDB reader, wraps Ole32 native calls
    /// </summary>
    public static class NativePDBReaderWrapper
    {
		#region Methods (2) 

		// Public Methods (1) 

        /// <summary>
        /// Gets the symbol reader for file.
        /// </summary>
        /// <param name="pathModule">The path module.</param>
        /// <param name="searchPath">The search path.</param>
        /// <returns></returns>
        public static ISymbolReader GetSymbolReaderForFile(string pathModule, string searchPath)
        {
            return NativePDBReaderWrapper.GetSymbolReaderForFile(
                new System.Diagnostics.SymbolStore.SymBinder(), pathModule, searchPath);
        }
		// Private Methods (1) 

        // We demand Unmanaged code permissions because we're reading from the file 
        // system and calling out to the Symbol Reader
        // @TODO - make this more specific.
        /// <summary>
        /// Gets the symbol reader for file.
        /// </summary>
        /// <param name="binder">The binder.</param>
        /// <param name="pathModule">The path module.</param>
        /// <param name="searchPath">The search path.</param>
        /// <returns></returns>
        [System.Security.Permissions.SecurityPermission(
            System.Security.Permissions.SecurityAction.Demand,
            Flags = System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode)]
        private static ISymbolReader GetSymbolReaderForFile(
           System.Diagnostics.SymbolStore.SymBinder binder, string pathModule, string searchPath)
        {
            // Guids for imported metadata interfaces.
            Guid dispenserClassID = new Guid(0xe5cb7a31, 0x7512, 0x11d2, 0x89,
                0xce, 0x00, 0x80, 0xc7, 0x92, 0xe5, 0xd8); // CLSID_CorMetaDataDispenser
            Guid dispenserIID = new Guid(0x809c652e, 0x7396, 0x11d2, 0x97, 0x71,
                0x00, 0xa0, 0xc9, 0xb4, 0xd5, 0x0c); // IID_IMetaDataDispenser
            Guid importerIID = new Guid(0x7dac8207, 0xd3ae, 0x4c75, 0x9b, 0x67,
                0x92, 0x80, 0x1a, 0x49, 0x7d, 0x44); // IID_IMetaDataImport

            // First create the Metadata dispenser.
            object objDispenser;
            NativeMethods.CoCreateInstance(ref dispenserClassID, null, 1,
                ref dispenserIID, out objDispenser);

            // Now open an Importer on the given filename. We'll end up passing this importer 
            // straight through to the Binder.
            object objImporter;
            IMetaDataDispenser dispenser = (IMetaDataDispenser)objDispenser;
            dispenser.OpenScope(pathModule, 0, ref importerIID, out objImporter);

            IntPtr importerPtr = IntPtr.Zero;
            ISymbolReader reader = null;
            try
            {
                // This will manually AddRef the underlying object, so we need to 
                // be very careful to Release it.
                importerPtr = Marshal.GetComInterfaceForObject(objImporter,
                    typeof(IMetadataImport));

                reader = binder.GetReader(importerPtr, pathModule, searchPath);
            }
            finally
            {
                if (importerPtr != IntPtr.Zero)
                {
                    Marshal.Release(importerPtr);
                }
            }
            return reader;
        }

		#endregion Methods 

		#region Nested Classes (1) 


        /// <summary>
        /// Wraps a native call
        /// </summary>
        static class NativeMethods
        {
            /// <summary>
            /// Coes the create instance.
            /// </summary>
            /// <param name="rclsid">The rclsid.</param>
            /// <param name="pUnkOuter">The p unk outer.</param>
            /// <param name="dwClsContext">The dw CLS context.</param>
            /// <param name="riid">The riid.</param>
            /// <param name="ppv">The PPV.</param>
            /// <returns></returns>
            [DllImport("ole32.dll")]
            public static extern int CoCreateInstance(
                [In] ref Guid rclsid,
                [In, MarshalAs(UnmanagedType.IUnknown)] Object pUnkOuter,
                [In] uint dwClsContext,
                [In] ref Guid riid,
                [Out, MarshalAs(UnmanagedType.Interface)] out Object ppv);
        }
		#endregion Nested Classes 
    }
}
