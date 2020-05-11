using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Map.Surface;
using QC = TheManXS.Model.Settings.QuickConstants;
using IT = TheManXS.Model.ParametersForGame.InfrastructureType;

namespace TheManXS.ViewModel.MapBoardVM.Infrastructure
{
    class SideRoad
    {
        Game _game;
        SQ _fromSQ;
        SQ _toSQ;
        System.Random rnd = new System.Random();

        public SideRoad(Game game, SQ fromSQ)
        {
            _game = game;
            _fromSQ = fromSQ;
        }
        private SQ ToSQ
        {
            get
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
                                adjacentSQ = _game.SquareDictionary[Coordinate.GetSQKey(tRow, tCol)];
                                if (adjacentSQ.IsHub || adjacentSQ.IsRoadConnected) { return adjacentSQ; }
                            }
                        }
                    }
                }
                return _fromSQ;
            }
        }
        private List<SQ> ListOfSQsOnRoute
        {
            get
            {
                List<SQ> sqRoute = new List<SQ>();
                int rDirection = GetDirection(ToSQ.Row - _fromSQ.Row);
                int cDirection = GetDirection(ToSQ.Col - _fromSQ.Col);

                int tCol = _fromSQ.Col;
                int tRow = _fromSQ.Row;
                SQ sq;

                do
                {
                    if(tRow != ToSQ.Row)
                    {
                        if(tRow != ToSQ.Row)
                        {
                            if(Math.Abs(tRow - ToSQ.Row) >= 3) { tRow += GetAdjustment(); }
                            else { tRow += rDirection; }
                        }

                        if (tCol != ToSQ.Col)
                        {
                            if(Math.Abs(tCol - ToSQ.Col) >= 2) { tCol += GetAdjustment(); }
                            else { tCol += cDirection; }
                        }

                        if (Coordinate.DoesSquareExist(tRow,tCol))
                        {
                            sq = _game.SquareDictionary[Coordinate.GetSQKey(tRow, tCol)];
                            sqRoute.Add(sq);
                        }
                    }
                } while (tRow != ToSQ.Row || tCol != ToSQ.Col);

                return sqRoute;

                int GetDirection(int diff)
                {
                    if (diff < 0)
                        return -1;
                    else if (diff == 0)
                        return 0;
                    else
                        return 1;
                }
                int GetAdjustment() => rnd.Next(-1, 2);
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

                foreach (SQ sq in ListOfSQsOnRoute)
                {
                    SKPoint nextPoint = pathCal.GetInfrastructureSKPoint(sq, IT.Road);
                    path.LineTo(nextPoint);
                }

                return path;
            }
        }
    }
}
