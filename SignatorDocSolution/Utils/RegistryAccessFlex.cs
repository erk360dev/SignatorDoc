
using Microsoft.Win32;
using System;
namespace SignatorDocSolution.Utils
{
    public static class RegistryAccessFlex
    {

        public static RegistryKey OpenSubKey(RegistryKey parentKey, string subKeyName, bool writable, RegWow64Options options)
        {
            if (parentKey == null || GetRegistryKeyHandle(parentKey) == IntPtr.Zero)
            {
                return null;
            }

            int rights = (int)RegistryRights.ReadKey;
            if (writable)
                rights = (int)RegistryRights.WriteKey;

            int subKeyHandle, result = RegOpenKeyEx(GetRegistryKeyHandle(parentKey), subKeyName, 0, rights | (int)options, out subKeyHandle);

            if (result != 0)
            {
                return null;
            }

            RegistryKey subKey = PointerToRegistryKey((IntPtr)subKeyHandle, writable, false);
            return subKey;
        }

        public static IntPtr GetRegistryKeyHandle(RegistryKey registryKey)
        {
            Type registryKeyType = typeof(RegistryKey);
            System.Reflection.FieldInfo fieldInfo =
            registryKeyType.GetField("hkey", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            System.Runtime.InteropServices.SafeHandle handle = (System.Runtime.InteropServices.SafeHandle)fieldInfo.GetValue(registryKey);
            IntPtr dangerousHandle = handle.DangerousGetHandle();
            return dangerousHandle;
        }

        public static RegistryKey PointerToRegistryKey(IntPtr hKey, bool writable, bool ownsHandle)
        {
            System.Reflection.BindingFlags privateConstructors = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic;
            Type safeRegistryHandleType = typeof(Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid).Assembly.GetType("Microsoft.Win32.SafeHandles.SafeRegistryHandle");
            Type[] safeRegistryHandleCtorTypes = new Type[] { typeof(IntPtr), typeof(bool) };
            System.Reflection.ConstructorInfo safeRegistryHandleCtorInfo = safeRegistryHandleType.GetConstructor(
            privateConstructors, null, safeRegistryHandleCtorTypes, null);
            Object safeHandle = safeRegistryHandleCtorInfo.Invoke(new Object[] { hKey, ownsHandle });

            Type registryKeyType = typeof(RegistryKey);
            Type[] registryKeyConstructorTypes = new Type[] { safeRegistryHandleType, typeof(bool) };
            System.Reflection.ConstructorInfo registryKeyCtorInfo = registryKeyType.GetConstructor(
            privateConstructors, null, registryKeyConstructorTypes, null);
            RegistryKey resultKey = (RegistryKey)registryKeyCtorInfo.Invoke(new Object[] { safeHandle, writable });
            return resultKey;
        }



        [System.Runtime.InteropServices.DllImport("advapi32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern int RegOpenKeyEx(IntPtr hKey, string subKey, int ulOptions, int samDesired, out int phkResult);

        enum RegistryRights
        {
            ReadKey = 131097,
            WriteKey = 131078
        }
    }

    public enum RegWow64Options
    {
        None = 0,
        KEY_WOW64_64KEY = 0x0100,
        KEY_WOW64_32KEY = 0x0200
    }
}
