using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xamarin.Forms;
using QC = TheManXS.Model.Settings.QuickConstants;
using SkiaSharp;
using TheManXS.Model.Map.Surface;

namespace TheManXS.ViewModel.MapBoardVM.TouchTracking
{
    public class MapTouchID : List<TouchActionEventArgs>
    {
        public long ID { get; set; }
        public bool TouchEventHasExited { get; set; }    
    }
    public class MapTouchListOfMapTouchIDLists : List<MapTouchID>
    {
        public MapTouchListOfMapTouchIDLists() {}
        public MapTouchType MapTouchType { get; set; }
        public bool AllTouchEffectsExited { get; set; }
        public bool NoExecutionRequired { get; set; }        
        public void AddTouchAction(TouchActionEventArgs args)
        {
            bool listExistsForThisID = this.Any(l => l.ID == args.Id);
            int index = listExistsForThisID ? this.IndexOf(this.FirstOrDefault(l => l.ID == args.Id)) : 0;

            if(args.Type != TouchActionType.Entered)
            {
                if (!listExistsForThisID && args.IsInContact)
                {
                    Add(new MapTouchID() { ID = args.Id });
                    this[Count - 1].Add(args);
                }
                else if (args.Type == TouchActionType.Exited && Count != 0)
                {
                    this[index].TouchEventHasExited = true;
                    if (CheckIfThisIDIsMouseMovementOnly(index)) { NoExecutionRequired = true; }
                    else { if (CheckIfAllTouchIDsExited()) { SetTouchType(); } }
                }
                else if (args.Type == TouchActionType.Released || args.IsInContact) { this[index].Add(args); }
            }    
        }
        private void SetTouchType()
        {
            this.AllTouchEffectsExited = true;
            if (IsSingleFinger())
            {
                if(IsTap()) { MapTouchType = MapTouchType.OneFingerSelect; }
                else { MapTouchType = MapTouchType.OneFingerDragSelect; }
            }
            else { SetTwoFingerTouchType(); }
        }
        private bool IsSingleFinger() => Count == 1 ? true : false;
        private bool CheckIfAllTouchIDsExited()
        {
            foreach (MapTouchID m in this) { if(!m.TouchEventHasExited) { return false; }}
            return true;
        }
        private bool CheckIfThisIDIsMouseMovementOnly(int index)
        {
            foreach (TouchActionEventArgs mt in this[index]) { if(mt.IsInContact) { return false; }}
            return true;
        }
        private void SetTwoFingerTouchType()
        {
            int pressed = 0;
            int released = 1;
            int finger1 = 0;
            int finger2 = 1;

            Point[,] points = getPointArray();  
            if(!DetermineIfPan()) { MapTouchType = MapTouchType.Pinch; }

            Point[,] getPointArray()
            {
                Point[,] pointArray = new Point[2, 2];
                for (int i = 0; i < 2; i++)
                {
                    pointArray[i,pressed] = this[i].FirstOrDefault(l => l.Type == TouchActionType.Pressed).Location;
                    pointArray[i,released] = this[i].FirstOrDefault(l => l.Type == TouchActionType.Released).Location;
                }
                return pointArray;
            }
            bool DetermineIfPan()
            {
                Direction firstFinger = GetDirection(points[finger1, pressed], points[finger1, released]);
                Direction secondFinger = GetDirection(points[finger2, pressed], points[finger2, released]);

                if(firstFinger == secondFinger) 
                {
                    MapTouchType = MapTouchType.TwoFingerPan;
                    return true; 
                }
                else { return false; }
            }
        }
        private bool IsTap()
        {
            Coordinate pressed = new Coordinate(this[0].FirstOrDefault(p => p.Type == TouchActionType.Pressed).Location);
            Coordinate released = new Coordinate(this[0].FirstOrDefault(r => r.Type == TouchActionType.Released).Location);                       
            return pressed.SQKey == released.SQKey ? true : false;
        }
        public Direction GetDirection(Point pt1, Point pt2)
        {
            double xDelta = pt2.X - pt1.X;
            double yDelta = pt2.Y - pt1.Y;

            if (Math.Abs(xDelta) > Math.Abs(yDelta)) { return (xDelta >= 0) ? Direction.East : Direction.West; }
            else { return (yDelta >= 0) ? Direction.South : Direction.North; }
        }
    }
}
