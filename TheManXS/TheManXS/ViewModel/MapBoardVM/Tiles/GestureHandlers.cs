using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Map.Surface;
using TheManXS.Model.Services.EntityFrameWork;
using TheManXS.Model.Units;
using TheManXS.View;
using TheManXS.ViewModel.MapBoardVM.Map;
using TheManXS.ViewModel.Services;
using Xamarin.Forms;

namespace TheManXS.ViewModel.MapBoardVM.Tiles
{
    public class GestureHandlers : BaseViewModel
    {
        private PageService _pageService;
        private bool tapHandled;
        private int _clickedRow = 0;
        private int _clickedCol = 0;
        private GameBoardSplitScreenGrid _gameBoardSplitGrid;
        private SQ _activeSQ;

        public GestureHandlers(Tile tile)
        {
            ActiveTile = tile;
            _pageService = new PageService();
            GameBoardVM g = (GameBoardVM)(Application.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.GameBoardVM)]);
            _gameBoardSplitGrid = g.ActualGameBoardVM.GameBoardSplitScreenGrid;
        }

        public Tile ActiveTile { get; set; }

        public void SingleTap(object sender, EventArgs e)
        {
            Tile t = sender as Tile;
            _clickedRow = t.Row;
            _clickedCol = t.Col;

            SetActiveSQ();

            tapHandled = false;
            ActiveTile = (Tile)sender;
            Device.StartTimer(new TimeSpan(0, 0, 0, 0, 300), ExecuteIfSingleTap);
        }
        public void DoubleTap(object sender, EventArgs e)
        {
            Tile t = sender as Tile;
            _clickedRow = t.Row;
            _clickedCol = t.Col;

            tapHandled = true;
            ExecuteOnDoubleTap();
        }

        private bool ExecuteIfSingleTap()
        {
            if (!tapHandled) { ExecuteOnSingleTap(); }            
            return false;
        }        

        private void ExecuteOnSingleTap()
        {
            _gameBoardSplitGrid.AddSideActionPanel(MapBoardVM.Action.ActionPanel.PanelType.SQ,ActiveTile);
        }
        private void ExecuteOnDoubleTap()
        {
            Tile t = _gameBoardSplitGrid.MapScrollView.PinchToZoomContainer.GameBoard.FocusedGameBoard.GetTile(_clickedRow, _clickedCol);
            if (!_gameBoardSplitGrid.IsThereActiveUnit)
            {
                Unit activeUnit = new Unit(t.SQ);
                Application.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.ActiveUnit)] = activeUnit;
                _gameBoardSplitGrid.ActiveUnit = activeUnit;
                _gameBoardSplitGrid.IsThereActiveUnit = true;
                _gameBoardSplitGrid.AddSideActionPanel(MapBoardVM.Action.ActionPanel.PanelType.Unit, t);
            }
            else if(_gameBoardSplitGrid.ActiveUnit.IsSQAdjacentToSQsAlreadyInUnit(t.SQ))
            { 
                _gameBoardSplitGrid.ActiveUnit.AddSQToUnit(t.SQ); 
            }
        }

        void SetActiveSQ()
        {
            using (DBContext db = new DBContext())
            {
                SQ activeSQ = db.SQ.Find(Coordinate.GetSQKey(_clickedRow, _clickedCol));
                Application.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.ActiveSQ)] = activeSQ;
                _activeSQ = activeSQ;
            }
        }
    }
}
