using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security;

namespace WpfMultiframeApp
{
    /// <summary>
    /// Debug class to write inputs & data to an output file
    /// </summary>
    class DebugOutput
    {
        private Data data = null;

        public DebugOutput(Data data)
        {
            this.data = data;

            DebugWriteToOutput();
        }

        private void DebugWriteToOutput()
        {
            StringBuilder sb = new StringBuilder();

            // Create what is written to the text file, put it all in a stringBuilder object
            sb.Append("Multiframe Automation Program - Debug Output" + Environment.NewLine);
            sb.Append("Debug File Created : " + DateTime.Now.ToString() + Environment.NewLine + Environment.NewLine + Environment.NewLine);

            sb.Append(" ********** C# Application - User Input Data **********" + Environment.NewLine + Environment.NewLine);

            sb.Append("ultimateWindSpeed : " + data.ultimateWindSpeed + Environment.NewLine);
            sb.Append("serviceWindSpeed : " + data.serviceWindSpeed + Environment.NewLine);
            sb.Append("internalPressure : " + data.internalPressure + Environment.NewLine);
            sb.Append("span : " + data.span + Environment.NewLine);
            sb.Append("eaveHeight : " + data.eaveHeight + Environment.NewLine);
            sb.Append("baySpacing : " + data.baySpacing + Environment.NewLine);
            sb.Append("roofPitch : " + data.roofPitch + Environment.NewLine);
            sb.Append("numberOfBays : " + data.numberOfBays + Environment.NewLine);
            sb.Append("supports : " + data.supports + Environment.NewLine);
            sb.Append("roofType : " + data.roofType + Environment.NewLine);
            sb.Append("wallType : " + data.wallType + Environment.NewLine);
            sb.Append("emptyBlocked : " + data.emptyBlocked + Environment.NewLine);
            sb.Append("endColumnSection : " + data.endColumnSection + Environment.NewLine);
            sb.Append("midColumnSection : " + data.midColumnSection + Environment.NewLine);
            sb.Append("endRafterSection : " + data.endRafterSection + Environment.NewLine);
            sb.Append("midRafterSection : " + data.midRafterSection + Environment.NewLine);
            sb.Append("eavePurlinSection : " + data.eavePurlinSection + Environment.NewLine);
            sb.Append("roofPurlinSection : " + data.roofPurlinSection + Environment.NewLine);
            sb.Append("endColumnType : " + data.endColumnType + Environment.NewLine);
            sb.Append("midColumnType : " + data.midColumnType + Environment.NewLine);
            sb.Append("endRafterType : " + data.endRafterType + Environment.NewLine);
            sb.Append("midRafterType : " + data.midRafterType + Environment.NewLine);
            sb.Append("eavePurlinType : " + data.eavePurlinType + Environment.NewLine);
            sb.Append("roofPurlinType : " + data.roofPurlinType + Environment.NewLine);
            sb.Append("endKneeBraceSection : " + data.endKneeBraceSection + Environment.NewLine);
            sb.Append("endApexBraceSection : " + data.endApexBraceSection + Environment.NewLine);
            sb.Append("midKneeBraceSection : " + data.midKneeBraceSection + Environment.NewLine);
            sb.Append("midApexBraceSection : " + data.midApexBraceSection + Environment.NewLine);
            sb.Append("endKneeBraceType : " + data.endKneeBraceType + Environment.NewLine);
            sb.Append("endApexBraceType : " + data.endApexBraceType + Environment.NewLine);
            sb.Append("midKneeBraceType : " + data.midKneeBraceType + Environment.NewLine);
            sb.Append("midApexBraceType : " + data.midApexBraceType + Environment.NewLine);
            sb.Append("kneeBracePercentEave : " + data.kneeBracePercentEave + Environment.NewLine);
            sb.Append("kneeBracePercentSpan : " + data.kneeBracePercentSpan + Environment.NewLine);
            sb.Append("numberOfMullions : " + data.numberOfMullions + Environment.NewLine);
            sb.Append("mullionSection : " + data.mullionSection + Environment.NewLine);
            sb.Append("strutSection : " + data.strutSection + Environment.NewLine);
            sb.Append("mullionType : " + data.mullionType + Environment.NewLine);
            sb.Append("strutType : " + data.strutType + Environment.NewLine + Environment.NewLine);


            sb.Append(" ********** Excel Application - Geometry Ratios **********" + Environment.NewLine);
            sb.Append("Refer to Australian Standard AS1170.2 Chapter 5 and Appendix D for guidance" + Environment.NewLine + Environment.NewLine);

            sb.Append("d/b transverse : " + data.excelGeometryRatios[0] + Environment.NewLine);
            sb.Append("h/d transverse : " + data.excelGeometryRatios[1] + Environment.NewLine);
            sb.Append("b/d transverse : " + data.excelGeometryRatios[2] + Environment.NewLine);
            sb.Append("d/b longitudinal : " + data.excelGeometryRatios[3] + Environment.NewLine);
            sb.Append("h/d longitudinal : " + data.excelGeometryRatios[4] + Environment.NewLine);
            sb.Append("cOnh : " + data.excelGeometryRatios[5] + Environment.NewLine);
            sb.Append("bOncTransverse : " + data.excelGeometryRatios[6] + Environment.NewLine);
            sb.Append("bOncLongitudinal : " + data.excelGeometryRatios[7] + Environment.NewLine + Environment.NewLine);


            sb.Append(" ********** Excel Application - Pressure Coefficients **********" + Environment.NewLine);
            sb.Append("Notes :" + Environment.NewLine);
            sb.Append("Coefficients are calculated using Australian Standard AS1170.2 Chapter 5 and Appendix D" + Environment.NewLine);
            sb.Append("Note: LW = Longwind, TW = Transverse Wind, End = end portal, Mid = mid portal" + Environment.NewLine + Environment.NewLine);

            sb.Append("Cpe End Wall LW windward : " + data.excelPressureCoefficients[0] + Environment.NewLine);
            sb.Append("Cpe End Wall TW windward : " + data.excelPressureCoefficients[1] + Environment.NewLine);
            sb.Append("Cpe End Wall LW leeward : " + data.excelPressureCoefficients[2] + Environment.NewLine);
            sb.Append("Cpe End Wall TW leeward : " + data.excelPressureCoefficients[3] + Environment.NewLine);
            sb.Append("Cpe Side Wall end : " + data.excelPressureCoefficients[4] + Environment.NewLine);
            sb.Append("Cpe Side Wall mid : " + data.excelPressureCoefficients[5] + Environment.NewLine);
            sb.Append("Cpe Roof Upwind TW Max : " + data.excelPressureCoefficients[6] + Environment.NewLine);
            sb.Append("Cpe Roof Downwind TW Max : " + data.excelPressureCoefficients[7] + Environment.NewLine);
            sb.Append("Cpe Roof Upwind TW Min : " + data.excelPressureCoefficients[8] + Environment.NewLine);
            sb.Append("Cpe Roof Downwind TW Min : " + data.excelPressureCoefficients[9] + Environment.NewLine);
            sb.Append("Cpe Roof Max LW End : " + data.excelPressureCoefficients[10] + Environment.NewLine);
            sb.Append("Cpe Roof Max LW Mid : " + data.excelPressureCoefficients[11] + Environment.NewLine);
            sb.Append("Cpe Roof Min LW End : " + data.excelPressureCoefficients[12] + Environment.NewLine);
            sb.Append("Cpe Roof Min LW Mid : " + data.excelPressureCoefficients[13] + Environment.NewLine + Environment.NewLine);

            sb.Append("Cpn End Wall LW windward : " + data.excelPressureCoefficients[14] + Environment.NewLine);
            sb.Append("Cpn End Wall TW windward : " + data.excelPressureCoefficients[15] + Environment.NewLine);
            sb.Append("Cpn End Wall LW leeward : " + data.excelPressureCoefficients[16] + Environment.NewLine);
            sb.Append("Cpn End Wall TW leeward : " + data.excelPressureCoefficients[17] + Environment.NewLine);
            sb.Append("Cpn Side Wall Max End : " + data.excelPressureCoefficients[18] + Environment.NewLine);
            sb.Append("Cpn Side Wall Max Mid : " + data.excelPressureCoefficients[19] + Environment.NewLine);
            sb.Append("Cpn Side Wall Min End : " + data.excelPressureCoefficients[20] + Environment.NewLine);
            sb.Append("Cpn Side Wall Min Mid : " + data.excelPressureCoefficients[21] + Environment.NewLine);
            sb.Append("Cpn Roof Upwind Finalised Max : " + data.excelPressureCoefficients[22] + Environment.NewLine);
            sb.Append("Cpn Roof Downwind Finalised Max : " + data.excelPressureCoefficients[23] + Environment.NewLine);
            sb.Append("Cpn Roof Upwind Finalised Min : " + data.excelPressureCoefficients[24] + Environment.NewLine);
            sb.Append("Cpn Roof Downwind Finalised Min : " + data.excelPressureCoefficients[25] + Environment.NewLine);
            sb.Append("Cpn Roof Max End : " + data.excelPressureCoefficients[26] + Environment.NewLine);
            sb.Append("Cpn Roof Max Mid : " + data.excelPressureCoefficients[27] + Environment.NewLine);
            sb.Append("Cpn Roof Min End : " + data.excelPressureCoefficients[28] + Environment.NewLine);
            sb.Append("Cpn Roof Min Mid : " + data.excelPressureCoefficients[29] + Environment.NewLine + Environment.NewLine);

            sb.Append("Cpi Positive : " + data.excelPressureCoefficients[30] + Environment.NewLine);
            sb.Append("Cpi Negative : " + data.excelPressureCoefficients[31] + Environment.NewLine + Environment.NewLine);


            sb.Append(" ********** Excel Application - Load Case Names **********" + Environment.NewLine);
            sb.Append("Notes.... " + Environment.NewLine);
            sb.Append("Both Static and Combination cases are listed.  Combination cases begin at 1.35G" + Environment.NewLine);
            sb.Append("TW = Transverse Wind, LW = Longwind, G = Dead Load, Q = Live Load" + Environment.NewLine);
            sb.Append("IP = Internal Pressure, IS = Internal Suction, Left/Right/Front/Rear = Wind Direction" + Environment.NewLine + Environment.NewLine);
            for (int i = 0; i < data.excelLoadCases.Length; i++)
                sb.Append(data.excelLoadCases[i] + Environment.NewLine);

            sb.Append(Environment.NewLine);


            sb.Append(" ********** Excel Application - Loads **********" + Environment.NewLine);
            sb.Append("Notes :" + Environment.NewLine);
            sb.Append("Loads are in kN/m and are for a 1 metre load width" + Environment.NewLine);
            sb.Append("Combination Factor Kc = 0.9 for end walls, Kc = 0.8 elsewhere.  Ka is not used" + Environment.NewLine);
            sb.Append("LC = Left Column, LR = Left Rafter, RR = Right Rafter" + Environment.NewLine);
            sb.Append("RC = Right Column, FW = Front Wall, RW = Rear Wall" + Environment.NewLine + Environment.NewLine);

            string z = String.Format("{0, -30}{1, -10}{2, -10}{3, -10}{4, -10}{5, -10}{6, -10}", "Load Case", "LC", "LR", "RR", "RC", "FW", "RW"); 
            sb.Append(z + Environment.NewLine);
            
            for (int i = 0; i <= data.excelLoads.GetUpperBound(1); i++)
            {
                for (int j = 0; j <= data.excelLoads.GetUpperBound(0); j++)
                {
                    string s;

                    if (j == 0)
                        s = String.Format("{0, -30}", data.excelLoads[j, i]);
                    else
                        s = String.Format("{0, -10}", data.excelLoads[j, i]);

                    sb.Append(s);
                }
                sb.Append(Environment.NewLine);
            }



            // Write to file
            string fileName = "MFA Program - Debug Output.txt";
            string filePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            
            filePath = filePath + @"\" + fileName;

            try
            {
                File.WriteAllText(filePath, sb.ToString());
                System.Windows.Forms.MessageBox.Show("Debug output file saved to : " + System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Debug Output Saved");
            }
            catch (Exception ex) // Catch everything imaginable
            {
                if (ex is ArgumentNullException)
                {
                    System.Windows.Forms.MessageBox.Show("Error Saving output file", "MFA Program - Error Saving Output File");
                }
                else if (ex is PathTooLongException)
                {
                    System.Windows.Forms.MessageBox.Show("FilePath too long", "MFA Program - Error Saving Output File");
                }
                else if (ex is DirectoryNotFoundException)
                {
                    System.Windows.Forms.MessageBox.Show("Directory not found", "MFA Program - Error Saving Output File");
                }
                else if (ex is IOException)
                {
                    System.Windows.Forms.MessageBox.Show("Input/Output Error", "MFA Program - Error Saving Output File");
                }
                else if (ex is UnauthorizedAccessException)
                {
                    System.Windows.Forms.MessageBox.Show("Unauthorized access error while saving file", "MFA Program - Error Saving Output File");
                }
                else if (ex is FileNotFoundException)
                {
                    System.Windows.Forms.MessageBox.Show("File Not Found", "MFA Program - Error Saving Output File");
                }
                else if (ex is NotSupportedException)
                {
                    System.Windows.Forms.MessageBox.Show("Operation not supported", "MFA Program - Error Saving Output File");
                }
                else if (ex is SecurityException)
                {
                    System.Windows.Forms.MessageBox.Show("Security Exception", "MFA Program - Error Saving Output File");
                }
            }
        }
    }
}
