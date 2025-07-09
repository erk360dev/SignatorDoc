using SignatorDocSolution.Utils;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using pdftron.PDF;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SignatorDocSolution
{
    class DocumentsHandle
    {
        private List<PDFDocInfo> listPdfDocInfo = null;
        private PDFDoc pdfDocument = null;
        private List<string> docFileNameList= new List<string>();
        private List<PDFDocInfo> listPDFDocPasswd = new List<PDFDocInfo>();
        private string tempPathMassive = null;
        private bool IsMassiveSearch = false;
        private int docIndex = 0;
        private int fieldIndex = 0;
        private Rect tempPageRect = null;

        public void findStringPattern(string strPattern) {
            PageIterator pageIterator = this.pdfDocument.GetPageIterator();
            TextExtractor textExtractor = null;
            Regex regEx = new Regex(strPattern);
            int pageNumber;
            string tempWord;
            if (!this.IsMassiveSearch)
            {
                this.fieldIndex = 0;
                this.listPdfDocInfo = null;
                this.listPdfDocInfo = new List<PDFDocInfo>();
            }

            while (pageIterator.HasNext())
            {
                Page page = pageIterator.Current();
                pageNumber = pageIterator.GetPageNumber();
                textExtractor = new TextExtractor();
                try { textExtractor.Begin(page); } 
                catch (Exception exc) {
                    string dname = this.pdfDocument.GetFileName();
                    textExtractor.Dispose();
                    this.pdfDocument.Close();
                    AweSomeUtils.setMsgFailLastOperation("Uma ou mais páginas do documento [" + System.IO.Path.GetFileNameWithoutExtension(dname) + "] não estão formatadas corretamente!\nA codificação é desconhecida. A pesquisa não pode continuar.\n\nO SignatorDoc será reiniciado.");
                    AweSomeUtils.closeApplication = true;
                    return;
                }
                TextExtractor.Word word;
                for (TextExtractor.Line textLine = textExtractor.GetFirstLine(); textLine.IsValid(); textLine = textLine.GetNextLine())
                {
                    for (word = textLine.GetFirstWord(); word.IsValid(); word = word.GetNextWord())
                    {
                        if (word.GetStringLen() != 0) {
                            tempWord = word.GetString();
                            if (regEx.Match(tempWord).Success) {
                                Rect rectWord = word.GetBBox();
                                System.Drawing.PointF wordPoint;
                                
                                if (this.IsMassiveSearch)
                                {
                                    wordPoint = new System.Drawing.PointF((float)rectWord.x1, (float)rectWord.y1);
                                    this.listPdfDocInfo.Add(new PDFDocInfo(this.docFileNameList[this.docIndex], this.docIndex, wordPoint, pageNumber, fieldIndex, tempPageRect));
                                }
                                else{
                                    wordPoint = new System.Drawing.PointF(((float)rectWord.x1 / 72.0F) * Defins.pagesDPI, ((((float)(page.GetPageHeight() - rectWord.y1)) / 72.0F) * Defins.pagesDPI) - Defins.SignatureFieldSize.Height);
                                    this.listPdfDocInfo.Add(new PDFDocInfo(wordPoint, pageNumber, fieldIndex));
                                }
                                fieldIndex++;                    
                            }
                        }
                    }
                }
                pageIterator.Next();
            }
        }

        public List<PDFDocInfo> getListPDFDocInfo() {
            return this.listPdfDocInfo;
        }

        public Rect getPageSize() {
            Page page=this.pdfDocument.GetPage(1);
            return new Rect(0, 0, page.GetPageWidth(), page.GetPageHeight());
        }

        public void setDocumentConfig(PDFDoc pdfdoc) {
            if(pdfdoc!=this.pdfDocument)
                this.pdfDocument = pdfdoc;
        }

        public void closeResources() {
            if(this.pdfDocument!=null)
                this.pdfDocument.Close();
        }

        public void setDocFileNameList(string tempPath, List<string> docNameList, List<PDFDocInfo> listDocPass)
        {
            this.docFileNameList.Clear();
            this.tempPathMassive = tempPath;
            this.docFileNameList= docNameList;
            this.listPDFDocPasswd = listDocPass;
        }

        public void setIsMassiveSearch(bool isMassive) {
            this.IsMassiveSearch = isMassive;
        }

        public void findStringPatternMassive(string strPattern) {
            this.docIndex = 0;
            this.fieldIndex = 0;
            this.listPdfDocInfo = null;
            this.listPdfDocInfo = new List<PDFDocInfo>();
            PDFDocInfo tempAuxPdfDocinfo = null;
            bool passIsNeed = false;
            try{
                foreach (string fileName in this.docFileNameList)
                {
                    passIsNeed = false;
                    if (this.listPDFDocPasswd.Count > 0)
                    {
                        tempAuxPdfDocinfo = this.listPDFDocPasswd.Find(doc => doc.documentName == fileName);
                        if (tempAuxPdfDocinfo != null)
                            passIsNeed = true;
                    }

                    try
                    {
                        using (this.pdfDocument = new PDFDoc(this.tempPathMassive + fileName))
                        {
                            if (passIsNeed)
                                this.pdfDocument.InitStdSecurityHandler(tempAuxPdfDocinfo.passwdToEdit);

                            Page p = this.pdfDocument.GetPage(1);
                            this.tempPageRect = new Rect(0, 0, p.GetPageWidth(), p.GetPageHeight());
                            this.findStringPattern(strPattern);
                            if(AweSomeUtils.closeApplication){
                                return;
                            }
                            this.pdfDocument.Close();
                        }
                    }
                    catch (Exception excp) {
                        if (excp.Message.Contains("Message: Attempt to load a free object\n\t Conditional expression: obj.IsFree() "))
                            System.Windows.Forms.MessageBox.Show("O documento [" + fileName + "] está corrompido e não pode ser restaurado!", "Procura de Palavra-Chave", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        else
                            throw new System.Exception( excp .Message);
                    }

                    this.docIndex++;
                }
            }
            catch (Exception pdne)
            {
                if (pdne.Message.Contains("Compressed object is corrupt"))
                {
                    AweSomeUtils.setFailLastOperation(true);
                    return;
                }
                else
                    throw new System.Exception("\n\n[Ocorreu um erro em um objeto interno responsável por procurar conteúdos no documento. Se o Erro persistir, contate o fornecedor do sistema.]\n\nErro:" + pdne.Message);
            }
        }

       
    }

    public class PDFDocInfo {
        public System.Drawing.PointF SigStringPosition;
        public int SigStringPage;
        public int SigStringIndex;
        public string documentName;
        public int documentIndex;
        public Rect pageRectangle;
        public System.Drawing.Image fieldImage = null;
        public SignatureField_SealType FieldSealType = SignatureField_SealType.Stamp;
        public string bioDataSignature = null;
        public bool fieldEnabled = false;
        public bool fieldAlreadyToSign = false;
        public string passwdToOpen = null;
        public string passwdToEdit = null;
        public bool isNeedPassToOpen = false;
        public bool isNeedPassToEdit = false;

        public PDFDocInfo(System.Drawing.PointF FieldPointLocation, int page, int fieldIndex) {
            this.SigStringPosition = FieldPointLocation;
            this.SigStringPage = page;
            this.SigStringIndex = fieldIndex;
        }

        public PDFDocInfo(string docName, int docIndex, System.Drawing.PointF FieldPointLocation, int page, int fieldIndex,Rect pageRect)
        {
            this.SigStringPosition = FieldPointLocation;
            this.SigStringPage = page;
            this.SigStringIndex = fieldIndex;
            this.documentName = docName;
            this.documentIndex = docIndex;
            this.pageRectangle = pageRect;
        }

        public PDFDocInfo(string docName,string password,bool isPassToOpen) {
            this.documentName = docName;
            if (isPassToOpen)
            {
                this.passwdToOpen = password;
                this.isNeedPassToOpen = true;
            }
            else
            {
                this.passwdToEdit = password;
                this.isNeedPassToEdit = true;
            }
        }
        public PDFDocInfo(){}
    }
}
