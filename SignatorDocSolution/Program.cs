using SignatorDocSolution.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SignatorDocSolution
{
    /// CLASS ID=01
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        
        [STAThread]
        static void Main(string[] args)
        {
            if (checkLicense())
            {
                string lic = Configurations.getLicense();
                //pdftron.PDFNet.Initialize(lic);
                checkInitialArguments(args);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form_Main());
               // pdftron.PDFNet.Terminate();
            }
            else
            {
                Application.Exit();
            }

        }

        private static void checkInitialArguments(string[] main_Args){
            try
            {
                if(main_Args.Length==1)
                {
                    if(main_Args[0].Length>4){
                        if(File.Exists(main_Args[0])){
                            string ext=Path.GetExtension(main_Args[0]);
                            if (ext.Equals(".pdf") || ext.Equals(".doc") || ext.Equals(".docx")) 
                            {
                                AweSomeUtils.IsInitialFileArg = true;
                                AweSomeUtils.InitialFileArgPath = main_Args[0];
                            }
                        }
                    }
                }
            }
            catch (Exception exc) {
                MessageBox.Show("Houve uma falha ao verificar um argumento de entrada no SignatorDoc!\nExceção: "+exc.Message,"Atenção",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        private static bool checkLicense()
        {
#if DEBUG
            return true;
#endif
            try
            {
                string localTempPath = new Uri(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)).LocalPath + @"\Licensing";
                if (!System.IO.Directory.Exists(localTempPath))
                    System.IO.Directory.CreateDirectory(localTempPath);

                if (File.Exists(localTempPath + @"\License.lic"))
                {

                    LicenseSignatorDoc license = new LicenseSignatorDoc();
                    return license.checkIsValidLicense(localTempPath + @"\License.lic");
                }
                else
                {
                    if (File.Exists(localTempPath + @"\PreLicense.lic"))
                    {
                        bool preLicTrial = false;
                        using (Utils.LicenseSignatorDoc lic = new LicenseSignatorDoc())
                        {
                            preLicTrial = lic.checkIsValidTrial(localTempPath + @"\PreLicense.lic");
                        }

                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        FormLicense frmLicense = new FormLicense(false);

                        if (preLicTrial)
                        {
                            frmLicense.setConfTrial();
                        }

                        frmLicense.setLicenseFileNameDefaultPath(localTempPath + @"\PreLicense.lic");
                        Application.Run(frmLicense);
                    }
                    else
                    {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        FormLicense frmLicense = new FormLicense(true);
                        frmLicense.setLicenseFileNameDefaultPath(localTempPath + @"\PreLicense.lic");
                        Application.Run(frmLicense);
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Ocorreu um erro na tentativa de executar um procesimento!\n\nErro: " + exc.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
            return false;
        }

    }
}
