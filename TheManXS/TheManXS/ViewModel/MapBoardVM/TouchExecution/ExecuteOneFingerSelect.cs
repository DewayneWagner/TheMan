using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Map.Surface;
using TheManXS.Model.Services.EntityFrameWork;
using TheManXS.ViewModel.MapBoardVM.Action;
using TheManXS.ViewModel.MapBoardVM.MainElements;
using TheManXS.ViewModel.MapBoardVM.TouchTracking;
using Xamarin.Forms;

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
            // old version
            //Coordinate touchPoint = new Coordinate(getTouchPointOnScreen());
            //_game.ActiveSQ = _game.SquareDictionary[touchPoint.SQKey];

            // new method
            int sqKey = getTouchPoint2OnScreen();
            _game.ActiveSQ = _game.SQList[sqKey];
            
            SKPoint getTouchPointOnScreen() => m.MapTouchList[0].FirstOrDefault(p => p.Type == TouchActionType.Pressed).SKPoint;
            int getTouchPoint2OnScreen() => m.MapTouchList[0].FirstOrDefault(p => p.Type == TouchActionType.Pressed).SQKey;
        }
    }
}
