using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Company;
using TheManXS.Model.Financial;
using TheManXS.Model.Financial.CommodityStuff;
using TheManXS.Model.Map;
using TheManXS.Model.Services.EntityFrameWork;
using TheManXS.Model.Settings;
using TheManXS.Model.Units;
using TheManXS.Services.EntityFrameWork;
using TheManXS.ViewModel.MapBoardVM.MainElements;
using TheManXS.ViewModel.Services;
using Xamarin.Forms;
using QC = TheManXS.Model.Settings.QuickConstants;
using TheManXS.Model.ParametersForGame;
using TheManXS.ViewModel.Style;
using TheManXS.Model.Financial.Debt;

namespace TheManXS.Model.Main
{
    public class Game
    {
        PageService _pageService = new PageService();
        private GameSpecificParameters _gsp;
        private GameBoardMap _map;

        public Game(bool isForAppDictionary) 
        {
            LoadAllParameters();
            InitQuickConstants();
        }

        public Game(GameSpecificParameters gsp, bool isNewGame)
        {
            App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.Game)] = this;
            App.MapHasBeenCreated = true;
            LoadAllParameters();
            QC.CurrentSavedGameSlot = gsp.Slot;
            _gsp = gsp;

            if (isNewGame) 
            {
                new DBPurgeForNewGame();
                InitPropertiesForNewGame();
            }

            else if (!isNewGame)
            {
                InitPropertiesForLoadedGame();
                _map = new GameBoardMap(this,false);
            }
        }
        private void InitPropertiesForNewGame()
        {
            //LoadAllParameters();
            Quarter = "1900-Q1";
            PlayerList = new PlayerList(_gsp,this);
            ActivePlayer = PlayerList[QC.PlayerIndexActual];

            // this is the problem.....
            _map = new GameBoardMap(this,true);

            CommodityList = new CommodityList(this);
            FinancialValuesList = new FinancialValuesList(this);
            PrimeInterestRate = ParameterConstantList.GetConstant(AllConstantParameters.CashConstant, (int)CashConstantSecondary.StartPrimeRate) / 100;
            LoanList = new LoansList(this, true);
#if DEBUG
            DebugTesting.InitExtraLoansForEachPlayer(this);
#endif
        }
        private void InitPropertiesForLoadedGame()
        {
            using (DBContext db = new DBContext())
            {
                GameSpecificParameters gsp = db.GameSpecificParameters.Where(g => g.Slot == QC.CurrentSavedGameSlot).FirstOrDefault();
                Quarter = gsp.Quarter;
                TurnNumber = gsp.TurnNumber;

                PlayerList = new PlayerList(true);
                ActivePlayer = PlayerList[gsp.ActivePlayerNumber];

                CommodityList = new CommodityList(true);
                FinancialValuesList = new FinancialValuesList(true);

                ParameterBoundedList = new ParameterBoundedList();
                ParameterConstantList = new ParameterConstantList();
                PrimeInterestRate = ParameterConstantList.GetConstant(AllConstantParameters.CashConstant, (int)CashConstantSecondary.StartPrimeRate) / 100;
                LoanList = new LoansList(this, false);

                Unit.LoadUnitWithSavedGameData(this);
            }            
        }

        public SQList SQList { get; set; } = new SQList();
        public List<Unit> ListOfCreatedProductionUnits { get; set; }
        public LoansList LoanList { get; set; }
        public PlayerList PlayerList { get; set; }
        public CommodityList CommodityList { get; set; } 
        public FinancialValuesList FinancialValuesList { get; set; }
        public GameBoardVM GameBoardVM { get; set; }
        public Player ActivePlayer { get; set; }
        public SQ ActiveSQ { get; set; }
        public Unit ActiveUnit { get; set; }
        public string Quarter { get; set; }
        public int TurnNumber { get; set; }
        public double PrimeInterestRate { get; set; }       
        public ParameterBoundedList ParameterBoundedList { get; set; }
        public ParameterConstantList ParameterConstantList { get; set; }
        public PaletteColorList PaletteColors { get; set; } = new PaletteColorList();

        private void LoadAllParameters()
        {
            ParameterBoundedList = new ParameterBoundedList();
            ParameterConstantList = new ParameterConstantList();
        }
        private void InitQuickConstants()
        {
            int playerQ = (int)ParameterConstantList.GetConstant(AllConstantParameters.GameConstants, (int)GameConstantsSecondary.PlayerQ);
            int rowQ = (int)ParameterConstantList.GetConstant(AllConstantParameters.MapConstants, (int)MapConstantsSecondary.RowQ);
            int colQ = (int)ParameterConstantList.GetConstant(AllConstantParameters.MapConstants, (int)MapConstantsSecondary.ColQ);
            int sqSize = (int)ParameterConstantList.GetConstant(AllConstantParameters.MapConstants, (int)MapConstantsSecondary.SqSize);
            double theManCut = ParameterConstantList.GetConstant(AllConstantParameters.CashConstant, (int)CashConstantSecondary.TheManCut);
            int maxResourceSqsInPool = (int)ParameterConstantList.GetConstant(AllConstantParameters.ResourceConstant, (int)ResourceConstantSecondary.MaxPoolSQ);
            int maxResourceSQsOnMap = (int)(ParameterConstantList.GetConstant(AllConstantParameters.ResourceConstant, (int)ResourceConstantSecondary.ResSqRatio) * (rowQ * colQ));

            QC.InitProperties(playerQ, rowQ, colQ, sqSize, theManCut, maxResourceSqsInPool, maxResourceSQsOnMap);
        }
    }
}
