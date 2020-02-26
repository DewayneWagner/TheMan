using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Company;
using TheManXS.Model.Financial.CommodityStuff;
using TheManXS.Model.Map;
using TheManXS.Model.Services.EntityFrameWork;
using TheManXS.Model.Settings;
using TheManXS.Model.Units;
using TheManXS.ViewModel.MapBoardVM.MainElements;
using TheManXS.ViewModel.Services;
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
            InitClassesNeededForNewOrLoadedGame();
            QC.CurrentSavedGameSlot = gsp.Slot;
            _gsp = gsp;

            if (isNewGame)
            {
                InitClassesNeededForNewGameMethods();
                //
            }
            else if (!isNewGame)
            {
                InitClassesNeededForLoadedGame();
                Map = new GameBoardMap(false);
            }
            LoadDictionaries();
        }
        private void InitClassesNeededForNewOrLoadedGame()
        {
            //new QuickConstants(); moved initialization to MainMenu
            //new TerrainImage();
        }
        private void InitClassesNeededForNewGameMethods()
        {            
            PlayerList = new PlayerList(_gsp);
            ActivePlayer = PlayerList[QC.PlayerIndexActual];

            new CommodityList(true);
            Map = new GameBoardMap(true);
        }
        private void InitClassesNeededForLoadedGame()
        {
            
        }

        public GameBoardMap Map { get; set; }
        public Dictionary<int, SQ> SquareDictionary { get; set; } = new Dictionary<int, SQ>();
        public List<Player> PlayerList { get; set; }
        public List<Commodity> CommodityList { get; set; } = new List<Commodity>();
        public GameBoardVM GameBoardVM { get; set; }

        public Player ActivePlayer { get; set; }
        public SQ ActiveSQ { get; set; }
        public Unit ActiveUnit { get; set; }

        private void LoadDictionaries()
        {
            using (DBContext db = new DBContext())
            {
                SquareDictionary = db.SQ.ToDictionary(sq => sq.Key);
                CommodityList = db.Commodity.ToList();
            }
        }
    }
}
