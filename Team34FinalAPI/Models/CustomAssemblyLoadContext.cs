using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;

namespace Team34FinalAPI.Models
{
    public class CustomAssemblyLoadContext: AssemblyLoadContext
    {
        public IntPtr LoadUnmanagedLibrary(string libraryPath)
        {
            return NativeLibrary.Load(libraryPath);
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            return LoadUnmanagedDllFromPath(unmanagedDllName);
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            return null;
        }
    }
}
