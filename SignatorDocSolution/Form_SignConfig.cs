using SignatorDocSolution.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;

namespace SignatorDocSolution
{
    /// <summary>
    /// CLASS_ID=07;
    /// </summary>
    public partial class Form_SignConfig : Form
    {
        private List<string> ListCertThumbPrints = new List<string>();
        private int CertIndex = 0;
        private System.Drawing.Image selectedStampImage = null;
        private int imageStampDesired_Width = 300;
        private int imageStampDesired_Height = 160;
        private bool deviceIsAvailable = false;
        private bool canceledCheck_chkVisible = false;

        public Form_SignConfig()
        {
            InitializeComponent();
            fill_cboCertificatesList();
            Fill_cboSelStamp();
        }

        private void fill_cboCertificatesList() 
        {
            X509Store x509CertificateStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            x509CertificateStore.Open(OpenFlags.ReadOnly);
            
            this.cboCertificatesList.Items.Insert(0, "Selecione um Certificado");
            this.cboCertificatesList.SelectedIndex = 0;
            foreach (X509Certificate2 x509Cert2 in x509CertificateStore.Certificates)
            {
                this.cboCertificatesList.Items.Insert(CertIndex+1, x509Cert2.GetNameInfo(X509NameType.SimpleName, false));
                ListCertThumbPrints.Insert(CertIndex, x509Cert2.Thumbprint);
                CertIndex++;
            }
        }

        public void SetEvents_Handler(EventHandler eventHandler1)
        {
            this.btnConfirmConfigSigs.Click += eventHandler1;
        }

        public X509Certificate2 getSelectedCertificate()
        {
            X509Store x509CertificateStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            x509CertificateStore.Open(OpenFlags.ReadOnly);
            if (this.cboCertificatesList.SelectedIndex > 0)
            {
                foreach (X509Certificate2 x509Cert2 in x509CertificateStore.Certificates.Find(X509FindType.FindByThumbprint, this.ListCertThumbPrints[this.cboCertificatesList.SelectedIndex-1], false))
                {
                    return x509Cert2;
                }
                MessageBox.Show("O certificado selecionado não está mais disponível na base de certificados do Sistema. Se você estiver utilizando um Token ou Smart-Card, verifique se o dispositivo está devidamente conectado ao computador.", "Configuração de Assinatura", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return null;
        }

        private void Form_SignConfig_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        public string getSignerName()
        {
            return txtSignerName.Text;
        }

        public void setExternalSignerName(string name){
            this.txtSignerName.Text = name;
        }

        public void setExternalCboCertificateIndex(int index) {
            this.cboCertificatesList.SelectedIndex = index;
        }

        public int getCboCertificateIndex() {
            return this.cboCertificatesList.SelectedIndex;
        }

        public bool IsValidatedFilds() {
            ClearValidationError();
            if (this.txtSignerName.Text.Length < 4)
            {
                if (this.chkShowUserNameDigSig.Checked)
                {
                    this.errorProvider.SetError(this.chkShowUserNameDigSig, "Insira um nome de signatário válido!");
                    return false;
                }
            }
            if (this.chkStamp.Checked) {
                if (!this.chkVisible.Checked)
                {
                    if (this.cboSelStamp.SelectedIndex < 1)
                    {
                        this.errorProvider.SetError(this.cboSelStamp, "Selecione um tipo de Carimbo!");
                        return false;
                    }
                }
            }
            if ((!this.chkVisible.Checked) && (!this.chkStamp.Checked) && (!this.chkHandWritten.Checked))
            {
                MessageBox.Show("É preciso selecionar o tipo de assinatura para fixar ao documento.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;

        }

        private void ClearValidationError() {
            this.errorProvider.SetError(this.chkShowUserNameDigSig, "");
            this.errorProvider.SetError(this.cboSelStamp, "");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearValidationError();
            this.Hide();
        }

        private void chkStamp_CheckedChanged(object sender, System.EventArgs e)
        {
            if (!this.canceledCheck_chkVisible)
            {
                if (this.chkStamp.Checked)
                {
                    this.cboSelStamp.Enabled = true;
                }
                else
                {
                    this.cboSelStamp.Enabled = false;
                    if (!this.chkVisible.Checked)
                    {
                        if (this.deviceIsAvailable && !this.chkHandWritten.Checked)
                        {
                            MessageBox.Show("É preciso selecionar o tipo de assinatura que será fixada ao documento.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            this.chkStamp.Checked = true;
                        }
                    }
                }
            } 
            
            this.canceledCheck_chkVisible = false;
        }

        private void chkHandWritten_CheckedChanged(object sender, System.EventArgs e)
        {
            if (!this.chkHandWritten.Checked)
            {
                if (!this.chkVisible.Checked)
                {
                    if (!this.chkStamp.Checked)
                    {
                        MessageBox.Show("É preciso selecionar o tipo de assinatura que será fixada ao documento.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.chkStamp.Checked = true;
                    }
                }
            }
        }

        private void chkVisible_CheckedChanged(object sender, System.EventArgs e)
        {
            this.enableSigOptions(!this.chkVisible.Checked);
        }

        public bool isInvisibleSignature()
        {
            return chkVisible.Checked;
        }

        private void Fill_cboSelStamp()
        {
            this.cboSelStamp.Items.Add("Selecione um Carimbo");
            this.cboSelStamp.Items.Add("Imagem de Carimbo no Computador");
            this.cboSelStamp.Items.Add("Carimbo Assinado");
            this.cboSelStamp.Items.Add("Carimbo Aprovado");
            if (this.deviceIsAvailable)
                this.cboSelStamp.SelectedIndex = 0;
            else
                this.cboSelStamp.SelectedIndex = 2;

        }

        private void cboCertificatesList_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cboSelStamp_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cboSelStamp_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            switch (this.cboSelStamp.SelectedIndex)
            {
                case 1:
                    {
                        using (OpenFileDialog openFileDialog = new OpenFileDialog())
                        {
                            openFileDialog.Filter = "Arquivos PNG|*.png";
                            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                System.Drawing.Image auxStampImage= System.Drawing.Image.FromFile(openFileDialog.FileName);
                                this.selectedStampImage = this.resizeStampImage(auxStampImage);
                            }
                        }
                        break;
                    }
                case 2:
                    {
                        this.selectedStampImage = global::SignatorDocSolution.Properties.Resources.StampSignedDigitaly;
                        break;
                    }
                case 3:
                    {
                        this.selectedStampImage = global::SignatorDocSolution.Properties.Resources.StampApproved;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        public System.Drawing.Image getSelectedStamp()
        {
            return this.selectedStampImage;
        }

        public System.Drawing.Image resizeStampImage(System.Drawing.Image originalImage) {

            Bitmap stampImage_Resized = new Bitmap(this.imageStampDesired_Width, this.imageStampDesired_Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            using (Graphics stampGraphics = Graphics.FromImage(stampImage_Resized))
            {
                stampImage_Resized.SetResolution(originalImage.HorizontalResolution, originalImage.VerticalResolution);
                stampGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                stampGraphics.DrawImage(originalImage,
                                        new Rectangle(0, 0, this.imageStampDesired_Width, this.imageStampDesired_Height),
                                        new Rectangle(0, 0, originalImage.Width, originalImage.Height),
                                        GraphicsUnit.Pixel);
            }

            return stampImage_Resized;
        }

        public int getSelectedStampIndex() {
            return this.cboSelStamp.SelectedIndex;
        }

        public bool getIsHandWrittenSig() {
            return this.chkHandWritten.Checked;
        }

        public void clearOptions(bool resetOnlyStamp) {

            this.chkVisible.Checked = false;
            if (this.deviceIsAvailable)
            {
                this.chkHandWritten.Checked = true;
                this.chkStamp.Checked = false;
                this.chkStamp.Enabled = true;
                this.cboSelStamp.SelectedIndex = 0;
            }
            else {
                this.chkStamp.Checked = true;
                this.chkHandWritten.Checked = false;
                this.chkStamp.Enabled = false;
                this.cboSelStamp.SelectedIndex = 2;
            }

            this.chkShowUserNameDigSig.Checked = false;
            this.chkShowDateTime.Checked = false;
            this.chkShowHash.Checked = false;
            if (!resetOnlyStamp)
            {
                this.cboCertificatesList.SelectedIndex = 0;
                this.txtSignerName.Text = string.Empty;
            }          
            
        }

        public void resetCertificateChoosed() {
            this.cboCertificatesList.SelectedIndex = 0;
        }

        public void setInitialConfig(bool thereIsDeviceAvailable){
            this.deviceIsAvailable = thereIsDeviceAvailable;

            this.chkVisible.Checked = false;
            if (!this.deviceIsAvailable)
            {
                this.lblDeviceNotAvailable.Visible = true;
                this.chkStamp.Checked = true;
                this.chkHandWritten.Checked = false;
                this.chkHandWritten.Enabled = false;
                this.chkStamp.Enabled = false;

            }
            else {
                this.lblDeviceNotAvailable.Visible = false;
                this.chkHandWritten.Enabled = true;
                this.chkHandWritten.Checked = true;
                this.chkStamp.Checked = false;
                this.chkStamp.Enabled = true;

            }

        }

        public bool getIsHandWrittenStamp() {
            return (this.chkHandWritten.Checked && this.chkStamp.Checked);
        }


        #region sigFieldOptions
        public SignatureField_SealType getSignatureFieldOptions()
        {
            if(this.chkVisible.Checked){
                return SignatureField_SealType.SigInvisible;
            }
            else{
                #region
            //#################### Hand Written Signature ##################
            if (this.chkHandWritten.Checked && !this.chkStamp.Checked)//1A
            {
                if (chkShowUserNameDigSig.Checked)//2A
                {
                    if (chkShowDateTime.Checked)//3A
                    {
                        if (chkShowHash.Checked)//4A
                        {
                            return SignatureField_SealType.SigImageAll;
                        }
                        else {//4B 
                            return SignatureField_SealType.SigImageUserNameDateTime;
                        }
                    }
                    else { //3B
                        if (chkShowHash.Checked)//4A
                        {
                            return SignatureField_SealType.SigImageUserNameHash;
                        }
                        else
                        {//4B 
                            return SignatureField_SealType.SigImageUserName;
                        }
                    }
                }
                else { //2B
                    if (chkShowDateTime.Checked)//3A
                    {
                        if (chkShowHash.Checked)//4A
                        {
                            return SignatureField_SealType.SigImageDateTimeHash;
                        }
                        else
                        {//4B 
                            return SignatureField_SealType.SigImageDateTime;
                        }
                    }
                    else
                    { //3B
                        if (chkShowHash.Checked)//4A
                        {
                            return SignatureField_SealType.SigImageHash;
                        }
                        else
                        {//4B 
                            return SignatureField_SealType.SigImage;
                        }
                    }
                }
            } 
            else {
                //#######################   Signature Stamp ###################################
                if (!this.chkHandWritten.Checked && this.chkStamp.Checked)//1B 
                {
                    if (chkShowUserNameDigSig.Checked)//2A
                    {
                        if (chkShowDateTime.Checked)//3A
                        {
                            if (chkShowHash.Checked)//4A
                            {
                                return SignatureField_SealType.StampAll;
                            }
                            else
                            {//4B 
                                return SignatureField_SealType.StampUserNameDateTime;
                            }
                        }
                        else
                        { //3B
                            if (chkShowHash.Checked)//4A
                            {
                                return SignatureField_SealType.StampUserNameHash;
                            }
                            else
                            {//4B 
                                return SignatureField_SealType.StampUserName;
                            }
                        }
                    }
                    else
                    { //2B
                        if (chkShowDateTime.Checked)//3A
                        {
                            if (chkShowHash.Checked)//4A
                            {
                                return SignatureField_SealType.StampDateTimeHash;
                            }
                            else
                            {//4B 
                                return SignatureField_SealType.StampDateTime;
                            }
                        }
                        else
                        { //3B
                            if (chkShowHash.Checked)//4A
                            {
                                return SignatureField_SealType.StampHash;
                            }
                            else
                            {//4B 
                                return SignatureField_SealType.Stamp;
                            }
                        }
                    }
                }

                else {
                    //#######################   Signature Stamp Hand Written ###################################
                    if (this.chkHandWritten.Checked && this.chkStamp.Checked) //1C
                    {
                        if (this.chkShowUserNameDigSig.Checked)//2A
                        {
                            if (chkShowDateTime.Checked)//3A
                            {
                                if (chkShowHash.Checked)//4A
                                {
                                    return SignatureField_SealType.SigImageStampAll;
                                }
                                else
                                {//4B 
                                    return SignatureField_SealType.SigImageStampUserNameDateTime;
                                }
                            }
                            else
                            { //3B
                                if (chkShowHash.Checked)//4A
                                {
                                    return SignatureField_SealType.SigImageStampUserNameHash;
                                }
                                else
                                {//4B 
                                    return SignatureField_SealType.SigImageStampUserName;
                                }
                            }
                        }
                        else
                        { //2B
                            if (chkShowDateTime.Checked)//3A
                            {
                                if (chkShowHash.Checked)//4A
                                {
                                    return SignatureField_SealType.SigImageStampDateTimeHash;
                                }
                                else
                                {//4B 
                                    return SignatureField_SealType.SigImageStampDateTime;
                                }
                            }
                            else
                            { //3B
                                if (chkShowHash.Checked)//4A
                                {
                                    return SignatureField_SealType.SigImageStampHash;
                                }
                                else
                                {//4B 
                                    return SignatureField_SealType.SigImageStamp;
                                }
                            }
                        }
                    }
                    else {
                        return SignatureField_SealType.SigImageStampUserNameDateTime;
                    }
                }
                
            }
            #endregion
            }
        }

        public void setSignatureFieldOptions(SignatureField_SealType signatureFieldOption, int stampIndex)
        {
            switch (signatureFieldOption)
            {
                    //#######################  Signature Hand Written     ########################
                case SignatureField_SealType.SigImageAll:       //1  Hand Written Signature
                    {
                        this.chkVisible.Checked = false;
                        this.chkHandWritten.Checked=true;
                        this.chkStamp.Checked = false;
                        this.chkShowUserNameDigSig.Checked=true;
                        this.chkShowDateTime.Checked=true;
                        this.chkShowHash.Checked = true;
                        break;
                    }
                case SignatureField_SealType.SigImageUserNameDateTime:       //2  Hand Written Signature
                    {
                        this.chkVisible.Checked = false;
                        this.chkHandWritten.Checked = true;
                        this.chkStamp.Checked = false;
                        this.chkShowUserNameDigSig.Checked = true;
                        this.chkShowDateTime.Checked = true;
                        this.chkShowHash.Checked = false;
                        break;
                    }
                case SignatureField_SealType.SigImageUserNameHash:       //3  Hand Written Signature
                    {
                        this.chkVisible.Checked = false;
                        this.chkHandWritten.Checked = true;
                        this.chkStamp.Checked = false;
                        this.chkShowUserNameDigSig.Checked = true;
                        this.chkShowDateTime.Checked = false;
                        this.chkShowHash.Checked = true;
                        break;
                    }
                case SignatureField_SealType.SigImageUserName:       //4  Hand Written Signature
                    {
                        this.chkVisible.Checked = false;
                        this.chkHandWritten.Checked = true;
                        this.chkStamp.Checked = false;
                        this.chkShowUserNameDigSig.Checked = true;
                        this.chkShowDateTime.Checked = false;
                        this.chkShowHash.Checked = false;
                        break;
                    }
                case SignatureField_SealType.SigImageDateTimeHash:       //5  Hand Written Signature
                    {
                        this.chkVisible.Checked = false;
                        this.chkHandWritten.Checked = true;
                        this.chkStamp.Checked = false;
                        this.chkShowUserNameDigSig.Checked = false;
                        this.chkShowDateTime.Checked = true;
                        this.chkShowHash.Checked = true;
                        break;
                    }
                case SignatureField_SealType.SigImageDateTime:       //6  Hand Written Signature
                    {
                        this.chkVisible.Checked = false;
                        this.chkHandWritten.Checked = true;
                        this.chkStamp.Checked = false;
                        this.chkShowUserNameDigSig.Checked = false;
                        this.chkShowDateTime.Checked = true;
                        this.chkShowHash.Checked = false;
                        break;
                    }
                case SignatureField_SealType.SigImageHash:       //7  Hand Written Signature
                    {
                        this.chkVisible.Checked = false;
                        this.chkHandWritten.Checked = true;
                        this.chkStamp.Checked = false;
                        this.chkShowUserNameDigSig.Checked = false;
                        this.chkShowDateTime.Checked = false;
                        this.chkShowHash.Checked = true;
                        break;
                    }
                case SignatureField_SealType.SigImage:       //8  Hand Written Signature
                    {
                        this.chkVisible.Checked = false;
                        this.chkHandWritten.Checked = true;
                        this.chkStamp.Checked = false;
                        this.chkShowUserNameDigSig.Checked = false;
                        this.chkShowDateTime.Checked = false;
                        this.chkShowHash.Checked = false;
                        break;
                    }
                //#######################  Signature With Stamp    ########################
                case SignatureField_SealType.StampAll:       //1 Stamp Signature
                    {
                        this.chkVisible.Checked = false;
                        this.chkStamp.Checked = true;
                        this.chkHandWritten.Checked = false;
                        this.chkShowUserNameDigSig.Checked = true;
                        this.chkShowDateTime.Checked = true;
                        this.chkShowHash.Checked = true;
                        this.cboSelStamp.SelectedIndex = stampIndex;
                        break;
                    }
                case SignatureField_SealType.StampUserNameDateTime:       //2 Stamp Signature
                    {
                        this.chkVisible.Checked = false;
                        this.chkStamp.Checked = true;
                        this.chkHandWritten.Checked = false;
                        this.chkShowUserNameDigSig.Checked = true;
                        this.chkShowDateTime.Checked = true;
                        this.chkShowHash.Checked = false;
                        this.cboSelStamp.SelectedIndex = stampIndex; 
                        break;
                    }
                case SignatureField_SealType.StampUserNameHash:       //3 Stamp Signature
                    {
                        this.chkVisible.Checked = false;
                        this.chkStamp.Checked = true;
                        this.chkHandWritten.Checked = false;
                        this.chkShowUserNameDigSig.Checked = true;
                        this.chkShowDateTime.Checked = false;
                        this.chkShowHash.Checked = true;
                        this.cboSelStamp.SelectedIndex = stampIndex;
                        break;
                    }
                case SignatureField_SealType.StampUserName:       //4 Stamp Signature
                    {
                        this.chkVisible.Checked = false;
                        this.chkStamp.Checked = true;
                        this.chkHandWritten.Checked = false;
                        this.chkShowUserNameDigSig.Checked = true;
                        this.chkShowDateTime.Checked = false;
                        this.chkShowHash.Checked = false;
                        this.cboSelStamp.SelectedIndex = stampIndex;
                        break;
                    }
                case SignatureField_SealType.StampDateTimeHash:       //5 Stamp Signature
                    {
                        this.chkVisible.Checked = false;
                        this.chkStamp.Checked = true;
                        this.chkHandWritten.Checked = false;
                        this.chkShowUserNameDigSig.Checked = false;
                        this.chkShowDateTime.Checked = true;
                        this.chkShowHash.Checked = true;
                        this.cboSelStamp.SelectedIndex = stampIndex;
                        break;
                    }
                case SignatureField_SealType.StampDateTime:       //6 Stamp Signature
                    {
                        this.chkVisible.Checked = false;
                        this.chkStamp.Checked = true;
                        this.chkHandWritten.Checked = false;
                        this.chkShowUserNameDigSig.Checked = false;
                        this.chkShowDateTime.Checked = true;
                        this.chkShowHash.Checked = false;
                        this.cboSelStamp.SelectedIndex = stampIndex;
                        break;
                    }
                case SignatureField_SealType.StampHash:       //7 Stamp Signature
                    {
                        this.chkVisible.Checked = false;
                        this.chkStamp.Checked = true;
                        this.chkHandWritten.Checked = false;
                        this.chkShowUserNameDigSig.Checked = false;
                        this.chkShowDateTime.Checked = false;
                        this.chkShowHash.Checked = true;
                        this.cboSelStamp.SelectedIndex = stampIndex;
                        break;
                    }
                case SignatureField_SealType.Stamp:       //8 Stamp Signature
                    {
                        this.chkVisible.Checked = false;
                        this.chkStamp.Checked = true;
                        this.chkHandWritten.Checked = false;
                        this.chkShowUserNameDigSig.Checked = false;
                        this.chkShowDateTime.Checked = false;
                        this.chkShowHash.Checked = false;
                        this.cboSelStamp.SelectedIndex = stampIndex;
                        break;
                    }
                //#######################  Signature Hand Written and Stamp     ########################
                
                case SignatureField_SealType.SigImageStampAll:       //1    Stamp Hand Written Signature
                    {
                        this.chkVisible.Checked = false;
                        this.chkStamp.Checked = true;
                        this.chkHandWritten.Checked = true;
                        this.chkShowUserNameDigSig.Checked = true;
                        this.chkShowDateTime.Checked = true;
                        this.chkShowHash.Checked = true;
                        this.cboSelStamp.SelectedIndex = stampIndex;
                        break;
                    }
                case SignatureField_SealType.SigImageStampUserNameDateTime:       //2 Stamp Hand Written Signature
                    {
                        this.chkVisible.Checked = false;
                        this.chkStamp.Checked = true;
                        this.chkHandWritten.Checked = true;
                        this.chkShowUserNameDigSig.Checked = true;
                        this.chkShowDateTime.Checked = true;
                        this.chkShowHash.Checked = false;
                        this.cboSelStamp.SelectedIndex = stampIndex;
                        break;
                    }
                case SignatureField_SealType.SigImageStampUserNameHash:       //3 Stamp Hand Written Signature
                    {
                        this.chkVisible.Checked = false;
                        this.chkStamp.Checked = true;
                        this.chkHandWritten.Checked = true;
                        this.chkShowUserNameDigSig.Checked = true;
                        this.chkShowDateTime.Checked = false;
                        this.chkShowHash.Checked = true;
                        this.cboSelStamp.SelectedIndex = stampIndex;
                        break;
                    }
                case SignatureField_SealType.SigImageStampUserName:       //4  Stamp Hand Written Signature
                    {
                        this.chkVisible.Checked = false;
                        this.chkStamp.Checked = true;
                        this.chkHandWritten.Checked = true;
                        this.chkShowUserNameDigSig.Checked = true;
                        this.chkShowDateTime.Checked = false;
                        this.chkShowHash.Checked = false;
                        this.cboSelStamp.SelectedIndex = stampIndex;
                        break;
                    }
                case SignatureField_SealType.SigImageStampDateTimeHash:       //5  Stamp Hand Written Signature
                    {
                        this.chkVisible.Checked = false;
                        this.chkStamp.Checked = true;
                        this.chkHandWritten.Checked = true;
                        this.chkShowUserNameDigSig.Checked = false;
                        this.chkShowDateTime.Checked = true;
                        this.chkShowHash.Checked = true;
                        this.cboSelStamp.SelectedIndex = stampIndex;
                        break;
                    }
                case SignatureField_SealType.SigImageStampDateTime:       //6  Stamp Hand Written Signature
                    {
                        this.chkVisible.Checked = false;
                        this.chkStamp.Checked = true;
                        this.chkHandWritten.Checked = true;
                        this.chkShowUserNameDigSig.Checked = false;
                        this.chkShowDateTime.Checked = true;
                        this.chkShowHash.Checked = false;
                        this.cboSelStamp.SelectedIndex = stampIndex;
                        break;
                    }
                case SignatureField_SealType.SigImageStampHash:       //7  Stamp Hand Written Signature
                    {
                        this.chkVisible.Checked = false;
                        this.chkStamp.Checked = true;
                        this.chkHandWritten.Checked = true;
                        this.chkShowUserNameDigSig.Checked = false;
                        this.chkShowDateTime.Checked = false;
                        this.chkShowHash.Checked = true;
                        this.cboSelStamp.SelectedIndex = stampIndex;
                        break;
                    }
                case SignatureField_SealType.SigImageStamp:       //8  Stamp Hand Written Signature
                    {
                        this.chkVisible.Checked = false;
                        this.chkStamp.Checked = true;
                        this.chkHandWritten.Checked = true;
                        this.chkShowUserNameDigSig.Checked = false;
                        this.chkShowDateTime.Checked = false;
                        this.chkShowHash.Checked = false;
                        this.cboSelStamp.SelectedIndex = stampIndex;
                        break;
                    }
                case SignatureField_SealType.SigInvisible:       //9  InvisibleSignature
                    {
                        this.chkVisible.Checked = true;
                        this.chkStamp.Checked = false;
                        this.chkHandWritten.Checked = false;
                        this.chkShowUserNameDigSig.Checked = false;
                        this.chkShowDateTime.Checked = false;
                        this.chkShowHash.Checked = false;
                        this.chkShowUserNameDigSig.Checked = false;
                        this.cboSelStamp.SelectedIndex = 0;
                        break;
                    }
                default: {
                    this.chkVisible.Checked = false;
                    this.chkStamp.Checked = true;
                    this.chkHandWritten.Checked = false;
                    this.chkShowUserNameDigSig.Checked = true;
                    this.chkShowDateTime.Checked = true;
                    this.chkShowHash.Checked = false;
                    this.cboSelStamp.SelectedIndex = 0;
                    break;
                }

            }

        }

        private void enableSigOptions(bool enable) {
            if (enable)
            {
                if (!this.txtSignerName.Enabled) {
                    this.chkStamp.Enabled = this.deviceIsAvailable;
                    this.chkHandWritten.Enabled = this.deviceIsAvailable;
                    this.chkShowUserNameDigSig.Enabled = true;
                    this.chkShowDateTime.Enabled = true;
                    this.chkShowHash.Enabled = true;
                    this.chkShowUserNameDigSig.Enabled = true;
                    this.txtSignerName.Enabled = true;
                    this.cboSelStamp.Enabled = true;
                    this.canceledCheck_chkVisible = true;
                    this.chkStamp.Checked = true;
                }
            }
            else {
                this.chkStamp.Enabled = false;
                this.chkHandWritten.Enabled = false;
                this.chkShowUserNameDigSig.Enabled = false;
                this.chkShowDateTime.Enabled = false;
                this.chkShowHash.Enabled = false;
                this.chkShowUserNameDigSig.Enabled = false;
                this.txtSignerName.Enabled = false;
                this.cboSelStamp.Enabled = false;
            }
        }

        #endregion

    }
}
