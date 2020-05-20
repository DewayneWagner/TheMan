using SkiaSharp;
using System.Linq;
using TheManXS.Model.Main;
using TheManXS.ViewModel.MapBoardVM.TouchTracking;

namespace TheManXS.ViewModel.MapBoardVM.TouchExecution
{
    public class ExecuteTwoFingerPan
    {
        Game _game;
        public ExecuteTwoFingerPan(Game game)
        {
            _game = game;
            ExecuteTwoFingerPanAction();
        }

        private void ExecuteTwoFingerPanAction()
        {
            var m = _game.GameBoardVM.MapVM;
            SKPoint pressed = m.MapTouchList[0].FirstOrDefault(p => p.Type == TouchActionType.Pressed).SKPoint;
            SKPoint released = m.MapTouchList[0].FirstOrDefault(p => p.Type == TouchActionType.Released).SKPoint;

            float xDelta = released.X - pressed.X;
            float yDelta = released.Y - pressed.Y;

            if (FloatIsValid(xDelta) && FloatIsValid(yDelta))
            {
                SKMatrix panMatrix = SKMatrix.MakeTranslation(xDelta, yDelta);
                SKMatrix.PostConcat(ref m.MapMatrix, panMatrix);
                m.MapCanvasView.InvalidateSurface();
            }
        }
        private bool FloatIsValid(float f) => !float.IsNaN(f) && !float.IsInfinity(f);
    }
}
