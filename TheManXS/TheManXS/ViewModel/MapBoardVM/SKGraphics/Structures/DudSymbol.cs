using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Structures
{
    class DudSymbol
    {
        private const float OutsideBorderFromEdgeRatio = 0.1f;
        private const float EdgeOfXFromEdgeRatio = 0.25f;
        private const float LineWidthRatio = 0.05f;

        private float _outsideBorderFromEdge;
        private float _edgeOfXFromEdge;

        private SKRect _skRect;
        private SKRect _border;
        private SKPath _xPath = new SKPath();

        private SKPaint _strokePaint = new SKPaint()
        {
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Crimson,
            StrokeCap = SKStrokeCap.Square,
        };

        public DudSymbol(Game game, SQ sq)
        {
            _skRect = sq.SKRect;
            initFields();
            InitOutsideRect();
            InitXPath();
            DrawX(game);
        }

        void initFields()
        {
            _outsideBorderFromEdge = _skRect.Width * OutsideBorderFromEdgeRatio;
            _edgeOfXFromEdge = _skRect.Width * EdgeOfXFromEdgeRatio;
            _strokePaint.StrokeWidth = _skRect.Width * LineWidthRatio;
        }

        void InitOutsideRect()
        {
            float left = _skRect.Left + _outsideBorderFromEdge;
            float top = _skRect.Top + _outsideBorderFromEdge;
            float right = _skRect.Right - _outsideBorderFromEdge;
            float bottom = _skRect.Bottom - _outsideBorderFromEdge;

            _border = new SKRect(left, top, right, bottom);
        }
        void InitXPath()
        {
            float left = _skRect.Left + _edgeOfXFromEdge;
            float top = _skRect.Top + _edgeOfXFromEdge;
            float right = _skRect.Right - _edgeOfXFromEdge;
            float bottom = _skRect.Bottom - _edgeOfXFromEdge;

            _xPath.MoveTo(new SKPoint(left, top));
            _xPath.LineTo(new SKPoint(right, bottom));
            _xPath.MoveTo(new SKPoint(right, top));
            _xPath.LineTo(new SKPoint(left, bottom));
        }
        void DrawX(Game game)
        {
            using (SKCanvas canvas = new SKCanvas(game.GameBoardVM.MapVM.SKBitMapOfMap))
            {
                canvas.DrawRect(_border, _strokePaint);
                _strokePaint.StrokeWidth *= 2;
                canvas.DrawPath(_xPath, _strokePaint);
                canvas.Save();
            }
        }
    }
}
