
namespace NativeDeviceControlLib.Utils
{
    public class STUDataPack
    {
        public uint pktTime;
        public ushort pktPressure;
        public ushort pktX;
        public ushort pktY;

        public STUDataPack(long time, ushort pressure, ushort X, ushort Y)
        {
            this.pktTime = System.Convert.ToUInt32(time);
            this.pktPressure = pressure;
            this.pktX = X;
            this.pktY = Y;
        }
    }
    public class Button_ScreenDevice
    {
        public System.Drawing.Rectangle Bounds;
        public CommonEventHandler clickHandler;
    }
    public static class DeviceModelButtons
    {
        //STU500
        public static int STU500_W = 160,
        STU500_H = 50,
        STU500_Accept_X = 20,
        STU500_Accept_Y = 417,

        STU500_Clear_X = 241,
        STU500_Clear_Y = 417,

        STU500_Seal_X = 460,
        STU500_Seal_Y = 417,

        STU500_Close_X = 600,
        STU500_Close_Y = 13,
        STU500_Close_W = 22,
        STU500_Close_H = 22,

        //STU520
        STU520_W = 180,
        STU520_H = 55,
        STU520_Accept_X = 40,
        STU520_Accept_Y = 404,

        STU520_Clear_X = 302,
        STU520_Clear_Y = 404,

        STU520_Seal_X = 580,
        STU520_Seal_Y = 404,

        STU520_Close_X = 743,
        STU520_Close_Y = 6,
        STU520_Close_W = 45,
        STU520_Close_H = 45,

        //STU520
        STU530_W = 180,
        STU530_H = 55,
        STU530_Accept_X = 40,
        STU530_Accept_Y = 404,

        STU530_Clear_X = 302,
        STU530_Clear_Y = 404,

        STU530_Seal_X = 580,
        STU530_Seal_Y = 404,

        STU530_Close_X = 743,
        STU530_Close_Y = 6,
        STU530_Close_W = 45,
        STU530_Close_H = 45;

    }
    public enum DeviceModel { STU300, STU430, STU500, STU520, STU530, Unknown }
    public delegate void CommonEventHandler();
    public delegate void STUGetDataPack_EventHandler(STUDataPack dataPack, bool flag_ScribbleCommand);
    public delegate void STUScribbleClient_EventHandler(System.Drawing.Pen pen, System.Drawing.PointF p1, System.Drawing.PointF p2);
}
