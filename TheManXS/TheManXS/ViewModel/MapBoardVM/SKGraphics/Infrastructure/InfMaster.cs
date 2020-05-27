using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TheManXS.Model.Main;
using IT = TheManXS.Model.ParametersForGame.InfrastructureType;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Infrastructure
{
    class InfMaster
    {
        Game _game;
        private InfSegmentList _infSegmentList;
        public InfMaster(Game game, SKCanvas canvas, bool isForNewGame) 
        { 
            _game = game;
            _infSegmentList = new InfSegmentList(_game.SQList, this);
            PaintPathsOnMap(canvas);
        }
        public PaintTypes PaintTypes { get; set; } = new PaintTypes();
        private SKPath[] ArrayOfPaths
        {
            get
            {
                SKPath[] arrayOfPaths = new SKPath[(int)IT.Total];

                for (int i = 0; i < (int)IT.Total; i++)
                {
                    IT thisIT = (IT)i;
                    List<InfSegment> requiredInfSegmentsForEachIT = _infSegmentList.Where(inf => inf.InfrastructureType == thisIT).ToList();
                    
                    foreach (InfSegment infSegment in requiredInfSegmentsForEachIT)
                    {
                        addInfSegmentsToPath(infSegment);
                    }
                }

                return arrayOfPaths;

                void addInfSegmentsToPath(InfSegment infSeg)
                {
                    SKPath path = arrayOfPaths[(int)infSeg.InfrastructureType];
                    switch (infSeg.SegmentType)
                    {
                        case SegmentType.NW_out_to_W:
                            path.MoveTo(infSeg.ThisSQSKPoints.NW);
                            path.LineTo(infSeg.AdjSqSKPoints.NE);
                            break;

                        case SegmentType.NW_out_to_N:
                            path.MoveTo(infSeg.ThisSQSKPoints.NW);
                            path.LineTo(infSeg.AdjSqSKPoints.SW);
                            break;

                        case SegmentType.NE_out_to_N:
                            path.MoveTo(infSeg.ThisSQSKPoints.NE);
                            path.LineTo(infSeg.AdjSqSKPoints.SE);
                            break;

                        case SegmentType.NE_out_to_E:
                            path.MoveTo(infSeg.ThisSQSKPoints.NE);
                            path.LineTo(infSeg.AdjSqSKPoints.NW);
                            break;

                        case SegmentType.SW_out_to_S:
                            path.MoveTo(infSeg.ThisSQSKPoints.SW);
                            path.LineTo(infSeg.AdjSqSKPoints.NW);
                            break;

                        case SegmentType.SW_out_to_W:
                            path.MoveTo(infSeg.ThisSQSKPoints.SW);
                            path.LineTo(infSeg.AdjSqSKPoints.SE);
                            break;

                        case SegmentType.SE_out_to_E:
                            path.MoveTo(infSeg.ThisSQSKPoints.SE);
                            path.LineTo(infSeg.AdjSqSKPoints.SW);
                            break;

                        case SegmentType.SE_out_to_S:
                            path.MoveTo(infSeg.ThisSQSKPoints.SE);
                            path.LineTo(infSeg.AdjSqSKPoints.NE);
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
                canvas.DrawPath(ArrayOfPaths[i], PaintTypes[i]);
            }
            canvas.Save();
        }
    }
}
