using NativeDeviceControlLib;
using NativeDeviceControlLib.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SignatorDocSolution
{
    class SignatureDeviceDrawSTU : IDisposable
    {
        private BiometricData biometricData = new BiometricData();
        private System.Windows.Forms.Timer timerDeviceElapsed = null;
        private bool enabledTimer = false;
        private SignatureField signatureFieldContainer;
        private STUGetDataPack_EventHandler stuGetDataPack_EventHandler;
        private STUScribbleClient_EventHandler stuScribbleClient_EventHandler;
        private CommonEventHandler stuDeviceClearSignature_EventHandler;
        private CommonEventHandler stuDeviceAcceptSignature_EventHandler;
        private CommonEventHandler stuDeviceSealSignature_EventHandler;
        private CommonEventHandler stuDeviceCloseSignature_EventHandler;
        private Graphics gfx;

        public SignatureDeviceDrawSTU(SignatureField signatureField)
        {
            if (!NativeDeviceSTU.isInitialized)
            {
                NativeDeviceSTU.Initialize();
            }
            this.signatureFieldContainer = signatureField;
            this.signatureFieldContainer.Image = new Bitmap(this.signatureFieldContainer.Size.Width, this.signatureFieldContainer.Size.Height);
            this.gfx = Graphics.FromImage(this.signatureFieldContainer.Image);

            this.stuGetDataPack_EventHandler = new STUGetDataPack_EventHandler(getDeviceDataPack);
            this.stuScribbleClient_EventHandler = new STUScribbleClient_EventHandler(scribbleClientContainer);
            this.stuDeviceClearSignature_EventHandler = new CommonEventHandler(deviceClearSignatureClick);
            this.stuDeviceAcceptSignature_EventHandler = new CommonEventHandler(deviceAcceptSignatureClick);
            this.stuDeviceSealSignature_EventHandler = new CommonEventHandler(deviceSealSignatureClick);
            this.stuDeviceCloseSignature_EventHandler = new CommonEventHandler(deviceCloseSignatureClick);

            this.enabledTimer=true;
            this.instantiate_timerDeviceElapsed();

            NativeDeviceSTU.swapExternalEventsHandler(this.stuGetDataPack_EventHandler, this.stuScribbleClient_EventHandler, this.stuDeviceClearSignature_EventHandler, this.stuDeviceAcceptSignature_EventHandler, this.stuDeviceSealSignature_EventHandler, this.stuDeviceCloseSignature_EventHandler);
            NativeDeviceSTU.setClientSize(new Size(this.signatureFieldContainer.Width, this.signatureFieldContainer.Height));
            NativeDeviceSTU.performSign();
            NativeDeviceSTU.setActiveDevice(true);
            if (signatureFieldContainer.isDesactiveScreenAnyCommands) 
                NativeDeviceSTU.desactiveCommands = true;
            else
                NativeDeviceSTU.desactiveCommands = false;
        }

        private void instantiate_timerDeviceElapsed()
        {
            if (this.timerDeviceElapsed == null)
            {
                this.timerDeviceElapsed = new System.Windows.Forms.Timer();
                this.timerDeviceElapsed.Tick+= new EventHandler(timerDeviceElapsed_Tick);
                this.timerDeviceElapsed.Interval = 5000;
            }
        }

        private void timerDeviceElapsed_Tick(object sender, EventArgs e)
        {
            if (this.timerDeviceElapsed.Enabled)
                this.timerDeviceElapsed.Stop();
            enabledTimer = false;
            this.afterSignature();
            NativeDeviceSTU.finishCapture();
        }

        private void scribbleClientContainer(Pen pen, PointF p1, PointF p2)
        {
                this.gfx.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                this.gfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                this.gfx.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                this.gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                this.gfx.DrawLine(pen, p1, p2);

                this.signatureFieldContainer.Invalidate();
        }

        private void getDeviceDataPack(STUDataPack dataPack, bool flagScbCommand)
        {
            if (this.timerDeviceElapsed.Enabled)
                this.timerDeviceElapsed.Stop();

            if (flagScbCommand) {
                
               enabledTimer = false;
                return;
            }

            if (this.enabledTimer)
            {
                this.timerDeviceElapsed.Start();
                this.biometricData.AddInfo(dataPack.pktX, dataPack.pktY, dataPack.pktPressure, dataPack.pktTime);
            }
            this.enabledTimer = true;
        }

        public void deviceAcceptSignatureClick()
        {
            if (this.timerDeviceElapsed.Enabled)
                this.timerDeviceElapsed.Stop();
            this.afterSignature();
        }
    
        public void deviceSealSignatureClick()
        {

            if (this.timerDeviceElapsed.Enabled)
                this.timerDeviceElapsed.Stop();
            this.afterSignature();
            this.signatureFieldContainer.deviceSealScreenClick();
        }

        public void deviceCloseSignatureClick()
        {
            if (this.timerDeviceElapsed.Enabled)
                this.timerDeviceElapsed.Stop();
            ((System.Windows.Forms.Button)(this.signatureFieldContainer.Controls["SigField_CloseButton"])).PerformClick();
        }

        private void deviceClearSignatureClick()
        {
            if (this.timerDeviceElapsed.Enabled)
                this.timerDeviceElapsed.Stop();
            this.signatureFieldContainer.Image = new Bitmap(this.signatureFieldContainer.Size.Width, this.signatureFieldContainer.Size.Height);
            this.gfx = Graphics.FromImage(this.signatureFieldContainer.Image);
            enabledTimer = true;
        }

        private void afterSignature()
        {
            NativeDeviceSTU.setActiveDevice(false);
            this.signatureFieldContainer.tempHandWrittenImage = this.signatureFieldContainer.Image;
            this.signatureFieldContainer.setIsHandWrittenDone(true);
            this.signatureFieldContainer.setAlreadyToSign(true);
            this.signatureFieldContainer.setDataBioSig(this.biometricData.getBioData());
            this.signatureFieldContainer.Show_btn_SigFieldRepeat(true);

            if (this.signatureFieldContainer.SigFieldSealType.ToString().Contains("SigImageStamp") && this.signatureFieldContainer.stampHasBeenSelected)
            {
                this.signatureFieldContainer.setSigHandWrittenStamp(this.signatureFieldContainer.tempStampImage);
            }
            else
            {
                this.signatureFieldContainer.clearSigImageStamp();
            }
            this.signatureFieldContainer.fireAfterSign();
        }

        public void abortBusySign()
        {
            NativeDeviceSTU.setActiveDevice(false);
            this.signatureFieldContainer.tempHandWrittenImage = this.signatureFieldContainer.Image;
            this.signatureFieldContainer.setIsHandWrittenDone(false);
            this.signatureFieldContainer.setAlreadyToSign(false);
            this.signatureFieldContainer.setDataBioSig(null);
            this.signatureFieldContainer.Show_btn_SigFieldRepeat(true);

            if (this.signatureFieldContainer.SigFieldSealType.ToString().Contains("SigImageStamp") && this.signatureFieldContainer.stampHasBeenSelected)
            {
                this.signatureFieldContainer.setSigHandWrittenStamp(this.signatureFieldContainer.tempStampImage);
            }
            else
            {
                this.signatureFieldContainer.clearSigImageStamp();
            }
            NativeDeviceSTU.finishCapture();
        }

        public void clearScreenDeviceBusy(){
            NativeDeviceSTU.finishCapture();
        }

        private void swapEvents() {
            NativeDeviceSTU.swapExternalEventsHandler(this.stuGetDataPack_EventHandler, this.stuScribbleClient_EventHandler, this.stuDeviceClearSignature_EventHandler, this.stuDeviceAcceptSignature_EventHandler, this.stuDeviceSealSignature_EventHandler, this.stuDeviceCloseSignature_EventHandler);
        }

        public void setScribbleConfig() {
            this.swapEvents();
            this.signatureFieldContainer.Image = new Bitmap(this.signatureFieldContainer.Size.Width, this.signatureFieldContainer.Size.Height);
            this.gfx = Graphics.FromImage(this.signatureFieldContainer.Image);
            NativeDeviceSTU.performSign();
            NativeDeviceSTU.setActiveDevice(true);
            if (signatureFieldContainer.isDesactiveScreenAnyCommands)
                NativeDeviceSTU.desactiveCommands = true;
            else
                NativeDeviceSTU.desactiveCommands = false;
            
            this.enabledTimer = true;
        }

        public void release() {
            NativeDeviceSTU.finishCapture();
        }

        #region Dispose
        private bool disposed;
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~SignatureDeviceDrawSTU()
        {
            this.Dispose(false);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                }
            }

            disposed = true;
        }

        public void DisposeContext()
        {
            this.Dispose();
        }
        #endregion

        public static void releaseDeviceBusy() {
            NativeDeviceSTU.finishCapture();
        }

    }

}
