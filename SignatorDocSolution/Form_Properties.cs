using iTextSharp.text.pdf;
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
    public partial class Form_Properties : Form
    {
        private string fileName = null;
        private string tempFileName = null;
        private bool isNeedViewPasswd = false;
        private bool isNeedEditPasswd=false;
        private string viewPasswd = null;
        private string editPasswd = null;

        public Form_Properties()
        {
            InitializeComponent();
        }

        private void Form_Properties_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            #region security check
            if (this.isNeedViewPasswd)
            {
                if (this.chkPassDocView.Checked)
                {
                    if (!this.viewPasswd.Equals(this.txtViewOld.Text))
                    {
                        MessageBox.Show("A senha atual de visualização do documento está incorreta. Tente novamente!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
                 
            if (this.isNeedEditPasswd)
            {
                if (this.chkEditPassDoc.Checked)
                {
                    if (!this.editPasswd.Equals(this.txtEditOld.Text))
                    {
                        MessageBox.Show("A senha atual de alteração do documento está incorreta. Tente novamente!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
            

            if (this.chkPassDocView.Checked) {
                if(!string.IsNullOrEmpty(this.txtViewNew.Text))
                {
                    if (!this.txtViewNew.Text.Equals(this.txtViewNewConfirm.Text)) 
                    {
                        MessageBox.Show("As senhas de visualização do documento não conferem. Tente novamente!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                else
                {
                        MessageBox.Show("As senhas de visualização do documento devem ser preenchidas. Tente novamente!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                }
            }

            if (this.chkEditPassDoc.Checked)
            {
                if (!string.IsNullOrEmpty(this.txtEditNew.Text))
                {
                    if (!this.txtEditNew.Text.Equals(this.txtEditNewConfirm.Text))
                    {
                        MessageBox.Show("As senhas de alteração do documento não conferem. Tente novamente!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("As senhas de alteração do documento devem ser preenchidas. Tente novamente!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            if ((!this.chkPassDocView.Checked) && (!this.isNeedViewPasswd) && (!this.chkEditPassDoc.Checked) && (!this.isNeedEditPasswd))
            {
                MessageBox.Show("Nenhuma propriedade foi definida!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (!this.chkPassDocView.Checked && this.isNeedViewPasswd)
            {
                string auxpasswdv=SignatorDocSolution.Utils.AweSomeUtils.ShowPasswordDialog("Digite a senha de visualização atual para confirmar a remoção.", "Confirmação de Senha");
                if (auxpasswdv.Length == 0)
                    return;
                if (!this.viewPasswd.Equals(auxpasswdv))
                {
                    MessageBox.Show("A senha atual de visualização está incorreta!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            if (!this.chkEditPassDoc.Checked && this.isNeedEditPasswd)
            {
                string auxpasswd = SignatorDocSolution.Utils.AweSomeUtils.ShowPasswordDialog("Digite a senha de alteração atual para confirmar a remoção.", "Confirmação de Senha");
                if (auxpasswd.Length == 0)
                    return;
                if (!this.editPasswd.Equals(auxpasswd))
                {
                    MessageBox.Show("A senha atual de alteraçao está incorreta!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }


#endregion

            try
            {
                string outFileName = this.fileName;
                string inFileName = this.tempFileName;
                bool isOldPassNeed = this.isNeedEditPasswd || this.isNeedViewPasswd;
                string needPassword = this.isNeedEditPasswd ? this.editPasswd : (this.isNeedViewPasswd ? this.viewPasswd : "");
                // int permission =  (PdfWriter.ALLOW_PRINTING | PdfWriter.ALLOW_MODIFY_CONTENTS | PdfWriter.ALLOW_COPY |PdfWriter.ALLOW_MODIFY_ANNOTATIONS | PdfWriter.ALLOW_FILL_IN | PdfWriter.ALLOW_SCREENREADERS | PdfWriter.ALLOW_ASSEMBLY | PdfWriter.ALLOW_DEGRADED_PRINTING);

               // if (((!this.chkPassDocView.Checked) && this.isNeedViewPasswd) || ((!this.chkEditPassDoc.Checked) && this.isNeedEditPasswd))
                if ( (!this.chkEditPassDoc.Checked) && (!this.chkPassDocView.Checked) )
                {
                    using (FileStream outFileStream = new FileStream(outFileName, FileMode.Create))
                    {

                        using (PdfReader pdfReader = isOldPassNeed ? new PdfReader(inFileName, System.Text.Encoding.Default.GetBytes(needPassword)) : new PdfReader(inFileName))
                        {
                            PdfStamper pdfStamper = new PdfStamper(pdfReader, outFileStream);
                            pdfStamper.Close();
                        }
                    }
                }

                if (this.chkPassDocView.Checked || this.chkEditPassDoc.Checked)
                {
                    using (FileStream outFileStream = new FileStream(outFileName, FileMode.Create))
                    {
                        using (PdfReader pdfReader = isOldPassNeed ? new PdfReader(inFileName, System.Text.Encoding.Default.GetBytes(needPassword)) : new PdfReader(inFileName))
                        {
                            //using(PdfStamper pdfStamper = new PdfStamper(pdfReader, outFileStream)){
                            //    pdfStamper.SetEncryption(PdfWriter.ENCRYPTION_AES_128, (this.chkPassDocView.Checked ? this.txtViewNew.Text : null),(this.chkEditPassDoc.Checked ? this.txtEditNew.Text : this.txtViewNew.Text),PdfWriter.ALLOW_PRINTING);
                            //}
                            PdfEncryptor.Encrypt(pdfReader, outFileStream, true, (this.chkPassDocView.Checked ? this.txtViewNew.Text : null), (this.chkEditPassDoc.Checked ? this.txtEditNew.Text : this.txtViewNew.Text), PdfWriter.ALLOW_PRINTING);
                        }
                    }
                }

                this.Hide();
                MessageBox.Show("Propriedades de segurança alteradas com sucesso!\nPara as alteração de segurança terem efeito, o documento deve ser recarregado.\nO SignatorDoc será REINICIADO.", "Senhas", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Restart();
                Application.ExitThread(); 
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        public void setIntialDocProperties(string file_Name, string tempFile_Name) {
            this.fileName = file_Name;
            this.tempFileName = tempFile_Name;
            this.txtViewOld.Enabled = false;
            this.txtEditOld.Enabled = false;
        }

        public bool checkDocInitialProperties (bool isNeedPassToOpen, bool isNeedPassToEdit, bool isPassEditOk, string passEdit, string passOpen, bool isDocChanged, bool isDocSaved)
        {
            this.cleanProperties();

            if (!SignatorDocSolution.Utils.Configurations.Documents_DisableCheckPassword)
            {

                if (isNeedPassToOpen)
                {
                    this.viewPasswd = passOpen;
                    this.isNeedViewPasswd = true;
                    this.txtViewNew.Text = this.viewPasswd;
                    this.txtViewNewConfirm.Text = this.viewPasswd;

                    if (!this.chkPassDocView.Checked)
                        this.chkPassDocView.Checked = true;
                    else
                    {
                        this.txtViewOld.Enabled = true;
                        this.txtViewNew.Enabled = true;
                        this.txtViewNewConfirm.Enabled = true;
                    }
                }

                if (isPassEditOk)
                {
                    this.editPasswd = passEdit;
                    this.isNeedEditPasswd = true;
                    this.txtEditNew.Text = this.editPasswd;
                    this.txtEditNewConfirm.Text=this.editPasswd;

                    if (!this.chkEditPassDoc.Checked)
                        this.chkEditPassDoc.Checked = true;
                    else
                    {
                        this.txtEditOld.Enabled = true;
                        this.txtEditNew.Enabled = true;
                        this.txtEditNewConfirm.Enabled = true;
                    }
                }
                else
                {
                    if (isNeedPassToEdit)
                    {
                        if (this.checkEditPassword())
                        {
                            this.isNeedEditPasswd = true;
                            this.txtEditNew.Text = this.editPasswd;
                            this.txtEditNewConfirm.Text = this.editPasswd;

                            if (!this.chkEditPassDoc.Checked)
                                this.chkEditPassDoc.Checked = true;
                            else
                            {
                                this.txtEditOld.Enabled = true;
                                this.txtEditNew.Enabled = true;
                                this.txtEditNewConfirm.Enabled = true;
                            }
                        }
                        else
                            return false;

                    }
                }
            }
            else
            {
                int resCheckPass = SignatorDocSolution.Utils.AweSomeUtils.checkIsDocPassEditNeed(this.fileName, false, null);
                if (resCheckPass == 1)
                {
                    if (this.checkEditPassword())
                    {
                        this.isNeedEditPasswd = true;
                        this.txtEditNew.Text = this.editPasswd;
                        this.txtEditNewConfirm.Text = this.editPasswd;

                        if (!this.chkEditPassDoc.Checked)
                            this.chkEditPassDoc.Checked = true;
                        else
                        {
                            this.txtEditOld.Enabled = true;
                            this.txtEditNew.Enabled = true;
                            this.txtEditNewConfirm.Enabled = true;
                        }
                    }
                    else
                        return false;
                }
                else
                {
                    if (resCheckPass > 1)
                    {
                        MessageBox.Show("Houve uma falha ao checar uma ou mais propriedades de segurança do documento. Não será possível alterar as propriedades editáveis do documento!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else
                    {
                        if (this.chkEditPassDoc.Checked)
                            this.chkEditPassDoc.Checked = false;
                        else
                        {
                            this.txtEditOld.Enabled = false;
                            this.txtEditNew.Enabled = false;
                            this.txtEditNewConfirm.Enabled = false;
                        }
                    }

                }
            }

            return true;
        }

        private string getEditPasswd()
        {
            string resPasswd = SignatorDocSolution.Utils.AweSomeUtils.ShowPasswordDialog("Para editar as propriedade deste documento é preciso digitar a senha de alteração.", "Propriedades");
            if (resPasswd.Length == 0)
                return "`\01´";

            try
            {
                using (iTextSharp.text.pdf.PdfReader pdfReader = new iTextSharp.text.pdf.PdfReader(this.tempFileName, System.Text.Encoding.Default.GetBytes(resPasswd)))
                {
                    if (pdfReader.IsOpenedWithFullPermissions)
                    {
                        pdfReader.Close();
                        return resPasswd;
                    }
                    else
                    {
                        throw new iTextSharp.text.pdf.BadPasswordException("Bad user password");
                    }
                }
            }
            catch (iTextSharp.text.pdf.BadPasswordException bpexc)
            {
                return "`\02´";
            }
            catch (System.Exception exc)
            {
                throw new System.Exception(exc.Message);
            }

        }

        private bool checkEditPassword(){
            string resPasswd = this.getEditPasswd();
            switch (resPasswd)
            {
                case "`\01´": { return false; }
                case "`\02´": { MessageBox.Show("A senha digitada está incorreta!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
                default:
                    {
                        this.editPasswd = resPasswd; 
                        return true;
                    }
            }
        }

        private void cleanProperties() {
            this.isNeedViewPasswd = false;
            this.isNeedEditPasswd = false;
            this.txtViewOld.Text = ""; 
            this.txtViewNew.Text = ""; 
            this.txtViewNewConfirm.Text = "";
            this.txtEditOld.Text = ""; 
            this.txtEditNew.Text = "";
            this.txtEditNewConfirm.Text = "";
            this.viewPasswd = null;
            this.editPasswd = null;

            if (this.chkPassDocView.Checked)
                this.chkPassDocView.Checked = false;
            else 
            {
                this.txtViewOld.Enabled = false;
                this.txtViewNew.Enabled = false;
                this.txtViewNewConfirm.Enabled=false;
            }

            if (this.chkEditPassDoc.Checked)
                this.chkEditPassDoc.Checked = false;
            else 
            {
                this.txtEditOld.Enabled = false; ;
                this.txtEditNew.Enabled = false; ;
                this.txtEditNewConfirm.Enabled = false; ;
            }
            
        }

        private void chkPassDocView_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkPassDocView.Checked)
            {
                this.txtViewOld.Enabled = this.isNeedViewPasswd;
                this.txtViewNew.Enabled = true;
                this.txtViewNewConfirm.Enabled = true;
            }
            else {
                this.txtViewOld.Enabled = false;
                this.txtViewNew.Enabled = false;
                this.txtViewNewConfirm.Enabled = false;
            }
        }

        private void chkEditPassDoc_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkEditPassDoc.Checked)
            {
                this.txtEditOld.Enabled = this.isNeedEditPasswd;
                this.txtEditNew.Enabled = true;
                this.txtEditNewConfirm.Enabled = true;
            }
            else
            {
                this.txtEditOld.Enabled = false;
                this.txtEditNew.Enabled = false;
                this.txtEditNewConfirm.Enabled = false;
            }
        }


    }
}
