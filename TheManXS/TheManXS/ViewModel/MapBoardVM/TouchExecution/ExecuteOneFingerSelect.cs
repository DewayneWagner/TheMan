using SkiaSharp;
using System.Linq;
using TheManXS.Model.Main;
using TheManXS.Model.Map.Surface;
using TheManXS.ViewModel.MapBoardVM.Action;
using TheManXS.ViewModel.MapBoardVM.TouchTracking;

namespace TheManXS.ViewModel.MapBoardVM.TouchExecution
{
    public class ExecuteOneFingerSelect
    {
        Game _game;
        SKPaint highlightedSq = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.Red,
        };

        public ExecuteOneFingerSelect(Game game)
        {
            _game = game;
            ExecuteOneFingerSelectAction();
            _game.GameBoardVM.SidePanelManager.AddSidePanel(ActionPanelGrid.PanelType.SQ);
        }

        public SelectedSQHighlight SelectedSQHighlight { get; set; }

        private void ExecuteOneFingerSelectAction()
        {
            var m = _game.GameBoardVM.MapVM;
            Coordinate touchPoint = getTouchPointCoordinate();
            _game.ActiveSQ = _game.SQList[touchPoint.Row, touchPoint.Col];

            Coordinate getTouchPointCoordinate() => m.MapTouchList[0]
                                                        .Where(p => p.Type == TouchActionType.Pressed)
                                                        .Select(p => p.Coordinate)
                                                        .FirstOrDefault();
        }
    }
}
