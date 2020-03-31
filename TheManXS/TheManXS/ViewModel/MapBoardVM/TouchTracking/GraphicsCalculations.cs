using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using QC = TheManXS.Model.Settings.QuickConstants;
using TheManXS.Model.Main;

namespace TheManXS.ViewModel.MapBoardVM.TouchTracking
{
    public class GraphicsCalculations
    {
        public static double GetDistanceBetweenPoints(Point pt1, Point pt2) =>
            Math.Sqrt(Math.Pow((Math.Abs(pt1.X - pt2.X)), 2) + Math.Pow((Math.Abs(pt1.Y - pt2.Y)), 2));
        
        public static double GetDistanceBetweenPoints(SKPoint pt1, SKPoint pt2) =>
            Math.Sqrt(Math.Pow((Math.Abs(pt1.X - pt2.X)), 2) + Math.Pow((Math.Abs(pt1.Y - pt2.Y)), 2));

        public static SKPoint GetSKPointFromXPoint(Point pt)
        {
            Game game = (Game)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.Game)];
            SKPoint p = new SKPoint();
            double x = pt.X;
            double y = pt.Y;
            
            double topLeftCornerOfBitMapOnScreenX = game.GameBoardVM.MapVM.MapCanvasView.X;
            double topRightCornerOfBitMapOnScreenY = game.GameBoardVM.MapVM.MapCanvasView.Y;

            double widthOfVisibleMapCanvasView = game.GameBoardVM.MapVM.MapCanvasView.Width;
            double heightOfVisibleMapCanvasView = game.GameBoardVM.MapVM.MapCanvasView.Height;



            return p;
        }

        //public static SKPoint GetSKPointFromXPoint(Point pt) => new SKPoint(
        //    (float)(pt.X * (QC.MapCanvasViewWidth / QC.ScreenWidth)),
        //    (float)(pt.Y * (QC.MapCanvasViewHeight / QC.ScreenHeight)));            

        //public static SKPoint GetSKPointFromXPoint(Point pt) =>
        //    new SKPoint((float))

        //public static SKPoint GetSKPointFromXPoint(Point pt) => 
        //    new SKPoint((float)(pt.X * (QC.ScreenWidth / QC.MapCanvasViewWidth)),
        //        (float) (pt.Y * (QC.ScreenHeight / QC.MapCanvasViewHeight)));

        public static Direction GetDirection(Point pt1, Point pt2)
        {
            double xDelta = pt2.X - pt1.X;
            double yDelta = pt2.Y - pt1.Y;

            if (Math.Abs(xDelta) > Math.Abs(yDelta)) { return (xDelta >= 0) ? Direction.East : Direction.West; }
            else { return (yDelta >= 0) ? Direction.South : Direction.North; }
        }


    }
}
