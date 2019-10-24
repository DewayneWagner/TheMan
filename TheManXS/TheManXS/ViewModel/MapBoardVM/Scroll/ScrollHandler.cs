using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using TheManXS.ViewModel.MapBoardVM.MainElements;
using TheManXS.ViewModel.Services;
using Xamarin.Forms;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.Scroll
{
    public class ScrollHandler
    {
        private int _counter;
        private ActualGameBoardVM _actualGameBoardVM;

        public ScrollHandler(ActualGameBoardVM a)
        {
            _actualGameBoardVM = a;

            _counter = 1;

            SegmentArray = new SegmentArray(_actualGameBoardVM);
            SegmentArray.InitNewSegments();

            CornerPointArray = new CornersPointArray(_actualGameBoardVM);
        }

        public SegmentArray SegmentArray { get; set; }
        private CornersPointArray CornerPointArray { get; set; }
        
        public void OnScroll(object sender, ScrolledEventArgs e)
        {
            if (_counter % 2 == 0)
            {
                _actualGameBoardVM.GameBoardSplitScreenGrid.MapScrollView.ScrollViewIsEnabled = false;
                CornerPointArray.OnScroll(e.ScrollX,e.ScrollY);
            }
            _counter++;
        }
        public void ExecuteShift(int direction, int numberOfSegmentsToAdd)
        {
            MapScrollView.Direction d = (MapScrollView.Direction)direction;

            for (int i = 0; i < numberOfSegmentsToAdd; i++)
            {
                if(SegmentArray.CountSegments(d) > 0)
                {
                    Segment s = SegmentArray.GetNextSegment(d);
                    _actualGameBoardVM.GameBoardSplitScreenGrid.MapScrollView.PinchToZoomContainer.GameBoard.FocusedGameBoard.ExecuteShift(s);
                }
                else { break; }
            }
            SegmentArray.InitNewSegments();

            Thread.Sleep(ScrollConstants.MillsecondsToDelayScroll);
            _actualGameBoardVM.GameBoardSplitScreenGrid.MapScrollView.ScrollViewIsEnabled = true;
        }
    }
}
