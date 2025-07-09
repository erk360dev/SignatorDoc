namespace SignatorDocSolution
{
    partial class Form_SignConfig
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_SignConfig));
            this.label1 = new System.Windows.Forms.Label();
            this.txtSignerName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboCertificatesList = new System.Windows.Forms.ComboBox();
            this.btnConfirmConfigSigs = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkHandWritten = new System.Windows.Forms.CheckBox();
            this.chkStamp = new System.Windows.Forms.CheckBox();
            this.lblDeviceNotAvailable = new System.Windows.Forms.Label();
            this.chkShowHash = new System.Windows.Forms.CheckBox();
            this.chkShowDateTime = new System.Windows.Forms.CheckBox();
            this.cboSelStamp = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkShowUserNameDigSig = new System.Windows.Forms.CheckBox();
            this.chkVisible = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nome do Signatário:";
            // 
            // txtSignerName
            // 
            this.txtSignerName.Location = new System.Drawing.Point(122, 10);
            this.txtSignerName.Name = "txtSignerName";
            this.txtSignerName.Size = new System.Drawing.Size(290, 20);
            this.txtSignerName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(51, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Certificados:";
            // 
            // cboCertificatesList
            // 
            this.cboCertificatesList.FormattingEnabled = true;
            this.cboCertificatesList.Location = new System.Drawing.Point(122, 42);
            this.cboCertificatesList.Name = "cboCertificatesList";
            this.cboCertificatesList.Size = new System.Drawing.Size(290, 21);
            this.cboCertificatesList.TabIndex = 3;
            this.cboCertificatesList.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboCertificatesList_KeyPress);
            // 
            // btnConfirmConfigSigs
            // 
            this.btnConfirmConfigSigs.Location = new System.Drawing.Point(338, 210);
            this.btnConfirmConfigSigs.Name = "btnConfirmConfigSigs";
            this.btnConfirmConfigSigs.Size = new System.Drawing.Size(75, 23);
            this.btnConfirmConfigSigs.TabIndex = 4;
            this.btnConfirmConfigSigs.Text = "OK";
            this.btnConfirmConfigSigs.UseVisualStyleBackColor = true;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(228, 210);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox1
            //
            this.groupBox1.Controls.Add(this.chkHandWritten);
            this.groupBox1.Controls.Add(this.chkStamp);
            this.groupBox1.Controls.Add(this.chkVisible);
            this.groupBox1.Controls.Add(this.lblDeviceNotAvailable);
            this.groupBox1.Controls.Add(this.chkShowHash);
            this.groupBox1.Controls.Add(this.chkShowDateTime);
            this.groupBox1.Controls.Add(this.cboSelStamp);
            this.groupBox1.Location = new System.Drawing.Point(122, 67);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(291, 129);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // chkHandWritten
            // 
            this.chkHandWritten.AutoSize = true;
            this.chkHandWritten.Location = new System.Drawing.Point(7, 14);
            this.chkHandWritten.Name = "chkHandWritten";
            this.chkHandWritten.Size = new System.Drawing.Size(130, 17);
            this.chkHandWritten.TabIndex = 11;
            this.chkHandWritten.Text = "Assinatura Manuscrita";
            this.chkHandWritten.UseVisualStyleBackColor = true;
            this.chkHandWritten.CheckedChanged += new System.EventHandler(this.chkHandWritten_CheckedChanged);
            // 
            // chkStamp
            // 
            this.chkStamp.AutoSize = true;
            this.chkStamp.Location = new System.Drawing.Point(147, 14);
            this.chkStamp.Name = "chkStamp";
            this.chkStamp.Size = new System.Drawing.Size(64, 17);
            this.chkStamp.TabIndex = 10;
            this.chkStamp.Text = "Carimbo";
            this.chkStamp.UseVisualStyleBackColor = true;
            this.chkStamp.CheckedChanged += new System.EventHandler(this.chkStamp_CheckedChanged);
            // 
            // chkVisible
            // 
            this.chkVisible.AutoSize = true;
            this.chkVisible.Location = new System.Drawing.Point(219, 14);
            this.chkVisible.Name = "chkVisible";
            this.chkVisible.Size = new System.Drawing.Size(66, 17);
            this.chkVisible.TabIndex = 12;
            this.chkVisible.Text = "Invisível";
            this.chkVisible.UseVisualStyleBackColor = true;
            this.chkVisible.CheckedChanged += new System.EventHandler(this.chkVisible_CheckedChanged);
            // 
            // lblDeviceNotAvailable
            // 
            this.lblDeviceNotAvailable.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDeviceNotAvailable.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.lblDeviceNotAvailable.Location = new System.Drawing.Point(30, 28);
            this.lblDeviceNotAvailable.Name = "lblDeviceNotAvailable";
            this.lblDeviceNotAvailable.Size = new System.Drawing.Size(100, 10);
            this.lblDeviceNotAvailable.TabIndex = 9;
            this.lblDeviceNotAvailable.Text = "Tablet não detectado";
            this.lblDeviceNotAvailable.Visible = false;
            // 
            // chkShowHash
            // 
            this.chkShowHash.AutoSize = true;
            this.chkShowHash.Checked = true;
            this.chkShowHash.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowHash.Location = new System.Drawing.Point(139, 98);
            this.chkShowHash.Name = "chkShowHash";
            this.chkShowHash.Size = new System.Drawing.Size(146, 17);
            this.chkShowHash.TabIndex = 4;
            this.chkShowHash.Text = "Mostrar Identidade Digital";
            this.chkShowHash.UseVisualStyleBackColor = true;
            // 
            // chkShowDateTime
            // 
            this.chkShowDateTime.AutoSize = true;
            this.chkShowDateTime.Checked = true;
            this.chkShowDateTime.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowDateTime.Location = new System.Drawing.Point(7, 98);
            this.chkShowDateTime.Name = "chkShowDateTime";
            this.chkShowDateTime.Size = new System.Drawing.Size(122, 17);
            this.chkShowDateTime.TabIndex = 3;
            this.chkShowDateTime.Text = "Mostrar Data e Hora";
            this.chkShowDateTime.UseVisualStyleBackColor = true;
            // 
            // cboSelStamp
            // 
            this.cboSelStamp.Enabled = false;
            this.cboSelStamp.FormattingEnabled = true;
            this.cboSelStamp.Location = new System.Drawing.Point(6, 56);
            this.cboSelStamp.Name = "cboSelStamp";
            this.cboSelStamp.Size = new System.Drawing.Size(264, 21);
            this.cboSelStamp.TabIndex = 2;
            this.cboSelStamp.SelectedIndexChanged += new System.EventHandler(this.cboSelStamp_SelectedIndexChanged);
            this.cboSelStamp.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboSelStamp_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Opções de Assinatura:";
            // 
            // chkShowUserNameDigSig
            // 
            this.chkShowUserNameDigSig.AutoSize = true;
            this.chkShowUserNameDigSig.Checked = true;
            this.chkShowUserNameDigSig.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowUserNameDigSig.Location = new System.Drawing.Point(416, 13);
            this.chkShowUserNameDigSig.Name = "chkShowUserNameDigSig";
            this.chkShowUserNameDigSig.Size = new System.Drawing.Size(15, 14);
            this.chkShowUserNameDigSig.TabIndex = 8;
            this.chkShowUserNameDigSig.UseVisualStyleBackColor = true;
            // 
            // Form_SignConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(448, 241);
            this.Controls.Add(this.chkShowUserNameDigSig);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirmConfigSigs);
            this.Controls.Add(this.cboCertificatesList);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSignerName);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_SignConfig";
            this.Text = "SignatorDoc - Configurações de Assinatura";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_SignConfig_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSignerName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboCertificatesList;
        private System.Windows.Forms.Button btnConfirmConfigSigs;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboSelStamp;
        private System.Windows.Forms.CheckBox chkShowUserNameDigSig;
        private System.Windows.Forms.CheckBox chkShowHash;
        private System.Windows.Forms.CheckBox chkShowDateTime;
        private System.Windows.Forms.Label lblDeviceNotAvailable;
        private System.Windows.Forms.CheckBox chkHandWritten;
        private System.Windows.Forms.CheckBox chkStamp;
        private System.Windows.Forms.CheckBox chkVisible;

        //protected override void WndProc(ref System.Windows.Forms.Message message)
        //{
        //    const int WM_SYSCOMMAND = 0x0112;  // System command
        //    const int SC_MOVE = 0xF010;        // Moves the window    

        //    if (message.Msg == WM_SYSCOMMAND)
        //    {
        //        // To obtain the correct result when testing the value of wParam, 
        //        // an application must combine the value 0xFFF0 with the wParam 
        //        // value by using the bitwise AND operator. 
        //        int command = message.WParam.ToInt32() & 0xFFF0;

        //        if (command == SC_MOVE)
        //            return; // disallow window to move           
        //    }

        //    base.WndProc(ref message);
        //}

    }
}