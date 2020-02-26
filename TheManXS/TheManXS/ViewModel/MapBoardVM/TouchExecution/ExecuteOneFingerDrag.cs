using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Map.Surface;
using TheManXS.ViewModel.MapBoardVM.MainElements;
using TheManXS.ViewModel.MapBoardVM.TouchTracking;
using ST = TheManXS.Model.Settings.SettingsMaster.StatusTypeE;
using RT = TheManXS.Model.Settings.SettingsMaster.ResourceTypeE;
using QC = TheManXS.Model.Settings.QuickConstants;
using TheManXS.Model.Main;
using TheManXS.Model.Units;
using TheManXS.ViewModel.MapBoardVM.Action;
using Xamarin.Forms;

namespace TheManXS.ViewModel.MapBoardVM.TouchExecution
{
    public class ExecuteOneFingerDrag
    {
        /******************************
         * 
         * There are 3 reasons to execute a drag select of multiple squares:
         * 1. selecting multiple unowned / unexplored squares to purchase all at once
         * 2. selecting multiple squares that are owned by the player, explored, and have the same resource present to combine
         * into a production unit
         * 3. combine producing sq's into production unit?
         * 
         * ***************************/

        Game _game;
        private List<SQ> _listOfTouchedSQs;
        private List<SQ> _filteredList;
        public ExecuteOneFingerDrag(Game game)
        {
            _game = game;
            ExecuteOneFingerDragSelect();
        }

        private void ExecuteOneFingerDragSelect()
        {
            _listOfTouchedSQs = GetListOfTouchedSQs();
            CreateFilteredListOfSqsToBeIncluded();
            _game.GameBoardVM.MapVM.ActiveUnit = new Unit(_filteredList,_game);
            addSidePanel();
        }

        private List<SQ> GetListOfTouchedSQs()
        {
            List<SQ> listOfTouchedSQs = new List<SQ>();
            foreach (TouchActionEventArgs args in _game.GameBoardVM.MapVM.MapTouchList[0])
            {
                SKPoint touchPointOnMap = GetTouchPointOnBitMap(args.SKPoint);
                Coordinate touchCoord = new Coordinate(touchPointOnMap);
                SQ sq = _game.SquareDictionary[touchCoord.SQKey];
                if (!listOfTouchedSQs.Contains(sq)) { listOfTouchedSQs.Add(sq); }
            }
            return listOfTouchedSQs;
        }

        void CreateFilteredListOfSqsToBeIncluded()
        {
            _filteredList = new List<SQ>();
            SQ firstTouchedSQ = _listOfTouchedSQs[0];

            if (isNotOwnedAndStatusNada()) { filterOutSqsThatAreOwned(); }
            else if (isReadyToFormAProductionUnit()) { createFilteredListOfSQsThatAreReadyForProductionUnit(); }
            
            bool isNotOwnedAndStatusNada() => firstTouchedSQ.Status == ST.Nada
                && firstTouchedSQ.OwnerNumber == QC.PlayerIndexTheMan ? true : false;

            bool isReadyToFormAProductionUnit() => firstTouchedSQ.Status == ST.Explored && firstTouchedSQ.ResourceType != RT.Nada ?
                true : false;

            void createFilteredListOfSQsThatAreReadyForProductionUnit()
            {
                _filteredList.Add(firstTouchedSQ);
                _listOfTouchedSQs.Remove(firstTouchedSQ);

                foreach (SQ sq in _listOfTouchedSQs)
                {
                    if (sq.OwnerNumber == QC.PlayerIndexActual && sq.Status == ST.Explored && sq.ResourceType == firstTouchedSQ.ResourceType)
                    {
                        _filteredList.Add(sq);
                    }
                }
            }

            void filterOutSqsThatAreOwned()
            {
                foreach (SQ sq in _listOfTouchedSQs) 
                { 
                    if (sq.OwnerNumber == QC.PlayerIndexTheMan) { _filteredList.Add(sq); } 
                }
            }
        }

        private SKPoint GetTouchPointOnBitMap(SKPoint pt)
        {
            var m = _game.GameBoardVM.MapVM;
            float bitmapX = ((pt.X - m.MapMatrix.TransX) / m.MapMatrix.ScaleX);
            float bitmapY = ((pt.Y - m.MapMatrix.TransY) / m.MapMatrix.ScaleY);

            return new SKPoint(bitmapX, bitmapY);
        }

        void addSidePanel()
        {
            var g = _game.GameBoardVM;
            g.ActionPanelGrid = new ActionPanelGrid(ActionPanelGrid.PanelType.Unit, _game);
            g.SplitScreenGrid.ColumnDefinitions.Add(new Xamarin.Forms.ColumnDefinition()
                { Width = new GridLength(1, GridUnitType.Auto) });
            g.SplitScreenGrid.Children.Add(_game.GameBoardVM.ActionPanelGrid, 1, 0);
            g.SideSQActionPanelExists = true;
            g.MapVM.TouchEffectsEnabled = false;
        }
    }
}
