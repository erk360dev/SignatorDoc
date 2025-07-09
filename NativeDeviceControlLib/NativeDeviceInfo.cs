using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace NativeDeviceControlLib
{
    public class NativeDeviceInfo
    {
        private const int MAX_STRING_SIZE = 256;
        private static bool exceptionOccurred = false;
        private static string exceptionMessage = null;

        public static bool IsWintabAvailable()
        {
            IntPtr buf = IntPtr.Zero;
            bool status = false;

            try
            {
                status = (NativeDeviceFunctions.WTInfoA(0, 0, buf) > 0);
                exceptionOccurred = false;
            }
            catch (Exception ex)
            {
                exceptionOccurred = true;
                exceptionMessage = ex.Message;
            }

            return status;
        }

        public static bool getIsExceptionOcurred(){
            return exceptionOccurred;
        }
        public static string getExceptionMessage() {
            return exceptionMessage;
        }


        /// <summary>
        /// Returns a string containing device name.
        /// </summary>
        /// <returns></returns>
        public static String GetDeviceInfo()
        {
            string devInfo = null;
            IntPtr buf = NativeMemoryUtils.AllocUnmanagedBuf(MAX_STRING_SIZE);

            try
            {
                int size = (int)NativeDeviceFunctions.WTInfoA(
                    (uint)EWTICategoryIndex.WTI_DEVICES,
                    (uint)EWTIDevicesIndex.DVC_NAME, buf);

                if (size < 1)
                {
                    throw new Exception("GetDeviceInfo returned empty string.");
                }

                // Strip off final null character before marshalling.
                devInfo = NativeMemoryUtils.MarshalUnmanagedString(buf, size - 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("FAILED GetDeviceInfo: " + ex.ToString(), "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            NativeMemoryUtils.FreeUnmanagedBuf(buf);
            return devInfo;
        }

        /// <summary>
        /// Returns the default digitizing context, with useful context overrides. 
        /// </summary>
        /// <param name="options_I">caller's options; OR'd into context options</param>
        /// <returns>A valid context object or null on error.</returns>
        public static NativeDeviceContext GetDefaultDigitizingContext(ECTXOptionValues options_I = 0)
        {
            // Send all possible data bits (not including extended data).
            // This is redundant with NativeDeviceContext initialization, which
            // also inits with PK_PKTBITS_ALL.
            uint PACKETDATA = (uint)EWintabPacketBit.PK_PKTBITS_ALL;  // The Full Monty
            uint PACKETMODE = (uint)EWintabPacketBit.PK_BUTTONS;

            NativeDeviceContext context = GetDefaultContext(EWTICategoryIndex.WTI_DEFCONTEXT);

            if (context != null)
            {
                // Add digitizer-specific context tweaks.
                context.PktMode = 0;        // all data in absolute mode (set EWintabPacketBit bit(s) for relative mode)
                context.SysMode = false;    // system cursor tracks in absolute mode (zero)

                // Add caller's options.
                context.Options |= (uint)options_I;

                // Set the context data bits.
                context.PktData = PACKETDATA;
                context.PktMode = PACKETMODE;
                context.MoveMask = PACKETDATA;
                context.BtnUpMask = context.BtnDnMask;
            }

            return context;
        }

        /// <summary>
        /// Returns the default system context, with useful context overrides.
        /// </summary>
        /// <returns>A valid context object or null on error.</returns>
        public static NativeDeviceContext GetDefaultSystemContext()
        {
            NativeDeviceContext context = GetDefaultContext(EWTICategoryIndex.WTI_DEFSYSCTX);

            if (context != null)
            {
                // TODO: Add system-specific context tweaks.
            }

            return context;
        }

        /// <summary>
        /// Helper function to get digitizing or system default context.
        /// </summary>
        /// <param name="contextType_I">Use WTI_DEFCONTEXT for digital context or WTI_DEFSYSCTX for system context</param>
        /// <returns>Returns the default context or null on error.</returns>
        private static NativeDeviceContext GetDefaultContext(EWTICategoryIndex contextIndex_I)
        {
            NativeDeviceContext context = new NativeDeviceContext();
            IntPtr buf = NativeMemoryUtils.AllocUnmanagedBuf(context.LogContext);

            try
            {
                int size = (int)NativeDeviceFunctions.WTInfoA((uint)contextIndex_I, 0, buf);

                context.LogContext = NativeMemoryUtils.MarshalUnmanagedBuf<WintabLogContext>(buf, size);
            }
            catch (Exception ex)
            {
                MessageBox.Show("FAILED GetDefaultContext: " + ex.ToString(), "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            NativeMemoryUtils.FreeUnmanagedBuf(buf);

            return context;
        }

        /// <summary>
        /// Returns the default device.  If this value is -1, then it also known as a "virtual device".
        /// </summary>
        /// <returns></returns>
        public static Int32 GetDefaultDeviceIndex()
        {
            Int32 devIndex = 0;
            IntPtr buf = NativeMemoryUtils.AllocUnmanagedBuf(devIndex);

            try
            {
                int size = (int)NativeDeviceFunctions.WTInfoA(
                    (uint)EWTICategoryIndex.WTI_DEFCONTEXT,
                    (uint)EWTIContextIndex.CTX_DEVICE, buf);

                devIndex = NativeMemoryUtils.MarshalUnmanagedBuf<Int32>(buf, size);
            }
            catch (Exception ex)
            {
                MessageBox.Show("FAILED GetDefaultDeviceIndex: " + ex.ToString(), "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            NativeMemoryUtils.FreeUnmanagedBuf(buf);

            return devIndex;
        }

        /// <summary>
        /// Returns the WintabAxis object for specified device and dimension.
        /// </summary>
        /// <param name="devIndex_I">Device index (-1 = virtual device)</param>
        /// <param name="dim_I">Dimension: AXIS_X, AXIS_Y or AXIS_Z</param>
        /// <returns></returns>
        public static WintabAxis GetDeviceAxis(Int32 devIndex_I, EAxisDimension dim_I)
        {
            WintabAxis axis = new WintabAxis();
            IntPtr buf = NativeMemoryUtils.AllocUnmanagedBuf(axis);

            try
            {
                int size = (int)NativeDeviceFunctions.WTInfoA(
                    (uint)(EWTICategoryIndex.WTI_DEVICES + devIndex_I),
                    (uint)dim_I, buf);

                // If size == 0, then returns a zeroed struct.
                axis = NativeMemoryUtils.MarshalUnmanagedBuf<WintabAxis>(buf, size);
            }
            catch (Exception ex)
            {
                MessageBox.Show("FAILED GetDeviceAxis: " + ex.ToString(), "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            NativeMemoryUtils.FreeUnmanagedBuf(buf);

            return axis;
        }

        /// <summary>
        /// Returns a 3-element array describing the tablet's orientation range and resolution capabilities.
        /// </summary>
        /// <returns></returns>
        public static WintabAxisArray GetDeviceOrientation(out bool tiltSupported_O)
        {
            WintabAxisArray axisArray = new WintabAxisArray();
            tiltSupported_O = false;
            IntPtr buf = NativeMemoryUtils.AllocUnmanagedBuf(axisArray);

            try
            {
                int size = (int)NativeDeviceFunctions.WTInfoA(
                    (uint)EWTICategoryIndex.WTI_DEVICES,
                    (uint)EWTIDevicesIndex.DVC_ORIENTATION, buf);

                // If size == 0, then returns a zeroed struct.
                axisArray = NativeMemoryUtils.MarshalUnmanagedBuf<WintabAxisArray>(buf, size);
                tiltSupported_O = (axisArray.array[0].axResolution != 0 && axisArray.array[1].axResolution != 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show("FAILED GetDeviceOrientation: " + ex.ToString(), "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            NativeMemoryUtils.FreeUnmanagedBuf(buf);

            return axisArray;
        }


        /// <summary>
        /// Returns a 3-element array describing the tablet's rotation range and resolution capabilities
        /// </summary>
        /// <returns></returns>
        public static WintabAxisArray GetDeviceRotation(out bool rotationSupported_O)
        {
            WintabAxisArray axisArray = new WintabAxisArray();
            rotationSupported_O = false;
            IntPtr buf = NativeMemoryUtils.AllocUnmanagedBuf(axisArray);

            try
            {
                int size = (int)NativeDeviceFunctions.WTInfoA(
                    (uint)EWTICategoryIndex.WTI_DEVICES,
                    (uint)EWTIDevicesIndex.DVC_ROTATION, buf);

                // If size == 0, then returns a zeroed struct.
                axisArray = NativeMemoryUtils.MarshalUnmanagedBuf<WintabAxisArray>(buf, size);
                rotationSupported_O = (axisArray.array[0].axResolution != 0 && axisArray.array[1].axResolution != 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show("FAILED GetDeviceRotation: " + ex.ToString(), "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            NativeMemoryUtils.FreeUnmanagedBuf(buf);

            return axisArray;
        }

        /// <summary>
        /// Returns the number of devices connected.
        /// </summary>
        /// <returns></returns>
        public static UInt32 GetNumberOfDevices()
        {
            UInt32 numDevices = 0;
            IntPtr buf = NativeMemoryUtils.AllocUnmanagedBuf(numDevices);

            try
            {
                int size = (int)NativeDeviceFunctions.WTInfoA(
                    (uint)EWTICategoryIndex.WTI_INTERFACE,
                    (uint)EWTIInterfaceIndex.IFC_NDEVICES, buf);

                numDevices = NativeMemoryUtils.MarshalUnmanagedBuf<UInt32>(buf, size);
            }
            catch (Exception ex)
            {
                MessageBox.Show("FAILED GetNumberOfDevices: " + ex.ToString(), "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            NativeMemoryUtils.FreeUnmanagedBuf(buf);

            return numDevices;
        }

        /// <summary>
        /// Returns whether a stylus is currently connected to the active cursor.
        /// </summary>
        /// <returns></returns>
        public static bool IsStylusActive()
        {
            bool isStylusActive = false;
            IntPtr buf = NativeMemoryUtils.AllocUnmanagedBuf(isStylusActive);

            try
            {
                int size = (int)NativeDeviceFunctions.WTInfoA(
                    (uint)EWTICategoryIndex.WTI_INTERFACE,
                    (uint)EWTIInterfaceIndex.IFC_NDEVICES, buf);

                isStylusActive = NativeMemoryUtils.MarshalUnmanagedBuf<bool>(buf, size);
            }
            catch (Exception ex)
            {
                MessageBox.Show("FAILED GetNumberOfDevices: " + ex.ToString(), "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            NativeMemoryUtils.FreeUnmanagedBuf(buf);

            return isStylusActive;
        }


        /// <summary>
        /// Returns a string containing the name of the selected stylus. 
        /// </summary>
        /// <param name="index_I">indicates stylus type</param>
        /// <returns></returns>
        public static string GetStylusName(EWTICursorNameIndex index_I)
        {
            string stylusName = null;
            IntPtr buf = NativeMemoryUtils.AllocUnmanagedBuf(MAX_STRING_SIZE);

            try
            {
                int size = (int)NativeDeviceFunctions.WTInfoA(
                    (uint)index_I,
                    (uint)EWTICursorsIndex.CSR_NAME, buf);

                if (size < 1)
                {
                    throw new Exception("GetStylusName returned empty string.");
                }

                // Strip off final null character before marshalling.
                stylusName = NativeMemoryUtils.MarshalUnmanagedString(buf, size - 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("FAILED GetDeviceInfo: " + ex.ToString(), "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            NativeMemoryUtils.FreeUnmanagedBuf(buf);

            return stylusName;
        }

        /// <summary>
        /// Return the extension mask for the given tag.
        /// </summary>
        /// <param name="tag_I"></param>
        /// <returns></returns>
        public static UInt32 GetExtensionMask(EWTXExtensionTag tag_I)
        {
            UInt32 extMask = 0;
            IntPtr buf = NativeMemoryUtils.AllocUnmanagedBuf(extMask);

            try
            {
                UInt32 extIndex = FindWTXExtensionIndex(tag_I);

                // Supported if extIndex != -1
                if (extIndex != 0xffffffff)
                {
                    int size = (int)NativeDeviceFunctions.WTInfoA(
                        (uint)EWTICategoryIndex.WTI_EXTENSIONS + extIndex,
                        (uint)EWTIExtensionIndex.EXT_MASK, buf);

                    extMask = NativeMemoryUtils.MarshalUnmanagedBuf<UInt32>(buf, size);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("FAILED GetExtensionMask: " + ex.ToString(), "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            NativeMemoryUtils.FreeUnmanagedBuf(buf);

            return extMask;
        }

        /// <summary>
        /// Returns extension index for given tag, if possible.
        /// </summary>
        /// <param name="tag_I">type of extension being searched for</param>
        /// <returns></returns>
        private static UInt32 FindWTXExtensionIndex(EWTXExtensionTag tag_I)
        {
            UInt32 thisTag = 0;
            UInt32 extIndex = 0xffffffff;
            IntPtr buf = NativeMemoryUtils.AllocUnmanagedBuf(thisTag);

            for (Int32 loopIdx = 0, size = -1; size != 0; loopIdx++)
            {
                size = (int)NativeDeviceFunctions.WTInfoA(
                    (uint)EWTICategoryIndex.WTI_EXTENSIONS + (UInt32)loopIdx,
                    (uint)EWTIExtensionIndex.EXT_TAG, buf);

                if (size > 0)
                {
                    thisTag = NativeMemoryUtils.MarshalUnmanagedBuf<UInt32>(buf, size);

                    if ((EWTXExtensionTag)thisTag == tag_I)
                    {
                        extIndex = (UInt32)loopIdx;
                        break;
                    }
                }
            }

            NativeMemoryUtils.FreeUnmanagedBuf(buf);

            return extIndex;
        }


        /// <summary>
        /// Return max normal pressure supported by tablet.
        /// </summary>
        /// <param name="getNormalPressure_I">TRUE=> normal pressure; 
        /// FALSE=> tangential pressure (not supported on all tablets)</param>
        /// <returns>maximum pressure value or zero on error</returns>
        public static Int32 GetMaxPressure(bool getNormalPressure_I = true)
        {
            WintabAxis pressureAxis = new WintabAxis();
            IntPtr buf = NativeMemoryUtils.AllocUnmanagedBuf(pressureAxis);

            EWTIDevicesIndex devIdx = (getNormalPressure_I ?
                EWTIDevicesIndex.DVC_NPRESSURE : EWTIDevicesIndex.DVC_TPRESSURE);

            try
            {
                int size = (int)NativeDeviceFunctions.WTInfoA(
                    (uint)EWTICategoryIndex.WTI_DEVICES,
                    (uint)devIdx, buf);

                pressureAxis = NativeMemoryUtils.MarshalUnmanagedBuf<WintabAxis>(buf, size);
            }
            catch (Exception ex)
            {
                MessageBox.Show("FAILED GetMaxPressure: " + ex.ToString(), "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            NativeMemoryUtils.FreeUnmanagedBuf(buf);

            return pressureAxis.axMax;
        }

    }

}
