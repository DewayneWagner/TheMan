using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Main;
using IT = TheManXS.Model.ParametersForGame.InfrastructureType;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Infrastructure
{
    class SKPathList : List<SKPath>
    {
        List<InfSegment> _scrubbedInfSegmentList;        
        Game _game;

        public SKPathList(Game game, InfSegmentList infSegmentList)
        {
            _game = game;
            for(int i = 0; i < (int)IT.Total; i++) { this.Add(new SKPath()); }
            //InitTestSQsWhereThereIsInf(infSegmentList);
            _scrubbedInfSegmentList = GetScrubbedList(infSegmentList);
            
            InitPaths();
        }
        void InitTestSQsWhereThereIsInf(InfSegmentList infSegmentList)
        {
            SKPaint testSqPaint = new SKPaint()
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.Yellow.WithAlpha(0x50),
            };

            using (SKCanvas canvas = new SKCanvas(_game.GameBoardVM.MapVM.SKBitMapOfMap))
            {
                foreach (InfSegment infSegment in infSegmentList)
                {
                    canvas.DrawRect(infSegment.SQFrom.SKRect, testSqPaint);
                }
                canvas.Save();
            }
        }
        void InitPaths()
        {
            foreach (InfSegment infSegment in _scrubbedInfSegmentList)
            {
                addSegmentToPath(infSegment);
            }
            
            void addSegmentToPath(InfSegment inf)
            {
                IT it = inf.InfrastructureType;
                int i = (int)it;
                InfSKPoints passThroughPts;

                if (inf.InfrastructureType == IT.MainRiver || inf.InfrastructureType == IT.Tributary)
                {
                    this[i].MoveTo(inf.From.NW);

                    if (inf.IsDiagonal)
                    {
                        passThroughPts = new InfSKPoints(_game.SQList[inf.SQFrom.Row, inf.SQTo.Col], it);
                        this[i].LineTo(passThroughPts.NW);
                        this[i].LineTo(inf.To.NW);
                    }
                    else { this[i].LineTo(inf.To.NW); }
                }
                else
                {
                    this[i].MoveTo(inf.From.SE);

                    if (inf.IsDiagonal)
                    {
                        passThroughPts = new InfSKPoints(_game.SQList[inf.SQFrom.Row, inf.SQTo.Col], it);
                        this[i].LineTo(passThroughPts.SE);
                        this[i].LineTo(inf.To.SE);
                    }
                    else { this[i].LineTo(inf.To.SE); }
                }
            }            
        }
        List<InfSegment> GetScrubbedList(InfSegmentList infSegmentList)
        {
            List<InfSegment> scrubbedList = new List<InfSegment>();

            foreach (InfSegment infSeg in infSegmentList)
            {
                if (!doesOppositeSegmentAlreadyExist()) { scrubbedList.Add(infSeg); }

                bool doesOppositeSegmentAlreadyExist()
                {
                    return scrubbedList.Any(i => i.SQFrom == infSeg.SQTo
                                && i.SQTo == infSeg.SQFrom
                                && i.InfrastructureType == infSeg.InfrastructureType
                                && i.ConnectionDirection == getOppositeDirection(infSeg.ConnectionDirection));

                    CD getOppositeDirection(CD cd)
                    {
                        int cdInt = (int)cd;
                        if(cdInt < (int)CD.SE) { return (CD)(cdInt + 4); }
                        else { return (CD)(cdInt - 4); }
                    }
                }
            }
            return scrubbedList;
        }
    }
}
