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

namespace TheManXS.Model.Main
{
    public class Game
    {
        PageService _pageService = new PageService();
        private GameSpecificParameters _gsp;
        private double _startingPrimeInterestRate = 0.05;
        public Game(bool isForAppDictionary) { }
        public Game(GameSpecificParameters gsp, bool isNewGame)
        {
            App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.Game)] = this;
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
                Map = new GameBoardMap(this,false);
            }
        }
        private void InitPropertiesForNewGame()
        {
            LoadAllParameters();
            PlayerList = new PlayerList(_gsp,this);
            ActivePlayer = PlayerList[QC.PlayerIndexActual];
            Map = new GameBoardMap(this,true);
            CommodityList = new CommodityList(this);
            FinancialValuesList = new FinancialValuesList(this);
            PrimeInterestRate = _startingPrimeInterestRate;

            ParameterBoundedList.WriteDataToBinaryFile();
            ParameterConstantList.WriteDataToBinaryFile();
        }
        private void InitPropertiesForLoadedGame()
        {
            
        }

        public GameBoardMap Map { get; set; }
        public Dictionary<int, SQ> SquareDictionary { get; set; } = new Dictionary<int, SQ>();
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

        private void LoadAllParameters()
        {
            ParameterBoundedList = new ParameterBoundedList();
            ParameterConstantList = new ParameterConstantList();
        }
    }
}
