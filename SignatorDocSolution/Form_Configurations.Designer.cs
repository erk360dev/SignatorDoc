namespace SignatorDocSolution
{
    partial class Form_Configurations
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.chk_DisableHandlePassword = new System.Windows.Forms.CheckBox();
            this.grpB_DocumentSettings = new System.Windows.Forms.GroupBox();
            this.btn_Confirm = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.grpB_DocumentSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // chk_DisableHandlePassword
            // 
            this.chk_DisableHandlePassword.AutoSize = true;
            this.chk_DisableHandlePassword.Location = new System.Drawing.Point(6, 22);
            this.chk_DisableHandlePassword.Name = "chk_DisableHandlePassword";
            this.chk_DisableHandlePassword.Size = new System.Drawing.Size(314, 17);
            this.chk_DisableHandlePassword.TabIndex = 10;
            this.chk_DisableHandlePassword.Text = "Desabilitar Verificação de Senha de Proteção do Documento";
            this.chk_DisableHandlePassword.UseVisualStyleBackColor = true;
            // 
            // grpB_DocumentSettings
            // 
            this.grpB_DocumentSettings.Controls.Add(this.chk_DisableHandlePassword);
            this.grpB_DocumentSettings.Location = new System.Drawing.Point(12, 12);
            this.grpB_DocumentSettings.Name = "grpB_DocumentSettings";
            this.grpB_DocumentSettings.Size = new System.Drawing.Size(348, 65);
            this.grpB_DocumentSettings.TabIndex = 11;
            this.grpB_DocumentSettings.TabStop = false;
            this.grpB_DocumentSettings.Text = "Configuração de Documento";
            // 
            // btn_Confirm
            // 
            this.btn_Confirm.Location = new System.Drawing.Point(285, 97);
            this.btn_Confirm.Name = "btn_Confirm";
            this.btn_Confirm.Size = new System.Drawing.Size(75, 23);
            this.btn_Confirm.TabIndex = 12;
            this.btn_Confirm.Text = "Confirmar";
            this.btn_Confirm.UseVisualStyleBackColor = true;
            this.btn_Confirm.Click += new System.EventHandler(this.btn_Confirm_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Location = new System.Drawing.Point(194, 97);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_Cancel.TabIndex = 13;
            this.btn_Cancel.Text = "Cancelar";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // Form_Configurations
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 132);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_Confirm);
            this.Controls.Add(this.grpB_DocumentSettings);
            this.Name = "Form_Configurations";
            this.Text = "SignatorDoc - Configurações";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Configurations_FormClosing);
            this.grpB_DocumentSettings.ResumeLayout(false);
            this.grpB_DocumentSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chk_DisableHandlePassword;
        private System.Windows.Forms.GroupBox grpB_DocumentSettings;
        private System.Windows.Forms.Button btn_Confirm;
        private System.Windows.Forms.Button btn_Cancel;

    }
}