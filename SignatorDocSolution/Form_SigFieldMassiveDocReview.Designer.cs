namespace SignatorDocSolution
{
    partial class Form_SigFieldMassiveDocReview
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
            this.lstB_Docs = new System.Windows.Forms.ListBox();
            this.chkL_SigFieldsInf = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_Confirm = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.btn_selectAll = new System.Windows.Forms.Button();
            this.btn_ClearAll = new System.Windows.Forms.Button();
            this.btn_ClearAllDocs = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstB_Docs
            // 
            this.lstB_Docs.FormattingEnabled = true;
            this.lstB_Docs.Location = new System.Drawing.Point(15, 25);
            this.lstB_Docs.Name = "lstB_Docs";
            this.lstB_Docs.Size = new System.Drawing.Size(299, 316);
            this.lstB_Docs.TabIndex = 0;
            this.lstB_Docs.SelectedIndexChanged += new System.EventHandler(this.lstB_Docs_SelectedIndexChanged);
            // 
            // chkL_SigFieldsInf
            // 
            this.chkL_SigFieldsInf.CheckOnClick = true;
            this.chkL_SigFieldsInf.FormattingEnabled = true;
            this.chkL_SigFieldsInf.Location = new System.Drawing.Point(321, 25);
            this.chkL_SigFieldsInf.Name = "chkL_SigFieldsInf";
            this.chkL_SigFieldsInf.Size = new System.Drawing.Size(269, 259);
            this.chkL_SigFieldsInf.TabIndex = 1;
            this.chkL_SigFieldsInf.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chkL_SigFieldsInf_ItemCheck);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Documentos";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(318, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Assinaturas";
            // 
            // btn_Confirm
            // 
            this.btn_Confirm.Location = new System.Drawing.Point(515, 318);
            this.btn_Confirm.Name = "btn_Confirm";
            this.btn_Confirm.Size = new System.Drawing.Size(75, 23);
            this.btn_Confirm.TabIndex = 4;
            this.btn_Confirm.Text = "Confirmar";
            this.btn_Confirm.UseVisualStyleBackColor = true;
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Location = new System.Drawing.Point(420, 318);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_Cancel.TabIndex = 5;
            this.btn_Cancel.Text = "Cancelar";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // btn_selectAll
            // 
            this.btn_selectAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.3F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_selectAll.Location = new System.Drawing.Point(374, 285);
            this.btn_selectAll.Name = "btn_selectAll";
            this.btn_selectAll.Size = new System.Drawing.Size(46, 18);
            this.btn_selectAll.TabIndex = 6;
            this.btn_selectAll.Text = "Todos";
            this.btn_selectAll.UseVisualStyleBackColor = true;
            this.btn_selectAll.Click += new System.EventHandler(this.btn_selectAll_Click);
            // 
            // btn_ClearAll
            // 
            this.btn_ClearAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.3F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_ClearAll.Location = new System.Drawing.Point(321, 285);
            this.btn_ClearAll.Name = "btn_ClearAll";
            this.btn_ClearAll.Size = new System.Drawing.Size(47, 18);
            this.btn_ClearAll.TabIndex = 7;
            this.btn_ClearAll.Text = "Limpar";
            this.btn_ClearAll.UseVisualStyleBackColor = true;
            this.btn_ClearAll.Click += new System.EventHandler(this.btn_ClearAll_Click);
            // 
            // btn_ClearAllDocs
            // 
            this.btn_ClearAllDocs.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.3F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_ClearAllDocs.Location = new System.Drawing.Point(15, 342);
            this.btn_ClearAllDocs.Name = "btn_ClearAllDocs";
            this.btn_ClearAllDocs.Size = new System.Drawing.Size(87, 18);
            this.btn_ClearAllDocs.TabIndex = 8;
            this.btn_ClearAllDocs.Text = "Remover Tudo";
            this.btn_ClearAllDocs.UseVisualStyleBackColor = true;
            this.btn_ClearAllDocs.Click += new System.EventHandler(this.btn_ClearAllDocs_Click);
            // 
            // Form_SigFieldMassiveDocReview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(607, 367);
            this.Controls.Add(this.btn_ClearAllDocs);
            this.Controls.Add(this.btn_ClearAll);
            this.Controls.Add(this.btn_selectAll);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_Confirm);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkL_SigFieldsInf);
            this.Controls.Add(this.lstB_Docs);
            this.Name = "Form_SigFieldMassiveDocReview";
            this.Text = "SignatorDoc - Revisão de Campos de Assinaturas em Lote";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_SigFieldMassiveDocReview_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstB_Docs;
        private System.Windows.Forms.CheckedListBox chkL_SigFieldsInf;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_Confirm;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.Button btn_selectAll;
        private System.Windows.Forms.Button btn_ClearAll;
        private System.Windows.Forms.Button btn_ClearAllDocs;
    }
}