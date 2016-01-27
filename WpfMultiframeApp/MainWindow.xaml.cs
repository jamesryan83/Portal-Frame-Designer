using HelixToolkit.Wpf;
using System;
using System.Windows;
using System.Collections;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace WpfMultiframeApp
{    
    public partial class MainWindow : Window
    {

        DrawShedPreview shedPreview;
        Data data;
        
        //Main
        public MainWindow()
        {
            InitializeComponent();

            MenuItemDebugOutput.IsChecked = Properties.Settings.Default.DebugMode;
            MenuItemDisplayExcel.IsChecked = Properties.Settings.Default.DisplayExcel;

            ResetForm();

            attachComboBoxEvents();
            setFormToData();
        }


        #region Attach/Detach ComboBox Events

        private void attachComboBoxEvents()
        {
            // Events are attached here so they don't fire before the form initializes
            // Also, call selectionchanged to set initial values of sections comboBoxes
            ComboBoxEndPortalColumnType.SelectionChanged += new SelectionChangedEventHandler(ComboBoxEndPortalColumnType_SelectionChanged);
            ComboBoxEndPortalColumnType_SelectionChanged(null, null);
            ComboBoxMidPortalColumnType.SelectionChanged += new SelectionChangedEventHandler(ComboBoxMidPortalColumnType_SelectionChanged);
            ComboBoxMidPortalColumnType_SelectionChanged(null, null);
            ComboBoxEndPortalRafterType.SelectionChanged += new SelectionChangedEventHandler(ComboBoxEndPortalRafterType_SelectionChanged);
            ComboBoxEndPortalRafterType_SelectionChanged(null, null);
            ComboBoxMidPortalRafterType.SelectionChanged += new SelectionChangedEventHandler(ComboBoxMidPortalRafterType_SelectionChanged);
            ComboBoxMidPortalRafterType_SelectionChanged(null, null);

            ComboBoxEavePurlinType.SelectionChanged += new SelectionChangedEventHandler(ComboBoxEavePurlinType_SelectionChanged);
            ComboBoxEavePurlinType_SelectionChanged(null, null);
            ComboBoxRoofPurlinType.SelectionChanged += new SelectionChangedEventHandler(ComboBoxRoofPurlinType_SelectionChanged);
            ComboBoxRoofPurlinType_SelectionChanged(null, null);

            ComboBoxApexTypeEnd.SelectionChanged += new SelectionChangedEventHandler(ComboBoxApexTypeEnd_SelectionChanged);
            ComboBoxApexTypeEnd_SelectionChanged(null, null);
            ComboBoxApexTypeMid.SelectionChanged += new SelectionChangedEventHandler(ComboBoxApexTypeMid_SelectionChanged);
            ComboBoxApexTypeMid_SelectionChanged(null, null);
            ComboBoxKneeTypeEnd.SelectionChanged += new SelectionChangedEventHandler(ComboBoxKneeTypeEnd_SelectionChanged);
            ComboBoxKneeTypeEnd_SelectionChanged(null, null);
            ComboBoxKneeTypeMid.SelectionChanged += new SelectionChangedEventHandler(ComboBoxKneeTypeMid_SelectionChanged);
            ComboBoxKneeTypeMid_SelectionChanged(null, null);

            ComboBoxMullionType.SelectionChanged += new SelectionChangedEventHandler(ComboBoxMullionType_SelectionChanged);
            ComboBoxMullionType_SelectionChanged(null, null);
            ComboBoxCompressionStrutType.SelectionChanged += new SelectionChangedEventHandler(ComboBoxCompressionStrutType_SelectionChanged);
            ComboBoxCompressionStrutType_SelectionChanged(null, null);
        }

        private void detachComboBoxEvents()
        {
            // Events are detached here so they don't fire when the comboBox selectedIndex values are changed in Reset method            
            ComboBoxEndPortalColumnType.SelectionChanged -= new SelectionChangedEventHandler(ComboBoxEndPortalColumnType_SelectionChanged);            
            ComboBoxMidPortalColumnType.SelectionChanged -= new SelectionChangedEventHandler(ComboBoxMidPortalColumnType_SelectionChanged);            
            ComboBoxEndPortalRafterType.SelectionChanged -= new SelectionChangedEventHandler(ComboBoxEndPortalRafterType_SelectionChanged);            
            ComboBoxMidPortalRafterType.SelectionChanged -= new SelectionChangedEventHandler(ComboBoxMidPortalRafterType_SelectionChanged);            
            ComboBoxEavePurlinType.SelectionChanged -= new SelectionChangedEventHandler(ComboBoxEavePurlinType_SelectionChanged);            
            ComboBoxRoofPurlinType.SelectionChanged -= new SelectionChangedEventHandler(ComboBoxRoofPurlinType_SelectionChanged);
            ComboBoxApexTypeEnd.SelectionChanged -= new SelectionChangedEventHandler(ComboBoxApexTypeEnd_SelectionChanged);            
            ComboBoxApexTypeMid.SelectionChanged -= new SelectionChangedEventHandler(ComboBoxApexTypeMid_SelectionChanged);            
            ComboBoxKneeTypeEnd.SelectionChanged -= new SelectionChangedEventHandler(ComboBoxKneeTypeEnd_SelectionChanged);            
            ComboBoxKneeTypeMid.SelectionChanged -= new SelectionChangedEventHandler(ComboBoxKneeTypeMid_SelectionChanged);
            ComboBoxMullionType.SelectionChanged -= new SelectionChangedEventHandler(ComboBoxMullionType_SelectionChanged);            
            ComboBoxCompressionStrutType.SelectionChanged -= new SelectionChangedEventHandler(ComboBoxCompressionStrutType_SelectionChanged);            
        }

        #endregion
        

        #region Button Actions

        // Update model view and data object.  Also, output debug info if MenuItemDebugOutput is checked
        private void ButtonUpdateDemo_Click(object sender, RoutedEventArgs e)
        {
            if (validateInputs() == true)
            {
                setFormToData();
                
                shedPreview.DrawShed(view1, data);
                //view1.Children.Add(GetGrid());
                view1.DefaultCamera = GetNewCamera(view1, data.span, data.eaveHeight, (data.baySpacing * data.numberOfBays));
                view1.ResetCamera();
            }else
                System.Windows.Forms.MessageBox.Show("Error in inputs");
        }


        // Run multiframe through Excel
        private void ButtonSendToMultiframe_Click(object sender, RoutedEventArgs e)
        {
            string filePath = Properties.Settings.Default.ExcelWorkbookFilepath;            

            if (filePath.Contains(".xls") == false)
                MessageBox.Show("Filepath to Excel workbook not set.  Check filepath in Settings menu");
            else
            {
                if (validateInputs() == true)
                {
                    setFormToData();

                    Multiframe multiframe = new Multiframe();
                    multiframe.RunMultiframe(data, ButtonSendToMultiframe);

                    // debug output
                    DebugOutput debugOutput;
                    if (Properties.Settings.Default.DebugMode == true)
                        debugOutput = new DebugOutput(data);
                }
                else
                    System.Windows.Forms.MessageBox.Show("Error in inputs");
            }
        }

        #endregion
        

        #region MenuItem Actions

        // Resets Form
        private void MenuItemNew_Click(object sender, RoutedEventArgs e)
        {
            ResetForm();
        }


        // Save As
        private void MenuItemSaveAs_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.Filter = "MFA Program files (*.mfa)|*.mfa";
            dialog.RestoreDirectory = true;            
            bool? result = dialog.ShowDialog();

            string fileName = null;

            if (result == true)
            {
                fileName = dialog.FileName;
                setFormToData();

                using (Stream stream = File.Open(fileName, FileMode.Create))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(stream, data);
                }
            }

        }

        // Load from file
        private void MenuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "MFA Program files (*.mfa)|*.mfa";
            bool? result = dialog.ShowDialog();

            string fileName = null;

            if (result == true)
            {
                fileName = dialog.FileName;

                if (File.Exists(fileName))
                {
                    using (Stream stream = File.OpenRead(fileName))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        data = (Data) bf.Deserialize(stream);
                        setDataToForm();
                    }
                }
            }
        }

        // Exit
        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit ?", "About to Exit", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                this.Close();
        }

        // Debug - output data class values to textFile
        private void MenuItemDebugOutput_Click(object sender, RoutedEventArgs e)
        {            
            Properties.Settings.Default.DebugMode = MenuItemDebugOutput.IsChecked;
            Properties.Settings.Default.Save();
        }


        // Get Excel Workbook filepath and put into Properties.Settings.Default.ExcelWorkbookFilepath
        private void MenuItemExcelWorkbookFilepath_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Excel Workbooks|*.xlsm";
            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                Properties.Settings.Default.ExcelWorkbookFilepath = dialog.FileName;
                Properties.Settings.Default.Save();
            }
        }

        // Check Excel Workbook locaion - Courtesy Function
        private void MenuItemCheckExcelWorkbookFilepath_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Properties.Settings.Default.ExcelWorkbookFilepath, "Currently Selected Excel Workbook");
        }


        // Display Excel when Multiframe is run - affects excelApp.Visible in Multiframe class
        private void MenuItemDisplayExcel_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.DisplayExcel = MenuItemDisplayExcel.IsChecked;
            Properties.Settings.Default.Save();
        }

        #endregion


        #region Data Handling

        // Form data -> Data Object
        private void setFormToData()
        {
            data.ultimateWindSpeed = Convert.ToDouble(TextBoxWindSpeedUltimate.Text);
            data.serviceWindSpeed = Convert.ToDouble(TextBoxWindSpeedService.Text);
            
            data.span = Convert.ToDouble(TextBoxShedSpan.Text);
            data.eaveHeight = Convert.ToDouble(TextBoxShedEaveHeight.Text);
            data.baySpacing = Convert.ToDouble(TextBoxShedBaySpacing.Text);
            data.roofPitch = Convert.ToDouble(TextBoxShedPitch.Text);
            data.numberOfBays = Convert.ToInt16(TextBoxShedNumberOfBays.Text);

            data.kneeBracePercentEave = Convert.ToDouble(TextBoxKneePercentEave.Text);
            data.kneeBracePercentSpan = Convert.ToDouble(TextBoxKneePercentSpan.Text);

            data.internalPressure = ComboBoxInternalPressure.SelectedItem.ToString();
            data.supports = ComboBoxSupports.SelectedItem.ToString();
            data.roofType = ComboBoxRoofType.SelectedItem.ToString();
            data.wallType = ComboBoxWallType.SelectedItem.ToString();
            data.emptyBlocked = ComboBoxEmptyBlocked.SelectedItem.ToString();
                        
            data.endColumnSection = ComboBoxEndPortalColumnSection.SelectedItem.ToString();            
            data.midColumnSection = ComboBoxMidPortalColumnSection.SelectedItem.ToString();
            data.endRafterSection = ComboBoxEndPortalRafterSection.SelectedItem.ToString();
            data.midRafterSection = ComboBoxMidPortalRafterSection.SelectedItem.ToString();

            data.endColumnType = ComboBoxEndPortalColumnType.SelectedItem.ToString();
            data.midColumnType = ComboBoxMidPortalColumnType.SelectedItem.ToString();
            data.endRafterType = ComboBoxEndPortalRafterType.SelectedItem.ToString();
            data.midRafterType = ComboBoxMidPortalRafterType.SelectedItem.ToString();

            data.eavePurlinSection = ComboBoxEavePurlinSection.SelectedItem.ToString();
            data.roofPurlinSection = ComboBoxRoofPurlinSection.SelectedItem.ToString();

            data.eavePurlinType = ComboBoxEavePurlinType.SelectedItem.ToString();
            data.roofPurlinType = ComboBoxRoofPurlinType.SelectedItem.ToString();

            data.endKneeBraceSection = (ComboBoxKneeSectionEnd.SelectedItem == null) ? "" : ComboBoxKneeSectionEnd.SelectedItem.ToString();
            data.midKneeBraceSection = (ComboBoxKneeSectionMid.SelectedItem == null) ? "" : ComboBoxKneeSectionMid.SelectedItem.ToString();
            data.endApexBraceSection = (ComboBoxApexSectionEnd.SelectedItem == null) ? "" : ComboBoxApexSectionEnd.SelectedItem.ToString();
            data.midApexBraceSection = (ComboBoxApexSectionMid.SelectedItem == null) ? "" : ComboBoxApexSectionMid.SelectedItem.ToString();

            data.endKneeBraceType = ComboBoxKneeTypeEnd.SelectedItem.ToString();
            data.midKneeBraceType = ComboBoxKneeTypeMid.SelectedItem.ToString();
            data.endApexBraceType = ComboBoxApexTypeEnd.SelectedItem.ToString();
            data.midApexBraceType = ComboBoxApexTypeMid.SelectedItem.ToString();

            data.numberOfMullions = Convert.ToInt16(ComboBoxMullions.SelectedItem.ToString());
            data.mullionSection = ComboBoxMullionSection.SelectedItem.ToString();
            data.strutSection = ComboBoxCompressionStrutSection.SelectedItem.ToString();

            data.mullionType = ComboBoxMullionType.SelectedItem.ToString();
            data.strutType = ComboBoxCompressionStrutType.SelectedItem.ToString();
        }


        // Data Object -> Form data
        public void setDataToForm()
        {
            TextBoxWindSpeedUltimate.Text = data.ultimateWindSpeed.ToString();
            TextBoxWindSpeedService.Text = data.serviceWindSpeed.ToString();

            TextBoxShedSpan.Text = data.span.ToString();
            TextBoxShedPitch.Text = data.roofPitch.ToString();
            TextBoxShedEaveHeight.Text = data.eaveHeight.ToString();
            TextBoxShedNumberOfBays.Text = data.numberOfBays.ToString();
            TextBoxShedBaySpacing.Text = data.baySpacing.ToString();
            TextBoxKneePercentEave.Text = data.kneeBracePercentEave.ToString();
            TextBoxKneePercentSpan.Text = data.kneeBracePercentSpan.ToString();

            ComboBoxInternalPressure.SelectedItem = data.internalPressure;
            ComboBoxSupports.SelectedItem = data.supports;
            ComboBoxRoofType.SelectedItem = data.roofType;
            ComboBoxWallType.SelectedItem = data.wallType;
            ComboBoxEmptyBlocked.SelectedItem = data.emptyBlocked;

            ComboBoxEndPortalColumnSection.SelectedItem = data.endColumnSection;
            ComboBoxMidPortalColumnSection.SelectedItem = data.midColumnSection;
            ComboBoxEndPortalRafterSection.SelectedItem = data.endRafterSection;
            ComboBoxMidPortalRafterSection.SelectedItem = data.midRafterSection;
            ComboBoxEndPortalColumnType.SelectedItem = data.endColumnType;
            ComboBoxMidPortalColumnType.SelectedItem = data.midColumnType;
            ComboBoxEndPortalRafterType.SelectedItem = data.endRafterType;
            ComboBoxMidPortalRafterType.SelectedItem = data.midRafterType;

            ComboBoxEavePurlinSection.SelectedItem = data.eavePurlinSection;
            ComboBoxRoofPurlinSection.SelectedItem = data.roofPurlinSection;
            ComboBoxEavePurlinType.SelectedItem = data.eavePurlinType;
            ComboBoxRoofPurlinType.SelectedItem = data.roofPurlinType;

            ComboBoxKneeSectionEnd.SelectedItem = data.endKneeBraceSection;
            ComboBoxKneeSectionMid.SelectedItem = data.midKneeBraceSection;
            ComboBoxApexSectionEnd.SelectedItem = data.endApexBraceSection;
            ComboBoxApexSectionMid.SelectedItem = data.midApexBraceSection;
            ComboBoxKneeTypeEnd.SelectedItem = data.endKneeBraceType;
            ComboBoxKneeTypeMid.SelectedItem = data.midKneeBraceType;
            ComboBoxApexTypeEnd.SelectedItem = data.endApexBraceType;
            ComboBoxApexTypeMid.SelectedItem = data.midApexBraceType;

            ComboBoxMullions.SelectedItem = data.numberOfMullions;
            ComboBoxMullionSection.SelectedItem = data.mullionSection;
            ComboBoxCompressionStrutSection.SelectedItem = data.strutSection;
            ComboBoxMullionType.SelectedItem = data.mullionType;
            ComboBoxCompressionStrutType.SelectedItem = data.strutType;
        }

        #endregion


        #region Input Validation

        private bool validateInputs()
        {
            if (Constants.ValidateDouble(TextBoxShedSpan.Text) && Constants.ValidateDouble(TextBoxShedEaveHeight.Text) && Constants.ValidateDouble(TextBoxShedBaySpacing.Text)
                && Constants.ValidateDouble(TextBoxShedPitch.Text) && Constants.ValidateInteger(TextBoxShedNumberOfBays.Text) && Constants.ValidateKneeTextBox(TextBoxKneePercentEave.Text)
                && Constants.ValidateKneeTextBox(TextBoxKneePercentSpan.Text) && Constants.ValidateDouble(TextBoxWindSpeedUltimate.Text) && Constants.ValidateDouble(TextBoxWindSpeedService.Text))
                return true;
            else
                return false;
        }

        #endregion


        #region 3D View related stuff

        public PerspectiveCamera GetNewCamera(HelixViewport3D view, double span, double eaveHeight, double length)
        {
            PerspectiveCamera cam = new PerspectiveCamera();

            double factor = 8 * ((span / 6) + (length / 6)) / 2;

            cam.Position = new Point3D(factor - (length / 2), factor + (span / 2), factor + (eaveHeight / 2));            
            cam.LookDirection = new Vector3D(-factor, -factor, -factor);
            cam.UpDirection = new Vector3D(0, 0, 1);
            cam.FieldOfView = 61;

            return cam;
        }

        // Not used - looks weird and is hard to line up properly
        public GridLinesVisual3D GetGrid()
        {
            GridLinesVisual3D grid = new GridLinesVisual3D();

            grid.Width = 50;
            grid.Length = 50;
            grid.MajorDistance = 5;
            grid.MinorDistance = 1;
            grid.Thickness = 0.01;

            return grid;
        }

        #endregion
        

        #region ComboBox SelectionChanged Handlers
        
        private void ComboBoxEndPortalColumnType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] temp = Constants.GetComboBoxSections((SectionSelection) ComboBoxEndPortalColumnType.SelectedIndex);

            ComboBoxEndPortalColumnSection.Items.Clear();

            foreach (string s in temp)
                ComboBoxEndPortalColumnSection.Items.Add(s);

            ComboBoxEndPortalColumnSection.SelectedIndex = 0;
        }

        private void ComboBoxMidPortalColumnType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] temp = Constants.GetComboBoxSections((SectionSelection)ComboBoxMidPortalColumnType.SelectedIndex);

            ComboBoxMidPortalColumnSection.Items.Clear();

            foreach (string s in temp)
                ComboBoxMidPortalColumnSection.Items.Add(s);

            ComboBoxMidPortalColumnSection.SelectedIndex = 0;
        }

        private void ComboBoxEndPortalRafterType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] temp = Constants.GetComboBoxSections((SectionSelection)ComboBoxEndPortalRafterType.SelectedIndex);

            ComboBoxEndPortalRafterSection.Items.Clear();

            foreach (string s in temp)
                ComboBoxEndPortalRafterSection.Items.Add(s);

            ComboBoxEndPortalRafterSection.SelectedIndex = 0;
        }

        private void ComboBoxMidPortalRafterType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] temp = Constants.GetComboBoxSections((SectionSelection)ComboBoxMidPortalRafterType.SelectedIndex);

            ComboBoxMidPortalRafterSection.Items.Clear();

            foreach (string s in temp)
                ComboBoxMidPortalRafterSection.Items.Add(s);

            ComboBoxMidPortalRafterSection.SelectedIndex = 0;
        }

        private void ComboBoxEavePurlinType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] temp = Constants.GetComboBoxSections((SectionSelection)ComboBoxEavePurlinType.SelectedIndex);

            ComboBoxEavePurlinSection.Items.Clear();

            foreach (string s in temp)
                ComboBoxEavePurlinSection.Items.Add(s);

            ComboBoxEavePurlinSection.SelectedIndex = 0;
        }

        private void ComboBoxRoofPurlinType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] temp = Constants.GetComboBoxSections((SectionSelection)ComboBoxRoofPurlinType.SelectedIndex);

            ComboBoxRoofPurlinSection.Items.Clear();

            foreach (string s in temp)
                ComboBoxRoofPurlinSection.Items.Add(s);

            ComboBoxRoofPurlinSection.SelectedIndex = 0;
        }

        private void ComboBoxApexTypeEnd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] temp = Constants.GetComboBoxSections((SectionSelection)ComboBoxApexTypeEnd.SelectedIndex);

            ComboBoxApexSectionEnd.Items.Clear();

            foreach (string s in temp)
                ComboBoxApexSectionEnd.Items.Add(s);

            ComboBoxApexSectionEnd.SelectedIndex = 0;
        }

        private void ComboBoxApexTypeMid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] temp = Constants.GetComboBoxSections((SectionSelection)ComboBoxApexTypeMid.SelectedIndex);

            ComboBoxApexSectionMid.Items.Clear();

            foreach (string s in temp)
                ComboBoxApexSectionMid.Items.Add(s);

            ComboBoxApexSectionMid.SelectedIndex = 0;
        }

        private void ComboBoxKneeTypeEnd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] temp = Constants.GetComboBoxSections((SectionSelection)ComboBoxKneeTypeEnd.SelectedIndex);

            ComboBoxKneeSectionEnd.Items.Clear();

            foreach (string s in temp)
                ComboBoxKneeSectionEnd.Items.Add(s);

            ComboBoxKneeSectionEnd.SelectedIndex = 0;
        }

        private void ComboBoxKneeTypeMid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] temp = Constants.GetComboBoxSections((SectionSelection)ComboBoxKneeTypeMid.SelectedIndex);

            ComboBoxKneeSectionMid.Items.Clear();

            foreach (string s in temp)
                ComboBoxKneeSectionMid.Items.Add(s);

            ComboBoxKneeSectionMid.SelectedIndex = 0;
        }

        private void ComboBoxMullionType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] temp = Constants.GetComboBoxSections((SectionSelection)ComboBoxMullionType.SelectedIndex);

            ComboBoxMullionSection.Items.Clear();

            foreach (string s in temp)
                ComboBoxMullionSection.Items.Add(s);

            ComboBoxMullionSection.SelectedIndex = 0;
        }

        private void ComboBoxCompressionStrutType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] temp = Constants.GetComboBoxSections((SectionSelection)ComboBoxCompressionStrutType.SelectedIndex);

            ComboBoxCompressionStrutSection.Items.Clear();

            foreach (string s in temp)
                ComboBoxCompressionStrutSection.Items.Add(s);

            ComboBoxCompressionStrutSection.SelectedIndex = 0;
        }

        #endregion


        public void ResetForm()
        {
            detachComboBoxEvents();

            shedPreview = new DrawShedPreview();
            data = new Data();

            TextBoxWindSpeedUltimate.Text = "40";
            TextBoxWindSpeedService.Text = "35";

            TextBoxShedSpan.Text = "6";
            TextBoxShedEaveHeight.Text = "3";
            TextBoxShedBaySpacing.Text = "3";
            TextBoxShedPitch.Text = "10";
            TextBoxShedNumberOfBays.Text = "2";

            TextBoxKneePercentEave.Text = "25";
            TextBoxKneePercentSpan.Text = "10";

            ComboBoxInternalPressure.SelectedIndex = 0;
            ComboBoxSupports.SelectedIndex = 0;
            ComboBoxRoofType.SelectedIndex = 0;
            ComboBoxWallType.SelectedIndex = 0;
            ComboBoxEmptyBlocked.SelectedIndex = 0;

            ComboBoxEndPortalColumnType.SelectedIndex = 0;
            ComboBoxEndPortalColumnSection.SelectedIndex = 0;
            ComboBoxMidPortalColumnType.SelectedIndex = 0;
            ComboBoxMidPortalColumnSection.SelectedIndex = 0;
            ComboBoxEndPortalRafterType.SelectedIndex = 0;
            ComboBoxEndPortalRafterSection.SelectedIndex = 0;
            ComboBoxMidPortalRafterType.SelectedIndex = 0;
            ComboBoxMidPortalRafterSection.SelectedIndex = 0;
            ComboBoxEavePurlinType.SelectedIndex = 0;
            ComboBoxEavePurlinSection.SelectedIndex = 0;
            ComboBoxRoofPurlinType.SelectedIndex = 0;
            ComboBoxRoofPurlinSection.SelectedIndex = 0;


            ComboBoxApexTypeEnd.SelectedIndex = 8;
            ComboBoxApexSectionEnd.SelectedIndex = 0;
            ComboBoxApexTypeMid.SelectedIndex = 8;
            ComboBoxApexSectionMid.SelectedIndex = 0;
            ComboBoxKneeTypeEnd.SelectedIndex = 8;
            ComboBoxKneeSectionEnd.SelectedIndex = 0;
            ComboBoxKneeTypeMid.SelectedIndex = 8;
            ComboBoxKneeSectionMid.SelectedIndex = 0;

            ComboBoxMullions.SelectedIndex = 0;
            ComboBoxMullionType.SelectedIndex = 0;
            ComboBoxMullionSection.SelectedIndex = 0;
            ComboBoxCompressionStrutType.SelectedIndex = 0;
            ComboBoxCompressionStrutSection.SelectedIndex = 0;

            attachComboBoxEvents();
            view1.Children.Clear();
            setFormToData();
        }
                
    }

    
}


