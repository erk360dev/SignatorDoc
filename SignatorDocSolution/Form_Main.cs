using SignatorDocSolution.Utils;
using GhostScriptUtils.GhostScriptManagement;
using pdftron.PDF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;

namespace SignatorDocSolution
{
    /// <summary>
    /// CLASS_ID=02;
    /// </summary>

    public partial class Form_Main : Form
    {
        #region Atributes
        private PDFDoc PDFDocument = null;
        private PictureBox[] pdfPages = null;
        private List<SignatureField> SignatureFieldCollection = new List<SignatureField>();
        private List<PDFDocInfo> PDFDocInfoList = new List<PDFDocInfo>();
        private Pdf2Image pdf2ImageConverter = null;
        private List<string> massiveFileNames = new List<string>();
        private List<PDFDocInfo> listDocumentName = new List<PDFDocInfo>(); // HandleEncryption
        private DocSigner docSigner = new DocSigner();
        private System.Windows.Forms.Timer tmrAfterLoadingPanel = null;
        private BackgroundWorker backGroungWorker_SignDoc = null;
        private BackgroundWorker backGroungWorker_SignDocMassive = null;
        private System.Drawing.Point pageLocation = new System.Drawing.Point(5, 3);
        private int currentPageNumber = 1;
        private int currentPageIndex = 0;
        private int oldPageIndex = 0;
        private int CountPages = 0;
        private int oldHandMouseMovePosY = 10000;
        private int countHandMouseMoveShiftY = 0;
        private int auxPageNumber = 1;
        private int SigContainerCount = (-1);
        private int SigContainerCurrentIndex = (-1);
        private string FileName = null;
        private string tempFileName = null;
        private string localTempPath = null;
        private bool AllowDragPage = false;
        private bool AllowPageMouseWhell = false;
        private bool pdfDocLoaded = false;
        private bool isSavedDoc = false;
        private bool isDocChanged = false;
        private bool isLayoutAdobeMode = false;
        private bool isSignSuccess = false;
        private bool isFieldPatternAdded = false;
        private bool isMassiveSign = false;
        private bool isPDFNetBug = false;
        private bool isPassNeedOpenDoc = false; // HandleEncryption
        private bool isPassNeedEditDoc = false; // HandleEncryption
        private bool isPassEditDocOk = false; // HandleEncryption
        
        #endregion

        public Form_Main()
        {
            try
            {
                this.localTempPath = Defins.DefaultRootPath + @"\TempDoc";
                if (!System.IO.Directory.Exists(this.localTempPath))
                    System.IO.Directory.CreateDirectory(this.localTempPath);
                if (!System.IO.Directory.Exists(Defins.SupportDir))
                    System.IO.Directory.CreateDirectory(Defins.SupportDir);

                this.InitializeComponent();

                //Create static form password dialog
                AweSomeUtils.InstanceForm_PasswordRequest();

                if (!this.isAdbeReaderInstalled)
                {
                    this.rdo_AdobeMode.Enabled = false;

                }
                else
                {
                    this.createEmptyPDFFile();
                }

                if (this.tmrAfterLoadingPanel == null)
                {
                    this.tmrAfterLoadingPanel = new System.Windows.Forms.Timer();
                    this.tmrAfterLoadingPanel.Tick += new EventHandler(tmrAfterLoadingPanel_Tick);
                    this.tmrAfterLoadingPanel.Interval = 500;
                }

                if (DetectSTUDevice.isSTUPlugged())
                {
                    DetectSTUDevice.showDeviceIntroduction();
                }
                DetectSTUDevice.isAlredyTested = false;

                Configurations.checkConfigurations();
                this.frm_Configurations.setInitialConfiguration();
            }
            catch (Exception exc) { 
                MessageBox.Show("Ocorreu um erro na tentativa de executar um procesimento!\n\nErro: "+exc.Message,"Erro",MessageBoxButtons.OK,MessageBoxIcon.Error);
                Environment.Exit(0);
            }

            try
            {
                string emptypdf = this.localTempPath + @"\EmptyPDF.pdf";
                foreach (string auxFileName in System.IO.Directory.GetFiles(this.localTempPath))
                {
                    if (!auxFileName.Equals(emptypdf))
                        System.IO.File.Delete(auxFileName);
                }
            }
            catch (System.IO.IOException ioexc) { }
            catch (System.Exception exc) { }

            try
            {
                if (AweSomeUtils.IsInitialFileArg) {
                    this.OpenDocument(false, new string[1] { AweSomeUtils.InitialFileArgPath });
                    AweSomeUtils.IsInitialFileArg = false;
                }
            }
            catch (Exception exc) {
                MessageBox.Show("Houve uma falha ao tentar carregar um argumento de entrada no SignatorDoc!\nExceção: " + exc.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #region Page MouseEvents
        private void pictureBox_Page_MouseEnter(object sender, EventArgs e)
        {
            this.AllowPageMouseWhell = true;
        }

        private void pictureBox_Page_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.AllowDragPage)
            {
                if (oldHandMouseMovePosY == e.Y)
                {
                    return;
                }
                else
                {
                    countHandMouseMoveShiftY++;
                }
                if (countHandMouseMoveShiftY < 2)
                {
                    oldHandMouseMovePosY = e.Y;
                    return;
                }

                if (oldHandMouseMovePosY < e.Y)
                {
                    if (this.pnl_PDFPageViewer.VerticalScroll.Value > this.pnl_PDFPageViewer.VerticalScroll.Minimum)
                    {
                        this.pnl_PDFPageViewer.VerticalScroll.Value = (this.pnl_PDFPageViewer.VerticalScroll.Value - 4) < 0 ? 0 : this.pnl_PDFPageViewer.VerticalScroll.Value - 4;//doc up         
                        this.pnl_PDFPageViewer.PerformLayout();
                        countHandMouseMoveShiftY = 0;
                    }
                }
                else
                {
                    if (this.pnl_PDFPageViewer.VerticalScroll.Value < this.pnl_PDFPageViewer.VerticalScroll.Maximum)
                    {
                        this.pnl_PDFPageViewer.VerticalScroll.Value = (this.pnl_PDFPageViewer.VerticalScroll.Value + 4) > this.pnl_PDFPageViewer.VerticalScroll.Maximum ? this.pnl_PDFPageViewer.VerticalScroll.Maximum : this.pnl_PDFPageViewer.VerticalScroll.Value + 4;//doc down         
                        this.pnl_PDFPageViewer.PerformLayout();
                        countHandMouseMoveShiftY = 0;
                    }
                }
                oldHandMouseMovePosY = e.Y;
            }
        }

        private void pictureBox_Page_MouseLeave(object sender, EventArgs e)
        {
            this.AllowDragPage = false;
            this.AllowPageMouseWhell = false;
        }

        private void pictureBox_Page_MouseUp(object sender, MouseEventArgs e)
        {
            this.AllowDragPage = false;
        }

        private void pictureBox_Page_MouseDown(object sender, MouseEventArgs e)
        {
            this.AllowDragPage = true;
        }

        #endregion

        #region General Events
        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.OpenDocument(true,null);
        }

        private void btn_OpenDocument_Click(object sender, EventArgs e)
        {
            this.OpenDocument(true, null);
        }

        private void btn_SaveDocument_Click(object sender, EventArgs e)
        {
            if (this.isMassiveSign)
            {
                if (this.saveMassiveDocuments(false, false))
                {
                    this.btn_SaveDocument.Enabled = false;
                    this.Mnu_Item_Save.Enabled = false;
                }
            }
            else
            {
                this.saveSimpleDocument(false, true);
            }
        }

        private void Form_Main_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (this.AllowPageMouseWhell)
            {
                if (e.Delta > 0)
                {
                    if (this.pnl_PDFPageViewer.VerticalScroll.Value > this.pnl_PDFPageViewer.VerticalScroll.Minimum)
                        this.pnl_PDFPageViewer.VerticalScroll.Value = (this.pnl_PDFPageViewer.VerticalScroll.Value - 100) < 0 ? 0 : this.pnl_PDFPageViewer.VerticalScroll.Value - 100;//sobe
                }
                else
                {
                    if (this.pnl_PDFPageViewer.VerticalScroll.Value < this.pnl_PDFPageViewer.VerticalScroll.Maximum)
                        this.pnl_PDFPageViewer.VerticalScroll.Value = (this.pnl_PDFPageViewer.VerticalScroll.Value + 100) > this.pnl_PDFPageViewer.VerticalScroll.Maximum ? this.pnl_PDFPageViewer.VerticalScroll.Maximum : this.pnl_PDFPageViewer.VerticalScroll.Value + 100; ;//desce
                }
                this.pnl_PDFPageViewer.PerformLayout();
            }
        }

        private void tlStripMenuItemAbout_click(object sender, EventArgs e)
        {
            this.aboutBox_Form.ShowDialog();
        }

        private void Form_Main_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (e.Effect == DragDropEffects.Copy)
            {
                try
                {
                    string[] data=e.Data.GetData(DataFormats.FileDrop) as string[];
                    if (File.Exists(data[0]))
                    {
                        this.OpenDocument(false,data);
                    }
                }
                catch (Exception exc) {}
            }
        }

        private void Form_Main_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            string[] data = e.Data.GetData(DataFormats.FileDrop) as string[];
            bool fileAccept = false;

            if (data.Length == 1)
            {
                fileAccept = Path.GetExtension(data[0]).Equals(".pdf") || Path.GetExtension(data[0]).Equals(".doc") || Path.GetExtension(data[0]).Equals(".docx");
            }

            if ((e.Data.GetDataPresent(DataFormats.FileDrop)) && fileAccept)
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        } 

        private void Form_Main_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (AweSomeUtils.closeApplication)
            {
                if (this.PDFDocument != null)
                {
                    this.PDFDocument.Dispose();
                }
            }
            else
            {
                if (this.isDocChanged)
                {
                    if (!this.isSavedDoc)
                    {
                        string notsavedMessage = this.isMassiveSign ? "Os documentos assinados ainda não foram salvos. Gostaria de salvá-los agora?" : "O documento ainda não foi salvo, Gostaria de salvá-lo agora?";
                        if (MessageBox.Show(notsavedMessage, "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                        {
                            if (this.isMassiveSign)
                            {
                                this.saveMassiveDocuments(true, false);
                            }
                            else
                            {
                                this.saveSimpleDocument(true, true);
                            }
                        }
                    }
                }

                if (this.PDFDocument != null)
                {
                    this.PDFDocument.Close();
                }
            }
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.PDFDocument != null)
            {
                this.PDFDocument.Close();
            }
            this.Close();
        }

        private void Mnu_Item_Save_Click(object sender, EventArgs e)
        {
            if (this.isMassiveSign) 
            {
                if (this.saveMassiveDocuments(false, false))
                {
                    this.btn_SaveDocument.Enabled = false;
                    this.Mnu_Item_Save.Enabled = false;
                }
            }
            else
            {
                this.saveSimpleDocument(false, true);
            }
        }

        private void SettingsToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            this.frm_Configurations.ShowDialog();
        }

        private void salvarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.isMassiveSign)
            {
                if (this.isDocChanged)
                {
                    if (this.saveMassiveDocuments(true, false))
                    {
                        this.btn_SaveDocument.Enabled = false;
                        this.Mnu_Item_Save.Enabled = false;
                    }
                }
                else
                {
                    try
                    {
                        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                        folderBrowserDialog.RootFolder = Environment.SpecialFolder.Desktop;
                        folderBrowserDialog.ShowNewFolderButton = true;
                        folderBrowserDialog.Description = "Selecione o Diretório onde você quer salvar o lote de documentos.";
                        string tempFileName;
                        if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            foreach (string fn in this.massiveFileNames)
                            {
                                tempFileName = folderBrowserDialog.SelectedPath + @"\" + Path.GetFileName(fn);
                                File.WriteAllBytes(tempFileName, File.ReadAllBytes(this.localTempPath+@"\"+Path.GetFileNameWithoutExtension(fn)));
                            }
                            MessageBox.Show("Os documentos foram salvos com sucesso.", "Salvar Lotes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (IOException ioe)
                    {
                        MessageBox.Show("Ocorreu uma falha de leitura ou escrita no lote de documentos. Erro:"+ioe.Message,"Erro",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                    catch (Exception ex) {
                        MessageBox.Show("Ocorreu uma falha gravação no lote de documentos. Erro:" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                this.saveSimpleDocument(true, true);
            }
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.isDocChanged)
            {
                MessageBox.Show("Documentos já assinados não podem ter suas propriedades alteradas.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (!string.IsNullOrEmpty(this.docSigner.getSignedFilePath()))
                {
                    MessageBox.Show("Documentos já assinados não podem ter suas propriedades alteradas.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (!SignatorDocSolution.Utils.Configurations.Documents_DisableCheckPassword)
                    {
                        using (iTextSharp.text.pdf.PdfReader pdfReader = (this.isPassNeedOpenDoc ? new iTextSharp.text.pdf.PdfReader(this.tempFileName, System.Text.Encoding.Default.GetBytes(this.listDocumentName[0].passwdToOpen)) : new iTextSharp.text.pdf.PdfReader(this.tempFileName)))
                        {
                            if (pdfReader.AcroFields.GetSignatureNames().Count > 0)
                            {
                                MessageBox.Show("Documentos já assinados não podem ter suas propriedades alteradas.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }
                    else
                    {
                       using (iTextSharp.text.pdf.PdfReader pdfReader = new iTextSharp.text.pdf.PdfReader(this.tempFileName))
                        {
                            if (pdfReader.AcroFields.GetSignatureNames().Count > 0)
                            {
                                MessageBox.Show("Este documento está assinado. Suas propriedades não podem ser alteradas.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }

                    this.frm_Properties.setIntialDocProperties(this.FileName, this.tempFileName);

                    if (this.frm_Properties.checkDocInitialProperties(this.isPassNeedOpenDoc, this.isPassNeedEditDoc, this.isPassEditDocOk, (this.isPassEditDocOk ? this.listDocumentName[0].passwdToEdit : ""), (this.isPassNeedOpenDoc ? this.listDocumentName[0].passwdToOpen : ""), this.isDocChanged, this.isSavedDoc))
                        this.frm_Properties.ShowDialog();
                }
            }          

        }

        private void removerLicencaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.frm_License == null) {
                this.frm_License = new FormLicense(false);
                this.frm_License.setLicenseFileNameDefaultPath(Directory.GetParent(this.localTempPath) + @"\Licensing\PreLicense.lic");
                this.frm_License.setInitialConfigAlreadyLicense();
                this.frm_License.setExternalEvents(new CustomEvent.GenericDelegateEventHandler(AfterRemoveLicence));
                this.frm_License.isApplicationOpened = true;
            }
            this.frm_License.ShowDialog();
        }

        private void setFieldSearchPatternToolStripMenuItem_Click(object sender, EventArgs e) 
        {
            if (!this.isMassiveSign)
            {
                PDFDoc pdfdoc = new PDFDoc(this.tempFileName);
                if (this.isPassNeedOpenDoc)
                    pdfdoc.InitStdSecurityHandler(this.listDocumentName[0].passwdToOpen);
                this.frm_SetSigStringPattern.setInitialDefinition(pdfdoc);     
            }
            else {
                if (this.isDocChanged && this.PDFDocInfoList.Count > 0)
                    this.PDFDocInfoList.Clear();
                this.frm_SetSigStringPattern.setCleanSignatureFieldConfig();
            }
            this.frm_SetSigStringPattern.ShowDialog();
        }

        private void SigChartCheckStripMenuItem_Click(object sender, System.EventArgs e)
        {
            this.frm_SignatureChartCompare.showForm();
        }

        private void gpb_PDFMode_MouseHover(object sender, System.EventArgs e)
        {
            if (!this.isAdbeReaderInstalled)
                this.tlTip_Main.Show("Modo Visual Indisponível. O recurso Adobe Acrobat Reader não está instalado.", (Control)sender);
        }

        private void TStripBtn_Next_Click(object sender, EventArgs e)
        {
            if (this.currentPageNumber >= this.CountPages || (!this.pdfDocLoaded))
                return;
            this.currentPageIndex++;
            this.currentPageNumber++;

            this.setPageViewContent(this.currentPageIndex);
            ((PictureBox)this.pnl_PDFPageViewer.Controls[0]).Image = null;
            this.pnl_PDFPageViewer.Controls[0].Location = new System.Drawing.Point(pageLocation.X, pageLocation.Y);
            this.pnl_PDFPageViewer.Controls.Remove(this.pnl_PDFPageViewer.Controls[0]);
            this.pnl_PDFPageViewer.Controls.Add(this.pdfPages[this.currentPageIndex]);
            this.txt_PDFNavCurrPage.Text = this.currentPageNumber.ToString() + " / " + this.CountPages;

        }

        private void TStripBtn_Previous_Click(object sender, EventArgs e)
        {
            if (this.currentPageNumber <= 1 || (!this.pdfDocLoaded))
                return;
            this.currentPageIndex--;
            this.currentPageNumber--;

            this.setPageViewContent(this.currentPageIndex);
            ((PictureBox)this.pnl_PDFPageViewer.Controls[0]).Image = null;
            this.pnl_PDFPageViewer.Controls[0].Location = new System.Drawing.Point(pageLocation.X, pageLocation.Y);
            this.pnl_PDFPageViewer.Controls.Remove(this.pnl_PDFPageViewer.Controls[0]);
            this.pnl_PDFPageViewer.Controls.Add(this.pdfPages[this.currentPageIndex]);

            this.txt_PDFNavCurrPage.Text = currentPageNumber.ToString() + " / " + this.CountPages;
        }

        private void TStripBtn_Last_Click(object sender, EventArgs e)
        {
            if (!this.pdfDocLoaded)
                return;
            if (this.currentPageIndex < (this.CountPages - 1))
            {
                this.currentPageIndex = this.CountPages - 1;
                this.currentPageNumber = this.CountPages;

                this.setPageViewContent(this.currentPageIndex);
                ((PictureBox)this.pnl_PDFPageViewer.Controls[0]).Image = null;
                this.pnl_PDFPageViewer.Controls[0].Location = new System.Drawing.Point(pageLocation.X, pageLocation.Y);
                this.pnl_PDFPageViewer.Controls.Remove(this.pnl_PDFPageViewer.Controls[0]);
                this.pnl_PDFPageViewer.Controls.Add(this.pdfPages[this.currentPageIndex]);

                this.txt_PDFNavCurrPage.Text = currentPageNumber.ToString() + " / " + this.CountPages;
            }
        }

        private void TStripBtn_First_Click(object sender, EventArgs e)
        {
            if (!this.pdfDocLoaded)
                return;
            if (this.currentPageIndex != 0)
            {
                this.currentPageIndex = 0;
                this.currentPageNumber = 1;

                this.setPageViewContent(this.currentPageIndex);
                ((PictureBox)this.pnl_PDFPageViewer.Controls[0]).Image = null;
                this.pnl_PDFPageViewer.Controls[0].Location = new System.Drawing.Point(pageLocation.X, pageLocation.Y);
                this.pnl_PDFPageViewer.Controls.Remove(this.pnl_PDFPageViewer.Controls[0]);
                this.pnl_PDFPageViewer.Controls.Add(this.pdfPages[this.currentPageIndex]);
                this.txt_PDFNavCurrPage.Text = currentPageNumber.ToString() + " / " + this.CountPages;
            }
        }

        private void TStripTxt_CurrPage_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if ((char.IsDigit(e.KeyChar)) || (e.KeyChar == (char)Keys.Enter) || (e.KeyChar == (char)Keys.Back))
            {
                if ((e.KeyChar == (char)Keys.Enter) && (!string.IsNullOrEmpty(this.txt_PDFNavCurrPage.Text)))
                {
                    this.auxPageNumber = int.Parse(this.txt_PDFNavCurrPage.Text);
                    if ((this.auxPageNumber > this.CountPages) || (this.auxPageNumber < 1))
                    {
                        if (this.CountPages > 0)
                            this.txt_PDFNavCurrPage.Text = this.currentPageNumber.ToString() + " / " + this.CountPages;
                        else
                            this.txt_PDFNavCurrPage.Text = string.Empty;
                        return;
                    }
                    if (this.currentPageNumber != this.auxPageNumber)
                    {
                        this.currentPageNumber = this.auxPageNumber;
                        this.currentPageIndex = this.auxPageNumber - 1;

                        this.setPageViewContent(this.currentPageIndex);
                        ((PictureBox)this.pnl_PDFPageViewer.Controls[0]).Image = null;
                        this.pnl_PDFPageViewer.Controls[0].Location = new System.Drawing.Point(pageLocation.X, pageLocation.Y);
                        this.pnl_PDFPageViewer.Controls.Remove(this.pnl_PDFPageViewer.Controls[0]);
                        this.pnl_PDFPageViewer.Controls.Add(this.pdfPages[this.currentPageIndex]);

                    }
                    this.txt_PDFNavCurrPage.Text = currentPageNumber.ToString() + " / " + this.CountPages;
                }
            }
            else
            {
                e.Handled = true;
            }
        }

        private void btn_AddSignatureField_MouseHover(object sender, System.EventArgs e)
        {
            this.tlTip_Main.Show("Adicionar um local de assinatura na página atual.", (Control)sender);
        }

        private void btn_SealSignature_MouseHover(object sender, System.EventArgs e)
        {
            this.tlTip_Main.Show( (isMassiveSign ? "Fixar as assinaturas nos documentos.": "Fixar as assinaturas no documento."), (Control)sender);
        }

        private void btn_SaveDocument_MouseHover(object sender, System.EventArgs e)
        {
            this.tlTip_Main.Show( (this.isMassiveSign ? "Salvar os Documentos":"Salvar o Documento."), (Control)sender);
        }

        private void btn_OpenDocument_MouseHover(object sender, System.EventArgs e)
        {
            this.tlTip_Main.Show("Abrir um Documento.", (Control)sender);
        }

        private void btn_DoubleSignatureField_MouseHover(object sender, System.EventArgs e)
        {
            this.tlTip_Main.Show("Duplicar o Campo de Assinatura Atual.", (Control)sender);
        }

        private void btn_ReplicationSignatureFields_MouseHover(object sender, System.EventArgs e)
        {
            this.tlTip_Main.Show("Replicar Vários Campos de Assinatura.", (Control)sender);
        }

        private void btn_RemoveSignatureFields_MouseHover(object sender, System.EventArgs e)
        {
            this.tlTip_Main.Show("Remover um ou mais Campos de Assinatura.", (Control)sender);
        }

        private void btn_ReviewMassiveSignDocsFields_MouseHover(object sender, System.EventArgs e)
        {
            this.tlTip_Main.Show("Revisar os Campos de Assinatura Criados", (Control)sender);
        }  
        
        private void PanelThumbNail_ThumbnailN_Click(object sender, System.EventArgs e)
        {
            int thumbnailindex = this.pnlThumbnail.getCurrentThumbnailPage();
            if (thumbnailindex >= (this.CountPages) || thumbnailindex < 0)
                return;
            else
            {
                if (this.currentPageIndex != thumbnailindex)
                {
                    this.auxPageNumber = thumbnailindex;

                    this.currentPageNumber = this.auxPageNumber + 1;
                    this.currentPageIndex = this.auxPageNumber;
                    try
                    {
                        this.setPageViewContent(this.currentPageIndex);
                    }
                    catch (Exception exc)
                    {
                        //          SignatorDocSolution.Utils.RegisterWinEventLogViewer.RegisterLog("The Source Form_Main_PanelThumbNail_ThumbnailN_Click throws the following message : ", exc.Message, 20, 2);//classid+error 0(objectnull or disposed)
                        return;
                    }
                    ((PictureBox)this.pnl_PDFPageViewer.Controls[0]).Image = null;
                    this.pnl_PDFPageViewer.Controls[0].Location = new System.Drawing.Point(pageLocation.X, pageLocation.Y);
                    this.pnl_PDFPageViewer.Controls.Remove(this.pnl_PDFPageViewer.Controls[0]);
                    this.pnl_PDFPageViewer.Controls.Add(this.pdfPages[this.currentPageIndex]);
                    this.txt_PDFNavCurrPage.Text = currentPageNumber.ToString() + " / " + this.CountPages;
                }

            }
        }

        private void rdo_AdobeMode_CheckedChanged(object sender, System.EventArgs e)
        {
            if (!this.isLayoutAdobeMode)
            {
                if (this.rdo_AdobeMode.Checked)
                {
                    this.isLayoutAdobeMode = true;
                    if (this.pdfDocLoaded)
                    {
                        int auxCurrentPage = this.currentPageNumber;
                        releasePDFViewerResources();
                        this.tpnl_PDFNavigation.Visible = false;
                        this.pnl_ViewerBodyMain.Controls.Remove(this.tpnl_PDFViewerSignMode);
                        this.pnl_ViewerBodyMain.Controls.Add(this.pnl_PDFViewAdobeMode);
                        this.ShowDocumentAdobeMode(this.tempFileName, auxCurrentPage);
                    }
                    else
                    {
                        this.tpnl_PDFNavigation.Visible = false;
                        this.pnl_ViewerBodyMain.Controls.Remove(this.tpnl_PDFViewerSignMode);
                        this.pnl_ViewerBodyMain.Controls.Add(this.pnl_PDFViewAdobeMode);
                    }
                }
            }
        }

        private void rdo_SignMode_CheckedChanged(object sender, System.EventArgs e)
        {
            if (this.rdo_SignMode.Checked)
            {

                if (this.pdfDocLoaded)
                {
                    #region Handle Passwords Encryption Document
                    // Begin handleEncryption
                    if (!Configurations.Documents_DisableCheckPassword)
                    {
                        if (this.isPassNeedEditDoc && (!this.isPassEditDocOk))
                        {
                            string passToEdit = AweSomeUtils.getDocPassToEdit(this.tempFileName);
                            switch (passToEdit)
                            {
                                case "`\01´": { this.rdo_SignMode.Checked = false; this.rdo_AdobeMode.Checked = true; return; }
                                case "`\02´": { MessageBox.Show("A senha digitada está incorreta! Para abrir o documento em modo de assinatura é necessário digitar a senha Edição. Se esqueceu a senha, você pode abrir o documento em modo de Visualização mas não poderá assiná-lo.", "Abertura de Documento", MessageBoxButtons.OK, MessageBoxIcon.Warning); this.rdo_SignMode.Checked = false; this.rdo_AdobeMode.Checked = true; return; }
                                default:
                                    {
                                        if (this.isPassNeedOpenDoc)
                                        {
                                            this.listDocumentName[0].isNeedPassToEdit = true; ;
                                            this.listDocumentName[0].passwdToEdit = passToEdit;
                                        }
                                        else
                                            this.listDocumentName.Add(new PDFDocInfo(Path.GetFileNameWithoutExtension(this.tempFileName), passToEdit, false));
                                        this.isPassEditDocOk = true;
                                        break;
                                    }
                            }

                        }
                    }
                    // End handleEncryption
                    #endregion

                    string auxFileName = this.axAcroPDF.src;
                    this.axAcroPDF.LoadFile(this.localTempPath + @"\EmptyPDF.pdf");
                    this.isLayoutAdobeMode = false;
                    this.tpnl_PDFNavigation.Visible = true;
                    this.pnl_ViewerBodyMain.Controls.Remove(this.pnl_PDFViewAdobeMode);
                    this.pnl_ViewerBodyMain.Controls.Add(this.tpnl_PDFViewerSignMode);
                    if (!this.isPDFNetBug)
                    {
                        try
                        {
                            this.PDFDocument = new PDFDoc(auxFileName);
                            if (this.isPassNeedOpenDoc) // HandleEncryption
                                this.PDFDocument.InitStdSecurityHandler(this.listDocumentName[0].passwdToOpen); // HandleEncryption
                            this.ShowDocumentSignMode();
                        }
                        catch (Exception exc) {
                            MessageBox.Show("Ocorreu um erro ao tentar abrir o documento em modo de assinatura. Se o erro persistir, contate o fornecedor do sistema. \nErro:"+exc.Message,"Visualização de Documento",MessageBoxButtons.OK,MessageBoxIcon.Error);
                            this.Text="SignatorDoc - Sistema de Assinatura de Documentos Digitais";
                            this.isPDFNetBug = true;
                        }
                    }
                    else {
                        if (this.pdf2ImageConverter != null)
                            this.pdf2ImageConverter.Dispose();
                        this.pdf2ImageConverter = null;
                        this.pdf2ImageConverter = new Pdf2Image(auxFileName);
                        this.ShowDocumentSignModeGhostScript();
                    }
                }
                else
                {
                    this.isLayoutAdobeMode = false;
                    this.tpnl_PDFNavigation.Visible = true;
                    this.pnl_ViewerBodyMain.Controls.Remove(this.pnl_PDFViewAdobeMode);
                    this.pnl_ViewerBodyMain.Controls.Add(this.tpnl_PDFViewerSignMode);
                }

            }
        }

        private void tmrAfterLoadingPanel_Tick(object sender, EventArgs e)
        {
            this.tmrAfterLoadingPanel.Stop();
            this.showOrHideLoadingPanel(false);
        }

        private void frm_SigConfig_ConfigConfigurations_Button_Click(object sender, EventArgs e)
        {
            if (this.frm_SignConfig.IsValidatedFilds())
            {
                if (AweSomeUtils.isLockSignDevice)
                    AweSomeUtils.isLockSignDevice = false;

                this.docSigner.setSignerName(this.frm_SignConfig.getSignerName());
                this.docSigner.setX509Certificate2(this.frm_SignConfig.getSelectedCertificate());

                if (((this.frm_SignConfig.getSelectedStamp() != null) && (!this.frm_SignConfig.getIsHandWrittenSig())) || (this.frm_SignConfig.isInvisibleSignature()))
                {
                    if(this.frm_SignConfig.isInvisibleSignature())
                        this.SignatureFieldCollection[this.SigContainerCurrentIndex].setSigImageStamp(global::SignatorDocSolution.Properties.Resources.EmptyField);
                    else
                        this.SignatureFieldCollection[this.SigContainerCurrentIndex].setSigImageStamp(this.frm_SignConfig.getSelectedStamp());
                    this.SignatureFieldCollection[this.SigContainerCurrentIndex].setAlreadyToSign(true);
                    this.SignatureFieldCollection[this.SigContainerCurrentIndex].Show_btn_SigFieldRepeat(false);
                    if (this.frm_SignConfig.isInvisibleSignature())
                        this.SignatureFieldCollection[this.SigContainerCurrentIndex].StampIndex = 0;
                    else
                        this.SignatureFieldCollection[this.SigContainerCurrentIndex].StampIndex = this.frm_SignConfig.getSelectedStampIndex();
                    if (!this.btn_SealSignature.Enabled && (!this.isMassiveSign))
                        this.btn_SealSignature.Enabled = true;

                }
                else
                {
                    if (this.frm_SignConfig.getIsHandWrittenSig())
                    {
                        if (this.SignatureFieldCollection[this.SigContainerCurrentIndex].getIsHandWrittenDone())
                        {
                            this.SignatureFieldCollection[this.SigContainerCurrentIndex].Show_btn_SigFieldRepeat(true);
                            if (this.frm_SignConfig.getIsHandWrittenStamp())
                            {
                                this.SignatureFieldCollection[this.SigContainerCurrentIndex].StampIndex = this.frm_SignConfig.getSelectedStampIndex();
                                this.SignatureFieldCollection[this.SigContainerCurrentIndex].tempStampImage = this.frm_SignConfig.getSelectedStamp();
                                this.SignatureFieldCollection[this.SigContainerCurrentIndex].setSigHandWrittenStamp(this.frm_SignConfig.getSelectedStamp());
                                this.SignatureFieldCollection[this.SigContainerCurrentIndex].stampHasBeenSelected = true;
                            }
                            else
                            {
                                this.SignatureFieldCollection[this.SigContainerCurrentIndex].StampIndex = 0;
                                if (this.SignatureFieldCollection[this.SigContainerCurrentIndex].stampHasBeenSelected)
                                {
                                    this.SignatureFieldCollection[this.SigContainerCurrentIndex].clearSigImageStamp();
                                    this.SignatureFieldCollection[this.SigContainerCurrentIndex].Image = this.SignatureFieldCollection[this.SigContainerCurrentIndex].tempHandWrittenImage;
                                }
                            }
                        }
                        else
                        {
                            this.SignatureFieldCollection[this.SigContainerCurrentIndex].reconfigureSigHandWritten();
                            AweSomeUtils.isLockSignDevice = true;
                            if (this.frm_SignConfig.getIsHandWrittenStamp())
                            {
                                this.SignatureFieldCollection[this.SigContainerCurrentIndex].tempStampImage = this.frm_SignConfig.getSelectedStamp();
                                this.SignatureFieldCollection[this.SigContainerCurrentIndex].StampIndex = this.frm_SignConfig.getSelectedStampIndex();
                                this.SignatureFieldCollection[this.SigContainerCurrentIndex].tempStampImage = this.frm_SignConfig.getSelectedStamp();
                                this.SignatureFieldCollection[this.SigContainerCurrentIndex].stampHasBeenSelected = true;
                            }
                            else
                                this.SignatureFieldCollection[this.SigContainerCurrentIndex].StampIndex = 0;
                        }
                    }

                }

                this.SignatureFieldCollection[this.SigContainerCurrentIndex].setSignatureFieldOptions(this.frm_SignConfig.getSignatureFieldOptions());

                this.frm_SignConfig.Hide();

                this.SignatureFieldCollection[this.SigContainerCurrentIndex].Focus();
            }
        }

        private void frm_ReplicSigField_ConfirmReplicate_Button_Click(object sender, EventArgs e)
        {
            int pageIndex=0;
            int sigFieldIndex = 0;              
            foreach (PairItemsInt pit in this.frm_ReplicSigField.getListPairPagesSigFiels())
            {
                try
                {
                    pageIndex = pit.Item;
                    sigFieldIndex = pit.Value;
                    SigContainerCount++;
                    if (SigContainerCount < 50)
                    {

                        SignatureField sigFieldAux = this.SignatureFieldCollection[sigFieldIndex];
                        SignatureFieldCollection.Add(new SignatureField("sig_" + SigContainerCount + "_page_" + (pageIndex - 1), (new System.Drawing.Point(sigFieldAux.Location.X, sigFieldAux.Location.Y)), (pageIndex - 1), sigFieldAux.initialDeviceIsPresent, new EventHandler(SignatureFieldClose_Click), new EventHandler(SignatureFieldConfig_Click), new SignatorDocSolution.Utils.CustomEvent.GenericDelegateEventHandler(SignatureField_AfterSign), this.tlTip_Main));
                        SignatureFieldCollection[SigContainerCount].KeyDownEvent += new KeyEventHandler(SignatureField_KeyDown);

                        SignatureFieldCollection[SigContainerCount].deviceSerieNumber = sigFieldAux.deviceSerieNumber;
                        if (SignatureFieldCollection[SigContainerCount].deviceSerieNumber == 1)
                        {
                            SignatureFieldCollection[SigContainerCount].setExtenalEventDeviceScreenClick(new CustomEvent.GenericDelegateEventHandler(afterDeviceScreenClick));
                        }

                        if (this.currentPageIndex == (pageIndex-1))
                        {
                            this.pnl_PDFPageViewer.Controls[0].Controls.Add(SignatureFieldCollection[SigContainerCount]);
                            this.pnl_PDFPageViewer.Controls[0].Controls[(this.pnl_PDFPageViewer.Controls[0].Controls.Count - 1)].BringToFront();
                        }
                        else{
                            this.pdfPages[(pageIndex - 1)].Controls.Add(SignatureFieldCollection[SigContainerCount]);
                            this.pdfPages[(pageIndex - 1)].Controls[(this.pdfPages[(pageIndex - 1)].Controls.Count - 1)].BringToFront();
                        }
                        SignatureFieldCollection[SigContainerCount].StampIndex = sigFieldAux.StampIndex;
                        SignatureFieldCollection[SigContainerCount].setIsHandWrittenDone(sigFieldAux.getIsHandWrittenDone());
                        SignatureFieldCollection[SigContainerCount].setAlreadyToSign(sigFieldAux.getAlreadyToSign());
                        SignatureFieldCollection[SigContainerCount].setSignatureFieldOptions(sigFieldAux.SigFieldSealType);
                        SignatureFieldCollection[SigContainerCount].BackgroundImage = sigFieldAux.BackgroundImage;
                        SignatureFieldCollection[SigContainerCount].Image = sigFieldAux.Image;
                        SignatureFieldCollection[SigContainerCount].setDataBioSig(sigFieldAux.getDataBioSig());
                        SignatureFieldCollection[SigContainerCount].stampHasBeenSelected = sigFieldAux.stampHasBeenSelected;
                        SignatureFieldCollection[SigContainerCount].tempHandWrittenImage = sigFieldAux.tempHandWrittenImage;
                        SignatureFieldCollection[SigContainerCount].tempStampImage = sigFieldAux.tempStampImage;
                        if (SignatureFieldCollection[SigContainerCount].getIsHandWrittenDone())
                            SignatureFieldCollection[SigContainerCount].Show_btn_SigFieldRepeat(true);
                        if (currentPageIndex == pageIndex)
                        {
                            SignatureFieldCollection[SigContainerCount].Focus();
                            this.SigContainerCurrentIndex = SigContainerCount;
                        }
                    }
                }
                catch (Exception exc)
                {
                    MessageBox.Show("O recurso solicitado está ocioso. " + exc.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            this.frm_ReplicSigField.clearReplicatedFields();
            this.frm_ReplicSigField.Hide();
        }

        private void frm_RemoveSigField_ConfirmRemove_Button_Click(object sender, EventArgs e)
        {
            foreach (int indexSigFieldRemove in this.frm_RemoveSigField.getSelectedFields())
            {
                SignatureField signatureFieldAux = SignatureFieldCollection[indexSigFieldRemove];
                if ((!signatureFieldAux.getIsHandWrittenDone()) && (signatureFieldAux.getIsSignatureDrawConfigured()) && (signatureFieldAux.SigFieldSealType.ToString().Contains("SigImage")))
                {
                    AweSomeUtils.isLockSignDevice = false;
                    signatureFieldAux.releaseResouces();
                }
                else
                    signatureFieldAux.releaseAndremoveItSelf();
                
                this.SignatureFieldCollection.Remove(signatureFieldAux);

                if (((indexSigFieldRemove == this.SigContainerCurrentIndex) || (this.SigContainerCount <= this.SigContainerCurrentIndex)) && (this.SigContainerCurrentIndex > 0))
                    this.SigContainerCurrentIndex--;

                if (this.SigContainerCount >= 0)
                {
                    this.SigContainerCount--;
                }
                if (this.SigContainerCount < 0)
                {
                    if (this.btn_SealSignature.Enabled)
                        this.btn_SealSignature.Enabled = false;

                    if (this.btn_DoubleSignatureField.Enabled)
                        this.btn_DoubleSignatureField.Enabled = false;

                    if (this.btn_ReplicationSignatureFields.Enabled)
                        this.btn_ReplicationSignatureFields.Enabled = false;
                }
            }
        }

        private void frm_SetSigStringPattern_Confirm_Button_Click(object sender, EventArgs e)
        {
            if (this.isMassiveSign)
            {
                this.confirmFrmSearchPatternMassive();
            }
            else {
                this.confirmFrmSearchPatternSimple();
            }
        }

        private void frm_DefineSigFieldsMassiveDocs_Confirm_Button_Click(object sender, System.EventArgs e)
        {
            PDFDocInfo tempAuxdocinfo = null;
            foreach (PDFDocInfo docinfo in this.frm_DefineSigFieldsMassiveDocs.getlistPDFDocInfo())
            {
                if (docinfo.fieldEnabled)
                {
                    #region Handle Passwords Encryption Document
                    // Begin handleEncryption
                    if (this.isPassNeedEditDoc)
                    {
                        tempAuxdocinfo = this.listDocumentName.Find(doc => doc.documentName == docinfo.documentName);
                        if (tempAuxdocinfo != null)
                        {
                            if (tempAuxdocinfo.isNeedPassToEdit)
                            {
                                docinfo.isNeedPassToEdit = true;
                                docinfo.passwdToEdit = tempAuxdocinfo.passwdToEdit;
                            }
                        }

                    }
                    // End handleEncryption
                    #endregion

                    this.PDFDocInfoList.Add(docinfo);
                }
            }

            if (this.PDFDocInfoList.Count > 0)
            {
                if (!this.btn_SealSignature.Enabled)
                    this.btn_SealSignature.Enabled = true;
                if (!this.btn_ReviewMassiveSignDocsFields.Enabled)
                    this.btn_ReviewMassiveSignDocsFields.Enabled = true;
            }

            this.frm_DefineSigFieldsMassiveDocs.Hide();
        }

        private void frm_SigFieldMassiveDocReview_Confirm_Button_Click(object sender, System.EventArgs e){
            if (this.frm_SigFieldMassiveDocReview.checkIfListChanged()) {
                this.PDFDocInfoList = this.frm_SigFieldMassiveDocReview.getSelectedPDFDocInfoList();
                if (this.PDFDocInfoList.Count < 1)
                {
                    this.btn_ReviewMassiveSignDocsFields.Enabled = false;
                    this.btn_SealSignature.Enabled = false;
                }
            }
            this.frm_SigFieldMassiveDocReview.Hide();
        }

        private void AfterRemoveLicence() {
            this.tpnl_PDFMain.Enabled = false;
        }

        #endregion

        #region Signature Events
        private void btn_AddSignatureField_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.pdfDocLoaded)
                {
                    if (SigContainerCount <= 0)
                    {
                        if (this.optionLocationSigField == null)
                        {
                            this.optionLocationSigField = new OptionSigFieldLocationStart();
                            this.optionLocationSigField.setClickEvent(new EventHandler(OptionSigFieldLocationStart_Button_Click));
                            this.popUp.setPopupContent(this.optionLocationSigField);
                            this.popUp.Show(sender as Button);
                        }
                    }
                    else
                        if (this.isFieldPatternAdded) {
                            this.isFieldPatternAdded = false;
                            if (this.optionLocationSigField == null)
                            {
                                this.optionLocationSigField = new OptionSigFieldLocationStart();
                                this.optionLocationSigField.setClickEvent(new EventHandler(OptionSigFieldLocationStart_Button_Click));
                                this.popUp.setPopupContent(this.optionLocationSigField);
                                this.popUp.Show(sender as Button);
                            }
                        }
                    this.popUp.Show();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("O recurso demorou para responder. " + exc.Message, "Aviso", MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        private void btn_SealSignature_Click(object sender, System.EventArgs e)
        {
            if (this.isMassiveSign)
            {
                this.signMassiveDocuments();
            }
            else
            {
                this.signSimpleDocument();
            }
            
        }

        private void OptionSigFieldLocationStart_Button_Click(object sender, EventArgs e)
        {
            try
            {
                SigContainerCount++;
                if (SigContainerCount < 50)
                {
                    bool isDevicePresent = false;
                    if (DetectDevice.thereIsDeviceAvailable())
                        isDevicePresent = true;

                    this.frm_SignConfig.setInitialConfig(isDevicePresent);
                    this.frm_SignConfig.clearOptions(true);
                    SignatureFieldCollection.Add(new SignatureField("sig_" + SigContainerCount + "_page_" + this.currentPageIndex, getStartSignatureFieldLocation(((Button)sender).Name), this.currentPageIndex, isDevicePresent, new EventHandler(SignatureFieldClose_Click), new EventHandler(SignatureFieldConfig_Click), new SignatorDocSolution.Utils.CustomEvent.GenericDelegateEventHandler(SignatureField_AfterSign), this.tlTip_Main));
                    SignatureFieldCollection[SigContainerCount].KeyDownEvent += new KeyEventHandler(SignatureField_KeyDown);
                    this.pnl_PDFPageViewer.Controls[0].Controls.Add(SignatureFieldCollection[SigContainerCount]);
                    this.pnl_PDFPageViewer.Controls[0].Controls[(this.pnl_PDFPageViewer.Controls[0].Controls.Count - 1)].BringToFront();

                    if (SignatureFieldCollection[SigContainerCount].deviceSerieNumber == 1) {
                        SignatureFieldCollection[SigContainerCount].setExtenalEventDeviceScreenClick(new CustomEvent.GenericDelegateEventHandler(afterDeviceScreenClick));
                    }

                    if (!this.isMassiveSign)
                    {
                        if (!isDevicePresent)
                            SignatureFieldCollection[SigContainerCount].StampIndex = 2;
                        if (!isDevicePresent)
                            if (!this.btn_SealSignature.Enabled)
                                this.btn_SealSignature.Enabled = true;
                        if (!this.btn_RemoveSignatureFields.Enabled)
                            this.btn_RemoveSignatureFields.Enabled = true;
                        this.SigContainerCurrentIndex = this.SigContainerCount;
                    }

                    if (!this.btn_DoubleSignatureField.Enabled)
                        this.btn_DoubleSignatureField.Enabled = true;

                    if (!this.btn_ReplicationSignatureFields.Enabled)
                        this.btn_ReplicationSignatureFields.Enabled = true;

                }
                this.popUp.Close();
                SignatureFieldCollection[SigContainerCount].Focus();
                if (this.isMassiveSign && this.isDocChanged && this.PDFDocInfoList.Count > 0)
                    this.PDFDocInfoList.Clear();
            }
            catch (Exception exc)
            {
                MessageBox.Show("O recurso solicitado está ocioso. " + exc.Message,"Aviso", MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        private void SignatureFieldClose_Click(object sender, EventArgs e)
        {
            SignatureField signatureFieldAux = (SignatureField)((Button)sender).Parent;

            if ((!signatureFieldAux.getIsHandWrittenDone()) && (signatureFieldAux.getIsSignatureDrawConfigured()) && (signatureFieldAux.SigFieldSealType.ToString().Contains("SigImage")))
            {
                AweSomeUtils.isLockSignDevice = false;
                signatureFieldAux.releaseResouces();
            }
            else
                signatureFieldAux.releaseAndremoveItSelf();
                   
            int indexSigFieldRemove = SignatureFieldCollection.IndexOf(signatureFieldAux);
            
            this.SignatureFieldCollection.Remove(signatureFieldAux);

            if (((indexSigFieldRemove == this.SigContainerCurrentIndex) || (this.SigContainerCount <= this.SigContainerCurrentIndex)) && (this.SigContainerCurrentIndex > 0))
                this.SigContainerCurrentIndex--;

            if (this.SigContainerCount >= 0)
            {
                this.SigContainerCount--;
            }

            if (this.SigContainerCount < 0)
            {
                if (!this.isMassiveSign)
                    if (this.btn_SealSignature.Enabled)
                        this.btn_SealSignature.Enabled = false;

                if (this.btn_DoubleSignatureField.Enabled)
                    this.btn_DoubleSignatureField.Enabled = false;

                if (this.btn_ReplicationSignatureFields.Enabled)
                    this.btn_ReplicationSignatureFields.Enabled = false;

                if (this.btn_RemoveSignatureFields.Enabled)
                    this.btn_RemoveSignatureFields.Enabled = false;
            }
            
        }

        private void SignatureField_AfterSign() {
            if ((!this.btn_SealSignature.Enabled) && (!this.isMassiveSign))
                this.btn_SealSignature.Enabled = true;
        }

        private void afterDeviceScreenClick() {
            this.btn_SealSignature.PerformClick();
        }

        private void SignatureField_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (!AweSomeUtils.isLockSignDevice)
                {
                    SigContainerCount++;
                    if (SigContainerCount < 50)
                    {
                        bool isDevicePresent = false;
                        if (DetectDevice.thereIsDeviceAvailable())
                            isDevicePresent = true;

                        SignatureField sigFieldAux = (SignatureField)sender;
                        SignatureFieldCollection.Add(new SignatureField("sig_" + SigContainerCount + "_page_" + this.currentPageIndex, (new System.Drawing.Point(sigFieldAux.Location.X + (sigFieldAux.Size.Width / 2), sigFieldAux.Location.Y + (sigFieldAux.Size.Height / 2))), this.currentPageIndex, isDevicePresent, new EventHandler(SignatureFieldClose_Click), new EventHandler(SignatureFieldConfig_Click), new SignatorDocSolution.Utils.CustomEvent.GenericDelegateEventHandler(SignatureField_AfterSign), this.tlTip_Main));
                        SignatureFieldCollection[SigContainerCount].KeyDownEvent += new KeyEventHandler(SignatureField_KeyDown);
                        this.pnl_PDFPageViewer.Controls[0].Controls.Add(SignatureFieldCollection[SigContainerCount]);
                        this.pnl_PDFPageViewer.Controls[0].Controls[(this.pnl_PDFPageViewer.Controls[0].Controls.Count - 1)].BringToFront();

                        SignatureFieldCollection[SigContainerCount].deviceSerieNumber = sigFieldAux.deviceSerieNumber;
                        if (SignatureFieldCollection[SigContainerCount].deviceSerieNumber == 1)
                        {
                            SignatureFieldCollection[SigContainerCount].setExtenalEventDeviceScreenClick(new CustomEvent.GenericDelegateEventHandler(afterDeviceScreenClick));
                        }

                        SignatureFieldCollection[SigContainerCount].StampIndex = sigFieldAux.StampIndex;
                        SignatureFieldCollection[SigContainerCount].setIsHandWrittenDone(sigFieldAux.getIsHandWrittenDone());
                        SignatureFieldCollection[SigContainerCount].setAlreadyToSign(sigFieldAux.getAlreadyToSign());
                        SignatureFieldCollection[SigContainerCount].setSignatureFieldOptions(sigFieldAux.SigFieldSealType);
                        SignatureFieldCollection[SigContainerCount].BackgroundImage = sigFieldAux.BackgroundImage;
                        SignatureFieldCollection[SigContainerCount].Image = sigFieldAux.Image;
                        SignatureFieldCollection[SigContainerCount].setDataBioSig(sigFieldAux.getDataBioSig());
                        SignatureFieldCollection[SigContainerCount].stampHasBeenSelected = sigFieldAux.stampHasBeenSelected;
                        SignatureFieldCollection[SigContainerCount].tempHandWrittenImage = sigFieldAux.tempHandWrittenImage;
                        SignatureFieldCollection[SigContainerCount].tempStampImage = sigFieldAux.tempStampImage;
                        if (SignatureFieldCollection[SigContainerCount].getIsHandWrittenDone())
                            SignatureFieldCollection[SigContainerCount].Show_btn_SigFieldRepeat(true);
                        SignatureFieldCollection[SigContainerCount].Focus();
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("O recurso solicitado está ocioso. " + exc.Message,"Aviso",MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_DoubleSignatureField_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (!AweSomeUtils.isLockSignDevice)
                {
                    SigContainerCount++;
                    if (SigContainerCount < 50)
                    {
                        bool isDevicePresent = false;
                        if (DetectDevice.thereIsDeviceAvailable())
                            isDevicePresent = true;

                        SignatureField sigFieldAux = this.SignatureFieldCollection[SigContainerCurrentIndex];
                        SignatureFieldCollection.Add(new SignatureField("sig_" + SigContainerCount + "_page_" + this.currentPageIndex, (new System.Drawing.Point(sigFieldAux.Location.X + (sigFieldAux.Size.Width / 2), sigFieldAux.Location.Y + (sigFieldAux.Size.Height / 2))), this.currentPageIndex, isDevicePresent, new EventHandler(SignatureFieldClose_Click), new EventHandler(SignatureFieldConfig_Click), new SignatorDocSolution.Utils.CustomEvent.GenericDelegateEventHandler(SignatureField_AfterSign), this.tlTip_Main));
                        SignatureFieldCollection[SigContainerCount].KeyDownEvent += new KeyEventHandler(SignatureField_KeyDown);
                        this.pnl_PDFPageViewer.Controls[0].Controls.Add(SignatureFieldCollection[SigContainerCount]);
                        this.pnl_PDFPageViewer.Controls[0].Controls[(this.pnl_PDFPageViewer.Controls[0].Controls.Count - 1)].BringToFront();

                        SignatureFieldCollection[SigContainerCount].deviceSerieNumber = sigFieldAux.deviceSerieNumber;
                        if (SignatureFieldCollection[SigContainerCount].deviceSerieNumber == 1)
                        {
                            SignatureFieldCollection[SigContainerCount].setExtenalEventDeviceScreenClick(new CustomEvent.GenericDelegateEventHandler(afterDeviceScreenClick));
                        }

                        SignatureFieldCollection[SigContainerCount].StampIndex = sigFieldAux.StampIndex;
                        SignatureFieldCollection[SigContainerCount].setIsHandWrittenDone(sigFieldAux.getIsHandWrittenDone());
                        SignatureFieldCollection[SigContainerCount].setAlreadyToSign(sigFieldAux.getAlreadyToSign());
                        SignatureFieldCollection[SigContainerCount].setSignatureFieldOptions(sigFieldAux.SigFieldSealType);
                        SignatureFieldCollection[SigContainerCount].BackgroundImage = sigFieldAux.BackgroundImage;
                        SignatureFieldCollection[SigContainerCount].Image = sigFieldAux.Image;
                        SignatureFieldCollection[SigContainerCount].setDataBioSig(sigFieldAux.getDataBioSig());
                        SignatureFieldCollection[SigContainerCount].stampHasBeenSelected = sigFieldAux.stampHasBeenSelected;
                        SignatureFieldCollection[SigContainerCount].tempHandWrittenImage = sigFieldAux.tempHandWrittenImage;
                        SignatureFieldCollection[SigContainerCount].tempStampImage = sigFieldAux.tempStampImage;
                        if (SignatureFieldCollection[SigContainerCount].getIsHandWrittenDone())
                            SignatureFieldCollection[SigContainerCount].Show_btn_SigFieldRepeat(true);
                        SignatureFieldCollection[SigContainerCount].Focus();
                        this.SigContainerCurrentIndex = SigContainerCount;
                    }
                }
                else
                    System.Windows.Forms.MessageBox.Show("Um campo está aguardando ser assinado!\nFinalize a assinatura em andamento antes de duplicar campos.", "Assinatura de Campos", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
            catch (Exception exc)
            {
                MessageBox.Show("O recurso solicitado está ocioso. " + exc.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_ReplicationSignatureFields_Click(object sender, System.EventArgs e)
        {
            if (!AweSomeUtils.isLockSignDevice)
            {

                if (this.isMassiveSign)
                {
                    this.frm_DefineSigFieldsMassiveDocs.cleanConfiguration();
                    this.frm_DefineSigFieldsMassiveDocs.setPreSigFields(this.SignatureFieldCollection);
                    this.frm_DefineSigFieldsMassiveDocs.ShowDialog();
                }
                else
                {
                    this.ReplicationSignatureFieldSimple();
                }
            }
            else
                System.Windows.Forms.MessageBox.Show("Um campo de assinatura está aguardando ser assinado!\nFinalize a assinatura antes de replicar campos.", "Assinatura de Campos", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
        }
    
        private void btn_RemoveSignatureFields_Click(object sender, System.EventArgs e)
        {
            object[] objFields = new object[this.SignatureFieldCollection.Count];

            string fieldCodLocationName = null;

            for (int i = 0; i < objFields.Length; i++)
            {
                fieldCodLocationName = this.SignatureFieldCollection[i].Location.X + " H  " + this.SignatureFieldCollection[i].Location.Y + " V";

                objFields[i] = "Campo " + fieldCodLocationName + "   -   Página " + this.SignatureFieldCollection[i].getPageNumber();
            }
            this.frm_RemoveSigField.setSigFields(objFields);

            this.frm_RemoveSigField.ShowDialog();
        }

        private void SignatureFieldConfig_Click(object sender, EventArgs e)
        {
            if (AweSomeUtils.isLockSignDevice)
            {
                System.Windows.Forms.MessageBox.Show("Não é possível abrir as configurações enquanto um campo estiver em modo de assinatura! Finalize a assinatura para abrir as configurações de assinatura.", "Assinatura de Campos", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
            else
            {
                SignatureField auxSignatureField = (SignatureField)(((Button)sender).Parent);
                this.SigContainerCurrentIndex = SignatureFieldCollection.IndexOf(auxSignatureField);
                if (auxSignatureField.getAlreadyToSign())
                {
                    this.frm_SignConfig.setSignatureFieldOptions(auxSignatureField.SigFieldSealType, auxSignatureField.StampIndex);
                }
                else
                {
                    this.frm_SignConfig.clearOptions(true);
                }
                this.frm_SignConfig.ShowDialog();
            }
        }

        private void btn_ReviewMassiveSignDocsFields_Click(object sender, System.EventArgs e)
        {
            this.frm_SigFieldMassiveDocReview.setSigFieldsConfig(this.PDFDocInfoList);
            this.frm_SigFieldMassiveDocReview.ShowDialog();
        }

        #endregion

        #region General Methods

        private void ShowDocumentSignMode()
        {
            try
            {
                this.CountPages = this.PDFDocument.GetPageCount();           

                this.pdfPages = new PictureBox[this.CountPages];

                Page page = this.PDFDocument.GetPage(1);
                Defins.setPagebaseSize(new Size((int)((page.GetPageWidth() / 72) * Defins.pagesDPI), (int)((page.GetPageHeight() / 72) * Defins.pagesDPI)));

                for (int i = 0; i < this.CountPages; i++)
                {
                    this.pdfPages[i] = createPageView();
                }
                this.setPageViewContent(0);
                this.pnl_PDFPageViewer.Controls.Add(pdfPages[0]);

                this.txt_PDFNavCurrPage.Text = this.currentPageNumber.ToString() + " / " + this.CountPages;

                this.pdfDocLoaded = true;

                this.pnlThumbnail = new PanelThumbnail();
                pnlThumbnail.setEventHandler(new EventHandler(PanelThumbNail_ThumbnailN_Click));
                pnlThumbnail.Location = new System.Drawing.Point(0, 0);
                pnlThumbnail.Dock = DockStyle.Fill;
                this.pnl_PDFThumbnail.Controls.Add(pnlThumbnail);
                pnlThumbnail.setTbNailPages(this.CountPages);

                if (this.CountPages < 11)
                    pnlThumbnail.ConfigureThumbNails(this.PDFDocument, 1, this.CountPages);
                else
                {
                    pnlThumbnail.ConfigureThumbNails(this.PDFDocument, 1, 10);
                    System.Threading.Thread threadFillThumnails = new System.Threading.Thread(() =>
                    {
                        pnlThumbnail.ConfigureThumbNails(this.PDFDocument, 11, this.CountPages);
                    });
                    threadFillThumnails.Start();
                }

                this.btn_AddSignatureField.Enabled = true;
                this.setFieldSearchPatternToolStripMenuItem.Enabled = true;
                if (this.isDocChanged)
                {
                    this.btn_SaveDocument.Enabled = true;
                    this.Mnu_Item_Save.Enabled = true;
                }
                else
                {
                    this.btn_SaveDocument.Enabled = false;
                    this.Mnu_Item_Save.Enabled = false;
                }

                if (!this.propertiesToolStripMenuItem.Enabled)
                    this.propertiesToolStripMenuItem.Enabled = true;

            }
            catch (pdftron.Common.PDFNetException pdne)
            {
                if (pdne.Message.Contains("Compressed object is corrupt"))
                {
                    MessageBox.Show("Este Documento está protegido com senha.\nPara habilitar o uso de senhas em documentos protegidos, acesse o menu ARQUIVO => CONFIGURAÇÕES e desmarque a opção \"Desabilitar Verificação de Senha de Proteção do Documento\".\n\nA ativação da verificação de senha pode deixar a abertura de documentos um pouco lenta.", "Abertura de Documento", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Restart();
                    Application.ExitThread();
                }
                else
                    throw new System.Exception("Ocorreu um erro em objeto interno responsável por carregar as páginas do documento. Se o Erro persistir, contate o fornecedor do sistema. Erro:" + pdne.Message);
            }
        }

        private void ShowDocumentSignModeGhostScript()
        {
            if (this.isPassNeedOpenDoc)
            {
                MessageBox.Show("Ocorreu uma falha no objeto gsdll32 ao tentar abrir o documento. Se o erro persistir contate o Fornecedor do sistema.", "Abertura de Documento", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Restart();
                Application.ExitThread();
            }
            else
            {
                try
                {
                    iTextSharp.text.pdf.PdfReader itextReader = null;
                    itextReader = new iTextSharp.text.pdf.PdfReader(File.ReadAllBytes(this.tempFileName));


                    iTextSharp.text.Rectangle pageRect = itextReader.GetPageSize(1);
                    itextReader.Close();

                    this.pdf2ImageConverter = new Pdf2Image(this.tempFileName);
                    this.pdf2ImageConverter.Settings.Dpi = System.Convert.ToInt32(Defins.pagesDPI);
                    this.pdf2ImageConverter.Settings.ImageFormat = GhostScriptUtils.NativeGhostScript.ImageFormat.Png24;
                    this.CountPages = this.pdf2ImageConverter.PageCount;
                    this.frm_ReplicSigField.setPageNumberQts(this.CountPages);


                    this.pdfPages = new PictureBox[this.CountPages];

                    Defins.setPagebaseSize(new Size((int)((pageRect.Width / 72) * Defins.pagesDPI), (int)((pageRect.Height / 72) * Defins.pagesDPI)));

                    for (int i = 0; i < this.CountPages; i++)
                    {
                        this.pdfPages[i] = createPageView();
                    }
                    this.setPageViewContentGhostScript(0);
                    this.pnl_PDFPageViewer.Controls.Add(pdfPages[0]);

                    this.txt_PDFNavCurrPage.Text = this.currentPageNumber.ToString() + " / " + this.CountPages;

                    this.pdfDocLoaded = true;

                    this.pnlThumbnail = new PanelThumbnail();
                    pnlThumbnail.setEventHandler(new EventHandler(PanelThumbNail_ThumbnailN_Click));
                    pnlThumbnail.Location = new System.Drawing.Point(0, 0);
                    pnlThumbnail.Dock = DockStyle.Fill;
                    this.pnl_PDFThumbnail.Controls.Add(pnlThumbnail);
                    pnlThumbnail.setTbNailPages(this.CountPages);

                    if (this.CountPages < 11)
                        pnlThumbnail.ConfigureThumbNailsGhostScript(1, this.CountPages);
                    else
                    {
                        pnlThumbnail.ConfigureThumbNailsGhostScript(1, 10);
                        System.Threading.Thread threadFillThumnails = new System.Threading.Thread(() =>
                        {
                            pnlThumbnail.ConfigureThumbNailsGhostScript(11, this.CountPages);
                        });
                        threadFillThumnails.Start();
                    }

                    this.btn_AddSignatureField.Enabled = true;
                    this.setFieldSearchPatternToolStripMenuItem.Enabled = true;
                    if (this.isDocChanged)
                    {
                        this.btn_SaveDocument.Enabled = true;
                        this.Mnu_Item_Save.Enabled = true;
                    }
                    else
                    {
                        this.btn_SaveDocument.Enabled = false;
                        this.Mnu_Item_Save.Enabled = false;
                    }

                    if (!this.propertiesToolStripMenuItem.Enabled)
                        this.propertiesToolStripMenuItem.Enabled = true;

                }catch(iTextSharp.text.pdf.BadPasswordException bpe){
                    MessageBox.Show("Ocorreu uma falha ao tentar abrir o documento, a criptografia não pode ser verificada. Se o erro persistir contate o Fornecedor do sistema.", "Abertura de Documento", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Restart();
                    Application.ExitThread();
                }
                catch(System.IndexOutOfRangeException ioe){
                    throw new System.Exception("Ocorreu uma falha na obtenção das páginas através do objeto gsdll32");
                }
            }
        }

        private PictureBox createPageView()
        {
            PictureBox pictureBox_Page = new PictureBox();
            pictureBox_Page.BorderStyle = BorderStyle.FixedSingle;
            pictureBox_Page.Location = this.pageLocation;
            pictureBox_Page.Size = Defins.pageSizeBase;
            pictureBox_Page.Cursor = Cursors.Hand;
            pictureBox_Page.MouseDown += new MouseEventHandler(pictureBox_Page_MouseDown);
            pictureBox_Page.MouseUp += new MouseEventHandler(pictureBox_Page_MouseUp);
            pictureBox_Page.MouseLeave += new EventHandler(pictureBox_Page_MouseLeave);
            pictureBox_Page.MouseEnter += new EventHandler(pictureBox_Page_MouseEnter);
            pictureBox_Page.MouseMove += new MouseEventHandler(pictureBox_Page_MouseMove);
            return pictureBox_Page;
        }

        private void setPageViewContent(int pageIndex)
        {
            if (!this.isPDFNetBug)
            {
                byte[] pageContentBuffer = null;
                using (PDFDraw pdfDraw = new PDFDraw())
                {
                    int pageNumber = pageIndex < this.CountPages ? pageIndex + 1 : pageIndex;
                    pdfDraw.SetDPI(Defins.pagesDPI);
                    pdfDraw.SetDrawAnnotations(true);
                    try
                    {
                        using (Bitmap bitmap = pdfDraw.GetBitmap(this.PDFDocument.GetPage(pageNumber)))
                        {
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                bitmap.Save((Stream)memoryStream, ImageFormat.Png);
                                pageContentBuffer = memoryStream.ToArray();
                            }
                        }
                    }
                    catch (pdftron.Common.PDFNetException pexc)
                    {
                        return;
                    }
                    catch (Exception exc)
                    {
                        //    SignatorDocSolution.Utils.RegisterWinEventLogViewer.RegisterLog("The Source Form_Main_setPageViewContent throws the following message : ", exc.Message, 20, 2);//classid+error 0(objectnull or disposed)
                        return;
                    }
                }

                if ((pageIndex > 0) && (pageIndex < this.CountPages))
                {
                    if (this.pdfPages[pageIndex - 1].Image != null)
                        this.pdfPages[pageIndex - 1].Image.Dispose();
                }
                if ((pageIndex < (this.CountPages - 1)))
                {
                    if (this.pdfPages[pageIndex + 1].Image != null)
                        this.pdfPages[pageIndex + 1].Image.Dispose();
                }
                if (this.pdfPages[oldPageIndex].Image != null)
                    this.pdfPages[oldPageIndex].Image.Dispose();

                this.pdfPages[pageIndex].Image = System.Drawing.Image.FromStream(new MemoryStream(pageContentBuffer));
                oldPageIndex = pageIndex;
            }
            else {
                this.setPageViewContentGhostScript(pageIndex);
            }
        }

        private void setPageViewContentGhostScript(int pageIndex)
        {
            System.Drawing.Image imageContent = null;
            try
            {
                int pageNumber = pageIndex < this.CountPages ? pageIndex + 1 : pageIndex;
                imageContent = this.pdf2ImageConverter.GetImage(pageIndex + 1);
            }
            catch (Exception exc)
            {
                //    SignatorDocSolution.Utils.RegisterWinEventLogViewer.RegisterLog("The Source Form_Main_setPageViewContent throws the following message : ", exc.Message, 20, 2);//classid+error 0(objectnull or disposed)
                return;
            }


            if ((pageIndex > 0) && (pageIndex < this.CountPages))
            {
                if (this.pdfPages[pageIndex - 1].Image != null)
                    this.pdfPages[pageIndex - 1].Image.Dispose();
            }
            if ((pageIndex < (this.CountPages - 1)))
            {
                if (this.pdfPages[pageIndex + 1].Image != null)
                    this.pdfPages[pageIndex + 1].Image.Dispose();
            }
            if (this.pdfPages[oldPageIndex].Image != null)
                this.pdfPages[oldPageIndex].Image.Dispose();

            this.pdfPages[pageIndex].Image = imageContent;
            oldPageIndex = pageIndex;
        }

        public System.Drawing.Point getStartSignatureFieldLocation(string sigFieldPosition)
        {
            switch (sigFieldPosition)
            {
                case "btn_StartTopLeft":
                    {
                        this.pnl_PDFPageViewer.VerticalScroll.Value = 0;
                        this.pnl_PDFPageViewer.PerformLayout();
                        return new System.Drawing.Point(System.Convert.ToInt32(Defins.pageSizeBase.Width * 0.05), System.Convert.ToInt32(Defins.pageSizeBase.Height * 0.05));
                    }
                case "btn_StartTopCenter":
                    {
                        this.pnl_PDFPageViewer.VerticalScroll.Value = 0;
                        this.pnl_PDFPageViewer.PerformLayout();
                        return new System.Drawing.Point(System.Convert.ToInt32(Defins.pageSizeBase.Width * 0.5) - (Defins.SignatureField_Size_Base.Width / 2), System.Convert.ToInt32(Defins.pageSizeBase.Height * 0.05));
                    }
                case "btn_StartTopRight":
                    {
                        this.pnl_PDFPageViewer.VerticalScroll.Value = 0;
                        this.pnl_PDFPageViewer.PerformLayout();
                        return new System.Drawing.Point(System.Convert.ToInt32(Defins.pageSizeBase.Width * 0.95) - Defins.SignatureField_Size_Base.Width, System.Convert.ToInt32(Defins.pageSizeBase.Height * 0.05));
                    }
                case "btn_StartMiddleLeft":
                    {
                        this.pnl_PDFPageViewer.VerticalScroll.Value = System.Convert.ToInt32((this.pnl_PDFPageViewer.VerticalScroll.Maximum / 2) - (this.pnl_PDFPageViewer.VerticalScroll.Maximum / 4));
                        this.pnl_PDFPageViewer.PerformLayout();
                        return new System.Drawing.Point(System.Convert.ToInt32(Defins.pageSizeBase.Width * 0.05), System.Convert.ToInt32(Defins.pageSizeBase.Height * 0.5) - (Defins.SignatureField_Size_Base.Height / 2));
                    }
                case "btn_StartMiddleCenter":
                    {
                        this.pnl_PDFPageViewer.VerticalScroll.Value = System.Convert.ToInt32((this.pnl_PDFPageViewer.VerticalScroll.Maximum / 2) - (this.pnl_PDFPageViewer.VerticalScroll.Maximum / 4));
                        this.pnl_PDFPageViewer.PerformLayout();
                        return new System.Drawing.Point(System.Convert.ToInt32(Defins.pageSizeBase.Width * 0.5) - (Defins.SignatureField_Size_Base.Width / 2), System.Convert.ToInt32(Defins.pageSizeBase.Height * 0.5) - (Defins.SignatureField_Size_Base.Height / 2));
                    }
                case "btn_StartMiddleRight":
                    {
                        this.pnl_PDFPageViewer.VerticalScroll.Value = System.Convert.ToInt32((this.pnl_PDFPageViewer.VerticalScroll.Maximum / 2) - (this.pnl_PDFPageViewer.VerticalScroll.Maximum / 4));
                        this.pnl_PDFPageViewer.PerformLayout();
                        return new System.Drawing.Point(System.Convert.ToInt32(Defins.pageSizeBase.Width * 0.95) - Defins.SignatureField_Size_Base.Width, System.Convert.ToInt32(Defins.pageSizeBase.Height * 0.5) - (Defins.SignatureField_Size_Base.Height / 2));
                    }
                case "btn_StartBottomLeft":
                    {
                        this.pnl_PDFPageViewer.VerticalScroll.Value = this.pnl_PDFPageViewer.VerticalScroll.Maximum;
                        this.pnl_PDFPageViewer.PerformLayout();
                        return new System.Drawing.Point(System.Convert.ToInt32(Defins.pageSizeBase.Width * 0.05), System.Convert.ToInt32(Defins.pageSizeBase.Height * 0.95) - Defins.SignatureField_Size_Base.Height);
                    }
                case "btn_StartBottomCenter":
                    {
                        this.pnl_PDFPageViewer.VerticalScroll.Value = this.pnl_PDFPageViewer.VerticalScroll.Maximum;
                        this.pnl_PDFPageViewer.PerformLayout();
                        return new System.Drawing.Point(System.Convert.ToInt32(Defins.pageSizeBase.Width * 0.5) - (Defins.SignatureField_Size_Base.Width / 2), System.Convert.ToInt32(Defins.pageSizeBase.Height * 0.95) - Defins.SignatureField_Size_Base.Height);
                    }
                case "btn_StartBottomRight":
                    {
                        this.pnl_PDFPageViewer.VerticalScroll.Value = this.pnl_PDFPageViewer.VerticalScroll.Maximum;
                        this.pnl_PDFPageViewer.PerformLayout();
                        return new System.Drawing.Point(System.Convert.ToInt32(Defins.pageSizeBase.Width * 0.95) - Defins.SignatureField_Size_Base.Width, System.Convert.ToInt32(Defins.pageSizeBase.Height * 0.95) - Defins.SignatureField_Size_Base.Height);
                    }
                default:
                    {
                        this.pnl_PDFPageViewer.VerticalScroll.Value = System.Convert.ToInt32(this.pnl_PDFPageViewer.VerticalScroll.Maximum / 2);
                        this.pnl_PDFPageViewer.PerformLayout();
                        return new System.Drawing.Point(System.Convert.ToInt32(Defins.pageSizeBase.Width * 0.5) - (Defins.SignatureField_Size_Base.Width / 2), System.Convert.ToInt32(Defins.pageSizeBase.Height * 0.5) - (Defins.SignatureField_Size_Base.Height / 2));
                    }
            }
        }

        private void releasePDFViewerResources()
        {
            this.txt_PDFNavCurrPage.Text = string.Empty;
            this.currentPageNumber = 1;
            this.auxPageNumber = 1;
            this.currentPageIndex = 0;
            this.oldPageIndex = 0;
            this.SigContainerCount = (-1);
            this.SigContainerCurrentIndex = 0;

            this.SignatureFieldCollection.Clear();
            this.pnl_PDFPageViewer.Controls.Clear();
            try
            {
                if (this.pdfPages != null)
                {
                    this.pdfPages[this.currentPageIndex].Image.Dispose();
                    for (int i = 0; i < this.pdfPages.Length; i++)
                        this.pdfPages[this.currentPageIndex].Dispose();
                }
            }
            catch (Exception exc) { }
            if (pnlThumbnail != null)
            {
                pnlThumbnail.releaseThumbnail();

                this.pnl_PDFThumbnail.Controls.Remove(pnlThumbnail);
            }
            this.pdfPages = null;
            GC.Collect();

            this.docSigner.setSignerName(string.Empty);
            this.docSigner.setX509Certificate2(null);

            this.docSigner.clearSignerContext();

            this.btn_SealSignature.Enabled = false;
            this.btn_DoubleSignatureField.Enabled = false;
            if (this.isLayoutAdobeMode)
            {
                this.btn_AddSignatureField.Enabled = false;
                this.setFieldSearchPatternToolStripMenuItem.Enabled = false;
            }

            if (this.isDocChanged)
            {
                this.btn_SaveDocument.Enabled = true;
                this.Mnu_Item_Save.Enabled = true;
            }
            else
            {
                this.btn_SaveDocument.Enabled = false;
                this.Mnu_Item_Save.Enabled = false;
            }

            try
            {
                if (!this.isPDFNetBug)
                {
                    if (this.PDFDocument != null)
                    {
                        this.PDFDocument.Close();
                        this.PDFDocument.Dispose();
                    }
                }
                else {
                    if(this.pdf2ImageConverter!=null)
                        this.pdf2ImageConverter.Dispose();
                    this.pdf2ImageConverter = null;
                }
            }
            catch (Exception exc) { }

            if (this.isMassiveSign) {
                this.PDFDocInfoList.Clear();
                this.massiveFileNames.Clear();
                if(this.btn_ReviewMassiveSignDocsFields.Enabled)
                    this.btn_ReviewMassiveSignDocsFields.Enabled = false;
            }

            if (this.btn_SealSignature.Enabled)
                this.btn_SealSignature.Enabled = false;

            if (this.btn_DoubleSignatureField.Enabled)
                this.btn_DoubleSignatureField.Enabled = false;

            this.frm_SignConfig.clearOptions(false);

            this.frm_ReplicSigField.clearReplicatedFields();

            this.btn_ReplicationSignatureFields.Enabled = false;
            this.btn_RemoveSignatureFields.Enabled = false;

            if (AweSomeUtils.isLockSignDevice)
                NativeAweSome.UNBusyDevice();

            AweSomeUtils.isLockSignDevice = false;

            ConverterDocFormats.listErrorDocs = null;
        }

        private void releaseMassiveDocsConfigs()
        {
            this.SigContainerCount = (-1);
            this.SigContainerCurrentIndex = 0;
            this.SignatureFieldCollection.Clear();
            this.pnl_PDFPageViewer.Controls[0].Controls.Clear();
            if(!this.isDocChanged && (this.isSavedDoc))
                this.PDFDocInfoList.Clear();
            this.frm_SignConfig.clearOptions(false);
            this.docSigner.clearSignerContext();
            this.btn_SealSignature.Enabled = false;
            this.btn_ReviewMassiveSignDocsFields.Enabled = false;
            this.btn_ReplicationSignatureFields.Enabled = false;
            this.btn_DoubleSignatureField.Enabled = false;
            this.btn_SaveDocument.Enabled = true;
            this.Mnu_Item_Save.Enabled = true;

            if (AweSomeUtils.isLockSignDevice)
                NativeAweSome.UNBusyDevice();

            AweSomeUtils.isLockSignDevice = false;
        }

        private void reloadDocument()
        {

            if (this.frm_SignatureChartCompare.checkDocIsSigned(this.docSigner.getSignedFilePath(), this.isPassNeedOpenDoc, (this.isPassNeedOpenDoc ? this.listDocumentName[0].passwdToOpen : null))) // Changed in HandleEncryption
                this.SigChartCheckStripMenuItem.Enabled = true;
            else
                this.SigChartCheckStripMenuItem.Enabled = false;

            if (this.isAdbeReaderInstalled)
            {
                this.isLayoutAdobeMode = true;
                int auxCurrentPage = this.currentPageNumber;
                this.btn_AddSignatureField.Enabled = false;
                this.setFieldSearchPatternToolStripMenuItem.Enabled = false;
                this.rdo_AdobeMode.Checked = true;
                this.releasePDFViewerResources();
                this.tpnl_PDFNavigation.Visible = false;
                this.pnl_ViewerBodyMain.Controls.Remove(this.tpnl_PDFViewerSignMode);
                this.pnl_ViewerBodyMain.Controls.Add(this.pnl_PDFViewAdobeMode);

                this.ShowDocumentAdobeMode(this.docSigner.getSignedFilePath(), auxCurrentPage);

                this.btn_ReviewMassiveSignDocsFields.Visible = false;
                this.btn_ReviewMassiveSignDocsFields.Enabled = false;
            }
            else
            {
                try
                {
                    if (this.PDFDocument != null)
                    {
                        this.PDFDocument.Close();
                        this.PDFDocument.Dispose();
                        this.PDFDocument = null;
                    }
                }
                catch (Exception exc) { }

                if (!this.isPDFNetBug)
                {
                    this.PDFDocument = new PDFDoc(this.docSigner.getSignedFilePath());
                    if(this.isPassNeedOpenDoc)
                        this.PDFDocument.InitStdSecurityHandler(this.listDocumentName[0].passwdToOpen);
                }
                else
                {
                    if (this.pdf2ImageConverter != null)
                        this.pdf2ImageConverter.Dispose();
                    this.pdf2ImageConverter = null;
                    this.pdf2ImageConverter = new Pdf2Image(this.docSigner.getSignedFilePath());
                }

                this.docSigner.clearSignerContext();

                this.pnl_PDFPageViewer.Controls[0].Controls.Clear();

                this.SigContainerCount = (-1);
                this.SigContainerCurrentIndex = 0;
                this.SignatureFieldCollection.Clear();

                this.pdfPages[this.currentPageIndex].Image.Dispose();

                for (int i = 0; i < this.pdfPages.Length; i++)
                    this.pdfPages[i].Controls.Clear();

                if (!this.isPDFNetBug)
                    this.setPageViewContent(this.currentPageIndex);
                else
                    this.setPageViewContentGhostScript(this.currentPageIndex);

                this.docSigner.setSignerName(string.Empty);
                this.docSigner.setX509Certificate2(null);

                if (this.btn_SealSignature.Enabled)
                    this.btn_SealSignature.Enabled = false;

                if (this.btn_DoubleSignatureField.Enabled)
                    this.btn_DoubleSignatureField.Enabled = false;

                this.frm_SignConfig.clearOptions(false);

                this.frm_ReplicSigField.clearReplicatedFields();

                this.btn_ReplicationSignatureFields.Enabled = false;
                this.btn_RemoveSignatureFields.Enabled = false;
                this.btn_ReviewMassiveSignDocsFields.Enabled = false;
            }

            if (this.isMassiveSign)
            {
                this.PDFDocInfoList.Clear();
            }

            if (this.isDocChanged)
            {
                this.btn_SaveDocument.Enabled = true;
                this.Mnu_Item_Save.Enabled = true;
            }
            else
            {
                this.btn_SaveDocument.Enabled = false;
                this.Mnu_Item_Save.Enabled = false;
            }


        }

        private void OpenDocument(bool openInteration,string[] fileArgs)
        {
            try
            {
                bool IsOpenOk = false;
                string[] FileNamesOpenned = null;
                if (!openInteration)
                {
                    if (fileArgs.Length > 0)
                    {
                        IsOpenOk = true;
                        FileNamesOpenned = fileArgs;
                    }
                }
                else { 
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Multiselect = true;
                    openFileDialog.Filter = "Documentos (*.pdf,*.doc)|*.pdf;*.doc;*.docx";
                    if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                        IsOpenOk = true;
                        FileNamesOpenned = openFileDialog.FileNames;
                    }
                }
  
                if (IsOpenOk)
                {
                    #region Handle Passwords Encryption Document
                    // Begin handleEncryption
                    if (!Configurations.Documents_DisableCheckPassword)
                    {
                        if (FileNamesOpenned.Length == 1)
                        {
                            if (!checkDocPermission(FileNamesOpenned[0]))
                                return;
                        }
                        else
                        {
                            string tempSignatorDocTitle = this.Text;
                            this.Text = "SignatorDoc - Abrindo Documentos em Lote. Aguarde...";
                            if (!checkDocPermissionMassive(ref FileNamesOpenned))
                            {
                                this.Text = tempSignatorDocTitle;
                                return;
                            }
                            else
                                this.Text = tempSignatorDocTitle;
                        }
                    }
                    // End HandleEncryption
                    #endregion

                    if (this.pdfDocLoaded)
                    {
                        if (this.isDocChanged)
                        {
                            if (!this.isSavedDoc)
                            {
                                if (MessageBox.Show( (this.isMassiveSign? "Os documentos ainda não foram salvos, Gostaria de salvá-los agora?": "O documento ainda não foi salvo, Gostaria de salvá-lo agora?"), "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                                {
                                    if (this.isMassiveSign)
                                    {
                                        if (this.saveMassiveDocuments(false, true))
                                        {
                                            this.btn_SaveDocument.Enabled = false;
                                            this.Mnu_Item_Save.Enabled = false;
                                        }
                                    }else
                                        this.saveSimpleDocument(true,false);
                                }
                                else
                                {
                                    this.isDocChanged = false;
                                }
                            }
                        }
                        releasePDFViewerResources();

                        if (this.SigChartCheckStripMenuItem.Enabled)
                            this.SigChartCheckStripMenuItem.Enabled = false;

                        if (System.IO.File.Exists(tempFileName))
                            System.IO.File.Delete(tempFileName);
                        this.pdfDocLoaded = false;
                        
                        if(this.isMassiveSign){
                            foreach (string fname in Directory.GetFiles(this.localTempPath))
                                File.Delete(fname);
                        }

                        this.docSigner.clearSignedPathFile();
                    }

                    this.isPDFNetBug = false;

                    if (FileNamesOpenned.Length > 1)
                    {

                        if (!this.rdo_SignMode.Checked)
                            this.rdo_SignMode.Checked = true;

                        this.Text = "SignatorDoc - Assinatura de Documentos em Lote.";
                        byte[] pdfData = global::SignatorDocSolution.Properties.Resources.GuidePage;
                        this.PDFDocument = new PDFDoc(pdfData, pdfData.Length);
                        Page page = this.PDFDocument.GetPage(1);
                        Defins.setPagebaseSize(new Size((int)((page.GetPageWidth() / 72) * Defins.pagesDPI), (int)((page.GetPageHeight() / 72) * Defins.pagesDPI)));
                        this.CountPages = 1;
                        this.pdfPages = new PictureBox[1];
                        this.pdfPages[0] = createPageView();
                        this.setPageViewContent(0);
                        this.pnl_PDFPageViewer.Controls.Add(this.pdfPages[0]);
                        this.txt_PDFNavCurrPage.Text = this.currentPageNumber.ToString() + " / " + this.CountPages;
                        this.pdfDocLoaded = true;
                        this.btn_AddSignatureField.Enabled = true;
                        this.btn_ReviewMassiveSignDocsFields.Visible = true;
                        this.isMassiveSign = true;
                        this.setFieldSearchPatternToolStripMenuItem.Enabled = true;
                        this.frm_SetSigStringPattern.setIsMassiveDocSearchPattern(true);
                        List<string> listOnlyFileName = new List<string>();

                        string docTempPath = null;
                        if (this.isAdbeReaderInstalled)
                            this.rdo_AdobeMode.Enabled = false;
                        string auxFileNamewotExt = null;
                        int extensioncode = 0;
                        string extension = null;
                        int countDocSignatorDoced = 1;

                        foreach (string sFileName in FileNamesOpenned)
                        {
                            auxFileNamewotExt = Path.GetFileNameWithoutExtension(sFileName);
                            if (!listOnlyFileName.Contains(auxFileNamewotExt))
                            {
                                this.Text = "SignatorDoc - Abrindo Lote de Documentos. Aguarde... ( " + countDocSignatorDoced + " ) ";

                                docTempPath = this.localTempPath + @"\" + auxFileNamewotExt;
                                extension = Path.GetExtension(sFileName);
                                extensioncode = extension.Equals(".pdf") ? 0 : (extension.Equals(".doc") ? 1 : 2);

                                if (extensioncode == 0)
                                    File.WriteAllBytes(docTempPath, File.ReadAllBytes(sFileName));
                                else
                                {
                                    if (!this.convertToPDF(sFileName, docTempPath, extensioncode))
                                    {
                                        continue;
                                    }
                                }

                                this.massiveFileNames.Add((Path.GetDirectoryName(sFileName) + @"\" + auxFileNamewotExt) + ".pdf");
                                listOnlyFileName.Add(auxFileNamewotExt);
                                countDocSignatorDoced++;
                            }
                        }

                        {
                            if (ConverterDocFormats.listErrorDocs != null)
                            {
                                if (ConverterDocFormats.listErrorDocs.Contains("80040154") || ConverterDocFormats.listErrorDocs.Contains("está instalado"))
                                    MessageBox.Show("Um suplemento Microsoft Office está faltando. Instale o suplemento \"Microsoft Office Salvar Como\" para converter formatos DOC para PDF.", "Abertura de Documentos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                else
                                    MessageBox.Show(ConverterDocFormats.listErrorDocs, "Abertura de Documentos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            ConverterDocFormats.listErrorDocs = null;
                        }

                        if (this.propertiesToolStripMenuItem.Enabled)
                            this.propertiesToolStripMenuItem.Enabled = false;

                        this.Text = "SignatorDoc - Assinatura de Documentos em Lote (" + listOnlyFileName.Count + ")";
                        this.frm_SetSigStringPattern.setListDocuments(this.localTempPath + @"\", listOnlyFileName, this.listDocumentName);
                        this.frm_DefineSigFieldsMassiveDocs.setListBoxDocuments(listOnlyFileName.ToArray(),this.listDocumentName);
                        this.frm_DefineSigFieldsMassiveDocs.setTempFilesLocation(this.localTempPath);

                    }
                    else{
                        string openFileNameAux = FileNamesOpenned[0];
                        string extension = Path.GetExtension(openFileNameAux);
                        int extencionCode = extension.Equals(".pdf") ? 0 : ( extension.Equals(".doc") ? 1 : 2 ) ;
                        
                        this.Text = "SignatorDoc - [ " + Path.GetFileNameWithoutExtension(openFileNameAux)+" ]";

                        if (extencionCode == 0)
                            this.tempFileName = this.localTempPath + @"\" + System.IO.Path.GetFileName(openFileNameAux);
                        else
                            this.tempFileName = this.localTempPath + @"\" + System.IO.Path.GetFileNameWithoutExtension(openFileNameAux) + ".pdf";

                        if (System.IO.File.Exists(tempFileName))
                            System.IO.File.Delete(tempFileName);

                        if (extencionCode == 0)
                        {
                            System.IO.File.Copy(openFileNameAux, tempFileName);
                            this.FileName = openFileNameAux;
                        }
                        else
                        {
                            if (!this.convertToPDF(openFileNameAux, tempFileName, extencionCode))
                            {
                                if (ConverterDocFormats.listErrorDocs.Contains("80040154") || ConverterDocFormats.listErrorDocs.Contains("está instalado"))
                                    MessageBox.Show("Um suplemento Microsoft Office está faltando. Instale o suplemento \"Microsoft Office Salvar Como\" para converter formatos DOC para PDF.", "Abertura de Documentos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                else
                                    MessageBox.Show(ConverterDocFormats.listErrorDocs, "Abertura de Documentos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                ConverterDocFormats.listErrorDocs=null;
                                releasePDFViewerResources();
                                this.OpenDocument(true,null);
                                return;
                            }
                            this.FileName = Path.GetDirectoryName(openFileNameAux) + @"\" + Path.GetFileNameWithoutExtension(openFileNameAux) + ".pdf";
                        }

                        this.SigChartCheckStripMenuItem.Enabled = this.frm_SignatureChartCompare.checkDocIsSigned(this.tempFileName, this.isPassNeedOpenDoc, this.isPassNeedOpenDoc ? this.listDocumentName[0].passwdToOpen : null); // Changed in HandleEncryption
                        bool isFailLastOp = AweSomeUtils.checkWhetherFailLastOperation();
                        
                        if (rdo_AdobeMode.Checked)
                        {
                            ShowDocumentAdobeMode(this.tempFileName, 0);  
                        }
                        else
                        {

                            if (isFailLastOp)
                            {
                                Application.Restart();
                                Application.ExitThread();
                                return;
                            }

                            int pdfdoc_page_count = 0;
                            try
                            {
                                this.PDFDocument = new PDFDoc(this.tempFileName);
                                if (this.isPassNeedOpenDoc)
                                    this.PDFDocument.InitStdSecurityHandler(this.listDocumentName[0].passwdToOpen);

                                pdfdoc_page_count = this.PDFDocument.GetPageCount();
                            }
                            catch(Exception exc){
                                this.isPDFNetBug = true;
                            }

                            if (!this.isPDFNetBug)
                            {
                                this.frm_ReplicSigField.setPageNumberQts(this.PDFDocument.GetPageCount());
                                this.ShowDocumentSignMode();
                            }
                            else 
                            {
                                if (this.PDFDocument != null)
                                {
                                    this.PDFDocument.Close();
                                    this.PDFDocument = null;
                                }
                                this.ShowDocumentSignModeGhostScript();
                            }                            
                        }
                        this.isMassiveSign = false;
                        this.frm_SetSigStringPattern.setIsMassiveDocSearchPattern(false);
                        if(this.isAdbeReaderInstalled)
                            this.rdo_AdobeMode.Enabled = true;

                        if (this.btn_ReviewMassiveSignDocsFields.Visible)
                            this.btn_ReviewMassiveSignDocsFields.Visible = false;

                    }
                }
            }
            catch (IOException expIO)
            {
                if (expIO.Message.Contains("because it is being used by another process") || expIO.Message.Contains("porque ele está sendo usado por outro processo"))
                {
                    MessageBox.Show("O SignatorDoc não pode abrir um ou mais documentos, porque um ou mais documentos já estão sendo utilizados por outro programa.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Um ou mais documentos não podem ser abertos! O arquivo está danificado e não pode ser restaurado.\n\n" + expIO.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception exc)
            {
                if (exc.Message.Contains("@pdftron"))
                {
                    MessageBox.Show("Um recurso necessário para abrir o documento em modo de assinatura não foi inicializado corretamente. Caso o erro persista entre em contato com o fornecedor do sistema. Erro: 0xEV01", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.releasePDFViewerResources();
                    this.Dispose();
                }
                else
                    MessageBox.Show("Um ou mais documentos não podem ser abertos! O arquivo está danificado e não pode ser restaurado.\n\n" + exc.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void ShowDocumentAdobeMode(string fileNamePath, int page)
        {
            if (this.propertiesToolStripMenuItem.Enabled)
                this.propertiesToolStripMenuItem.Enabled = false;

            this.showOrHideLoadingPanel(true);
            this.axAcroPDF.LoadFile(fileNamePath);
            this.axAcroPDF.setShowToolbar(true);
            this.axAcroPDF.setPageMode("thumbs");
            this.pdfDocLoaded = true;
            if (page > 0)
                this.axAcroPDF.setCurrentPage(page);
            this.tmrAfterLoadingPanel.Start();
        }

        private bool saveSimpleDocument(bool isSaveAs,bool isFlag_1)
        {
            try
            {
                if (!this.pdfDocLoaded)
                    return false;

                if (this.isDocChanged)
                {
                    if (isSaveAs)
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";
                        saveFileDialog.FileName = this.FileName;
                        if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            System.IO.File.WriteAllBytes(saveFileDialog.FileName, System.IO.File.ReadAllBytes(this.docSigner.getSignedFilePath()));
                            if (isFlag_1)
                            {
                                this.FileName = saveFileDialog.FileName;
                                this.Text = "SignatorDoc - [ " + Path.GetFileNameWithoutExtension(this.FileName) +" ]";
                            }
                            this.isSavedDoc = true;
                            this.isDocChanged = false;
                            MessageBox.Show("O documento foi salvo com sucesso!", "Salvar Documento", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.btn_SaveDocument.Enabled = false;
                            this.Mnu_Item_Save.Enabled = false;
                            return true;
                        }
                        else
                        {
                            if (!isFlag_1)
                            {
                                this.isSavedDoc = false;
                                this.isDocChanged = false;
                                this.btn_SaveDocument.Enabled = false;
                                this.Mnu_Item_Save.Enabled = false;
                                MessageBox.Show("O documento não foi salvo! O usuário cancelou a operação", "Salvar Documento", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            } 
                            return false;
                        }
                    }
                    else
                    {
                        if (MessageBox.Show("Manter o documento assinado no mesmo local?", "Salvar Documento", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                        {
                            System.IO.File.WriteAllBytes(this.FileName, System.IO.File.ReadAllBytes(this.docSigner.getSignedFilePath()));
                            this.isSavedDoc = true;
                            this.isDocChanged = false;
                            MessageBox.Show("O documento foi salvo com sucesso!", "Salvar Documento", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.btn_SaveDocument.Enabled = false;
                            this.Mnu_Item_Save.Enabled = false;
                            return true;
                        }
                        else
                        {
                            SaveFileDialog saveFileDialog = new SaveFileDialog();
                            saveFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";
                            saveFileDialog.FileName = this.FileName;
                            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                System.IO.File.WriteAllBytes(saveFileDialog.FileName, System.IO.File.ReadAllBytes(this.docSigner.getSignedFilePath()));
                                this.FileName = saveFileDialog.FileName;
                                this.Text = "SignatorDoc - [ " + Path.GetFileNameWithoutExtension(this.FileName)+" ]";
                                this.isSavedDoc = true;
                                this.isDocChanged = false;
                                MessageBox.Show("O documento foi salvo com sucesso!", "Salvar Documento", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.btn_SaveDocument.Enabled = false;
                                this.Mnu_Item_Save.Enabled = false;
                                return true;
                            }
                            else{
                                return false;
                            }
                        }

                    }

                }
                else {
                    if (isSaveAs)
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";
                        saveFileDialog.FileName = this.FileName;
                        if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            string auxFileName = this.isDocChanged ? this.docSigner.getSignedFilePath() : this.tempFileName;
                            System.IO.File.WriteAllBytes(saveFileDialog.FileName, System.IO.File.ReadAllBytes(auxFileName));
                            this.FileName = saveFileDialog.FileName;
                            this.Text = "SignatorDoc - [ " + Path.GetFileNameWithoutExtension(this.FileName)+" ]";
                            this.isSavedDoc = true;
                            this.isDocChanged = false;
                            MessageBox.Show("O documento foi salvo com sucesso!", "Salvar Documento", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.btn_SaveDocument.Enabled = false;
                            this.Mnu_Item_Save.Enabled = false;
                            return true;
                        }
                        else
                            return false;
                    }
                }
            }
            catch (IOException expIO)
            {
                if (expIO.Message.Contains("because it is being used by another process") || expIO.Message.Contains("porque ele está sendo usado por outro processo"))
                {
                    MessageBox.Show("O SignatorDoc não pode salvar este documento, porque ele já está sendo utilizado por outro programa.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("O documento não pode ser salvo: " + expIO.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("O documento não pode ser salvo: " + exc.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }

        private bool saveMassiveDocuments(bool isSaveAs, bool isFlag_1)
        {
            try
            {
                if (!this.pdfDocLoaded)
                    return false;

                if (this.isDocChanged) {
                    bool docListSaved = false;
                    if (isSaveAs)
                    {
                        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                        folderBrowserDialog.RootFolder = Environment.SpecialFolder.Desktop;
                        folderBrowserDialog.ShowNewFolderButton = true;
                        folderBrowserDialog.Description = "Selecione o diretório onde irá salvar o lote de documentos.";
                        if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            string folderPath = folderBrowserDialog.SelectedPath + @"\";
                            string auxFileName;
                            string auxSourceFileName;
                            List<PDFDocInfo> distinctList = HandleStructs.RemoveDocInfoDuplicateList(this.PDFDocInfoList);
                            foreach (PDFDocInfo docInfo in distinctList)
                            {
                                auxFileName = folderPath + docInfo.documentName + ".pdf";
                                auxSourceFileName = this.localTempPath + @"\" + docInfo.documentName;
                                if (File.Exists(auxFileName))
                                {
                                    if (MessageBox.Show("Já existe um arquivo com o nome " + docInfo.documentName + ".pdf no diretório selecionado. Gostaria de substituí-lo?", "Salvar Documento", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                                    {
                                        System.IO.File.WriteAllBytes(auxFileName, System.IO.File.ReadAllBytes(auxSourceFileName));
                                    }
                                    else
                                    {
                                        using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                                        {
                                            saveFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";
                                            saveFileDialog.FileName = folderPath + docInfo.documentName + " (SignedBySignatorDoc)" + ".pdf";
                                            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                            {
                                                System.IO.File.WriteAllBytes(saveFileDialog.FileName, System.IO.File.ReadAllBytes(auxSourceFileName));
                                            }
                                            else
                                            {
                                                System.IO.File.WriteAllBytes(saveFileDialog.FileName, System.IO.File.ReadAllBytes(auxSourceFileName));
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    System.IO.File.WriteAllBytes(auxFileName, System.IO.File.ReadAllBytes(auxSourceFileName));
                                }
                            }
                            docListSaved = true;
                        }
                        else {
                            if (isFlag_1)
                            {
                                this.isSavedDoc = false;
                                this.isDocChanged = false;
                                this.btn_SaveDocument.Enabled = false;
                                this.Mnu_Item_Save.Enabled = false;
                                this.PDFDocInfoList.Clear();
                                MessageBox.Show("Os documentos não foram salvos! O usuário cancelou a operação.", "Salvar Documentos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    else {
                        if (MessageBox.Show("Manter os documentos assinados no mesmo local?", "Salvar Documentos", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                        {
                            string tempSourceDoc;
                            string tempName;
                            List<PDFDocInfo> distinctList = HandleStructs.RemoveDocInfoDuplicateList(this.PDFDocInfoList);

                            foreach (string fn in this.massiveFileNames)
                            {
                                tempName = Path.GetFileNameWithoutExtension(fn);
                                PDFDocInfo pdfDocInfo = distinctList.Find(docinfo => docinfo.documentName.Equals(tempName));
                                if (pdfDocInfo != null) { 
                                    tempSourceDoc= this.localTempPath + @"\"+ pdfDocInfo.documentName;
                                    File.WriteAllBytes(fn, File.ReadAllBytes(tempSourceDoc));
                                }
                            }
                        }
                        else {
                            return this.saveMassiveDocuments(true, isFlag_1);
                        }
                        docListSaved = true;
                    }

                    if (docListSaved)
                    {
                        this.isSavedDoc = true;
                        this.isDocChanged = false;
                        this.PDFDocInfoList.Clear();
                        MessageBox.Show("Todos os documentos assinados foram salvos com sucesso.", "Salvar Documentos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return true;
                    }
                }
            }
            catch (IOException expIO)
            {
                if (expIO.Message.Contains("because it is being used by another process") || expIO.Message.Contains("porque ele está sendo usado por outro processo"))
                {
                    MessageBox.Show("O SignatorDoc não pode salvar o lote de documentos, porque um ou mais documentos estão sendo utilizados por outro programa.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Um ou mais documentos não podem ser salvos: " + expIO.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Um ou mais documentos não podem ser salvos: " + exc.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }
  
        private void createEmptyPDFFile()
        {
            if (!File.Exists(this.localTempPath + @"\EmptyPDF.pdf"))
                File.WriteAllBytes(this.localTempPath + @"\EmptyPDF.pdf", global::SignatorDocSolution.Properties.Resources.EmptyPDF);
        }

        private void showOrHideLoadingPanel(bool isShow) {
            if (isShow)
            {
                this.loadingPanel.Location = new System.Drawing.Point((this.Size.Width / 2) - (this.loadingPanel.Size.Width / 2), (this.Size.Height / 2) - (this.loadingPanel.Size.Height / 2));
                this.loadingPanel.Focus();
                this.loadingPanel.BringToFront();
                this.tpnl_PDFMain.Enabled = false;
                this.loadingPanel.Visible = true;
                this.MaximizeBox = false;
            }
            else {
                this.loadingPanel.Visible = false;
                this.tpnl_PDFMain.Enabled = true;
                this.MaximizeBox = true;
            }
        }

        public void setCurrentSigFieldIndex(object obj_sigField) {
            this.SigContainerCurrentIndex = SignatureFieldCollection.IndexOf((SignatureField)obj_sigField);
        }

        private void ReplicationSignatureFieldSimple()
        {
            object[] objFields = new object[this.SignatureFieldCollection.Count];

            string fieldCodLocationName = null;

            for (int i = 0; i < objFields.Length; i++)
            {
                fieldCodLocationName = this.SignatureFieldCollection[i].Location.X + " H  " + this.SignatureFieldCollection[i].Location.Y + " V ";

                objFields[i] = "Campo " + fieldCodLocationName + "   -   Página " + this.SignatureFieldCollection[i].getPageNumber();
            }
            this.frm_ReplicSigField.setSigFields(objFields);
            this.frm_ReplicSigField.ShowDialog();
        }

        private void confirmFrmSearchPatternSimple() { 
            try
            {
                List<PDFDocInfo> listPDFDocInfo = this.frm_SetSigStringPattern.getSelectedFieldsInfo();
                bool isDevicePresent = false;
                bool isSigFieldPreConfigChecked = this.frm_SetSigStringPattern.getIsPreConfigSigField();
                bool isSigFieldConfigured = this.frm_SetSigStringPattern.getIsSigFieldConfigured();
                SignatureField auxSignatureField = null;

                if (listPDFDocInfo.Count > 0)
                {
                    if (DetectDevice.thereIsDeviceAvailable())
                            isDevicePresent = true;
                }

                try
                {
                    auxSignatureField = this.frm_SetSigStringPattern.getSetPreSigFieldConfig_SignatureField();
                    string signerName = auxSignatureField.frm_SignConfig.getSignerName();
                    int selectedCertIndex = auxSignatureField.frm_SignConfig.getCboCertificateIndex();
                            
                    if(!string.IsNullOrEmpty(signerName)){
                        this.frm_SignConfig.setExternalSignerName(signerName);
                        this.docSigner.setSignerName(this.frm_SignConfig.getSignerName());
                    }
                    if (selectedCertIndex > 0)
                    {
                        this.frm_SignConfig.setExternalCboCertificateIndex(selectedCertIndex);
                        this.docSigner.setX509Certificate2(this.frm_SignConfig.getSelectedCertificate());
                    }
                    
                    foreach (PDFDocInfo pdfdocInfo in listPDFDocInfo)
                    {
                        SigContainerCount++;
                        if (SigContainerCount < 50)
                        {
                            this.SignatureFieldCollection.Add(new SignatureField("sig_" + SigContainerCount + "_page_" + pdfdocInfo.SigStringPage, new System.Drawing.Point((int)pdfdocInfo.SigStringPosition.X, (int)pdfdocInfo.SigStringPosition.Y), pdfdocInfo.SigStringPage - 1, isDevicePresent, new EventHandler(SignatureFieldClose_Click), new EventHandler(SignatureFieldConfig_Click), new SignatorDocSolution.Utils.CustomEvent.GenericDelegateEventHandler(SignatureField_AfterSign), this.tlTip_Main));
                            this.SignatureFieldCollection[SigContainerCount].KeyDownEvent += new KeyEventHandler(SignatureField_KeyDown);

                            SignatureFieldCollection[SigContainerCount].deviceSerieNumber = auxSignatureField.deviceSerieNumber;
                            if (SignatureFieldCollection[SigContainerCount].deviceSerieNumber == 1)
                            {
                                SignatureFieldCollection[SigContainerCount].setExtenalEventDeviceScreenClick(new CustomEvent.GenericDelegateEventHandler(afterDeviceScreenClick));
                            }

                            if (this.currentPageIndex == (pdfdocInfo.SigStringPage - 1))
                            {
                                this.pnl_PDFPageViewer.Controls[0].Controls.Add(SignatureFieldCollection[SigContainerCount]);
                                this.pnl_PDFPageViewer.Controls[0].Controls[(this.pnl_PDFPageViewer.Controls[0].Controls.Count - 1)].BringToFront();
                            }
                            else
                            {
                                this.pdfPages[(pdfdocInfo.SigStringPage - 1)].Controls.Add(SignatureFieldCollection[SigContainerCount]);
                                this.pdfPages[(pdfdocInfo.SigStringPage - 1)].Controls[(this.pdfPages[(pdfdocInfo.SigStringPage - 1)].Controls.Count - 1)].BringToFront();
                            }
                            if (isSigFieldPreConfigChecked && isSigFieldConfigured)
                            {
                                SignatureFieldCollection[SigContainerCount].StampIndex = auxSignatureField.StampIndex;
                                SignatureFieldCollection[SigContainerCount].setIsHandWrittenDone(auxSignatureField.getIsHandWrittenDone());
                                SignatureFieldCollection[SigContainerCount].setAlreadyToSign(auxSignatureField.getAlreadyToSign());
                                SignatureFieldCollection[SigContainerCount].setSignatureFieldOptions(auxSignatureField.SigFieldSealType);
                                SignatureFieldCollection[SigContainerCount].BackgroundImage = auxSignatureField.BackgroundImage;
                                SignatureFieldCollection[SigContainerCount].Image = auxSignatureField.Image;
                                SignatureFieldCollection[SigContainerCount].setDataBioSig(auxSignatureField.getDataBioSig());
                                SignatureFieldCollection[SigContainerCount].stampHasBeenSelected = auxSignatureField.stampHasBeenSelected;
                                SignatureFieldCollection[SigContainerCount].tempHandWrittenImage = auxSignatureField.tempHandWrittenImage;
                                SignatureFieldCollection[SigContainerCount].tempStampImage = auxSignatureField.tempStampImage;
                                if (SignatureFieldCollection[SigContainerCount].getIsHandWrittenDone())
                                    SignatureFieldCollection[SigContainerCount].Show_btn_SigFieldRepeat(true);
                            }
                            else
                            {
                                if (!isDevicePresent)
                                    SignatureFieldCollection[SigContainerCount].StampIndex = 2;
                            }
                            this.SigContainerCurrentIndex = this.SigContainerCount;
                            
                        }
                    }
                }
                catch (Exception exc)
                {
                    MessageBox.Show("O recurso solicitado está ocupado.\n" + exc.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (listPDFDocInfo.Count > 0)
                {
                    if (!isDevicePresent)
                        if (!this.btn_SealSignature.Enabled)
                            this.btn_SealSignature.Enabled = true;
                    if (!this.btn_DoubleSignatureField.Enabled)
                        this.btn_DoubleSignatureField.Enabled = true;
                    if (!this.btn_ReplicationSignatureFields.Enabled)
                        this.btn_ReplicationSignatureFields.Enabled = true;
                    if (!this.btn_RemoveSignatureFields.Enabled)
                        this.btn_RemoveSignatureFields.Enabled = true;

                    this.isFieldPatternAdded = true;
                    this.SignatureFieldCollection[SigContainerCount].Focus();

                    if ((!this.btn_SealSignature.Enabled) && this.SignatureFieldCollection[SigContainerCount].getAlreadyToSign())
                        this.btn_SealSignature.Enabled = true;
                }
                this.frm_SetSigStringPattern.closeForm();
            }
            catch (Exception ex) {
                MessageBox.Show("Não foi possível inserir um ou mais campos de assinatura. Um ou mais locais de assinatura encontrados podem estar corrompidos." + ex.Message, "Erro",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void confirmFrmSearchPatternMassive() {
            try
            {
                this.frm_SetSigStringPattern.isAlreadyConfimed = true;
                bool isSigFieldConfigured = this.frm_SetSigStringPattern.getIsSigFieldAlredyPreConfigured();
                if (!isSigFieldConfigured) {
                    this.frm_SetSigStringPattern.setIsSigFieldAlredyPreConfigured(true);
                    return;
                }

                List<PDFDocInfo> listPDFDocInfo = this.frm_SetSigStringPattern.getSelectedFieldsInfo();
                SignatureField sigField= this.frm_SetSigStringPattern.getSetPreSigFieldConfig_SignatureField();

                string signerName = sigField.frm_SignConfig.getSignerName();
                int selectedCertIndex = sigField.frm_SignConfig.getCboCertificateIndex();
                            
                if(!string.IsNullOrEmpty(signerName)){
                    this.frm_SignConfig.setExternalSignerName(signerName);
                    this.docSigner.setSignerName(this.frm_SignConfig.getSignerName());
                }
                if (selectedCertIndex > 0)
                {
                    this.frm_SignConfig.setExternalCboCertificateIndex(selectedCertIndex);
                    this.docSigner.setX509Certificate2(this.frm_SignConfig.getSelectedCertificate());
                }

                PDFDocInfo tempAuxdocinfo = null; // Handle Encryption
                foreach (PDFDocInfo pdfdocinfo in listPDFDocInfo) {

                    #region Handle Passwords Encryption Document
                    // Begin handleEncryption
                    if (this.isPassNeedEditDoc)
                    {
                        tempAuxdocinfo = this.listDocumentName.Find(doc => doc.documentName == pdfdocinfo.documentName);
                        if (tempAuxdocinfo != null)
                        {
                            pdfdocinfo.isNeedPassToEdit = tempAuxdocinfo.isNeedPassToEdit;
                            pdfdocinfo.passwdToEdit = tempAuxdocinfo.passwdToEdit;
                        }
                    }
                    // End HandleEncryption
                    #endregion

                    pdfdocinfo.fieldImage = sigField.Image;
                    pdfdocinfo.FieldSealType = sigField.SigFieldSealType;
                    pdfdocinfo.bioDataSignature = sigField.getDataBioSig();
                    pdfdocinfo.fieldAlreadyToSign = sigField.getAlreadyToSign();
                    this.PDFDocInfoList.Add(pdfdocinfo);
                }
                if (this.PDFDocInfoList.Count > 0)
                {
                    if (!this.btn_SealSignature.Enabled)
                        this.btn_SealSignature.Enabled = true;
                    if (!this.btn_ReviewMassiveSignDocsFields.Enabled)
                        this.btn_ReviewMassiveSignDocsFields.Enabled = true; ;
                }
            }
            catch (Exception ex) { 
                throw new System.Exception("Não foi possível continuar com o processo, uma ou mais campos de assinatura podem estar corrompidos. Erro:" + ex.Message);
            }

            this.frm_SetSigStringPattern.closeForm();
        }

        private void signSimpleDocument() {
            if (this.SigContainerCount >= 0)
            {
                string msgSignConfirm = this.SigContainerCount == 0 ? "Foi criado 1 campo de assinatura. Clique em [SIM] para fixá-lo ao documento." :
                                                                     "Foram criados " + (this.SigContainerCount + 1) + " campos de assinatura. Clique em [SIM] para fixá-los ao documento.";
                if (MessageBox.Show(msgSignConfirm, "Confirmação de Assinatura", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    this.showOrHideLoadingPanel(true);

                    int SignedCount = 0;

                    bool isFailLastOP = false;
                    if (backGroungWorker_SignDoc == null)
                    {
                        int CountSignedDone=0;
                        backGroungWorker_SignDoc = new BackgroundWorker();
                        backGroungWorker_SignDoc.WorkerReportsProgress = true;
                        backGroungWorker_SignDoc.WorkerSupportsCancellation = true;
                        backGroungWorker_SignDoc.DoWork += (sw, ew) =>
                        {
                            try
                            {

                                int errorCountSigned = 0;
                                this.isSignSuccess = false;
                                string passToSign = null; // Handle Encryption
                                this.PDFDocument.Close();
                                foreach (SignatureField sf in this.SignatureFieldCollection)
                                {
                                    if (sf.getAlreadyToSign())
                                    {
                                        passToSign = (this.isPassNeedOpenDoc || this.isPassNeedEditDoc) ? (this.isPassNeedEditDoc ? this.listDocumentName[0].passwdToEdit : this.listDocumentName[0].passwdToOpen) : null; // Handle Encryption
                                        if (this.docSigner.SignDocument(this.localTempPath, this.tempFileName, sf, Defins.pagesDPI, passToSign)) // Change Handle Encryption
                                            CountSignedDone++;
                                        else
                                        {
                                            isFailLastOP = AweSomeUtils.checkWhetherFailLastOperation();
                                            if (isFailLastOP)
                                            {
                                                this.isSignSuccess = false;
                                                return;
                                            }
                                            errorCountSigned++;
                                            if (this.docSigner.cancelOperation)
                                                break;
                                        }
                                           
                                    }
                                    else {
                                        SignedCount++;
                                        continue;
                                    }
                                    
                                }

                                this.docSigner.clearExceptions();

                                if (errorCountSigned > 0)
                                {
                                    this.frm_SignConfig.resetCertificateChoosed();
                                    return;
                                }
                                this.isDocChanged = true;
                                this.isSavedDoc = false;

                                this.isSignSuccess = true;

                            }
                            catch (Exception exc)
                            {
                                this.isDocChanged = false;
                                MessageBox.Show("Não foi possível fixar uma ou mais assinaturas no documento. A seguinte mensagem de exceção foi gerada: " + exc.Message, "Assinatura de Documento", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                this.docSigner.clearSignerContext();
                            }
                        };

                        backGroungWorker_SignDoc.RunWorkerCompleted += (sw, ew) =>
                        {
                            if (isSignSuccess)
                            {
                                this.showOrHideLoadingPanel(false);
                                if (SignedCount > 0)
                                {
                                    string msgSigFieldNot = SignedCount == 1 ? "1 campo de assinatura não foi fixado ao documento. Este campo não está assinado ou não possuí um carimbo válido!" :
                                                                              SignedCount + " campos de assinatura não foram fixados ao documento. Estes campos não estão assinados ou nao possuem um carimbo válido!";

                                    MessageBox.Show(msgSigFieldNot, "Assinatura", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                }
                                else
                                    MessageBox.Show("Documento Assinado com Sucesso.", "Assinatura", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                
                                if(CountSignedDone>0)
                                    this.reloadDocument();
                                CountSignedDone = 0;
                                SignedCount = 0;
                            }
                            else
                            {
                                this.showOrHideLoadingPanel(false);
                                if (isFailLastOP)
                                {
                                    MessageBox.Show("Este documento está protegido com senha contra alteração.\nPara habilitar o uso de senhas em documentos protegidos, acesse o menu ARQUIVO => CONFIGURAÇÕES e desmarque a opção \"Desabilitar Verificação de Senha de Proteção do Documento\".\n\nA ativação da verificação de senha pode deixar a abertura de documentos um pouco lenta.", "Abertura de Documento", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                                    Application.Restart();
                                    Application.ExitThread();
                                }
                                else
                                {
                                    MessageBox.Show("Um erro ocorrido no processo de assinatura, poderá gerar instabilidade em umas das propriedades da instância atual do programa. É recomendável reiniciar o programa para realocar os recursos necessários.", "Assinatura de Documento", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }                            
                             }
                        };

                    }

                    backGroungWorker_SignDoc.RunWorkerAsync();

                }
            }
        }

        private void signMassiveDocuments() {
            try
            {
                int countDocuments = this.PDFDocInfoList.Count;
                string msgSignConfirm = countDocuments == 1 ? "Foi criado 1 campo de assinatura. Clique em [SIM] para fixá-la ao documento." :
                                                                    "Foram criados " + countDocuments + " campos de assinatura. Clique em [SIM] para fixá-las aos documentos.";
                if (MessageBox.Show(msgSignConfirm, "Confirmação de Assinatura", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    if (backGroungWorker_SignDocMassive == null)
                    {
                        this.showOrHideLoadingPanel(true);
                        bool errorHappened = false;
                        backGroungWorker_SignDocMassive = new BackgroundWorker();
                        backGroungWorker_SignDocMassive.WorkerReportsProgress = true;
                        backGroungWorker_SignDocMassive.WorkerSupportsCancellation = true;

                        backGroungWorker_SignDocMassive.DoWork += (sw, ew) =>
                        {
                            errorHappened = false;
                            if (!this.docSigner.SignDocumentsMassive(this.localTempPath, this.PDFDocInfoList))
                            {
                                errorHappened = true;
                            }
                        };

                        backGroungWorker_SignDocMassive.RunWorkerCompleted += (sw, ew) =>
                        {
                            this.showOrHideLoadingPanel(false);
                            if (errorHappened)
                            {
                                //throw new System.Exception(this.docSigner.getMessageException());
                                if (this.docSigner.ListMassiveNotSigned.Count > 0)
                                {
                                    string documentserror = "";
                                    bool failNoPermissionDocs=false;
                                    foreach (KeyValuePair<string, string> kvp in this.docSigner.ListMassiveNotSigned)
                                    {
                                        documentserror += "[ " + kvp.Key + " ] ; ";
                                        if(kvp.Value.Equals("NoPermission") && (!failNoPermissionDocs))
                                            failNoPermissionDocs=true;
                                    }
                                    if (failNoPermissionDocs)
                                    {
                                        MessageBox.Show("Os documentos: " + documentserror + " não foram assinados porque estão protegidos com uma senha definida pelo criador do documento.\n\nPara habilitar o uso de senha em documentos protegidos, acesse o menu ARQUIVO => CONFIGURAÇÔES \"Desabilitar Verificação de Senha de Proteção do Documento\".\n\nA ativação da verificação de senha pode deixar a abertura de documentos um pouco lenta.", "Assinatura de Documentos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        this.isDocChanged = true;
                                        this.isSavedDoc = false;
                                        this.releaseMassiveDocsConfigs();
                                    }
                                    else
                                    {
                                        MessageBox.Show("Um ou mais documentos não puderam ser assinados, uma falha pode ter occorrido no conjunto de regras Postscript de um ou mais documentos.", "Assinatura de Documentos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        Application.Restart();
                                        Application.ExitThread();
                                        return;
                                    }
                                    
                                }
                                else
                                    MessageBox.Show("Não foi possível fixar uma ou mais assinatura em um ou mais documentos. A seguinte mensagem de exceção foi gerada: " + this.docSigner.getMessageException(), "Assinatura de Documentos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                this.isDocChanged = true;
                                this.isSavedDoc = false;
                                MessageBox.Show("Todos os documentos foram Assinados com sucesso!", "Assinatura de Documentos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.releaseMassiveDocsConfigs();
                            }
                        };

                    }
                    else
                    {
                        this.showOrHideLoadingPanel(true);
                    }

                    backGroungWorker_SignDocMassive.RunWorkerAsync();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Não foi possível fixar uma ou mais assinatura em um ou mais documentos. A seguinte mensagem de exceção foi gerada: " + exc.Message, "Assinatura de Documentos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.releaseMassiveDocsConfigs();
            }
        
        }

        private bool convertToPDF(string fromfile, string tofile, int extencionCode) {

            if (ConverterDocFormats.ConvertWordToPdf(fromfile, tofile))
                return true;
            else
                return false;
        }


        #region Handle Passwords Encryption Document
        // Begin HandleEncription Password
        
        private bool checkDocPermission(string fileName)
        {
            string extension = Path.GetExtension(fileName);
            List<PDFDocInfo> auxPDFDocInfoList = new List<PDFDocInfo>();
            bool isPassOpen = false;
            bool isPassEdit = false;
            bool passEditOk = false;

            if (extension.Equals(".pdf"))
            {
                #region checkdocproperties
                isPassOpen = AweSomeUtils.checkIsNeedPassOpenDoc(fileName);
                if (isPassOpen)
                {
                    string passToOpen = AweSomeUtils.getDocPassToOpen(fileName);
                    switch (passToOpen)
                    {
                        case "`\01´": { return false; }
                        case "`\02´": { MessageBox.Show("A senha digitada está incorreta!", "Abertura de Documento", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
                        default: { auxPDFDocInfoList.Add(new PDFDocInfo(Path.GetFileNameWithoutExtension(fileName), passToOpen, true)); break; }
                    }
                }

                int resCheckIsNeedPassEdit = AweSomeUtils.checkIsDocPassEditNeed(fileName, isPassOpen, ((isPassOpen && auxPDFDocInfoList.Count > 0) ? auxPDFDocInfoList[0].passwdToOpen : null));
                if (resCheckIsNeedPassEdit > 1)
                {
                    MessageBox.Show("Não é possível abrir o documento. Ocorreu uma exceção ao tentar desbloqueá-lo com a senha digitada!", "Abertura de Documento", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                    isPassEdit = resCheckIsNeedPassEdit == 1;

                #endregion

                #region checkPassNeedToEdit
                if (!rdo_AdobeMode.Checked)
                {
                    if (isPassEdit)
                    {
                        string passToEdit = AweSomeUtils.getDocPassToEdit(fileName);
                        switch (passToEdit)
                        {
                            case "`\01´": { return false; }
                            case "`\02´": { MessageBox.Show("A senha digitada está incorreta! Para abrir o documento em modo de assinatura é necessário digitar a senha correta. Se esqueceu a senha, você pode abrir o documento em modo de Visualização mas não poderá assiná-lo.", "Abertura de Documento", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
                            default:
                                {
                                    if (isPassOpen)
                                    {
                                        auxPDFDocInfoList[0].isNeedPassToEdit = true; ;
                                        auxPDFDocInfoList[0].passwdToEdit = passToEdit;
                                    }
                                    else
                                        auxPDFDocInfoList.Add(new PDFDocInfo(Path.GetFileNameWithoutExtension(fileName), passToEdit, false));
                                    passEditOk = true;
                                    break;
                                }
                        }
                    }
                }
                #endregion
            }

            this.isPassNeedOpenDoc = isPassOpen;
            this.isPassNeedEditDoc = isPassEdit;
            this.isPassEditDocOk = passEditOk;
            this.listDocumentName.Clear();
            if (auxPDFDocInfoList.Count > 0)
                this.listDocumentName = auxPDFDocInfoList;

            return true;
        }

        private bool checkDocPermissionMassive(ref string[] FileNames)
        {
            string extension = null;
            string passToEdit = null;
            bool isAtLeastOneIncluded = false;
            List<PDFDocInfo> auxPDFDocInfoList = new List<PDFDocInfo>();
            List<string> listFileNames = new List<string>(FileNames);

            foreach (string fileName in FileNames)
            {
                extension = Path.GetExtension(fileName);

                if (extension.Equals(".pdf"))
                {
                    try
                    {
                        passToEdit = AweSomeUtils.getIsDocPassGeneric(fileName);
                    }
                    catch(Exception exc){
                        if (exc.Message.Contains("Rebuild failed"))
                        {
                            listFileNames.Remove(fileName);
                            MessageBox.Show("O documento [" + System.IO.Path.GetFileNameWithoutExtension(fileName) + "] não pode ser incluido no lote! O arquivo está danificado e não pode ser recuperado.", "Abertura de documentos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            continue;
                        }
                        else
                            throw new System.Exception(exc.Message);
                    }

                    switch (passToEdit)
                    {
                        case "`\0´": { isAtLeastOneIncluded = true; break; }
                        case "`\01´": { MessageBox.Show("A senha de edição do documento \"" + System.IO.Path.GetFileNameWithoutExtension(fileName) + "\" é necessária para incluir assinaturas. Sem ela o documento não será incluído no lote.", "Abertura de Documentos", MessageBoxButtons.OK, MessageBoxIcon.Warning); listFileNames.Remove(fileName); break; }
                        case "`\02´": { MessageBox.Show("A senha digitada para o documento \"" + System.IO.Path.GetFileNameWithoutExtension(fileName) + "\" está incorreta! Este Documento não será incluído no lote.", "Abertura de Documentos", MessageBoxButtons.OK, MessageBoxIcon.Warning); listFileNames.Remove(fileName); break; }
                        default:
                            {
                                auxPDFDocInfoList.Add(new PDFDocInfo(Path.GetFileNameWithoutExtension(fileName), passToEdit, false));
                                isAtLeastOneIncluded = true;
                                break;
                            }
                    }

                }
                else
                    isAtLeastOneIncluded = true;
            }

            FileNames = listFileNames.ToArray();

            if (isAtLeastOneIncluded)
            {
                this.isPassNeedOpenDoc = false;
                this.isPassNeedEditDoc = false;
                this.isPassEditDocOk = false;
                this.listDocumentName.Clear();
                if (auxPDFDocInfoList.Count > 0)
                {
                    this.isPassNeedEditDoc = true;
                    this.listDocumentName = auxPDFDocInfoList;
                }
            }

            return isAtLeastOneIncluded;
        }

        // End HandleEncription Password
        #endregion

        #endregion


    }
}
