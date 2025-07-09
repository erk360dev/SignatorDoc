using SignatorDocSolution.Utils;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SignatorDocSolution
{
    /// <summary>
    /// CLASS_ID=09;
    /// </summary>
    class DocSigner
    {
        private X509Certificate2 _X509Certificate2 = null;
        private string SignerName = "SignatorDoc";
        private float fieldStarPosition_X = 0;
        private float fieldStarPosition_Y = 0;
        private float fieldStarPosition_Width = 0;
        private float fieldStarPosition_Heigh = 0;
        private string signedFilePath = null;
        private string processTempFilePath = null;
        private List<string> SignedMassiveDocs = new List<string>();
        private bool showUserName = false;
        private bool showDateTime = false;
        private bool showTextInField = false;
        private bool showUserDigitalID = false;
        private bool isExceptionThrowed = false;
        private string MessageException = null;
        private int countTrySign = 0;
        public bool cancelOperation = false;
        public string fileInProccessTempName = "_InProcess.tmp";
        public Dictionary<string, string> ListMassiveNotSigned = new Dictionary<string, string>();

        public DocSigner() {
            this._X509Certificate2 = this.configureDefaultCertificate();
        }

        public void setX509Certificate2(X509Certificate2 x509certificate2){
            if (x509certificate2 == null)
                this._X509Certificate2 = configureDefaultCertificate();
            else
                this._X509Certificate2 = x509certificate2;
        }

        public bool SignDocument(string outTempDirectory, string fileNamePath, SignatureField signatureField, float pageDPIs, string password)
        {
            return SignDocument_V2(outTempDirectory, fileNamePath, signatureField, pageDPIs, password);
        }

        public bool SignDocument_V2(string outTempDirectory, string fileNamePath, SignatureField signatureField, float pageDPIs, string password)
        {
            Org.BouncyCastle.X509.X509CertificateParser x509CertificateParser = new Org.BouncyCastle.X509.X509CertificateParser();
            Org.BouncyCastle.X509.X509Certificate[] x509CertificateChain = new Org.BouncyCastle.X509.X509Certificate[] { x509CertificateParser.ReadCertificate(this._X509Certificate2.RawData) };
            int C_Size = 8192;

            this.signedFilePath = outTempDirectory + @"\" + Path.GetFileNameWithoutExtension(fileNamePath) + ".temp";
            this.processTempFilePath = outTempDirectory + @"\" + Path.GetFileNameWithoutExtension(fileNamePath) + "InProcess.temp";

            PdfReader pdfReader = null;
            PdfStamper pdfStamper = null;
            PdfSignatureAppearance pdfSignatureAppaerance = null;
            try
            {
                using (FileStream outFileStream = new FileStream(this.signedFilePath, FileMode.Create))
                {
                    pdfReader = password == null ? new PdfReader(fileNamePath) : new PdfReader(fileNamePath, System.Text.Encoding.Default.GetBytes(password));
                    #region ATTEMPT CREATE PDF STAMPER
                    try
                    {
                        pdfStamper = PdfStamper.CreateSignature(pdfReader, outFileStream, '\0', processTempFilePath, true);
                    }
                    catch (DocumentException dcexcp) {
                        if (dcexcp.Message.Equals("Append mode requires a document without errors even if recovery was possible."))
                        {
                            pdfReader.Close();
                            pdfReader = password == null ? new PdfReader(fileNamePath) : new PdfReader(fileNamePath, System.Text.Encoding.Default.GetBytes(password));
                            processTempFilePath = processTempFilePath + "2";
                            pdfStamper = PdfStamper.CreateSignature(pdfReader, outFileStream, '\0', processTempFilePath, false);
                        }
                        else
                            throw new iTextSharp.text.DocumentException(dcexcp.Message);
                    }
                    #endregion
                    pdfSignatureAppaerance = pdfStamper.SignatureAppearance;

                    if (signatureField.SigFieldSealType == SignatureField_SealType.SigInvisible)
                    {
                        pdfSignatureAppaerance.SetVisibleSignature( new Rectangle(0,0,0,0),1, "Assinatura " + (pdfStamper.AcroFields.Fields.Count + 1));
                        pdfSignatureAppaerance.SignDate = DateTime.Now;
                    }
                    else
                    {
                        fieldStarPosition_X = (signatureField.Location.X / pageDPIs) * 72.0F;
                        fieldStarPosition_Y = (((signatureField.Parent.Height - signatureField.Location.Y) - signatureField.Size.Height) / pageDPIs) * 72.0F;
                        fieldStarPosition_Width = (signatureField.Size.Width / pageDPIs) * 72.0F;
                        fieldStarPosition_Heigh = (signatureField.Size.Height / pageDPIs) * 72.0F;

                        this.showUserName = false;
                        this.showDateTime = false;
                        this.showUserDigitalID = false;
                        this.showTextInField = false;

                        if ((signatureField.SigFieldSealType.ToString().Contains("UserName")) || (signatureField.SigFieldSealType.ToString().Contains("All")))
                        {
                            this.showUserName = true;
                            this.showTextInField = true;
                        }
                        if ((signatureField.SigFieldSealType.ToString().Contains("DateTime")) || (signatureField.SigFieldSealType.ToString().Contains("All")))
                        {
                            this.showDateTime = true;
                            this.showTextInField = true;
                        }
                        if ((signatureField.SigFieldSealType.ToString().Contains("Hash")) || (signatureField.SigFieldSealType.ToString().Contains("All")))
                        {
                            this.showUserDigitalID = true;
                            this.showTextInField = true;
                        }

                        pdfSignatureAppaerance.SetVisibleSignature(new Rectangle(fieldStarPosition_X,
                                                                            fieldStarPosition_Y,
                                                                            fieldStarPosition_X + fieldStarPosition_Width,
                                                                            fieldStarPosition_Y + fieldStarPosition_Heigh),
                                                                            signatureField.getPageNumber(),
                                                                            "Assinatura " + (pdfStamper.AcroFields.Fields.Count + 1));

                        DateTime dateTime = DateTime.Now;

                        if (this.showTextInField)
                        {
                            PdfTemplate pdfTemplate_n0 = pdfSignatureAppaerance.GetLayer(0);
                            float tptn0Width = pdfTemplate_n0.BoundingBox.Width;
                            float tptn0Height = pdfTemplate_n0.BoundingBox.Height;
                            float tptn0X = pdfTemplate_n0.BoundingBox.Left;
                            float tptn0Y = pdfTemplate_n0.BoundingBox.Bottom;
                            Image image = Image.GetInstance(this.ImageToByteArraybyImageConverter(signatureField.Image));
                            image.SetAbsolutePosition(tptn0X, tptn0Y);
                            image.ScaleAbsoluteWidth(tptn0Width);
                            image.ScaleAbsoluteHeight(tptn0Height);
                            pdfTemplate_n0.AddImage(image);

                            PdfTemplate pdfTemplate_n2 = pdfSignatureAppaerance.GetLayer(2);

                            ColumnText columnTextTpt_n2 = new ColumnText(pdfTemplate_n2);
                            columnTextTpt_n2.SetSimpleColumn(pdfTemplate_n2.BoundingBox);
                            columnTextTpt_n2.YLine = 30;

                            if (this.showUserName)
                            {
                                string auxName = (SignerName.Length < 4) ? "SignatorDoc" : (SignerName.Length > 21) ? SignerName.Substring(0, 20) : this.SignerName;
                                Paragraph paragraphy1Tptn2 = new Paragraph("Assinado digitalmente por: " + auxName);
                                paragraphy1Tptn2.Font.Size = 6.0f;
                                paragraphy1Tptn2.Alignment = Element.ALIGN_CENTER;
                                columnTextTpt_n2.AddElement(paragraphy1Tptn2);
                            }
                            if (this.showDateTime)
                            {
                                Paragraph paragraphy2Tptn2 = new Paragraph("Hora e Data: " + dateTime.ToString("dd/MM/yyyy HH:mm:ss"));
                                paragraphy2Tptn2.Font.Size = 6.0f;
                                paragraphy2Tptn2.Alignment = Element.ALIGN_CENTER;
                                columnTextTpt_n2.AddElement(paragraphy2Tptn2);
                            }
                            if (this.showUserDigitalID)
                            {
                                Paragraph paragraphy3Tptn2 = new Paragraph(this._X509Certificate2.Thumbprint.ToUpper());
                                paragraphy3Tptn2.Font.Size = 6.0f;
                                paragraphy3Tptn2.Alignment = Element.ALIGN_CENTER;
                                columnTextTpt_n2.AddElement(paragraphy3Tptn2);
                            }

                            columnTextTpt_n2.Go();
                        }
                        else
                        {
                            pdfSignatureAppaerance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.GRAPHIC;
                            pdfSignatureAppaerance.SignatureGraphic = Image.GetInstance(this.ImageToByteArraybyImageConverter(signatureField.Image));
                        }

                        pdfSignatureAppaerance.SignDate = dateTime;
                    }

                    PdfSignature pdfSignatureDic = new PdfSignature(PdfName.ADOBE_PPKMS, PdfName.ADBE_PKCS7_SHA1);
                    pdfSignatureDic.Date = new PdfDate(pdfSignatureAppaerance.SignDate);
                    pdfSignatureDic.Name = this._X509Certificate2.Subject;

                    pdfSignatureAppaerance.CryptoDictionary = pdfSignatureDic;

                    PdfDictionary pdfDictionarySignatorDoc = new PdfDictionary();
                    pdfDictionarySignatorDoc.Put(new PdfName("Version"), (PdfObject)new PdfString(Defins.SignatorDocVersion));
                    pdfDictionarySignatorDoc.Put(new PdfName("SignerName"), (PdfObject)new PdfString(this.SignerName));

                    if (signatureField.SigFieldSealType.ToString().Contains("SigImage"))
                    {
                        pdfDictionarySignatorDoc.Put(new PdfName("BioData"), (PdfObject)new PdfString(signatureField.getDataBioSig()));
                    }
                    pdfSignatureDic.Put(new PdfName("SignatureSignatorDoc"), (PdfObject)pdfDictionarySignatorDoc);

                    pdfSignatureDic.SignatureCreator = "SignatorDoc";
                    Dictionary<iTextSharp.text.pdf.PdfName, int> exclusionSizes = new Dictionary<iTextSharp.text.pdf.PdfName, int>();
                    exclusionSizes[PdfName.CONTENTS] = (C_Size * 2 + 2);
                    pdfSignatureAppaerance.PreClose(exclusionSizes);

                    HashAlgorithm sha1HadhALgoritm = new SHA1CryptoServiceProvider();

                    Stream SigApp_RangeStream = pdfSignatureAppaerance.GetRangeStream();
                    int read = 0;
                    byte[] buff = new byte[C_Size * 2];


                    while ((read = SigApp_RangeStream.Read(buff, 0, (C_Size * 2))) > 0)
                    {
                        sha1HadhALgoritm.TransformBlock(buff, 0, read, buff, 0);
                    }
                    sha1HadhALgoritm.TransformFinalBlock(buff, 0, 0);

                    byte[] signedHash_PK = SignHashCMS(sha1HadhALgoritm.Hash, this._X509Certificate2, false);

                    if (isExceptionThrowed)
                    {
                        iTextSharp.text.pdf.PdfDictionary pdfDictionarySignatureAux = new iTextSharp.text.pdf.PdfDictionary();
                        pdfDictionarySignatureAux.Put(iTextSharp.text.pdf.PdfName.CONTENTS, new iTextSharp.text.pdf.PdfString(new byte[512]).SetHexWriting(true));
                        pdfStamper.SignatureAppearance.Close(pdfDictionarySignatureAux);
                        return false;
                        // throw new System.Exception(this.MessageException);
                    }

                    byte[] outc = new byte[C_Size];
                    Array.Copy(signedHash_PK, 0, outc, 0, signedHash_PK.Length);

                    iTextSharp.text.pdf.PdfDictionary pdfDictionarySignature = new iTextSharp.text.pdf.PdfDictionary();
                    pdfDictionarySignature.Put(iTextSharp.text.pdf.PdfName.CONTENTS, new iTextSharp.text.pdf.PdfString(outc).SetHexWriting(true));

                    pdfSignatureAppaerance.Close(pdfDictionarySignature);
                    pdfStamper.Close();
                    pdfReader.Close();
                    

                }
                File.WriteAllBytes(fileNamePath, File.ReadAllBytes(signedFilePath));
            }
            catch (iTextSharp.text.pdf.BadPasswordException bpe)
            {
                if (pdfStamper != null)
                {
                    if (!pdfStamper.SignatureAppearance.IsPreClosed())
                       this.releaseResourcesPDFStamper(ref pdfStamper, C_Size);
                    pdfStamper.Close();
                }
                if (pdfReader != null)
                    pdfReader.Close();
                AweSomeUtils.setFailLastOperation(true);
                return false;
            }
            catch (Exception exc)
            {
                if (!pdfStamper.SignatureAppearance.IsPreClosed())
                {
                    this.releaseResourcesPDFStamper(ref pdfStamper, C_Size);
                }

                pdfStamper.Close();
                pdfReader.Close();
                throw new System.Exception(exc.Message);
            }
            
            return true;
        }

        public bool SignDocumentsMassive(string outTempDirectory, List<PDFDocInfo> PDFDocInfoList)
        {
            Org.BouncyCastle.X509.X509CertificateParser x509CertificateParser = new Org.BouncyCastle.X509.X509CertificateParser();
            Org.BouncyCastle.X509.X509Certificate[] x509CertificateChain = new Org.BouncyCastle.X509.X509Certificate[] { x509CertificateParser.ReadCertificate(this._X509Certificate2.RawData) };
            int C_Size = 8192;
            string currentFileName=null;
            string tempFileNameSigned;
            string tempFileNameInProcess;
            this.ListMassiveNotSigned.Clear();
            bool isLocalFailHappened = false;

           // PdfReader pdfReader = null;
            //PdfStamper pdfStamper = null;
           // PdfSignatureAppearance pdfSignatureAppaerance = null;
            try
            {
                int countNotSigned = 0;
                foreach (PDFDocInfo pdfdocInfo in PDFDocInfoList)
                {
                    if (pdfdocInfo.fieldAlreadyToSign)
                    {
                        currentFileName = outTempDirectory + @"\" + pdfdocInfo.documentName;
                        tempFileNameSigned = outTempDirectory + @"\" + pdfdocInfo.documentName + ".temp";
                        tempFileNameInProcess = outTempDirectory + @"\" + pdfdocInfo.documentName + this.fileInProccessTempName;
                        using (FileStream outFileStream = new FileStream(tempFileNameSigned, FileMode.Create))
                        {
                            using (PdfReader pdfReader = AweSomeUtils.OpenPdfReader(File.ReadAllBytes(currentFileName), (pdfdocInfo.isNeedPassToEdit ? pdfdocInfo.passwdToEdit : null) )) // Changed Handle Encryption
                            {
                                if (!pdfReader.IsOpenedWithFullPermissions)
                                {
                                    if (!this.ListMassiveNotSigned.ContainsKey(pdfdocInfo.documentName))
                                        this.ListMassiveNotSigned.Add(pdfdocInfo.documentName, "NoPermission");
                                    else
                                        if (!this.ListMassiveNotSigned[pdfdocInfo.documentName].Contains("NoPermission"))
                                            this.ListMassiveNotSigned.Add(pdfdocInfo.documentName, "NoPermission");
                                    if (!isLocalFailHappened)
                                        isLocalFailHappened = true;

                                    continue;
                                }

                                try
                                {
                                    #region
                                    using (PdfStamper pdfStamper = PdfStamper.CreateSignature(pdfReader, outFileStream, '\0', tempFileNameInProcess, true))
                                    {
                                        PdfSignatureAppearance pdfSignatureAppaerance = pdfStamper.SignatureAppearance;

                                        if (pdfdocInfo.FieldSealType == SignatureField_SealType.SigInvisible)
                                        {
                                            pdfSignatureAppaerance.SetVisibleSignature(new Rectangle(0, 0, 0, 0), 1, "Assinatura " + (pdfStamper.AcroFields.Fields.Count + 1));
                                            pdfSignatureAppaerance.SignDate = DateTime.Now;
                                        }
                                        else
                                        {
                                            this.showUserName = false;
                                            this.showDateTime = false;
                                            this.showUserDigitalID = false;
                                            this.showTextInField = false;
                                            if ((pdfdocInfo.FieldSealType.ToString().Contains("UserName")) || (pdfdocInfo.FieldSealType.ToString().Contains("All")))
                                            {
                                                this.showUserName = true;
                                                this.showTextInField = true;
                                            }
                                            if ((pdfdocInfo.FieldSealType.ToString().Contains("DateTime")) || (pdfdocInfo.FieldSealType.ToString().Contains("All")))
                                            {
                                                this.showDateTime = true;
                                                this.showTextInField = true;
                                            }
                                            if ((pdfdocInfo.FieldSealType.ToString().Contains("Hash")) || (pdfdocInfo.FieldSealType.ToString().Contains("All")))
                                            {
                                                this.showUserDigitalID = true;
                                                this.showTextInField = true;
                                            }

                                            pdfSignatureAppaerance.SetVisibleSignature(new Rectangle(pdfdocInfo.SigStringPosition.X,
                                                                                                pdfdocInfo.SigStringPosition.Y,
                                                                                                pdfdocInfo.SigStringPosition.X + Defins.SignatureField_Size_Itext.Width,
                                                                                                pdfdocInfo.SigStringPosition.Y + Defins.SignatureField_Size_Itext.Height),
                                                                                                pdfdocInfo.SigStringPage,
                                                                                                "Assinatura " + (pdfStamper.AcroFields.Fields.Count + 1));

                                            DateTime dateTime = DateTime.Now;

                                            if (this.showTextInField)
                                            {
                                                PdfTemplate pdfTemplate_n0 = pdfSignatureAppaerance.GetLayer(0);
                                                float tptn0Width = pdfTemplate_n0.BoundingBox.Width;
                                                float tptn0Height = pdfTemplate_n0.BoundingBox.Height;
                                                float tptn0X = pdfTemplate_n0.BoundingBox.Left;
                                                float tptn0Y = pdfTemplate_n0.BoundingBox.Bottom;
                                                Image image = Image.GetInstance(this.ImageToByteArraybyImageConverter(pdfdocInfo.fieldImage));
                                                image.SetAbsolutePosition(tptn0X, tptn0Y);
                                                image.ScaleAbsoluteWidth(tptn0Width);
                                                image.ScaleAbsoluteHeight(tptn0Height);
                                                pdfTemplate_n0.AddImage(image);

                                                PdfTemplate pdfTemplate_n2 = pdfSignatureAppaerance.GetLayer(2);

                                                ColumnText columnTextTpt_n2 = new ColumnText(pdfTemplate_n2);
                                                columnTextTpt_n2.SetSimpleColumn(pdfTemplate_n2.BoundingBox);
                                                columnTextTpt_n2.YLine = 30;

                                                if (this.showUserName)
                                                {
                                                    string auxName = (SignerName.Length < 4) ? "SignatorDoc" : (SignerName.Length > 21) ? SignerName.Substring(0, 20) : SignerName;
                                                    Paragraph paragraphy1Tptn2 = new Paragraph("Assinado digitalmente por: " + auxName);
                                                    paragraphy1Tptn2.Font.Size = 6.0f;
                                                    paragraphy1Tptn2.Alignment = Element.ALIGN_CENTER;
                                                    columnTextTpt_n2.AddElement(paragraphy1Tptn2);
                                                }
                                                if (this.showDateTime)
                                                {
                                                    Paragraph paragraphy2Tptn2 = new Paragraph("Hora e Data: " + dateTime.ToString("dd/MM/yyyy HH:mm:ss"));
                                                    paragraphy2Tptn2.Font.Size = 6.0f;
                                                    paragraphy2Tptn2.Alignment = Element.ALIGN_CENTER;
                                                    columnTextTpt_n2.AddElement(paragraphy2Tptn2);
                                                }
                                                if (this.showUserDigitalID)
                                                {
                                                    Paragraph paragraphy3Tptn2 = new Paragraph(this._X509Certificate2.Thumbprint.ToUpper());
                                                    paragraphy3Tptn2.Font.Size = 6.0f;
                                                    paragraphy3Tptn2.Alignment = Element.ALIGN_CENTER;
                                                    columnTextTpt_n2.AddElement(paragraphy3Tptn2);
                                                }

                                                columnTextTpt_n2.Go();
                                            }
                                            else
                                            {
                                                pdfSignatureAppaerance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.GRAPHIC;
                                                pdfSignatureAppaerance.SignatureGraphic = Image.GetInstance(this.ImageToByteArraybyImageConverter(pdfdocInfo.fieldImage));
                                            }

                                            pdfSignatureAppaerance.SignDate = dateTime;
                                        }

                                        PdfSignature pdfSignatureDic = new PdfSignature(PdfName.ADOBE_PPKMS, PdfName.ADBE_PKCS7_SHA1);
                                        pdfSignatureDic.Date = new PdfDate(pdfSignatureAppaerance.SignDate);
                                        pdfSignatureDic.Name = this._X509Certificate2.Subject;

                                        pdfSignatureAppaerance.CryptoDictionary = pdfSignatureDic;

                                        PdfDictionary pdfDictionarySignatorDoc = new PdfDictionary();
                                        pdfDictionarySignatorDoc.Put(new PdfName("Version"), (PdfObject)new PdfString(Defins.SignatorDocVersion));
                                        pdfDictionarySignatorDoc.Put(new PdfName("SignerName"), (PdfObject)new PdfString(this.SignerName));

                                        if ((pdfdocInfo.FieldSealType == Utils.SignatureField_SealType.SigImage) || (pdfdocInfo.FieldSealType == Utils.SignatureField_SealType.SigImageAll) || (pdfdocInfo.FieldSealType == Utils.SignatureField_SealType.SigImageDateTime) || (pdfdocInfo.FieldSealType == Utils.SignatureField_SealType.SigImageDateTimeHash) || (pdfdocInfo.FieldSealType == Utils.SignatureField_SealType.SigImageHash) || (pdfdocInfo.FieldSealType == Utils.SignatureField_SealType.SigImageUserName) || (pdfdocInfo.FieldSealType == Utils.SignatureField_SealType.SigImageUserNameDateTime) || (pdfdocInfo.FieldSealType == Utils.SignatureField_SealType.SigImageUserNameHash))
                                        {
                                            if (!string.IsNullOrEmpty(pdfdocInfo.bioDataSignature))
                                                pdfDictionarySignatorDoc.Put(new PdfName("BioData"), (PdfObject)new PdfString(pdfdocInfo.bioDataSignature));
                                        }
                                        pdfSignatureDic.Put(new PdfName("SignatureSignatorDoc"), (PdfObject)pdfDictionarySignatorDoc);

                                        pdfSignatureDic.SignatureCreator = "SignatorDoc";
                                        Dictionary<iTextSharp.text.pdf.PdfName, int> exclusionSizes = new Dictionary<iTextSharp.text.pdf.PdfName, int>();
                                        exclusionSizes[PdfName.CONTENTS] = (C_Size * 2 + 2);
                                        pdfSignatureAppaerance.PreClose(exclusionSizes);

                                        HashAlgorithm sha1HadhALgoritm = new SHA1CryptoServiceProvider();

                                        Stream SigApp_RangeStream = pdfSignatureAppaerance.GetRangeStream();
                                        int read = 0;
                                        byte[] buff = new byte[C_Size * 2];


                                        while ((read = SigApp_RangeStream.Read(buff, 0, (C_Size * 2))) > 0)
                                        {
                                            sha1HadhALgoritm.TransformBlock(buff, 0, read, buff, 0);
                                        }
                                        sha1HadhALgoritm.TransformFinalBlock(buff, 0, 0);

                                        byte[] signedHash_PK = SignHashCMS(sha1HadhALgoritm.Hash, this._X509Certificate2, false);

                                        if (isExceptionThrowed)
                                        {
                                            iTextSharp.text.pdf.PdfDictionary pdfDictionarySignatureAux = new iTextSharp.text.pdf.PdfDictionary();
                                            pdfDictionarySignatureAux.Put(iTextSharp.text.pdf.PdfName.CONTENTS, new iTextSharp.text.pdf.PdfString(new byte[512]).SetHexWriting(true));
                                            pdfStamper.SignatureAppearance.Close(pdfDictionarySignatureAux);
                                            return false;
                                        }

                                        byte[] outc = new byte[C_Size];
                                        Array.Copy(signedHash_PK, 0, outc, 0, signedHash_PK.Length);

                                        iTextSharp.text.pdf.PdfDictionary pdfDictionarySignature = new iTextSharp.text.pdf.PdfDictionary();
                                        pdfDictionarySignature.Put(iTextSharp.text.pdf.PdfName.CONTENTS, new iTextSharp.text.pdf.PdfString(outc).SetHexWriting(true));

                                        pdfSignatureAppaerance.Close(pdfDictionarySignature);
                                        pdfStamper.Close();
                                        pdfReader.Close();
                                    }
                                    #endregion
                                }
                                catch(DocumentException dcexcp){
                                    if (dcexcp.Message.Equals("Append mode requires a document without errors even if recovery was possible."))
                                    {
                                        #region ATTEMPT SIGN NOT APPEND MODE
                                        using (PdfStamper pdfStamper = this.attemptCreatePDFStamperAppendMode(pdfReader, outFileStream, tempFileNameInProcess, currentFileName, pdfdocInfo))
                                        {
                                            PdfSignatureAppearance pdfSignatureAppaerance = pdfStamper.SignatureAppearance;

                                            this.showUserName = false;
                                            this.showDateTime = false;
                                            this.showUserDigitalID = false;
                                            this.showTextInField = false;
                                            if ((pdfdocInfo.FieldSealType.ToString().Contains("UserName")) || (pdfdocInfo.FieldSealType.ToString().Contains("All")))
                                            {
                                                this.showUserName = true;
                                                this.showTextInField = true;
                                            }
                                            if ((pdfdocInfo.FieldSealType.ToString().Contains("DateTime")) || (pdfdocInfo.FieldSealType.ToString().Contains("All")))
                                            {
                                                this.showDateTime = true;
                                                this.showTextInField = true;
                                            }
                                            if ((pdfdocInfo.FieldSealType.ToString().Contains("Hash")) || (pdfdocInfo.FieldSealType.ToString().Contains("All")))
                                            {
                                                this.showUserDigitalID = true;
                                                this.showTextInField = true;
                                            }

                                            pdfSignatureAppaerance.SetVisibleSignature(new Rectangle(pdfdocInfo.SigStringPosition.X,
                                                                                                pdfdocInfo.SigStringPosition.Y,
                                                                                                pdfdocInfo.SigStringPosition.X + Defins.SignatureField_Size_Itext.Width,
                                                                                                pdfdocInfo.SigStringPosition.Y + Defins.SignatureField_Size_Itext.Height),
                                                                                                pdfdocInfo.SigStringPage,
                                                                                                "Assinatura " + (pdfStamper.AcroFields.Fields.Count + 1));

                                            DateTime dateTime = DateTime.Now;

                                            if (this.showTextInField)
                                            {
                                                PdfTemplate pdfTemplate_n0 = pdfSignatureAppaerance.GetLayer(0);
                                                float tptn0Width = pdfTemplate_n0.BoundingBox.Width;
                                                float tptn0Height = pdfTemplate_n0.BoundingBox.Height;
                                                float tptn0X = pdfTemplate_n0.BoundingBox.Left;
                                                float tptn0Y = pdfTemplate_n0.BoundingBox.Bottom;
                                                Image image = Image.GetInstance(this.ImageToByteArraybyImageConverter(pdfdocInfo.fieldImage));
                                                image.SetAbsolutePosition(tptn0X, tptn0Y);
                                                image.ScaleAbsoluteWidth(tptn0Width);
                                                image.ScaleAbsoluteHeight(tptn0Height);
                                                pdfTemplate_n0.AddImage(image);

                                                PdfTemplate pdfTemplate_n2 = pdfSignatureAppaerance.GetLayer(2);

                                                ColumnText columnTextTpt_n2 = new ColumnText(pdfTemplate_n2);
                                                columnTextTpt_n2.SetSimpleColumn(pdfTemplate_n2.BoundingBox);
                                                columnTextTpt_n2.YLine = 30;

                                                if (this.showUserName)
                                                {
                                                    string auxName = (SignerName.Length < 4) ? "SignatorDoc" : (SignerName.Length > 21) ? SignerName.Substring(0, 20) : SignerName;
                                                    Paragraph paragraphy1Tptn2 = new Paragraph("Assinado digitalmente por: " + auxName);
                                                    paragraphy1Tptn2.Font.Size = 6.0f;
                                                    paragraphy1Tptn2.Alignment = Element.ALIGN_CENTER;
                                                    columnTextTpt_n2.AddElement(paragraphy1Tptn2);
                                                }
                                                if (this.showDateTime)
                                                {
                                                    Paragraph paragraphy2Tptn2 = new Paragraph("Hora e Data: " + dateTime.ToString("dd/MM/yyyy HH:mm:ss"));
                                                    paragraphy2Tptn2.Font.Size = 6.0f;
                                                    paragraphy2Tptn2.Alignment = Element.ALIGN_CENTER;
                                                    columnTextTpt_n2.AddElement(paragraphy2Tptn2);
                                                }
                                                if (this.showUserDigitalID)
                                                {
                                                    Paragraph paragraphy3Tptn2 = new Paragraph(this._X509Certificate2.Thumbprint.ToUpper());
                                                    paragraphy3Tptn2.Font.Size = 6.0f;
                                                    paragraphy3Tptn2.Alignment = Element.ALIGN_CENTER;
                                                    columnTextTpt_n2.AddElement(paragraphy3Tptn2);
                                                }

                                                columnTextTpt_n2.Go();
                                            }
                                            else
                                            {
                                                pdfSignatureAppaerance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.GRAPHIC;
                                                pdfSignatureAppaerance.SignatureGraphic = Image.GetInstance(this.ImageToByteArraybyImageConverter(pdfdocInfo.fieldImage));
                                            }

                                            pdfSignatureAppaerance.SignDate = dateTime;

                                            PdfSignature pdfSignatureDic = new PdfSignature(PdfName.ADOBE_PPKMS, PdfName.ADBE_PKCS7_SHA1);
                                            pdfSignatureDic.Date = new PdfDate(pdfSignatureAppaerance.SignDate);
                                            pdfSignatureDic.Name = this._X509Certificate2.Subject;

                                            pdfSignatureAppaerance.CryptoDictionary = pdfSignatureDic;

                                            PdfDictionary pdfDictionarySignatorDoc = new PdfDictionary();
                                            pdfDictionarySignatorDoc.Put(new PdfName("Version"), (PdfObject)new PdfString(Defins.SignatorDocVersion));
                                            pdfDictionarySignatorDoc.Put(new PdfName("SignerName"), (PdfObject)new PdfString(this.SignerName));

                                            if ((pdfdocInfo.FieldSealType == Utils.SignatureField_SealType.SigImage) || (pdfdocInfo.FieldSealType == Utils.SignatureField_SealType.SigImageAll) || (pdfdocInfo.FieldSealType == Utils.SignatureField_SealType.SigImageDateTime) || (pdfdocInfo.FieldSealType == Utils.SignatureField_SealType.SigImageDateTimeHash) || (pdfdocInfo.FieldSealType == Utils.SignatureField_SealType.SigImageHash) || (pdfdocInfo.FieldSealType == Utils.SignatureField_SealType.SigImageUserName) || (pdfdocInfo.FieldSealType == Utils.SignatureField_SealType.SigImageUserNameDateTime) || (pdfdocInfo.FieldSealType == Utils.SignatureField_SealType.SigImageUserNameHash))
                                            {
                                                if (!string.IsNullOrEmpty(pdfdocInfo.bioDataSignature))
                                                    pdfDictionarySignatorDoc.Put(new PdfName("BioData"), (PdfObject)new PdfString(pdfdocInfo.bioDataSignature));
                                            }
                                            pdfSignatureDic.Put(new PdfName("SignatureSignatorDoc"), (PdfObject)pdfDictionarySignatorDoc);

                                            pdfSignatureDic.SignatureCreator = "SignatorDoc";
                                            Dictionary<iTextSharp.text.pdf.PdfName, int> exclusionSizes = new Dictionary<iTextSharp.text.pdf.PdfName, int>();
                                            exclusionSizes[PdfName.CONTENTS] = (C_Size * 2 + 2);
                                            pdfSignatureAppaerance.PreClose(exclusionSizes);

                                            HashAlgorithm sha1HadhALgoritm = new SHA1CryptoServiceProvider();

                                            Stream SigApp_RangeStream = pdfSignatureAppaerance.GetRangeStream();
                                            int read = 0;
                                            byte[] buff = new byte[C_Size * 2];


                                            while ((read = SigApp_RangeStream.Read(buff, 0, (C_Size * 2))) > 0)
                                            {
                                                sha1HadhALgoritm.TransformBlock(buff, 0, read, buff, 0);
                                            }
                                            sha1HadhALgoritm.TransformFinalBlock(buff, 0, 0);

                                            byte[] signedHash_PK = SignHashCMS(sha1HadhALgoritm.Hash, this._X509Certificate2, false);

                                            if (isExceptionThrowed)
                                            {
                                                iTextSharp.text.pdf.PdfDictionary pdfDictionarySignatureAux = new iTextSharp.text.pdf.PdfDictionary();
                                                pdfDictionarySignatureAux.Put(iTextSharp.text.pdf.PdfName.CONTENTS, new iTextSharp.text.pdf.PdfString(new byte[512]).SetHexWriting(true));
                                                pdfStamper.SignatureAppearance.Close(pdfDictionarySignatureAux);
                                                return false;
                                            }

                                            byte[] outc = new byte[C_Size];
                                            Array.Copy(signedHash_PK, 0, outc, 0, signedHash_PK.Length);

                                            iTextSharp.text.pdf.PdfDictionary pdfDictionarySignature = new iTextSharp.text.pdf.PdfDictionary();
                                            pdfDictionarySignature.Put(iTextSharp.text.pdf.PdfName.CONTENTS, new iTextSharp.text.pdf.PdfString(outc).SetHexWriting(true));

                                            pdfSignatureAppaerance.Close(pdfDictionarySignature);
                                            pdfStamper.Close();
                                            pdfReader.Close();
                                        }
                                        #endregion
                                    }
                                    else
                                        throw new iTextSharp.text.DocumentException(dcexcp.Message);
                                }
                            }
                        }
                        File.WriteAllBytes(currentFileName, File.ReadAllBytes(tempFileNameSigned));
                    }
                    else
                        countNotSigned++;
                }
                if (countNotSigned > 0) {
                    this.MessageException = countNotSigned == 1 ? "1 campo não está assinado ou não possuí um carimbo válido!" :
                                                                  countNotSigned + " campos não estão assinados ou não possuem um carimbo válido!";
                    return false;
                    
                }
                if (isLocalFailHappened)
                    return false;
                else
                    return true;
            }
            catch(iTextSharp.text.DocumentException dexc){
                this.MessageException =dexc.Message + "\n\nDocumento: [" + Path.GetFileNameWithoutExtension(currentFileName) +"]" ;
                return false;
            }
            catch (iTextSharp.text.exceptions.InvalidPdfException ipexc)
            {
                this.MessageException = "\n\nUm ou mais parametros internos do documento [" + Path.GetFileNameWithoutExtension(currentFileName) + "] estão formatados incorretamente.\n\n" + ipexc.Message;
                return false;
            }
            catch (Exception exc)
            {
                this.MessageException =  exc.Message + "\n\nDocumento: [" + Path.GetFileNameWithoutExtension(currentFileName) + "]";
                return false;
            }
        }

        private PdfStamper attemptCreatePDFStamperAppendMode(PdfReader pdfrdr,FileStream fs,string prcsFn,string fn,PDFDocInfo pdi) {
            try
            {
                pdfrdr.Close();
                pdfrdr = null;
                pdfrdr = AweSomeUtils.OpenPdfReader(File.ReadAllBytes(fn), (pdi.isNeedPassToEdit ? pdi.passwdToEdit : null));
                prcsFn = prcsFn + 2;
                this.fileInProccessTempName = this.fileInProccessTempName.Equals("_InProcess.tmp") ? "_InProcess.temp" : "_InProcess.tmp";
                return PdfStamper.CreateSignature(pdfrdr, fs, '\0', prcsFn, false);
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        public bool getIsThrowedExcpetion()
        {
            return this.isExceptionThrowed;
        }

        public string getMessageException() {
            return this.MessageException;
        }

        public string getSignedFilePath() {
            return this.signedFilePath;
        }

        public void clearSignedPathFile() {
            this.signedFilePath = null;
        }

        private X509Certificate2 configureDefaultCertificate()
        {
            string certDefaultB64 = "MIIDBjCCAe6gAwIBAgIQFwx5n3AahplK3Xx4Nd1I+DANBgkqhkiG9w0BAQsFADAWMRQwEgYDVQQDDAtTaWduYXRvckRvYzAeFw0yNTA3MDkwMzQ3MDNaFw0yNjA3MDkwNDA3MDNaMBYxFD" +
                                    "ASBgNVBAMMC1NpZ25hdG9yRG9jMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAufBc4GVt3MUOO7oDhocnjkV7ZuhDi16Z+08mMEKjlusALuGThYJwY5Q4LBMe04232ny9S" +
                                    "vsDBnRUj5DUG4hwQ8TcuIM0NaoxqeDKJ/hIbaMxxAp6T8Trh9Y/nSpp2H54wI49TBHwBT1Z4ucDbWqcunvOyB2seYCrH/dt6aIvfEbvG84qQt7I19AQ4fI24igpTXYZYbE2GPUaJ/Bt" +
                                    "Q05rdTe3tgMNeqBz9LUVKVvbgRH+YkACK2DpqK0Ru02Cig6SZKKGRUHQpWRdLXuaWplEs6K7yLARhKbnR6Gml7zI8k1gAKGS+aF8gEqd5eSVvw3Ic1lz8M2CE6bxphCcSTvFQQIDAQA" +
                                    "Bo1AwTjAOBgNVHQ8BAf8EBAMCBaAwHQYDVR0lBBYwFAYIKwYBBQUHAwIGCCsGAQUFBwMBMB0GA1UdDgQWBBS9kD0aEm/YmO8esavms4vlWdtWWTANBgkqhkiG9w0BAQsFAAOCAQEAAI" +
                                    "LLhkWaW+6eijm3mJAAMw0fzGJr/rP+yEz0gvXYm9mfONolRw0qS3LI+nkv5HNVTaO6HzMicAfj6g2Ha2DVAFVxBB2doK6NfmIOBCf/JjJeldxVYJXn+iGCQHH/rir2bBiCH6A9oHeJg" +
                                    "ZGNzz80a/7iZeKS1P571+5rSwUdyuACXvTtltFDPHGdU8VVIkAWZMyPJHGOBnsEDLcFtCXn0K2KxmhBLUwFZZvdFYXWrvnymQcPJ8ip43zlNXgwZX9syVbXT+WG7RlbxtR9Wotq4j7b" +
                                    "qLXJnonGBs9rKfvL5jQBjOUjntEA9ErvyuwYDm45hZ7Tm1WmluMClQUhH36mzQ==";
            return new X509Certificate2(System.Convert.FromBase64String(certDefaultB64));
        }

        public void setSignerName(string signer_Name) {
            this.SignerName = signer_Name.Length < 4 ? "SignatorDoc" : signer_Name;
        }

        private byte[] ImageToByteArraybyImageConverter(System.Drawing.Image image)
        {
            System.Drawing.ImageConverter imageConverter = new System.Drawing.ImageConverter();
            byte[] imageByte = (byte[])imageConverter.ConvertTo(image, typeof(byte[]));
            return imageByte;
        }

        private  byte[] SignHashCMS(Byte[] msgHash, X509Certificate2 cert, bool detached)
        {
            try
            {
                System.Security.Cryptography.Pkcs.ContentInfo contentInfo = new System.Security.Cryptography.Pkcs.ContentInfo(msgHash);

                System.Security.Cryptography.Pkcs.SignedCms signedCms = new System.Security.Cryptography.Pkcs.SignedCms(contentInfo, detached);

                System.Security.Cryptography.Pkcs.CmsSigner cmsSigner = new System.Security.Cryptography.Pkcs.CmsSigner(cert);

                cmsSigner.IncludeOption = X509IncludeOption.ExcludeRoot;

                signedCms.ComputeSignature(cmsSigner, false);

                return signedCms.Encode();
            }
            catch(CryptographicException cexp){
                this.MessageException = "Occorreu uma exceção ao tentar selar o documento. A seguinte mensagem foi gerada : " + cexp.Message;
                if (cexp.Message.Equals("Keyset does not exist\r\n") || cexp.Message.Equals("O conjunto de chaves não está definido.\r\n"))
                {
                    if (this.countTrySign < 3)
                    {
                        if (System.Windows.Forms.MessageBox.Show("Não foi posível selar o documento. O certificado selecionado não está mais disponível. Se você estiver utilizando um Token ou Smart-Card, verifique se o dispositivo está devidamente conectado ao computador. Caso queira tentar novamente, clique em [SIM]", "Assinatura de Documento", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
                        {
                            this.countTrySign++;
                            return SignHashCMS(msgHash, cert, detached);
                        }
                    }
                }
                this.cancelOperation = true;
                System.Windows.Forms.MessageBox.Show(this.MessageException, "Assinatura de Documento", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
            catch (Exception exp)
            {
                this.MessageException = "Occorreu uma falha com o certificado ao tentar selar o documento, e a seguinte mensagem foi gerada : " + exp.Message;
                System.Windows.Forms.MessageBox.Show(this.MessageException, "Assinatura de Documento", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
               
            }
            this.isExceptionThrowed = true;  
            return null;
        }

        private void releaseResourcesPDFStamper(ref PdfStamper pdfStamper, int C_Size) 
        {           
                Dictionary<iTextSharp.text.pdf.PdfName, int> exclusionSizes = new Dictionary<iTextSharp.text.pdf.PdfName, int>();
                exclusionSizes[PdfName.CONTENTS] = (C_Size * 2 + 2);
                PdfSignature pdfSignatureDic = new PdfSignature(PdfName.ADOBE_PPKMS, PdfName.ADBE_PKCS7_SHA1);
                pdfStamper.SignatureAppearance.CryptoDictionary = pdfSignatureDic;
                pdfStamper.SignatureAppearance.PreClose(exclusionSizes);
                iTextSharp.text.pdf.PdfDictionary pdfDictionarySignature = new iTextSharp.text.pdf.PdfDictionary();
                pdfDictionarySignature.Put(iTextSharp.text.pdf.PdfName.CONTENTS, new iTextSharp.text.pdf.PdfString(new byte[512]).SetHexWriting(true));
                pdfStamper.SignatureAppearance.Close(pdfDictionarySignature);
            
        }

        public void clearExceptions() {
            this.isExceptionThrowed = false;
        }

        public void clearSignerContext() {
            this._X509Certificate2 = this.configureDefaultCertificate();
            this.SignerName = "SignatorDoc";
            this.fieldStarPosition_X = 0;
            this.fieldStarPosition_Y = 0;
            this.fieldStarPosition_Width = 0;
            this.fieldStarPosition_Heigh = 0;
            this.processTempFilePath = null;
            this.showUserName = false;
            this.showDateTime = false;
            this.showTextInField = false;
            this.isExceptionThrowed = false;
            this.MessageException = null;
            this.countTrySign = 0;
        
        }

    }
}
