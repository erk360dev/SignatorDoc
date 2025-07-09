using NativeDeviceControlLib.Utils;
using System;
using System.Drawing;
using System.Windows.Forms;
using wgssSTU;

namespace NativeDeviceControlLib
{
    public class NativeDeviceSTU
    {
        private static IUsbDevice usbDevice;
        private static Tablet m_tablet;
        private static ICapability m_capability;
        private static System.Collections.Generic.List<IPenData> m_penData;
        private static System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();    
        private static DeviceModel deviceModel;
        private static Button_ScreenDevice[] screenButtons;
        private static Pen m_pen = new Pen(Color.Blue);
        private static Size clientSize;
        private static bool isPenDown = false;
        private static bool enableCountTime = false;
        private static bool enableScribbling = false;
        private static bool activeDevice = false;
        private static bool useColor = false;
        private static bool flagSribbleCommand = false;
        public static bool isInitialized = false;
        public static bool deviceAlredyDetected = false;
        public static bool desactiveCommands=false;
        private static byte encodingMode;
        private static byte[] backGroundImageDeviceData;
        private static byte[] BGSignImageDeviceData;
        private static int buttonNumberClicked = -1;
        private static int countInitialClick = 0;
        private static float pressureFactor = 0;
        private static float constPressure = 1.2F;
        private static STUGetDataPack_EventHandler rcvdDataPack_EventHandler=null;
        private static STUScribbleClient_EventHandler scribbleClient_EventHandler=null;
        private static CommonEventHandler clearClientContainer_EventHandler = null;
        private static CommonEventHandler acceptClientContainer_EventHandler = null;
        private static CommonEventHandler sealClientContainer_EventHandler = null;
        private static CommonEventHandler closeClientContainer_EventHandler = null;
        private static CommonEventHandler startScreenClick_EventHandler = null;

        public static void Initialize()
        {
            if (detectSTUDevice())
            {
                m_tablet = new Tablet();

                IErrorCode tabletStatus = m_tablet.usbConnect(usbDevice, true);
                if (tabletStatus.value == 0)
                {
                    m_capability = m_tablet.getCapability();
                    detectDeviceModel();
                    configureEncodedingMode();
                    configureScreenButtons();
                }
                else
                {
                    MessageBox.Show(tabletStatus.message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (useColor)
                {
                    handwritingThicknessColor thc = new handwritingThicknessColor();
                    thc.penColor = 31;
                    thc.penThickness = 1;
                    m_tablet.setHandwritingThicknessColor(thc);
                }
                pressureFactor = constPressure / m_capability.tabletMaxPressure;
                m_tablet.onPenData += new ITabletEvents2_onPenDataEventHandler(onPenDataFromDevice);
                m_tablet.onGetReportException += new ITabletEvents2_onGetReportExceptionEventHandler(onGetReportException);
                m_tablet.setInkingMode(0x01);

                m_penData = new System.Collections.Generic.List<IPenData>();

                //m_pen.Width = 1F;
                isInitialized = true;
            }
        }

        public static bool detectSTUDevice()
        {
            try
            {
                if (usbDevice == null)
                {
                    UsbDevices listUsbDevices = new UsbDevices();
                    deviceAlredyDetected = false;
                    if (listUsbDevices.Count != 0)
                    {
                        usbDevice = listUsbDevices[0];
                        deviceAlredyDetected = true;
                        return true;
                    }
                    else
                        return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }

        private static void onPenDataFromDevice(IPenData penData)
        {
            if (!activeDevice)
            {
                if (!enableScribbling)
                {
                    if (penData.sw != 0)
                        countInitialClick++;
                    if (countInitialClick > 1 && penData.sw == 0)
                        startScreenClick_EventHandler.Invoke();
                }
                return;
            }

            if (!enableScribbling)
            {
                return;
            }

            if (!enableCountTime)
            {
                stopWatch.Start();
                enableCountTime = true;
            }

            flagSribbleCommand = false;
            if (penData.sw != 0)
            {

                Point pointButtonClick = tabletToScreen(penData);
                buttonNumberClicked = -1;
                for (int i = 0; i < screenButtons.Length; i++)
                {
                    if (screenButtons[i].Bounds.Contains(pointButtonClick))
                    {
                        buttonNumberClicked = i;
                        break;
                    }
                }

                if (!isPenDown)
                {
                    if (buttonNumberClicked >= 0)
                        isPenDown = false;
                    else
                        isPenDown = true;
                }

                if (m_penData.Count != 0 && isPenDown)
                {
                    IPenData prevPenData = m_penData[m_penData.Count - 1];
                    PointF prev = tabletToClient(prevPenData);
                    m_pen.Width = 0.5F + (pressureFactor * penData.pressure);
                    scribbleClient_EventHandler.Invoke(m_pen, prev, tabletToClient(penData));
                }

                if (isPenDown)
                    m_penData.Add(penData);
            }
            else
            {
                if (m_penData.Count != 0)
                    m_penData.Add(penData);

                if (isPenDown)
                    isPenDown = false;
                else
                    if (buttonNumberClicked >= 0)
                    {
                        stopWatch.Stop();
                        flagSribbleCommand = true;
                        if (desactiveCommands && buttonNumberClicked > 1)
                            buttonNumberClicked = -1;
                        else{
                            screenButtons[buttonNumberClicked].clickHandler.Invoke();
                            buttonNumberClicked = -1;
                        }
                    }
            }
            rcvdDataPack_EventHandler.Invoke(new STUDataPack(stopWatch.ElapsedMilliseconds, penData.pressure, penData.x, penData.y), flagSribbleCommand);
        }

        public static void setActiveDevice(bool activated) {
            activeDevice = activated;
        }

        private static void onGetReportException(ITabletEventsException pException)
        {
            pException.getException();
        }

        public static void detectDeviceModel()
        {
            IInformation tabletInformation = m_tablet.getInformation();

            if (tabletInformation.modelName.ToUpper().Contains("STU"))
            {
                deviceModel = tabletInformation.modelName.Contains("300") ? DeviceModel.STU300 :
                              tabletInformation.modelName.Contains("430") ? DeviceModel.STU430 :
                              tabletInformation.modelName.Contains("500") ? DeviceModel.STU500 :
                              tabletInformation.modelName.Contains("520") ? DeviceModel.STU520 :
                              tabletInformation.modelName.Contains("530") ? DeviceModel.STU530 : DeviceModel.Unknown;
            }
            else
                deviceModel = DeviceModel.Unknown;
        }

        public static void clearScreen()
        {
            stopWatch.Reset();
            m_tablet.writeImage(encodingMode, backGroundImageDeviceData);
            m_penData.Clear();
            isPenDown = false;
            buttonNumberClicked = -1;
            enableScribbling = false;
            countInitialClick = 0;
            enableCountTime = false;
        }

        public static void setClientSize(Size csize)
        {
            clientSize = csize;
        }

        public static void clearSignatureScreen()
        {
            stopWatch.Reset();
            m_tablet.writeImage(encodingMode, BGSignImageDeviceData);
            m_penData.Clear();
            isPenDown = false;
            buttonNumberClicked = -1;
            countInitialClick = 0;
            enableCountTime = false;
        }

        public static void configureEncodedingMode()
        {
            encodingMode m_encodingMode;
            ProtocolHelper protocolHelper = new ProtocolHelper();
            ushort idP = m_tablet.getProductId();
            useColor = false;
            bool useZlibCompression = false;
            encodingFlag encodingFlag = (encodingFlag)protocolHelper.simulateEncodingFlag(idP, 0);

            if ((encodingFlag & (wgssSTU.encodingFlag.EncodingFlag_16bit | wgssSTU.encodingFlag.EncodingFlag_24bit)) != 0)
                if (m_tablet.supportsWrite())
                    useColor = true;
            if ((encodingFlag & wgssSTU.encodingFlag.EncodingFlag_24bit) != 0)
                m_encodingMode = m_tablet.supportsWrite() ? wgssSTU.encodingMode.EncodingMode_24bit_Bulk : wgssSTU.encodingMode.EncodingMode_24bit;
            else
                if ((encodingFlag & wgssSTU.encodingFlag.EncodingFlag_16bit) != 0)
                    m_encodingMode = m_tablet.supportsWrite() ? wgssSTU.encodingMode.EncodingMode_16bit_Bulk : wgssSTU.encodingMode.EncodingMode_16bit;
                else
                    m_encodingMode = wgssSTU.encodingMode.EncodingMode_1bit;

            if (!useColor && useZlibCompression)
            {
                m_encodingMode |= wgssSTU.encodingMode.EncodingMode_Zlib;
            }
            encodingMode = (byte)m_encodingMode;

            using (Bitmap bitmap = getDeviceBG_Image())
            {
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                backGroundImageDeviceData = (byte[])protocolHelper.resizeAndFlatten(stream.ToArray(), 0, 0, (uint)bitmap.Width, (uint)bitmap.Height, m_capability.screenWidth, m_capability.screenHeight, (byte)m_encodingMode, Scale.Scale_Fit, 0, 0);
                stream.Dispose();
            }

            using (Bitmap bitmap = getDeviceBGSign_Image())
            {
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                BGSignImageDeviceData = (byte[])protocolHelper.resizeAndFlatten(stream.ToArray(), 0, 0, (uint)bitmap.Width, (uint)bitmap.Height, m_capability.screenWidth, m_capability.screenHeight, (byte)m_encodingMode, Scale.Scale_Fit, 0, 0);
                stream.Dispose();
            }



        }

        public static Bitmap getDeviceBG_Image()
        {
            return deviceModel == DeviceModel.STU530 ? Properties.Resources.STU530 :
                   deviceModel == DeviceModel.STU520 ? Properties.Resources.STU520 :
                   deviceModel == DeviceModel.STU500 ? Properties.Resources.STU500 :
                   deviceModel == DeviceModel.STU430 ? Properties.Resources.STU430 :
                   deviceModel == DeviceModel.STU300 ? Properties.Resources.STU300 : null;
        }

        public static Bitmap getDeviceBGSign_Image()
        {
            return deviceModel == DeviceModel.STU530 ? Properties.Resources.STU530Sign :
                   deviceModel == DeviceModel.STU520 ? Properties.Resources.STU520Sign :
                   deviceModel == DeviceModel.STU500 ? Properties.Resources.STU500Sign :
                   deviceModel == DeviceModel.STU430 ? Properties.Resources.STU430 :
                   deviceModel == DeviceModel.STU300 ? Properties.Resources.STU300 : null;
        }

        private static PointF tabletToClient(wgssSTU.IPenData penData)
        {
            return new PointF((float)penData.x * clientSize.Width / m_capability.tabletMaxX, (float)penData.y * clientSize.Height / m_capability.tabletMaxY);
        }

        private static Point tabletToScreen(wgssSTU.IPenData penData)
        {
            return Point.Round(new PointF((float)penData.x * m_capability.screenWidth / m_capability.tabletMaxX, (float)penData.y * m_capability.screenHeight / m_capability.tabletMaxY));
        }

        private static void configureScreenButtons()
        {
            screenButtons = new Button_ScreenDevice[4];
            screenButtons[0] = new Button_ScreenDevice(); //Accept
            screenButtons[1] = new Button_ScreenDevice(); //Clear
            screenButtons[2] = new Button_ScreenDevice(); //Seal
            screenButtons[3] = new Button_ScreenDevice(); //Close

            if (usbDevice.idProduct != 0x00a2)
            {
                switch (deviceModel)
                {
                    case DeviceModel.STU500:
                        {
                            screenButtons[0].Bounds = new System.Drawing.Rectangle(DeviceModelButtons.STU500_Accept_X, DeviceModelButtons.STU500_Accept_Y, DeviceModelButtons.STU500_W, DeviceModelButtons.STU500_H);
                            screenButtons[1].Bounds = new System.Drawing.Rectangle(DeviceModelButtons.STU500_Clear_X, DeviceModelButtons.STU500_Clear_Y, DeviceModelButtons.STU500_W, DeviceModelButtons.STU500_H);
                            screenButtons[2].Bounds = new System.Drawing.Rectangle(DeviceModelButtons.STU500_Seal_X, DeviceModelButtons.STU500_Seal_Y, DeviceModelButtons.STU500_W, DeviceModelButtons.STU500_H);
                            screenButtons[3].Bounds = new System.Drawing.Rectangle(DeviceModelButtons.STU500_Close_X, DeviceModelButtons.STU500_Close_Y, DeviceModelButtons.STU500_Close_W, DeviceModelButtons.STU500_Close_H);
                            break;
                        }
                    case DeviceModel.STU520:
                        {
                            screenButtons[0].Bounds = new System.Drawing.Rectangle(DeviceModelButtons.STU520_Accept_X, DeviceModelButtons.STU520_Accept_Y, DeviceModelButtons.STU520_W, DeviceModelButtons.STU520_H);
                            screenButtons[1].Bounds = new System.Drawing.Rectangle(DeviceModelButtons.STU520_Clear_X, DeviceModelButtons.STU520_Clear_Y, DeviceModelButtons.STU520_W, DeviceModelButtons.STU520_H);
                            screenButtons[2].Bounds = new System.Drawing.Rectangle(DeviceModelButtons.STU520_Seal_X, DeviceModelButtons.STU520_Seal_Y, DeviceModelButtons.STU520_W, DeviceModelButtons.STU520_H);
                            screenButtons[3].Bounds = new System.Drawing.Rectangle(DeviceModelButtons.STU520_Close_X, DeviceModelButtons.STU520_Close_Y, DeviceModelButtons.STU520_Close_W, DeviceModelButtons.STU520_Close_H);
                            break;
                        }
                    case DeviceModel.STU530:
                        {
                            screenButtons[0].Bounds = new System.Drawing.Rectangle(DeviceModelButtons.STU530_Accept_X, DeviceModelButtons.STU530_Accept_Y, DeviceModelButtons.STU530_W, DeviceModelButtons.STU530_H);
                            screenButtons[1].Bounds = new System.Drawing.Rectangle(DeviceModelButtons.STU530_Clear_X, DeviceModelButtons.STU530_Clear_Y, DeviceModelButtons.STU530_W, DeviceModelButtons.STU530_H);
                            screenButtons[2].Bounds = new System.Drawing.Rectangle(DeviceModelButtons.STU530_Seal_X, DeviceModelButtons.STU530_Seal_Y, DeviceModelButtons.STU530_W, DeviceModelButtons.STU530_H);
                            screenButtons[3].Bounds = new System.Drawing.Rectangle(DeviceModelButtons.STU530_Close_X, DeviceModelButtons.STU530_Close_Y, DeviceModelButtons.STU530_Close_W, DeviceModelButtons.STU530_Close_H);
                            break;
                        }
                    default:
                        throw new System.Exception("Este dispositivo não permite a interação com botões.");
                }
                screenButtons[0].clickHandler += new CommonEventHandler(acceptScreenClick);
                screenButtons[1].clickHandler += new CommonEventHandler(clearScreenClick);
                screenButtons[2].clickHandler += new CommonEventHandler(sealScreenClick);
                screenButtons[3].clickHandler += new CommonEventHandler(closeScreenClick);

            }
            else
            {
                throw new System.Exception("Este dispositivo não permite a interação com botões");
            }


        }

        private static void clearScreenClick()
        {
            stopWatch.Stop();
            clearSignatureScreen();
            m_penData.Clear();
            isPenDown = false;
            buttonNumberClicked = -1;
            clearClientContainer_EventHandler.Invoke();
        }

        private static void acceptScreenClick()
        {
            stopWatch.Stop();
            clearScreen();
            acceptClientContainer_EventHandler.Invoke();
        }

        private static void sealScreenClick()
        {
            stopWatch.Stop();
            if (m_penData.Count > 0)
            {
                clearScreen();
                sealClientContainer_EventHandler.Invoke();
            }
        }

        private static void closeScreenClick()
        {
            closeClientContainer_EventHandler.Invoke();
        }

        public static void performSign()
        {
            clearSignatureScreen();
            enableScribbling = true;
            countInitialClick = 0;
        }

        public static void releaseResources()
        {
            clearScreen();
            isInitialized = false;
            deviceAlredyDetected = false;
            m_tablet.endCapture();
            m_tablet.disconnect();         
        }

        public static void disconectDevice() {
            m_tablet.endCapture();
            m_tablet.disconnect();   
        }

        public static void setEnableScribbling(bool enable) {
            enableScribbling = enable;
        }

        public static void swapExternalEventsHandler(STUGetDataPack_EventHandler evt1, STUScribbleClient_EventHandler evt2, CommonEventHandler evt3, CommonEventHandler evt4, CommonEventHandler evt5, CommonEventHandler evt6)
        {
            if(rcvdDataPack_EventHandler!=null)
                rcvdDataPack_EventHandler -= rcvdDataPack_EventHandler;
            if(scribbleClient_EventHandler!=null)
                scribbleClient_EventHandler -= scribbleClient_EventHandler;
            if (clearClientContainer_EventHandler != null)
                clearClientContainer_EventHandler -= clearClientContainer_EventHandler;
            if (acceptClientContainer_EventHandler != null)
                acceptClientContainer_EventHandler -= acceptClientContainer_EventHandler;
            if (sealClientContainer_EventHandler != null)
                sealClientContainer_EventHandler -= sealClientContainer_EventHandler;
            if (closeClientContainer_EventHandler != null)
                closeClientContainer_EventHandler -= closeClientContainer_EventHandler;

            rcvdDataPack_EventHandler += evt1;
            scribbleClient_EventHandler += evt2;
            clearClientContainer_EventHandler += evt3;
            acceptClientContainer_EventHandler += evt4;
            sealClientContainer_EventHandler += evt5;
            closeClientContainer_EventHandler += evt6;
        }

        public static void setExternalEventHandler_StartClick(CommonEventHandler evt) {
            if (startScreenClick_EventHandler != null)
                startScreenClick_EventHandler -= startScreenClick_EventHandler;
            startScreenClick_EventHandler += evt;
        }

        public static void showIntroduction() {
            m_tablet = new Tablet();
            IErrorCode tabletStatus = m_tablet.usbConnect(usbDevice, true);
            if (tabletStatus.value == 0)
            {
                m_capability = m_tablet.getCapability();
                detectDeviceModel();
                configureEncodedingMode();
                configureScreenButtons();
            }
            else
            {
                MessageBox.Show(tabletStatus.message,"Aviso",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }
            if (useColor)
            {
                handwritingThicknessColor thc = new handwritingThicknessColor();
                thc.penColor = 31;
                thc.penThickness = 1;
                m_tablet.setHandwritingThicknessColor(thc);
            }
            pressureFactor = constPressure / m_capability.tabletMaxPressure;

            m_tablet.onPenData += new ITabletEvents2_onPenDataEventHandler(onPenDataFromDevice);
            m_tablet.onGetReportException += new ITabletEvents2_onGetReportExceptionEventHandler(onGetReportException);
            m_tablet.setInkingMode(0x01);
            m_penData = new System.Collections.Generic.List<IPenData>();
            isInitialized = true;
            clearScreen();
            
        }

        public static void finishCapture(){
            stopWatch.Stop();
            clearScreen();
        }

    }


}
