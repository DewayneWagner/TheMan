using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.ViewModel.MapBoardVM.MainElements;
using TheManXS.ViewModel.MapBoardVM.TouchTracking;

namespace TheManXS.ViewModel.MapBoardVM.TouchExecution
{
    public class ExecutePinch
    {
        Game _game;

        float _pinchScale = 1.75f;
        float _spreadScale = 0.5f;

        public ExecutePinch(Game game)
        {
            _game = game;
            ExecutePinchAction();
        }
        private void ExecutePinchAction()
        {
            var m = _game.GameBoardVM.MapVM.MapTouchList;

            SKPoint finger1TouchPoint = m[0][0].SKPoint;
            SKPoint finger1ReleasePoint = m[0][m[0].Count - 1].SKPoint;

            SKPoint finger2TouchPoint = m[1][0].SKPoint;
            SKPoint finger2ReleasePoint = m[1][m[1].Count - 1].SKPoint;

            float pivot_X = finger1TouchPoint.X;
            float pivot_Y = Math.Abs(finger1TouchPoint.Y - finger2TouchPoint.Y);

            float pinchScale = getPinchScale();

            if (FloatIsValid(pinchScale))
            {
                SKMatrix scaleMatrix = SKMatrix.MakeScale(pinchScale, pinchScale, pivot_X, pivot_Y);
                SKMatrix.PostConcat(ref _game.GameBoardVM.MapVM.MapMatrix, scaleMatrix);
                _game.GameBoardVM.MapVM.MapCanvasView.InvalidateSurface();
            }

            float getPinchScale()
            {
                float yDiffPress = Math.Abs(finger1TouchPoint.Y - finger2TouchPoint.Y);
                float yDiffRelease = Math.Abs(finger1ReleasePoint.Y - finger2ReleasePoint.Y);

                if(yDiffPress < yDiffRelease) { return _pinchScale; }
                else { return _spreadScale; }
            }
        }
        private void ExecutePinchAction(bool isOldCrappyVersionThatKindaWorked)
        {
            var m = _game.GameBoardVM.MapVM;
            SKPoint pivot = m.MapTouchList[0].FirstOrDefault(p => p.Type == TouchActionType.Pressed).SKPoint;
            SKPoint prevPoint = m.MapTouchList[1].FirstOrDefault(p => p.Type == TouchActionType.Pressed).SKPoint;
            SKPoint newPoint = m.MapTouchList[1].FirstOrDefault(p => p.Type == TouchActionType.Released).SKPoint;

            SKPoint oldVector = prevPoint - pivot;
            SKPoint newVector = newPoint - pivot;

            float scaleX = newVector.X / oldVector.X;
            float scaleY = newVector.Y / oldVector.Y;
            
            float pinchScale = (scaleX > scaleY) ? scaleX : scaleY;

            if (FloatIsValid(pinchScale))
            {
                SKMatrix scaleMatrix = SKMatrix.MakeScale(pinchScale, pinchScale, pivot.X, pivot.Y);
                SKMatrix.PostConcat(ref m.MapMatrix, scaleMatrix);
                m.MapCanvasView.InvalidateSurface();
            }
        }
        private bool FloatIsValid(float f) => !float.IsNaN(f) && !float.IsInfinity(f);
    }
}
