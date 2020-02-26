﻿using SkiaSharp;
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
        Game _game;
        PanelType _panelType;
        List<SQ> _listOfSqsToHighlight;

        SKPaint highlightedSQ = new SKPaint()
        {
            Color = SKColors.Yellow,
            Style = SKPaintStyle.Fill,
        };

        public SelectedSQHighlight(Game game, PanelType pt)
        {
            _game = game;
            _panelType = pt;
            InitListOfSQsToHighlight();
            HighlightSQForSelection();
        }
        void InitListOfSQsToHighlight()
        {
            _listOfSqsToHighlight = new List<SQ>();
            if(_panelType == PanelType.SQ) { _listOfSqsToHighlight.Add(_game.GameBoardVM.MapVM.ActiveSQ); }
            else { foreach(SQ sq in _game.GameBoardVM.MapVM.ActiveUnit) { _listOfSqsToHighlight.Add(sq); } }
        }

        void HighlightSQForSelection()
        {
            using (SKCanvas canvas = new SKCanvas(_game.GameBoardVM.MapVM.SKBitMapOfMap))
            {
                foreach (SQ sq in _listOfSqsToHighlight)
                {
                    canvas.DrawRect(GetSKRect(sq), highlightedSQ);
                }
            }

            SKRect GetSKRect(SQ sq) => new SKRect(sq.Col * QC.SqSize, sq.Row * QC.SqSize, (sq.Col + 1) * QC.SqSize, 
                (sq.Row + 1) * QC.SqSize);
        }
        public void PermanentlyHighlightSQWithCompanyColors()
        {
            if (_panelType == PanelType.SQ)
            {

            }
        }
        public void RemoveSelectionHighlight()
        {
            if (_panelType == PanelType.SQ)
            {

            }
        }
        
    }
}
