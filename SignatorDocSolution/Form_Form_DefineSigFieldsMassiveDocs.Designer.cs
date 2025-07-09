namespace SignatorDocSolution
{
    partial class Form_DefineSigFieldsMassiveDocs
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
            this.chkLPages = new System.Windows.Forms.CheckedListBox();
            this.chkLCorfirmedSigFields = new System.Windows.Forms.CheckedListBox();
            this.chkLSigFields = new System.Windows.Forms.CheckedListBox();
            this.lstBDocuments = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.chkSelAllLastDocPages = new System.Windows.Forms.CheckBox();
            this.btn_CheckAllPages = new System.Windows.Forms.Button();
            this.btn_ClearAllPages = new System.Windows.Forms.Button();
            this.btn_CheckAllSigFields = new System.Windows.Forms.Button();
            this.btn_ClearAllSigFields = new System.Windows.Forms.Button();
            this.btn_ClearWholeConfig = new System.Windows.Forms.Button();
            this.btn_ConfirmSigFields = new System.Windows.Forms.Button();
            this.btn_Exit = new System.Windows.Forms.Button();
            this.btn_ConfirmPageDocsFields = new System.Windows.Forms.Button();
            this.btn_SelectAllFieldsDefs = new System.Windows.Forms.Button();
            this.btn_ClearAllFieldsDefs = new System.Windows.Forms.Button();
            this.btn_SigDefsRemove = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chkLPages
            // 
            this.chkLPages.CheckOnClick = true;
            this.chkLPages.FormattingEnabled = true;
            this.chkLPages.Location = new System.Drawing.Point(313, 25);
            this.chkLPages.Name = "chkLPages";
            this.chkLPages.Size = new System.Drawing.Size(184, 184);
            this.chkLPages.TabIndex = 0;
            this.chkLPages.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chkLPages_ItemCheck);
            // 
            // chkLCorfirmedSigFields
            // 
            this.chkLCorfirmedSigFields.CheckOnClick = true;
            this.chkLCorfirmedSigFields.FormattingEnabled = true;
            this.chkLCorfirmedSigFields.Location = new System.Drawing.Point(15, 248);
            this.chkLCorfirmedSigFields.Name = "chkLCorfirmedSigFields";
            this.chkLCorfirmedSigFields.Size = new System.Drawing.Size(482, 199);
            this.chkLCorfirmedSigFields.TabIndex = 1;
            this.chkLCorfirmedSigFields.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chkLCorfirmedSigFields_ItemCheck);
            // 
            // chkLSigFields
            // 
            this.chkLSigFields.CheckOnClick = true;
            this.chkLSigFields.FormattingEnabled = true;
            this.chkLSigFields.Location = new System.Drawing.Point(516, 25);
            this.chkLSigFields.Name = "chkLSigFields";
            this.chkLSigFields.Size = new System.Drawing.Size(261, 184);
            this.chkLSigFields.TabIndex = 2;
            this.chkLSigFields.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chkLSigFields_ItemCheck);
            // 
            // lstBDocuments
            // 
            this.lstBDocuments.FormattingEnabled = true;
            this.lstBDocuments.Location = new System.Drawing.Point(15, 25);
            this.lstBDocuments.Name = "lstBDocuments";
            this.lstBDocuments.Size = new System.Drawing.Size(281, 186);
            this.lstBDocuments.TabIndex = 3;
            this.lstBDocuments.SelectedIndexChanged += new System.EventHandler(this.lstBDocuments_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Lista de Documentos";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(313, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Páginas do Documento";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(513, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(218, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Campos de Assinatura";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 232);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(159, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Campos de Assinatura Adicionados";
            // 
            // chkSelAllLastDocPages
            // 
            this.chkSelAllLastDocPages.AutoSize = true;
            this.chkSelAllLastDocPages.Location = new System.Drawing.Point(516, 391);
            this.chkSelAllLastDocPages.Name = "chkSelAllLastDocPages";
            this.chkSelAllLastDocPages.Size = new System.Drawing.Size(216, 17);
            this.chkSelAllLastDocPages.TabIndex = 14;
            this.chkSelAllLastDocPages.Text = "Última Página de Todos os Documentos";
            this.chkSelAllLastDocPages.UseVisualStyleBackColor = true;
            this.chkSelAllLastDocPages.CheckedChanged += new System.EventHandler(this.chkSelAllLastDocPages_CheckedChanged);
            // 
            // btn_CheckAllPages
            // 
            this.btn_CheckAllPages.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_CheckAllPages.Location = new System.Drawing.Point(313, 211);
            this.btn_CheckAllPages.Name = "btn_CheckAllPages";
            this.btn_CheckAllPages.Size = new System.Drawing.Size(44, 20);
            this.btn_CheckAllPages.TabIndex = 15;
            this.btn_CheckAllPages.Text = "Todas";
            this.btn_CheckAllPages.UseVisualStyleBackColor = true;
            this.btn_CheckAllPages.Click += new System.EventHandler(this.btn_CheckAllPages_Click);
            // 
            // btn_ClearAllPages
            // 
            this.btn_ClearAllPages.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_ClearAllPages.Location = new System.Drawing.Point(363, 211);
            this.btn_ClearAllPages.Name = "btn_ClearAllPages";
            this.btn_ClearAllPages.Size = new System.Drawing.Size(49, 20);
            this.btn_ClearAllPages.TabIndex = 16;
            this.btn_ClearAllPages.Text = "Limpar";
            this.btn_ClearAllPages.UseVisualStyleBackColor = true;
            this.btn_ClearAllPages.Click += new System.EventHandler(this.btn_ClearAllPages_Click);
            // 
            // btn_CheckAllSigFields
            // 
            this.btn_CheckAllSigFields.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_CheckAllSigFields.Location = new System.Drawing.Point(516, 211);
            this.btn_CheckAllSigFields.Name = "btn_CheckAllSigFields";
            this.btn_CheckAllSigFields.Size = new System.Drawing.Size(47, 20);
            this.btn_CheckAllSigFields.TabIndex = 17;
            this.btn_CheckAllSigFields.Text = "Todos";
            this.btn_CheckAllSigFields.UseVisualStyleBackColor = true;
            this.btn_CheckAllSigFields.Click += new System.EventHandler(this.btn_CheckAllSigFields_Click);
            // 
            // btn_ClearAllSigFields
            // 
            this.btn_ClearAllSigFields.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_ClearAllSigFields.Location = new System.Drawing.Point(569, 211);
            this.btn_ClearAllSigFields.Name = "btn_ClearAllSigFields";
            this.btn_ClearAllSigFields.Size = new System.Drawing.Size(51, 20);
            this.btn_ClearAllSigFields.TabIndex = 18;
            this.btn_ClearAllSigFields.Text = "Limpar";
            this.btn_ClearAllSigFields.UseVisualStyleBackColor = true;
            this.btn_ClearAllSigFields.Click += new System.EventHandler(this.btn_ClearAllSigFields_Click);
            // 
            // btn_ClearWholeConfig
            // 
            this.btn_ClearWholeConfig.Location = new System.Drawing.Point(516, 424);
            this.btn_ClearWholeConfig.Name = "btn_ClearWholeConfig";
            this.btn_ClearWholeConfig.Size = new System.Drawing.Size(75, 23);
            this.btn_ClearWholeConfig.TabIndex = 19;
            this.btn_ClearWholeConfig.Text = "Limpar Tudo";
            this.btn_ClearWholeConfig.UseVisualStyleBackColor = true;
            this.btn_ClearWholeConfig.Click += new System.EventHandler(this.btn_ClearWholeConfig_Click);
            // 
            // btn_ConfirmSigFields
            // 
            this.btn_ConfirmSigFields.Enabled = false;
            this.btn_ConfirmSigFields.Location = new System.Drawing.Point(597, 424);
            this.btn_ConfirmSigFields.Name = "btn_ConfirmSigFields";
            this.btn_ConfirmSigFields.Size = new System.Drawing.Size(75, 23);
            this.btn_ConfirmSigFields.TabIndex = 20;
            this.btn_ConfirmSigFields.Text = "Confirmar";
            this.btn_ConfirmSigFields.UseVisualStyleBackColor = true;
            // 
            // btn_Exit
            // 
            this.btn_Exit.Location = new System.Drawing.Point(702, 424);
            this.btn_Exit.Name = "btn_Exit";
            this.btn_Exit.Size = new System.Drawing.Size(75, 23);
            this.btn_Exit.TabIndex = 21;
            this.btn_Exit.Text = "Sair";
            this.btn_Exit.UseVisualStyleBackColor = true;
            this.btn_Exit.Click += new System.EventHandler(this.btn_Exit_Click);
            // 
            // btn_ConfirmPageDocsFields
            // 
            this.btn_ConfirmPageDocsFields.Enabled = false;
            this.btn_ConfirmPageDocsFields.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_ConfirmPageDocsFields.Location = new System.Drawing.Point(516, 248);
            this.btn_ConfirmPageDocsFields.Name = "btn_ConfirmPageDocsFields";
            this.btn_ConfirmPageDocsFields.Size = new System.Drawing.Size(67, 20);
            this.btn_ConfirmPageDocsFields.TabIndex = 24;
            this.btn_ConfirmPageDocsFields.Text = "Adicionar";
            this.btn_ConfirmPageDocsFields.UseVisualStyleBackColor = true;
            this.btn_ConfirmPageDocsFields.Click += new System.EventHandler(this.btn_ConfirmPageDocsFields_Click);
            // 
            // btn_SelectAllFieldsDefs
            // 
            this.btn_SelectAllFieldsDefs.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_SelectAllFieldsDefs.Location = new System.Drawing.Point(15, 449);
            this.btn_SelectAllFieldsDefs.Name = "btn_SelectAllFieldsDefs";
            this.btn_SelectAllFieldsDefs.Size = new System.Drawing.Size(44, 20);
            this.btn_SelectAllFieldsDefs.TabIndex = 25;
            this.btn_SelectAllFieldsDefs.Text = "Todos";
            this.btn_SelectAllFieldsDefs.UseVisualStyleBackColor = true;
            this.btn_SelectAllFieldsDefs.Click += new System.EventHandler(this.btn_SelectAllFieldsDefs_Click);
            // 
            // btn_ClearAllFieldsDefs
            // 
            this.btn_ClearAllFieldsDefs.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_ClearAllFieldsDefs.Location = new System.Drawing.Point(65, 449);
            this.btn_ClearAllFieldsDefs.Name = "btn_ClearAllFieldsDefs";
            this.btn_ClearAllFieldsDefs.Size = new System.Drawing.Size(49, 20);
            this.btn_ClearAllFieldsDefs.TabIndex = 26;
            this.btn_ClearAllFieldsDefs.Text = "Limpar";
            this.btn_ClearAllFieldsDefs.UseVisualStyleBackColor = true;
            this.btn_ClearAllFieldsDefs.Click += new System.EventHandler(this.btn_ClearAllFieldsDefs_Click);
            // 
            // btn_SigDefsRemove
            // 
            this.btn_SigDefsRemove.Enabled = false;
            this.btn_SigDefsRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_SigDefsRemove.Location = new System.Drawing.Point(516, 274);
            this.btn_SigDefsRemove.Name = "btn_SigDefsRemove";
            this.btn_SigDefsRemove.Size = new System.Drawing.Size(65, 20);
            this.btn_SigDefsRemove.TabIndex = 27;
            this.btn_SigDefsRemove.Text = "Remover";
            this.btn_SigDefsRemove.UseVisualStyleBackColor = true;
            this.btn_SigDefsRemove.Click += new System.EventHandler(this.btn_SigDefsRemove_Click);
            // 
            // Form_DefineSigFieldsMassiveDocs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 474);
            this.Controls.Add(this.btn_SigDefsRemove);
            this.Controls.Add(this.btn_ClearAllFieldsDefs);
            this.Controls.Add(this.btn_SelectAllFieldsDefs);
            this.Controls.Add(this.btn_ConfirmPageDocsFields);
            this.Controls.Add(this.btn_Exit);
            this.Controls.Add(this.btn_ConfirmSigFields);
            this.Controls.Add(this.btn_ClearWholeConfig);
            this.Controls.Add(this.btn_ClearAllSigFields);
            this.Controls.Add(this.btn_CheckAllSigFields);
            this.Controls.Add(this.btn_ClearAllPages);
            this.Controls.Add(this.btn_CheckAllPages);
            this.Controls.Add(this.chkSelAllLastDocPages);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstBDocuments);
            this.Controls.Add(this.chkLSigFields);
            this.Controls.Add(this.chkLCorfirmedSigFields);
            this.Controls.Add(this.chkLPages);
            this.Name = "Form_DefineSigFieldsMassiveDocs";
            this.Text = "SignatorDoc - Definição de Campos de Assinatura em Lote";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_DefineSigFieldsMassiveDocs_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        

        #endregion

        private System.Windows.Forms.CheckedListBox chkLPages;
        private System.Windows.Forms.CheckedListBox chkLCorfirmedSigFields;
        private System.Windows.Forms.CheckedListBox chkLSigFields;
        private System.Windows.Forms.ListBox lstBDocuments;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkSelAllLastDocPages;
        private System.Windows.Forms.Button btn_CheckAllPages;
        private System.Windows.Forms.Button btn_ClearAllPages;
        private System.Windows.Forms.Button btn_CheckAllSigFields;
        private System.Windows.Forms.Button btn_ClearAllSigFields;
        private System.Windows.Forms.Button btn_ClearWholeConfig;
        private System.Windows.Forms.Button btn_ConfirmSigFields;
        private System.Windows.Forms.Button btn_Exit;
        private System.Windows.Forms.Button btn_ConfirmPageDocsFields;
        private System.Windows.Forms.Button btn_SelectAllFieldsDefs;
        private System.Windows.Forms.Button btn_ClearAllFieldsDefs;
        private System.Windows.Forms.Button btn_SigDefsRemove;
    }
}