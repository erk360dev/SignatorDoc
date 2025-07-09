using System;
using System.Collections.Generic;
using System.Text;

namespace SignatorDocSolution
{
    class SignatureDataChart
    {
        private float XFact1, XFact2, XFact3, XFactIndex;
        private int min1, min2, min3, min4, max1, max2, max3, max4, XLarger, YLarger, SizePack;
        public int[] XPack;
        public int[] YPack;
        public int[] PPack;
        public int[] TPack;

        public SignatureDataChart()
        {
        }

        private void reduceScaleData(ref int[] dPack1, ref int[] dPack2, ref int[] dPack3, ref int[] dPack4)
        {
            min1 = min2 = min3 = YLarger = int.MaxValue;
            max1 = max2 = max3 = XLarger = 0;

            for (int i = 0; i < TPack.Length; i++)
            {
                if (dPack1[i] < min1) min1 = dPack1[i];
                if (dPack1[i] > max1) max1 = dPack1[i];

                if (dPack2[i] < min2) min2 = dPack2[i];
                if (dPack2[i] > max2) max2 = dPack2[i];

                if (dPack3[i] < min3) min3 = dPack3[i];
                if (dPack3[i] > max3) max3 = dPack3[i];

                if (dPack4[i] < min4) min4 = dPack4[i];
                if (dPack4[i] > max4) max4 = dPack4[i];
            }

            for (int i = 0; i < TPack.Length; i++)
            {
                dPack1[i] = dPack1[i] - this.min1;
                dPack2[i] = dPack2[i] - this.min2;
            }

            max1 = max1 - min1; max2 = max2 - min2; max3 = max3 - min3;
            min1 = min2 = min3 = 0;

            SizePack = this.TPack.Length;
            YLarger = max1 > max2 ? (max1 > max3 ? max1 : max3) : (max2 > max3 ? max2 : max3);
            XLarger = max4;

        }

        public void AdjustSerieDataPack()
        {

            this.reduceScaleData(ref this.XPack, ref this.YPack, ref this.PPack, ref this.TPack);

            this.XFact1 = this.YLarger / (this.max1 - this.min1);
            this.XFact2 = this.YLarger / (this.max2 - this.min2);
            this.XFact3 = this.YLarger / ( max3.Equals(min3) ? 1 : (this.max3 - this.min3));

            this.XFactIndex = (this.SizePack * 1.0F) / this.XLarger;
        }

        #region Factors

        public float getXFact()
        {
            return this.XFact1;
        }

        public float getYFact()
        {
            return this.XFact2;
        }

        public float getPFact()
        {
            return this.XFact3;
        }

        public float getXFactIndex()
        {
            return this.XFactIndex;
        }

        public int getMin1()
        {
            return this.min1;
        }

        public int getMin2()
        {
            return this.min2;
        }

        public int getMin3()
        {
            return this.min3;
        }

        public int getMin4()
        {
            return this.min4;
        }

        public int getMax1()
        {
            return this.max1;
        }

        public int getMax2()
        {
            return this.max2;
        }

        public int getMax3()
        {
            return this.max3;
        }

        public int getMax4()
        {
            return this.max4;
        }

        public int getXLarger()
        {
            return this.XLarger;
        }

        public int getYLarger()
        {
            return this.YLarger;
        }

        public int getSizePack()
        {
            return this.SizePack;
        }

        #endregion
    }
}
