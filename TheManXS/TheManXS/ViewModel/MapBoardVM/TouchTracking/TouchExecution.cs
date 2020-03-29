using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using System.Linq;
using SkiaSharp;
using TheManXS.ViewModel.Services;
using TheManXS.Model.Map.Surface;
using TheManXS.ViewModel.MapBoardVM.MainElements;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.TouchTracking
{
    public class TouchExecution
    {        
        private PageService _pageService;
        private MapVM _mapVM;

        public TouchExecution(MapVM mapVM)
        {
            _pageService = new PageService();
            _mapVM = mapVM;

            switch (mapVM.MapTouchList.MapTouchType)
            {
                case MapTouchType.OneFingerSelect:
                    ExecuteOneFingerSelect();
                    break;
                case MapTouchType.OneFingerDragSelect:
                    ExecuteOneFingerDragSelect();
                    break;
                case MapTouchType.TwoFingerPan:
                    ExecuteTwoFingerPan();
                    break;
                case MapTouchType.Pinch:
                    ExecutePinch();
                    break;
                default:
                    break;
            }
            _mapVM.MapTouchList = new MapTouchListOfMapTouchIDLists();
        }
        private async void ExecuteOneFingerSelect() 
        {
            //Coordinate activeSQ = new Coordinate();
            ////Coordinate activeSQ = new Coordinate(GetOneFingerTouchPoint());
            //await _pageService.DisplayAlert("Touch" + "\n" +
            //    "Row: " + Convert.ToString(activeSQ.Row) + "\n" +
            //    "Col: " + Convert.ToString(activeSQ.Col)); 
        }
        private async void ExecuteOneFingerDragSelect() 
        {
            Dictionary<int, Coordinate> touchedSQsInDragEvent = GetDictionaryOfTouchedSQs();
            string message = null;
            foreach (KeyValuePair<int,Coordinate> coordinate in touchedSQsInDragEvent) 
                { message += Convert.ToString(coordinate.Key) + "\n";}
            await _pageService.DisplayAlert(message);
        }
        private Dictionary<int, Coordinate> GetDictionaryOfTouchedSQs()
        {
            Dictionary<int, Coordinate> touchedSQs = new Dictionary<int, Coordinate>();
            foreach (TouchActionEventArgs args in _mapVM.MapTouchList[0])
            {
                SKPoint touchPointOnMap = GetTouchPointOnBitMap(args.SKPoint);
                Coordinate touchCoord = new Coordinate(touchPointOnMap);
                if (!touchedSQs.ContainsKey(touchCoord.SQKey)) { touchedSQs.Add(touchCoord.SQKey, touchCoord); }
            }
            return touchedSQs;
        }
        private void ExecuteTwoFingerPan()
        {
            SKPoint pressed = _mapVM.MapTouchList[0].FirstOrDefault(p => p.Type == TouchActionType.Pressed).SKPoint;
            SKPoint released = _mapVM.MapTouchList[0].FirstOrDefault(p => p.Type == TouchActionType.Released).SKPoint;

            float xDelta = released.X - pressed.X;
            float yDelta = released.Y - pressed.Y;

            if(FloatIsValid(xDelta) && FloatIsValid(yDelta))
            {
                SKMatrix panMatrix = SKMatrix.MakeTranslation(xDelta, yDelta);
                SKMatrix.PostConcat(ref _mapVM.MapMatrix, panMatrix);
                _mapVM.MapCanvasView.InvalidateSurface();
            }
        }
        private void ExecutePinch()
        {
            SKPoint pivot = _mapVM.MapTouchList[0].FirstOrDefault(p => p.Type == TouchActionType.Pressed).SKPoint;
            SKPoint prevPoint = _mapVM.MapTouchList[1].FirstOrDefault(p => p.Type == TouchActionType.Pressed).SKPoint;
            SKPoint newPoint = _mapVM.MapTouchList[1].FirstOrDefault(p => p.Type == TouchActionType.Released).SKPoint;

            SKPoint oldVector = prevPoint - pivot;
            SKPoint newVector = newPoint - pivot;

            float scaleX = newVector.X / oldVector.X;
            float scaleY = newVector.Y / oldVector.Y;

            float pinchScale = (scaleX > scaleY) ? scaleX : scaleY;

            if(FloatIsValid(pinchScale))
            {
                SKMatrix scaleMatrix = SKMatrix.MakeScale(pinchScale, pinchScale, pivot.X, pivot.Y);
                SKMatrix.PostConcat(ref _mapVM.MapMatrix, scaleMatrix);
                _mapVM.MapCanvasView.InvalidateSurface();
            }
        }
        private SKPoint GetOneFingerTouchPoint()
        {
            SKPoint touchedPointOnScreen = _mapVM.MapTouchList[0].FirstOrDefault(p => p.Type == TouchActionType.Pressed).SKPoint;
            return GetTouchPointOnBitMap(touchedPointOnScreen);
        }
        private SKPoint GetTouchPointOnBitMap(SKPoint skp)
        {


            float ptx = (float)(skp.X * (QC.MapCanvasViewWidth / QC.ScreenWidth));
            float pty = (float)(skp.Y * (QC.MapCanvasViewHeight / QC.ScreenHeight));
            return new SKPoint(ptx, pty);

            //float bitmapX = ((skp.X - _mapVM.MapMatrix.TransX) / _mapVM.MapMatrix.ScaleX);
            //float bitmapY = ((skp.Y - _mapVM.MapMatrix.TransY) / _mapVM.MapMatrix.ScaleY);
            //return new SKPoint(bitmapX, bitmapY);
        }
        //return new SKPoint(
        //        (float)(pt.X* (QC.ScreenWidth / QC.MapCanvasViewWidth)),
        //        (float) (pt.Y* (QC.ScreenHeight / QC.MapCanvasViewWidth)));
        private bool FloatIsValid(float f) => !float.IsNaN(f) && !float.IsInfinity(f);
    }
}
