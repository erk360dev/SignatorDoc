namespace SignatorDocSolution
{
    partial class Form_ReplicateSigField
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
            this.lstB_SigFieldPagesReview = new System.Windows.Forms.ListBox();
            this.btn_ReplicateConfirm = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_Replicate = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btn_ClearSigFieldListReview = new System.Windows.Forms.Button();
            this.btn_SelectAllPages = new System.Windows.Forms.Button();
            this.chkL_SigFields = new System.Windows.Forms.CheckedListBox();
            this.chkL_Pages = new System.Windows.Forms.CheckedListBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // lstB_SigFieldPagesReview
            // 
            this.lstB_SigFieldPagesReview.FormattingEnabled = true;
            this.lstB_SigFieldPagesReview.Location = new System.Drawing.Point(15, 262);
            this.lstB_SigFieldPagesReview.Name = "lstB_SigFieldPagesReview";
            this.lstB_SigFieldPagesReview.Size = new System.Drawing.Size(311, 108);
            this.lstB_SigFieldPagesReview.TabIndex = 5;
            // 
            // btn_ReplicateConfirm
            // 
            this.btn_ReplicateConfirm.Enabled = false;
            this.btn_ReplicateConfirm.Location = new System.Drawing.Point(255, 400);
            this.btn_ReplicateConfirm.Name = "btn_ReplicateConfirm";
            this.btn_ReplicateConfirm.Size = new System.Drawing.Size(71, 23);
            this.btn_ReplicateConfirm.TabIndex = 6;
            this.btn_ReplicateConfirm.Text = "Confirmar";
            this.btn_ReplicateConfirm.UseVisualStyleBackColor = true;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Location = new System.Drawing.Point(165, 400);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_Cancel.TabIndex = 7;
            this.btn_Cancel.Text = "Cancelar";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Sair_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(159, 16);
            this.label1.TabIndex = 8;
            this.label1.Text = "Campos de Assinatura Criados";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(263, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Páginas";
            // 
            // btn_Replicate
            // 
            this.btn_Replicate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Replicate.Location = new System.Drawing.Point(12, 229);
            this.btn_Replicate.Name = "btn_Replicate";
            this.btn_Replicate.Size = new System.Drawing.Size(314, 28);
            this.btn_Replicate.TabIndex = 11;
            this.btn_Replicate.Text = "Confirmar Assinaturas";
            this.btn_Replicate.UseVisualStyleBackColor = true;
            this.btn_Replicate.Click += new System.EventHandler(this.btn_Replicate_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.3F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(15, 373);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 12);
            this.label4.TabIndex = 12;
            this.label4.Text = "* H - Posição Horizontal";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.3F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(123, 373);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 12);
            this.label5.TabIndex = 13;
            this.label5.Text = "* V - Posição Vertical";
            // 
            // btn_ClearSigFieldListReview
            // 
            this.btn_ClearSigFieldListReview.Enabled = false;
            this.btn_ClearSigFieldListReview.Location = new System.Drawing.Point(12, 400);
            this.btn_ClearSigFieldListReview.Name = "btn_ClearSigFieldListReview";
            this.btn_ClearSigFieldListReview.Size = new System.Drawing.Size(56, 23);
            this.btn_ClearSigFieldListReview.TabIndex = 14;
            this.btn_ClearSigFieldListReview.Text = "Limpar";
            this.btn_ClearSigFieldListReview.UseVisualStyleBackColor = true;
            this.btn_ClearSigFieldListReview.Click += new System.EventHandler(this.btn_ClearSigFieldListReview_Click);
            // 
            // btn_SelectAllPages
            // 
            this.btn_SelectAllPages.Enabled = false;
            this.btn_SelectAllPages.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_SelectAllPages.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_SelectAllPages.Location = new System.Drawing.Point(266, 200);
            this.btn_SelectAllPages.Margin = new System.Windows.Forms.Padding(1);
            this.btn_SelectAllPages.Name = "btn_SelectAllPages";
            this.btn_SelectAllPages.Size = new System.Drawing.Size(43, 20);
            this.btn_SelectAllPages.TabIndex = 8;
            this.btn_SelectAllPages.Text = "Todos";
            this.btn_SelectAllPages.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btn_SelectAllPages.UseVisualStyleBackColor = true;
            this.btn_SelectAllPages.Click+= new System.EventHandler (this.btn_SelectAllPages_Click);
            // 
            // chkL_SigFields
            // 
            this.chkL_SigFields.CheckOnClick = true;
            this.chkL_SigFields.FormattingEnabled = true;
            this.chkL_SigFields.Location = new System.Drawing.Point(15, 45);
            this.chkL_SigFields.Name = "chkL_SigFields";
            this.chkL_SigFields.Size = new System.Drawing.Size(245, 154);
            this.chkL_SigFields.TabIndex = 15;
            this.chkL_SigFields.ItemCheck+= new System.Windows.Forms.ItemCheckEventHandler(chkL_SigFields_ItemCheck);
            // 
            // chkL_Pages
            // 
            this.chkL_Pages.CheckOnClick = true;
            this.chkL_Pages.FormattingEnabled = true;
            this.chkL_Pages.Location = new System.Drawing.Point(266, 45);
            this.chkL_Pages.Name = "chkL_Pages";
            this.chkL_Pages.Size = new System.Drawing.Size(60, 154);
            this.chkL_Pages.TabIndex = 16;
            // 
            // Form_ReplicateSigField
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 432);
            this.Controls.Add(this.chkL_Pages);
            this.Controls.Add(this.chkL_SigFields);
            this.Controls.Add(this.btn_ClearSigFieldListReview);
            this.Controls.Add(this.btn_SelectAllPages);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btn_Replicate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_ReplicateConfirm);
            this.Controls.Add(this.lstB_SigFieldPagesReview);
            this.Name = "Form_ReplicateSigField";
            this.Text = "SignatorDoc - Replicar Campos de Assinatura";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_ReplicateSigField_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstB_SigFieldPagesReview;
        private System.Windows.Forms.Button btn_ReplicateConfirm;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_Replicate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn_ClearSigFieldListReview;
        private System.Windows.Forms.Button btn_SelectAllPages;
        private System.Windows.Forms.CheckedListBox chkL_SigFields;
        private System.Windows.Forms.CheckedListBox chkL_Pages;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}