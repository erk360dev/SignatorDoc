using SignatorDocSolution.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace SignatorDocSolution
{
    public partial class Form_SetSigStringPattern : Form
    {
        private DocumentsHandle docsHandle = new DocumentsHandle();
        private BackgroundWorker backGroungWorker_SearchPattern = null;
        private List<string> listItems = new List<string>();
        private bool isMassiveSearch = false;
        private bool isSigFieldAlredyConfigured = false;
        private bool thereIsPatternFounded = false;
        public bool isAlreadyConfimed = false;
        private bool isFailLastOp = false;

        public Form_SetSigStringPattern()
        {
            InitializeComponent();
            this.configureBackGroundWorker();
        }

        public void configureBackGroundWorker() { 
            if (backGroungWorker_SearchPattern == null)
                {
                    backGroungWorker_SearchPattern = new BackgroundWorker();
                    backGroungWorker_SearchPattern.WorkerReportsProgress = true;
                    backGroungWorker_SearchPattern.WorkerSupportsCancellation = true;
                    backGroungWorker_SearchPattern.DoWork += (sw, ew) =>
                    {
                        if (this.isMassiveSearch)
                        {
                            this.searchSigFieldPatternMassive();
                        }
                        else
                        {
                            this.searchSigFieldPatternSimple();
                        }
                    };

                    backGroungWorker_SearchPattern.RunWorkerCompleted += (sw, ew) =>
                    {
                        if (AweSomeUtils.closeApplication)
                        {
                            this.showOrHideLoadingPanel(false);
                            if (AweSomeUtils.closeApplication)
                            {
                                MessageBox.Show(AweSomeUtils.getMsgFailLastOperation(), "Procura por Palavra-Chave.", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                                Application.Restart();
                                Application.ExitThread();
                            }
                        }
                        else
                        {
                            if (this.isFailLastOp)
                            {
                                this.isFailLastOp = false;
                                this.showOrHideLoadingPanel(false);
                                MessageBox.Show("Um ou mais documentos estão protegidos com senha.\nPara habilitar o uso de senhas em documentos protegidos, acesse o menu ARQUIVO => CONFIGURAÇÕES e desmarque a opção \"Desabilitar Verificação de Senha de Proteção do Documento\".\n\nA ativação da verificação de senha pode deixar a abertura de documentos um pouco lenta.", "Abertura de Documento", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                                Application.Restart();
                                Application.ExitThread();
                            }
                            else
                            {
                                this.showOrHideLoadingPanel(false);
                                this.chkL_FoundPatterns.Items.AddRange(this.listItems.ToArray());
                                if (!this.thereIsPatternFounded)
                                {
                                    this.btn_SelectAll.Enabled = false;
                                    this.btn_Clear.Enabled = false;
                                    MessageBox.Show(this.isMassiveSearch ? "Nenhum dos documentos contém a palavra-chave procurada." : "Este documento não contém a palavra-chave procurada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    this.btn_SelectAll.Enabled = true;
                                    this.btn_Clear.Enabled = true;
                                }
                            }
                        }
                    };

                    this.loadingPanel.Location = new System.Drawing.Point((this.Size.Width / 2) - (this.loadingPanel.Size.Width / 2), (this.Size.Height / 2) - (this.loadingPanel.Size.Height / 2) - (this.loadingPanel.Size.Height / 4));
            }
        }

        public void showOrHideLoadingPanel(bool isShow) {
            if (isShow)
            {            
                this.loadingPanel.Focus();
                this.loadingPanel.BringToFront();
                this.loadingPanel.Visible = true;
                this.pnl_BodyMain.Enabled = false;
            }
            else
            {
                this.loadingPanel.Visible = false;
                this.pnl_BodyMain.Enabled = true;
            }
        }

        public void setIsMassiveDocSearchPattern(bool isMassive) {
            this.docsHandle.setIsMassiveSearch(isMassive);
            this.isMassiveSearch = isMassive;
        }

        public void setListDocuments(string tempLocPath, List<string> docList, List<PDFDocInfo> listDocPass)
        {
            this.docsHandle.setDocFileNameList(tempLocPath, docList, listDocPass);
        }

        private void Form_SetSigStringPattern_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            this.docsHandle.closeResources();
            this.releaseResources();
            e.Cancel = true;
            this.Hide();
        }

        public void setExternalEvents(System.EventHandler event_HandleClick_1)
        {
            this.btn_Confirm.Click += event_HandleClick_1;
        }

        public void setInitialDefinition(pdftron.PDF.PDFDoc doc){
                docsHandle.setDocumentConfig(doc);
        }

        public List<PDFDocInfo> getSelectedFieldsInfo() 
        {
            List<PDFDocInfo> auxListFieldInfo = new List<PDFDocInfo>(); ;
            foreach (PDFDocInfo pdfdocInfo in docsHandle.getListPDFDocInfo())
            {
                if (this.chkL_FoundPatterns.GetItemChecked(pdfdocInfo.SigStringIndex))
                {
                    auxListFieldInfo.Add(pdfdocInfo);
                }
            }

            return auxListFieldInfo;
        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            this.docsHandle.closeResources();
            this.releaseResources();
            this.Hide();
        }

        private void closeByExtern_Event()
        {
            if (this.isMassiveSearch)
                this.Hide();
        }

        private void btn_SearchPattern_Click(object sender, EventArgs e)
        {
            this.chkL_FoundPatterns.Items.Clear();
            this.showOrHideLoadingPanel(true);
            this.backGroungWorker_SearchPattern.RunWorkerAsync();
        }

        private void txt_PatternSearch_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txt_PatternSearch.Text))
                this.btn_SearchPattern.Enabled = true;
            else
                this.btn_SearchPattern.Enabled = false;
        }

        private void chkL_FoundPatterns_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked )
            {
                if (!this.btn_Confirm.Enabled) this.btn_Confirm.Enabled = true;
            }
            else
            {
                if (this.chkL_FoundPatterns.CheckedIndices.Count <=1){
                    if (this.btn_Confirm.Enabled) this.btn_Confirm.Enabled = false;
                }
            }
        }

        private void chk_SetSigFieldConfig_CheckedChanged(object sender, System.EventArgs e)
        {
            if (this.chk_SetSigFieldConfig.Checked)
            {
                if (!AweSomeUtils.isLockSignDevice)
                {
                    if (isMassiveSearch)
                        this.isSigFieldAlredyConfigured = true;

                    this.frm_SetPreSigFieldConfig.isClosed = false;
                    this.frm_SetPreSigFieldConfig.ShowDialog();
                }
                else
                {
                    this.chk_SetSigFieldConfig.Checked = false;
                    MessageBox.Show("Um campo de assinatura está aguardando para ser assinado!\n\nFinalise a assinatura pendende antes assinar outros campos.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else { 
                if(isMassiveSearch)
                    this.isSigFieldAlredyConfigured = false;
            }
        }

        private void releaseResources() {
            this.chkL_FoundPatterns.Items.Clear();
            this.btn_SearchPattern.Enabled = false;
            this.btn_Confirm.Enabled = false;
            this.txt_PatternSearch.Text = string.Empty;
            this.chk_SetSigFieldConfig.Checked = false;
        }

        public void closeForm() {
            this.docsHandle.closeResources();
            this.releaseResources();
            this.Hide();
        }

        public bool getIsPreConfigSigField() {
            return this.chk_SetSigFieldConfig.Checked;
        }

        public bool getIsSigFieldConfigured() {
             return this.frm_SetPreSigFieldConfig.getIsSigFieldConfigured();
        }

        public void setCleanSignatureFieldConfig() {
            this.isSigFieldAlredyConfigured = false;
            this.isAlreadyConfimed = false;
        }

        public SignatureField getSetPreSigFieldConfig_SignatureField()
        {
            return this.frm_SetPreSigFieldConfig.getSignatureField();
        }

        private void searchSigFieldPatternSimple() {
            try
            {
                //this.chkL_FoundPatterns.Items.Clear();
                this.listItems.Clear();
                bool isPattFound = false;

                docsHandle.findStringPattern(this.txt_PatternSearch.Text);
                if (AweSomeUtils.closeApplication)
                    return;
                //pdftron.PDF.Rect pageSizePdf = docsHandle.getPageSize();

                //float[] pageWidthPart = { (Defins.pageSizeBase.Width / 3), ((Defins.pageSizeBase.Width / 3) * 2), Defins.pageSizeBase.Width };
                //float[] pageHeightPart = { (Defins.pageSizeBase.Height / 3), ((Defins.pageSizeBase.Height / 3) * 2), Defins.pageSizeBase.Height };
                float[] pageWidthPart = { ((Defins.pageSizeBase.Width / 2) - Defins.SignatureFieldSize.Width), (Defins.pageSizeBase.Width / 2), Defins.pageSizeBase.Width };
                float[] pageHeightPart = { ((Defins.pageSizeBase.Height / 2) - (Defins.SignatureFieldSize.Height * 2.5f)), ((Defins.pageSizeBase.Height / 2) + (Defins.SignatureFieldSize.Height * 1.25f)), Defins.pageSizeBase.Height };
                string textCheck = null;
               
                foreach (PDFDocInfo pdfdocInfo in docsHandle.getListPDFDocInfo())
                {
                    if (pdfdocInfo.SigStringPosition.Y < pageHeightPart[0])
                    {
                        textCheck = "Campo_" + (pdfdocInfo.SigStringIndex + 1 ) + " - Superior ";
                    }
                    else
                        if (pdfdocInfo.SigStringPosition.Y >= pageHeightPart[0] && pdfdocInfo.SigStringPosition.Y < pageHeightPart[1])
                        {
                            textCheck = "Campo_" + (pdfdocInfo.SigStringIndex + 1) + " - Meio ";
                        }
                        else
                        {
                            textCheck = "Campo_" + (pdfdocInfo.SigStringIndex + 1) + " - Inferior ";
                        }


                    if (pdfdocInfo.SigStringPosition.X < pageWidthPart[0])
                    {
                        textCheck += "Esquerdo - Página " + pdfdocInfo.SigStringPage;
                    }
                    else
                        if (pdfdocInfo.SigStringPosition.X >= pageWidthPart[0] && pdfdocInfo.SigStringPosition.X < pageWidthPart[1])
                        {
                            textCheck += "Central - Página " + pdfdocInfo.SigStringPage;
                        }
                        else
                        {
                            textCheck += "Direito - Página " + pdfdocInfo.SigStringPage;
                        }

                    //this.chkL_FoundPatterns.Items.Insert(pdfdocInfo.SigStringIndex, textCheck);
                    this.listItems.Insert(pdfdocInfo.SigStringIndex, textCheck);

                    if (!isPattFound)
                        isPattFound = true;
                }

                this.thereIsPatternFounded = isPattFound;
            }
            catch (Exception exc)
            {
                MessageBox.Show("Não foi possível continuar a procura por este padrão. O documento pode estar corrompido ou o padrão pode ter causado um tempo excessivo na busca." + exc.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void searchSigFieldPatternMassive() {

            try
            {
              //  this.chkL_FoundPatterns.Items.Clear();
                this.listItems.Clear();

                bool isPattFound = false;

                this.docsHandle.findStringPatternMassive(this.txt_PatternSearch.Text);
                this.isFailLastOp = AweSomeUtils.checkWhetherFailLastOperation();
                if (isFailLastOp || AweSomeUtils.closeApplication)
                {               
                    return;
                }

                string textCheck = null;

                foreach (PDFDocInfo pdfdocInfo in docsHandle.getListPDFDocInfo())
                {
                    //float[] pageWidthPart = { ((float)pdfdocInfo.pageRectangle.Width() / 3), (((float)pdfdocInfo.pageRectangle.Width() / 3) * 2), (float)pdfdocInfo.pageRectangle.Width() };
                    //float[] pageHeightPart = { ((float)pdfdocInfo.pageRectangle.Width() / 3), (((float)pdfdocInfo.pageRectangle.Width() / 3) * 2), (float)pdfdocInfo.pageRectangle.Width() };
                    float[] pageWidthPart = { (((float)pdfdocInfo.pageRectangle.Width() / 2) - Defins.SignatureField_Size_Itext.Width), ((float)pdfdocInfo.pageRectangle.Width() / 2), (float)pdfdocInfo.pageRectangle.Width() };
                    float[] pageHeightPart = { (((float)pdfdocInfo.pageRectangle.Height() / 2) - (Defins.SignatureField_Size_Itext.Height * 2.25f)), (((float)pdfdocInfo.pageRectangle.Height() / 2) + (Defins.SignatureField_Size_Itext.Height * 1.5f)), (float)pdfdocInfo.pageRectangle.Height() };

                    if (pdfdocInfo.SigStringPosition.Y < pageHeightPart[0])
                    {
                        textCheck = "["+Path.GetFileNameWithoutExtension(pdfdocInfo.documentName) + "]  Campo_" + (pdfdocInfo.SigStringIndex+1) + " - Inferior ";
                    }
                    else
                        if (pdfdocInfo.SigStringPosition.Y >= pageHeightPart[0] && pdfdocInfo.SigStringPosition.Y < pageHeightPart[1])
                        {
                            textCheck = "[" + Path.GetFileNameWithoutExtension(pdfdocInfo.documentName) + "]  Campo_" + (pdfdocInfo.SigStringIndex + 1) + " - Meio ";
                        }
                        else
                        {
                            textCheck = "[" + Path.GetFileNameWithoutExtension(pdfdocInfo.documentName) + "]  Campo_" + (pdfdocInfo.SigStringIndex + 1) + " - Superior ";
                        }


                    if (pdfdocInfo.SigStringPosition.X < pageWidthPart[0])
                    {
                        textCheck += "Esquerdo - Página " + pdfdocInfo.SigStringPage;
                    }
                    else
                        if (pdfdocInfo.SigStringPosition.X >= pageWidthPart[0] && pdfdocInfo.SigStringPosition.X < pageWidthPart[1])
                        {
                            textCheck += "Central - Página " + pdfdocInfo.SigStringPage;
                        }
                        else
                        {
                            textCheck += "Direito - Página " + pdfdocInfo.SigStringPage;
                        }

                   // this.chkL_FoundPatterns.Items.Insert(pdfdocInfo.SigStringIndex, textCheck);
                    this.listItems.Insert(pdfdocInfo.SigStringIndex, textCheck);

                    if (!isPattFound)
                        isPattFound = true;
                }

                this.thereIsPatternFounded = isPattFound;
                //if (!isPattFound)
                //{
                //    this.btn_SelectAll.Enabled = false;
                //    this.btn_Clear.Enabled = false;
                //    MessageBox.Show("Este documento não contém o padrão procurado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
                //else
                //{
                //    this.btn_SelectAll.Enabled = true;
                //    this.btn_Clear.Enabled = true;
                //}
            }
            catch (Exception exc)
            {
                MessageBox.Show("Não foi possível continuar a procura por este padrão. O documento pode estar corrompido ou o padrão pode ter causado um tempo excessivo na busca." + exc.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void btn_SelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.chkL_FoundPatterns.Items.Count; i++) {
                this.chkL_FoundPatterns.SetItemChecked(i, true);
            }
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.chkL_FoundPatterns.Items.Count; i++)
            {
                this.chkL_FoundPatterns.SetItemChecked(i, false);
            }
        }

        public void showPreSigFieldConfig() {
            this.frm_SetPreSigFieldConfig.isClosed = false;
            this.frm_SetPreSigFieldConfig.ShowDialog();
        }

        public bool getIsSigFieldAlredyPreConfigured() {
            return this.isSigFieldAlredyConfigured;
        }

        public void setIsSigFieldAlredyPreConfigured(bool isConfig) {
            this.isSigFieldAlredyConfigured = isConfig;
            this.chk_SetSigFieldConfig.Checked = true;
        }

        public void setChieldIcon(Icon icon) {
            this.frm_SetPreSigFieldConfig.Icon = icon;
        }

        private void AfterSerchPatternConfigSigField_Event() {
            if (this.isMassiveSearch) {
                if (this.isAlreadyConfimed && this.isSigFieldAlredyConfigured)
                {
                    this.btn_Confirm.PerformClick();
                }
            }
        }
       
    }
}
