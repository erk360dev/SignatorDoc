using NativeDeviceControlLib;
using System;
using System.Drawing;
namespace SignatorDocSolution
{
    /// <summary>
    /// CLASS_ID=05;
    /// </summary>
    class SignatureDeviceDraw : IDisposable
    {
        #region Device Atributes
        private NativeDeviceContext m_logContext = null;
        private NativeDeviceData m_wtData = null;
        private UInt32 m_maxPkts = 1;   // max num pkts to capture/display at a time

        private Int32 m_pkX = 0;
        private Int32 m_pkY = 0;
        private UInt32 m_pressure = 0;
        private UInt32 m_pkTime = 0;
        private UInt32 m_pkTimeLast = 0;
        private UInt32 m_pkTimeFirst = 0;
        private UInt32 currentTime = 0;
        private UInt32 diffTime = 0;

        private Point m_lastPoint = Point.Empty;
        private Graphics m_graphics;
        private Pen m_pen;
        private Pen m_backPen;
        private float penWithFactor =0.5F;

        private const Int32 m_TABEXTX = 10000;
        private const Int32 m_TABEXTY = 10000;

        private System.Windows.Forms.Timer tmDdeviceElapsed = null;
        private bool captureRelease = false;

        public HCTX HLogContext { get { return m_logContext.HCtx; } }

        private bool disposed;
        BiometricData biometricData = new BiometricData();

        #endregion

        private SignatureField SignatureDeviceContainer = null;

        public SignatureDeviceDraw(SignatureField sigDeviceContainer)
        {
            this.SignatureDeviceContainer = sigDeviceContainer;
            this.penWithFactor= 1.1F/NativeDeviceInfo.GetMaxPressure();
            this.ClearDisplay();
            this.instantiate_tmrDeviceElapsed();
            this.Enable_Scribble(true);
            this.InitDataCapture(m_TABEXTX, m_TABEXTY, false);
        }

        #region Device Handle
        private void Enable_Scribble(bool enable = false)
        {
            if (enable)
            {
                m_maxPkts = 1;

                this.SignatureDeviceContainer.Image = new Bitmap(this.SignatureDeviceContainer.Size.Width, this.SignatureDeviceContainer.Size.Height);
                this.m_graphics = Graphics.FromImage(this.SignatureDeviceContainer.Image);
                this.m_graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                m_pen = new Pen(Color.Blue);
                m_backPen = new Pen(Color.White);
            }
            else
            {
                this.CloseCurrentContext();

                if (m_graphics != null)
                {
                    this.SignatureDeviceContainer.Invalidate();
                    m_graphics = null;
                }
            }
        }

        private void CloseCurrentContext()
        {
            try
            {
                if (this.m_logContext != null)
                {
                    this.m_logContext.Close();
                    this.m_logContext = null;
                    this.m_wtData = null;
                }

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString(), "Erro", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        private void InitDataCapture(int ctxHeight_I = m_TABEXTX, int ctxWidth_I = m_TABEXTY, bool ctrlSysCursor_I = true)
        {
            try
            {
                this.CloseCurrentContext();

                m_logContext = OpenTestDigitizerContext(ctxWidth_I, ctxHeight_I, ctrlSysCursor_I);

                if (m_logContext == null)
                {
                    return;
                }

                m_wtData = new NativeDeviceData(m_logContext);
                m_wtData.SetWTPacketEventHandler(devicePacketEventHandler);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message,"Erro",System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        private NativeDeviceContext OpenTestDigitizerContext(int width_I = m_TABEXTX, int height_I = m_TABEXTY, bool ctrlSysCursor = true)
        {
            bool status = false;
            NativeDeviceContext logContext = null;

            try
            {
                logContext = NativeDeviceInfo.GetDefaultDigitizingContext(ECTXOptionValues.CXO_MESSAGES);

                if (ctrlSysCursor)
                {
                    logContext.Options |= (uint)ECTXOptionValues.CXO_SYSTEM;
                }

                if (logContext == null)
                {
                    System.Windows.Forms.MessageBox.Show("Falha na tentativa de obter um contexto digitalizado.\n", "Erro", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return null;
                }

                logContext.Name = "WintabDN Event Data Context";

                logContext.OutOrgX = logContext.OutOrgY = 0;
                logContext.OutExtX = width_I;
                logContext.OutExtY = height_I;

                status = logContext.Open();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("OpenTestDigitizerContext ERRO: " + ex.ToString(), "Erro", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return logContext;
        }

        public void devicePacketEventHandler(Object sender_I, MessageReceivedEventArgs eventArgs_I)
        {
            if (m_wtData == null)
            {
                return;
            }

            try
            {
                if (m_maxPkts == 1)
                {
                    uint pktID = (uint)eventArgs_I.Message.WParam;
                    WintabPacket pkt = m_wtData.GetDataPacket(pktID);

                    if (pkt.pkContext != 0)
                    {

                        if (!captureRelease)
                        {
                            captureRelease = true;
                            this.tmDdeviceElapsed.Start();
                            this.m_pkTimeFirst = pkt.pkTime;
                        }
                        else
                        {
                            this.tmDdeviceElapsed.Stop();
                            this.tmDdeviceElapsed.Start();
                        }

                        m_pkX = pkt.pkX;
                        m_pkY = pkt.pkY;
                        m_pressure = pkt.pkNormalPressure.pkAbsoluteNormalPressure;

                        m_pkTime = pkt.pkTime;

                        if (m_graphics == null)
                        {
                            string nda = "nothing";
                        }
                        else
                        {
                            if (this.m_pkTimeLast != 0)
                            {
                                diffTime=m_pkTime - m_pkTimeLast;
                                currentTime+=diffTime;
                                diffTime =m_pkTimeLast-this.m_pkTimeFirst;
                                if (currentTime > diffTime)
                                    currentTime = diffTime;
                            }

                            this.biometricData.AddInfo(m_pkX, m_pkY, m_pressure, currentTime);//m_pkTime - this.m_pkTimeFirst);

                            int clientWidth = this.SignatureDeviceContainer.Width;
                            int clientHeight = this.SignatureDeviceContainer.Height;

                            int X = (int)((double)(m_pkX * clientWidth) / (double)m_TABEXTX);
                            int Y = (int)((double)clientHeight - ((double)(m_pkY * clientHeight) / (double)m_TABEXTY));

                            Point tabPoint = new Point(X, Y);

                            if (m_lastPoint.Equals(Point.Empty))
                            {
                                m_lastPoint = tabPoint;
                                m_pkTimeLast = m_pkTime;
                            }

                            m_pen.Width = 0.5F + (this.penWithFactor * m_pressure);// (float)(m_pressure / 1023);

                            if (m_pressure > 0)
                            {
                                if (m_pkTime - m_pkTimeLast < 5)
                                {
                                    m_graphics.DrawRectangle(m_pen, X, Y, 1, 1);
                                }
                                else
                                {
                                    m_graphics.DrawLine(m_pen, tabPoint, m_lastPoint);
                                }
                                this.SignatureDeviceContainer.Invalidate();
                            }
                            m_lastPoint = tabPoint;
                            m_pkTimeLast = m_pkTime;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("FAILED to get packet data: " + ex.ToString());
            }
        }

        private void ClearDisplay()
        {
            this.SignatureDeviceContainer.Invalidate();
        }

        private void instantiate_tmrDeviceElapsed()
        {
            if (this.tmDdeviceElapsed == null)
            {
                this.tmDdeviceElapsed = new System.Windows.Forms.Timer();
                this.tmDdeviceElapsed.Tick += new EventHandler(tmrDeviceElapsed_Tick);
                this.tmDdeviceElapsed.Interval = 2000;
            }
        }

        private void tmrDeviceElapsed_Tick(Object sender, EventArgs e)
        {
            this.tmDdeviceElapsed.Stop();
            this.captureRelease = false;
            this.Enable_Scribble(false);
            this.SignatureDeviceContainer.tempHandWrittenImage = this.SignatureDeviceContainer.Image;
            this.SignatureDeviceContainer.setIsHandWrittenDone(true);
            this.SignatureDeviceContainer.setAlreadyToSign(true);
            this.SignatureDeviceContainer.setDataBioSig(this.biometricData.getBioData());
            this.SignatureDeviceContainer.Show_btn_SigFieldRepeat(true);
            
            if (this.SignatureDeviceContainer.SigFieldSealType.ToString().Contains("SigImageStamp") && this.SignatureDeviceContainer.stampHasBeenSelected)
            {
                this.SignatureDeviceContainer.setSigHandWrittenStamp(this.SignatureDeviceContainer.tempStampImage);
            }
            else 
            {
                this.SignatureDeviceContainer.clearSigImageStamp();
            }
            this.SignatureDeviceContainer.fireAfterSign();
        }

        public void abortBusySign()
        {
            if (this.tmDdeviceElapsed.Enabled)
                this.tmDdeviceElapsed.Stop();

            this.captureRelease = false;
            this.Enable_Scribble(false);
            this.SignatureDeviceContainer.tempHandWrittenImage = this.SignatureDeviceContainer.Image;
            this.SignatureDeviceContainer.setIsHandWrittenDone(false);
            this.SignatureDeviceContainer.setAlreadyToSign(false);
            this.SignatureDeviceContainer.setDataBioSig(null);
            this.SignatureDeviceContainer.Show_btn_SigFieldRepeat(true);

            if (this.SignatureDeviceContainer.SigFieldSealType.ToString().Contains("SigImageStamp") && this.SignatureDeviceContainer.stampHasBeenSelected)
            {
                this.SignatureDeviceContainer.setSigHandWrittenStamp(this.SignatureDeviceContainer.tempStampImage);
            }
            else
            {
                this.SignatureDeviceContainer.clearSigImageStamp();
            }

        }

        #endregion

        #region Dispose
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~SignatureDeviceDraw()
        {
            this.Dispose(false);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources here.
                }

                // Dispose unmanaged resources here.
            }

            disposed = true;
        }

        public void DisposeContext()
        {
            this.Dispose();
        }
        #endregion

        public void setScribbleConfig() {
            this.ClearDisplay();
            this.Enable_Scribble(true);
            this.InitDataCapture(m_TABEXTX, m_TABEXTY, false);
        }
        
    }
}
