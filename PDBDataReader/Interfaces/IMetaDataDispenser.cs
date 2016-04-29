using System;
using System.Runtime.InteropServices;

namespace Zenwalker.BoxCop.PDBDataReader.Interfaces
{
    // We can use reflection-only load context to use reflection to query for 
    // metadata information rather
    // than painfully import the com-classic metadata interfaces.
    [Guid("809c652e-7396-11d2-9771-00a0c9b4d50c"),
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComVisible(true)]
    interface IMetaDataDispenser
    {
		#region Operations (1) 

        // We need to be able to call OpenScope, which is the 2nd vtable slot.
        // Thus we need this one placeholder here to occupy the first slot..
        void DefineScope_Placeholder();

		#endregion Operations 
        //STDMETHOD(OpenScope)(                   // Return code.
        //LPCWSTR     szScope,                // [in] The scope to open.
        //  DWORD       dwOpenFlags,            // [in] Open mode flags.
        //  REFIID      riid,                   // [in] The interface desired.
        //  IUnknown    **ppIUnk) PURE;         // [out] Return interface on success.
        void OpenScope([In, MarshalAs(UnmanagedType.LPWStr)] String szScope,
            [In] Int32 dwOpenFlags, [In] ref Guid riid,
            [Out, MarshalAs(UnmanagedType.IUnknown)] out Object punk);
        // Don't need any other methods.
    }

}
