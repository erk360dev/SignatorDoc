using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SignatorDocSolution
{
    public partial class Form_RemoveFields : Form
    {
        public Form_RemoveFields()
        {
            InitializeComponent();
        }

        public void setExternalEvents(System.EventHandler event_HandleClick_1)
        {
            this.btn_ConfirmRemFields.Click += event_HandleClick_1;
        }

        public void setSigFields(object[] listSigFiels)
        {
            this.chkL_FieldsPages.Items.AddRange(listSigFiels);
        }

        private void Form_RemoveSigField_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            clearRemoveFieldsList();
            e.Cancel = true;
            this.Hide();
        }

        public List<int> getSelectedFields() {
            List<int> listIndexes = new List<int>();

            for (int i = (this.chkL_FieldsPages.Items.Count-1); i >= 0; i--)
            {
                if (this.chkL_FieldsPages.GetItemChecked(i)) {
                    this.chkL_FieldsPages.Items.RemoveAt(i);
                    listIndexes.Add(i);
                }
            }

            return listIndexes;
        }

        public void removeFieldsCheckedItems() {
            foreach (object item in this.chkL_FieldsPages.CheckedItems) {
                this.chkL_FieldsPages.Items.Remove(item);
            }
        }

        private void btn_CleanRemFields_Click(object sender, EventArgs e)
        {
            foreach (int indice in this.chkL_FieldsPages.CheckedIndices)
            {
                this.chkL_FieldsPages.SetItemChecked(indice, false);
            }
        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            clearRemoveFieldsList();
            this.Hide();
        }

        private void chkL_FieldsPages_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
        {
            if (this.chkL_FieldsPages.CheckedItems.Count == 1 && e.NewValue == CheckState.Unchecked)
                this.btn_ConfirmRemFields.Enabled = false;       
            else
               this.btn_ConfirmRemFields.Enabled = true;
        }

        private void clearRemoveFieldsList(){
            this.chkL_FieldsPages.Items.Clear();
        }

        private void btn_SelectAll_Click(object sender, EventArgs e)
        {
            for(int i=0;i<this.chkL_FieldsPages.Items.Count;i++){
                this.chkL_FieldsPages.SetItemChecked(i, true);
            }
        }
    }
}
