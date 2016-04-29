using System;
using System.Runtime.InteropServices;

namespace Zenwalker.BoxCop.PDBDataReader.Interfaces
{
    // Since we're just blindly passing this interface through managed code to the Symbinder, 
    // we don't care about actually importing the specific methods.
    // This needs to be public so that we can call Marshal.GetComInterfaceForObject() on 
    // it to get the underlying metadata pointer.
    [Guid("7DAC8207-D3AE-4c75-9B67-92801A497D44"),
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComVisible(true)]
    [CLSCompliant(true)]
    public interface IMetadataImport
    {
		#region Operations (1) 

        // Just need a single placeholder method so that it doesn't complain
        // about an empty interface.
        void Placeholder();

		#endregion Operations 
    }
}
