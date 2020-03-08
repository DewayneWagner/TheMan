using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Settings;
using TheManXS.Model.Services.EntityFrameWork;
using TheManXS.Model.Main;
using BP = TheManXS.Model.ParametersForGame.AllBoundedParameters;
using CP = TheManXS.Model.ParametersForGame.AllConstantParameters;
using TheManXS.Model.ParametersForGame;

namespace TheManXS.Model.Settings
{
    public class QuickConstants
    {
        Game _game;
        public QuickConstants()
        {
            _game = (Game)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.Game)];

            InitProperties();
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
        void InitProperties()
        {
            PlayerQ = (int)_game.ParameterConstantList.GetConstant(CP.GameConstants, (int)GameConstantsSecondary.PlayerQ);
            RowQ = (int)_game.ParameterConstantList.GetConstant(CP.MapConstants, (int)MapConstantsSecondary.RowQ);
            ColQ = (int)_game.ParameterConstantList.GetConstant(CP.MapConstants, (int)MapConstantsSecondary.ColQ);
            SqQ = RowQ * ColQ;
            SqSize = (int)_game.ParameterConstantList.GetConstant(CP.MapConstants, (int)MapConstantsSecondary.SqSize);
            SqLateralLength = (int)((double)SqSize * Math.Sqrt(2));
            PlayerIndexTheMan = PlayerQ;
            TheManCut = _game.ParameterConstantList.GetConstant(CP.CashConstant, (int)CashConstantSecondary.TheManCut);
            MaxResourceSQsInPool = (int)_game.ParameterConstantList.GetConstant(CP.ResourceConstant, (int)ResourceConstantSecondary.MaxPoolSQ);
            MaxResourceSQsOnMap = (int)_game.ParameterConstantList.GetConstant(CP.ResourceConstant, (int)ResourceConstantSecondary.ResSqRatio) * SqSize;

            UnitCounter = 0;
        }
    }
}
