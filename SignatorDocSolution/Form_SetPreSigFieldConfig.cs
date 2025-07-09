using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SignatorDocSolution
{
    public partial class Form_SetPreSigFieldConfig : Form
    {
        private SignatorDocSolution.Utils.CustomEvent AfterSignatureFieldConfig=new Utils.CustomEvent();
        private SignatorDocSolution.Utils.CustomEvent CloseSigStringPattern = new Utils.CustomEvent();
        public bool isClosed=false;

        public Form_SetPreSigFieldConfig()
        {
            InitializeComponent();
        }

        private void Form_SetPreSigFieldConfig_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            e.Cancel = true;
            if (!isClosed)
            {
                bool isSigFieldMissingSign = false;
                if ((signatureField.SigFieldSealType.ToString().Contains("SigImage")) && (!signatureField.getIsHandWrittenDone()) && Utils.AweSomeUtils.isLockSignDevice) {
                    isSigFieldMissingSign = true;
                }

                if (this.signatureField.getAlreadyToSign() && (!isSigFieldMissingSign))
                {
                    isClosed = true;
                    this.Hide();
                  //  this.AfterSignatureFieldConfig.Invoker.Invoke();
                }
                else
                {
                    if (MessageBox.Show("Não foi identificada uma assinatura ou carimbo válido. Gostaria de assinar ou selecionar um carimbo?\n\nPara selecionar um carimbo clique no Ícone de Configuração no canto superior esquerdo do campo no assinatura.", "Configuração de Assinatura", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                    {
                        if (isSigFieldMissingSign)
                        {
                            Utils.AweSomeUtils.isLockSignDevice = false;
                            this.signatureField.abortSignatureDrawBusy();
                        }

                        isClosed = true;
                        this.Hide();
                        this.CloseSigStringPattern.Invoker.Invoke();
                    }
                }
                
            }
        }

        public SignatureField getSignatureField() {
            return this.signatureField;
        }

        public bool getIsSigFieldConfigured() {
            return this.signatureField.getAlreadyToSign();
        }

        private void SignatureFieldConfirmClick(object sender, System.EventArgs e)
        {
            bool isSigFieldMissingSign = false;
            if ((signatureField.SigFieldSealType.ToString().Contains("SigImage")) && (!signatureField.getIsHandWrittenDone()) && Utils.AweSomeUtils.isLockSignDevice)
            {
                isSigFieldMissingSign = true;
            }

            if (this.signatureField.getAlreadyToSign() && (!isSigFieldMissingSign))
            {
                this.Hide();
                AfterSignatureFieldConfig.Invoker.Invoke();
            }
            else
                MessageBox.Show("Configure um carimbo ou assine com o dispositivo coletor de assinatura.", "Assinatura", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        
        }

        public void setAfterSigFieldConfigEvent(SignatorDocSolution.Utils.CustomEvent.GenericDelegateEventHandler customEventHandler)
        {
            this.AfterSignatureFieldConfig.Invoker = customEventHandler;
        }

        public void setCloseSigStringPatternEvent(SignatorDocSolution.Utils.CustomEvent.GenericDelegateEventHandler customEventHandler)
        {
            this.CloseSigStringPattern.Invoker = customEventHandler;
        }

    }
}
