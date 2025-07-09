using SignatorDocSolution.Utils;
namespace SignatorDocSolution
{
    /// <summary>
    /// CLASS_ID=04;
    /// </summary>
    public class SignatureField : System.Windows.Forms.PictureBox
    {
        private System.Drawing.Point currentPointMouseButtonDownLocation;
        private int PageNumberAdded = 0;
        public int StampIndex = 0;
        private string dataBioSig = null;
        public System.Drawing.Image tempHandWrittenImage = null;
        public System.Drawing.Image tempStampImage = null;
        private SignatureDeviceDraw signatureDeviceDraw=null;
        private SignatureDeviceDrawSTU signatureDeviceDrawSTU = null;
        private System.Windows.Forms.ToolTip toolTip;
        private bool IsAlreadyToSeal = false;
        private bool allowMouseMove = false;
        private bool isSignatureDrawConfigured = false;
        private bool isHandWrittenDone = false;
        private bool someParsDisabled = false;
        public bool stampHasBeenSelected = false;
        public bool initialDeviceIsPresent = false;
        public bool isDesactiveScreenAnyCommands = false;
        public bool isBioAnaliseMode = false;
        private CustomEvent AfterSign = new CustomEvent();
        public SignatureField_SealType SigFieldSealType { get; private set; }
        public int deviceSerieNumber = 0;
        public event System.Windows.Forms.KeyEventHandler KeyDownEvent;
        private CustomEvent.GenericDelegateEventHandler DeviceScreenClick_Event;
               
        public Form_SignConfig frm_SignConfig= null;

        public SignatureField()
        {

        }

        public SignatureField(string signatureFieldID, System.Drawing.Point fieldLocation, int pageNumber, bool thereIsDeviceSigAvailable, System.EventHandler event_HandleClick_1, System.EventHandler event_HandleClick_2, CustomEvent.GenericDelegateEventHandler event_HandleClick_3, System.Windows.Forms.ToolTip tooltip)
        {
            this.SigFieldSealType = thereIsDeviceSigAvailable ? SignatureField_SealType.SigImage : SignatureField_SealType.Stamp;
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(SignatureField_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(SignatureField_MouseUp);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(SignatureField_MouseMove);
            this.DoubleClick += new System.EventHandler(SignatureField_DoubleClick);
            this.Size = new System.Drawing.Size(Defins.SignatureFieldSize.Width, Defins.SignatureFieldSize.Height);
            this.Location = fieldLocation;
            this.PageNumberAdded = pageNumber;
            this.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.BackColor = System.Drawing.Color.FromArgb(1, 255, 255, 255);
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            if (thereIsDeviceSigAvailable)
            {
                this.deviceSerieNumber = DetectDevice.deviceSerieNumber;
                this.BackgroundImage = global::SignatorDocSolution.Properties.Resources.Signature_Field;
                //  this.AfterSign.GenericEvent += new CustomEvent.GenericDelegateEventHandler(event_HandleClick_3); 
                this.AfterSign.Invoker = new CustomEvent.GenericDelegateEventHandler(event_HandleClick_3);
                if (this.deviceSerieNumber == 1)
                    DetectSTUDevice.setStartClickEventHandler(new NativeDeviceControlLib.Utils.CommonEventHandler(this.deviceStartClick_EventHandler));
            }
            else
            {
                this.BackgroundImage = global::SignatorDocSolution.Properties.Resources.EmptySignatureFieldSignedBG;
                this.Image = global::SignatorDocSolution.Properties.Resources.StampSignedDigitaly;
                this.StampIndex = 2;
                this.IsAlreadyToSeal = true;
            }
            this.Name = signatureFieldID;
            this.toolTip = tooltip;
            this.Controls.Add(createCloseButtonSigField(event_HandleClick_1));
            this.Controls.Add(createRepeatSigFieldButton());
            this.Controls.Add(createConfigSigFieldButton(event_HandleClick_2));
            this.initialDeviceIsPresent = thereIsDeviceSigAvailable;
        }

        public SignatureField(System.EventHandler event_HandlerClick_1)
        {
            bool isDevicePresent = false;
            if (DetectDevice.thereIsDeviceAvailable())
            {
                isDevicePresent = true;
                this.deviceSerieNumber = DetectDevice.deviceSerieNumber;
                if (this.deviceSerieNumber == 1)
                    DetectSTUDevice.setStartClickEventHandler(new NativeDeviceControlLib.Utils.CommonEventHandler(this.deviceStartClick_EventHandler));
            }
            this.Name = "SignatureField000";
            this.SigFieldSealType = isDevicePresent ? SignatureField_SealType.SigImage : SignatureField_SealType.Stamp;
            this.Size = new System.Drawing.Size(Defins.SignatureFieldSize.Width, Defins.SignatureFieldSize.Height);
            this.BackColor = System.Drawing.Color.FromArgb(1, 255, 255, 255);
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.DoubleClick += new System.EventHandler(SignatureField_DoubleClick);
          //this.Controls.Add(createCloseButtonSigField(new System.EventHandler(delegate(object o1, System.EventArgs e1) { this.Parent.Hide(); })));
            if (isDevicePresent)
            {
                this.BackgroundImage = global::SignatorDocSolution.Properties.Resources.Signature_Field;
                this.AfterSign.Invoker = new CustomEvent.GenericDelegateEventHandler(AfterSign_Event);
            }
            else
            {
                this.BackgroundImage = global::SignatorDocSolution.Properties.Resources.EmptySignatureFieldSignedBG;
                this.Image = global::SignatorDocSolution.Properties.Resources.StampSignedDigitaly;
                this.StampIndex = 2;
                this.IsAlreadyToSeal = true;
            }
            this.toolTip = new System.Windows.Forms.ToolTip();
            this.Controls.Add(createRepeatSigFieldButton());
            this.Controls.Add(createConfigSigFieldButton(configFieldButton_EventClick));
            this.Controls.Add(createConfirmConfigButton(event_HandlerClick_1));

            this.initialDeviceIsPresent = isDevicePresent;

            if (this.frm_SignConfig == null)
            {
                this.frm_SignConfig = new Form_SignConfig();
                this.frm_SignConfig.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
              //this.frm_SignConfig.Icon = ((System.Drawing.Icon)(resources.GetObject("frm_SignConfig.Icon")));
                this.frm_SignConfig.Left = (int)((SignatorDocSolution.Utils.Defins.rectPrimaryScreen.Width * 0.35F)+60);
                this.frm_SignConfig.Top = (int)((SignatorDocSolution.Utils.Defins.rectPrimaryScreen.Height * 0.20F)+85);
                this.frm_SignConfig.MaximizeBox = false;
                this.frm_SignConfig.MinimizeBox = false;
                this.frm_SignConfig.Name = "frm_SignConfig_2";
                this.frm_SignConfig.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
                this.frm_SignConfig.Text = "SignatorDoc - Configurações de Assinatura";
                this.frm_SignConfig.Visible = false;
                this.frm_SignConfig.SetEvents_Handler(new System.EventHandler(frm_SigConfig_ConfigConfigurations_Button_Click));
                this.frm_SignConfig.setInitialConfig(isDevicePresent);
                this.frm_SignConfig.clearOptions(true);
            }
        }

        public SignatureField(CustomEvent.GenericDelegateEventHandler event_HandleClick_1)
        {
            this.Name = "SignatureField100";
            this.Size = new System.Drawing.Size(Defins.SignatureFieldSize.Width, Defins.SignatureFieldSize.Height);
            this.BackColor = System.Drawing.Color.FromArgb(1, 255, 255, 255);
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.DoubleClick += new System.EventHandler(SignatureField_DoubleClick);
            this.BackgroundImage = global::SignatorDocSolution.Properties.Resources.Signature_Field;
            this.AfterSign.Invoker = new CustomEvent.GenericDelegateEventHandler(event_HandleClick_1);
            
            this.toolTip = new System.Windows.Forms.ToolTip();
            this.Controls.Add(createRepeatSigFieldButton());

            this.initialDeviceIsPresent = true;

            this.deviceSerieNumber = DetectDevice.deviceSerieNumber;
        }

        private void AfterSign_Event()
        {
        }

        private void configFieldButton_EventClick(object sender, System.EventArgs e)
        {
            if (this.getAlreadyToSign())
            {
                this.frm_SignConfig.setSignatureFieldOptions(this.SigFieldSealType, this.StampIndex);
            }
            else
            {
                this.frm_SignConfig.clearOptions(true);
            }
            this.frm_SignConfig.ShowDialog();
        }

        private void frm_SigConfig_ConfigConfigurations_Button_Click(object sender, System.EventArgs e)
        {
            if (this.frm_SignConfig.IsValidatedFilds())
            {
                if (AweSomeUtils.isLockSignDevice)
                    AweSomeUtils.isLockSignDevice = false;

                if (((this.frm_SignConfig.getSelectedStamp() != null) && (!this.frm_SignConfig.getIsHandWrittenSig())) || (this.frm_SignConfig.isInvisibleSignature()))
                {
                    if(this.frm_SignConfig.isInvisibleSignature())
                        this.setSigImageStamp(global::SignatorDocSolution.Properties.Resources.EmptyField);
                    else
                        this.setSigImageStamp(this.frm_SignConfig.getSelectedStamp());
                    this.setAlreadyToSign(true);
                    this.Show_btn_SigFieldRepeat(false);
                    if (this.frm_SignConfig.isInvisibleSignature())
                        this.StampIndex = 0;
                    else
                        this.StampIndex = this.frm_SignConfig.getSelectedStampIndex();
                }
                else {
                    if (this.frm_SignConfig.getIsHandWrittenSig()) {
                        if (this.getIsHandWrittenDone())
                        {
                            this.Show_btn_SigFieldRepeat(true);
                            if (this.frm_SignConfig.getIsHandWrittenStamp())
                            {
                                this.StampIndex = this.frm_SignConfig.getSelectedStampIndex();
                                this.tempStampImage = this.frm_SignConfig.getSelectedStamp();
                                this.setSigHandWrittenStamp(this.frm_SignConfig.getSelectedStamp());
                                this.stampHasBeenSelected = true;
                            }
                            else
                            {
                                this.StampIndex = 0;
                                if (this.stampHasBeenSelected)
                                {
                                    this.clearSigImageStamp();
                                    this.Image = this.tempHandWrittenImage;
                                }
                            }
                        }
                        else {
                            this.reconfigureSigHandWritten();
                            AweSomeUtils.isLockSignDevice = true;
                            if (this.frm_SignConfig.getIsHandWrittenStamp())
                            {
                                this.tempStampImage = this.frm_SignConfig.getSelectedStamp();
                                this.StampIndex = this.frm_SignConfig.getSelectedStampIndex();
                                this.tempStampImage = this.frm_SignConfig.getSelectedStamp();
                                this.stampHasBeenSelected = true;
                            }
                            else
                                this.StampIndex = 0;
                        }
                    }
                }

                this.setSignatureFieldOptions(this.frm_SignConfig.getSignatureFieldOptions());
                this.frm_SignConfig.Hide();
            }
        }

        protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            if(!someParsDisabled){
                if (e.Control && e.KeyCode == System.Windows.Forms.Keys.D)
                {
                    this.KeyDownEvent.Invoke(this, e);
                }
               // base.OnKeyDown(e);
            }
        }
      
        private System.Windows.Forms.Button createCloseButtonSigField(System.EventHandler click_Handler ) {
            System.Windows.Forms.Button btn_CloseSigField = new System.Windows.Forms.Button();
            btn_CloseSigField.Name = "SigField_CloseButton";
            btn_CloseSigField.Cursor = System.Windows.Forms.Cursors.Arrow;
            btn_CloseSigField.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            btn_CloseSigField.FlatAppearance.BorderSize = 0;
            btn_CloseSigField.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            btn_CloseSigField.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ActiveCaption;
            btn_CloseSigField.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn_CloseSigField.Image = global::SignatorDocSolution.Properties.Resources.CloseBtnSigField;
            btn_CloseSigField.Location = new System.Drawing.Point(this.Size.Width-35,5);
            btn_CloseSigField.Size = new System.Drawing.Size(20, 20);
            btn_CloseSigField.UseVisualStyleBackColor = true;
            btn_CloseSigField.Click += click_Handler;
            btn_CloseSigField.MouseHover+=new System.EventHandler(btn_CloseSigField_MouseHover);
            return btn_CloseSigField;
        }

        private System.Windows.Forms.Button createConfirmConfigButton(System.EventHandler click_Handler)
        {
            System.Windows.Forms.Button btn_ConfirmConfig= new System.Windows.Forms.Button();
            btn_ConfirmConfig.Cursor = System.Windows.Forms.Cursors.Arrow;
            btn_ConfirmConfig.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            btn_ConfirmConfig.FlatAppearance.BorderSize = 0;
            btn_ConfirmConfig.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            btn_ConfirmConfig.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ActiveCaption;
            btn_ConfirmConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn_ConfirmConfig.Image = global::SignatorDocSolution.Properties.Resources.okSigFieldConfPatt;
            btn_ConfirmConfig.Location = new System.Drawing.Point(this.Size.Width - 35, 5);
            btn_ConfirmConfig.Size = new System.Drawing.Size(22, 22);
            btn_ConfirmConfig.UseVisualStyleBackColor = true;
            btn_ConfirmConfig.Click += click_Handler;
            btn_ConfirmConfig.MouseHover += new System.EventHandler(btn_ConfirmConfig_MouseHover);
            return btn_ConfirmConfig;
        }

        private System.Windows.Forms.Button createRepeatSigFieldButton()
        {
            System.Windows.Forms.Button btn_RepeatSigField = new System.Windows.Forms.Button();
            btn_RepeatSigField.Cursor = System.Windows.Forms.Cursors.Arrow;
            btn_RepeatSigField.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            btn_RepeatSigField.FlatAppearance.BorderSize = 0;
            btn_RepeatSigField.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            btn_RepeatSigField.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ActiveCaption;
            btn_RepeatSigField.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn_RepeatSigField.Name = this.Name + "_btn_RepeatSigField";
            btn_RepeatSigField.Image = global::SignatorDocSolution.Properties.Resources.SigFieldRepeat;
            btn_RepeatSigField.Location = new System.Drawing.Point(this.Size.Width-35, this.Size.Height-35);
            btn_RepeatSigField.Size = new System.Drawing.Size(20, 20);
            btn_RepeatSigField.UseVisualStyleBackColor = true;
            btn_RepeatSigField.Click += new System.EventHandler(btn_RepeatSigField_Click);
            btn_RepeatSigField.MouseHover += new System.EventHandler(btn_RepeatSigField_MouseHover);
            btn_RepeatSigField.Visible = false;
            return btn_RepeatSigField;
        }

        private System.Windows.Forms.Button createConfigSigFieldButton(System.EventHandler click_Handler)
        {
            System.Windows.Forms.Button btn_ConfigSigField = new System.Windows.Forms.Button();
            btn_ConfigSigField.Cursor = System.Windows.Forms.Cursors.Arrow;
            btn_ConfigSigField.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            btn_ConfigSigField.FlatAppearance.BorderSize = 0;
            btn_ConfigSigField.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            btn_ConfigSigField.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ActiveCaption;
            btn_ConfigSigField.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn_ConfigSigField.Image = global::SignatorDocSolution.Properties.Resources.SigFieldConfig;
            btn_ConfigSigField.Location = new System.Drawing.Point(10, 5);
            btn_ConfigSigField.Size = new System.Drawing.Size(25, 25);
            btn_ConfigSigField.UseVisualStyleBackColor = true;
            btn_ConfigSigField.Click += click_Handler;
            btn_ConfigSigField.MouseHover+= new System.EventHandler(btn_ConfigSigField_MouseHover);
            return btn_ConfigSigField;
        }      
       
        #region SignatureFieldMoveHandles
        private void SignatureField_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (this.allowMouseMove)
            {
                this.Left = e.X + this.Left - this.currentPointMouseButtonDownLocation.X;
                this.Top = e.Y + this.Top - this.currentPointMouseButtonDownLocation.Y;
                this.Update();
            }
        }

        private void SignatureField_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.allowMouseMove = false;
        }

        private void SignatureField_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.Focus();
            ((Form_Main)this.Parent.Parent.Parent.Parent.Parent.Parent).setCurrentSigFieldIndex(sender);

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.allowMouseMove = true;
                this.currentPointMouseButtonDownLocation = e.Location;
            }
        }

        private void SignatureField_DoubleClick(object sender, System.EventArgs e)
        {
            if (this.SigFieldSealType.ToString().Contains("SigImage"))
            {               
                if (!IsAlreadyToSeal)
                {
                    if (AweSomeUtils.isLockSignDevice)
                    {
                        System.Windows.Forms.MessageBox.Show("Aguarde a finalização de uma assinatura antes de começar outra.", "Assinatura de Campos", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    }
                    else
                    {
                        if (this.deviceSerieNumber == 1)
                            this.BackgroundImage = global::SignatorDocSolution.Properties.Resources.EmptySignatureFieldSTU500Series;
                        else
                            this.BackgroundImage = global::SignatorDocSolution.Properties.Resources.EmptySignatureField;

                        if (this.isBioAnaliseMode && this.isSignatureDrawConfigured)
                            this.reconfigureSigHandWritten();
                        else
                            this.setSignatureDeviceConfiguration();

                        AweSomeUtils.isLockSignDevice = true;
                    }
                }              
            }
        }

        #endregion

        private void btn_RepeatSigField_Click(object sender, System.EventArgs e)
        {
            if(AweSomeUtils.isLockSignDevice){
                System.Windows.Forms.MessageBox.Show("Aguarde a finalização de uma assinatura antes de começar outra.","Assinatura de Campos", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }else{
                if(deviceSerieNumber==1)
                    this.BackgroundImage = global::SignatorDocSolution.Properties.Resources.EmptySignatureFieldSTU500Series;
                else
                    this.BackgroundImage = global::SignatorDocSolution.Properties.Resources.EmptySignatureField;
                
                if (!this.isSignatureDrawConfigured)
                {
                    this.setSignatureDeviceConfiguration();
                }else{
                    if (this.deviceSerieNumber == 1)
                        this.signatureDeviceDrawSTU.setScribbleConfig();
                    else
                        this.signatureDeviceDraw.setScribbleConfig();
                    
                    this.isHandWrittenDone = false;
                }

                AweSomeUtils.isLockSignDevice = true;
            }
        }

        private void btn_ConfigSigField_MouseHover(object sender, System.EventArgs e)
        {
            this.toolTip.Show("Configurar Opções de Assinatura.", (System.Windows.Forms.Control)sender);
        }

        private void btn_ConfirmConfig_MouseHover(object sender, System.EventArgs e)
        {
            this.toolTip.Show("Confirmar Configuração.", (System.Windows.Forms.Control)sender);
        }

        private void btn_RepeatSigField_MouseHover(object sender, System.EventArgs e)
        {
            this.toolTip.Show("Repetir Assinatura", (System.Windows.Forms.Control)sender);
        }

        private void btn_CloseSigField_MouseHover(object sender, System.EventArgs e)
        {
            this.toolTip.Show("Remover o Campo de Assinatura", (System.Windows.Forms.Control)sender);
        }

        public void fireAfterSign() {
            AweSomeUtils.isLockSignDevice = false;
            this.AfterSign.Invoker.Invoke();
        }

        public void Show_btn_SigFieldRepeat(bool isToShow)
        {
            try
            {
                ((System.Windows.Forms.Button)this.Controls[this.Name + "_btn_RepeatSigField"]).Visible = isToShow;
            }
            catch (System.Exception exc) { }
        }

        public void releaseResouces()
        {
            if (this.signatureDeviceDraw != null)
                this.signatureDeviceDraw.DisposeContext();
            if (this.signatureDeviceDrawSTU != null)
            {
                this.signatureDeviceDrawSTU.release();
                this.signatureDeviceDrawSTU.DisposeContext();
            }

            this.Parent.Controls.Remove(this);
            this.Dispose();
        }

        public void releaseAndremoveItSelf(){
            if (this.signatureDeviceDraw != null)
                this.signatureDeviceDraw.DisposeContext();
            if (this.signatureDeviceDrawSTU != null)
            {
                this.signatureDeviceDrawSTU.DisposeContext();
            }

            this.Parent.Controls.Remove(this);
            this.Dispose();
        }

        public void releaseForAnalise() {
            this.isHandWrittenDone = false;
            this.IsAlreadyToSeal = false;
            this.setDataBioSig(null);
            this.BackgroundImage = global::SignatorDocSolution.Properties.Resources.Signature_Field;
            
            if (this.Image != null)
            {
                this.Image.Dispose();
                this.Image = null;
            }

            if ((!this.isHandWrittenDone) && AweSomeUtils.isLockSignDevice)
            {
                if(this.deviceSerieNumber == 1)
                    if (this.signatureDeviceDrawSTU != null)
                        this.signatureDeviceDrawSTU.clearScreenDeviceBusy();
                
                AweSomeUtils.isLockSignDevice = false;
            }

            this.Show_btn_SigFieldRepeat(false);

        }

        public int getPageNumber() {
            return this.PageNumberAdded+1;
        }

        public bool getAlreadyToSign() {
            return this.IsAlreadyToSeal;
        }

        public void setAlreadyToSign(bool already) {
            this.IsAlreadyToSeal = already;
            this.BackgroundImage = global::SignatorDocSolution.Properties.Resources.EmptySignatureFieldSignedBG;
        }

        public void setSigImageStamp(System.Drawing.Image sigImage) {
            this.Image = sigImage;
            this.stampHasBeenSelected = true;
            this.isHandWrittenDone = false;
        }

        public void clearSigImageStamp(){
            this.tempStampImage=null;
            this.stampHasBeenSelected = false;
        }

        public void setDataBioSig(string base64DataBioSig){
            this.dataBioSig = base64DataBioSig;
        }

        public string getDataBioSig() {
            return this.dataBioSig;
        }

        public void setSignatureFieldOptions(SignatureField_SealType sigFieldSealType) {
            this.SigFieldSealType = sigFieldSealType;
        }

        public void setIsHandWrittenDone(bool done) {
            this.isHandWrittenDone = done;
        }

        public bool getIsHandWrittenDone() {
            return this.isHandWrittenDone;
        }

        public bool getIsSignatureDrawConfigured() {
            return this.isSignatureDrawConfigured;
        }

        public void reconfigureSigHandWritten() { 
            if(this.deviceSerieNumber==1)
                this.BackgroundImage = global::SignatorDocSolution.Properties.Resources.EmptySignatureFieldSTU500Series;
            else
                this.BackgroundImage = global::SignatorDocSolution.Properties.Resources.EmptySignatureField;
            
            if (!this.isSignatureDrawConfigured)
            {
                this.setSignatureDeviceConfiguration();
            }else{
                if (this.deviceSerieNumber == 1)
                    this.signatureDeviceDrawSTU.setScribbleConfig();
                else
                    this.signatureDeviceDraw.setScribbleConfig();

                this.isHandWrittenDone = false;
            }
        }

        public void setSigHandWrittenStamp(System.Drawing.Image sigStampImage)
        {
            System.IO.Stream imageStream = new System.IO.MemoryStream();
            using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(sigStampImage))
            {
                using (System.Drawing.Bitmap newBitmap = new System.Drawing.Bitmap(bitmap))
                {
                    newBitmap.SetResolution(this.Image.HorizontalResolution, this.Image.VerticalResolution);
                    newBitmap.Save(imageStream, System.Drawing.Imaging.ImageFormat.Png);
                }
            }

            System.Drawing.Image ImageToWrite = System.Drawing.Image.FromStream(imageStream);
            if (this.stampHasBeenSelected)
                this.Image = this.tempHandWrittenImage;

            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(ImageToWrite))
            {
                g.DrawImage(this.Image, new System.Drawing.Point(0, 0));
                g.Save();
            }

            this.Image= ImageToWrite;
            this.isHandWrittenDone = true;
        }

        public void setExtenalEventDeviceScreenClick(CustomEvent.GenericDelegateEventHandler evt) {
            this.DeviceScreenClick_Event += evt;
        }

        public void deviceSealScreenClick() {
            this.DeviceScreenClick_Event.Invoke();
        }

        #region SignatureDevice
        public void setSignatureDeviceConfiguration() 
        {
            if (deviceSerieNumber==1)
            {
                this.signatureDeviceDrawSTU = new SignatureDeviceDrawSTU(this);
                this.isSignatureDrawConfigured = true;
            }
            else
            {
                this.signatureDeviceDraw = new SignatureDeviceDraw(this);
                this.isSignatureDrawConfigured = true;
            }
            this.isHandWrittenDone = false;
        }

        private void deviceStartClick_EventHandler() {
            if (AweSomeUtils.isLockSignDevice)
            {
                System.Windows.Forms.MessageBox.Show("Aguarde a finalização de uma assinatura antes de começar outra.", "Assinatura de Campos", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
            else{
                if (!IsAlreadyToSeal)
                {
                    if (this.deviceSerieNumber == 1)
                        this.BackgroundImage = global::SignatorDocSolution.Properties.Resources.EmptySignatureFieldSTU500Series;

                    this.setSignatureDeviceConfiguration();

                    
                }
                AweSomeUtils.isLockSignDevice = true;
            }
        }

        public void abortSignatureDrawBusy()
        {
            if (deviceSerieNumber == 1)
            {
                if (this.signatureDeviceDrawSTU != null)
                    this.signatureDeviceDrawSTU.abortBusySign();

                AweSomeUtils.isLockSignDevice = false;
            }
            else
            {
                if (this.signatureDeviceDraw != null)
                    this.signatureDeviceDraw.abortBusySign();

                AweSomeUtils.isLockSignDevice = false;

            }
        }

        #endregion

    }

    
}
