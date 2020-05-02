using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using static TheManXS.ViewModel.MapBoardVM.Action.ActionPanelGrid;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.TouchExecution
{
    public class SelectedSQHighlight
    {
        Game _game;
        PanelType _panelType;
        List<SQ> _listOfSqsToHighlight;
        private SKCanvas _canvas;

        public SelectedSQHighlight(bool isForSidePanelInitialization) { }

        public SelectedSQHighlight(Game game, PanelType pt)
        {
            _game = game;
            _panelType = pt;
            _canvas = new SKCanvas(_game.GameBoardVM.MapVM.SKBitMapOfMap);
            InitListOfSQsToHighlight();
            HighlightSQForSelection();
        }

        void InitListOfSQsToHighlight()
        {
            _listOfSqsToHighlight = new List<SQ>();
            if(_panelType == PanelType.SQ) { _listOfSqsToHighlight.Add(_game.ActiveSQ); }
            else { foreach(SQ sq in _game.ActiveUnit) { _listOfSqsToHighlight.Add(sq); } }
        }

        void HighlightSQForSelection()
        {
            using (new SKAutoCanvasRestore(_canvas))
            {
                foreach (SQ sq in _listOfSqsToHighlight)
                {
                    SKPaint highlightedSQ = new SKPaint()
                    {
                        Color = _game.ActivePlayer.SKColor.WithAlpha(0x75),
                        Style = SKPaintStyle.Fill,
                    };
                    _canvas.DrawRect(sq.SKRect, highlightedSQ);
                }
            }
            //_canvas.Save();
        }

        public void RemoveSelectionHighlight()
        {
            // this doesn't work.
            _canvas.Restore();
            _canvas.Save();
            _canvas.Dispose();
        }        
    }
}
