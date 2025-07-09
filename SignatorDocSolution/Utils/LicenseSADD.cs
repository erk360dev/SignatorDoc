
using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
namespace SignatorDocSolution.Utils
{
    /// <summary>
    /// CLASS_ID=10;
    /// </summary>
    class LicenseSignatorDoc : IDisposable
    {
        private string tempLicense = null;
        private string internal_License = null;
        private string defaultPreLicenseFileName = null;
        private string definitiveLicenceEncriptedB64 = null;
        private string patternBegin = "IK3L9", patternsep = "`", patternend = "E3G7M",internalPatternBegin= "ZP4L3", internalPatternEnd="J45MU";
        private bool disposed;
        private int daysToExpire = 0;

        public string getPreLicense() {

            this.generateLicense();

            return this.tempLicense;
        }

        public string getPreInternalLicense() {
            return this.internal_License;
        }

        private void generateLicense()
        {
            
            string CID = null;
            try
            {
                ComputerIdentifier computerID = new ComputerIdentifier();
                CID = computerID.getFingerPrint();
            }
            catch (Exception ex) { 
                MessageBox.Show(ex.Message, "An error occured in getting Machine Information.", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }

            string uniqueKey = GetUniqueKey(16);

            DateTime tNow = DateTime.Now;

            this.tempLicense = this.patternBegin + this.patternsep + CID + this.patternsep + uniqueKey + this.patternsep + tNow.ToString("yyyyMMddhhmmss") + this.patternsep + this.patternend;

            this.internal_License = this.internalPatternBegin + this.patternsep + CID + this.patternsep + uniqueKey + this.patternsep + tNow.ToString("yyyyMMddhhmmss") + this.patternsep + this.internalPatternEnd;
            
        }

        public bool processLicense(byte[] encryptedLicense) {
            try
            {
                RSACryptoServiceProvider rsaDEncriptor = (RSACryptoServiceProvider)this.configureDefaultCertificate().PrivateKey;

                byte[] decryptedLicence = rsaDEncriptor.Decrypt(encryptedLicense, false);
                string finalLicenseDecrypted = Encoding.Default.GetString(decryptedLicence);

                string[] licenseParams = finalLicenseDecrypted.Split('|');

                if (licenseParams.Length > 5)
                    if (licenseParams[5].EndsWith("def"))
                        return processDefinitiveLicense(licenseParams);

                string[] prelicenseParam = this.getPrelicenseGentd().Split('`');

                if ((!prelicenseParam[0].Equals(this.internalPatternBegin)) && (!prelicenseParam[4].Equals(this.internalPatternEnd)))
                {
                    MessageBox.Show("Não foi possível fazer o licenciamento do sistema. O pedido de licença não é mais válido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                bool validLicense = false;

                string CID = string.Empty;

                if (licenseParams.Length == 5)
                {
                    if (prelicenseParam.Length == 5)
                    {
                        try
                        {
                            ComputerIdentifier computerID = new ComputerIdentifier();
                            CID = computerID.getFingerPrint();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "An error occured in getting Machine Information.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        if (licenseParams[0].EndsWith(CID))
                            if (licenseParams[1].EndsWith(prelicenseParam[2]))
                                if (licenseParams[2].EndsWith(prelicenseParam[3]))
                                    validLicense = true;
                                else
                                    return false;
                    }
                }


                long daysRest;

                long.TryParse(licenseParams[4].Substring(7).Split('@')[0], out daysRest);

                string[] paramDateExpire = licenseParams[3].Substring(7).Split('-');
                string dateExpire = paramDateExpire[0] + paramDateExpire[1] + paramDateExpire[2];

                DateTime today = DateTime.Now;


                if (today > (new DateTime(int.Parse(paramDateExpire[0]), int.Parse(paramDateExpire[1]), int.Parse(paramDateExpire[2]))))
                    validLicense = false;


                if (validLicense)
                {
                    string definitiveLicense = GetUniqueKey(7) + "`" + CID + "`" + today.AddDays(daysRest).ToString("yyyy-MM-dd") + "`" + daysRest + "`ED7M3";

                    byte[] encodedDefinitiveLicense = Encoding.Default.GetBytes(definitiveLicense);

                    RSACryptoServiceProvider rsaEncriptor = new RSACryptoServiceProvider();
                    rsaEncriptor.ImportParameters(((RSACryptoServiceProvider)this.configureDefaultCertificate().PrivateKey).ExportParameters(false));

                    byte[] definitiveEncriptedLicense = rsaEncriptor.Encrypt(encodedDefinitiveLicense, false);
                    this.definitiveLicenceEncriptedB64 = "CID:" + CID + "\r\nLicense:" + Convert.ToBase64String(definitiveEncriptedLicense);
                    return true;
                }
            }
            catch (CryptographicException cexc) {
                MessageBox.Show("Não foi possível importar a licença selecionada. Um ou mais parâmetros desta licença podem estar corrompidos ou não correspondem ao pedido de licença gerado. Neste processo o sistema gerou a seguinte exceção:" + cexc.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception exc) {
                MessageBox.Show("Não foi possível importar a licença selecionada. Um ou mais parâmetros desta licença podem estar corrompidos ou não correspondem ao pedido de licença gerado. Neste processo o sistema gerou a seguinte exceção:" + exc.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }

        public bool processDefinitiveLicense(string [] licParams) {
            try
            {
                if (licParams.Length == 6)
                {
                    ComputerIdentifier computerID = new ComputerIdentifier();
                    string CID = computerID.getFingerPrint();
                    if (licParams[0].EndsWith(CID) && licParams[4].EndsWith("ZXCER"))
                    {
                        DateTime today = DateTime.Now;
                        string definitiveLicense = GetUniqueKey(7) + "`" + CID + "`" + today.AddDays(11 * 365).ToString("yyyy-MM-dd") + "`" + (11 * 365) + "`ED7M3";

                        byte[] encodedDefinitiveLicense = Encoding.Default.GetBytes(definitiveLicense);

                        RSACryptoServiceProvider rsaEncriptor = new RSACryptoServiceProvider();
                        rsaEncriptor.ImportParameters(((RSACryptoServiceProvider)this.configureDefaultCertificate().PrivateKey).ExportParameters(false));

                        byte[] definitiveEncriptedLicense = rsaEncriptor.Encrypt(encodedDefinitiveLicense, false);
                        this.definitiveLicenceEncriptedB64 = "CID:" + CID + "\r\nLicense:" + Convert.ToBase64String(definitiveEncriptedLicense);
                        return true;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "An error occured in getting Machine Information.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }

        private string getPrelicenseGentd()
        {
            string fileLicense = System.IO.File.ReadAllText(defaultPreLicenseFileName, Encoding.ASCII);

            string licenseHex = Encoding.Default.GetString(Convert.FromBase64String(fileLicense));

            int charsCount = licenseHex.Length;
            byte[] bytes = new byte[charsCount / 2];
            for (int i = 0; i < charsCount; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(licenseHex.Substring(i, 2), 16);
            }

            return System.Text.Encoding.Default.GetString(bytes);
        }

        public string getDefinitiveLicenseToSaveFile() {
            return this.definitiveLicenceEncriptedB64;
        }

        public bool checkIsValidLicense(string licenseFileName) {

            try
            {
                string[] licenceContent = System.IO.File.ReadAllText(licenseFileName, Encoding.ASCII).Split(new string[] { "\r\n", "CID:", "License:" }, StringSplitOptions.RemoveEmptyEntries);
                RSACryptoServiceProvider rsaDEncriptor = (RSACryptoServiceProvider)this.configureDefaultCertificate().PrivateKey;

                byte[] decryptedLicence = rsaDEncriptor.Decrypt(Convert.FromBase64String(licenceContent[1]), false);
                string[] licenseParams = Encoding.Default.GetString(decryptedLicence).Split('`');
                
                string[] dateParams = licenseParams[2].Split('-');
                int restDays = int.Parse(licenseParams[3]);
                DateTime dateExpire = new DateTime(int.Parse(dateParams[0]), int.Parse(dateParams[1]), int.Parse(dateParams[2]));


                if (!licenseParams[1].Equals(licenceContent[0]))
                {
                    if (MessageBox.Show("Esta licença não corresponde a atual instalação do sistema. Gostaria de removê-la?", "Licenciamento SignatorDoc", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (System.IO.File.Exists(licenseFileName))
                            System.IO.File.Delete(licenseFileName);
                        MessageBox.Show("Licença removida com sucesso. Reinicie o programa para gerar um novo pedido de licença", "Licenciamento SignatorDoc", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return false;
                }


                if (restDays >= 0)
                {
                    DateTime today= DateTime.Now.Date;
                    DateTime auxBaseDate = dateExpire.AddDays(-restDays);
                    if (auxBaseDate != today)
                    {
                        if (dateExpire < today)
                        {
                            if (MessageBox.Show("A licença do SignatorDoc está vencida. Entre em contado com o fornecedor do sistema. Remover licença vencida?", "Licenciamento SignatorDoc", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                if(System.IO.File.Exists(licenseFileName))
                                    System.IO.File.Delete(licenseFileName);
                                MessageBox.Show("Licença removida com sucesso. Reinicie o programa para gerar um novo pedido de licença", "Licenciamento SignatorDoc", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            
                            return false;
                        }

                        restDays--;

                        if (restDays < 0) {
                            if (MessageBox.Show("A licença do SignatorDoc está vencida. Entre em contado com o fornecedor do sistema. Remover licença vencida?", "Licenciamento SignatorDoc", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                if (System.IO.File.Exists(licenseFileName))
                                    System.IO.File.Delete(licenseFileName);
                                MessageBox.Show("Licença removida com sucesso. Reinicie o programa para gerar um novo pedido de licença", "Licenciamento SignatorDoc", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        
                        }

                        string updateLicense = licenseParams[0] + "`" + licenseParams[1] + "`" + licenseParams[2] + "`" + restDays + "`" + licenseParams[4];
                        byte[] encodedLicense = Encoding.Default.GetBytes(updateLicense);

                        RSACryptoServiceProvider rsaEncriptor = new RSACryptoServiceProvider();
                        rsaEncriptor.ImportParameters(((RSACryptoServiceProvider)this.configureDefaultCertificate().PrivateKey).ExportParameters(false));

                        byte[] EncriptedLicense = rsaEncriptor.Encrypt(encodedLicense, false);
                        string licenseToWrite = "CID:" + licenceContent[0] + "\r\nLicense:" + Convert.ToBase64String(EncriptedLicense);
                        System.IO.File.WriteAllText(licenseFileName, licenseToWrite, Encoding.ASCII);

                    }
                    return true;
                }
                else {
                    if (MessageBox.Show("A licença do SignatorDoc está vencida. Entre em contado com o fornecedor do sistema. Remover licença vencida?", "Licenciamento SignatorDoc", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (System.IO.File.Exists(licenseFileName))
                            System.IO.File.Delete(licenseFileName);
                        MessageBox.Show("Licença removida com sucesso. Reinicie o programa para gerar um novo pedido de licença", "Licenciamento SignatorDoc", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return false;
                }

            }
            catch (CryptographicException cexc) {
                MessageBox.Show("Não foi possível verificar a licença de utilização do sistema. Um ou mais parametros não puderam ser identificados. O processo gerou a seguinte exceção: " + cexc.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Não foi possível verificar a licença de utilização do sistema. Um ou mais parametros não puderam ser identificados. O processo gerou a seguinte exceção: " + exc.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        public void setDefaultPreLicenseFileName(string filenamePrelicence) {
            this.defaultPreLicenseFileName = filenamePrelicence;
        }

        public string GetUniqueKey(int maxSize)
        {
            char[] chars = new char[72];
            chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@$?%&/+_-".ToCharArray();
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            data = new byte[maxSize];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }

        private System.Security.Cryptography.X509Certificates.X509Certificate2 configureDefaultCertificate()
        {
            string certDefaultB64 = "MIIDBjCCAe6gAwIBAgIQFwx5n3AahplK3Xx4Nd1I+DANBgkqhkiG9w0BAQsFADAWMRQwEgYDVQQDDAtTaWduYXRvckRvYzAeFw0yNTA3MDkwMzQ3MDNaFw0yNjA3MDkwNDA3MDNaMBYxFD" +
                                    "ASBgNVBAMMC1NpZ25hdG9yRG9jMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAufBc4GVt3MUOO7oDhocnjkV7ZuhDi16Z+08mMEKjlusALuGThYJwY5Q4LBMe04232ny9S" +
                                    "vsDBnRUj5DUG4hwQ8TcuIM0NaoxqeDKJ/hIbaMxxAp6T8Trh9Y/nSpp2H54wI49TBHwBT1Z4ucDbWqcunvOyB2seYCrH/dt6aIvfEbvG84qQt7I19AQ4fI24igpTXYZYbE2GPUaJ/Bt" +
                                    "Q05rdTe3tgMNeqBz9LUVKVvbgRH+YkACK2DpqK0Ru02Cig6SZKKGRUHQpWRdLXuaWplEs6K7yLARhKbnR6Gml7zI8k1gAKGS+aF8gEqd5eSVvw3Ic1lz8M2CE6bxphCcSTvFQQIDAQA" +
                                    "Bo1AwTjAOBgNVHQ8BAf8EBAMCBaAwHQYDVR0lBBYwFAYIKwYBBQUHAwIGCCsGAQUFBwMBMB0GA1UdDgQWBBS9kD0aEm/YmO8esavms4vlWdtWWTANBgkqhkiG9w0BAQsFAAOCAQEAAI" +
                                    "LLhkWaW+6eijm3mJAAMw0fzGJr/rP+yEz0gvXYm9mfONolRw0qS3LI+nkv5HNVTaO6HzMicAfj6g2Ha2DVAFVxBB2doK6NfmIOBCf/JjJeldxVYJXn+iGCQHH/rir2bBiCH6A9oHeJg" +
                                    "ZGNzz80a/7iZeKS1P571+5rSwUdyuACXvTtltFDPHGdU8VVIkAWZMyPJHGOBnsEDLcFtCXn0K2KxmhBLUwFZZvdFYXWrvnymQcPJ8ip43zlNXgwZX9syVbXT+WG7RlbxtR9Wotq4j7b" +
                                    "qLXJnonGBs9rKfvL5jQBjOUjntEA9ErvyuwYDm45hZ7Tm1WmluMClQUhH36mzQ==";
            return new System.Security.Cryptography.X509Certificates.X509Certificate2(System.Convert.FromBase64String(certDefaultB64));
        }

        public bool checkIsValidTrial(string licenseFileName)
        {
            string[] licenceContent = System.IO.File.ReadAllText(licenseFileName, Encoding.ASCII).Split(new string[] { "\r\n", "CID:", "License:" }, StringSplitOptions.RemoveEmptyEntries);
            if ((licenceContent.Length == 2) && (licenceContent[0].Equals("TRIAL")))
            {
                RSACryptoServiceProvider rsaDEncriptor = (RSACryptoServiceProvider)this.configureDefaultCertificate().PrivateKey;

                byte[] decryptedLicence = rsaDEncriptor.Decrypt(Convert.FromBase64String(licenceContent[1]), false);
                string[] licenseParams = Encoding.Default.GetString(decryptedLicence).Split('|');
                string[] auxTrialInf = licenseParams[4].Split('@');
                int.TryParse(auxTrialInf[0].Split('>')[1], out this.daysToExpire);

                if (auxTrialInf[1].Equals("TRIAL") && (this.daysToExpire <= 30) && (licenseParams[1].Contains("use")))
                {
                    if (this.checkRegistryIsValidTrial())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool checkRegistryIsValidTrial()
        {
            Microsoft.Win32.RegistryKey SignatorDocReg = null;

            SignatorDocReg = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("Classes\\CompanyDoc\\Setup\\SignatorDoc");

            if (SignatorDocReg != null)
            {
                string regValue = SignatorDocReg.GetValue("Permission").ToString();
                if (regValue.Equals("SignatorDoc"))
                    return true;
            }
            return false;
        }

        public bool processTrialLicense()
        {
            if (checkIsValidTrial(this.defaultPreLicenseFileName))
            {
                string CID = string.Empty;
                DateTime today = DateTime.Now;

                try
                {
                    ComputerIdentifier computerID = new ComputerIdentifier();
                    CID = computerID.getFingerPrint();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "An error occured in getting Machine Information.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                string definitiveLicense = GetUniqueKey(7) + "`" + CID + "`" + today.AddDays(this.daysToExpire).ToString("yyyy-MM-dd") + "`" + this.daysToExpire + "`ED2M4";

                byte[] encodedDefinitiveLicense = Encoding.Default.GetBytes(definitiveLicense);

                RSACryptoServiceProvider rsaEncriptor = new RSACryptoServiceProvider();
                rsaEncriptor.ImportParameters(((RSACryptoServiceProvider)this.configureDefaultCertificate().PrivateKey).ExportParameters(false));

                byte[] definitiveEncriptedLicense = rsaEncriptor.Encrypt(encodedDefinitiveLicense, false);
                this.definitiveLicenceEncriptedB64 = "CID:" + CID + "\r\nLicense:" + Convert.ToBase64String(definitiveEncriptedLicense);
                return true;
            }
            return false;
        }

        #region Dispose
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~LicenseSignatorDoc()
        {
            this.Dispose(false);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources here.
                }

                // Dispose unmanaged resources here.
            }

            disposed = true;
        }

        public void DisposeContext()
        {
            this.Dispose();
        }
        #endregion  

    }
}
