using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.InfrastructureStuff;
using TheManXS.Model.Main;
using TheManXS.Model.Services.EntityFrameWork;
using TheManXS.ViewModel.MapBoardVM.MainElements;
using TheManXS.ViewModel.MapBoardVM.MapConstruct;
using IT = TheManXS.Model.ParametersForGame.InfrastructureType;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.Infrastructure
{
    public class NewMapInitializer
    {
        MapVM _mapVM;
        Builder _infrastructureBuilder;
        PathCalculations _calc;

        List<SQInfrastructure>[] _allInfrastructure = new List<SQInfrastructure>[(int)IT.Total];
        List<SKPath> _listOfAllSKPaths = new List<SKPath>((int)IT.Total);
        

        public NewMapInitializer(MapVM mapVM, Builder infrastructureBuilder)
        {
            _mapVM = mapVM;
            _calc = new PathCalculations();
            _infrastructureBuilder = infrastructureBuilder;            

            InitListOfInfrastructureSQs();
            InitInfrastructure();
            DrawAllPathsOnCanvas();
            InitHubs();
        }
        private void InitListOfInfrastructureSQs()
        {
            using (DBContext db = new DBContext())
            {
                var allSQInfrastructure = db.SQInfrastructure
                    .Where(s => s.SavedGameSlot == QC.CurrentSavedGameSlot)
                    .ToList();

                _allInfrastructure[(int)IT.MainRiver] = allSQInfrastructure
                    .Where(s => s.IsMainRiver == true)
                    .ToList();

                _allInfrastructure[(int)IT.Tributary] = allSQInfrastructure
                    .Where(s => s.IsTributary)
                    .ToList();

                _allInfrastructure[(int)IT.Road] = allSQInfrastructure
                    .Where(s => s.IsRoadConnected)
                    .ToList();

                _allInfrastructure[(int)IT.Pipeline] = allSQInfrastructure
                    .Where(s => s.IsPipelineConnected)
                    .ToList();

                _allInfrastructure[(int)IT.RailRoad] = allSQInfrastructure
                    .Where(s => s.IsTrainConnected)
                    .ToList();

                _allInfrastructure[(int)IT.Hub] = allSQInfrastructure
                    .Where(s => s.IsHub)
                    .ToList();
            }
        }
        private void InitInfrastructure()
        {
            for (int i = 0; i < (int)IT.Total; i++)
            {
                var sortedList = _allInfrastructure[i]
                    .OrderBy(s => s.Col)
                    .ToList();

                IT it = (IT)i;

                if(it != IT.Hub && it != IT.Tributary)
                { CreateMainTransporationCorridorAndMainRiver(it, sortedList); }

                //if (it != IT.Hub && it != IT.MainRiver)
                //{ CreateSmallPaths((IT)i, _allInfrastructure[i].OrderBy(s => s.Col).ThenBy(s => s.Row).ToList()); }
            }
        }

        private void CreateMainTransporationCorridorAndMainRiver(IT it, List<SQInfrastructure> sortedList)
        {
            SKPath path = new SKPath();
            PathSegmentList pathSegmentList = new PathSegmentList(sortedList, it);

            for (int i = 0; i < pathSegmentList.Count; i++)
            {
                PathSegment p = pathSegmentList[i];
                switch (p.SegmentType)
                {
                    case SegmentType.EdgePointStart:
                        path.MoveTo(p.SKPoint);
                        break;

                    case SegmentType.Curve:
                        //path.QuadTo(path.LastPoint, p.SKPoint);
                        path.CubicTo(path[path.PointCount - 2], path.LastPoint, p.SKPoint);
                        break;

                    case SegmentType.Straight:
                    case SegmentType.EdgePointEnd:
                        path.LineTo(p.SKPoint);
                        break;
                    default:
                        break;
                }
            }
            path.Close();
            _listOfAllSKPaths.Add(path);
        }
        
        
        //private void oldversion()
        //{
        //    SKPath path = new SKPath();
        //    List<bool> pathSegmentsThatAreCurves = _calc.GetListOfPathSegmentsThatAreCurves(sortedList);
        //    SQInfrastructure sq = new SQInfrastructure();

        //    for(int i = 0; i < sortedList.Count; i++)
        //    {
        //        sq = sortedList[i];
        //        if (_calc.IsMapEdge(sq))
        //        {
        //            SKPoint edgePoint = _calc.GetEdgePoint(sq, it);
        //            path.MoveTo(edgePoint);
        //        }

        //        SKPoint nextPoint = _calc.GetInfrastructureSKPoint(sq, it);

        //        if (pathSegmentsThatAreCurves[i]) { path.ArcTo(path[i], nextPoint, _radiusOfCurves); }
        //        else { path.LineTo(nextPoint); }
        //    }

        //    path.Close();
        //    _listOfAllSKPaths.Add(path);
        //}


        private void CreateMainTransporationCorridorAndMainRiver(IT it, List<SQInfrastructure> sortedList, bool isOldMethod)
        {
            SKPath path = new SKPath();
            SQInfrastructure lastSQ = new SQInfrastructure();
            SKPoint lastPoint = new SKPoint();

            for (int i = 0; i < sortedList.Count; i++)
            {
                SQInfrastructure sq = sortedList[i];
                if (_calc.IsMapEdge(sq))
                {
                    SKPoint edgePoint = _calc.GetEdgePoint(sq, it);
                    path.MoveTo(edgePoint);
                }

                SKPoint nextPoint = _calc.GetInfrastructureSKPoint(sq, it);

                if(IsCurve()) { path.QuadTo(lastPoint, nextPoint); }
                //if (IsCurve()) { path.ArcTo(lastPoint, nextPoint, getRadius()) ; }
                else { path.LineTo(nextPoint); }

                path.LineTo(nextPoint);

                lastSQ = sq;
                lastPoint = nextPoint;
                if (!sq.IsHub) { _allInfrastructure[(int)it].Remove(sq); }
            }

            path.Close();
            _listOfAllSKPaths.Add(path);

            bool IsCurve()
            {
                bool isCurve = false;



                return isCurve;
            }
            float getRadius()
            {
                float radius = 0;



                return radius;
            }
        }

        private static int[] R = new int[(int)AdjSqsDirection.Total] { 0, 1, 1, 1 };
        private static int[] C = new int[(int)AdjSqsDirection.Total] { 1, 1, 0, -1 };
        private enum AdjSqsDirection { E, SE, S, SW, Total }

        private void CreateSmallPaths(IT it, List<SQInfrastructure> doubleSortedList)
        {
            SKPath path;
            if(it == IT.Tributary) 
            { 
                path = new SKPath();
                _listOfAllSKPaths.Add(path);            
            }
            else { path = _listOfAllSKPaths[(int)it]; }

            foreach (SQInfrastructure sq in doubleSortedList)
            {
                if (_calc.IsMapEdge(sq)) { _calc.ProcessMapEdge(sq, ref path, it); }
                path.MoveTo(_calc.GetInfrastructureSKPoint(sq, it));
                for (int i = 0; i < (int)AdjSqsDirection.Total; i++)
                {
                    SQInfrastructure adjSQ = doubleSortedList.Where(s => s.Row == (sq.Row + R[i]))
                        .Where(s => s.Col == sq.Col + C[i])
                        .FirstOrDefault();
                    if(adjSQ != null) { path.LineTo(_calc.GetInfrastructureSKPoint(adjSQ, it)); }
                }
            }
            path.Close();
        }
        private void DrawAllPathsOnCanvas()
        {
            using (SKCanvas gameBoard = new SKCanvas(_mapVM.SKBitMapOfMap))
            {
                for (int i = 0; i < _listOfAllSKPaths.Count; i++)
                {
                    SKPaint paint = _infrastructureBuilder.Formats[i];
                    gameBoard.DrawPath(_listOfAllSKPaths[i], paint);
                    gameBoard.Save();
                }
            }
        }
        private void InitHubs()
        {
            using (SKCanvas gameboard = new SKCanvas(_mapVM.SKBitMapOfMap))
            {
                foreach (SQInfrastructure sq in _allInfrastructure[(int)IT.Hub])
                {
                    gameboard.DrawRect(_calc.GetHubRect(sq), _infrastructureBuilder.Formats[(int)IT.Hub]);
                }
                gameboard.Save();
            }
        }
    }
}
