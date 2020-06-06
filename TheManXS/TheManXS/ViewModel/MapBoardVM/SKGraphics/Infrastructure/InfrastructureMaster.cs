using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Map.Surface;
using IT = TheManXS.Model.ParametersForGame.InfrastructureType;
using ST = TheManXS.ViewModel.MapBoardVM.SKGraphics.Infrastructure.SegmentType;
using SkiaSharp;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Infrastructure
{
    public enum CD { NW, N, NE, E, SE, S, SW, W, Total }
    class InfrastructureMaster
    {        
        Game _game;
        List<SQ> _sqList;

        private SKPath[] _paths;
        
        List<InfSegment> _listOfInfSegments;

        // at start of game
        public InfrastructureMaster(Game game)
        {
            _game = game;
            _sqList = new List<SQ>();
            foreach(SQ sq in game.SQList) { _sqList.Add(sq); }
            _paths = getInitializedArray();
            _listOfInfSegments = GetListOfInfSegments();
            CreatePaths();
            PaintAllPathsOnCanvas();
        }

        // for adding new infrastructure during game
        public InfrastructureMaster(Game game, List<SQ> sqList)
        {
            _game = game;
            _sqList = sqList;
        }

        private SKPath[] getInitializedArray()
        {
            SKPath[] paths = new SKPath[(int)IT.Total];

            for (int i = 0; i < (int)IT.Total; i++)
            {
                paths[i] = new SKPath();
            }

            return paths;
        }

        private List<InfSegment> GetListOfInfSegments()
        {
            
        }
        
        void CreatePaths()
        {
            foreach (InfSegment infSegment in _listOfInfSegments)
            {
                setSegmentType(infSegment);
            }
            void setSegmentType(InfSegment inf)
            {
                IT it = inf.InfrastructureType;
                int i = (int)it;
                InfSKPoints passThroughPts;                

                if (inf.InfrastructureType == IT.MainRiver || inf.InfrastructureType == IT.Tributary)
                {
                    _paths[i].MoveTo(inf.From.NW);
                    switch (inf.ConnectionDirection)
                    {
                        case CD.NW:
                            passThroughPts = new InfSKPoints(_game.SQList[inf.SQFrom.Row, inf.SQTo.Col], it);                                                                                
                            _paths[i].LineTo(inf.From.NE);
                            _paths[i].LineTo(passThroughPts.NW);
                            _paths[i].LineTo(inf.To.NW);
                            break;

                        case CD.N:                                        
                            _paths[i].LineTo(inf.To.NW);
                            break;

                        case CD.NE:
                            passThroughPts = new InfSKPoints(_game.SQList[inf.SQFrom.Row, inf.SQTo.Col], it);                                                                                
                            _paths[i].LineTo(inf.From.NE);
                            _paths[i].LineTo(passThroughPts.NW);
                            _paths[i].LineTo(inf.To.NW);
                            break;

                        case CD.E:                                        
                            _paths[i].LineTo(inf.To.NW);
                            break;

                        case CD.SE:
                            passThroughPts = new InfSKPoints(_game.SQList[inf.SQTo.Row, inf.SQFrom.Col], it);                                                                                
                            _paths[i].LineTo(passThroughPts.NW);
                            _paths[i].LineTo(inf.To.NW);
                            break;

                        case CD.S:
                            _paths[i].LineTo(inf.To.NW);
                            break;

                        case CD.SW:
                            passThroughPts = new InfSKPoints(_game.SQList[inf.SQTo.Row, inf.SQFrom.Col], it);
                            _paths[i].LineTo(passThroughPts.NW);
                            _paths[i].LineTo(inf.To.NW);
                            break;

                        case CD.W:
                            _paths[i].LineTo(inf.To.NW);
                            break;

                        case CD.Total:
                        default:
                            break;
                    }
                }
                else
                {
                    _paths[i].MoveTo(inf.From.SE);
                    switch (inf.ConnectionDirection)
                    {
                        case CD.NW:
                            passThroughPts = new InfSKPoints(_game.SQList[inf.SQFrom.Row, inf.SQTo.Col], it);
                            _paths[i].LineTo(passThroughPts.SE);
                            _paths[i].LineTo(inf.To.SE);                                        
                            break;

                        case CD.N:
                            _paths[i].LineTo(inf.To.SE);
                            break;

                        case CD.NE:
                            passThroughPts = new InfSKPoints(_game.SQList[inf.SQFrom.Row, inf.SQTo.Col], it);
                            _paths[i].LineTo(passThroughPts.SE);
                            _paths[i].LineTo(inf.To.SE);
                            break;

                        case CD.E:
                            _paths[i].LineTo(inf.To.SE);
                            break;

                        case CD.SE:
                            passThroughPts = new InfSKPoints(_game.SQList[inf.SQFrom.Row, inf.SQTo.Col], it);
                            _paths[i].LineTo(passThroughPts.SE);
                            _paths[i].LineTo(inf.To.SE);
                            break;

                        case CD.S:
                            _paths[i].LineTo(inf.To.SE);
                            break;

                        case CD.SW:
                            passThroughPts = new InfSKPoints(_game.SQList[inf.SQFrom.Row, inf.SQTo.Col], it);
                            _paths[i].LineTo(passThroughPts.SE);
                            _paths[i].LineTo(inf.To.SE);
                            break;

                        case CD.W:
                            _paths[i].LineTo(inf.To.SE);
                            break;

                        case CD.Total:
                        default:
                            break;
                    }
                }
            }
        }
        void PaintAllPathsOnCanvas()
        {
            PaintTypes pt = new PaintTypes();
            using (SKCanvas canvas = new SKCanvas(_game.GameBoardVM.MapVM.SKBitMapOfMap))
            {
                for (int i = 0; i < (int)IT.Total; i++)
                {
                    if (i != (int)IT.Hub)
                    {
                        canvas.DrawPath(_paths[i], pt[i]);
                    }                
                }
                canvas.Save();
            }
        }
    }
}
