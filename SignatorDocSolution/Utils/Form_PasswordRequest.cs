using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SignatorDocSolution.Utils
{
    public partial class Form_PasswordRequest : Form
    {
        public Form_PasswordRequest()
        {
            InitializeComponent();
        }

        private void txt_Password_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back)
            {
                if (!this.btn_Ok.Enabled)
                    this.btn_Ok.Enabled = true;
            }
            else
                if(this.txt_Password.Text.Length<2)
                    this.btn_Ok.Enabled = false;
        }

        public static System.Windows.Forms.DialogResult ShowCustomDialog(Form_PasswordRequest InstancedForm, string msgText, string textTitle, ref string resultText)
        {
            InstancedForm.Text = textTitle;
            InstancedForm.lblMessage.Text = msgText;
            System.Windows.Forms.DialogResult dialogResult=InstancedForm.ShowDialog();
            if(dialogResult == DialogResult.OK)
                resultText = InstancedForm.txt_Password.Text.Trim();

            InstancedForm.txt_Password.Text = "";
            InstancedForm.btn_Ok.Enabled = false;

            return dialogResult;
        }
    }
}
