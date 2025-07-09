using SignatorDocSolution.Utils;
using pdftron.PDF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SignatorDocSolution
{
    public partial class Form_DefineSigFieldsMassiveDocs : Form
    {
        private string tempFilesLocation = null;
        private List<SignatureField> listSignatureField = new List<SignatureField>();
        private List<PDFDocInfo> listPDFDocInfo = new List<PDFDocInfo>();
        private List<PDFDocInfo> listDocPassword = new List<PDFDocInfo>();
        private int addFieldCount = 0;

        public Form_DefineSigFieldsMassiveDocs()
        {
            InitializeComponent();
           
        }

        private void Form_DefineSigFieldsMassiveDocs_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        public void setListBoxDocuments(object[] items, List<PDFDocInfo> listdocpass) {
            this.lstBDocuments.Items.Clear();
            this.lstBDocuments.Items.AddRange(items);
            this.listDocPassword = listdocpass;
        }

        public void setTempFilesLocation(string tempLocation) {
            this.tempFilesLocation = tempLocation;
        }

        private void lstBDocuments_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string pdfFileName = this.tempFilesLocation+ @"\"+this.lstBDocuments.SelectedItem.ToString();
            try
            {
                using (pdftron.PDF.PDFDoc pdfdoc = new pdftron.PDF.PDFDoc(pdfFileName))
                {
                    this.chkLPages.Items.Clear();
                    for (int i = 1; i <= pdfdoc.GetPageCount(); i++)
                    {
                        this.chkLPages.Items.Add("Página " + i);
                    }
                    pdfdoc.Close();
                }
            }catch(pdftron.Common.PDFNetException pexp){
                if (pexp.Message.Contains("Message: Attempt to load a free object\n\t Conditional expression: obj.IsFree() "))
                {
                    MessageBox.Show("Ocorreu um erro na tentativa de manipular o documento [" + System.IO.Path.GetFileNameWithoutExtension(pdfFileName) + "].\nO documento está danificado e não pode ser restaurado.", "Replicar Assinaturas", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (pexp.Message.Contains("Compressed object is corrupt"))
                    {
                        MessageBox.Show("Um ou mais documentos estão protegidos com senha.\nPara habilitar o uso de senhas em documentos protegidos, acesse o menu ARQUIVO => CONFIGURAÇÕES e desmarque a opção \"Desabilitar Verificação de Senha de Proteção do Documento\".\n\nA ativação da verificação de senha pode deixar a abertura de documentos um pouco lenta.", "Replicar Assinaturas", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Restart();
                        Application.ExitThread();
                    }
                    else
                    {
                        throw new System.Exception(pexp.Message);
                    }
                }    
            }
            this.uncheckAllSigFieldList();
        }

        public void cleanConfiguration() {
            this.chkLPages.Items.Clear();
            this.listPDFDocInfo.Clear();
            this.chkLCorfirmedSigFields.Items.Clear();
            this.addFieldCount = 0;
            this.chkSelAllLastDocPages.Checked = false;
            this.btn_SigDefsRemove.Enabled = false;
            this.btn_ConfirmSigFields.Enabled = false;
        }

        private void chkLPages_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                if (this.chkLSigFields.CheckedItems.Count > 0)
                    if (!this.btn_ConfirmPageDocsFields.Enabled)
                        this.btn_ConfirmPageDocsFields.Enabled = true;
             //   if (!this.btn_AddPageDocs.Enabled)
              //      this.btn_AddPageDocs.Enabled = true;
            }
            else {
                if (this.chkLPages.CheckedIndices.Count <= 1)
                    this.btn_ConfirmPageDocsFields.Enabled = false;
                    //this.btn_AddPageDocs.Enabled = false;
            }
        }

        private void btn_CheckAllSigFields_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.chkLSigFields.Items.Count; i++)
            {
                this.chkLSigFields.SetItemChecked(i, true);
            }
        }

        private void btn_ClearAllSigFields_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.chkLSigFields.Items.Count; i++)
            {
                this.chkLSigFields.SetItemChecked(i, false);
            }
        }

        public void setPreSigFields(List<SignatureField> sigFieldList ) {
            this.chkLSigFields.Items.Clear();
            //float[] pageWidthPart = { (Defins.pageSizeBase.Width / 3), ((Defins.pageSizeBase.Width / 3) * 2), Defins.pageSizeBase.Width };
            //float[] pageHeightPart = { (Defins.pageSizeBase.Height / 3), ((Defins.pageSizeBase.Height / 3) * 2), Defins.pageSizeBase.Height };
            float[] pageWidthPart = { ((Defins.pageSizeBase.Width / 2) - Defins.SignatureFieldSize.Width), (Defins.pageSizeBase.Width / 2), Defins.pageSizeBase.Width };
            float[] pageHeightPart = { ((Defins.pageSizeBase.Height / 2) - (Defins.SignatureFieldSize.Height * 2.5f)), ((Defins.pageSizeBase.Height / 2) + (Defins.SignatureFieldSize.Height * 1.5f)), Defins.pageSizeBase.Height };
            string textCheck = null;
            int sfControl = 0;

            foreach (SignatureField sigField in sigFieldList) {
                if (sigField.Location.Y < pageHeightPart[0])
                {
                    textCheck = "Campo_" + (sfControl+1) + " - Superior ";
                }
                else
                    if (sigField.Location.Y >= pageHeightPart[0] && sigField.Location.Y < pageHeightPart[1])
                    {
                        textCheck = "Campo_" + (sfControl + 1) + " - Meio ";
                    }
                    else
                    {
                        textCheck = "Campo_" + (sfControl + 1) + " - Inferior ";
                    }


                if (sigField.Location.X < pageWidthPart[0])
                {
                    textCheck += "Esquerdo";
                }
                else
                    if (sigField.Location.X >= pageWidthPart[0] && sigField.Location.X < pageWidthPart[1])
                    {
                        textCheck += "Central";
                    }
                    else
                    {
                        textCheck += "Direito";
                    }

                this.chkLSigFields.Items.Insert(sfControl,textCheck);
                this.listSignatureField.Insert(sfControl, sigField);
                sfControl++;
            }
        }

        private void chkLSigFields_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                if (this.chkLPages.CheckedItems.Count > 0)
                    if (!this.btn_ConfirmPageDocsFields.Enabled)
                        this.btn_ConfirmPageDocsFields.Enabled = true;
            }
            else {
                if (this.chkLSigFields.CheckedItems.Count <= 1)
                    this.btn_ConfirmPageDocsFields.Enabled = false;
            }
        }

        private void btn_ConfirmPageDocsFields_Click(object sender, EventArgs e)
        {
            string currentDocName = this.lstBDocuments.SelectedItem.ToString();
            int currentDocIndex= this.lstBDocuments.SelectedIndex;
            string auxTextChech=null;
            float auxX;
            float auxY;
            bool isFieldAdded = false;
            foreach (string pItem in this.chkLPages.CheckedItems) {
                foreach (int sfItem in this.chkLSigFields.CheckedIndices) { 
                    auxX = (this.listSignatureField[sfItem].Location.X / Defins.pagesDPI) * 72.0F;
                    auxY = (((Defins.pageSizeBase.Height - this.listSignatureField[sfItem].Location.Y) - this.listSignatureField[sfItem].Size.Height) / Defins.pagesDPI) * 72.0F;            
                    PDFDocInfo auxDocInfo= new PDFDocInfo(currentDocName, currentDocIndex, new System.Drawing.PointF(auxX,auxY), int.Parse(pItem.Split(' ')[1]), addFieldCount,new Rect(0,0,0,0));
                    auxDocInfo.fieldImage=this.listSignatureField[sfItem].Image;
                    auxDocInfo.FieldSealType= this.listSignatureField[sfItem].SigFieldSealType;
                    auxDocInfo.bioDataSignature = this.listSignatureField[sfItem].getDataBioSig();
                    auxDocInfo.fieldAlreadyToSign = this.listSignatureField[sfItem].getAlreadyToSign();
                    if (!isFieldAdded)
                        isFieldAdded = true;

                    auxTextChech = "[ " + currentDocName + " ]  - Página " + auxDocInfo.SigStringPage + "  -  " + this.chkLSigFields.Items[sfItem];
                    if (!this.chkLCorfirmedSigFields.Items.Contains(auxTextChech))
                    {
                        this.chkLCorfirmedSigFields.Items.Insert(addFieldCount, auxTextChech);
                        this.listPDFDocInfo.Insert(addFieldCount, auxDocInfo);
                        addFieldCount++;
                    }
                }
            }
            if (isFieldAdded)
                this.btn_SigDefsRemove.Enabled = true;
        }

        private void uncheckAllSigFieldList() {
            for (int i = 0; i < this.chkLSigFields.Items.Count; i++) {
                this.chkLSigFields.SetItemChecked(i, false);
            }
        }

        private void btn_CheckAllPages_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.chkLPages.Items.Count; i++) {
                this.chkLPages.SetItemChecked(i, true);
            }
        }

        private void btn_SelectAllFieldsDefs_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.chkLCorfirmedSigFields.Items.Count; i++) {
                this.chkLCorfirmedSigFields.SetItemChecked(i, true);
            }
        }
        
        private void btn_ClearAllPages_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.chkLPages.Items.Count; i++) {
                this.chkLPages.SetItemChecked(i, false);
            }
        }

        private void btn_ClearAllFieldsDefs_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.chkLCorfirmedSigFields.Items.Count; i++)
            {
                this.chkLCorfirmedSigFields.SetItemChecked(i, false);
            }
        }

        public void setExternalEvents(System.EventHandler event_HandleClick_1)
        {
            this.btn_ConfirmSigFields.Click += event_HandleClick_1;
        }

        public List<PDFDocInfo> getlistPDFDocInfo() {
            return this.listPDFDocInfo;
        }

        private void btn_ClearWholeConfig_Click(object sender, EventArgs e)
        {
            this.chkLPages.Items.Clear();
            this.chkLCorfirmedSigFields.Items.Clear();
            this.uncheckAllSigFieldList();
            this.listPDFDocInfo.Clear();
            this.btn_SigDefsRemove.Enabled = false;
            this.btn_ConfirmPageDocsFields.Enabled = false;
            addFieldCount = 0;
        }

        private void btn_SigDefsRemove_Click(object sender, EventArgs e)
        {
            if (this.chkLCorfirmedSigFields.CheckedIndices.Count < 1)
                return;
            if (MessageBox.Show("Deletar Campos Selecionados?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                for(int index=(this.chkLCorfirmedSigFields.Items.Count-1);index>=0;index--)
                {
                    if (this.chkLCorfirmedSigFields.GetItemChecked(index))
                    {
                        this.listPDFDocInfo.RemoveAt(index);
                        this.chkLCorfirmedSigFields.Items.RemoveAt(index);
                        addFieldCount--;
                    }
                }
                if (this.chkLCorfirmedSigFields.Items.Count < 1)
                {
                    this.btn_SigDefsRemove.Enabled = false;
                    this.btn_ConfirmSigFields.Enabled = false;
                }
            }
            
        }

        private void chkLCorfirmedSigFields_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            this.listPDFDocInfo[e.Index].fieldEnabled = (e.NewValue == CheckState.Checked);
            if (e.NewValue == CheckState.Checked)
            {
                if (!this.btn_ConfirmSigFields.Enabled)
                    this.btn_ConfirmSigFields.Enabled = true;
            }
            else
            {
                if (this.chkLCorfirmedSigFields.CheckedItems.Count <= 1)
                    this.btn_ConfirmSigFields.Enabled = false;
            }
            
        }

        private void chkSelAllLastDocPages_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkSelAllLastDocPages.Checked) {
                if (this.chkLSigFields.CheckedIndices.Count > 0)
                {
                    string currentDocName=null;
                    try
                    {
                        string pdfFileName;
                        string auxTextChech;
                        int currentDocIndex = this.lstBDocuments.SelectedIndex;
                        bool isFieldAdded = false;
                        float auxX;
                        float auxY;
                        PDFDocInfo tempAuxpdfdocpass = null; // HandleEncryption
                        bool passNeed = false; // HandleEncryption

                        for (int idoc = 0; idoc < this.lstBDocuments.Items.Count; idoc++)
                        {
                            currentDocName = this.lstBDocuments.Items[idoc].ToString();
                            pdfFileName = this.tempFilesLocation + @"\" + currentDocName;

                            #region Handle Passwords Encryption Document
                            // Begin handleEncryption
                            passNeed = false;
                            if (this.listDocPassword.Count > 0)
                            {
                                tempAuxpdfdocpass = this.listDocPassword.Find(doc => doc.documentName == currentDocName);
                                if (tempAuxpdfdocpass != null)
                                    passNeed = true;
                            }
                            // End handleEncryption
                            #endregion
                            
                            try
                            {
                                using (pdftron.PDF.PDFDoc pdfdoc = new pdftron.PDF.PDFDoc(pdfFileName))
                                {
                                    if (passNeed) // HandleEncryption
                                        pdfdoc.InitStdSecurityHandler(tempAuxpdfdocpass.passwdToEdit); // HandleEncryption

                                    int lastPage = pdfdoc.GetPageCount();
                                    foreach (int sfIndex in this.chkLSigFields.CheckedIndices)
                                    {
                                        auxX = (this.listSignatureField[sfIndex].Location.X / Defins.pagesDPI) * 72.0F;
                                        auxY = (((Defins.pageSizeBase.Height - this.listSignatureField[sfIndex].Location.Y) - this.listSignatureField[sfIndex].Size.Height) / Defins.pagesDPI) * 72.0F;
                                        PDFDocInfo auxDocInfo = new PDFDocInfo(currentDocName, idoc, new System.Drawing.PointF(auxX, auxY), lastPage, addFieldCount, new Rect(0, 0, 0, 0));
                                        auxDocInfo.fieldImage = this.listSignatureField[sfIndex].Image;
                                        auxDocInfo.FieldSealType = this.listSignatureField[sfIndex].SigFieldSealType;
                                        auxDocInfo.bioDataSignature = this.listSignatureField[sfIndex].getDataBioSig();
                                        auxDocInfo.fieldAlreadyToSign = this.listSignatureField[sfIndex].getAlreadyToSign();

                                        auxTextChech = "[ " + currentDocName + " ]  - Página " + auxDocInfo.SigStringPage + "  -  " + this.chkLSigFields.Items[sfIndex];
                                        if (!this.chkLCorfirmedSigFields.Items.Contains(auxTextChech))
                                        {
                                            this.chkLCorfirmedSigFields.Items.Insert(addFieldCount, auxTextChech);
                                            this.listPDFDocInfo.Insert(addFieldCount, auxDocInfo);
                                            addFieldCount++;
                                        }

                                        if (!isFieldAdded)
                                            isFieldAdded = true;

                                    }
                                }
                            }catch(pdftron.Common.PDFNetException pexp){
                                if (pexp.Message.Contains("Message: Attempt to load a free object\n\t Conditional expression: obj.IsFree() "))
                                {
                                    MessageBox.Show("Ocorreu um erro na tentativa de replicar uma ou mais assinaturas no documento [" + currentDocName + "].\nO documento está danificado e não pode ser restaurado.", "Replicar Assinaturas", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                    throw new System.Exception(pexp.Message);
                            }
                        }

                        if (isFieldAdded)
                            this.btn_SigDefsRemove.Enabled = true;
                    }
                    catch (Exception pdne)
                    {
                        if (pdne.Message.Contains("Compressed object is corrupt"))
                        {
                            MessageBox.Show("Um ou mais documentos estão protegidos com senha.\nPara habilitar o uso de senhas em documentos protegidos, acesse o menu ARQUIVO => CONFIGURAÇÕES e desmarque a opção \"Desabilitar Verificação de Senha de Proteção do Documento\".\n\nA ativação da verificação de senha pode deixar a abertura de documentos um pouco lenta.", "Abertura de Documento", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Application.Restart();
                            Application.ExitThread();
                        }
                        else
                           throw new System.Exception("Ocorreu um erro em objeto interno referente ao documento [" + currentDocName + "].\nO documento pode estar danificado e ilégível.\n\n Se o Erro persistir, contate o fornecedor do sistema.\nErro:"+ pdne.Message);                            
                    }
                }
                else
                {
                    this.chkSelAllLastDocPages.Checked = false;
                    MessageBox.Show("É preciso selecionar ao menos um campo de assinatura!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }


    }
}
