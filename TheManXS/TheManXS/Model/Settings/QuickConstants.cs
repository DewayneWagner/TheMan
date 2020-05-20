using System;
using TheManXS.Model.Main;

namespace TheManXS.Model.Settings
{
    public class QuickConstants
    {
        Game _game;
        private const double TopToolBarHeightRatio = 0.05;
        public QuickConstants()
        {
            _game = (Game)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.Game)];
        }

        public static int PlayerQ { get; set; }
        public static int RowQ { get; set; }
        public static int ColQ { get; set; }
        public static int SqQ { get; set; }
        public static int SqSize { get; set; }
        public static int SqLateralLength { get; set; }
        public static int PlayerIndexTheMan { get; set; }
        public static double TheManCut { get; set; }

        // Constants /////////////////////////////////////////////////////////////////
        public const int StartSQProduction = 8;
        public const double WidthOfActionPanel = 300;
        public const double StartSQOpex = 15;
        public const string NameOfOwnerOfUnOwnedSquares = "The Man";
        public const double OPEXDiscountPerSQInUnit = 0.03;
        public const double CAPEXDiscountPerSQInUnit = 0.03;

        public static double TopToolBarHeight { get; set; }
        public static int MaxResourceSQsOnMap { get; set; }
        public static int MaxResourceSQsInPool { get; set; }
        public static int PlayerIndexActual => 0;
        public static int UnitCounter { get; set; }
        public static bool IsNewGame { get; set; }
        public static float PinchMaxScale => 10f;

        // set when game begins ////////////////////////////////////////////////
        public static int CurrentSavedGameSlot { get; set; }

        // these are initialized when the Main Menu is initialized
        public static int ScreenWidth { get; set; }
        public static int ScreenHeight { get; set; }
        public static double Rotation { get; set; }

        // set by Gameboard Code-behind
        public static double MapCanvasViewHeight { get; set; }
        public static double MapCanvasViewWidth { get; set; }

        public static void InitProperties(int playerQ, int rowQ, int colQ, int sqSize, double theManCut, int maxResourceSqsInPool, int maxResourceSQsOnMap)
        {
            PlayerQ = playerQ;
            RowQ = rowQ;
            ColQ = colQ;
            SqQ = RowQ * ColQ;
            SqSize = sqSize;
            SqLateralLength = (int)((double)SqSize * Math.Sqrt(2));
            PlayerIndexTheMan = playerQ;
            TheManCut = theManCut;
            MaxResourceSQsInPool = maxResourceSqsInPool;
            MaxResourceSQsOnMap = maxResourceSQsOnMap;
            UnitCounter = 0;
        }
    }
}
