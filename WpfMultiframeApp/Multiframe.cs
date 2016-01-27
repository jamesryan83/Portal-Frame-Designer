using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.ComponentModel;


namespace WpfMultiframeApp
{
    class Multiframe
    {
        Microsoft.Office.Interop.Excel.Application excelApp;
        Microsoft.Office.Interop.Excel.Workbook wb;
        Microsoft.Office.Interop.Excel.Worksheet ws;

        public bool IsRunning = false;
        private Data data;
        private string fileName;
        private System.Windows.Controls.Button mfButton;
        private ProgressWindow progressWindow;
        BackgroundWorker bw;

        // Send values to Excel
        public void RunMultiframe(Data data, System.Windows.Controls.Button mfButton)
        {
            this.mfButton = mfButton;
            this.data = data;
            progressWindow = new ProgressWindow();
            
            try
            {
                IsRunning = true;
                mfButton.IsEnabled = false;
                fileName = Properties.Settings.Default.ExcelWorkbookFilepath;

                // Run Excel/Multiframe in new thread
                bw = new BackgroundWorker();
                bw.WorkerReportsProgress = true;
                bw.DoWork += new DoWorkEventHandler(BackgroundWorker_DoWork);
                bw.ProgressChanged += new ProgressChangedEventHandler(BackgroundWorker_ProgressChanged);
                bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackgroundWorker_RunWorkerCompleted);
                bw.RunWorkerAsync(data);

                progressWindow.ShowDialog();
            }
            catch (Exception e)
            {                
                MessageBox.Show("Error while running Excel component", "Error in MFA Program");
            }
            finally
            {                
                IsRunning = false;
                mfButton.IsEnabled = true;                
            }
        }

        
        #region Background Worker

        public void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {            
            Data data = (Data) e.Argument; // Cast e argument to a local Data instance         

            // Start Excel/Multiframe calls - Either 11 or 20 stages, depending on debugMode (11 is debugMode, skips creating frame. Does debug info only)
            bool debugMode = Properties.Settings.Default.DebugMode;
            int progressTotal = debugMode == true ? 11 : 20;
            int progressCount = 0;
            bool unableToAddQ2Loads = false;


            try
            {
                bw.ReportProgress((int)(((double)progressCount / (double)progressTotal) * 100), "Opening Excel"); progressCount++;
                excelApp = new Microsoft.Office.Interop.Excel.Application();
                excelApp.Visible = Properties.Settings.Default.DisplayExcel;
                excelApp.DisplayAlerts = false;
                wb = excelApp.Workbooks.Open(fileName);
                ws = wb.Sheets.get_Item(1); // Expects "Input" sheet to be the first worksheet


                // Insert values into Excel Inputs Sheet
                bw.ReportProgress((int)(((double)progressCount / (double)progressTotal) * 100), "Inserting values into Input Sheet"); progressCount++;
                ws.Range["windSpeedVu"].Value = data.ultimateWindSpeed;
                ws.Range["windSpeedVs"].Value = data.serviceWindSpeed;
                ws.Range["internalPressure"].Value = data.internalPressure;

                ws.Range["shedType"].Value = data.roofType;
                ws.Range["shedWallType"].Value = data.wallType;

                ws.Range["shedEaveHeight"].Value = data.eaveHeight;
                ws.Range["shedRoofPitch"].Value = data.roofPitch;
                ws.Range["shedSpan"].Value = data.span;
                ws.Range["shedBaySpacing"].Value = data.baySpacing;
                ws.Range["shedNumberOfBays"].Value = data.numberOfBays;

                ws.Range["shedSupports"].Value = data.supports;

                ws.Range["shedEndColumnType"].Value = data.endColumnType;
                ws.Range["shedEndColumnSection"].Value = data.endColumnSection;
                ws.Range["shedEndRafterType"].Value = data.endRafterType;
                ws.Range["shedEndRafterSection"].Value = data.endRafterSection;
                ws.Range["shedMidColumnType"].Value = data.midColumnType;
                ws.Range["shedMidColumnSection"].Value = data.midColumnSection;
                ws.Range["shedMidRafterType"].Value = data.midRafterType;
                ws.Range["shedMidRafterSection"].Value = data.midRafterSection;

                ws.Range["shedMullions"].Value = data.numberOfMullions;
                ws.Range["shedMullionsType"].Value = data.mullionType;
                ws.Range["shedMullionsSection"].Value = data.mullionSection;
                ws.Range["shedCompressionStrutType"].Value = data.strutType;
                ws.Range["shedCompressionStrutSection"].Value = data.strutSection;

                ws.Range["shedKneeBraceEnd"].Value = data.endKneeBraceType == "None" ? "no" : "yes";
                if (data.endKneeBraceType != "None")
                {
                    ws.Range["shedKneeBraceEndType"].Value = data.endKneeBraceType;
                    ws.Range["shedKneeBraceEndSection"].Value = data.endKneeBraceSection;
                }
                ws.Range["shedKneeBraceMid"].Value = data.midKneeBraceType == "None" ? "no" : "yes";
                if (data.midKneeBraceType != "None")
                {
                    ws.Range["shedKneeBraceMidType"].Value = data.midKneeBraceType;
                    ws.Range["shedKneeBraceMidSection"].Value = data.midKneeBraceSection;
                }
                ws.Range["shedKneeBracePercentEave"].Value = data.kneeBracePercentEave;
                ws.Range["shedKneeBracePercentSpan"].Value = data.kneeBracePercentSpan;

                ws.Range["shedApexBraceEnd"].Value = data.endApexBraceType == "None" ? "no" : "yes";
                if (data.endApexBraceType != "None")
                {
                    ws.Range["shedApexBraceEndType"].Value = data.endApexBraceType;
                    ws.Range["shedApexBraceEndSection"].Value = data.endApexBraceSection;
                }
                ws.Range["shedApexBraceMid"].Value = data.midApexBraceType == "None" ? "no" : "yes";
                if (data.midApexBraceType != "None")
                {
                    ws.Range["shedApexBraceMidType"].Value = data.midApexBraceType;
                    ws.Range["shedApexBraceMidSection"].Value = data.midApexBraceSection;
                }

                ws.Range["shedEavePurlinType"].Value = data.eavePurlinType;
                ws.Range["shedEavePurlinSection"].Value = data.eavePurlinSection;
                ws.Range["shedRoofPurlinType"].Value = data.strutType;
                ws.Range["shedRoofPurlinSection"].Value = data.strutSection;


                // Start calling macros from Excel Workbook 
                bw.ReportProgress((int)(((double)progressCount / (double)progressTotal) * 100), "Setting initial excel values"); progressCount++;
                excelApp.Run("CsharpSetExcelInputValues");

                if (debugMode == false)
                {
                    bw.ReportProgress((int)(((double)progressCount / (double)progressTotal) * 100), "Setting units in Multiframe"); progressCount++;
                    excelApp.Run("CsharpSetUnits");
                    bw.ReportProgress((int)(((double)progressCount / (double)progressTotal) * 100), "Creating Frame"); progressCount++;
                    excelApp.Run("CsharpCreateFrame");
                    bw.ReportProgress((int)(((double)progressCount / (double)progressTotal) * 100), "Creating Knee Braces"); progressCount++;
                    excelApp.Run("CsharpAddKneeBraces");
                    bw.ReportProgress((int)(((double)progressCount / (double)progressTotal) * 100), "Creating Apex Braces"); progressCount++;
                    excelApp.Run("CsharpAddApexBraces");
                    bw.ReportProgress((int)(((double)progressCount / (double)progressTotal) * 100), "Creating Mullions"); progressCount++;
                    excelApp.Run("CsharpAddMullions");
                    bw.ReportProgress((int)(((double)progressCount / (double)progressTotal) * 100), "Creating Eave Purlins"); progressCount++;
                    excelApp.Run("CsharpAddEavePurlins");
                    bw.ReportProgress((int)(((double)progressCount / (double)progressTotal) * 100), "Adding Restraints and Sections"); progressCount++;
                    excelApp.Run("CsharpAddRestraintsAndSections");
                }

                bw.ReportProgress((int)(((double)progressCount / (double)progressTotal) * 100), "Calculating Wind Pressures"); progressCount++;
                excelApp.Run("CsharpGetWindPressures");
                bw.ReportProgress((int)(((double)progressCount / (double)progressTotal) * 100), "Calculating Longwind Reductions"); progressCount++;
                excelApp.Run("CsharpGetLWPressures");
                bw.ReportProgress((int)(((double)progressCount / (double)progressTotal) * 100), "Copying Pressures to Loads Sheet"); progressCount++;
                excelApp.Run("CsharpCopyPressures");

                if (debugMode == false)
                {
                    bw.ReportProgress((int)(((double)progressCount / (double)progressTotal) * 100), "Creating New Load Cases"); progressCount++;
                    excelApp.Run("CsharpCreateNewLoadCases");
                    bw.ReportProgress((int)(((double)progressCount / (double)progressTotal) * 100), "Setting up Loads"); progressCount++;
                    excelApp.Run("CsharpAddLoadsSetup");
                    bw.ReportProgress((int)(((double)progressCount / (double)progressTotal) * 100), "Adding Q2 Loads"); progressCount++;
                    unableToAddQ2Loads = (bool)excelApp.Run("CsharpAddQ2Loads");
                    bw.ReportProgress((int)(((double)progressCount / (double)progressTotal) * 100), "Adding End Portal Loads"); progressCount++;
                    excelApp.Run("CsharpAddEndPortalLoads");
                    bw.ReportProgress((int)(((double)progressCount / (double)progressTotal) * 100), "Adding Mid Portal Loads"); progressCount++;
                    excelApp.Run("CsharpAddMidPortalLoads");
                    bw.ReportProgress((int)(((double)progressCount / (double)progressTotal) * 100), "Adding End Wall Loads"); progressCount++;
                    excelApp.Run("CsharpAddEndWallLoads");
                }

                if (debugMode == true)
                {
                    bw.ReportProgress((int)(((double)progressCount / (double)progressTotal) * 100), "Getting Geometry Ratios"); progressCount++;
                    data.excelGeometryRatios = (double[]) excelApp.Run("CsharpGetGeometryRatiosDebug");
                    bw.ReportProgress((int)(((double)progressCount / (double)progressTotal) * 100), "Getting Pressure Coefficients"); progressCount++;
                    data.excelPressureCoefficients = (double[]) excelApp.Run("CsharpGetPressureCoefficientsDebug");
                    bw.ReportProgress((int)(((double)progressCount / (double)progressTotal) * 100), "Getting Load Cases"); progressCount++;
                    data.excelLoadCases = (string[])excelApp.Run("CsharpGetLoadCasesDebug");                    
                    bw.ReportProgress((int)(((double)progressCount / (double)progressTotal) * 100), "Getting Loads"); progressCount++;
                    data.excelLoads = (string[,]) excelApp.Run("CsharpGetLoadsDebug");
                }

                // Alert if Q2 loads were unable to be added
                if (unableToAddQ2Loads == true)
                    MessageBox.Show("Q2 loads were unable to be added.  You will need to add them manually", "Unable to add Q2 Loads");


            }
            catch (Exception ex)
            {
                if (ex is ArgumentException || ex is ArgumentNullException)
                    MessageBox.Show("Error closing Excel (Marshal.ReleaseFinalComObject)\n\n" + ex.Message + "\n\n" + ex.StackTrace, "Error Closing Excel");
                else
                    MessageBox.Show("Error closing Excel\n\n" + ex.Message + "\n\n" + ex.StackTrace, "Error Closing Excel");
            }
            finally
            {
                // Close Excel
                bw.ReportProgress((int)(((double)progressCount / (double)progressTotal) * 100), "Closing Excel"); progressCount++;

                GC.Collect();
                GC.WaitForPendingFinalizers();
                Marshal.FinalReleaseComObject(ws);
                wb.Close();
                Marshal.FinalReleaseComObject(wb);
                excelApp.Quit();
                Marshal.FinalReleaseComObject(excelApp);    
            }
        }

        public void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {            
            progressWindow.SetProgress((string) e.UserState, e.ProgressPercentage);
        }

        public void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {            
            mfButton.IsEnabled = false;
            progressWindow.Close();
        }

        #endregion
    }
}
