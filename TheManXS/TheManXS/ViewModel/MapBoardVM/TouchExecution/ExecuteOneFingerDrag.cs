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

        GameBoardVM _gameBoardVM;
        //private Dictionary<int, Coordinate> _touchedSQsInDragEvent;
        private List<SQ> _listOfTouchedSQs;
        private List<SQ> _filteredList;
        public ExecuteOneFingerDrag(GameBoardVM gameBoardVM)
        {
            _gameBoardVM = gameBoardVM;
            ExecuteOneFingerDragSelect();
        }

        private void ExecuteOneFingerDragSelect()
        {
            _listOfTouchedSQs = GetListOfTouchedSQs();
            //_touchedSQsInDragEvent = GetDictionaryOfTouchedSQs();
            CreateFilteredListOfSqsToBeIncluded();
            _gameBoardVM.MapVM.ActiveUnit = new Unit(_filteredList);
            HighlightAllTouchedSQs();
            addSidePanel();

            // this section displays message with touched squares - delete later.
            //string message = null;
            //foreach (KeyValuePair<int, Coordinate> coordinate in touchedSQsInDragEvent)
            //{ message += Convert.ToString(coordinate.Key) + "\n"; }
            //await _pageService.DisplayAlert(message);
        }

        private List<SQ> GetListOfTouchedSQs()
        {
            List<SQ> listOfTouchedSQs = new List<SQ>();
            foreach (TouchActionEventArgs args in _gameBoardVM.MapVM.MapTouchList[0])
            {
                SKPoint touchPointOnMap = GetTouchPointOnBitMap(args.SKPoint);
                Coordinate touchCoord = new Coordinate(touchPointOnMap);
                SQ sq = _gameBoardVM.MapVM.SquareDictionary[touchCoord.SQKey];
                if (!listOfTouchedSQs.Contains(sq)) { listOfTouchedSQs.Add(sq); }
            }
            return listOfTouchedSQs;
        }

        private Dictionary<int, Coordinate> GetDictionaryOfTouchedSQs()
        {
            Dictionary<int, Coordinate> touchedSQs = new Dictionary<int, Coordinate>();
            foreach (TouchActionEventArgs args in _gameBoardVM.MapVM.MapTouchList[0])
            {
                SKPoint touchPointOnMap = GetTouchPointOnBitMap(args.SKPoint);
                Coordinate touchCoord = new Coordinate(touchPointOnMap);
                if (!touchedSQs.ContainsKey(touchCoord.SQKey)) { touchedSQs.Add(touchCoord.SQKey, touchCoord); }
            }
            return touchedSQs;
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

        private void HighlightAllTouchedSQs()
        {
            




        }

        private SKPoint GetTouchPointOnBitMap(SKPoint pt)
        {
            float bitmapX = ((pt.X - _gameBoardVM.MapVM.MapMatrix.TransX) / _gameBoardVM.MapVM.MapMatrix.ScaleX);
            float bitmapY = ((pt.Y - _gameBoardVM.MapVM.MapMatrix.TransY) / _gameBoardVM.MapVM.MapMatrix.ScaleY);

            return new SKPoint(bitmapX, bitmapY);
        }

        void addSidePanel()
        {
            _gameBoardVM.ActionPanelGrid = new ActionPanelGrid(ActionPanelGrid.PanelType.Unit, _gameBoardVM);
            _gameBoardVM.SplitScreenGrid.ColumnDefinitions.Add(new Xamarin.Forms.ColumnDefinition()
                { Width = new GridLength(1, GridUnitType.Auto) });
            _gameBoardVM.SplitScreenGrid.Children.Add(_gameBoardVM.ActionPanelGrid, 1, 0);
            _gameBoardVM.SideSQActionPanelExists = true;
            _gameBoardVM.MapVM.TouchEffectsEnabled = false;
        }
    }
}
