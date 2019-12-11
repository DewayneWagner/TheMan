using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public TouchActionEventArgs(long id, TouchActionType type, Point location, bool isInContact)
        {
            Id = id;
            Type = type;
            Location = location;
            IsInContact = isInContact;
            SKPoint = GraphicsCalculations.GetSKPointFromXPoint(location);
        }
        public long Id { get; set; }
        public TouchActionType Type { get; set; }
        public Point Location { get; set; }
        public bool IsInContact { get; set; }
        public SKPoint SKPoint { get; set; }
    }   
}
