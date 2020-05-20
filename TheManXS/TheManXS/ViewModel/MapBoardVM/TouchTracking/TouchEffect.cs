using SkiaSharp;
using System;
using TheManXS.Model.Main;
using TheManXS.Model.Map.Surface;
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
        public TouchActionEventArgs(long id, TouchActionType type, Xamarin.Forms.Point location, bool isInContact)
        {
            if (_game == null) { _game = (Game)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.Game)]; }

            Id = id;
            Type = type;
            Location = location;
            IsInContact = isInContact;
        }
        public long Id { get; set; }
        public TouchActionType Type { get; set; }
        public Xamarin.Forms.Point Location { get; set; }
        public bool IsInContact { get; set; }
        public SKPoint SKPoint
        {
            get
            {
                return new SKPoint(
                    (float)(Location.X * (QC.MapCanvasViewWidth / QC.ScreenWidth)),
                    (float)(Location.Y * (QC.MapCanvasViewHeight / QC.ScreenHeight)));
            }
        }
        public Coordinate Coordinate
        {
            get
            {
                var m = _game.GameBoardVM.MapVM.MapMatrix;
                double bitmapX = (Location.X - m.TransX) / m.ScaleX;
                double bitmapY = (Location.Y - m.TransY) / m.ScaleY;
                int row = (int)Math.Floor(bitmapY / QC.SqSize);
                int col = (int)Math.Floor(bitmapX / QC.SqSize);
                return new Coordinate(row, col);
            }
        }
    }
}
