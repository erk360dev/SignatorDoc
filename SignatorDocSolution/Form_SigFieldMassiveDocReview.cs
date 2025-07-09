using SignatorDocSolution.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SignatorDocSolution
{
    public partial class Form_SigFieldMassiveDocReview : Form
    {
        private List<AuxLocalPdfDocInfo> PDFDocInfoSelectedList = new List<AuxLocalPdfDocInfo>();

        public Form_SigFieldMassiveDocReview()
        {
            InitializeComponent();
        }

        public void setExternalEvents(System.EventHandler event_HandleClick_1)
        {
            this.btn_Confirm.Click += event_HandleClick_1;
        }

        public void setSigFieldsConfig(List<PDFDocInfo> pdfDocInfoList) {
            string auxDocName = "";
            int auxIndex = 0;
            int docIndex = -1;
            this.PDFDocInfoSelectedList.Clear();
            this.lstB_Docs.Items.Clear();
            this.chkL_SigFieldsInf.Items.Clear();

            pdfDocInfoList.Sort((a, b) => a.documentName.CompareTo(b.documentName));
            foreach (PDFDocInfo pdfDocinf in pdfDocInfoList) { 
                AuxLocalPdfDocInfo auxPdfDocInf= new AuxLocalPdfDocInfo();
                auxPdfDocInf.PDFDoc_Info=pdfDocinf;
                auxPdfDocInf.Checked = true;

                if (!pdfDocinf.documentName.Equals(auxDocName))
                {
                    auxDocName = pdfDocinf.documentName;
                    docIndex++;
                }
                
                auxPdfDocInf.DocIndex = docIndex;
                
                this.PDFDocInfoSelectedList.Add(auxPdfDocInf);
            }
            
            auxDocName = "";
            for (int i = 0; i < this.PDFDocInfoSelectedList.Count;i++ )
            {
                if (!auxDocName.Equals(this.PDFDocInfoSelectedList[i].PDFDoc_Info.documentName))
                {
                    auxDocName = this.PDFDocInfoSelectedList[i].PDFDoc_Info.documentName;
                    if (!this.lstB_Docs.Items.Contains(auxDocName))
                    {
                        this.lstB_Docs.Items.Insert(auxIndex, auxDocName);
                        auxIndex++;
                    }
                }
            }

            this.lstB_Docs.SelectedIndex = 0;
            this.chkL_SigFieldsInf.SetItemChecked(0, true);
        }

        private void btn_ClearAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.chkL_SigFieldsInf.Items.Count; i++)
            {
                this.chkL_SigFieldsInf.SetItemChecked(i, false);
                this.PDFDocInfoSelectedList.Find(o => o.FieldIndex.Equals(i) && o.DocIndex.Equals(this.lstB_Docs.SelectedIndex)).Checked = false;
            }
        }

        private void lstB_Docs_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.chkL_SigFieldsInf.Items.Clear();
            System.Drawing.SizeF pageBaseItext = new System.Drawing.SizeF((Defins.pageSizeBase.Width / Defins.pagesDPI) * 72.0F, (Defins.pageSizeBase.Height / Defins.pagesDPI) * 72.0F);
            //float[] pageWidthPart = { (pageBaseItext.Width / 3), ((pageBaseItext.Width / 3) * 2), pageBaseItext.Width };
            //float[] pageHeightPart = { (pageBaseItext.Height / 3), ((pageBaseItext.Height / 3) * 2), pageBaseItext.Height };
            float[] pageWidthPart = { ((pageBaseItext.Width / 2) - Defins.SignatureField_Size_Itext.Width), (pageBaseItext.Width / 2), pageBaseItext.Width };
            float[] pageHeightPart = { ((pageBaseItext.Height / 2) - (Defins.SignatureField_Size_Itext.Height * 2.25f)), ((pageBaseItext.Height / 2) + (Defins.SignatureField_Size_Itext.Height * 1.5f)), pageBaseItext.Height };

            string textCheck = null;
            int auxFieldIndex = 0;
            int auxSigField2 = 0;
            for (int i = 0; i < this.PDFDocInfoSelectedList.Count;i++ )
            {
                if (this.PDFDocInfoSelectedList[i].PDFDoc_Info.documentName.Equals(this.lstB_Docs.SelectedItem))
                {
                    if (this.PDFDocInfoSelectedList[i].PDFDoc_Info.SigStringPosition.Y < pageHeightPart[0])
                    {
                        textCheck = "Página " + this.PDFDocInfoSelectedList[i].PDFDoc_Info.SigStringPage + " -  Campo_" + (auxSigField2+1) + " - Inferior ";
                    }
                    else
                        if (this.PDFDocInfoSelectedList[i].PDFDoc_Info.SigStringPosition.Y >= pageHeightPart[0] && this.PDFDocInfoSelectedList[i].PDFDoc_Info.SigStringPosition.Y < pageHeightPart[1])
                        {
                            textCheck = "Página " + this.PDFDocInfoSelectedList[i].PDFDoc_Info.SigStringPage + " -  Campo_" + (auxSigField2 + 1) + " - Meio ";
                        }
                        else
                        {
                            textCheck = "Página " + this.PDFDocInfoSelectedList[i].PDFDoc_Info.SigStringPage + " -  Campo_" + (auxSigField2 + 1) + " - Superior ";
                        }


                    if (this.PDFDocInfoSelectedList[i].PDFDoc_Info.SigStringPosition.X < pageWidthPart[0])
                    {
                        textCheck += "Esquerdo";
                    }
                    else
                        if (this.PDFDocInfoSelectedList[i].PDFDoc_Info.SigStringPosition.X >= pageWidthPart[0] && this.PDFDocInfoSelectedList[i].PDFDoc_Info.SigStringPosition.X < pageWidthPart[1])
                        {
                            textCheck += "Central";
                        }
                        else
                        {
                            textCheck += "Direito";
                        }

                    this.chkL_SigFieldsInf.Items.Insert(auxFieldIndex, textCheck);
                    this.PDFDocInfoSelectedList[i].FieldIndex = auxFieldIndex;

                    if (this.PDFDocInfoSelectedList[i].FieldIndex < 0)
                    {
                        
                        this.chkL_SigFieldsInf.SetItemChecked(auxFieldIndex, true);
                    }
                    else { 
                        if(this.PDFDocInfoSelectedList[i].Checked)
                            this.chkL_SigFieldsInf.SetItemChecked(auxFieldIndex, true);
                        else
                            this.chkL_SigFieldsInf.SetItemChecked(auxFieldIndex, false);
                    }
                    
                    auxFieldIndex++;
                    auxSigField2++;
                }
            }
        }

        private void chkL_SigFieldsInf_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            int index = this.PDFDocInfoSelectedList.FindIndex(o => o.FieldIndex.Equals(e.Index) && o.DocIndex.Equals(this.lstB_Docs.SelectedIndex));
            if(index>=0)
             this.PDFDocInfoSelectedList[index].Checked = (e.NewValue == CheckState.Checked);
        }

        public List<PDFDocInfo> getSelectedPDFDocInfoList() {
            List<PDFDocInfo> auxPdfDocInfoList = new List<PDFDocInfo>();

            foreach (AuxLocalPdfDocInfo aux in this.PDFDocInfoSelectedList)
            {
                if (aux.Checked)
                {
                    auxPdfDocInfoList.Add(aux.PDFDoc_Info);
                }
            }

            return auxPdfDocInfoList;
        }

        public bool checkIfListChanged() {
            AuxLocalPdfDocInfo aux = this.PDFDocInfoSelectedList.Find(o => (!o.Checked));
            if (aux != null || this.PDFDocInfoSelectedList.Count<1)
                return true;
            else
                return false;
            
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void Form_SigFieldMassiveDocReview_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void btn_selectAll_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < this.chkL_SigFieldsInf.Items.Count; i++) {
                this.chkL_SigFieldsInf.SetItemChecked(i, true);
                this.PDFDocInfoSelectedList.Find(o => o.FieldIndex.Equals(i) && o.DocIndex.Equals(this.lstB_Docs.SelectedIndex)).Checked = true;
            }
        }

        private void btn_ClearAllDocs_Click(object sender, EventArgs e)
        {
            if(this.chkL_SigFieldsInf.Items.Count>0)
                this.chkL_SigFieldsInf.Items.Clear();

            if (this.lstB_Docs.Items.Count > 0)
                this.lstB_Docs.Items.Clear();

            if( PDFDocInfoSelectedList.Count>0)
                 PDFDocInfoSelectedList.Clear();
        }

    }

    class AuxLocalPdfDocInfo {
        public PDFDocInfo PDFDoc_Info;
        public bool Checked=true;
        public int FieldIndex=(-1);
        public int DocIndex=(-1);
    }
}
