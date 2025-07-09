namespace SignatorDocSolution
{
    partial class Form_Main
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
                if (this.frm_SignConfig != null)
                    this.frm_SignConfig.Dispose();
                if (this.frm_ReplicSigField != null)
                    this.frm_ReplicSigField.Dispose();
                if (this.frm_RemoveSigField != null)
                    this.frm_RemoveSigField.Dispose();
                if (this.frm_SetSigStringPattern != null)
                    this.frm_SetSigStringPattern.Dispose();
                if (this.frm_Configurations != null)
                    this.frm_Configurations.Dispose();
                if (this.frm_Properties != null)
                    this.frm_Properties.Dispose();
                if (this.frm_DefineSigFieldsMassiveDocs != null)
                    this.frm_DefineSigFieldsMassiveDocs.Dispose();
                if (this.frm_SignatureChartCompare != null)
                    this.frm_SignatureChartCompare.Dispose();
                if (this.frm_SigFieldMassiveDocReview != null)
                    this.frm_SigFieldMassiveDocReview.Dispose();
                
                if (SignatorDocSolution.Utils.DetectDevice.deviceSerieNumber == 1)
                    SignatorDocSolution.Utils.DetectSTUDevice.releaseSTUResouces();
                
                try
                {
                    string emptypdf = this.localTempPath + @"\EmptyPDF.pdf";
                    foreach (string auxFileName in System.IO.Directory.GetFiles(this.localTempPath))
                    {
                        if (!auxFileName.Equals(emptypdf))
                            System.IO.File.Delete(auxFileName);
                    }

                    SignatorDocSolution.Utils.AweSomeUtils.ReleaseWhenExit();
                }
                catch (System.IO.IOException ioexc) { }
                catch (System.Exception exc) { }

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
            this.isAdbeReaderInstalled = this.checkAdbeReaderinstaled();
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Main));
            this.Mnu_Strip_Main = new System.Windows.Forms.MenuStrip();
            this.Mnu_File = new System.Windows.Forms.ToolStripMenuItem();
            this.Mnu_Item_Open = new System.Windows.Forms.ToolStripMenuItem();
            this.Mnu_Item_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.SettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salvarComoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Mnu_StripSeparator_Close = new System.Windows.Forms.ToolStripSeparator();
            this.Mnu_Item_Close = new System.Windows.Forms.ToolStripMenuItem();
            this.sobreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tlStripMenuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.removerLicencaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setFieldSearchPatternToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SigChartCheckStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnl_PDFPageViewer = new System.Windows.Forms.Panel();
            this.TBPnl_PDFTools = new System.Windows.Forms.TableLayoutPanel();
            this.TBPnl_PDFBarTools = new System.Windows.Forms.TableLayoutPanel();
            this.btn_OpenDocument = new System.Windows.Forms.Button();
            this.btn_AddSignatureField = new System.Windows.Forms.Button();
            this.btn_ReviewMassiveSignDocsFields = new System.Windows.Forms.Button();
            this.btn_SealSignature = new System.Windows.Forms.Button();
            this.btn_SaveDocument = new System.Windows.Forms.Button();
            this.btn_DoubleSignatureField = new System.Windows.Forms.Button();
            this.btn_ReplicationSignatureFields = new System.Windows.Forms.Button();
            this.btn_RemoveSignatureFields = new System.Windows.Forms.Button();
            this.tpnl_PDFNavigation = new System.Windows.Forms.TableLayoutPanel();
            this.Btn_PDFNavFirst = new System.Windows.Forms.Button();
            this.Btn_PDFNavPrevious = new System.Windows.Forms.Button();
            this.txt_PDFNavCurrPage = new System.Windows.Forms.TextBox();
            this.Btn_PDFNavNext = new System.Windows.Forms.Button();
            this.Btn_PDFNavLast = new System.Windows.Forms.Button();
            this.gpb_PDFMode = new System.Windows.Forms.GroupBox();
            this.rdo_SignMode = new System.Windows.Forms.RadioButton();
            this.pnl_PDFThumbnail = new System.Windows.Forms.Panel();
            this.tlTip_Main = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tpnl_PDFMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnl_ViewerBodyMain = new System.Windows.Forms.Panel();
            this.pnl_PDFViewAdobeMode = new System.Windows.Forms.Panel();
            this.tpnl_PDFViewerSignMode = new System.Windows.Forms.TableLayoutPanel();
            this.rdo_AdobeMode = new System.Windows.Forms.RadioButton();
            this.frm_SignConfig = new SignatorDocSolution.Form_SignConfig();
            this.frm_ReplicSigField = new Form_ReplicateSigField();
            this.frm_RemoveSigField = new Form_RemoveFields();
            this.frm_SetSigStringPattern = new Form_SetSigStringPattern();
            this.frm_Configurations = new Form_Configurations();
            this.frm_Properties = new Form_Properties();
            this.frm_DefineSigFieldsMassiveDocs =  new Form_DefineSigFieldsMassiveDocs();
            this.frm_SignatureChartCompare = new Form_SignatureChartCompare();
            this.frm_SigFieldMassiveDocReview = new Form_SigFieldMassiveDocReview();
            this.popUp = new SignatorDocSolution.PopupControl.Popup();
            this.loadingPanel = new Utils.LoadingPanel();
            this.aboutBox_Form = new SignatorDocSolution.Form_AboutBox();
            if(this.isAdbeReaderInstalled)this.axAcroPDF = new AxAcroPDFLib.AxAcroPDF();
            this.Mnu_Strip_Main.SuspendLayout();
            this.TBPnl_PDFTools.SuspendLayout();
            this.TBPnl_PDFBarTools.SuspendLayout();
            this.tpnl_PDFNavigation.SuspendLayout();
            this.gpb_PDFMode.SuspendLayout();
            this.tpnl_PDFMain.SuspendLayout();
            this.pnl_PDFViewAdobeMode.SuspendLayout();
            this.pnl_ViewerBodyMain.SuspendLayout();
            this.tpnl_PDFViewerSignMode.SuspendLayout();
            this.loadingPanel.SuspendLayout();
            if (this.isAdbeReaderInstalled)((System.ComponentModel.ISupportInitialize)(this.axAcroPDF)).BeginInit();
            this.SuspendLayout();
            // 
            // Mnu_Strip_Main
            // 
            this.Mnu_Strip_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Mnu_File,
            this.toolsToolStripMenuItem,
            this.sobreToolStripMenuItem});
            this.Mnu_Strip_Main.Location = new System.Drawing.Point(2, 2);
            this.Mnu_Strip_Main.Name = "Mnu_Strip_Main";
            this.Mnu_Strip_Main.Size = new System.Drawing.Size(738, 24);
            this.Mnu_Strip_Main.TabIndex = 0;
            this.Mnu_Strip_Main.Text = "menuStrip1";
            // 
            // Mnu_File
            // 
            this.Mnu_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Mnu_Item_Open,
            this.Mnu_Item_Save,
            this.salvarComoToolStripMenuItem,
            this.toolStripSeparator2,
            this.SettingsToolStripMenuItem,
            this.propertiesToolStripMenuItem,
            this.Mnu_StripSeparator_Close,
            this.Mnu_Item_Close});
            this.Mnu_File.Name = "Mnu_File";
            this.Mnu_File.Size = new System.Drawing.Size(61, 20);
            this.Mnu_File.Text = "Arquivo";
            // 
            // Mnu_Item_Open
            // 
            this.Mnu_Item_Open.Name = "Mnu_Item_Open";
            this.Mnu_Item_Open.Size = new System.Drawing.Size(141, 22);
            this.Mnu_Item_Open.Text = "Abrir";
            this.Mnu_Item_Open.Click += new System.EventHandler(this.abrirToolStripMenuItem_Click);
            // 
            // Mnu_Item_Save
            // 
            this.Mnu_Item_Save.Name = "Mnu_Item_Save";
            this.Mnu_Item_Save.Size = new System.Drawing.Size(141, 22);
            this.Mnu_Item_Save.Text = "Salvar";
            this.Mnu_Item_Save.Enabled = false;
            this.Mnu_Item_Save.Click += new System.EventHandler(this.Mnu_Item_Save_Click);
            // 
            // salvarComoToolStripMenuItem
            // 
            this.salvarComoToolStripMenuItem.Name = "salvarComoToolStripMenuItem";
            this.salvarComoToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.salvarComoToolStripMenuItem.Text = "Salvar Como";
            this.salvarComoToolStripMenuItem.Click += new System.EventHandler(this.salvarComoToolStripMenuItem_Click);
            //
            //
            //
            this.propertiesToolStripMenuItem.Name= "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.propertiesToolStripMenuItem.Text = "Propriedades";
            this.propertiesToolStripMenuItem.Enabled = false;
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            // 
            // Mnu_StripSeparator_Close
            // 
            this.Mnu_StripSeparator_Close.Name = "Mnu_StripSeparator_Close";
            this.Mnu_StripSeparator_Close.Size = new System.Drawing.Size(138, 6);
            // 
            // Mnu_Item_Close
            // 
            this.Mnu_Item_Close.Name = "Mnu_Item_Close";
            this.Mnu_Item_Close.Size = new System.Drawing.Size(141, 22);
            this.Mnu_Item_Close.Text = "Sair";
            this.Mnu_Item_Close.Click += new System.EventHandler(this.sairToolStripMenuItem_Click);
            //
            // SettingsToolStripMenuItem
            //
            this.SettingsToolStripMenuItem.Name = "Mnu_Item_Settings";
            this.SettingsToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.SettingsToolStripMenuItem.Text = "Configurações";
            this.SettingsToolStripMenuItem.Click+= new System.EventHandler(SettingsToolStripMenuItem_Click);
            // 
            // sobreToolStripMenuItem
            // 
            this.sobreToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.removerLicencaToolStripMenuItem,
            this.tlStripMenuItemAbout});
            this.sobreToolStripMenuItem.Name = "sobreToolStripMenuItem";
            this.sobreToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.sobreToolStripMenuItem.Text = "Ajuda";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(101, 6);
            //
            // toolStripSeparator2;
            //
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(138, 6);
            // 
            // sobreToolStripMenuItem1
            // 
            this.tlStripMenuItemAbout.Name = "sobreToolStripMenuItem1";
            this.tlStripMenuItemAbout.Size = new System.Drawing.Size(104, 22);
            this.tlStripMenuItemAbout.Text = "Sobre";
            this.tlStripMenuItemAbout.Click += new System.EventHandler(this.tlStripMenuItemAbout_click);
            // 
            // removerLicencaToolStripMenuItem
            // 
            this.removerLicencaToolStripMenuItem.Name = "removerLicencaToolStripMenuItem";
            this.removerLicencaToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.removerLicencaToolStripMenuItem.Text = "Remover Licença";
            this.removerLicencaToolStripMenuItem.Click += new System.EventHandler(this.removerLicencaToolStripMenuItem_Click);
            // 
            // ferramentasToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setFieldSearchPatternToolStripMenuItem,this.SigChartCheckStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(84, 20);
            this.toolsToolStripMenuItem.Text = "Ferramentas";
            // 
            // setFieldSearchPatternToolStripMenuItem
            // 
            this.setFieldSearchPatternToolStripMenuItem.Name = "setFieldSearchPatternToolStripMenuItem";
            this.setFieldSearchPatternToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.setFieldSearchPatternToolStripMenuItem.Text = "Procurar Palavra-Chave";
            this.setFieldSearchPatternToolStripMenuItem.Click += new System.EventHandler(this.setFieldSearchPatternToolStripMenuItem_Click);
            this.setFieldSearchPatternToolStripMenuItem.Enabled = false;
            //
            // SigChartCheckStripMenuItem
            //
            this.SigChartCheckStripMenuItem.Name = "SigChartCheckStripMenuItem";
            this.SigChartCheckStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.SigChartCheckStripMenuItem.Text = "Análise de Assinaturas";
            this.SigChartCheckStripMenuItem.Click += new System.EventHandler(SigChartCheckStripMenuItem_Click);
            this.SigChartCheckStripMenuItem.Enabled = false;
            // 
            // pnl_PDFPageViewer
            // 
            this.pnl_PDFPageViewer.AutoScroll = true;
            this.pnl_PDFPageViewer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.pnl_PDFPageViewer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnl_PDFPageViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_PDFPageViewer.Location = new System.Drawing.Point(143, 3);
            this.pnl_PDFPageViewer.Name = "pnl_PDFPageViewer";
            this.pnl_PDFPageViewer.Size = new System.Drawing.Size(586, 451);
            this.pnl_PDFPageViewer.TabIndex = 1;
            // 
            // TBPnl_PDFBarTools
            // 
            this.TBPnl_PDFBarTools.ColumnCount = 3;
            this.TBPnl_PDFBarTools.RowCount = 1;
            this.TBPnl_PDFBarTools.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TBPnl_PDFBarTools.Controls.Add(this.btn_DoubleSignatureField, 0, 0);
            this.TBPnl_PDFBarTools.Controls.Add(this.btn_ReplicationSignatureFields, 1, 0);
            this.TBPnl_PDFBarTools.Controls.Add(this.btn_RemoveSignatureFields, 2, 0);
            this.TBPnl_PDFBarTools.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TBPnl_PDFBarTools.Location = new System.Drawing.Point(0, 0);
            this.TBPnl_PDFBarTools.Name = "TBPnl_PDFBarTools";
            this.TBPnl_PDFBarTools.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TBPnl_PDFBarTools.Size = new System.Drawing.Size(130, 44);
            this.TBPnl_PDFBarTools.TabIndex = 0;
            // 
            // TBPnl_PDFTools
            // 
            this.TBPnl_PDFTools.ColumnCount = 10;
            this.TBPnl_PDFTools.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TBPnl_PDFTools.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TBPnl_PDFTools.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TBPnl_PDFTools.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TBPnl_PDFTools.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TBPnl_PDFTools.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TBPnl_PDFTools.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28F));
            this.TBPnl_PDFTools.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TBPnl_PDFTools.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 72F));
            this.TBPnl_PDFTools.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TBPnl_PDFTools.Controls.Add(this.btn_OpenDocument, 1, 0);
            this.TBPnl_PDFTools.Controls.Add(this.btn_AddSignatureField, 2, 0);
            this.TBPnl_PDFTools.Controls.Add(this.btn_SealSignature, 3, 0);
            this.TBPnl_PDFTools.Controls.Add(this.btn_SaveDocument, 4, 0);
            this.TBPnl_PDFTools.Controls.Add(this.btn_ReviewMassiveSignDocsFields, 5, 0);
            this.TBPnl_PDFTools.Controls.Add(this.tpnl_PDFNavigation, 7, 0);
            this.TBPnl_PDFTools.Controls.Add(this.TBPnl_PDFBarTools, 9, 0);
            this.TBPnl_PDFTools.Controls.Add(this.gpb_PDFMode, 0, 0);
            this.TBPnl_PDFTools.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TBPnl_PDFTools.Location = new System.Drawing.Point(5, 31);
            this.TBPnl_PDFTools.Name = "TBPnl_PDFTools";
            this.TBPnl_PDFTools.RowCount = 1;
            this.TBPnl_PDFTools.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TBPnl_PDFTools.Size = new System.Drawing.Size(827, 44);
            this.TBPnl_PDFTools.TabIndex = 0;
            // 
            // btn_OpenDocument
            // 
            this.btn_OpenDocument.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btn_OpenDocument.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn_OpenDocument.FlatAppearance.BorderSize = 0;
            this.btn_OpenDocument.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btn_OpenDocument.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btn_OpenDocument.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_OpenDocument.Image = global::SignatorDocSolution.Properties.Resources.OpenDoc;
            this.btn_OpenDocument.Location = new System.Drawing.Point(165, 2);
            this.btn_OpenDocument.Margin = new System.Windows.Forms.Padding(5, 2, 0, 0);
            this.btn_OpenDocument.Name = "btn_OpenDocument";
            this.btn_OpenDocument.Padding = new System.Windows.Forms.Padding(1, 0, 3, 1);
            this.btn_OpenDocument.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btn_OpenDocument.Size = new System.Drawing.Size(42, 42);
            this.btn_OpenDocument.TabIndex = 0;
            this.btn_OpenDocument.UseVisualStyleBackColor = true;
            this.btn_OpenDocument.Click += new System.EventHandler(this.btn_OpenDocument_Click);
            this.btn_OpenDocument.MouseHover += new System.EventHandler(this.btn_OpenDocument_MouseHover);
            // 
            // btn_AddSignatureField
            // 
            this.btn_AddSignatureField.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btn_AddSignatureField.Enabled = false;
            this.btn_AddSignatureField.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn_AddSignatureField.FlatAppearance.BorderSize = 0;
            this.btn_AddSignatureField.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btn_AddSignatureField.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btn_AddSignatureField.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_AddSignatureField.Image = global::SignatorDocSolution.Properties.Resources.signature;
            this.btn_AddSignatureField.Location = new System.Drawing.Point(212, 2);
            this.btn_AddSignatureField.Margin = new System.Windows.Forms.Padding(5, 2, 0, 0);
            this.btn_AddSignatureField.Name = "btn_AddSignatureField";
            this.btn_AddSignatureField.Padding = new System.Windows.Forms.Padding(1, 0, 3, 1);
            this.btn_AddSignatureField.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btn_AddSignatureField.Size = new System.Drawing.Size(42, 42);
            this.btn_AddSignatureField.TabIndex = 1;
            this.btn_AddSignatureField.UseVisualStyleBackColor = true;
            this.btn_AddSignatureField.Click += new System.EventHandler(this.btn_AddSignatureField_Click);
            this.btn_AddSignatureField.MouseHover += new System.EventHandler(this.btn_AddSignatureField_MouseHover);
            // 
            // btn_SealSignature
            // 
            this.btn_SealSignature.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btn_SealSignature.Enabled = false;
            this.btn_SealSignature.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn_SealSignature.FlatAppearance.BorderSize = 0;
            this.btn_SealSignature.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btn_SealSignature.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btn_SealSignature.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_SealSignature.Image = global::SignatorDocSolution.Properties.Resources.SignDoc_Seal;
            this.btn_SealSignature.Location = new System.Drawing.Point(259, 2);
            this.btn_SealSignature.Margin = new System.Windows.Forms.Padding(5, 2, 0, 0);
            this.btn_SealSignature.Name = "btn_SealSignature";
            this.btn_SealSignature.Padding = new System.Windows.Forms.Padding(1, 0, 3, 1);
            this.btn_SealSignature.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btn_SealSignature.Size = new System.Drawing.Size(42, 42);
            this.btn_SealSignature.TabIndex = 0;
            this.btn_SealSignature.UseVisualStyleBackColor = true;
            this.btn_SealSignature.Click += new System.EventHandler(this.btn_SealSignature_Click);
            this.btn_SealSignature.MouseHover += new System.EventHandler(this.btn_SealSignature_MouseHover);
            // 
            // btn_SaveDocument
            // 
            this.btn_SaveDocument.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btn_SaveDocument.Enabled = false;
            this.btn_SaveDocument.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn_SaveDocument.FlatAppearance.BorderSize = 0;
            this.btn_SaveDocument.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btn_SaveDocument.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btn_SaveDocument.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_SaveDocument.Image = global::SignatorDocSolution.Properties.Resources.SaveDoc;
            this.btn_SaveDocument.Location = new System.Drawing.Point(306, 2);
            this.btn_SaveDocument.Margin = new System.Windows.Forms.Padding(5, 2, 0, 0);
            this.btn_SaveDocument.Name = "btn_SaveDocument";
            this.btn_SaveDocument.Padding = new System.Windows.Forms.Padding(1, 0, 3, 1);
            this.btn_SaveDocument.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btn_SaveDocument.Size = new System.Drawing.Size(42, 42);
            this.btn_SaveDocument.TabIndex = 0;
            this.btn_SaveDocument.UseVisualStyleBackColor = true;
            this.btn_SaveDocument.Click += new System.EventHandler(this.btn_SaveDocument_Click);
            this.btn_SaveDocument.MouseHover += new System.EventHandler(this.btn_SaveDocument_MouseHover);
            //
            // btn_ReviewMassiveSignDocsFields
            //
            this.btn_ReviewMassiveSignDocsFields.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btn_ReviewMassiveSignDocsFields.Enabled = false;
            this.btn_ReviewMassiveSignDocsFields.Visible = false;
            this.btn_ReviewMassiveSignDocsFields.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn_ReviewMassiveSignDocsFields.FlatAppearance.BorderSize = 0;
            this.btn_ReviewMassiveSignDocsFields.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btn_ReviewMassiveSignDocsFields.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btn_ReviewMassiveSignDocsFields.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_ReviewMassiveSignDocsFields.Image = global::SignatorDocSolution.Properties.Resources.SigFieldReview;
            this.btn_ReviewMassiveSignDocsFields.Location = new System.Drawing.Point(353, 2);
            this.btn_ReviewMassiveSignDocsFields.Margin = new System.Windows.Forms.Padding(5, 2, 0, 0);
            this.btn_ReviewMassiveSignDocsFields.Name = "btn_ReviewMassiveSignDocsFields";
            this.btn_ReviewMassiveSignDocsFields.Padding = new System.Windows.Forms.Padding(1, 0, 3, 1);
            this.btn_ReviewMassiveSignDocsFields.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btn_ReviewMassiveSignDocsFields.Size = new System.Drawing.Size(42, 42);
            this.btn_ReviewMassiveSignDocsFields.TabIndex = 2;
            this.btn_ReviewMassiveSignDocsFields.UseVisualStyleBackColor = true;
            this.btn_ReviewMassiveSignDocsFields.Click+= new System.EventHandler(btn_ReviewMassiveSignDocsFields_Click);
            this.btn_ReviewMassiveSignDocsFields.MouseHover += new System.EventHandler(this.btn_ReviewMassiveSignDocsFields_MouseHover);
            // 
            // btn_DoubleSignatureField
            // 
            this.btn_DoubleSignatureField.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btn_DoubleSignatureField.Enabled = false;
            this.btn_DoubleSignatureField.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn_DoubleSignatureField.FlatAppearance.BorderSize = 0;
            this.btn_DoubleSignatureField.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btn_DoubleSignatureField.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btn_DoubleSignatureField.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_DoubleSignatureField.Image = global::SignatorDocSolution.Properties.Resources.DoubleSigField;
            this.btn_DoubleSignatureField.Location = new System.Drawing.Point(0, 0);
            this.btn_DoubleSignatureField.Margin = new System.Windows.Forms.Padding(3, 2, 0, 0);
            this.btn_DoubleSignatureField.Name = "btn_DoubleSignatureField";
            this.btn_DoubleSignatureField.Padding = new System.Windows.Forms.Padding(1, 0, 2, 1);
            this.btn_DoubleSignatureField.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btn_DoubleSignatureField.Size = new System.Drawing.Size(38, 38);
            this.btn_DoubleSignatureField.TabIndex = 0;
            this.btn_DoubleSignatureField.UseVisualStyleBackColor = true;
            this.btn_DoubleSignatureField.Click += new System.EventHandler(this.btn_DoubleSignatureField_Click);
            this.btn_DoubleSignatureField.MouseHover += new System.EventHandler(this.btn_DoubleSignatureField_MouseHover);
            // 
            // btn_ReplicationSignatureFields
            // 
            this.btn_ReplicationSignatureFields.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btn_ReplicationSignatureFields.Enabled = false;
            this.btn_ReplicationSignatureFields.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn_ReplicationSignatureFields.FlatAppearance.BorderSize = 0;
            this.btn_ReplicationSignatureFields.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btn_ReplicationSignatureFields.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btn_ReplicationSignatureFields.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_ReplicationSignatureFields.Image = global::SignatorDocSolution.Properties.Resources.ReplicationSigField;
            this.btn_ReplicationSignatureFields.Location = new System.Drawing.Point(0, 0);
            this.btn_ReplicationSignatureFields.Margin = new System.Windows.Forms.Padding(3, 2, 0, 0);
            this.btn_ReplicationSignatureFields.Name = "btn_ReplicationSignatureFields";
            this.btn_ReplicationSignatureFields.Padding = new System.Windows.Forms.Padding(1, 0, 2, 1);
            this.btn_ReplicationSignatureFields.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btn_ReplicationSignatureFields.Size = new System.Drawing.Size(38, 38);
            this.btn_ReplicationSignatureFields.TabIndex = 0;
            this.btn_ReplicationSignatureFields.UseVisualStyleBackColor = true;
            this.btn_ReplicationSignatureFields.Click += new System.EventHandler(this.btn_ReplicationSignatureFields_Click);
            this.btn_ReplicationSignatureFields.MouseHover += new System.EventHandler(this.btn_ReplicationSignatureFields_MouseHover);
            //
            //  btn_RemoveSignatureFields
            //
            this.btn_RemoveSignatureFields.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btn_RemoveSignatureFields.Enabled = false;
            this.btn_RemoveSignatureFields.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn_RemoveSignatureFields.FlatAppearance.BorderSize = 0;
            this.btn_RemoveSignatureFields.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btn_RemoveSignatureFields.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btn_RemoveSignatureFields.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_RemoveSignatureFields.Image = global::SignatorDocSolution.Properties.Resources.RemoveSigFields;
            this.btn_RemoveSignatureFields.Location = new System.Drawing.Point(0, 0);
            this.btn_RemoveSignatureFields.Margin = new System.Windows.Forms.Padding(3, 2, 0, 0);
            this.btn_RemoveSignatureFields.Name = "btn_RemoveSignatureFields";
            this.btn_RemoveSignatureFields.Padding = new System.Windows.Forms.Padding(1, 0, 2, 1);
            this.btn_RemoveSignatureFields.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btn_RemoveSignatureFields.Size = new System.Drawing.Size(38, 38);
            this.btn_RemoveSignatureFields.TabIndex = 0;
            this.btn_RemoveSignatureFields.UseVisualStyleBackColor = true;
            this.btn_RemoveSignatureFields.Click += new System.EventHandler(this.btn_RemoveSignatureFields_Click);
            this.btn_RemoveSignatureFields.MouseHover += new System.EventHandler(this.btn_RemoveSignatureFields_MouseHover);
            // 
            // tpnl_PDFNavigation
            //
            this.tpnl_PDFNavigation.AutoSize = false;
            this.tpnl_PDFNavigation.ColumnCount = 5;
            this.tpnl_PDFNavigation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tpnl_PDFNavigation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tpnl_PDFNavigation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tpnl_PDFNavigation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tpnl_PDFNavigation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tpnl_PDFNavigation.Controls.Add(this.Btn_PDFNavFirst, 0, 0);
            this.tpnl_PDFNavigation.Controls.Add(this.Btn_PDFNavPrevious, 1, 0);
            this.tpnl_PDFNavigation.Controls.Add(this.txt_PDFNavCurrPage, 2, 0);
            this.tpnl_PDFNavigation.Controls.Add(this.Btn_PDFNavNext, 3, 0);
            this.tpnl_PDFNavigation.Controls.Add(this.Btn_PDFNavLast, 4, 0);
            this.tpnl_PDFNavigation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpnl_PDFNavigation.Location = new System.Drawing.Point(405, 3);
            this.tpnl_PDFNavigation.Name = "tpnl_PDFNavigation";
            this.tpnl_PDFNavigation.RowCount = 1;
            this.tpnl_PDFNavigation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tpnl_PDFNavigation.Size = new System.Drawing.Size(260, 38);
            this.tpnl_PDFNavigation.TabIndex = 0;
            // 
            // Btn_PDFNavFirst
            // 
            this.Btn_PDFNavFirst.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Btn_PDFNavFirst.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Btn_PDFNavFirst.FlatAppearance.BorderSize = 0;
            this.Btn_PDFNavFirst.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.Btn_PDFNavFirst.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Btn_PDFNavFirst.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_PDFNavFirst.Image = ((System.Drawing.Image)(resources.GetObject("TStripBtn_First.Image")));
            this.Btn_PDFNavFirst.Location = new System.Drawing.Point(1, 1);
            this.Btn_PDFNavFirst.Margin = new System.Windows.Forms.Padding(1, 1, 3, 1);
            this.Btn_PDFNavFirst.Name = "Btn_PDFNavFirst";
            this.Btn_PDFNavFirst.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Btn_PDFNavFirst.Size = new System.Drawing.Size(36, 36);
            this.Btn_PDFNavFirst.TabIndex = 0;
            this.Btn_PDFNavFirst.UseVisualStyleBackColor = true;
            this.Btn_PDFNavFirst.Click += new System.EventHandler(this.TStripBtn_First_Click);
            // 
            // Btn_PDFNavPrevious
            // 
            this.Btn_PDFNavPrevious.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Btn_PDFNavPrevious.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Btn_PDFNavPrevious.FlatAppearance.BorderSize = 0;
            this.Btn_PDFNavPrevious.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.Btn_PDFNavPrevious.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Btn_PDFNavPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_PDFNavPrevious.Image = ((System.Drawing.Image)(resources.GetObject("TStripBtn_Previous.Image")));
            this.Btn_PDFNavPrevious.Location = new System.Drawing.Point(41, 1);
            this.Btn_PDFNavPrevious.Margin = new System.Windows.Forms.Padding(1);
            this.Btn_PDFNavPrevious.Name = "Btn_PDFNavPrevious";
            this.Btn_PDFNavPrevious.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Btn_PDFNavPrevious.Size = new System.Drawing.Size(36, 36);
            this.Btn_PDFNavPrevious.TabIndex = 0;
            this.Btn_PDFNavPrevious.UseVisualStyleBackColor = true;
            this.Btn_PDFNavPrevious.Click += new System.EventHandler(this.TStripBtn_Previous_Click);
            // 
            // txt_PDFNavCurrPage
            // 
            this.txt_PDFNavCurrPage.Location = new System.Drawing.Point(83, 10);
            this.txt_PDFNavCurrPage.Margin = new System.Windows.Forms.Padding(5, 10, 5, 5);
            this.txt_PDFNavCurrPage.Name = "txt_PDFNavCurrPage";
            this.txt_PDFNavCurrPage.Size = new System.Drawing.Size(90, 20);
            this.txt_PDFNavCurrPage.TabIndex = 1;
            this.txt_PDFNavCurrPage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_PDFNavCurrPage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TStripTxt_CurrPage_KeyPress);
            // 
            // Btn_PDFNavNext
            // 
            this.Btn_PDFNavNext.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Btn_PDFNavNext.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Btn_PDFNavNext.FlatAppearance.BorderSize = 0;
            this.Btn_PDFNavNext.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.Btn_PDFNavNext.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Btn_PDFNavNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_PDFNavNext.Image = ((System.Drawing.Image)(resources.GetObject("TStripBtn_Next.Image")));
            this.Btn_PDFNavNext.Location = new System.Drawing.Point(179, 1);
            this.Btn_PDFNavNext.Margin = new System.Windows.Forms.Padding(1, 1, 3, 1);
            this.Btn_PDFNavNext.Name = "Btn_PDFNavNext";
            this.Btn_PDFNavNext.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Btn_PDFNavNext.Size = new System.Drawing.Size(36, 36);
            this.Btn_PDFNavNext.TabIndex = 0;
            this.Btn_PDFNavNext.UseVisualStyleBackColor = true;
            this.Btn_PDFNavNext.Click += new System.EventHandler(this.TStripBtn_Next_Click);
            // 
            // Btn_PDFNavLast
            // 
            this.Btn_PDFNavLast.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Btn_PDFNavLast.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Btn_PDFNavLast.FlatAppearance.BorderSize = 0;
            this.Btn_PDFNavLast.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.Btn_PDFNavLast.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Btn_PDFNavLast.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_PDFNavLast.Image = ((System.Drawing.Image)(resources.GetObject("TStripBtn_Last.Image")));
            this.Btn_PDFNavLast.Location = new System.Drawing.Point(219, 1);
            this.Btn_PDFNavLast.Margin = new System.Windows.Forms.Padding(1);
            this.Btn_PDFNavLast.Name = "Btn_PDFNavLast";
            this.Btn_PDFNavLast.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Btn_PDFNavLast.Size = new System.Drawing.Size(36, 36);
            this.Btn_PDFNavLast.TabIndex = 0;
            this.Btn_PDFNavLast.UseVisualStyleBackColor = true;
            this.Btn_PDFNavLast.Click += new System.EventHandler(this.TStripBtn_Last_Click);            
            // 
            // gpb_PDFMode
            // 
            this.gpb_PDFMode.Controls.Add(this.rdo_AdobeMode);
            this.gpb_PDFMode.Controls.Add(this.rdo_SignMode);
            this.gpb_PDFMode.Location = new System.Drawing.Point(0, 0);
            this.gpb_PDFMode.Margin = new System.Windows.Forms.Padding(0);
            this.gpb_PDFMode.Name = "gpb_PDFMode";
            this.gpb_PDFMode.Size = new System.Drawing.Size(160, 44);
            this.gpb_PDFMode.TabIndex = 3;
            this.gpb_PDFMode.TabStop = false;
            this.gpb_PDFMode.MouseHover+=new System.EventHandler(gpb_PDFMode_MouseHover);
            // 
            // rdo_SignMode
            // 
            this.rdo_SignMode.AutoSize = true;
            this.rdo_SignMode.Checked = true;
            this.rdo_SignMode.Location = new System.Drawing.Point(10, 15);
            this.rdo_SignMode.Name = "rdo_SignMode";
            this.rdo_SignMode.Size = new System.Drawing.Size(60, 17);
            this.rdo_SignMode.TabIndex = 0;
            this.rdo_SignMode.TabStop = true;
            this.rdo_SignMode.Text = "Assinar";
            this.rdo_SignMode.UseVisualStyleBackColor = true;
            this.rdo_SignMode.CheckedChanged+=new System.EventHandler(rdo_SignMode_CheckedChanged);
            // 
            // pnl_PDFThumbnail
            // 
            this.pnl_PDFThumbnail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnl_PDFThumbnail.Cursor = System.Windows.Forms.Cursors.Default;
            this.pnl_PDFThumbnail.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnl_PDFThumbnail.Location = new System.Drawing.Point(3, 3);
            this.pnl_PDFThumbnail.Name = "pnl_PDFThumbnail";
            this.pnl_PDFThumbnail.Size = new System.Drawing.Size(134, 451);
            this.pnl_PDFThumbnail.TabIndex = 2;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // tpnl_PDFMain
            // 
            this.tpnl_PDFMain.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tpnl_PDFMain.ColumnCount = 1;
            this.tpnl_PDFMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tpnl_PDFMain.Controls.Add(this.Mnu_Strip_Main, 0, 0);
            this.tpnl_PDFMain.Controls.Add(this.TBPnl_PDFTools, 0, 1);
            this.tpnl_PDFMain.Controls.Add(this.pnl_ViewerBodyMain, 0, 2);
            this.tpnl_PDFMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpnl_PDFMain.Location = new System.Drawing.Point(0, 0);
            this.tpnl_PDFMain.Margin = new System.Windows.Forms.Padding(0);
            this.tpnl_PDFMain.Name = "tpnl_PDFMain";
            this.tpnl_PDFMain.RowCount = 3;
            this.tpnl_PDFMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tpnl_PDFMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tpnl_PDFMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tpnl_PDFMain.Size = new System.Drawing.Size(742, 545);
            this.tpnl_PDFMain.TabIndex = 0;
            // 
            // pnl_ViewerBodyMain
            // 
            this.pnl_ViewerBodyMain.Controls.Add(this.tpnl_PDFViewerSignMode);
            this.pnl_ViewerBodyMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_ViewerBodyMain.Location = new System.Drawing.Point(5, 83);
            this.pnl_ViewerBodyMain.Name = "pnl_ViewerBodyMain";
            this.pnl_ViewerBodyMain.Size = new System.Drawing.Size(732, 457);
            this.pnl_ViewerBodyMain.TabIndex = 0;
            //
            // pnl_PDFViewAdobeMode
            //
            this.pnl_PDFViewAdobeMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_PDFViewAdobeMode.Location = new System.Drawing.Point(0, 0);
            this.pnl_PDFViewAdobeMode.Name = "pnl_PDFViewAdobeMode";
            this.pnl_PDFViewAdobeMode.Size = new System.Drawing.Size(732, 457);
            this.pnl_PDFViewAdobeMode.TabIndex = 0;
            if (this.isAdbeReaderInstalled)
                this.pnl_PDFViewAdobeMode.Controls.Add(this.axAcroPDF);
            // 
            // tpnl_PDFViewerSignMode
            // 
            this.tpnl_PDFViewerSignMode.ColumnCount = 2;
            this.tpnl_PDFViewerSignMode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tpnl_PDFViewerSignMode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tpnl_PDFViewerSignMode.Controls.Add(this.pnl_PDFThumbnail, 0, 0);
            this.tpnl_PDFViewerSignMode.Controls.Add(this.pnl_PDFPageViewer, 1, 0);
            this.tpnl_PDFViewerSignMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpnl_PDFViewerSignMode.Location = new System.Drawing.Point(0, 0);
            this.tpnl_PDFViewerSignMode.Margin = new System.Windows.Forms.Padding(0);
            this.tpnl_PDFViewerSignMode.Name = "tpnl_PDFViewerSignMode";
            this.tpnl_PDFViewerSignMode.RowCount = 1;
            this.tpnl_PDFViewerSignMode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tpnl_PDFViewerSignMode.Size = new System.Drawing.Size(732, 457);
            this.tpnl_PDFViewerSignMode.TabIndex = 0;
            // 
            // axAcroPDF
            // 
            if (this.isAdbeReaderInstalled)
            {
                this.axAcroPDF.Enabled = true;
                this.axAcroPDF.Location = new System.Drawing.Point(0, 0);
                this.axAcroPDF.Name = "axAcroPDF";
                this.axAcroPDF.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axAcroPDF.OcxState")));
                this.axAcroPDF.Dock = System.Windows.Forms.DockStyle.Fill;
                this.axAcroPDF.Size = new System.Drawing.Size(500, 350);
                this.axAcroPDF.TabIndex = 0;
            }
            // 
            // rdo_AdobeMode
            // 
            this.rdo_AdobeMode.AutoSize = true;
            this.rdo_AdobeMode.Location = new System.Drawing.Point(80, 15);
            this.rdo_AdobeMode.Name = "rdo_AdobeMode";
            this.rdo_AdobeMode.Size = new System.Drawing.Size(70, 17);
            this.rdo_AdobeMode.TabIndex = 1;
            this.rdo_AdobeMode.Text = "Visualizar";
            this.rdo_AdobeMode.UseVisualStyleBackColor = true;
            this.rdo_AdobeMode.CheckedChanged+=new System.EventHandler(rdo_AdobeMode_CheckedChanged);
            // 
            // frm_SignConfig
            // 
            this.frm_SignConfig.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.frm_SignConfig.Icon = ((System.Drawing.Icon)(resources.GetObject("frm_SignConfig.Icon")));
            this.frm_SignConfig.Left = (int)(SignatorDocSolution.Utils.Defins.rectPrimaryScreen.Width * 0.35F);
            this.frm_SignConfig.Top = (int)(SignatorDocSolution.Utils.Defins.rectPrimaryScreen.Height * 0.20F);
            this.frm_SignConfig.MaximizeBox = false;
            this.frm_SignConfig.MinimizeBox = false;
            this.frm_SignConfig.Name = "frm_SignConfig";
            this.frm_SignConfig.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.frm_SignConfig.Text = "SignatorDoc - Configurações de Assinatura";
            this.frm_SignConfig.Visible = false;
            this.frm_SignConfig.SetEvents_Handler(new System.EventHandler(frm_SigConfig_ConfigConfigurations_Button_Click));
            // 
            // frm_ReplicSigField
            //
            this.frm_ReplicSigField.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.frm_ReplicSigField.Icon = ((System.Drawing.Icon)(resources.GetObject("frm_SignConfig.Icon")));
            this.frm_ReplicSigField.Left = (int)(SignatorDocSolution.Utils.Defins.rectPrimaryScreen.Width * 0.35F);
            this.frm_ReplicSigField.Top = (int)(SignatorDocSolution.Utils.Defins.rectPrimaryScreen.Height * 0.20F);
            this.frm_ReplicSigField.MaximizeBox = false;
            this.frm_ReplicSigField.MinimizeBox = false;
            this.frm_ReplicSigField.Name = "frm_ReplicSigField";
            this.frm_ReplicSigField.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.frm_ReplicSigField.setExternalEvents(new System.EventHandler(frm_ReplicSigField_ConfirmReplicate_Button_Click));
            this.frm_ReplicSigField.Visible = false;
            //
            // frm_RemoveSigField
            //
            this.frm_RemoveSigField.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.frm_RemoveSigField.Icon = ((System.Drawing.Icon)(resources.GetObject("frm_SignConfig.Icon")));
            this.frm_RemoveSigField.Left = (int)(SignatorDocSolution.Utils.Defins.rectPrimaryScreen.Width * 0.35F);
            this.frm_RemoveSigField.Top = (int)(SignatorDocSolution.Utils.Defins.rectPrimaryScreen.Height * 0.20F);
            this.frm_RemoveSigField.MaximizeBox = false;
            this.frm_RemoveSigField.MinimizeBox = false;
            this.frm_RemoveSigField.Name = "frm_RemoveSigField";
            this.frm_RemoveSigField.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.frm_RemoveSigField.setExternalEvents(new System.EventHandler(frm_RemoveSigField_ConfirmRemove_Button_Click));
            this.frm_RemoveSigField.Visible = false;
            //
            // frm_SetSigStringPattern
            //
            this.frm_SetSigStringPattern.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.frm_SetSigStringPattern.Icon = ((System.Drawing.Icon)(resources.GetObject("frm_SignConfig.Icon")));
            this.frm_SetSigStringPattern.Left = (int)(SignatorDocSolution.Utils.Defins.rectPrimaryScreen.Width * 0.35F);
            this.frm_SetSigStringPattern.Top = (int)(SignatorDocSolution.Utils.Defins.rectPrimaryScreen.Height * 0.20F);
            this.frm_SetSigStringPattern.MaximizeBox = false;
            this.frm_SetSigStringPattern.MinimizeBox = false;
            this.frm_SetSigStringPattern.Name = "frm_SetSigStringPattern";
            this.frm_SetSigStringPattern.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.frm_SetSigStringPattern.setExternalEvents(new System.EventHandler(frm_SetSigStringPattern_Confirm_Button_Click));
            this.frm_SetSigStringPattern.Visible = false;
            this.frm_SetSigStringPattern.setChieldIcon(this.frm_SetSigStringPattern.Icon);
            //
            //this.frm_Configurations
            //
            this.frm_Configurations.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.frm_Configurations.Icon = ((System.Drawing.Icon)(resources.GetObject("frm_SignConfig.Icon")));
            this.frm_Configurations.Left = (int)(SignatorDocSolution.Utils.Defins.rectPrimaryScreen.Width * 0.35F);
            this.frm_Configurations.Top = (int)(SignatorDocSolution.Utils.Defins.rectPrimaryScreen.Height * 0.20F);
            this.frm_Configurations.MaximizeBox = false;
            this.frm_Configurations.MinimizeBox = false;
            this.frm_Configurations.Name = "frm_Configurations";
            this.frm_Configurations.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.frm_Configurations.Visible = false;
            //
            // frm_Properties
            //
            this.frm_Properties.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.frm_Properties.Icon = ((System.Drawing.Icon)(resources.GetObject("frm_SignConfig.Icon")));
            this.frm_Properties.Left = (int)(SignatorDocSolution.Utils.Defins.rectPrimaryScreen.Width * 0.35F);
            this.frm_Properties.Top = (int)(SignatorDocSolution.Utils.Defins.rectPrimaryScreen.Height * 0.20F);
            this.frm_Properties.MaximizeBox = false;
            this.frm_Properties.MinimizeBox = false;
            this.frm_Properties.Name = "frm_Properties";
            this.frm_Properties.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.frm_Properties.Visible = false;
            //
            // frm_DefineSigFieldsMassiveDocs
            //
            this.frm_DefineSigFieldsMassiveDocs.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.frm_DefineSigFieldsMassiveDocs.Icon = ((System.Drawing.Icon)(resources.GetObject("frm_SignConfig.Icon")));
            this.frm_DefineSigFieldsMassiveDocs.Left = (int)(SignatorDocSolution.Utils.Defins.rectPrimaryScreen.Width * 0.20F);
            this.frm_DefineSigFieldsMassiveDocs.Top = (int)(SignatorDocSolution.Utils.Defins.rectPrimaryScreen.Height * 0.15F);
            this.frm_DefineSigFieldsMassiveDocs.MaximizeBox = false;
            this.frm_DefineSigFieldsMassiveDocs.MinimizeBox = false;
            this.frm_DefineSigFieldsMassiveDocs.Name = "frm_DefineSigFieldsMassiveDocs";
            this.frm_DefineSigFieldsMassiveDocs.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.frm_DefineSigFieldsMassiveDocs.setExternalEvents(new System.EventHandler(frm_DefineSigFieldsMassiveDocs_Confirm_Button_Click));
            this.frm_DefineSigFieldsMassiveDocs.Visible = false;
            //
            // frm_SignatureChartCompare
            //
            this.frm_SignatureChartCompare.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.frm_SignatureChartCompare.Icon = ((System.Drawing.Icon)(resources.GetObject("frm_SignConfig.Icon")));
            this.frm_SignatureChartCompare.Left = (int)(SignatorDocSolution.Utils.Defins.rectPrimaryScreen.Width * 0.15F);
            this.frm_SignatureChartCompare.Top = (int)(SignatorDocSolution.Utils.Defins.rectPrimaryScreen.Height * 0.05F);
            this.frm_SignatureChartCompare.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.frm_SignatureChartCompare.MaximizeBox = false;
            this.frm_SignatureChartCompare.MinimizeBox = false;
            this.frm_SignatureChartCompare.Name = "frm_SignatureChartCompare";
            this.frm_SignatureChartCompare.Visible = false;
            //
            // frm_SigFieldMassiveDocReview
            //
            this.frm_SigFieldMassiveDocReview.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.frm_SigFieldMassiveDocReview.Icon = ((System.Drawing.Icon)(resources.GetObject("frm_SignConfig.Icon")));
            this.frm_SigFieldMassiveDocReview.Left = (int)(SignatorDocSolution.Utils.Defins.rectPrimaryScreen.Width * 0.15F);
            this.frm_SigFieldMassiveDocReview.Top = (int)(SignatorDocSolution.Utils.Defins.rectPrimaryScreen.Height * 0.05F);
            this.frm_SigFieldMassiveDocReview.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.frm_SigFieldMassiveDocReview.MaximizeBox = false;
            this.frm_SigFieldMassiveDocReview.MinimizeBox = false;
            this.frm_SigFieldMassiveDocReview.Name = "frm_SigFieldMassiveDocReview";
            this.frm_SigFieldMassiveDocReview.setExternalEvents(frm_SigFieldMassiveDocReview_Confirm_Button_Click);
            this.frm_SigFieldMassiveDocReview.Visible = false;
            // 
            // popUp
            // 
            this.popUp.AcceptAlt = false;
            this.popUp.AnimationDuration = 0;
            this.popUp.FocusOnOpen = false;
            this.popUp.HidingAnimation = SignatorDocSolution.PopupControl.PopupAnimations.None;
            this.popUp.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.popUp.Name = "popUp";
            this.popUp.NonInteractive = false;
            this.popUp.Resizable = false;
            this.popUp.ShowingAnimation = SignatorDocSolution.PopupControl.PopupAnimations.None;
            this.popUp.Size = new System.Drawing.Size(2, 4);
            //
            //
            //
            this.loadingPanel.Visible = false;
            // 
            // aboutBox_Form
            // 
            this.aboutBox_Form.ClientSize = new System.Drawing.Size(435, 300);
            this.aboutBox_Form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.aboutBox_Form.Left = (int)((SignatorDocSolution.Utils.Defins.rectPrimaryScreen.Width / 2) - (this.aboutBox_Form.ClientSize.Width/2));
            this.aboutBox_Form.Top = (int)((SignatorDocSolution.Utils.Defins.rectPrimaryScreen.Height / 2) - (this.aboutBox_Form.ClientSize.Height / 2));
            this.aboutBox_Form.MaximizeBox = false;
            this.aboutBox_Form.MinimizeBox = false;
            this.aboutBox_Form.Name = "aboutBox_Form";
            this.aboutBox_Form.Padding = new System.Windows.Forms.Padding(9);
            this.aboutBox_Form.ShowIcon = false;
            this.aboutBox_Form.ShowInTaskbar = false;
            this.aboutBox_Form.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.aboutBox_Form.Text = "SignatorDoc - Sobre";
            this.aboutBox_Form.Visible = false;
            // 
            // Form_Main
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(SignatorDocSolution.Utils.Defins.initialFormSize.Width, SignatorDocSolution.Utils.Defins.initialFormSize.Height);
            this.Controls.Add(this.tpnl_PDFMain);
            this.Controls.Add(this.loadingPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.Mnu_Strip_Main;
            this.Name = "Form_Main";
            this.Text = "SignatorDoc - Sistema de Assinatura de Documentos Digitais";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.AllowDrop = true;
            this.DragEnter+= new System.Windows.Forms.DragEventHandler(Form_Main_DragEnter);
            this.DragDrop+= new System.Windows.Forms.DragEventHandler(Form_Main_DragDrop);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Main_FormClosing);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.Form_Main_MouseWheel);
            this.Mnu_Strip_Main.ResumeLayout(false);
            this.Mnu_Strip_Main.PerformLayout();
            this.TBPnl_PDFTools.ResumeLayout(false);
            this.TBPnl_PDFBarTools.ResumeLayout(false);
            this.tpnl_PDFNavigation.ResumeLayout(false);
            this.tpnl_PDFNavigation.PerformLayout();
            this.gpb_PDFMode.ResumeLayout(false);
            this.gpb_PDFMode.PerformLayout();
            this.tpnl_PDFMain.ResumeLayout(false);
            this.tpnl_PDFMain.PerformLayout();
            this.pnl_PDFViewAdobeMode.ResumeLayout(false);
            this.pnl_ViewerBodyMain.ResumeLayout(false);
            this.tpnl_PDFViewerSignMode.ResumeLayout(false);
            this.loadingPanel.ResumeLayout(false);
            if (this.isAdbeReaderInstalled)
                ((System.ComponentModel.ISupportInitialize)(this.axAcroPDF)).EndInit();
            this.ResumeLayout(false);

        }  

        #endregion

        private System.Windows.Forms.MenuStrip Mnu_Strip_Main;
        private System.Windows.Forms.ToolStripMenuItem Mnu_File;
        private System.Windows.Forms.ToolStripMenuItem Mnu_Item_Open;
        private System.Windows.Forms.ToolStripMenuItem Mnu_Item_Save;
        private System.Windows.Forms.ToolStripMenuItem SettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sobreToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tlStripMenuItemAbout;
        private System.Windows.Forms.ToolStripMenuItem removerLicencaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setFieldSearchPatternToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SigChartCheckStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem salvarComoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Mnu_Item_Close;
        private System.Windows.Forms.ToolStripSeparator Mnu_StripSeparator_Close;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.TableLayoutPanel tpnl_PDFViewerSignMode;
        private System.Windows.Forms.TableLayoutPanel tpnl_PDFNavigation;
        private System.Windows.Forms.TableLayoutPanel TBPnl_PDFTools;
        private System.Windows.Forms.TableLayoutPanel TBPnl_PDFBarTools;
        private System.Windows.Forms.TableLayoutPanel tpnl_PDFMain;
        private System.Windows.Forms.Panel pnl_PDFThumbnail;
        private System.Windows.Forms.Panel pnl_PDFPageViewer;
        private System.Windows.Forms.Panel pnl_PDFViewAdobeMode;
        private System.Windows.Forms.Panel pnl_ViewerBodyMain;
        private System.Windows.Forms.Button btn_AddSignatureField;
        private System.Windows.Forms.Button btn_OpenDocument;
        private System.Windows.Forms.Button btn_SaveDocument;
        private System.Windows.Forms.Button btn_SealSignature;
        private System.Windows.Forms.Button btn_DoubleSignatureField;
        private System.Windows.Forms.Button btn_ReplicationSignatureFields;
        private System.Windows.Forms.Button btn_RemoveSignatureFields;
        private System.Windows.Forms.Button btn_ReviewMassiveSignDocsFields;
        private System.Windows.Forms.Button Btn_PDFNavFirst;
        private System.Windows.Forms.Button Btn_PDFNavPrevious;
        private System.Windows.Forms.Button Btn_PDFNavNext;
        private System.Windows.Forms.Button Btn_PDFNavLast;
        private System.Windows.Forms.ToolTip tlTip_Main;
        private System.Windows.Forms.RadioButton rdo_SignMode;
        private System.Windows.Forms.RadioButton rdo_AdobeMode;
        private System.Windows.Forms.GroupBox gpb_PDFMode;
        private System.Windows.Forms.TextBox txt_PDFNavCurrPage; 
        private Form_SignConfig frm_SignConfig;
        private Form_ReplicateSigField frm_ReplicSigField;
        private Form_RemoveFields frm_RemoveSigField;
        private Form_SetSigStringPattern frm_SetSigStringPattern;
        private Form_DefineSigFieldsMassiveDocs  frm_DefineSigFieldsMassiveDocs;
        private Form_SignatureChartCompare frm_SignatureChartCompare;
        private Form_SigFieldMassiveDocReview frm_SigFieldMassiveDocReview;
        private Form_Configurations frm_Configurations;
        private Form_Properties frm_Properties;
        private OptionSigFieldLocationStart optionLocationSigField;
        private PanelThumbnail pnlThumbnail;
        private Form_AboutBox aboutBox_Form;
        private SignatorDocSolution.Utils.FormLicense frm_License;
        private PopupControl.Popup popUp;
        private SignatorDocSolution.Utils.LoadingPanel loadingPanel;
        private AxAcroPDFLib.AxAcroPDF axAcroPDF;
        private bool isAdbeReaderInstalled;  

        private bool checkAdbeReaderinstaled()
        {
            Microsoft.Win32.RegistryKey adobe = null;
            if (SignatorDocSolution.Utils.NativeAweSome.IsX64Platform())
            {
                Microsoft.Win32.RegistryKey wow6432node = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("Wow6432Node");
                if (wow6432node != null)
                    adobe = wow6432node.OpenSubKey("Adobe");
            }
            if (adobe == null)
                adobe = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("Adobe");

            if (adobe != null)
            {
                Microsoft.Win32.RegistryKey acroReader = adobe.OpenSubKey("Acrobat Reader");
                if (acroReader != null)
                {
                    string[] acroReaderVersions = acroReader.GetSubKeyNames();
                    float acroVersion;
                    if (acroReaderVersions != null)
                    {
                        foreach (string s in acroReaderVersions)
                        {
                            if (s.Contains("DC"))
                                return true;
                            else
                            {
                                float.TryParse(s.Replace(".", ","), out acroVersion);
                                if ((acroVersion > 7))
                                    return true;
                            }
                        }
                    }
                }

            }
            return false;

        }


    }
}

