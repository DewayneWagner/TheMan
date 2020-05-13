using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using TheManXS.Model.Main;
using TheManXS.Model.Services.EntityFrameWork;
using TheManXS.ViewModel.MapBoardVM.MainElements;
using IT = TheManXS.Model.ParametersForGame.InfrastructureType;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.Infrastructure
{
    public class NewMapInitializer
    {
        public enum DirectionsCompass { NW, N, NE, E, SE, S, SW, W, Total }

        MapVM _mapVM;
        PathCalculations _calc;

        List<SQ>[] _allInfrastructure = new List<SQ>[(int)IT.Total];
        List<SKPath> _listOfAllSKPaths = new List<SKPath>();
        List<IT> _infrastructureType = new List<IT>();
        List<int> _infrastructureTypesInSKPaths = new List<int>();
        PaintTypes _paintTypes = new PaintTypes();

        public NewMapInitializer(MapVM mapVM)
        {
            _mapVM = mapVM;
            _calc = new PathCalculations();
            InitListOfInfrastructureSQs();
            InitInfrastructure();
            DrawAllPathsOnCanvas();
            InitHubs();
        }
        private void InitListOfInfrastructureSQs()
        {
            using (DBContext db = new DBContext())
            {
                var allSQInfrastructure = db.SQ
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
                IT it = (IT)i;
                List<SQ> sortedList = getSortedList(_allInfrastructure[i]);

                if(it == IT.Tributary) { initTributaries(); }
                else if(it != IT.Hub) { AddAllInfrastructure(it, sortedList); }

                void initTributaries()
                {
                    List<SQ> tributaryList = _allInfrastructure[(int)IT.Tributary];
                    int qTributaries = tributaryList.Max(s => s.TributaryNumber);
                    for (int t = 0; t <= qTributaries; t++)
                    {
                        List<SQ> tributary = tributaryList.Where(tr => tr.TributaryNumber == t).ToList();
                        List<SQ> sortedListOfTributaries = getSortedList(tributary);
                        connectTributaryToMainRiver(ref sortedListOfTributaries);
                        AddAllInfrastructure(IT.Tributary, sortedListOfTributaries);
                    }
                }

                void connectTributaryToMainRiver(ref List<SQ> tributaryList)
                {
                    // the ends of list are either Main River or map edge
                    int indexOfMainRiverAdjacentSQOnTributary = getIndexOfMainRiverAdjacentSQOnTributary(tributaryList);
                    SQ mainRiverAdjacentSQOnTributary = tributaryList[indexOfMainRiverAdjacentSQOnTributary];
                    SQ mainRiverJoinPoint = getMainRiverJoinPoint();

                    if(indexOfMainRiverAdjacentSQOnTributary == 0) { tributaryList.Insert(0, mainRiverJoinPoint); }
                    else { tributaryList.Add(mainRiverJoinPoint); }

                    SQ getMainRiverJoinPoint()
                    {
                        using (DBContext db = new DBContext())
                        {
                            int row;
                            int col;

                            for (int rowChange = -1; rowChange < 2; rowChange++)
                            {
                                for (int colChange = -1; colChange < 2; colChange++)
                                {
                                    row = mainRiverAdjacentSQOnTributary.Row + rowChange;
                                    col = mainRiverAdjacentSQOnTributary.Col + colChange;

                                    if (TheManXS.Model.Map.Surface.Coordinate.DoesSquareExist(row, col))
                                    {
                                        SQ adjSQ = db.SQ.Where(s => s.Row == row)
                                            .Where(s => s.Col == col)
                                            .FirstOrDefault();

                                        if (adjSQ.IsMainRiver) { return adjSQ; }
                                    }
                                }
                            }
                            return mainRiverAdjacentSQOnTributary;
                        }                        
                    }
                    bool isMapEdge(SQ sq)
                    {
                        if (sq.Row == 0 || sq.Row == (QC.RowQ - 1) || sq.Col == 0 || sq.Col == (QC.ColQ - 1)) { return true; }
                        else { return false; }
                    }
                    int getIndexOfMainRiverAdjacentSQOnTributary(List<SQ> list)
                    {
                        if(!isMapEdge(list[0])) { return 0; }
                        else { return (list.Count - 1); }
                    }
                }
                List<SQ> getSortedList(List<SQ> unsortedList)
                {
                    List<SQ> sortedListM = new List<SQ>();
                    if (IsPathHorizontallyOriented(unsortedList))
                    {
                        sortedListM = unsortedList.OrderBy(s => s.Row)
                                        .ThenBy(s => s.Col)
                                        .ToList();
                    }
                    else
                    {
                        sortedListM = unsortedList.OrderBy(s => s.Col)
                                        .ThenBy(s => s.Row)
                                        .ToList();
                    }
                    return sortedListM;
                }
            }
        }

        private void AddAllInfrastructure(IT it, List<SQ> sortedList)
        {
            PathSegmentList pathSegmentList = new PathSegmentList(sortedList, it);
            SKPath path = new SKPath();
            _infrastructureType.Add(it);

            for (int i = 0; i < (pathSegmentList.Count -1); i++)
            {
                PathSegment p = pathSegmentList[i];

                switch (p.SegmentType)
                {
                    case SegmentType.EdgePointStart:
                        path.MoveTo(p.SKPoint);
                        break;
                         
                    case SegmentType.Curve:
                    case SegmentType.Straight:
                        path.LineTo(p.SKPoint);
                        //path.CubicTo(path.LastPoint, pathSegmentList[i].SKPoint, pathSegmentList[i + 1].SKPoint);
                        //i++;
                        break;
                         
                    case SegmentType.EdgePointEnd:
                        path.LineTo(p.SKPoint);
                        break;
                    default:
                        break;
                }
            }
            AddCompletedPathToList(path, it);
        }
        void AddCompletedPathToList(SKPath path, IT it)
        {
            _listOfAllSKPaths.Add(path);
            _infrastructureTypesInSKPaths.Add((int)it);
        }

        private static int[] R = new int[(int)AdjSqsDirection.Total] { 0, 1, 1, 1 };
        private static int[] C = new int[(int)AdjSqsDirection.Total] { 1, 1, 0, -1 };
        private enum AdjSqsDirection { E, SE, S, SW, Total }

        private void DrawAllPathsOnCanvas()
        {            
            using (SKCanvas gameBoard = new SKCanvas(_mapVM.SKBitMapOfMap))
            {
                for (int i = 0; i < _listOfAllSKPaths.Count; i++)
                {
                    if(_infrastructureType[i] == IT.MainRiver || _infrastructureType[i] == IT.Tributary)
                    {
                        SKPaint sandPaint = _paintTypes.GetSandPaint(_infrastructureType[i]);
                        gameBoard.DrawPath(_listOfAllSKPaths[i], sandPaint);
                    }

                    SKPaint paint = _paintTypes[_infrastructureTypesInSKPaths[i]];
                    gameBoard.DrawPath(_listOfAllSKPaths[i], paint);                    
                }
                gameBoard.Save();
            }
        }
        private void InitHubs()
        {
            using (SKCanvas gameboard = new SKCanvas(_mapVM.SKBitMapOfMap))
            {
                foreach (SQ sq in _allInfrastructure[(int)IT.Hub])
                {
                    gameboard.DrawRect(_calc.GetHubRect(sq), _paintTypes[(int)IT.Hub]);
                }
                gameboard.Save();
            }
        }
        private bool IsPathHorizontallyOriented(List<SQ> infrastructureList)
        {
            int rowChange = infrastructureList.Max(s => s.Row) - infrastructureList.Min(s => s.Row);
            int colChange = infrastructureList.Max(s => s.Col) - infrastructureList.Min(s => s.Col);
            return (colChange > rowChange ? false : true);
        }
    }
}
