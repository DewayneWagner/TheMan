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
            PathSegmentList pathSegmentList = new PathSegmentList(sortedList, it);
            SKPath path = new SKPath();

            for (int i = 0; i < pathSegmentList.Count; i++)
            {
                PathSegment p = pathSegmentList[i];
                switch (p.SegmentType)
                {
                    case SegmentType.EdgePointStart:
                        path.MoveTo(p.SKPoint);
                        break;

                    case SegmentType.Curve:
                        path.ArcTo(path.LastPoint, p.SKPoint,getRadius(i));
                        //path.ConicTo(path.LastPoint, p.SKPoint,100); // this does nothing
                        //path.LineTo(p.SKPoint);
                        //path.QuadTo(path.LastPoint, p.SKPoint); // this looks the same as LineTo
                        //path.CubicTo(path[path.PointCount - 2], path.LastPoint, p.SKPoint); // this make wonky curves
                        break;

                    case SegmentType.Straight:
                    case SegmentType.EdgePointEnd:
                        path.LineTo(p.SKPoint);
                        break;
                    default:
                        break;
                }
            }
            //path.Close(); // for some reason - this makes the end of first path connect to start of next line?
            _listOfAllSKPaths.Add(path);

            float getRadius(int index)
            {
                return 50;
            }
        }

        private static int[] R = new int[(int)AdjSqsDirection.Total] { 0, 1, 1, 1 };
        private static int[] C = new int[(int)AdjSqsDirection.Total] { 1, 1, 0, -1 };
        private enum AdjSqsDirection { E, SE, S, SW, Total }

        private void CreateSmallPaths(IT it, List<SQInfrastructure> doubleSortedList)
        {
            PathSegmentList ps = new PathSegmentList(doubleSortedList, it);
            SKPath path = new SKPath();

            for (int i = 0; i < ps.Count; i++)
            {
                PathSegment p = ps[i];
                switch (p.SegmentType)
                {
                    case SegmentType.EdgePointStart:
                        path.MoveTo(p.SKPoint);
                        break;

                    case SegmentType.Curve:
                    case SegmentType.Straight:
                    case SegmentType.EdgePointEnd:
                    default:
                        path.LineTo(p.SKPoint);
                        break;
                }
            }
            path.Close();
            _listOfAllSKPaths.Add(path);
        }

        private void CreateSmallPaths(IT it, List<SQInfrastructure> doubleSortedList, bool isOldVersion)
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
                    //_listOfAllSKPaths[i].Close(); //doesn't work
                }
                gameBoard.Save();
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
