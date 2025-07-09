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
    public partial class Form_ReplicateSigField : Form
    {
        private List<PairItemsInt> listPairPagesSigFields = new List<PairItemsInt>();

        public Form_ReplicateSigField()
        {
            InitializeComponent();
        }

        public void setExternalEvents(System.EventHandler event_HandleClick_1) {
            this.btn_ReplicateConfirm.Click += event_HandleClick_1;
        }

        private void Form_ReplicateSigField_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void btn_Sair_Click(object sender, EventArgs e)
        {
            this.clearReplicatedFields();
            this.Hide();
        }

        public void setPageNumberQts(int pageNumberQtd) {
            this.chkL_Pages.Items.Clear();
            for (int i = 0; i < pageNumberQtd; i++) {
                this.chkL_Pages.Items.Add( (i+1), CheckState.Unchecked);
            }
        }

        public void setSigFields(object[] listSigFiels) {
            this.chkL_SigFields.Items.AddRange(listSigFiels);
        }

        private void btn_Replicate_Click(object sender, EventArgs e)
        {
            if (this.chkL_Pages.CheckedItems.Count < 1)
            {
                this.errorProvider.SetError(this.btn_Replicate, "Nenhuma página foi selecionanada!");
                return;
            }
            else {
                if (this.chkL_SigFields.CheckedIndices.Count < 1)
                {
                    this.errorProvider.SetError(this.btn_Replicate, "Nenhum campo de assinatura foi selecionado!");
                    return;
                }
                else {
                    this.errorProvider.SetError(this.btn_Replicate, "");
                }
            }

            string sigFieldItemAux = null;
            PairItemsInt pit;
            foreach (int item in this.chkL_Pages.CheckedItems)
            {
                foreach (int index in this.chkL_SigFields.CheckedIndices)
                {
                    pit.Item = item;
                    pit.Value = index;
                    if (!this.listPairPagesSigFields.Contains(pit))
                        this.listPairPagesSigFields.Add(pit);

                    sigFieldItemAux = this.chkL_SigFields.Items[index] + " => " + item;
                    if (!this.lstB_SigFieldPagesReview.Items.Contains(sigFieldItemAux))
                        this.lstB_SigFieldPagesReview.Items.Add(sigFieldItemAux);
                }
            }

            this.btn_ReplicateConfirm.Enabled = true;
            this.btn_ClearSigFieldListReview.Enabled = true;
            
        }

        public void disable_btn_ReplicateConfirm() {
            this.btn_ReplicateConfirm.Enabled = false;
            this.btn_ClearSigFieldListReview.Enabled = false;
        }

        private void btn_ClearSigFieldListReview_Click(object sender, EventArgs e)
        {
            this.listPairPagesSigFields.Clear();
            this.lstB_SigFieldPagesReview.Items.Clear();
            
            foreach (int index in this.chkL_SigFields.CheckedIndices)
            {
                this.chkL_SigFields.SetItemChecked(index, false);
            }
            foreach (int index in this.chkL_Pages.CheckedIndices)
            {
                this.chkL_Pages.SetItemChecked(index, false);
            }
            this.btn_ClearSigFieldListReview.Enabled = false;
            this.btn_ReplicateConfirm.Enabled = false;
            this.chkL_SigFields.ClearSelected();
            this.chkL_Pages.ClearSelected();

            this.errorProvider.SetError(this.btn_Replicate, "");
        }
       
        private void btn_SelectAllPages_Click(object sender, EventArgs e)
        { 
            for (int i = 0; i < this.chkL_Pages.Items.Count; i++)
            {
                this.chkL_Pages.SetItemChecked(i, true);
            }
        }

        private void chkL_SigFields_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
        {
            if (this.chkL_SigFields.CheckedItems.Count == 1 && e.NewValue == CheckState.Unchecked)
            {
                this.btn_SelectAllPages.Enabled = false;
                for (int i = 0; i < this.chkL_Pages.Items.Count; i++)
                {
                    this.chkL_Pages.SetItemChecked(i, false);
                }
            }
            else
                if (this.chkL_SigFields.CheckedItems.Count == 0 && e.NewValue == CheckState.Checked)
                    this.btn_SelectAllPages.Enabled = true;
        }

        public List<PairItemsInt> getListPairPagesSigFiels() {
            return this.listPairPagesSigFields;
        }

        public void clearReplicatedFields() {
            foreach (int indice in this.chkL_Pages.CheckedIndices) {
                this.chkL_Pages.SetItemChecked(indice, false);
            }

            foreach (int indice in this.chkL_SigFields.CheckedIndices)
            {
                this.chkL_SigFields.SetItemChecked(indice, false);
            }

            this.chkL_SigFields.Items.Clear();

            this.listPairPagesSigFields.Clear();

            this.lstB_SigFieldPagesReview.Items.Clear();

            this.btn_ClearSigFieldListReview.Enabled = false;
            this.btn_ReplicateConfirm.Enabled = false;
            this.chkL_SigFields.ClearSelected();
            this.chkL_Pages.ClearSelected();

            this.errorProvider.SetError(this.btn_Replicate, "");
        }

    }
}
