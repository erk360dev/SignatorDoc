namespace SignatorDocSolution
{
    partial class Form_SetPreSigFieldConfig
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (this.signatureField.frm_SignConfig != null)
                this.signatureField.frm_SignConfig.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Drawing.Size formSize= new System.Drawing.Size(310, 180);
            this.signatureField = new SignatureField(new System.EventHandler(SignatureFieldConfirmClick));
            this.signatureField.isDesactiveScreenAnyCommands = true;
            this.SuspendLayout();
            //
            // signatureField
            //
            this.signatureField.Location = new System.Drawing.Point(((formSize.Width / 2) - (SignatorDocSolution.Utils.Defins.SignatureField_Size_Base.Width / 2)), (((formSize.Height / 2) - (SignatorDocSolution.Utils.Defins.SignatureField_Size_Base.Height / 2)))+20 );
            // 
            // Form_SetPreSigFieldConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = formSize;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.signatureField);
            this.FormClosing+=new System.Windows.Forms.FormClosingEventHandler(Form_SetPreSigFieldConfig_FormClosing);
            this.Name = "Form_SetPreSigFieldConfig";
            this.Text = "SignatorDoc - Configuração de Assinatura";
            this.ResumeLayout(false);

        }

        #endregion

        private SignatureField signatureField;
    }
}