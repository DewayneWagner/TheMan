using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Company;
using TheManXS.Model.Financial.CommodityStuff;
using TheManXS.Model.Map;
using TheManXS.Model.Settings;
using TheManXS.ViewModel.Services;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.Model.Main
{
    public class Game
    {
        PageService _pageService = new PageService();
        public Game(GameSpecificParameters gsp, bool isNewGame)
        {
            InitClassesNeededForNewOrLoadedGame();
            QC.CurrentSavedGameSlot = gsp.Slot;

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

        }
        private void InitClassesNeededForNewOrLoadedGame()
        {
            //new QuickConstants(); moved initialization to MainMenu
            //new TerrainImage();
        }
        private void InitClassesNeededForNewGameMethods()
        {            
            new PlayerList(true);
            new CommodityList(true);
            Map = new GameBoardMap(true);
        }
        private void InitClassesNeededForLoadedGame()
        {
            
        }

        public GameBoardMap Map { get; set; }
    }
}
