using System;
using TheManXS.ViewModel.MapBoardVM.MainElements;
using Xamarin.Forms;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.Scroll
{
    class CornersPointArray
    {
        private int _fGridOverhangX;
        private int _fGridOverhangY;
        private bool[] _done;
        private int _minimumQuantityOfTilesInFocusedGrid;
        private MapScrollView _mapScrollView;
        private FocusedABS _focusedABS;

        public int ScrollXX { get; set; }
        public int ScrollYY { get; set; }
        public CornersPointArray(ActualGameBoardVM a)
        {
            _mapScrollView = a.GameBoardSplitScreenGrid.MapScrollView;
            
            _done = new bool[ScrollConstants.TotDim];

            _focusedABS = _mapScrollView.PinchToZoomContainer.GameBoard.FocusedGameBoard;

            _fGridOverhangX = (int)((_focusedABS.Col.Quantity - (QC.ScreenWidth / QC.SqSize)) / 2) * QC.SqSize;
            _fGridOverhangY = (int)((_focusedABS.Row.Quantity - (QC.ScreenHeight / QC.SqSize)) / 2) * QC.SqSize;
            _minimumQuantityOfTilesInFocusedGrid = (int)((_focusedABS.Col.Quantity + _focusedABS.Row.Quantity) * 0.9);
        }
        public void OnScroll(double scrollX, double scrollY)
        {
            ScrollXX = (int)scrollX;
            ScrollYY = (int)scrollY;
            bool allRequiredShiftsDone = false;

            do
            {
                CheckZRatios();
                allRequiredShiftsDone = CheckIfDone();

                if (!allRequiredShiftsDone)
                {
                    CheckZRatios();
                }
            } while (!allRequiredShiftsDone);
            ResetDoneArray();
        }
        void CheckZRatios()
        {
            int z = 0, zzz = 0;

            for (int i = 0; i < ScrollConstants.TotDim; i++)
            {
                int fGridOverhang = (i % 2 == 0) ? _fGridOverhangY : _fGridOverhangX;
                z = GetZ(i);
                                
                if(z < fGridOverhang && fGridOverhang - z >= QC.SqSize)
                {
                        if(GetZZ(i) >= QC.SqSize)
                        {
                            zzz = GetZZZ(i);
                            if (zzz >= QC.SqSize)
                            {
                                _mapScrollView.ScrollHandler.ExecuteShift(i, GetNumberOfSegmentsToAdd(zzz));
                                _done[i] = false;
                            }
                            else { _done[i] = true; }
                        }
                        else { _done[i] = true; }
                }
                else { _done[i] = true; }
            }            
        }   
        // distance between focused edge and screen edge
        private int GetZ(int i)
        {
            switch (i)
            {
                case 0:
                    return ScrollXX == 0 ? 0 : (ScrollXX - (_focusedABS.Col.Start * QC.SqSize));
                case 1:
                    return ScrollYY == 0 ? 0 : (ScrollYY - (_focusedABS.Row.Start * QC.SqSize));
                case 2:
                    return ((_focusedABS.Col.End * QC.SqSize) - (ScrollXX + QC.ScreenWidth));
                case 3:
                    return ((_focusedABS.Row.End * QC.SqSize) - (ScrollYY + QC.ScreenHeight));
                default:
                    return 0;
            }
        }
        // distance between screen edge and edge of map
        private int GetZZ(int i)
        {
            switch(i)
            {
                case 0:
                    return (ScrollXX);
                case 1:
                    return (ScrollYY);
                case 2:
                    return ((QC.SqSize * QC.ColQ) - (ScrollXX + QC.ScreenWidth));
                case 3:
                    return ((QC.SqSize * QC.RowQ) - (ScrollYY + QC.ScreenHeight));
                default:
                    return 0;
            }
        }
        // distance between focused edge and edge of map
        private int GetZZZ(int i)
        {
            switch (i)
            {
                case 0:
                    return _focusedABS.Col.Start;
                case 1:
                    return _focusedABS.Row.Start;
                case 2:
                    return ((QC.SqSize * QC.ColQ) - (_focusedABS.Col.End * QC.SqSize));
                case 3:
                    return ((QC.SqSize * QC.RowQ) - (_focusedABS.Row.End * QC.SqSize));
                default:
                    return 0;
            }
        }
        private int GetNumberOfSegmentsToAdd(double zzz) => ((int)(zzz / QC.SqSize) > ScrollConstants.NumberOfSegmentsToHaveReady) ?
                    ScrollConstants.NumberOfSegmentsToHaveReady : (int)(zzz / QC.SqSize);
        bool CheckIfDone()
        {
            for (int i = 0; i < _done.Length; i++) { if (!_done[i]) return false; }
            if(_mapScrollView.PinchToZoomContainer.GameBoard.FocusedGameBoard.Count <= _minimumQuantityOfTilesInFocusedGrid) { return false; }
            return true;
        }
        void ResetDoneArray() { for (int i = 0; i < _done.Length; i++) { _done[i] = false; }}
    }
}
