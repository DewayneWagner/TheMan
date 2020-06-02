using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TheManXS.Model.Main;
using Windows.System;
using IT = TheManXS.Model.ParametersForGame.InfrastructureType;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Infrastructure
{
    class InfMaster
    {
        Game _game;
        private InfSegmentList _infSegmentList;
        private SKPath[] _arrayOfPaths;
        public InfMaster(Game game, SKCanvas canvas, bool isForNewGame) 
        { 
            _game = game;
            _infSegmentList = new InfSegmentList(_game.SQList, this);
            _arrayOfPaths = new SKPath[(int)IT.Total];

            InitArrayOfPaths();
            LoadPathsIntoArray();
            PaintPathsOnMap(canvas);
        }
        public PaintTypes PaintTypes { get; set; } = new PaintTypes();

        private void InitArrayOfPaths()
        {
            for (int i = 0; i < (int)IT.Total; i++)
            {
                _arrayOfPaths[i] = new SKPath();
            }
        }
        private void LoadPathsIntoArray()
        {
            List<InfSegment>[] arrayOfListsOfInfSegments = new List<InfSegment>[(int)IT.Total];
            splitInfSegmentsIntoIT();
            addInfSegmentsToPathArray();

            void splitInfSegmentsIntoIT()
            {
                for (int i = 0; i < (int)IT.Total; i++)
                {
                    if ((IT)i != IT.Hub)
                    {
                        arrayOfListsOfInfSegments[i] = _infSegmentList
                            .Where(infSeg => infSeg.InfrastructureType == (IT)i)
                            .ToList();
                    }
                    else { arrayOfListsOfInfSegments[i] = new List<InfSegment>(); }
                }
            }
            void addInfSegmentsToPathArray()
            {
                SearchModifierList searchModifiers = new SearchModifierList();

                for (int i = 0; i < (int)IT.Total; i++)
                {
                    if((IT)i != IT.Hub)
                    {
                        foreach (List<InfSegment> infSegmentList in arrayOfListsOfInfSegments)
                        {
                            foreach (InfSegment infSeg in infSegmentList)
                            {
                                foreach (SegmentType segmentType in infSeg.ListOfSegmentTypes)
                                {
                                    addSegmentToPathArray(infSeg, segmentType);
                                }
                            }
                        }
                    }
                }
                void addSegmentToPathArray(InfSegment infSeg, SegmentType st)
                {
                    SKPath path = _arrayOfPaths[(int)infSeg.InfrastructureType];
                    int adjRow, adjCol;
                    SQ adjSQ;
                    InfSKPoints adjSKPoints;
                    bool isWater = (infSeg.InfrastructureType == IT.MainRiver
                        || infSeg.InfrastructureType == IT.Tributary) ? true : false;

                    switch (st)
                    {
                        case SegmentType.NW_out_to_W:
                            adjRow = infSeg.Row + searchModifiers[ConnectDirection.W].Row;
                            adjCol = infSeg.Col + searchModifiers[ConnectDirection.W].Col;
                            
                            adjSQ = _game.SQList[adjRow, adjCol];
                            adjSKPoints = new InfSKPoints(adjSQ, infSeg.InfrastructureType);

                            path.MoveTo(infSeg.ThisSQSKPoints.NW);
                            path.LineTo(adjSKPoints.NW);
                            break;

                        case SegmentType.NW_out_to_N:
                            adjRow = infSeg.Row + searchModifiers[ConnectDirection.N].Row;
                            adjCol = infSeg.Col + searchModifiers[ConnectDirection.N].Col;

                            adjSQ = _game.SQList[adjRow, adjCol];
                            adjSKPoints = new InfSKPoints(adjSQ, infSeg.InfrastructureType);

                            path.MoveTo(infSeg.ThisSQSKPoints.NW);
                            path.LineTo(adjSKPoints.NW);
                            break;

                        case SegmentType.NE_out_to_N:
                            adjRow = infSeg.Row + searchModifiers[ConnectDirection.N].Row;
                            adjCol = infSeg.Col + searchModifiers[ConnectDirection.N].Col;

                            adjSQ = _game.SQList[adjRow, adjCol];
                            adjSKPoints = new InfSKPoints(adjSQ, infSeg.InfrastructureType);

                            path.MoveTo(infSeg.ThisSQSKPoints.NE);
                            path.LineTo(adjSKPoints.SE);
                            break;

                        case SegmentType.NE_out_to_E:
                            adjRow = infSeg.Row + searchModifiers[ConnectDirection.E].Row;
                            adjCol = infSeg.Col + searchModifiers[ConnectDirection.E].Col;

                            adjSQ = _game.SQList[adjRow, adjCol];
                            adjSKPoints = new InfSKPoints(adjSQ, infSeg.InfrastructureType);

                            path.MoveTo(infSeg.ThisSQSKPoints.NE);
                            path.LineTo(adjSKPoints.NW);
                            break;

                        case SegmentType.SW_out_to_S:
                            adjRow = infSeg.Row + searchModifiers[ConnectDirection.S].Row;
                            adjCol = infSeg.Col + searchModifiers[ConnectDirection.S].Col;

                            adjSQ = _game.SQList[adjRow, adjCol];
                            adjSKPoints = new InfSKPoints(adjSQ, infSeg.InfrastructureType);

                            path.MoveTo(infSeg.ThisSQSKPoints.SW);
                            path.LineTo(adjSKPoints.NW);
                            break;

                        case SegmentType.SW_out_to_W:
                            adjRow = infSeg.Row + searchModifiers[ConnectDirection.W].Row;
                            adjCol = infSeg.Col + searchModifiers[ConnectDirection.W].Col;

                            adjSQ = _game.SQList[adjRow, adjCol];
                            adjSKPoints = new InfSKPoints(adjSQ, infSeg.InfrastructureType);

                            path.MoveTo(infSeg.ThisSQSKPoints.SW);
                            path.LineTo(adjSKPoints.SE);
                            break;

                        case SegmentType.SE_out_to_E:
                            adjRow = infSeg.Row + searchModifiers[ConnectDirection.E].Row;
                            adjCol = infSeg.Col + searchModifiers[ConnectDirection.E].Col;

                            adjSQ = _game.SQList[adjRow, adjCol];
                            adjSKPoints = new InfSKPoints(adjSQ, infSeg.InfrastructureType);

                            path.MoveTo(infSeg.ThisSQSKPoints.SE);
                            path.LineTo(adjSKPoints.SE);
                            break;

                        case SegmentType.SE_out_to_S:
                            adjRow = infSeg.Row + searchModifiers[ConnectDirection.S].Row;
                            adjCol = infSeg.Col + searchModifiers[ConnectDirection.S].Col;

                            adjSQ = _game.SQList[adjRow, adjCol];
                            adjSKPoints = new InfSKPoints(adjSQ, infSeg.InfrastructureType);

                            path.MoveTo(infSeg.ThisSQSKPoints.SE);
                            path.LineTo(adjSKPoints.NE);
                            break;
                        
                        case SegmentType.NWxNE:
                            path.MoveTo(infSeg.ThisSQSKPoints.NW);
                            path.LineTo(infSeg.ThisSQSKPoints.NE);
                            break;

                        case SegmentType.NExSE:
                            path.MoveTo(infSeg.ThisSQSKPoints.NE);
                            path.LineTo(infSeg.ThisSQSKPoints.SE);
                            break;

                        case SegmentType.SWxNW:
                            path.MoveTo(infSeg.ThisSQSKPoints.SW);
                            path.LineTo(infSeg.ThisSQSKPoints.NW);
                            break;

                        case SegmentType.SExSW:
                            path.MoveTo(infSeg.ThisSQSKPoints.SE);
                            path.LineTo(infSeg.ThisSQSKPoints.SW);
                            break;

                        case SegmentType.Total:
                        case SegmentType.TotalAdjSqSegments:
                        default:
                            break;
                    }
                }
            }
        }

        private void PaintPathsOnMap(SKCanvas canvas)
        {
            for (int i = 0; i < (int)IT.Total; i++)
            {
                SKPath path = _arrayOfPaths[i];
                if (path != null) { canvas.DrawPath(path, PaintTypes[i]); }
            }
            canvas.Save();
        }

        //private SKPath[] GetPathArray()
        //{
        //    SKPath[] arrayOfPaths = new SKPath[(int)IT.Total];

        //    for (int i = 0; i < (int)IT.Total; i++)
        //    {                    
        //        IT thisIT = (IT)i;

        //        if (thisIT != IT.Hub)
        //        {
        //            List<InfSegment> requiredInfSegmentsForEachIT = _infSegmentList.Where(inf => inf.InfrastructureType == thisIT).ToList();
        //            SKPath path = new SKPath();

        //            foreach (InfSegment infSegment in requiredInfSegmentsForEachIT)
        //            {
        //                addInfSegmentsToPath(infSegment, path);
        //            }
        //            arrayOfPaths[i] = path;
        //        }                    
        //    }

        //    return arrayOfPaths;
        //}
        //private void addInfSegmentsToPath(InfSegment infSeg, SKPath path)
        //{
        //    if (infSeg.SQ != null)
        //    {
        //        switch (infSeg.SegmentType)
        //        {
        //            case SegmentType.NW_out_to_W:
        //                path.MoveTo(infSeg.ThisSQSKPoints.NW);
        //                if (infSeg.AdjSQ != null) { path.LineTo(infSeg.AdjSqSKPoints.NE); }
        //                break;

        //            case SegmentType.NW_out_to_N:
        //                path.MoveTo(infSeg.ThisSQSKPoints.NW);
        //                if (infSeg.AdjSQ != null) { path.LineTo(infSeg.AdjSqSKPoints.SW); }
        //                break;

        //            case SegmentType.NE_out_to_N:
        //                path.MoveTo(infSeg.ThisSQSKPoints.NE);
        //                if (infSeg.AdjSQ != null) { path.LineTo(infSeg.AdjSqSKPoints.SE); }
        //                break;

        //            case SegmentType.NE_out_to_E:
        //                path.MoveTo(infSeg.ThisSQSKPoints.NE);
        //                if (infSeg.AdjSQ != null) { path.LineTo(infSeg.AdjSqSKPoints.NW); }
        //                break;

        //            case SegmentType.SW_out_to_S:
        //                path.MoveTo(infSeg.ThisSQSKPoints.SW);
        //                if (infSeg.AdjSQ != null) { path.LineTo(infSeg.AdjSqSKPoints.NW); }
        //                break;

        //            case SegmentType.SW_out_to_W:
        //                path.MoveTo(infSeg.ThisSQSKPoints.SW);
        //                if (infSeg.AdjSQ != null) { path.LineTo(infSeg.AdjSqSKPoints.SE); }
        //                break;

        //            case SegmentType.SE_out_to_E:
        //                path.MoveTo(infSeg.ThisSQSKPoints.SE);
        //                if (infSeg.AdjSQ != null) { }

        //                break;

        //            case SegmentType.SE_out_to_S:
        //                path.MoveTo(infSeg.ThisSQSKPoints.SE);
        //                if (infSeg.AdjSQ != null) { path.LineTo(infSeg.AdjSqSKPoints.NE); }
        //                break;

        //            case SegmentType.NWxNE:
        //                path.MoveTo(infSeg.ThisSQSKPoints.NW);
        //                path.LineTo(infSeg.ThisSQSKPoints.NE);
        //                break;

        //            case SegmentType.NExSE:
        //                path.MoveTo(infSeg.ThisSQSKPoints.NE);
        //                path.LineTo(infSeg.ThisSQSKPoints.SE);
        //                break;

        //            case SegmentType.SWxNW:
        //                path.MoveTo(infSeg.ThisSQSKPoints.SW);
        //                path.LineTo(infSeg.ThisSQSKPoints.NW);
        //                break;

        //            case SegmentType.SExSW:
        //                path.MoveTo(infSeg.ThisSQSKPoints.SE);
        //                path.LineTo(infSeg.ThisSQSKPoints.SW);
        //                break;

        //            case SegmentType.Total:
        //            case SegmentType.TotalAdjSqSegments:
        //            default:
        //                break;
        //        }
        //    }
        //}


    }
}
