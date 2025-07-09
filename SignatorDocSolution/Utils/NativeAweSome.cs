using NativeDeviceControlLib;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
namespace SignatorDocSolution.Utils
{
    public static class NativeAweSome
    {
        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWow64Process([In] IntPtr hProcess, [Out] out bool lpSystemInfo);

        public static bool IsX64Platform()
        {
            bool retVal;
            IsWow64Process(Process.GetCurrentProcess().Handle, out retVal);

            return retVal;
        }

        public static void UNBusyDevice() {
            if (DetectDevice.deviceSerieNumber == 1)
                SignatureDeviceDrawSTU.releaseDeviceBusy();
        }
    }

    public static class DetectWintabDevice
    {
        public static bool isWinTabPlugged()
        {
            UInt32 numberDevicesConnected = NativeDeviceInfo.GetNumberOfDevices();
            bool isStylusActive = NativeDeviceInfo.IsStylusActive();

            if ((numberDevicesConnected > 0) && isStylusActive)
                return true;
            else
                return false;
        }

        public static bool isWintabInstaled()
        {
            bool winTabAvailable = NativeDeviceInfo.IsWintabAvailable();
            if (winTabAvailable)
                return true;
            else
            {
                //if (!NativeDeviceInfo.getIsExceptionOcurred())
                //    SignatorDocSolution.Utils.RegisterWinEventLogViewer.RegisterLog("The detection of device Wintab throws the following message : ", "Is not possible to check whether the device service is running or not.", 14, 2);
                //else {
                //    string exceptionMessage = NativeDeviceInfo.getExceptionMessage();
                //    if (exceptionMessage.Contains("Unable to load DLL 'Wintab32.dll'"))
                //        SignatorDocSolution.Utils.RegisterWinEventLogViewer.RegisterLog("The detection of device Wintab throws the following message : ", exceptionMessage, 14, 2);
                //    if (exceptionMessage.Contains("An attempt was made to load a program with an incorrect format"))
                //        SignatorDocSolution.Utils.RegisterWinEventLogViewer.RegisterLog("The detection of device Wintab throws the following message : ", exceptionMessage, 14, 2);
                //}
                return false;
            }

        }

        public static bool isWinTabAvailable() {
            if (isWintabInstaled())
                if (isWinTabPlugged())
                    return true;
            return false;
        }
    }

    public static class DetectSTUDevice {
        public static bool isAlredyTested=false;
        
        public static bool isSTUPlugged() {
            if (isAlredyTested)
                return NativeDeviceControlLib.NativeDeviceSTU.deviceAlredyDetected;
            else
            {
                isAlredyTested = true;
                if(checkSTUDriverInstaled())
                    if(checkWgssSTURegistered())
                        return NativeDeviceControlLib.NativeDeviceSTU.detectSTUDevice();
            }
            return false;
        }

        public static void showDeviceIntroduction()
        {
            NativeDeviceControlLib.NativeDeviceSTU.showIntroduction();
        }

        public static void setStartClickEventHandler(NativeDeviceControlLib.Utils.CommonEventHandler evt) {
            NativeDeviceSTU.setExternalEventHandler_StartClick(evt);
        }

        public static void releaseSTUResouces() {
            if(NativeDeviceSTU.isInitialized)
                NativeDeviceSTU.disconectDevice();
        }

        public static bool checkSTUDriverInstaled() {
            try
            {
                Microsoft.Win32.RegistryKey wacom = null;

                if (!NativeAweSome.IsX64Platform())
                {
                    wacom = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Wacom");
                }
                
                if(wacom==null)
                {
                    wacom = RegistryAccessFlex.OpenSubKey(Microsoft.Win32.Registry.LocalMachine, @"SOFTWARE\Wacom", false, RegWow64Options.KEY_WOW64_64KEY);
                }

                if (wacom != null)
                    wacom=wacom.OpenSubKey("Wacom STU Driver");

                if (wacom != null)
                {
                    return true;
                }
            }
            catch { }
            return false;
        }

        public static bool checkWgssSTURegistered() {
            try
            {
                Microsoft.Win32.RegistryKey stuClass = null;
                if (SignatorDocSolution.Utils.NativeAweSome.IsX64Platform())
                {
                    Microsoft.Win32.RegistryKey wow6432node = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("Wow6432Node").OpenSubKey("CLSID");
                    if (wow6432node != null)
                        stuClass = wow6432node.OpenSubKey("{2000d7a5-64f7-4826-b56e-85acc618e4d6}");
                }
                if (stuClass == null)
                    stuClass = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("CLSID").OpenSubKey("{2000d7a5-64f7-4826-b56e-85acc618e4d6}");

                if (stuClass != null)
                {
                    Microsoft.Win32.RegistryKey stuDevice = stuClass.OpenSubKey("ProgID");
                    if (stuDevice != null)
                    {
                        if (stuDevice.GetValueNames().Length > 0)
                            return true;
                    }

                }
            }
            catch (Exception exc) { }
            return false;
        }

    }

    public static class DetectDevice {
        public static int deviceSerieNumber = 0;
        public static bool thereIsDeviceAvailable() {
            if (DetectSTUDevice.isSTUPlugged())
            {
                deviceSerieNumber = 1;
                return true;
            }
            else {
                if (DetectWintabDevice.isWinTabAvailable()) {
                    deviceSerieNumber = 2;
                    return true;
                }
            }
            return false;
        }
    }

}
