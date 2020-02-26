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
        public ExecutePinch(Game game)
        {
            _game = game;
            ExecutePinchAction();
        }
        private void ExecutePinchAction()
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
