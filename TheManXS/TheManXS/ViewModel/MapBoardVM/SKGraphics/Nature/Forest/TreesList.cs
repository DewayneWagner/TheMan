using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Nature.Forest
{
    class TreesList
    {
        ForestConstants _fc = new ForestConstants();

        public TreesList(List<SQ> listOfAllForestSQs, SKBitmap gameBoard)
        {

        }

        private SKPaint _strokePaint;
        public SKPaint StrokePaint
        {
            get => _strokePaint;
            set
            {
                _strokePaint = new SKPaint()
                {
                    Style = SKPaintStyle.Stroke,
                    IsAntialias = true,
                    Color = SKColors.Black,
                    StrokeWidth = _fc.TreeStrokeWidth,
                };
            }
        }
    }
}
