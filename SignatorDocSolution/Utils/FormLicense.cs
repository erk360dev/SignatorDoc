using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SignatorDocSolution.Utils
{
    /// <summary>
    /// CLASS_ID=11;
    /// </summary>
    public partial class FormLicense : Form
    {
        private string license = null;
        private string internalLicense = null;
        private bool isLicenseGenerated = false;
        private string LicenseFileName_DefaultPath = null;
        private bool isSavedLicence = false;
        private bool isEmailEntered=false;
        private string userFileNameLocal = null;
        private bool isRequest = true;
        private bool successPreLicenseWrited = false;
        private bool isLicenseAlready = false;
        private string fileNameDefinitiveLicense = null;
        private bool isLicenced = false;
        private bool isTrialPreLicense = false;
        private bool simpleClose=false;
        public bool isApplicationOpened = false;
        private bool isEmailSended = false;
        private CustomEvent External_Event_1 = new CustomEvent();

        public FormLicense(bool is_Request)
        {
            InitializeComponent();
            this.isRequest = is_Request;
            if (!this.isRequest)
            {
                this.btnRemoveLicence.Enabled = true;
                this.btnGenLicense.Enabled = false;
                this.btnSelectLicLocalSave.Enabled = false;
                this.txtSaveReqLiq.Enabled = false;
                this.txtEmail.Enabled = false;
                //this.btnSendLicense.Text = "Importar Licença";
            }
            else {
                this.btnRemoveLicence.Enabled = false;
                this.txtFinalLicense.Enabled = false;
                this.btnSelectFinalLicense.Enabled = false;
            }
        }

        public void setLicenseFileNameDefaultPath(string licenseFileName) {
            this.LicenseFileName_DefaultPath = licenseFileName;
        }

        private void btnGenLicense_Click(object sender, EventArgs e)
        {
            if (!this.isLicenseGenerated)
            {
                LicenseSignatorDoc licenseSignatorDoc = new LicenseSignatorDoc();
                this.license = licenseSignatorDoc.getPreLicense();
                this.internalLicense = licenseSignatorDoc.getPreInternalLicense();
                this.isLicenseGenerated = true;

                if (this.isEmailEntered) {
                    this.btnSendLicense.Enabled = true;
                }
                
                saveLicense(0);
                
                MessageBox.Show("Licença gerada com sucesso!", "Licença SignatorDoc", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.btnGenLicense.Enabled = false;
                this.btnRemoveLicence.Enabled = true;
                this.btnSelectLicLocalSave.Enabled = true;
                this.txtSaveReqLiq.Enabled = true;
                this.txtEmail.Enabled = true;
                this.chkDefinitiveLicense.Enabled = false;
            }
        }

        private void btnSelectLicLocalSave_Click(object sender, EventArgs e)
        {
            if (this.isLicenseGenerated)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Arquivos de Licença (*.lic)|*.lic";
                saveFileDialog.FileName = "PedidoLicenca";
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    this.userFileNameLocal = saveFileDialog.FileName;
                    saveLicense(2);
                    this.txtSaveReqLiq.Text = saveFileDialog.FileName;
                    this.isLicenseAlready = true;
                }
            }
        }
       
        private void btnExit_Click(object sender, EventArgs e)
        {
            if (this.isLicenced && (!simpleClose))
                Application.Restart();
            else
            {
                if (!this.isApplicationOpened || (!this.isLicenced))
                    Application.Exit();
                else
                    this.Close();
            }
        }

        private void FormLicense_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (isRequest)
            {   
                if(this.isLicenseGenerated){
                    if (!this.isLicenseAlready)
                    {
                        if (MessageBox.Show("Seu pedido de licença foi gerado mas ainda não foi exportado. Para continuar com o processo de licenciamento do Sistema, Salve o pedido de licença em um local ou envie diretamente por E-Mail clicando em \"Enviar Pedido\". Clique em [SIM] para exportar um pedido de licença.", "Licença SignatorDoc", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                }
                if (!this.isLicenseAlready)
                    if (System.IO.File.Exists(this.LicenseFileName_DefaultPath))
                        System.IO.File.Delete(this.LicenseFileName_DefaultPath);
                }
        }

        private void txtEmail_LostFocus(object sender, System.EventArgs e)
        {
        }

        private void txtEmail_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (this.txtEmail.Text.Length > 3)
                this.btnSendLicense.Enabled = true;
            else
                this.btnSendLicense.Enabled = false;
        }

        private void btnSendLicense_Click(object sender, EventArgs e)
        {
            if (AweSomeUtils.IsValidEmail(this.txtEmail.Text))
            {
                if (isLicenseGenerated)
                {
                    if (!this.isEmailSended)
                    {
                        bool isEmailFail = false;
                        E_Email.ConfigMail(this.txtEmail.Text, this.LicenseFileName_DefaultPath);
                        
                        using(BackgroundWorker backGroungWorker_SignDoc = new BackgroundWorker()){
                            showOrHideLoadingPanel(true);
                            backGroungWorker_SignDoc.WorkerReportsProgress = true;
                            backGroungWorker_SignDoc.WorkerSupportsCancellation = true;
                            backGroungWorker_SignDoc.DoWork += (sw, ew) =>
                            {
                                if (!E_Email.sendEmail())
                                    isEmailFail = true;
                            };
                            backGroungWorker_SignDoc.RunWorkerCompleted += (sw, ew) =>
                            {
                                if (!isEmailFail) {
                                    MessageBox.Show("O Pedido de Licença foi enviado corretamente.", "Licença SignatorDoc", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    this.btnSendLicense.Enabled = false;
                                    this.isEmailSended = true;
                                }
                                this.showOrHideLoadingPanel(false);
                            };
                            backGroungWorker_SignDoc.RunWorkerAsync();
                        }
                        
                    }
                    else {
                        MessageBox.Show("Email já enviado", "Licença SignatorDoc", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                else
                {
                    MessageBox.Show("Licença ainda não gerada. Gere uma licença antes de enviar por E-Mail!", "Licença SignatorDoc", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else {
                MessageBox.Show("O E-Mail inserido é inválido.", "Licença SignatorDoc", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.btnSendLicense.Enabled = false;
            }

            
        }

        private void saveLicense(int saveLicenceType)   // 0= save install folder, 1 = save user choose with dialog, 2 save user choose withou dialog
        {
            if (saveLicenceType == 0) {
                if (!successPreLicenseWrited)
                {
                    string licenseHex = "";
                    foreach (char c in this.internalLicense.ToCharArray())
                    {
                        licenseHex += string.Format("{0:X0}", System.Convert.ToInt32(c));
                    }
                    string writeLic = Convert.ToBase64String(Encoding.Default.GetBytes(licenseHex));

                    System.IO.File.WriteAllText(LicenseFileName_DefaultPath, writeLic, Encoding.ASCII);
                    successPreLicenseWrited = true;
                }
                return;
            }


            if (saveLicenceType==1)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Arquivos de Licença (*.lic)|*.lic";
                saveFileDialog.FileName = "PedidoLicenca";
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string licenseHex = "";
                    foreach (char c in this.license.ToCharArray())
                    {
                        licenseHex += string.Format("{0:X0}", System.Convert.ToInt32(c));
                    }
                    string writeLic = Convert.ToBase64String(Encoding.Default.GetBytes(licenseHex));

                    System.IO.File.WriteAllText(saveFileDialog.FileName, writeLic, Encoding.ASCII);

                    this.isSavedLicence = true;
                }
            }
            else {

                string licenseHex = "";
                foreach (char c in this.license.ToCharArray())
                {
                    licenseHex += string.Format("{0:X0}", System.Convert.ToInt32(c));
                }
                string writeLic = Convert.ToBase64String(Encoding.Default.GetBytes(licenseHex));

                System.IO.File.WriteAllText(this.userFileNameLocal, writeLic, Encoding.ASCII);

                this.isSavedLicence = true;
            }
        }

        private void btnSelectFinalLicense_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Arquivos de Licença (*.lic)|*.lic";
            openFileDialog.FileName = "Licenca";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.txtFinalLicense.Text = openFileDialog.FileName;
                this.fileNameDefinitiveLicense = openFileDialog.FileName;
                this.btn_License.Enabled = true;
            }
        } 

        private void btnRemoveLicence_Click(object sender, EventArgs e)
        {
            if (this.isLicenced)
            {
                string fileLic = System.IO.Path.GetDirectoryName(LicenseFileName_DefaultPath)+@"\License.lic";
                if (System.IO.File.Exists(fileLic))
                    System.IO.File.Delete(fileLic);
            }

            if(System.IO.File.Exists(LicenseFileName_DefaultPath))
                System.IO.File.Delete(LicenseFileName_DefaultPath);
            
            if (this.isTrialPreLicense)
            {

                this.btnGenLicense.Enabled = true;
                this.btnRemoveLicence.Enabled = false;
                this.btnRemoveLicence.Text = "Remover Pedido de Licença";
                this.btn_License.Text = "Licenciar";
                this.btn_License.Enabled = false;
                this.btn_License.Width -= 40;
                this.btn_License.Location = new Point(this.btn_License.Location.X + 40, this.btn_License.Location.Y);
                this.isTrialPreLicense = false;
            }
            else
            {
                this.btnRemoveLicence.Enabled = false;
                this.btnGenLicense.Enabled = true;
                this.btnSendLicense.Enabled = false;
                this.btnSelectLicLocalSave.Enabled = false;
                this.successPreLicenseWrited = false;
                this.isLicenseGenerated = false;
                this.txtSaveReqLiq.Enabled = false;

                this.txtFinalLicense.Enabled = false;
                this.btnSelectFinalLicense.Enabled = false;
                this.txtEmail.Enabled = false;
                this.btnSendLicense.Text = "Enviar Pedido";
                this.btnRemoveLicence.Text = "Remover Pedido de Licença";

                MessageBox.Show("Licença removida com sucesso!", "Licença SignatorDoc", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            this.isRequest = true;
            this.chkDefinitiveLicense.Enabled = true;
            this.txtEmail.Enabled = false;
            this.isLicenced = false;
            this.isEmailSended = false;

            if (this.isApplicationOpened)
                External_Event_1.Invoker.Invoke();
        }

        private void btn_License_Click(object sender, EventArgs e)
        {
            if (this.isTrialPreLicense)
            {
                LicenseSignatorDoc license = new LicenseSignatorDoc();
                license.setDefaultPreLicenseFileName(this.LicenseFileName_DefaultPath);
                if (license.processTrialLicense())
                {
                    if (System.IO.File.Exists(this.LicenseFileName_DefaultPath))
                        System.IO.File.Delete(this.LicenseFileName_DefaultPath);
                    string fileNameTrialLicense = System.IO.Path.GetDirectoryName(this.LicenseFileName_DefaultPath) + @"\License.lic";
                    System.IO.File.WriteAllText(fileNameTrialLicense, license.getDefinitiveLicenseToSaveFile(), Encoding.ASCII);
                    this.isLicenced = true;
                    MessageBox.Show("O licenciamento de avaliação do sistema foi concluído com êxito.","Licença SignatorDoc",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    this.btn_License.Enabled = false;
                    this.chkDefinitiveLicense.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Não foi possível licenciar o sistema. Um ou mais parametros de licenciamento não puderam ser checados.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else{
                string fileFinalLicenseEncriptedB64 = System.IO.File.ReadAllText(this.fileNameDefinitiveLicense, Encoding.ASCII);
                byte[] finalLicenceEncripted = Convert.FromBase64String(fileFinalLicenseEncriptedB64);
                LicenseSignatorDoc license = new LicenseSignatorDoc();
                license.setDefaultPreLicenseFileName(this.LicenseFileName_DefaultPath);
                if (license.processLicense(finalLicenceEncripted))
                {
                    if (System.IO.File.Exists(this.LicenseFileName_DefaultPath))
                        System.IO.File.Delete(this.LicenseFileName_DefaultPath);
                    string fileNameFinalLicense = System.IO.Path.GetDirectoryName(this.LicenseFileName_DefaultPath) + @"\License.lic";
                    System.IO.File.WriteAllText(fileNameFinalLicense, license.getDefinitiveLicenseToSaveFile(), Encoding.ASCII);
                    this.isLicenced = true;
                    this.chkDefinitiveLicense.Enabled = false;
                    MessageBox.Show("O licenciamento do sistema foi concluído com êxito.", "Licença", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    MessageBox.Show("Não foi possível licenciar o sistema.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                this.btn_License.Enabled = false ;
            }
        }

        public void setInitialConfigAlreadyLicense() {
            this.btnRemoveLicence.Text = "Remover Licença";
            this.btnSelectFinalLicense.Enabled = false;
            this.txtFinalLicense.Enabled = false;
            this.isLicenced = true;
            this.simpleClose = true;
        }

        public void setConfTrial()
        {
            this.btnGenLicense.Enabled = false;
            this.btnSelectLicLocalSave.Enabled = false;
            this.btnSelectFinalLicense.Enabled = false;
            this.txtFinalLicense.Enabled = false;
            this.btnRemoveLicence.Text = "Remover Licença Trial";
            this.btnRemoveLicence.Enabled = true;
            this.btn_License.Enabled = true;
            this.btn_License.Location = new Point(this.btn_License.Location.X-20, this.btn_License.Location.Y);
            this.btn_License.Text = "Utilizar Licença Trial";
            this.btn_License.Width += 40;
            this.btn_License.Location = new Point(this.btn_License.Location.X - 20, this.btn_License.Location.Y);
            this.isTrialPreLicense = true;
            this.chkDefinitiveLicense.Enabled = false;
        }

        public void setExternalEvents(CustomEvent.GenericDelegateEventHandler eventHandler1) {
            External_Event_1.Invoker += eventHandler1;
        }

        private void showOrHideLoadingPanel(bool isShow)
        {
            if (isShow)
            {
                this.loadingPanel.Location = new System.Drawing.Point( (this.Size.Width / 2) - (this.loadingPanel.Size.Width / 2), (this.Size.Height / 2) - (this.loadingPanel.Size.Height / 2));          
                this.loadingPanel.Focus();
                this.loadingPanel.BringToFront();
                this.loadingPanel.Visible = true;
                this.Enabled = false;

            }
            else
            {
                this.Enabled = true;
                this.loadingPanel.Visible = false;

            }
        }

        private void chkDefinitiveLicense_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.isRequest)
                return;

            if (this.chkDefinitiveLicense.Checked)
            {
                this.txtFinalLicense.Enabled = true;
                this.btnSelectFinalLicense.Enabled = true;
            }
            else
            {
                this.txtFinalLicense.Enabled = false;
                this.btnSelectFinalLicense.Enabled = false;
                this.btn_License.Enabled = false;
            }
        }

    }
}
