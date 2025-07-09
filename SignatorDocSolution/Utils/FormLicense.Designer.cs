namespace SignatorDocSolution.Utils
{
    partial class FormLicense
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLicense));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnRemoveLicence = new System.Windows.Forms.Button();
            this.btnSendLicense = new System.Windows.Forms.Button();
            this.btnSelectFinalLicense = new System.Windows.Forms.Button();
            this.txtFinalLicense = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSelectLicLocalSave = new System.Windows.Forms.Button();
            this.txtSaveReqLiq = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnGenLicense = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkDefinitiveLicense = new System.Windows.Forms.CheckBox();
            this.btn_License = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.loadingPanel = new Utils.LoadingPanel();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.loadingPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnRemoveLicence);
            this.groupBox1.Controls.Add(this.btnSendLicense);
            this.groupBox1.Controls.Add(this.btnSelectFinalLicense);
            this.groupBox1.Controls.Add(this.txtFinalLicense);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtEmail);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnSelectLicLocalSave);
            this.groupBox1.Controls.Add(this.txtSaveReqLiq);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnGenLicense);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(409, 215);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Gerar Pedido de Licença";
            // 
            // btnRemoveLicence
            // 
            this.btnRemoveLicence.Location = new System.Drawing.Point(99, 20);
            this.btnRemoveLicence.Name = "btnRemoveLicence";
            this.btnRemoveLicence.Size = new System.Drawing.Size(158, 23);
            this.btnRemoveLicence.TabIndex = 9;
            this.btnRemoveLicence.Text = "Remover Pedido de Licença";
            this.btnRemoveLicence.UseVisualStyleBackColor = true;
            this.btnRemoveLicence.Click += new System.EventHandler(this.btnRemoveLicence_Click);
            // 
            // btnSendLicense
            // 
            this.btnSendLicense.Enabled = false;
            this.btnSendLicense.Location = new System.Drawing.Point(286, 131);
            this.btnSendLicense.Name = "btnSendLicense";
            this.btnSendLicense.Size = new System.Drawing.Size(110, 23);
            this.btnSendLicense.TabIndex = 1;
            this.btnSendLicense.Text = "Enviar Pedido";
            this.btnSendLicense.UseVisualStyleBackColor = true;
            this.btnSendLicense.Click += new System.EventHandler(this.btnSendLicense_Click);
            // 
            // btnSelectFinalLicense
            // 
            this.btnSelectFinalLicense.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectFinalLicense.Location = new System.Drawing.Point(363, 179);
            this.btnSelectFinalLicense.Name = "btnSelectFinalLicense";
            this.btnSelectFinalLicense.Size = new System.Drawing.Size(33, 25);
            this.btnSelectFinalLicense.TabIndex = 8;
            this.btnSelectFinalLicense.Text = "...";
            this.btnSelectFinalLicense.UseVisualStyleBackColor = true;
            this.btnSelectFinalLicense.Click += new System.EventHandler(this.btnSelectFinalLicense_Click);
            //
            // loadingPanel
            //
            this.loadingPanel.Visible = false;
            // 
            // txtFinalLicense
            // 
            this.txtFinalLicense.Location = new System.Drawing.Point(12, 184);
            this.txtFinalLicense.Name = "txtFinalLicense";
            this.txtFinalLicense.Size = new System.Drawing.Size(337, 20);
            this.txtFinalLicense.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 168);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(136, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Selecione sua licença final:";
            // 
            // txtEmail
            // 
            this.txtEmail.Enabled = false;
            this.txtEmail.Location = new System.Drawing.Point(10, 133);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(213, 20);
            this.txtEmail.TabIndex = 5;
            this.txtEmail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtEmail_KeyPress);
            this.txtEmail.LostFocus += new System.EventHandler(this.txtEmail_LostFocus);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(149, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Digite seu Endereço de Email:";
            // 
            // btnSelectLicLocalSave
            // 
            this.btnSelectLicLocalSave.Enabled = false;
            this.btnSelectLicLocalSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectLicLocalSave.Location = new System.Drawing.Point(363, 77);
            this.btnSelectLicLocalSave.Name = "btnSelectLicLocalSave";
            this.btnSelectLicLocalSave.Size = new System.Drawing.Size(33, 25);
            this.btnSelectLicLocalSave.TabIndex = 3;
            this.btnSelectLicLocalSave.Text = "...";
            this.btnSelectLicLocalSave.UseVisualStyleBackColor = true;
            this.btnSelectLicLocalSave.Click += new System.EventHandler(this.btnSelectLicLocalSave_Click);
            // 
            // txtSaveReqLiq
            // 
            this.txtSaveReqLiq.Enabled = false;
            this.txtSaveReqLiq.Location = new System.Drawing.Point(11, 81);
            this.txtSaveReqLiq.Name = "txtSaveReqLiq";
            this.txtSaveReqLiq.Size = new System.Drawing.Size(340, 20);
            this.txtSaveReqLiq.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Salvar Pedido de Licença em:";
            // 
            // btnGenLicense
            // 
            this.btnGenLicense.Location = new System.Drawing.Point(7, 20);
            this.btnGenLicense.Name = "btnGenLicense";
            this.btnGenLicense.Size = new System.Drawing.Size(75, 23);
            this.btnGenLicense.TabIndex = 0;
            this.btnGenLicense.Text = "Gerar";
            this.btnGenLicense.UseVisualStyleBackColor = true;
            this.btnGenLicense.Click += new System.EventHandler(this.btnGenLicense_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkDefinitiveLicense);
            this.panel1.Controls.Add(this.btn_License);
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(441, 280);
            this.panel1.TabIndex = 2;
            // 
            // chkDefinitiveLicense
            // 
            this.chkDefinitiveLicense.AutoSize = true;
            this.chkDefinitiveLicense.Enabled = true;
            this.chkDefinitiveLicense.Location = new System.Drawing.Point(12, 245);
            this.chkDefinitiveLicense.Name = "chkDefinitiveLicense";
            this.chkDefinitiveLicense.Size = new System.Drawing.Size(124, 17);
            this.chkDefinitiveLicense.TabIndex = 4;
            this.chkDefinitiveLicense.Text = "Licença Permanente";
            this.chkDefinitiveLicense.UseVisualStyleBackColor = true;
            this.chkDefinitiveLicense.CheckedChanged += new System.EventHandler(this.chkDefinitiveLicense_CheckedChanged);
            // 
            // btn_License
            // 
            this.btn_License.Enabled = false;
            this.btn_License.Location = new System.Drawing.Point(250, 245);
            this.btn_License.Name = "btn_License";
            this.btn_License.Size = new System.Drawing.Size(75, 23);
            this.btn_License.TabIndex = 3;
            this.btn_License.Text = "Licenciar";
            this.btn_License.UseVisualStyleBackColor = true;
            this.btn_License.Click += new System.EventHandler(this.btn_License_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(346, 245);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "Sair";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // FormLicense
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 280);
            this.Controls.Add(this.loadingPanel);
            this.Left = (int)((SignatorDocSolution.Utils.Defins.rectPrimaryScreen.Width / 2) - (this.ClientSize.Width/2));
            this.Top = (int)((SignatorDocSolution.Utils.Defins.rectPrimaryScreen.Height/2) - (this.ClientSize.Height/2));
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormLicense";
            this.Text = "SignatorDoc - Solicitar Licença de Uso";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormLicense_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
       

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSelectLicLocalSave;
        private System.Windows.Forms.TextBox txtSaveReqLiq;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnGenLicense;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSendLicense;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSelectFinalLicense;
        private System.Windows.Forms.TextBox txtFinalLicense;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnRemoveLicence;
        private System.Windows.Forms.Button btn_License;
        private SignatorDocSolution.Utils.LoadingPanel loadingPanel;
        private System.Windows.Forms.CheckBox chkDefinitiveLicense;

    }
}