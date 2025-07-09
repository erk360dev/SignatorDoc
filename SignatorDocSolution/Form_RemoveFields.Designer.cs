namespace SignatorDocSolution
{
    partial class Form_RemoveFields
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
            this.chkL_FieldsPages = new System.Windows.Forms.CheckedListBox();
            this.btn_CleanRemFields = new System.Windows.Forms.Button();
            this.btn_ConfirmRemFields = new System.Windows.Forms.Button();
            this.btn_Exit = new System.Windows.Forms.Button();
            this.btn_SelectAll = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chkL_FieldsPages
            // 
            this.chkL_FieldsPages.CheckOnClick = true;
            this.chkL_FieldsPages.FormattingEnabled = true;
            this.chkL_FieldsPages.Location = new System.Drawing.Point(12, 12);
            this.chkL_FieldsPages.Name = "chkL_FieldsPages";
            this.chkL_FieldsPages.Size = new System.Drawing.Size(319, 214);
            this.chkL_FieldsPages.TabIndex = 0;
            this.chkL_FieldsPages.ItemCheck+= new System.Windows.Forms.ItemCheckEventHandler(chkL_FieldsPages_ItemCheck);
            // 
            // btn_CleanRemFields
            // 
            this.btn_CleanRemFields.Location = new System.Drawing.Point(13, 239);
            this.btn_CleanRemFields.Name = "btn_CleanRemFields";
            this.btn_CleanRemFields.Size = new System.Drawing.Size(46, 23);
            this.btn_CleanRemFields.TabIndex = 1;
            this.btn_CleanRemFields.Text = "Limpar";
            this.btn_CleanRemFields.UseVisualStyleBackColor = true;
            this.btn_CleanRemFields.Click += new System.EventHandler(this.btn_CleanRemFields_Click);
            // 
            // btn_ConfirmRemFields
            // 
            this.btn_ConfirmRemFields.Enabled = false;
            this.btn_ConfirmRemFields.Location = new System.Drawing.Point(159, 239);
            this.btn_ConfirmRemFields.Name = "btn_ConfirmRemFields";
            this.btn_ConfirmRemFields.Size = new System.Drawing.Size(91, 23);
            this.btn_ConfirmRemFields.TabIndex = 2;
            this.btn_ConfirmRemFields.Text = "Excluir Campos";
            this.btn_ConfirmRemFields.UseVisualStyleBackColor = true;
            // 
            // btn_Exit
            // 
            this.btn_Exit.Location = new System.Drawing.Point(256, 239);
            this.btn_Exit.Name = "btn_Exit";
            this.btn_Exit.Size = new System.Drawing.Size(75, 23);
            this.btn_Exit.TabIndex = 3;
            this.btn_Exit.Text = "Sair";
            this.btn_Exit.UseVisualStyleBackColor = true;
            this.btn_Exit.Click += new System.EventHandler(this.btn_Exit_Click);
            // 
            // btn_SelectAll
            // 
            this.btn_SelectAll.Location = new System.Drawing.Point(64, 239);
            this.btn_SelectAll.Name = "btn_SelectAll";
            this.btn_SelectAll.Size = new System.Drawing.Size(46, 23);
            this.btn_SelectAll.TabIndex = 4;
            this.btn_SelectAll.Text = "Todos";
            this.btn_SelectAll.UseVisualStyleBackColor = true;
            this.btn_SelectAll.Click += new System.EventHandler(this.btn_SelectAll_Click);
            // 
            // Form_RemoveFields
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 282);
            this.Controls.Add(this.btn_SelectAll);
            this.Controls.Add(this.btn_Exit);
            this.Controls.Add(this.btn_ConfirmRemFields);
            this.Controls.Add(this.btn_CleanRemFields);
            this.Controls.Add(this.chkL_FieldsPages);
            this.Name = "Form_RemoveFields";
            this.Text = "SignatorDoc - Remover Campos de Assinatura";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_RemoveSigField_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox chkL_FieldsPages;
        private System.Windows.Forms.Button btn_CleanRemFields;
        private System.Windows.Forms.Button btn_ConfirmRemFields;
        private System.Windows.Forms.Button btn_Exit;
        private System.Windows.Forms.Button btn_SelectAll;
    }
}