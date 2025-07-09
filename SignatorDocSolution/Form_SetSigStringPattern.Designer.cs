namespace SignatorDocSolution
{
    partial class Form_SetSigStringPattern
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
            if (this.frm_SetPreSigFieldConfig != null)
                this.frm_SetPreSigFieldConfig.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txt_PatternSearch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_SearchPattern = new System.Windows.Forms.Button();
            this.chkL_FoundPatterns = new System.Windows.Forms.CheckedListBox();
            this.btn_Confirm = new System.Windows.Forms.Button();
            this.pnl_BodyMain = new System.Windows.Forms.Panel();
            this.btn_Exit = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsStatusLbl_Info = new System.Windows.Forms.ToolStripStatusLabel();
            this.chk_SetSigFieldConfig = new System.Windows.Forms.CheckBox();
            this.loadingPanel = new SignatorDocSolution.Utils.LoadingPanel();
            this.frm_SetPreSigFieldConfig = new SignatorDocSolution.Form_SetPreSigFieldConfig();
            this.btn_SelectAll = new System.Windows.Forms.Button();
            this.btn_Clear = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.pnl_BodyMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt_PatternSearch
            // 
            this.txt_PatternSearch.Location = new System.Drawing.Point(12, 29);
            this.txt_PatternSearch.Name = "txt_PatternSearch";
            this.txt_PatternSearch.Size = new System.Drawing.Size(275, 20);
            this.txt_PatternSearch.TabIndex = 0;
            this.txt_PatternSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txt_PatternSearch_KeyUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(214, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Digite a Palavra-Chave:";
            // 
            // btn_SearchPattern
            // 
            this.btn_SearchPattern.Enabled = false;
            this.btn_SearchPattern.Location = new System.Drawing.Point(293, 26);
            this.btn_SearchPattern.Name = "btn_SearchPattern";
            this.btn_SearchPattern.Size = new System.Drawing.Size(87, 23);
            this.btn_SearchPattern.TabIndex = 2;
            this.btn_SearchPattern.Text = "Procurar";
            this.btn_SearchPattern.UseVisualStyleBackColor = true;
            this.btn_SearchPattern.Click += new System.EventHandler(this.btn_SearchPattern_Click);
            // 
            // chkL_FoundPatterns
            // 
            this.chkL_FoundPatterns.CheckOnClick = true;
            this.chkL_FoundPatterns.FormattingEnabled = true;
            this.chkL_FoundPatterns.HorizontalScrollbar = true;
            this.chkL_FoundPatterns.Location = new System.Drawing.Point(12, 55);
            this.chkL_FoundPatterns.Name = "chkL_FoundPatterns";
            this.chkL_FoundPatterns.Size = new System.Drawing.Size(365, 184);
            this.chkL_FoundPatterns.TabIndex = 3;
            this.chkL_FoundPatterns.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chkL_FoundPatterns_ItemCheck);
            // 
            // btn_Confirm
            // 
            this.btn_Confirm.Enabled = false;
            this.btn_Confirm.Location = new System.Drawing.Point(200, 270);
            this.btn_Confirm.Name = "btn_Confirm";
            this.btn_Confirm.Size = new System.Drawing.Size(100, 23);
            this.btn_Confirm.TabIndex = 4;
            this.btn_Confirm.Text = "Confirmar Campos";
            this.btn_Confirm.UseVisualStyleBackColor = true;
            //
            // loadingPanel
            //
            this.loadingPanel.Visible = false;
            // 
            // btn_Exit
            // 
            this.btn_Exit.Location = new System.Drawing.Point(305, 270);
            this.btn_Exit.Name = "btn_Exit";
            this.btn_Exit.Size = new System.Drawing.Size(75, 23);
            this.btn_Exit.TabIndex = 5;
            this.btn_Exit.Text = "Sair";
            this.btn_Exit.UseVisualStyleBackColor = true;
            this.btn_Exit.Click += new System.EventHandler(this.btn_Exit_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsStatusLbl_Info});
            this.statusStrip1.Location = new System.Drawing.Point(0, 299);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(389, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsStatusLbl_Info
            // 
            this.tsStatusLbl_Info.Name = "tsStatusLbl_Info";
            this.tsStatusLbl_Info.Size = new System.Drawing.Size(0, 17);
            // 
            // chk_SetSigFieldConfig
            // 
            this.chk_SetSigFieldConfig.AutoSize = true;
            this.chk_SetSigFieldConfig.Location = new System.Drawing.Point(12, 274);
            this.chk_SetSigFieldConfig.Name = "chk_SetSigFieldConfig";
            this.chk_SetSigFieldConfig.Size = new System.Drawing.Size(175, 17);
            this.chk_SetSigFieldConfig.TabIndex = 7;
            this.chk_SetSigFieldConfig.Text = "Assinar Todos os Campos";
            this.chk_SetSigFieldConfig.UseVisualStyleBackColor = true;
            this.chk_SetSigFieldConfig.CheckedChanged += new System.EventHandler(this.chk_SetSigFieldConfig_CheckedChanged);
            // 
            // frm_SetPreSigFieldConfig
            // 
            this.frm_SetPreSigFieldConfig.BackColor = System.Drawing.Color.White;
            this.frm_SetPreSigFieldConfig.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.frm_SetPreSigFieldConfig.Left = (int)((SignatorDocSolution.Utils.Defins.rectPrimaryScreen.Width * 0.35F)+40);
            this.frm_SetPreSigFieldConfig.Top = (int)((SignatorDocSolution.Utils.Defins.rectPrimaryScreen.Height * 0.20F)+50);
            this.frm_SetPreSigFieldConfig.MaximizeBox = false;
            this.frm_SetPreSigFieldConfig.MinimizeBox = false;
            this.frm_SetPreSigFieldConfig.Name = "frm_SetPreSigFieldConfig";
            this.frm_SetPreSigFieldConfig.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.frm_SetPreSigFieldConfig.Text = "SignatorDoc - Configuração de Assinatura";
            this.frm_SetPreSigFieldConfig.Visible = false;
            this.frm_SetPreSigFieldConfig.setAfterSigFieldConfigEvent(new SignatorDocSolution.Utils.CustomEvent.GenericDelegateEventHandler(AfterSerchPatternConfigSigField_Event));
            this.frm_SetPreSigFieldConfig.setCloseSigStringPatternEvent(new SignatorDocSolution.Utils.CustomEvent.GenericDelegateEventHandler(closeByExtern_Event));

            // 
            // btn_SelectAll
            // 
            this.btn_SelectAll.Enabled = false;
            this.btn_SelectAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_SelectAll.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_SelectAll.Location = new System.Drawing.Point(12, 243);
            this.btn_SelectAll.Margin = new System.Windows.Forms.Padding(1);
            this.btn_SelectAll.Name = "btn_SelectAll";
            this.btn_SelectAll.Size = new System.Drawing.Size(43, 20);
            this.btn_SelectAll.TabIndex = 8;
            this.btn_SelectAll.Text = "Todos";
            this.btn_SelectAll.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_SelectAll.UseVisualStyleBackColor = true;
            this.btn_SelectAll.Click += new System.EventHandler(this.btn_SelectAll_Click);
            // 
            // btn_Clear
            // 
            this.btn_Clear.Enabled = false;
            this.btn_Clear.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Clear.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_Clear.Location = new System.Drawing.Point(57, 243);
            this.btn_Clear.Margin = new System.Windows.Forms.Padding(1);
            this.btn_Clear.Name = "btn_Clear";
            this.btn_Clear.Size = new System.Drawing.Size(47, 20);
            this.btn_Clear.TabIndex = 9;
            this.btn_Clear.Text = "Limpar";
            this.btn_Clear.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_Clear.UseVisualStyleBackColor = true;
            this.btn_Clear.Click += new System.EventHandler(this.btn_Clear_Click);
            //
            // pnl_BodyMain
            //
            this.pnl_BodyMain.Size = new System.Drawing.Size(380, 290);
            this.pnl_BodyMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_BodyMain.Location = new System.Drawing.Point(0,0);
            this.pnl_BodyMain.Controls.Add(this.loadingPanel);
            this.pnl_BodyMain.Controls.Add(this.btn_Clear);
            this.pnl_BodyMain.Controls.Add(this.btn_SelectAll);
            this.pnl_BodyMain.Controls.Add(this.chk_SetSigFieldConfig);
            this.pnl_BodyMain.Controls.Add(this.statusStrip1);
            this.pnl_BodyMain.Controls.Add(this.btn_Exit);
            this.pnl_BodyMain.Controls.Add(this.btn_Confirm);
            this.pnl_BodyMain.Controls.Add(this.chkL_FoundPatterns);
            this.pnl_BodyMain.Controls.Add(this.btn_SearchPattern);
            this.pnl_BodyMain.Controls.Add(this.label1);
            this.pnl_BodyMain.Controls.Add(this.txt_PatternSearch);
            // 
            // Form_SetSigStringPattern
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 321);
            this.Controls.Add(this.pnl_BodyMain);
            this.Name = "Form_SetSigStringPattern";
            this.Text = "SignatorDoc - Procura de Palavra-Chave";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_SetSigStringPattern_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.pnl_BodyMain.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private System.Windows.Forms.Panel pnl_BodyMain;
        private System.Windows.Forms.TextBox txt_PatternSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_SearchPattern;
        private System.Windows.Forms.CheckedListBox chkL_FoundPatterns;
        private System.Windows.Forms.Button btn_Confirm;
        private System.Windows.Forms.Button btn_Exit;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsStatusLbl_Info;
        private System.Windows.Forms.CheckBox chk_SetSigFieldConfig;
        private Form_SetPreSigFieldConfig frm_SetPreSigFieldConfig;
        private System.Windows.Forms.Button btn_SelectAll;
        private System.Windows.Forms.Button btn_Clear;
        private SignatorDocSolution.Utils.CustomEvent AfterSSerchPatternConfigSigField;
        private SignatorDocSolution.Utils.LoadingPanel loadingPanel;
    }
}