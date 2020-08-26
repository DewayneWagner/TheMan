using SkiaSharp;
using System.Collections.Generic;
using TheManXS.Model.Main;
using TheManXS.Model.Map.Surface;
using TheManXS.ViewModel.MapBoardVM.SKGraphics;
using IT = TheManXS.Model.ParametersForGame.InfrastructureType;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.Infrastructure
{
    class SideRoad
    {
        Game _game;
        SQ _fromSQ;
        SQ _toSQ;
        System.Random rnd = new System.Random();
        List<SQ> _listOfSQsOnRoute;

        public SideRoad(Game game, SQ fromSQ)
        {
            _game = game;
            _fromSQ = fromSQ;
            _toSQ = GetToSQ();
            _listOfSQsOnRoute = GetListOfSQsOnRoute();
        }
        private SQ GetToSQ()
        {
            int tRow, tCol;
            SQ adjacentSQ;

            for (int x = 1; x < QC.ColQ; x++)
            {
                for (int xx = x; xx >= -x; xx--)
                {
                    for (int xxx = x; xxx >= -x; xxx--)
                    {
                        tRow = _fromSQ.Row + xx;
                        tCol = _fromSQ.Col + xxx;

                        if (Coordinate.DoesSquareExist(tRow, tCol))
                        {
                            adjacentSQ = _game.SQList[tRow, tCol];
                            if (adjacentSQ.IsHub || adjacentSQ.IsRoadConnected) { return adjacentSQ; }
                        }
                    }
                }
            }
            return _fromSQ;
        }
        private List<SQ> GetListOfSQsOnRoute()
        {
            List<SQ> sqRoute = new List<SQ>();
            int rDirection = GetDirection(_toSQ.Row - _fromSQ.Row);
            int cDirection = GetDirection(_toSQ.Col - _fromSQ.Col);

            int tCol = _fromSQ.Col;
            int tRow = _fromSQ.Row;
            SQ sq;

            do
            {
                if (tRow != _toSQ.Row) { tRow += rDirection; }
                // this section is intended to make the path curve during long straight sections
                //{
                //    if (Math.Abs(tRow - _toSQ.Row) >= 3) { tRow += GetAdjustment(); }
                //    else { tRow += rDirection; }
                //}

                if (tCol != _toSQ.Col) { tCol += cDirection; }
                //{
                //    if (Math.Abs(tCol - _toSQ.Col) >= 2) { tCol += GetAdjustment(); }
                //    else { tCol += cDirection; }
                //}

                if (Coordinate.DoesSquareExist(tRow, tCol))
                {
                    sq = _game.SQList[tRow, tCol];
                    sqRoute.Add(sq);
                }
            } while (tRow != _toSQ.Row || tCol != _toSQ.Col);

            setIsRoadPropertyForEachSQInSQDictionary();

            return sqRoute;

            void setIsRoadPropertyForEachSQInSQDictionary() { foreach (SQ square in sqRoute) { square.IsRoadConnected = true; } }

            int GetDirection(int diff)
            {
                if (diff < 0)
                    return -1;
                else if (diff == 0)
                    return 0;
                else
                    return 1;
            }
        }
        public SKPath SKPath
        {
            get
            {
                SKPath path = new SKPath();
                PathCalculations pathCal = new PathCalculations();
                SKPoint startPoint = pathCal.GetInfrastructureSKPoint(_fromSQ, IT.Road);
                path.MoveTo(startPoint);

                foreach (SQ sq in _listOfSQsOnRoute)
                {
                    SKPoint nextPoint = pathCal.GetInfrastructureSKPoint(sq, IT.Road);
                    path.LineTo(nextPoint);
                }

                return path;
            }
        }
        public void DrawSideRoad()
        {
            using (SKCanvas canvas = new SKCanvas(_game.GameBoardVM.MapVM.SKBitMapOfMap))
            {
                SKPaint roadPaint = new PaintTypes()[(int)IT.Road];
                canvas.DrawPath(SKPath, roadPaint);
                canvas.Save();
            }
        }
    }
}
