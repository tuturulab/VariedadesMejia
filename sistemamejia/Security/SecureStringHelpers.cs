using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Variedades.Security
{
    public static class SecureStringHelpers
    {
        public static string Unsecure(this SecureString secureString)
        {
            //Make sure we have a secure string
            if (secureString == null)
                return string.Empty;

            //Get a pointer for and unsecure string in memory
            var unmanagedString = IntPtr.Zero;

            try
            {
                //usecures the password
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                //Clean up any memory allocation
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}
