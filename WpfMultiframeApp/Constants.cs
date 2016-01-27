using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace WpfMultiframeApp
{

    #region enums

    public enum SectionSelection
    {
        C_SECTION = 0,
        C_B2B_SECTION = 1,
        SHS = 2,
        RHS = 3,
        UB = 4,
        UC = 5,
        PFC = 6,
        TOPHAT = 7,
        NONE = 8
    }

    #endregion


    public static class Constants
    {
        

        // Wall Sheeting Types
        public static string[] WallSheetingTypes
        {
            get
            {
                return new string[] { "Fully Enclosed", "None", "Left Only", "Front Only", "Right Only", "Rear Only", "Left / Right Only", "Front / Rear Only", "Left / Front Only", 
                "Right / Front Only", "Left / Rear Only", "Right / Rear Only", "Left / Front / Right", "Left / Rear / Right", "Front / Rear / Left", "Front / Rear / Right" }; 
            }
        }

        // Internal Pressures
        public static string[] InternalPressures
        {
            get { return new string[] { "0 or -0.3", "+0.7 or -0.65", "None" }; }
        }

        // Support Conditions
        public static string[] Supports
        {
            get { return new string[] { "Pinned", "Fixed", "Spring" }; }
        }

        // Shed Roof Types
        public static string[] RoofTypes
        {
            get { return new string[] { "Gable", "Skillion" }; }
        }

        // Empty/blocked under
        public static string[] EmptyBlocked
        {
            get { return new string[] { "Empty Under", "Blocked Under" }; }
        }


        // Number of Mullions / Struts
        public static int[] NumberOfMullions
        {
            get { return new int[] { 0, 1, 2, 3, 4, 5 }; }
        }



        #region Sections

        // Section Types
        public static string[] SectionTypes
        {
            get { return new string[] { "CSection", "CB2BSection", "SHSsection", "RHSsection", "UBsection", "UCsection", "PFCsection", "TopHatSection" }; }
        }

        public static string[] SectionTypesWithNone
        {
            get { return new string[] { "CSection", "CB2BSection", "SHSsection", "RHSsection", "UBsection", "UCsection", "PFCsection", "TopHatSection", "None" }; }
        }
            
            
        // UB Sections
        public static string[] SectionsUB 
        {
            get
            {
                return new string[] { "150UB14", "150UB18", "180UB16.1", "180UB18.1", "180UB22.2", "200UB18.2", "200UB22.3", "200UB25.4", "200UB29.8", "250UB25.7", 
                    "250UB31.4", "250UB37.3", "310UB32.0", "310UB40.4", "310UB46.2", "360UB44.7", "360UB50.7", "360UB56.7", "410UB53.7", "410UB59.7", "460UB67.1", 
                    "460UB74.6", "460UB82.1", "530UB82.0", "530UB92.4", "610UB101", "610UB113", "610UB125" };
            }
        }

        // UC Sections
        public static string[] SectionsUC
        {
            get
            {
                return new string[] { "100UC14.8", "150UC23.4", "150UC30.0", "150UC37.2", "200UC46.2", "200UC52.2", "200UC59.5", "250UC72.9", "250UC89.5", 
                    "310UC96.8", "310UC118", "310UC137", "310UC158" };
            }
        }

        // C Sections
        public static string[] SectionsC
        {
            get
            {
                return new string[] { "C10010", "C10012", "C10015", "C10019", "C15010", "C15012", "C15015", "C15019", "C15024", "C20015", "C20019", "C20024", 
                    "C25019", "C25024", "C30024", "C30030", "C35030" };
            }
        }

        // B2B C Sections
        public static string[] SectionsB2BC
        {
            get
            {
                return new string[] { "2/C10010", "2/C10012", "2/C10015", "2/C10019", "2/C15012", "2/C15015", "2/C15019", "2/C15024", "2/C20015", "2/C20019", 
                    "2/C20024", "2/C25019", "2/C25024", "2/C30024", "2/C30030", "2/C35030" };
            }
        }
            
        // PFC sections
        public static string[] SectionsPFC
        {
            get
            {
                return new string[] { "75PFC", "100PFC", "125PFC", "150PFC", "180PFC", "200PFC", "230PFC", "250PFC", "300PFC", "380PFC" };
            }
        }

        // TopHat Section
        public static string[] SectionsTopHat
        {
            get
            {
                return new string[] { "Th64075", "Th64100", "Th64120", "Th96075", "Th96100", "Th96120" };
            }
        }

        // SHS sections
        public static string[] SectionsSHS
        {
            get
            {
                return new string[] {"150x150x6.0 SHS", "150x150x5.0 SHS", "150x150x4.0 SHS", "125x125x6.0 SHS", "125x125x5.0 SHS", "125x125x4.0 SHS", "100x100x6.0 SHS", "100x100x5.0 SHS", 
                    "100x100x4.0 SHS", "100x100x3.8 SHS", "100x100x3.3 SHS", "100x100x3.0 SHS", "100x100x2.8 SHS", "100x100x2.5 SHS", "100x100x2.3 SHS", "100x100x2.0 SHS", "90x90x3.0 SHS", 
                    "90x90x2.5 SHS", "90x90x2.0 SHS", "75x75x6.0 SHS", "75x75x5.0 SHS", "75x75x4.0 SHS", "75x75x3.5 SHS", "75x75x3.3 SHS", "75x75x3.0 SHS", "75x75x2.8 SHS", "75x75x2.5 SHS", 
                    "75x75x2.3 SHS", "75x75x2.0 SHS", "65x65x6.0 SHS", "65x65x5.0 SHS", "65x65x4.0 SHS", "65x65x3.0 SHS", "65x65x2.8 SHS", "65x65x2.5 SHS", "65x65x2.3 SHS", "65x65x2.0 SHS", 
                    "65x65x1.6 SHS", "50x50x5.0 SHS", "50x50x4.0 SHS", "50x50x3.0 SHS", "50x50x2.8 SHS", "50x50x2.5 SHS", "50x50x2.3 SHS", "50x50x2.0 SHS", "50x50x1.6 SHS", "40x40x4.0 SHS", 
                    "40x40x3.0 SHS", "40x40x2.8 SHS", "40x40x2.5 SHS", "40x40x2.3 SHS", "40x40x2.0 SHS", "40x40x1.6 SHS", "35x35x3.0 SHS", "35x35x2.8 SHS", "35x35x2.5 SHS", "35x35x2.3 SHS", 
                    "35x35x2.0 SHS", "35x35x1.6 SHS", "30x30x3.0 SHS", "30x30x2.8 SHS", "30x30x2.5 SHS", "30x30x2.3 SHS", "30x30x2.0 SHS", "30x30x1.6 SHS", "25x25x3.0 SHS", "25x25x2.5 SHS", 
                    "25x25x2.3 SHS", "25x25x2.0 SHS", "25x25x1.6 SHS", "20x20x2.0 SHS", "20x20x1.6 SHS" };
            }
        }

        // RHS sections
        public static string[] SectionsRHS
        {
            get
            {
                return new string[] {"200x100x6.0 RHS", "200x100x5.0 RHS", "200x100x4.0 RHS", "150x100x6.0 RHS", "150x100x5.0 RHS", "150x100x4.0 RHS", "150x50x6.0 RHS", "150x50x5.0 RHS", 
                    "150x50x4.0 RHS", "150x50x3.0 RHS", "150x50x2.5 RHS", "150x50x2.0 RHS", "127x51x6.0 RHS", "127x51x5.0 RHS", "127x51x3.5 RHS", "125x75x6.0 RHS", "125x75x5.0 RHS", "125x75x4.0 RHS", "125x75x3.8 RHS", 
                    "125x75x3.3 RHS", "125x75x3.0 RHS", "125x75x2.8 RHS", "125x75x2.5 RHS", "125x75x2.3 RHS", "125x75x2.0 RHS", "102x76x6.0 RHS", "102x76x5.0 RHS", "102x76x3.5 RHS", "100x50x6.0 RHS", "100x50x5.0 RHS", 
                    "100x50x4.0 RHS", "100x50x3.5 RHS", "100x50x3.3 RHS", "100x50x3.0 RHS", "100x50x2.8 RHS", "100x50x2.5 RHS", "100x50x2.3 RHS", "100x50x2.0 RHS", "100x50x1.6 RHS", "76x38x4.0 RHS", "76x38x3.0 RHS", 
                    "76x38x2.5 RHS", "76x38x2.0 RHS", "75x50x6.0 RHS", "75x50x5.0 RHS", "75x50x4.0 RHS", "75x50x3.0 RHS", "75x50x2.8 RHS", "75x50x2.5 RHS", "75x50x2.3 RHS", "75x50x2.0 RHS", "75x50x1.6 RHS", "75x25x2.5 RHS", 
                    "75x25x2.0 RHS", "75x25x1.6 RHS", "65x35x4.0 RHS", "65x35x3.0 RHS", "65x35x2.8 RHS", "65x35x2.5 RHS", "65x35x2.3 RHS", "65x35x2.0 RHS", "50x25x3.0 RHS", "50x25x2.8 RHS", "50x25x2.5 RHS", "50x25x2.3 RHS", 
                    "50x25x2.0 RHS", "50x25x1.6 RHS", "50x20x3.0 RHS", "50x20x2.8 RHS", "50x20x2.5 RHS", "50x20x2.3 RHS", "50x20x2.0 RHS", "50x20x1.6 RHS" };
            }
        }

        #endregion


        #region ComboBox selection methods

        public static string[] GetComboBoxSections(SectionSelection section)
        {
            switch (section)
            {
                case (SectionSelection.C_SECTION):
                    return SectionsC;
                case (SectionSelection.C_B2B_SECTION):
                    return SectionsB2BC;
                case (SectionSelection.PFC):
                    return SectionsPFC;
                case (SectionSelection.RHS):
                    return SectionsRHS;
                case (SectionSelection.SHS):
                    return SectionsSHS;
                case (SectionSelection.TOPHAT):
                    return SectionsTopHat;
                case (SectionSelection.UB):
                    return SectionsUB;
                case (SectionSelection.UC):
                    return SectionsUC;
                case (SectionSelection.NONE):
                    return new string[0];
                default:
                    return null;
            }
        }
        
        #endregion


        #region Validation methods

        // Validate a double value
        public static bool ValidateDouble(string value)
        {
            Regex regex = new Regex(@"^[0-9]+([.,][0-9]{1,3})?$");
            if (regex.IsMatch(value))
                return true;
            else
                return false;
        }

        // Validate an integer value
        public static bool ValidateInteger(string value)
        {
            Regex regex = new Regex(@"^[1-9]([0-9]+)?");
            if (regex.IsMatch(value))
                return true;
            else
                return false;
        }

        // Validate knee percentages between 5 and 45
        public static bool ValidateKneeTextBox(string value)
        {
            double x;

            if (Double.TryParse(value, out x))
            {
                if (x >= 5 && x <= 45)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        #endregion
    }
}
