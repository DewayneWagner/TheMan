using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using TheManXS.ViewModel.MapBoardVM.Pinch;
using TheManXS.ViewModel.MapBoardVM.Scroll;
using Xamarin.Forms;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.MainElements
{
    public class MapScrollView : ScrollView
    {
        public enum Direction { W, N, E, S, Total }

        GameBoardVM g;
        ActualGameBoardVM _actualGameBoardVM;

        public MapScrollView(ActualGameBoardVM a)
        {
            _actualGameBoardVM = a;
            CompressedLayout.SetIsHeadless(this, true);

            Orientation = ScrollOrientation.Both;
            VerticalOptions = LayoutOptions.FillAndExpand;
            HorizontalOptions = LayoutOptions.FillAndExpand;
            
            Margin = 0;
            Padding = 0;

            //ScrollHandler = new ScrollHandler(_actualGameBoardVM);
            //Scrolled += ScrollHandler.OnScroll;

            //PinchToZoomContainer = new PinchToZoomContainer(_actualGameBoardVM);
            //Content = PinchToZoomContainer;
        }

        public void InitChildrenClasses(ActualGameBoardVM a)
        {
            PinchToZoomContainer = new PinchToZoomContainer(_actualGameBoardVM);
            Content = PinchToZoomContainer;

            ScrollHandler = new ScrollHandler(_actualGameBoardVM);
            Scrolled += ScrollHandler.OnScroll;
        }

        public PinchGestureRecognizer Pinch { get; set; }     
        public ScrollHandler ScrollHandler { get; set; }

        private PinchToZoomContainer _pinchToZoomContainer;
        public PinchToZoomContainer PinchToZoomContainer
        {
            get => _pinchToZoomContainer;
            set
            {
                _pinchToZoomContainer = value;
                _actualGameBoardVM.SetValue(ref _pinchToZoomContainer, value);
            }
        }

        private bool _isEnabled;
        public bool ScrollViewIsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                _actualGameBoardVM.SetValue(ref _isEnabled, value);
            }
        }
        
        //private async void ScrollToCenter() => await ScrollToAsync(g.CenterTile, ScrollToPosition.Center, true);
        private async void ScrollToCenter() => await ScrollToAsync(Width / 2, Height / 2, false);
    }
}
