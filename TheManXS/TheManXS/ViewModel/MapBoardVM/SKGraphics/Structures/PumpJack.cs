using SkiaSharp;
using TheManXS.Model.Main;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Structures
{
    class PumpJack
    {
        Game _game;
        SQ _sq;

        /* RATIOS *****************************************/
        private float _strokeWidthRatio = 0.01f;
        // walking beam
        private float _widthRatioWalkingBeamRatio = 0.5f;
        private float _heightOfWalkingBeamRatio = 0.1f;
        private float _walkingBeamStartPointFromLeftRatio = 0.3f;

        // main bearing
        private float _mainBearingCenterPointFromLeftRatio = 0.5f;
        private float _mainBearingCenterPointFromTopRatio = 0.45f;
        private float _radiusOfMainBearingRatio = 0.025f;

        // ALegs
        private float _AlegStartPointRatioFromLeft = 0.3f;
        private float _ALegBottomRatioFromTop = 0.8f;
        private float _AlegEndPointRatioFromLeft = 0.7f;
        private float _widthOfALegsRatio = 0.05f;

        // horse head
        private float _horseHeadHeightRatio = 0.35f;
        private float _midPointRatioFromLeft = 0.05f;

        // calculated fields
        float left, top, right, bottom;
        float _topOfMainBearing, _bottomOfMainBearing, _radiusOfMainBearing, _centerX, _centerY, _heightOfWalkingBeam,
            _midPointOfWalkingBeamY, _horseHeadHeight;

        // SK Objects
        SKPath _horseHeadPath;
        SKRect _walkingBeamRect;
        SKPath _ALegsPath;

        // SKPaint Objects
        SKPaint _horseHeadPaintFill = new SKPaint()
        {
            IsAntialias = true,
            Style = SKPaintStyle.StrokeAndFill,
        };
        SKPaint _horseHeadPaintStroke = new SKPaint()
        {
            IsAntialias = true,
            Color = SKColors.Black,
            Style = SKPaintStyle.Stroke,
        };
        SKPaint _ALegPaint = new SKPaint()
        {
            IsAntialias = true,
            Color = SKColors.Black,
            Style = SKPaintStyle.Stroke,
        };

        public PumpJack(Game game, SQ sq)
        {
            _game = game;
            _sq = sq;

            InitFields();
            InitHorseHead();
            InitWalkingBeam();
            InitALegs();
            PaintPumpJack();
        }
        void InitFields()
        {
            left = QC.SqSize * _sq.Col;
            top = QC.SqSize * _sq.Row;
            right = QC.SqSize * (_sq.Col + 1);
            bottom = QC.SqSize * (_sq.Row + 1);

            _radiusOfMainBearing = _radiusOfMainBearingRatio * QC.SqSize;
            _centerX = _mainBearingCenterPointFromLeftRatio * QC.SqSize + left;
            _centerY = _mainBearingCenterPointFromTopRatio * QC.SqSize + top;

            _topOfMainBearing = _centerY - _radiusOfMainBearing;
            _bottomOfMainBearing = _centerY + _radiusOfMainBearing;

            _heightOfWalkingBeam = _heightOfWalkingBeamRatio * QC.SqSize;
            _midPointOfWalkingBeamY = _topOfMainBearing - _heightOfWalkingBeam / 2;

            _ALegPaint.StrokeWidth = _widthOfALegsRatio * QC.SqSize;
            _horseHeadHeight = _horseHeadHeightRatio * QC.SqSize;
            _horseHeadPaintStroke.StrokeWidth = _strokeWidthRatio * QC.SqSize;

            _horseHeadPaintFill.Color = _sq.OwnerNumber == QC.PlayerIndexTheMan ?
                SKColors.White : _game.PlayerList[_sq.OwnerNumber].SKColor;
        }
        void InitHorseHead()
        {
            float topPointX = _walkingBeamStartPointFromLeftRatio * QC.SqSize + left;
            float topPointY = _midPointOfWalkingBeamY - (_horseHeadHeight / 2);
            SKPoint topPoint = new SKPoint(topPointX, topPointY);

            float bottomPointY = _midPointOfWalkingBeamY + (_horseHeadHeight / 2);
            SKPoint bottomPoint = new SKPoint(topPointX, bottomPointY);

            float midPointX = _midPointRatioFromLeft * QC.SqSize + left;
            float midPointY = (bottomPointY - topPointY) / 2 + topPointY;
            SKPoint midPoint = new SKPoint(midPointX, midPointY);

            _horseHeadPath = new SKPath();
            _horseHeadPath.MoveTo(topPoint);
            _horseHeadPath.QuadTo(midPoint, bottomPoint);
            _horseHeadPath.Close();
        }
        void InitWalkingBeam()
        {
            float leftRect = _walkingBeamStartPointFromLeftRatio * QC.SqSize + left;
            float topRect = _topOfMainBearing - _heightOfWalkingBeam;
            float rightRect = leftRect + _widthRatioWalkingBeamRatio * QC.SqSize;

            _walkingBeamRect = new SKRect(leftRect, topRect, rightRect, _topOfMainBearing);
        }
        void InitALegs()
        {
            float firstPointX = _AlegStartPointRatioFromLeft * QC.SqSize + left;
            float firstPointY = _ALegBottomRatioFromTop * QC.SqSize + top;
            SKPoint firstPoint = new SKPoint(firstPointX, firstPointY);

            float secondPointX = _mainBearingCenterPointFromLeftRatio * QC.SqSize + left;
            SKPoint secondPoint = new SKPoint(secondPointX, _bottomOfMainBearing);

            float thirdPointX = _AlegEndPointRatioFromLeft * QC.SqSize + left;
            SKPoint thirdPoint = new SKPoint(thirdPointX, firstPointY);

            _ALegsPath = new SKPath();
            _ALegsPath.MoveTo(firstPoint);
            _ALegsPath.LineTo(secondPoint);
            _ALegsPath.LineTo(thirdPoint);
        }
        void PaintPumpJack()
        {
            using (SKCanvas canvas = new SKCanvas(_game.GameBoardVM.MapVM.SKBitMapOfMap))
            {
                canvas.DrawPath(_horseHeadPath, _horseHeadPaintFill);
                canvas.DrawPath(_horseHeadPath, _horseHeadPaintStroke);

                canvas.DrawRect(_walkingBeamRect, _horseHeadPaintFill);
                canvas.DrawRect(_walkingBeamRect, _horseHeadPaintStroke);

                canvas.DrawCircle(_centerX, _centerY, _radiusOfMainBearing, _horseHeadPaintFill);
                canvas.DrawPath(_ALegsPath, _ALegPaint);
                canvas.Save();
            }
        }
    }
}
