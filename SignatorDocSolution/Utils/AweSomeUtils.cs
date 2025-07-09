
namespace SignatorDocSolution.Utils
{
    public enum SignatureField_SealType 
    { 
        SigImageAll,
        StampAll,
        SigImageStampAll,
        SigImage,
        Stamp,
        SigImageStamp,
        SigImageUserName,
        StampUserName,
        SigImageStampUserName,
        SigImageUserNameDateTime,
        StampUserNameDateTime,
        SigImageStampUserNameDateTime,
        SigImageUserNameHash,
        StampUserNameHash,
        SigImageStampUserNameHash,
        SigImageDateTime,
        StampDateTime,
        SigImageStampDateTime,
        SigImageDateTimeHash,
        StampDateTimeHash,
        SigImageStampDateTimeHash,
        SigImageHash,
        StampHash,
        SigImageStampHash,
        SigInvisible
    }

    public static class Defins {
        internal static float pagesDPI = 136.0F;
        internal static System.Drawing.Size SignatureFieldSize= new System.Drawing.Size(300,160);
        internal static System.Drawing.Size pageSizeBase;
        internal static System.Drawing.Size SignatureField_Size_Base = new System.Drawing.Size(300, 200);
        internal static System.Drawing.Size SignatureField_Size_Itext = new System.Drawing.Size(159, 85);
        internal static System.Drawing.Rectangle rectPrimaryScreen = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
        internal static System.Drawing.Size initialFormSize = new System.Drawing.Size(800, 545);
        internal static string DefaultRootPath = new System.Uri(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)).LocalPath;
        internal static string SupportDir = DefaultRootPath + @"\Support";
        internal static string SignatorDocVersion = "1.0";

        internal static void setPagebaseSize(System.Drawing.Size size){
            pageSizeBase = size;
        }
        
    }
    public class CustomEvent { 
        public delegate void GenericDelegateEventHandler();
        public GenericDelegateEventHandler Invoker;
    }

    public static class HandleStructs {

        public static System.Collections.Generic.List<PDFDocInfo> RemoveDocInfoDuplicateList(System.Collections.Generic.List<PDFDocInfo> input)
        {
            System.Collections.Generic.List<PDFDocInfo> PDFDocInfoResult = new System.Collections.Generic.List<PDFDocInfo>();
            foreach (PDFDocInfo aux in input) {
                if (!PDFDocInfoResult.Contains(input.Find(dn => dn.documentName.Equals(aux.documentName))))
                {
                    PDFDocInfoResult.Add(aux);
                }
            }

            return PDFDocInfoResult;
        }
    }

    public static class AweSomeUtils {

        private static bool failLastOperation = false;
        public static bool closeApplication = false;
        private static string msgFailLastOperation = null;
        private static Form_PasswordRequest frm_PasswdRequest = null;
        public static bool isLockSignDevice=false;
        public static bool IsInitialFileArg = false;
        public static string InitialFileArgPath = null;

        public static System.Collections.Generic.List<string> Parse(string data, string delimiter)
        {
            if (data == null) return null;
            if (!delimiter.EndsWith("=")) delimiter = delimiter + "=";
            if (!data.Contains(delimiter)) return null;
            var result = new System.Collections.Generic.List<string>();
            int start = data.IndexOf(delimiter) + delimiter.Length;
            int length = data.IndexOf(',', start) - start;
            if (length == 0) return null; 
            if (length > 0)
            {
                result.Add(data.Substring(start, length));
                var rec = Parse(data.Substring(start + length), delimiter);
                if (rec != null) result.AddRange(rec); 
            }
            else
            {
                result.Add(data.Substring(start));
            }
            return result;
        }

        public static bool IsValidEmail(string email)
        {
            string MatchEmailPattern = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
                                     + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				                                [0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
                                     + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				                                [0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
                                     + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
            if (email != null) return System.Text.RegularExpressions.Regex.IsMatch(email, MatchEmailPattern);
            else return false;
        }

        public static iTextSharp.text.pdf.PdfReader OpenPdfReader(byte[] document, string password)
        {
            try
            {
                return password == null ? new iTextSharp.text.pdf.PdfReader(new iTextSharp.text.pdf.RandomAccessFileOrArray(document), (byte[])null) : new iTextSharp.text.pdf.PdfReader(new iTextSharp.text.pdf.RandomAccessFileOrArray(document), System.Text.Encoding.Default.GetBytes(password));
            }
            catch (iTextSharp.text.pdf.BadPasswordException ex)
            {
                if (string.IsNullOrEmpty(password))
                {
                    throw new System.Exception("ERROR-9924. Error on opening the pdf: Read-password (user-password) required. Error" + ex.Message);
                }
                else
                {
                    throw new System.Exception("ERROR-9923. Error on opening the pdf: The provided read-password (user-password) is wrong. Error" + ex.Message);
                }
            }
        }

        public static bool checkWhetherFailLastOperation() {
            if (failLastOperation)
            {
                failLastOperation = false;
                return true;
            }
            else
                return false;

        }
        public static  void setFailLastOperation(bool failOperation) {
            failLastOperation = failOperation;
        }
        public static void setMsgFailLastOperation(string msgLastFailOP) {
            msgFailLastOperation = msgLastFailOP;
        }
        public static bool getIsFailLastOperation() {
            return failLastOperation;
        }
        public static string getMsgFailLastOperation() { 
            string msg=msgFailLastOperation;
            msgFailLastOperation = null;
            return msg;

        }

        #region Handle Passwords Encryption Document
        // Begin handleEncryption
        public static string getDocPassToOpen(string fileName)
        {
            string resPasswd = AweSomeUtils.ShowPasswordDialog("O documento \"" + System.IO.Path.GetFileNameWithoutExtension(fileName) + "\" está protegido com senha de visualização, para abrí-lo é necessário digitá-la.", "Abertura de Documento");
            if (resPasswd.Length == 0)
                return "`\01´";

            try
            {
                using (iTextSharp.text.pdf.PdfReader pdfReader = new iTextSharp.text.pdf.PdfReader(fileName, System.Text.Encoding.Default.GetBytes(resPasswd)))
                {
                    pdfReader.Close();
                    return resPasswd;
                }
            }
            catch (iTextSharp.text.pdf.BadPasswordException bpexc)
            {
                return "`\02´";
            }
            catch (System.Exception exc)
            {
                throw new System.Exception(exc.Message);
            }
        }

        public static string getDocPassToEdit(string fileName)
        {
            string resPasswd = AweSomeUtils.ShowPasswordDialog("O documento \"" + System.IO.Path.GetFileNameWithoutExtension(fileName) + "\" está protegido com senha contra alteração, para assiná-lo será necessário digitá-la.", "Abertura de Documento");
            if (resPasswd.Length == 0)
                return "`\01´";

            try
            {
                using (iTextSharp.text.pdf.PdfReader pdfReader = new iTextSharp.text.pdf.PdfReader(fileName, System.Text.Encoding.Default.GetBytes(resPasswd)))
                {
                    if (pdfReader.IsOpenedWithFullPermissions)
                    {
                        pdfReader.Close();
                        return resPasswd;
                    }
                    else
                    {
                        throw new iTextSharp.text.pdf.BadPasswordException("Bad user password");
                    }
                }
            }
            catch (iTextSharp.text.pdf.BadPasswordException bpexc)
            {
                return "`\02´";
            }
            catch (System.Exception exc)
            {
                throw new System.Exception(exc.Message);
            }

        }

        public static bool checkIsNeedPassOpenDoc(string fileName)
        {
            try
            {
                using (iTextSharp.text.pdf.PdfReader pdfReader = new iTextSharp.text.pdf.PdfReader(fileName))
                {
                    pdfReader.Close();
                    return false;
                }
            }
            catch (iTextSharp.text.pdf.BadPasswordException bpexc)
            {
                return true;
            }
            catch (System.Exception exc)
            {
                throw new System.Exception(exc.Message);
            }
        }

        public static int checkIsDocPassEditNeed(string fileName, bool isNeedPasswdOpenDoc, string password)
        {
            int res = 0;

            try
            {
                using (iTextSharp.text.pdf.PdfReader pdfReader = isNeedPasswdOpenDoc ? new iTextSharp.text.pdf.PdfReader(fileName, System.Text.Encoding.Default.GetBytes(password)) : new iTextSharp.text.pdf.PdfReader(fileName))
                {
                    if (pdfReader.IsEncrypted() && (!pdfReader.IsOpenedWithFullPermissions))
                        res = 1;
                    else
                        res = 0;
                    pdfReader.Close();
                }
            }
            catch (iTextSharp.text.pdf.BadPasswordException bpexc)
            {
                res = 2;
            }
            return res;
        }

        public static string getIsDocPassGeneric(string fileName)
        {
            string resPasswd = null;

            try
            {
                using (iTextSharp.text.pdf.PdfReader pdfReader = new iTextSharp.text.pdf.PdfReader(fileName))
                {
                    if (!pdfReader.IsOpenedWithFullPermissions)
                        resPasswd = "`\01´";
                    else
                        resPasswd = "`\0´";
                    pdfReader.Close();
                }

                if (resPasswd == "`\01´")
                {
                    resPasswd = AweSomeUtils.ShowPasswordDialog("O documento \"" + System.IO.Path.GetFileNameWithoutExtension(fileName) + "\" está protegido com senha contra alteração, para assiná-lo será necessário digitá-la.", "Abertura de Documento");
                    if (resPasswd.Length == 0)
                        return "`\01´";
                    using (iTextSharp.text.pdf.PdfReader pdfReader = new iTextSharp.text.pdf.PdfReader(fileName, System.Text.Encoding.Default.GetBytes(resPasswd)))
                    {
                        pdfReader.Close();
                    }
                }
            }
            catch (iTextSharp.text.pdf.BadPasswordException bpexc)
            {
                try
                {
                    if (resPasswd == null)
                    {
                        resPasswd = AweSomeUtils.ShowPasswordDialog("O documento \"" + System.IO.Path.GetFileNameWithoutExtension(fileName) + "\" está protegido com senha contra alteração, para assiná-lo será necessário digitá-la.", "Abertura de Documento");
                        if (resPasswd.Length == 0)
                            return "`\01´";
                        using (iTextSharp.text.pdf.PdfReader pdfReader = new iTextSharp.text.pdf.PdfReader(fileName, System.Text.Encoding.Default.GetBytes(resPasswd)))
                        {
                            if (!pdfReader.IsOpenedWithFullPermissions)
                            {
                                resPasswd = "`\03´";
                            }
                            pdfReader.Close();
                        }

                        if (resPasswd == "`\03´")
                        {
                            resPasswd = AweSomeUtils.ShowPasswordDialog("A senha digitada não tem permissão para inserir assinaturas no documento. Para assiná-lo é necessário digitar uma senha com permissão para edição do documento.", "Abertura de Documento");
                            if (resPasswd.Length == 0)
                                return "`\01´";
                            using (iTextSharp.text.pdf.PdfReader pdfReader = new iTextSharp.text.pdf.PdfReader(fileName, System.Text.Encoding.Default.GetBytes(resPasswd)))
                            {
                                if (!pdfReader.IsOpenedWithFullPermissions)
                                {
                                    pdfReader.Close();
                                    throw new iTextSharp.text.pdf.BadPasswordException("Bad user password");
                                }
                                else
                                    pdfReader.Close();
                            }
                        }
                    }
                    else
                        return "`\02´";
                }
                catch (iTextSharp.text.pdf.BadPasswordException bpexc2)
                {
                    return "`\02´";
                }
                catch (System.Exception exc0)
                {
                    throw new System.Exception(exc0.Message);
                }
            }
            catch (System.Exception exc)
            {
                throw new System.Exception(exc.Message);
            }

            return resPasswd;
        }
        // End HandleEncryption
        #endregion

        public static string ShowPasswordDialog(string msg, string title)
        { 
            string passwd="";
            Form_PasswordRequest.ShowCustomDialog(frm_PasswdRequest, msg,title, ref passwd);
            return passwd;
        }

        public static void ReleaseWhenExit()
        {
            frm_PasswdRequest.Dispose();
        }

        public static void InstanceForm_PasswordRequest() {
            frm_PasswdRequest = new Form_PasswordRequest();
        }

    }

    public static class Configurations {
        public static bool Documents_DisableCheckPassword = true;
        public static bool isAsposeLicenseInit = false;

        public static void checkConfigurations()
        {
            string xmlPath = Defins.SupportDir + @"\SignatorDoc.settings";
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            if (System.IO.File.Exists(xmlPath))
            {
                try
                {
                    xmlDoc.Load(xmlPath);
                    System.Xml.XmlNode xmlRaizSignatorDoc = xmlDoc.SelectSingleNode("/SignatorDoc");
                    System.Xml.XmlNode xmlNodeDisableCheckPasswords = xmlRaizSignatorDoc.SelectSingleNode("Configuration/Documents/DisableCheckPassword");
                    if (xmlNodeDisableCheckPasswords != null)
                    {
                        Documents_DisableCheckPassword = xmlNodeDisableCheckPasswords.Attributes["Value"].Value.Equals("1");
                    }
                    else
                    {
                        xmlDoc = getInitialConfiguration();
                        xmlDoc.Save(xmlPath);
                    }
                }
                catch (System.Xml.XmlException xmlexc)
                {
                    xmlDoc = getInitialConfiguration();
                    xmlDoc.Save(xmlPath);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Um arquivo de configurações do SignatorDoc foi deletado da pasta de instalação do sistema. Algumas configurações serão restauradas para o estado inicial.", "Configurações", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                xmlDoc = getInitialConfiguration();
                xmlDoc.Save(xmlPath);
            }
        }

        public static void writeConfigurations()
        {
            string xmlPath = Defins.SupportDir + @"\SignatorDoc.settings";
            System.Xml.XmlDocument xmlDoc = getInitialConfiguration();
            if (System.IO.File.Exists(xmlPath))
            {
                try
                {
                    xmlDoc.Load(xmlPath);
                    System.Xml.XmlNode xmlRaizSignatorDoc = xmlDoc.SelectSingleNode("/SignatorDoc");
                    System.Xml.XmlNode xmlNodeDisableCheckPasswords = xmlRaizSignatorDoc.SelectSingleNode("Configuration/Documents/DisableCheckPassword");
                    if (xmlNodeDisableCheckPasswords != null)
                    {
                        xmlNodeDisableCheckPasswords.Attributes["Value"].Value = Documents_DisableCheckPassword ? "1" : "0";
                    }
                    else
                    {
                        xmlDoc = getInitialConfiguration();                     
                    }
                    xmlDoc.Save(xmlPath);
                }
                catch (System.Xml.XmlException xmlexc)
                {
                    xmlDoc = getInitialConfiguration();
                    xmlDoc.Save(xmlPath);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Um arquivo de configurações do SignatorDoc foi deletado da pasta de instalação do sistema. Algumas configurações serão restauradas para o estado inicial.", "Configurações", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                xmlDoc = getInitialConfiguration();
                xmlDoc.Save(xmlPath);
            }
        }

        private static System.Xml.XmlDocument getInitialConfiguration()
        {
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

            System.Xml.XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.InsertBefore(xmlDeclaration, xmlDoc.DocumentElement);

            System.Xml.XmlElement rootNode = xmlDoc.CreateElement("SignatorDoc");
            rootNode.InnerXml = "<Configuration><Documents><DisableCheckPassword Value=\"1\"/></Documents></Configuration>";

            xmlDoc.AppendChild(rootNode);
            return xmlDoc;
        }

        public static string getLicense()
        {
                return System.Text.Encoding.Default.GetString(System.Convert.FromBase64String("UHV0IHlvdXIgVmFsaWQgUERGTkVUIExpY2Vuc2U="));
        }

        private static string getAsposeLicense() { 
            return "UHV0IHlvdXIgVmFsaWQgQVNQT1NFIExpY2Vuc2U=";            
        }

        public static bool initAposeLicense() {
            try
            {
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Convert.FromBase64String(getAsposeLicense())))
                {
                    new Aspose.Words.License().SetLicense(ms);
                }
                isAsposeLicenseInit = true;
                return true;
            }catch(System.Exception exc){
                System.Windows.Forms.MessageBox.Show("Não foi possível iniciar a configuração do Aspose! O documento não pode ser convertido.\n\nErro:"+exc.Message,"Conversão de Documento", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
        }
    }

    public static class ConverterDocFormats {
        public static string listErrorDocs = null;

        public static bool ConvertWordToPdf(string inputFile, string outputFileName)
        {
            try
            {
                return ConvertWordToPdf_V1(inputFile, outputFileName);
            }
            catch (System.Exception exc)
            {
                try
                {
                    return ConvertWordToPdf_V2(inputFile, outputFileName);
                }
                catch (System.Exception exc2)
                {
                    listErrorDocs += "O documento \"" + System.IO.Path.GetFileName(inputFile) + "\" não pode ser convertido para pdf. \nERRO: " + exc2.Message+"\n\n";
                }
                return false;
            }
        }

        public static bool ConvertWordToPdf_V1(string inputFile, string outputFileName)
        {

            //Microsoft.Office.Interop.Word.ApplicationClass wordApp = null;
            //Microsoft.Office.Interop.Word.Document wordDoc = null;
            //object inputFileTemp = null;
            //try
            //{
            //    wordApp = new Microsoft.Office.Interop.Word.ApplicationClass();
            //    wordDoc = null;
            //    inputFileTemp = inputFile;
            //}
            //catch (System.Exception exc)
            //{
            //    throw new System.Exception(exc.Message);
            //}

            //try
            //{
            //    wordDoc = wordApp.Documents.Open(ref inputFileTemp);
            //    wordDoc.ExportAsFixedFormat(outputFileName, Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF);
            //    if (System.IO.File.Exists(outputFileName + ".pdf"))
            //        System.IO.File.Move(outputFileName + ".pdf", outputFileName);
            //}
            //catch (System.Exception exc)
            //{
            //    throw new System.Exception(exc.Message);
            //}
            //finally
            //{
            //    if (wordDoc != null)
            //    {
            //        wordDoc.Close(Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges);
            //    }
            //    if (wordApp != null)
            //    {
            //        wordApp.Quit(Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges);
            //        wordApp = null;
            //    }
            //}

            return true;
        }

        public static bool ConvertWordToPdf_V2(string inputFile, string outputFileName)
        {

            try
            {
                if (!Configurations.isAsposeLicenseInit)
                {
                    if (!Configurations.initAposeLicense())
                        return false;
                }

                Aspose.Words.Document doc = new Aspose.Words.Document(inputFile);
                doc.Save(outputFileName+".pdf");

                if (System.IO.File.Exists(outputFileName + ".pdf"))
                    System.IO.File.Move(outputFileName + ".pdf", outputFileName);
                
                return true;
                    
                //using (pdftron.PDF.PDFDoc pdfdoc = new pdftron.PDF.PDFDoc())
                //{

                //    pdftron.PDF.Convert.ToPdf(pdfdoc, inputFile);
                //    if (System.IO.File.Exists(outputFileName))
                //        System.IO.File.Delete(outputFileName);

                //    pdfdoc.Save(outputFileName, pdftron.SDF.SDFDoc.SaveOptions.e_linearized);
                //    pdfdoc.Close();
                //    return true;
                //}
            }
            catch (System.Exception exc)
            {
                throw new System.Exception(exc.Message);
            }
        }
            
    }

    public struct PairItemsInt { public int Item; public int Value;}
}
