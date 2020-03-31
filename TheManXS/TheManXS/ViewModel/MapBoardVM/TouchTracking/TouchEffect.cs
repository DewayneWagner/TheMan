using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Main;
using Xamarin.Forms;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.TouchTracking
{
    
    public enum TouchActionType { Entered, Pressed, Moved, Released, Exited, Cancelled }
    public enum MapTouchType { OneFingerSelect, OneFingerDragSelect, TwoFingerPan, Pinch }
    public enum Direction { West, North, East, South, Total }

    public delegate void TouchActionEventHandler(object sender, TouchActionEventArgs args);
    
    public class TouchEffect : RoutingEffect
    {
        public event TouchActionEventHandler TouchAction;
        public TouchEffect() : base("TheMan.TouchEffect") { }

        public bool Capture { get; set; }
        public void OnTouchAction(Element element, TouchActionEventArgs args)
        {
            TouchAction?.Invoke(element, args);
        }
    }
    public class TouchActionEventArgs : EventArgs
    {
        private static Game _game;
        private bool _touchPointIsOnVisiblePortionOfMap;
        public TouchActionEventArgs(long id, TouchActionType type, Point location, bool isInContact)
        {
            if(_game == null) { _game = (Game)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.Game)]; }

            Id = id;
            Type = type;
            Location = location;
            IsInContact = isInContact;
            SKPoint = GetSKPointFromXPoint(location);
            //SKPoint = GraphicsCalculations.GetSKPointFromXPoint(location);
        }
        public long Id { get; set; }
        public TouchActionType Type { get; set; }
        public Point Location { get; set; }
        public bool IsInContact { get; set; }
        public SKPoint SKPoint { get; set; }

        private SKPoint GetSKPointFromXPoint(Point pt)
        {
            //public static SKPoint GetSKPointFromXPoint(Point pt) => 
            //    new SKPoint((float)(pt.X * (QC.ScreenWidth / QC.MapCanvasViewWidth)),
            //        (float) (pt.Y * (QC.ScreenHeight / QC.MapCanvasViewHeight)));

            return new SKPoint(
                (float)(pt.X * (QC.MapCanvasViewWidth / QC.ScreenWidth)),
                (float)(pt.Y * (QC.MapCanvasViewHeight / QC.ScreenHeight)));

            //return new SKPoint(
            //    (float)(pt.X * (QC.ScreenWidth / QC.MapCanvasViewWidth)),
            //    (float)(pt.Y * (QC.ScreenHeight / QC.MapCanvasViewHeight)));
            
            //SKPoint p = new SKPoint();
            //var m = _game.GameBoardVM.MapVM.MapCanvasView;

            //double topLeftCornerOfBitMapOnScreenX = m.X;
            //double topRightCornerOfBitMapOnScreenY = m.Y;

            //double widthOfVisibleMapCanvasView = m.Width;
            //double heightOfVisibleMapCanvasView = m.Height;

            //double actualTouchPointOnMapCanvasViewX = 

            //return p;
        }
    }   
}
