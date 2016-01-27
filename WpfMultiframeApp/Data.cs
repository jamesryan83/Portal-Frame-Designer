using System;


namespace WpfMultiframeApp
{
    [Serializable()]
    public class Data
    {

        #region Variables

        public double ultimateWindSpeed;
        public double serviceWindSpeed;
        public string internalPressure;

        public double span;
        public double eaveHeight;
        public double baySpacing;
        public double roofPitch;
        public int numberOfBays;
        public string supports;
        public string roofType;
        public string wallType;
        public string emptyBlocked;

        public string endColumnSection;
        public string midColumnSection;
        public string endRafterSection;
        public string midRafterSection;
        public string eavePurlinSection;
        public string roofPurlinSection;
        public string endColumnType;
        public string midColumnType;
        public string endRafterType;
        public string midRafterType;
        public string eavePurlinType;
        public string roofPurlinType;

        public string endKneeBraceSection;
        public string endApexBraceSection;
        public string midKneeBraceSection;
        public string midApexBraceSection;
        public string endKneeBraceType;
        public string endApexBraceType;
        public string midKneeBraceType;
        public string midApexBraceType;

        public double kneeBracePercentEave;
        public double kneeBracePercentSpan;

        public int numberOfMullions;
        public string mullionSection;
        public string strutSection;
        public string mullionType;
        public string strutType;
               

        // Results from Excel
        public double[] excelGeometryRatios;
        public double[] excelPressureCoefficients;
        public string[] excelLoadCases;
        public string[,] excelLoads;    
    

        #endregion


        #region ToString

        // ToString
        public override string ToString()
        {            
            string stuff =
            " ******** Data Class Variables ********\n" +
            "ultimateWindSpeed : " + ultimateWindSpeed + "\n" +
            "serviceWindSpeed : " + serviceWindSpeed + "\n" +
            "internalPressure : " + internalPressure + "\n" +

            "span : " + span + "\n" +
            "eaveHeight : " + eaveHeight + "\n" +
            "baySpacing : " + baySpacing + "\n" +
            "roofPitch : " + roofPitch + "\n" +
            "numberOfBays : " + numberOfBays + "\n" +
            "supports : " + supports + "\n" +
            "roofType : " + roofType + "\n" +
            "wallType : " + wallType + "\n" +
            "emptyBlocked : " + emptyBlocked + "\n" +

            "endColumnType : " + endColumnType + "\n" +
            "midColumnType : " + midColumnType + "\n" +
            "endRafterType : " + endRafterType + "\n" +
            "midRafterType : " + midRafterType + "\n" +
            "eavePurlinType : " + eavePurlinType + "\n" +
            "roofPurlinType : " + roofPurlinType + "\n" +

            "endColumnSection : " + endColumnSection + "\n" +
            "midColumnSection : " + midColumnSection + "\n" +
            "endRafterSection : " + endRafterSection + "\n" +
            "midRafterSection : " + midRafterSection + "\n" +
            "eavePurlinSection : " + eavePurlinSection + "\n" +
            "roofPurlinSection : " + roofPurlinSection + "\n" +

            "endKneeBraceType : " + endKneeBraceType + "\n" +
            "endApexBraceType : " + endApexBraceType + "\n" +
            "midKneeBraceType : " + midKneeBraceType + "\n" +
            "midApexBraceType : " + midApexBraceType + "\n" +

            "endKneeBraceSection : " + endKneeBraceSection + "\n" +
            "endApexBraceSection : " + endApexBraceSection + "\n" +
            "midKneeBraceSection : " + midKneeBraceSection + "\n" +
            "midApexBraceSection : " + midApexBraceSection + "\n" +


            "kneeBracePercentEave : " + kneeBracePercentEave + "\n" +
            "kneeBracePercentSpan : " + kneeBracePercentSpan + "\n" +
            "numberOfMullions : " + numberOfMullions + "\n" +

            "mullionType : " + mullionType + "\n" +
            "strutType : " + strutType + "\n" +
            "mullionSection : " + mullionSection + "\n" +
            "strutSection : " + strutSection + "\n\n";
                        
            return stuff;
        }

        #endregion


    }
}
