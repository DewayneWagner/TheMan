using System;
using System.Collections.Generic;
using System.Text;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.Scroll
{
    class ScrollConstants
    {
        public static int Map = 0;
        public static int FGrid = 1;
        public static int Screen = 2;
        public static int ZRatio = 3;
        public static int TotDim = 4;

        public static int NW = 0;
        public static int NE = 1;
        public static int SE = 2;
        public static int SW = 3;
        public static int TotC = 4;

        public static int Z = 0;
        public static int ZZ = 1;
        public static int ZZZ = 2;
        public static int ZTotal = 3;

        public static double FocusedGridLargerThenScreenRatioX = 2;
        public static double FocusedGridLargerThenScreenRatioY = 2;
        public static int MillsecondsToDelayScroll = 5000;
        public static int NumberOfSegmentsToHaveReady = 5;
    }
}
