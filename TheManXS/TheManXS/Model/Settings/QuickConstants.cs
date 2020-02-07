using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Settings;
using TheManXS.Model.Services.EntityFrameWork;
using AS = TheManXS.Model.Settings.SettingsMaster.AS;

namespace TheManXS.Model.Settings
{
    public class QuickConstants
    {
        public const int playerQ = 5;
        public QuickConstants()
        {
            PlayerQ = (int)Setting.GetConstant(AS.PlayerConstants, (int)SettingsMaster.PlayerConstants.PlayerQ);
            RowQ = (int)Setting.GetConstant(AS.MapConstants, (int)SettingsMaster.TerrainConstructConstants.RowQ);
            ColQ = (int)Setting.GetConstant(AS.MapConstants, (int)SettingsMaster.TerrainConstructConstants.ColQ);
            SqQ = RowQ * ColQ;
            SqSize = (int)Setting.GetConstant(AS.MapConstants, (int)SettingsMaster.TerrainConstructConstants.SqSize);
            SqLateralLength = (int)((double)SqSize * Math.Sqrt(2));
            PlayerIndexTheMan = PlayerQ;
            TurnNumber = 0;

            double resSQRatioPerMap = (double)Setting.GetConstant(AS.ResConstant,(int)SettingsMaster.ResConstantParams.ResSqRatio);
            MaxResourceSQsOnMap = (int)(SqQ * resSQRatioPerMap);
            MaxResourceSQsInPool = (int)Setting.GetConstant(AS.ResConstant, (int)SettingsMaster.ResConstantParams.MaxPoolSQ);

            WidthOfActionPaneRatioOfScreenSize = 0.2;
            StartSQProduction = 8;
            StartSQOpex = 15;
            UnitCounter = 0;
            NameOfOwnerOfUnOwnedSquares = "The Man";
            OPEXDiscountPerSQInUnit = 0.03;
            CAPEXDiscountPerSQInUnit = 0.03;
        }
        public static int PlayerQ { get; set; }
        public static int RowQ { get; set; }
        public static int ColQ { get; set; }
        public static int SqQ { get; set; }       
        public static int SqSize { get; set; }
        public static int SqLateralLength { get; set; }
        public static int TurnNumber { get; set; }
        public static int PlayerIndexTheMan { get; set; }        
        public static double WidthOfActionPaneRatioOfScreenSize { get; set; }
        public static int MaxResourceSQsOnMap { get; set; }
        public static int MaxResourceSQsInPool { get; set; }
        public static int StartSQProduction { get; set; }
        public static double StartSQOpex { get; set; }
        public static int PlayerIndexActual => 0;      
        public static int UnitCounter { get; set; }
        public static string NameOfOwnerOfUnOwnedSquares { get; set; }
        public static double OPEXDiscountPerSQInUnit { get; set; }
        public static double CAPEXDiscountPerSQInUnit { get; set; }
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
    }
}
