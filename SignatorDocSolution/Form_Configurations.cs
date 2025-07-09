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
    public partial class Form_Configurations : Form
    {
        public Form_Configurations()
        {
            InitializeComponent();
        }

        private void btn_Confirm_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Confirmar configurações?\n\nObservação: Após a confirmação, o SignatorDoc será REINICIADO.", "Configuração", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes) {
                Configurations.Documents_DisableCheckPassword = this.chk_DisableHandlePassword.Checked;
                Configurations.writeConfigurations();
                Application.Restart();
                Application.ExitThread();  
            }
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void Form_Configurations_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        public void setInitialConfiguration() {
            this.chk_DisableHandlePassword.Checked = Configurations.Documents_DisableCheckPassword;
        }

    }
}
