using pdftron.PDF;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SignatorDocSolution
{
    /// <summary>
    /// CLASS_ID=06;
    /// </summary>
    class PanelThumbnail : Panel
    {
        private List<PictureBox> ListThumbNails = new List<PictureBox>();
        private int thumbnailList_Index = 0;
        private int currentThumbnailPage=0;
        private int DPI = 96;
        System.Drawing.Size baseSize = new Size(100, 120);
        private EventHandler Event_Handle_1=null;

        public PanelThumbnail() {
            this.AutoScroll = true;
        }

        public void setTbNailPages(int countPage) {
            int PointY=10;
            for (int i = 0; i < countPage; i++) {
                PictureBox pic = new PictureBox();
                pic = new PictureBox();
                pic.Size = baseSize;
                pic.Location = new System.Drawing.Point(10, PointY);
                PointY += baseSize.Height + 5;
                pic.Name = "Thumbnail_" + i;
                pic.Cursor = Cursors.Hand;
                pic.MouseEnter += (sender, e) =>
                {
                        string[] arrStr = ((PictureBox)sender).Name.Split('_');
                        int.TryParse(arrStr[1], out this.currentThumbnailPage);

                };
                pic.Click+=Event_Handle_1;
                this.ListThumbNails.Add(pic);
                this.Controls.Add(ListThumbNails[thumbnailList_Index]);
                thumbnailList_Index++;
            }
            thumbnailList_Index = 0;
        }

        public void ConfigureThumbNails(PDFDoc doc) {
            PDFDraw pdfDraw = new PDFDraw();
            pdfDraw.SetDPI(DPI);
            PageIterator itr = doc.GetPageIterator();
            int countPages = 1;
            float factorX = 0.35f;
            while (itr.HasNext())
            {
                Rect r = new Rect(0, 0, ((baseSize.Width / DPI) * 72), ((baseSize.Height / DPI) * 72));
                double wd = r.Width();
                Bitmap bitMap = new Bitmap(baseSize.Width, baseSize.Height);
                Graphics g = Graphics.FromImage(bitMap);

                pdfDraw.DrawInRect(itr.Current(), g, r);

                this.ListThumbNails[this.thumbnailList_Index].BackgroundImage = bitMap;

                Bitmap sigImageBitmap = new Bitmap((int)(this.baseSize.Width), (int)(this.baseSize.Height));
                using (Graphics sigGraphic = Graphics.FromImage(sigImageBitmap))
                {
                    sigGraphic.DrawString(countPages.ToString(), new System.Drawing.Font("Calibri", 22, FontStyle.Underline), Brushes.Green, (baseSize.Width * factorX), (this.baseSize.Height * 0.20f));
                    sigGraphic.Save();
                }

                countPages++;

                factorX = countPages <= 9 ? factorX : countPages > 9 && countPages < 100 ? 0.27f : 0.20f;  

                this.ListThumbNails[this.thumbnailList_Index].Image = sigImageBitmap;

                thumbnailList_Index++;
                itr.Next();
            }
        }

        public void ConfigureThumbNails(PDFDoc doc, int limitFrom, int limitTo)
        {
            PDFDraw pdfDraw = new PDFDraw();
            pdfDraw.SetDPI(DPI);
            int countPages = limitFrom;
            this.thumbnailList_Index = limitFrom - 1;
            float factorX = 0.35f;
            for(int i= limitFrom; i<=limitTo;i++)
            {

                Rect r = new Rect(0, 0, ((baseSize.Width / DPI) * 72), ((baseSize.Height / DPI) * 72));
                double wd = r.Width();
                Bitmap bitMap = new Bitmap(baseSize.Width, baseSize.Height);
                Graphics g = Graphics.FromImage(bitMap);

                try
                {
                    pdfDraw.DrawInRect(doc.GetPage(i), g, r);
                    this.ListThumbNails[this.thumbnailList_Index].BackgroundImage = bitMap;
                }
                catch (Exception exc) {
                  //  SignatorDocSolution.Utils.RegisterWinEventLogViewer.RegisterLog("The Source PanelThumbnail_ConfigureThumbNails throws the following message : ", exc.Message, 60,2);//classid+error 0(objectnull or disposed)
                    return;
                }

                try
                {
                    Bitmap sigImageBitmap = new Bitmap((int)(this.baseSize.Width), (int)(this.baseSize.Height));
                    using (Graphics sigGraphic = Graphics.FromImage(sigImageBitmap))
                    {
                        sigGraphic.DrawString(countPages.ToString(), new System.Drawing.Font("Calibri", 22, FontStyle.Underline), Brushes.Green, (baseSize.Width * factorX), (this.baseSize.Height * 0.20f));
                        sigGraphic.Save();
                    }

                    countPages++;

                    factorX = countPages <= 9 ? factorX : countPages > 9 && countPages < 100 ? 0.27f : 0.20f;

                    this.ListThumbNails[this.thumbnailList_Index].Image = sigImageBitmap;

                    thumbnailList_Index++;
                }
                catch (Exception exc) { 
                    Environment.Exit(0);
                }
            }
            pdfDraw.Dispose();
            thumbnailList_Index = 0;
        }

        public void ConfigureThumbNailsGhostScript(int limitFrom, int limitTo)
        {
            int countPages = limitFrom;
            this.thumbnailList_Index = limitFrom - 1;
            float factorX = 0.35f;
            for (int i = limitFrom; i <= limitTo; i++)
            {

                Bitmap bitMap = new Bitmap(baseSize.Width, baseSize.Height);
                Graphics g = Graphics.FromImage(bitMap);

                try
                {

                    g.FillRectangle(Brushes.White, new Rectangle(0, 0, baseSize.Width, baseSize.Height));
                    g.DrawRectangle(Pens.Black, new Rectangle(0, 0, baseSize.Width - 1, baseSize.Height - 1));
                    this.ListThumbNails[this.thumbnailList_Index].BackgroundImage = bitMap;
                }
                catch (Exception exc)
                {
                    //  SignatorDocSolution.Utils.RegisterWinEventLogViewer.RegisterLog("The Source PanelThumbnail_ConfigureThumbNails throws the following message : ", exc.Message, 60,2);//classid+error 0(objectnull or disposed)
                    return;
                }

                Bitmap sigImageBitmap = new Bitmap((int)(this.baseSize.Width), (int)(this.baseSize.Height));
                using (Graphics sigGraphic = Graphics.FromImage(sigImageBitmap))
                {
                    sigGraphic.DrawString(countPages.ToString(), new System.Drawing.Font("Calibri", 22, FontStyle.Underline), Brushes.Green, (baseSize.Width * factorX), (this.baseSize.Height * 0.20f));
                    sigGraphic.Save();
                }

                countPages++;

                factorX = countPages <= 9 ? factorX : countPages > 9 && countPages < 100 ? 0.27f : 0.20f;

                this.ListThumbNails[this.thumbnailList_Index].Image = sigImageBitmap;

                thumbnailList_Index++;
            }
            thumbnailList_Index = 0;
        }

        public void setEventHandler(EventHandler eventHandler1)
        {
            this.Event_Handle_1 = eventHandler1;
        }

        public int getCurrentThumbnailPage() {
            return this.currentThumbnailPage;
        }

        public void releaseThumbnail() {
            try
            {
                for (int i = 0; i < this.ListThumbNails.Count; i++)
                {
                    this.ListThumbNails.Remove(this.ListThumbNails[this.thumbnailList_Index]);
                }
                this.ListThumbNails.Clear();
            }catch(Exception exc){}
        }

    }

}
