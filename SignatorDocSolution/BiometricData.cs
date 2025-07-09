using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace SignatorDocSolution
{
    class BiometricData
    {
        public string XData = null;
        public string YData = null;
        public string PressureData = null;
        public string TimeData = null;

        public XmlDocument xmlDoc = null;
        private XmlDeclaration xmlDeclaration = null;
        private XmlElement rootNode = null;
        private int countBioData = 0;
        
        public void AddInfo(int x,int y,uint p,uint t) {
            this.XData+=x.ToString()+";";
            this.YData += y.ToString() + ";";
            this.PressureData += p.ToString() + ";";
            this.TimeData += t.ToString() + ";";
        }

        public BiometricData()
        {
        }

        public void AddBiometricRegister() {

            xmlDoc = new XmlDocument();

            xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.InsertBefore(xmlDeclaration, xmlDoc.DocumentElement);

            rootNode = xmlDoc.CreateElement("BiometricUserRegister");
            rootNode.SetAttribute("BioDataID", "BiometricID_001");

            this.XData = this.XData.Remove(this.XData.Length - 1, 1);
            this.YData = this.YData.Remove(this.YData.Length - 1, 1);
            this.PressureData = this.PressureData.Remove(this.PressureData.Length - 1, 1);
            this.TimeData = this.TimeData.Remove(this.TimeData.Length - 1, 1);
            this.rootNode.AppendChild(this.xmlDoc.CreateElement("BioPackage_" + (countBioData + 1)));
            this.rootNode.ChildNodes[countBioData].InnerXml = "<XDataPack>" + this.XData + "</XDataPack>" +
                                                  "<YDataPack>" + this.YData + "</YDataPack>" +
                                                  "<PDataPack>" + this.PressureData + "</PDataPack>" +
                                                  "<TDataPack>" + this.TimeData + "</TDataPack>";
            this.XData = null; ;
            this.YData=null;
            this.PressureData=null;
            this.TimeData = null;
  
           // countBioData++;
        }

        public string getBioData(){
            this.AddBiometricRegister();
            this.xmlDoc.AppendChild(this.rootNode);
            string xmlBioDataBase64= Convert.ToBase64String(Encoding.ASCII.GetBytes(this.xmlDoc.InnerXml));
            
            this.xmlDoc = null;
            this.xmlDeclaration = null;
            this.rootNode = null;

            return xmlBioDataBase64;
        }



    }
}
