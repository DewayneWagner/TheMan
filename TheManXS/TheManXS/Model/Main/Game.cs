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
using TheManXS.ViewModel.MapBoardVM.MainElements;
using TheManXS.ViewModel.Services;
using Xamarin.Forms;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.Model.Main
{
    public class Game
    {
        PageService _pageService = new PageService();
        private GameSpecificParameters _gsp;
        public Game(bool isForAppDictionary) { }
        public Game(GameSpecificParameters gsp, bool isNewGame)
        {
            App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.Game)] = this;
            QC.CurrentSavedGameSlot = gsp.Slot;
            _gsp = gsp;

            if (isNewGame) 
            { 
                InitPropertiesForNewGame();
                RemoveDataFromCurrentSavedGameSlot();
            }

            else if (!isNewGame)
            {
                InitPropertiesForLoadedGame();
                Map = new GameBoardMap(this,false);
            }
        }
        private void InitPropertiesForNewGame()
        {
            Quarter = "1900-Q1";
            TurnNumber = 1;
            PlayerList = new PlayerList(_gsp);
            ActivePlayer = PlayerList[QC.PlayerIndexActual];
            Map = new GameBoardMap(this,true);
            CommodityList = new CommodityList(this);
            FinancialValuesList = new FinancialValuesList(this);
        }
        private void InitPropertiesForLoadedGame()
        {
            
        }

        void WriteClassesWithJSON()
        {
            // classes to write to JSON

        }
        void ReadClassesFromJSON()
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

        void RemoveDataFromCurrentSavedGameSlot()
        {
            using (DBContext db = new DBContext())
            {
                // commodity pricing
                var cList = db.Commodity.Where(c => c.SavedGameSlot == QC.CurrentSavedGameSlot).ToList();
                if (cList.Count > 0) { db.RemoveRange(cList); }

                // financial values list
                var fList = db.Commodity.Where(f => f.SavedGameSlot == QC.CurrentSavedGameSlot).ToList();
                if(fList.Count > 0) { db.RemoveRange(fList); }

                // sq data - (done somewhere else?)
                var sqList = db.SQ.Where(s => s.SavedGameSlot == QC.CurrentSavedGameSlot).ToList();
                if(sqList.Count > 0) { db.RemoveRange(sqList); }

                // sq infrastructure
                var sqInfraList = db.SQInfrastructure.Where(s => s.SavedGameSlot == QC.CurrentSavedGameSlot).ToList();
                if(sqInfraList.Count > 0) { db.RemoveRange(sqInfraList); }

                // formation
                var formationList = db.Formation.Where(f => f.SavedGameSlot == QC.CurrentSavedGameSlot).ToList();
                if(formationList.Count > 0) { db.RemoveRange(formationList); }
            }
        }
    }
}
