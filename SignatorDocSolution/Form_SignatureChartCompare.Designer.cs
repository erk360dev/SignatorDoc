namespace SignatorDocSolution
{
    partial class Form_SignatureChartCompare
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
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lbl_SignerName = new System.Windows.Forms.Label();
            this.lbl_CertUsed = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lbl_Pressure = new System.Windows.Forms.Label();
            this.lbl_TotalTime = new System.Windows.Forms.Label();
            this.lbl_SigFieldDisabled = new System.Windows.Forms.Label();
            this.lstB_Signatures = new System.Windows.Forms.ListBox();
            this.pbDoneSignatureChartBio = new System.Windows.Forms.PictureBox();
            this.pbCurrSigBioChart = new System.Windows.Forms.PictureBox();
            this.pnlSignSigField = new System.Windows.Forms.Panel();
            this.signatureField = new SignatureField(new SignatorDocSolution.Utils.CustomEvent.GenericDelegateEventHandler(SignatureField_AfterSign));
            this.btn_Exit = new System.Windows.Forms.Button();
            this.grpB_Infs = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbDoneSignatureChartBio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCurrSigBioChart)).BeginInit();
            this.grpB_Infs.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(482, 11);
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Assinaturas";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(7, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Nome do Signatário:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(7, 172);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(163, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Tempo Total da Assinatura:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(7, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Versão SignatorDoc:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(6, 135);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Pressão Máxima:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(5, 61);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(125, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Certificado Utilizado:";
            // 
            // lbl_SignerName
            // 
            this.lbl_SignerName.Location = new System.Drawing.Point(127, 24);
            this.lbl_SignerName.Name = "lbl_SignerName";
            this.lbl_SignerName.Size = new System.Drawing.Size(173, 13);
            this.lbl_SignerName.TabIndex = 14;
            // 
            // lbl_CertUsed
            // 
            this.lbl_CertUsed.Location = new System.Drawing.Point(127, 61);
            this.lbl_CertUsed.Name = "lbl_CertUsed";
            this.lbl_CertUsed.Size = new System.Drawing.Size(173, 13);
            this.lbl_CertUsed.TabIndex = 15;
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(94, 96);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.Size = new System.Drawing.Size(0, 13);
            this.lblVersion.TabIndex = 16;
            // 
            // lbl_Pressure
            // 
            this.lbl_Pressure.AutoSize = true;
            this.lbl_Pressure.Location = new System.Drawing.Point(106, 133);
            this.lbl_Pressure.Name = "lbl_Pressure";
            this.lbl_Pressure.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Pressure.Size = new System.Drawing.Size(0, 13);
            this.lbl_Pressure.TabIndex = 17;
            // 
            // lbl_TotalTime
            // 
            this.lbl_TotalTime.AutoSize = true;
            this.lbl_TotalTime.Location = new System.Drawing.Point(169, 170);
            this.lbl_TotalTime.Name = "lbl_TotalTime";
            this.lbl_TotalTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_TotalTime.Size = new System.Drawing.Size(0, 13);
            this.lbl_TotalTime.TabIndex = 18;
            // 
            // grpB_Infs
            // 
            this.grpB_Infs.Controls.Add(this.label1);
            this.grpB_Infs.Controls.Add(this.lbl_TotalTime);
            this.grpB_Infs.Controls.Add(this.label3);
            this.grpB_Infs.Controls.Add(this.lbl_Pressure);
            this.grpB_Infs.Controls.Add(this.label4);
            this.grpB_Infs.Controls.Add(this.lblVersion);
            this.grpB_Infs.Controls.Add(this.label5);
            this.grpB_Infs.Controls.Add(this.lbl_CertUsed);
            this.grpB_Infs.Controls.Add(this.label7);
            this.grpB_Infs.Controls.Add(this.lbl_SignerName);
            this.grpB_Infs.Location = new System.Drawing.Point(482, 138);
            this.grpB_Infs.Name = "grpB_Infs";
            this.grpB_Infs.Size = new System.Drawing.Size(320, 224);
            this.grpB_Infs.TabIndex = 19;
            this.grpB_Infs.TabStop = false;
            this.grpB_Infs.Text = "Informações";
            // 
            // lstB_Signatures
            // 
            this.lstB_Signatures.FormattingEnabled = true;
            this.lstB_Signatures.Location = new System.Drawing.Point(482, 27);
            this.lstB_Signatures.Name = "lstB_Signatures";
            this.lstB_Signatures.Size = new System.Drawing.Size(320, 95);
            this.lstB_Signatures.TabIndex = 3;
            this.lstB_Signatures.SelectedIndexChanged += new System.EventHandler(lstB_Signatures_SelectedIndexChanged);
            //
            // signatureField
            //
            this.signatureField.Location = new System.Drawing.Point(8,25);
            this.signatureField.isBioAnaliseMode = true;
            // 
            // pbDoneSignatureChartBio
            // 
            this.pbDoneSignatureChartBio.Location = new System.Drawing.Point(12, 12);
            this.pbDoneSignatureChartBio.Name = "pbDoneSignatureChartBio";
            this.pbDoneSignatureChartBio.Size = new System.Drawing.Size(450, 350);
            this.pbDoneSignatureChartBio.TabIndex = 4;
            this.pbDoneSignatureChartBio.TabStop = false;
            // 
            // pbCurrSigBioChart
            // 
            this.pbCurrSigBioChart.BackColor = System.Drawing.Color.FromArgb(255, 253, 236);
            this.pbCurrSigBioChart.Location = new System.Drawing.Point(12, 368);
            this.pbCurrSigBioChart.Name = "pbCurrSigBioChart";
            this.pbCurrSigBioChart.Size = new System.Drawing.Size(450, 257);
            this.pbCurrSigBioChart.TabIndex = 5;
            this.pbCurrSigBioChart.TabStop = false;
            // 
            // pnlSignSigField
            // 
            this.pnlSignSigField.BackColor =  System.Drawing.Color.Beige;
            this.pnlSignSigField.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlSignSigField.Location = new System.Drawing.Point(482, 368);
            this.pnlSignSigField.Name = "pnlSignSigField";
            this.pnlSignSigField.Size = new System.Drawing.Size(320, 220);
            this.pnlSignSigField.TabIndex = 6;
            this.pnlSignSigField.Controls.Add(this.signatureField);
            this.pnlSignSigField.Controls.Add(this.lbl_SigFieldDisabled);
            //
            //lbl_SigFieldDisabled
            //
            this.lbl_SigFieldDisabled.Visible=true;
            this.lbl_SigFieldDisabled.Size = new System.Drawing.Size(240, 13);
            this.lbl_SigFieldDisabled.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_SigFieldDisabled.ForeColor= System.Drawing.Color.Red;
            this.lbl_SigFieldDisabled.Name="lbl_SigFieldDisabled.";
            this.lbl_SigFieldDisabled.Location = new System.Drawing.Point(60,200);
            this.lbl_SigFieldDisabled.Text= "Dispositivo de Assinatura Não Detectado!";
            // 
            // btn_Exit
            // 
            this.btn_Exit.Location = new System.Drawing.Point(702, 602);
            this.btn_Exit.Name = "btn_Exit";
            this.btn_Exit.Size = new System.Drawing.Size(75, 23);
            this.btn_Exit.TabIndex = 7;
            this.btn_Exit.Text = "Sair";
            this.btn_Exit.UseVisualStyleBackColor = true;
            this.btn_Exit.Click += new System.EventHandler(this.btn_Exit_Click);
            // 
            // Form_SignatureChartCompare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(817, 637);
            this.Controls.Add(this.grpB_Infs);
            this.Controls.Add(this.btn_Exit);
            this.Controls.Add(this.pnlSignSigField);
            this.Controls.Add(this.pbCurrSigBioChart);
            this.Controls.Add(this.pbDoneSignatureChartBio);
            this.Controls.Add(this.lstB_Signatures);
            this.Controls.Add(this.label2);
            this.Name = "Form_SignatureChartCompare";
            this.Text = "SignatorDoc - Gráfico de Análise Biométrica de Assinatura";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_SignatureChartCompare_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pbDoneSignatureChartBio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCurrSigBioChart)).EndInit();
            this.grpB_Infs.ResumeLayout(false);
            this.grpB_Infs.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lbl_SignerName;
        private System.Windows.Forms.Label lbl_CertUsed;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lbl_Pressure;
        private System.Windows.Forms.Label lbl_TotalTime;
        private System.Windows.Forms.Label lbl_SigFieldDisabled;
        private System.Windows.Forms.ListBox lstB_Signatures;
        private System.Windows.Forms.PictureBox pbDoneSignatureChartBio;
        private System.Windows.Forms.PictureBox pbCurrSigBioChart;
        private System.Windows.Forms.Panel pnlSignSigField;
        private System.Windows.Forms.Button btn_Exit;
        private System.Windows.Forms.GroupBox grpB_Infs;
        private SignatureField signatureField;
    }
}