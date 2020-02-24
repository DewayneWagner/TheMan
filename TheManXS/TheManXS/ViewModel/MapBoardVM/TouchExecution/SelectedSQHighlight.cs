using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Units;
using TheManXS.ViewModel.MapBoardVM.MainElements;
using Xamarin.Forms;
using static TheManXS.ViewModel.MapBoardVM.Action.ActionPanelGrid;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.TouchExecution
{
    public class SelectedSQHighlight
    {
        GameBoardVM _gameBoardVM;
        PanelType _panelType;

        SKPaint highlightedSQ = new SKPaint()
        {
            Color = SKColors.Yellow,
            Style = SKPaintStyle.Fill,
        };

        public SelectedSQHighlight(GameBoardVM gameBoardVM, PanelType pt)
        {
            _gameBoardVM = gameBoardVM;
            if (pt == PanelType.SQ) { HighlightSQ(_gameBoardVM.MapVM.ActiveSQ); }
            else { foreach (SQ sq in _gameBoardVM.MapVM.ActiveUnit) { HighlightSQ(sq); }}
        }

        void HighlightSQ(SQ sq)
        {
            using (SKCanvas canvas = new SKCanvas(_gameBoardVM.MapVM.Map))
            {
                canvas.DrawRect(GetSKRect(), highlightedSQ);
            }
            SKRect GetSKRect() => new SKRect(sq.Col * QC.SqSize, sq.Row * QC.SqSize, (sq.Col + 1) * QC.SqSize, 
                (sq.Row + 1) * QC.SqSize);
        }
    }
}
