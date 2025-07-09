using SignatorDocSolution.Utils;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace SignatorDocSolution
{
    public partial class Form_SignatureChartCompare : Form
    {
        private bool isDocSigned = false;
        private string itextDocName = null;
        private bool areadyConfigured = false;
        private XmlDocument auxXmlDoc;
        private List<SignatureDataChart> signatureDataChartList = new List<SignatureDataChart>();
        private List<SigChartExtraInformation> sigChardExtraInformationList = new List<SigChartExtraInformation>();
        private int indexSigChart = 0;
        private bool isClosed=false;
        private bool isSignLocked=false;

        public Form_SignatureChartCompare()
        {
            InitializeComponent();
        }

        private void Form_SignatureChartCompare_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!this.isClosed)
            {
                this.isClosed = true;
                this.pbCurrSigBioChart.Image = null;
                e.Cancel = true;
                this.Hide();

                if(!this.isSignLocked)
                    this.signatureField.releaseForAnalise();
                else
                    this.isSignLocked = false;
            }
        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void lstB_Signatures_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (this.areadyConfigured)
            {
                this.indexSigChart = this.lstB_Signatures.SelectedIndex;
                this.ConfigureDoneChart(this.signatureDataChartList[this.indexSigChart]);
                this.ConfigureExtraInfo(this.sigChardExtraInformationList[this.indexSigChart]);
            }

        }

        public bool checkDocIsSigned(string docPath, bool passwdRequire, string password)
        {
            try
            {
                this.itextDocName = docPath;
                using (PdfReader pdfReader = passwdRequire ? new PdfReader(this.itextDocName, System.Text.Encoding.Default.GetBytes(password)) : new PdfReader(this.itextDocName))
                {

                    this.areadyConfigured = false;

                    if (this.auxXmlDoc != null)
                    {
                        this.auxXmlDoc.RemoveAll();
                        this.auxXmlDoc = null;
                        this.pbCurrSigBioChart.Image = null;
                    }

                    if (this.lstB_Signatures.Items.Count > 0)
                    {
                        this.signatureDataChartList.Clear();
                        this.signatureDataChartList.Clear();
                        this.lstB_Signatures.Items.Clear();
                        this.indexSigChart = 0;
                    }

                    this.isDocSigned = false;
                    if (pdfReader.AcroFields.Fields.Count > 0)
                    {
                        PdfDictionary auxPdfDic;
                        try
                        {
                            foreach (KeyValuePair<string, AcroFields.Item> kvp in pdfReader.AcroFields.Fields)
                            {
                                auxPdfDic = kvp.Value.GetValue(0).GetAsDict(PdfName.V);
                                if (auxPdfDic.Contains(new PdfName("SignatureSignatorDoc")))
                                    if (auxPdfDic.GetAsDict(new PdfName("SignatureSignatorDoc")).Contains(new PdfName("BioData")))
                                    {
                                        this.isDocSigned = true;
                                        break;
                                    }
                            }
                        }
                        catch (Exception exp)
                        {
                            MessageBox.Show("Este documento contém assinaturas fora do padrão Postgree. Algumas hierarquias são inválidas. O documento será aberto mas não poderá ser feita uma análise nas assinaturas. Erro:" + exp.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    pdfReader.Close();
                }

            }catch(iTextSharp.text.pdf.BadPasswordException bpe){
                MessageBox.Show("Este Documento está protegido com senha.\nPara habilitar o uso de senhas em documentos protegidos, acesse o menu ARQUIVO => CONFIGURAÇÕES e desmarque a opção \"Desabilitar Verificação de Senha de Proteção do Documento\".\n\n A ativação da verificação de senha pode deixar a abertura de documentos um pouco lenta.", "Abertura de Documento", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AweSomeUtils.setFailLastOperation(true);
                return false;
            }
            return this.isDocSigned;
        }

        public void showForm() {
            bool isDevicePresent = false;

            if (!this.areadyConfigured)
            {
                this.auxXmlDoc = new XmlDocument();

                using (PdfReader pdfReader = new PdfReader(this.itextDocName))
                {
                    PdfDictionary auxPdfDictionary;
                    int countSigDict = 0;
                    int auxpage;
                    int auxSigIndex=1;
                    string auxUsedCert;
                    string auxBiodata64;
                    iTextSharp.text.Rectangle auxRectancle;
                    iTextSharp.text.Rectangle pageBase = pdfReader.GetPageSize(1);
                    //float[] pageWidthPart = { (pageBase.Width / 3), ((pageBase.Width / 3) * 2), pageBase.Width };
                    //float[] pageHeightPart = { (pageBase.Height / 3), ((pageBase.Height / 3) * 2), pageBase.Height };
                    float[] pageWidthPart = { ((pageBase.Width / 2) - Defins.SignatureField_Size_Itext.Width), (pageBase.Width / 2), pageBase.Width };
                    float[] pageHeightPart = { ((pageBase.Height / 2) - (Defins.SignatureField_Size_Itext.Height * 2.25f)), ((pageBase.Height / 2) + (Defins.SignatureField_Size_Itext.Height * 1.5f)), pageBase.Height };


                    foreach (KeyValuePair<string, AcroFields.Item> kvp in pdfReader.AcroFields.Fields) {
                        auxpage=kvp.Value.GetPage(0);
                        auxUsedCert = AweSomeUtils.Parse(kvp.Value.GetValue(0).GetAsDict(PdfName.V).GetAsString(new PdfName("Name")).ToString(), "CN")[0];
                        auxPdfDictionary = kvp.Value.GetValue(0).GetAsDict(PdfName.V).GetAsDict(new PdfName("SignatureSignatorDoc"));
                        auxRectancle = pdfReader.AcroFields.GetFieldPositions(kvp.Key)[0].position;

                        if(auxPdfDictionary.Contains(new PdfName("BioData"))){
                            auxBiodata64 = auxPdfDictionary.GetAsString(new PdfName("BioData")).ToString();
                            if (auxBiodata64.Length > 10)
                            {
                                this.configureSigDataChart(auxBiodata64);
                                this.lstB_Signatures.Items.Insert(this.indexSigChart, getEasySigFieldPosition(auxRectancle.Left, auxRectancle.Bottom, pageWidthPart, pageHeightPart, auxpage, auxSigIndex));
                                this.sigChardExtraInformationList.Insert(this.indexSigChart, new SigChartExtraInformation(
                                                                                                auxPdfDictionary.GetAsString(new PdfName("SignerName")).ToString(),
                                                                                                auxUsedCert,
                                                                                                auxPdfDictionary.GetAsString(new PdfName("Version")).ToString(),
                                                                                                (this.signatureDataChartList[this.indexSigChart].getMax3() * 2).ToString(),
                                                                                                (this.signatureDataChartList[this.indexSigChart].getXLarger() / 1000.0F).ToString() + " segundos ( s )"));

                                countSigDict++;
                                auxSigIndex++;
                                this.indexSigChart++;
                            }
                        }
                    }
                    
                }

                if (this.signatureDataChartList.Count < 1) {
                    this.Hide();
                    MessageBox.Show("Este documento contém informações biométricas incompletas. Não será possível analisá-las","Análise de Assinaturas",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    return;
                }

                this.lstB_Signatures.SelectedIndex = 0;
                this.ConfigureDoneChart(this.signatureDataChartList[0]);
                this.ConfigureExtraInfo(this.sigChardExtraInformationList[0]);
                this.areadyConfigured = true;
            }

            
            if (DetectDevice.thereIsDeviceAvailable())
                    isDevicePresent = true;

            if (isDevicePresent)
            {
                this.lbl_SigFieldDisabled.Visible = false;
                this.pnlSignSigField.Enabled = true;
                this.signatureField.Visible = true;
                this.signatureField.deviceSerieNumber = DetectDevice.deviceSerieNumber;
                this.signatureField.isDesactiveScreenAnyCommands = true;
            }
            else
            {
                this.lbl_SigFieldDisabled.Visible = true;
                this.lbl_SigFieldDisabled.BringToFront();
                this.pnlSignSigField.Enabled = false;
                this.signatureField.Visible = false;
            }
            if (AweSomeUtils.isLockSignDevice)
                this.isSignLocked = true;

            this.isClosed = false;    
            this.ShowDialog();
        }

        private string getEasySigFieldPosition(float left, float bottom, float[] pageWithPart,float[] pageHeightPart, int page, int sigFieldNumber)
        {
            string textCheck;

            if (bottom < pageHeightPart[0])
            {
                textCheck = "Campo_" + sigFieldNumber + " - Inferior ";
            }
            else
                if (bottom >= pageHeightPart[0] && bottom < pageHeightPart[1])
                {
                    textCheck = "Campo_" + sigFieldNumber + " - Meio ";
                }
                else
                {
                    textCheck = "Campo_" + sigFieldNumber + " - Superior ";
                }


            if (left < pageWithPart[0])
            {
                textCheck += "Esquerdo - Página " + page;
            }
            else
                if (left >= pageWithPart[0] && left < pageWithPart[1])
                {
                    textCheck += "Central - Página " + page;
                }
                else
                {
                    textCheck += "Direito - Página " + page;
                }

            return textCheck;
        }

        private void configureSigDataChart(string b64bioData){
            this.auxXmlDoc.InnerXml = Encoding.ASCII.GetString(Convert.FromBase64String(b64bioData));
            XmlNode xmlnode = this.auxXmlDoc.SelectSingleNode("BiometricUserRegister").SelectSingleNode("BioPackage_1");

            SignatureDataChart sigDC = new SignatureDataChart();          
            sigDC.XPack = Array.ConvertAll(xmlnode.SelectSingleNode("XDataPack").InnerText.Split(';'), int.Parse);
            sigDC.YPack = Array.ConvertAll(xmlnode.SelectSingleNode("YDataPack").InnerText.Split(';'), int.Parse);
            sigDC.PPack = Array.ConvertAll(xmlnode.SelectSingleNode("PDataPack").InnerText.Split(';'), int.Parse);
            sigDC.TPack = Array.ConvertAll(xmlnode.SelectSingleNode("TDataPack").InnerText.Split(';'), int.Parse);
            sigDC.AdjustSerieDataPack();
            this.signatureDataChartList.Insert(this.indexSigChart, sigDC);

        }

        private void ConfigureDoneChart(SignatureDataChart signatureDataChart) {
            
            NPlot.Bitmap.PlotSurface2D npSurface = new NPlot.Bitmap.PlotSurface2D(this.pbDoneSignatureChartBio.Width, this.pbDoneSignatureChartBio.Height);
            NPlot.LinePlot npPlot1 = new NPlot.LinePlot();
            NPlot.LinePlot npPlot2 = new NPlot.LinePlot();
            NPlot.LinePlot npPlot3 = new NPlot.LinePlot();
            NPlot.Legend npLegend = new NPlot.Legend();
            NPlot.Grid pg = new NPlot.Grid();
            System.IO.MemoryStream memoryStreamChart = new System.IO.MemoryStream();
            int countXSeries = signatureDataChart.getXLarger();
            float factIndex = signatureDataChart.getXFactIndex();
            float[] X1 = new float[countXSeries];
            float[] Y1 = new float[countXSeries];
            float[] X2 = new float[countXSeries];
            float[] Y2 = new float[countXSeries];
            float[] X3 = new float[countXSeries];
            float[] Y3 = new float[countXSeries];

            for (int i = 0; i < countXSeries; i++)
            {
                X1[i] = i; 
                X2[i] = i; 
                X3[i] = i;
                Y1[i] = signatureDataChart.XPack[(int)Math.Truncate(i * factIndex)];             
                Y2[i] = signatureDataChart.YPack[(int)Math.Truncate(i * factIndex)];              
                Y3[i] = signatureDataChart.PPack[(int)Math.Truncate(i * factIndex)] * 2;
            }

            npSurface.Clear();
            npSurface.BackColor = Color.FromArgb(255, 253, 236);
            npSurface.PlotBackColor = Color.Black;
            npSurface.Title = "Gráfico Movimento, Pressão e Tempo";         
            pg.MajorGridPen = new Pen(Color.FromArgb(40, 40, 40), 1);
            pg.MinorGridPen = new Pen(Color.FromArgb(40, 40, 40), 1);
            npSurface.Add(pg, NPlot.PlotSurface2D.XAxisPosition.Bottom, NPlot.PlotSurface2D.YAxisPosition.Left);

            npPlot1.AbscissaData = X1;
            npPlot1.DataSource = Y1;
            npPlot2.AbscissaData = X2;
            npPlot2.DataSource = Y2;
            npPlot3.AbscissaData = X3;
            npPlot3.DataSource = Y3;

            npSurface.Add(npPlot3, NPlot.PlotSurface2D.XAxisPosition.Bottom, NPlot.PlotSurface2D.YAxisPosition.Left);
            npSurface.Add(npPlot1, NPlot.PlotSurface2D.XAxisPosition.Bottom, NPlot.PlotSurface2D.YAxisPosition.Left);
            npSurface.Add(npPlot2, NPlot.PlotSurface2D.XAxisPosition.Bottom, NPlot.PlotSurface2D.YAxisPosition.Left);

            npPlot1.Pen = new Pen(Color.Blue, 2.0F);
            npPlot1.ShowInLegend = true;
            npPlot1.ShadowColor = Color.FromArgb(80, 80, 80);
            npPlot1.ShadowOffset = new Point(1, 1);
            npPlot1.Shadow = true;
            npPlot1.SuggestXAxis();
            npPlot1.Label = "Posição Horizontal";

            npPlot2.Pen = new Pen(Color.Green, 2.0F);
            npPlot2.ShowInLegend = true;
            npPlot2.ShadowColor = Color.Gray;
            npPlot2.ShadowOffset = new Point(1, 1);
            npPlot2.Shadow = true;
            npPlot2.SuggestXAxis();
            npPlot2.Label = "Posição Vertical";

            npPlot3.Pen = new Pen(Color.DarkOrange, 2.0F);
            npPlot3.ShowInLegend = true;
            npPlot3.ShadowColor = Color.Gray;
            npPlot3.ShadowOffset = new Point(1, 1);
            npPlot3.Shadow = true;
            npPlot3.SuggestXAxis();
            npPlot3.Label = "Pressão";

            npSurface.XAxis1.WorldMax = signatureDataChart.getXLarger();
            npSurface.XAxis1.NumberFormat = "{0:#,0}";
            npSurface.YAxis1.WorldMax = signatureDataChart.getYLarger();
            npSurface.YAxis1.Label = "Movimento e Pressão";
            npSurface.XAxis1.Label = "Tempo em Milisegundos ( ms )";

            npLegend.AttachTo(NPlot.PlotSurface2D.XAxisPosition.Bottom, NPlot.PlotSurface2D.YAxisPosition.Left);
            npLegend.VerticalEdgePlacement = NPlot.Legend.Placement.Inside;
            npLegend.HorizontalEdgePlacement = NPlot.Legend.Placement.Outside;
            npLegend.YOffset = 25;
            npLegend.XOffset = 0;
            npLegend.BorderStyle = NPlot.LegendBase.BorderType.None;
            npSurface.Legend = npLegend;

            npSurface.Refresh();

            npSurface.Bitmap.Save(memoryStreamChart, System.Drawing.Imaging.ImageFormat.Png);
            this.pbDoneSignatureChartBio.Image = Image.FromStream(memoryStreamChart);
        }

        private void ConfigureCurrentSigChart(string b64bioData)
        {
            SignatureDataChart currSigDataChart = new SignatureDataChart();
            XmlDocument xmldocCurrSig= new XmlDocument();
            xmldocCurrSig.InnerXml = Encoding.ASCII.GetString(Convert.FromBase64String(b64bioData));
            XmlNode xmlnode = xmldocCurrSig.SelectSingleNode("BiometricUserRegister").SelectSingleNode("BioPackage_1");

            currSigDataChart.XPack = Array.ConvertAll(xmlnode.SelectSingleNode("XDataPack").InnerText.Split(';'), int.Parse);
            currSigDataChart.YPack = Array.ConvertAll(xmlnode.SelectSingleNode("YDataPack").InnerText.Split(';'), int.Parse);
            currSigDataChart.PPack = Array.ConvertAll(xmlnode.SelectSingleNode("PDataPack").InnerText.Split(';'), int.Parse);
            currSigDataChart.TPack = Array.ConvertAll(xmlnode.SelectSingleNode("TDataPack").InnerText.Split(';'), int.Parse);
            currSigDataChart.AdjustSerieDataPack();

            NPlot.Bitmap.PlotSurface2D npSurface = new NPlot.Bitmap.PlotSurface2D(this.pbCurrSigBioChart.Width, this.pbCurrSigBioChart.Height);
            NPlot.LinePlot npPlot1 = new NPlot.LinePlot();
            NPlot.LinePlot npPlot2 = new NPlot.LinePlot();
            NPlot.LinePlot npPlot3 = new NPlot.LinePlot();
            NPlot.Legend npLegend = new NPlot.Legend();
            NPlot.Grid pg = new NPlot.Grid();
            System.IO.MemoryStream memoryStreamChart = new System.IO.MemoryStream();
            int countXSeries = currSigDataChart.getXLarger();
            float factIndex = currSigDataChart.getXFactIndex();
            float[] X1 = new float[countXSeries];
            float[] Y1 = new float[countXSeries];
            float[] X2 = new float[countXSeries];
            float[] Y2 = new float[countXSeries];
            float[] X3 = new float[countXSeries];
            float[] Y3 = new float[countXSeries];

            for (int i = 0; i < countXSeries; i++)
            {
                X1[i] = i;
                X2[i] = i;
                X3[i] = i;
                Y1[i] = currSigDataChart.XPack[(int)Math.Truncate(i * factIndex)];
                Y2[i] = currSigDataChart.YPack[(int)Math.Truncate(i * factIndex)];
                Y3[i] = currSigDataChart.PPack[(int)Math.Truncate(i * factIndex)] * 2;
            }

            npSurface.Clear();
            npSurface.BackColor = Color.FromArgb(255, 253, 236);
            npSurface.PlotBackColor = Color.Black;
            pg.MajorGridPen = new Pen(Color.FromArgb(40, 40, 40), 1);
            pg.MinorGridPen = new Pen(Color.FromArgb(40, 40, 40), 1);
            npSurface.Add(pg, NPlot.PlotSurface2D.XAxisPosition.Bottom, NPlot.PlotSurface2D.YAxisPosition.Left);

            npPlot1.AbscissaData = X1;
            npPlot1.DataSource = Y1;
            npPlot2.AbscissaData = X2;
            npPlot2.DataSource = Y2;
            npPlot3.AbscissaData = X3;
            npPlot3.DataSource = Y3;

            npSurface.Add(npPlot3, NPlot.PlotSurface2D.XAxisPosition.Bottom, NPlot.PlotSurface2D.YAxisPosition.Left);
            npSurface.Add(npPlot1, NPlot.PlotSurface2D.XAxisPosition.Bottom, NPlot.PlotSurface2D.YAxisPosition.Left);
            npSurface.Add(npPlot2, NPlot.PlotSurface2D.XAxisPosition.Bottom, NPlot.PlotSurface2D.YAxisPosition.Left);

            npPlot1.Pen = new Pen(Color.Blue, 2.0F);

            npPlot1.ShadowColor = Color.FromArgb(80, 80, 80);
            npPlot1.ShadowOffset = new Point(1, 1);
            npPlot1.Shadow = true;
            npPlot1.SuggestXAxis();

            npPlot2.Pen = new Pen(Color.Green, 2.0F);

            npPlot2.ShadowColor = Color.Gray;
            npPlot2.ShadowOffset = new Point(1, 1);
            npPlot2.Shadow = true;
            npPlot2.SuggestXAxis();

            npPlot3.Pen = new Pen(Color.DarkOrange, 2.0F);
            npPlot3.ShadowColor = Color.Gray;
            npPlot3.ShadowOffset = new Point(1, 1);
            npPlot3.Shadow = true;
            npPlot3.SuggestXAxis();

            npSurface.XAxis1.WorldMax = currSigDataChart.getXLarger();
            npSurface.XAxis1.NumberFormat = "{0:#,0}";
            npSurface.YAxis1.WorldMax = currSigDataChart.getYLarger();

            npSurface.Refresh();

            npSurface.Bitmap.Save(memoryStreamChart, System.Drawing.Imaging.ImageFormat.Png);
            this.pbCurrSigBioChart.Image = Image.FromStream(memoryStreamChart);

        }

        private void ConfigureExtraInfo(SigChartExtraInformation sigChartExInf) {
            this.lbl_SignerName.Text = sigChartExInf.SignerName;
            this.lbl_CertUsed.Text = sigChartExInf.UsedCert;
            this.lblVersion.Text = sigChartExInf.SignatorDocVersion;
            this.lbl_Pressure.Text = sigChartExInf.MaxPressure;
            this.lbl_TotalTime.Text = sigChartExInf.TotalTime;
        }
        
        private void SignatureField_AfterSign() {
            this.ConfigureCurrentSigChart(this.signatureField.getDataBioSig());
        }

    }

    public class SigChartExtraInformation {
        public string SignerName;
        public string UsedCert;
        public string SignatorDocVersion;
        public string MaxPressure;
        public string TotalTime;

        public SigChartExtraInformation(string sigName,string uCert,string sVersion,string mPressure,string tTime) {
            this.SignerName = sigName;
            this.UsedCert = uCert;
            this.SignatorDocVersion = sVersion;
            this.MaxPressure = mPressure;
            this.TotalTime = tTime;
        }
    }

}
