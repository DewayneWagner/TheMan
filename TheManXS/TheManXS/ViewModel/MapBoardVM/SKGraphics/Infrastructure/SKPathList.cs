using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using IT = TheManXS.Model.ParametersForGame.InfrastructureType;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Infrastructure
{
    class SKPathList : List<SKPath>
    {
        InfSegmentList _infSegmentList;
        Game _game;
        public SKPathList(Game game, InfSegmentList infSegmentList)
        {
            _game = game;
            for(int i = 0; i < (int)IT.Total; i++) { this.Add(new SKPath()); }
            _infSegmentList = infSegmentList;
            InitPaths();
        }
        void InitPaths()
        {
            foreach (InfSegment infSegment in _infSegmentList)
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
                    this[i].MoveTo(inf.From.NW);
                    switch (inf.ConnectionDirection)
                    {
                        case CD.NW:
                        case CD.NE:
                        case CD.SE:
                        case CD.SW:
                            passThroughPts = new InfSKPoints(_game.SQList[inf.SQTo.Row, inf.SQFrom.Col], it);
                            this[i].LineTo(passThroughPts.NW);                            
                            this[i].LineTo(inf.To.NW);
                            break;

                        case CD.N:
                        case CD.E:
                        case CD.S:
                        case CD.W:
                            this[i].LineTo(inf.To.NW);
                            break;

                        case CD.Total:
                        default:
                            break;
                    }
                }
                else
                {
                    this[i].MoveTo(inf.From.SE);
                    switch (inf.ConnectionDirection)
                    {
                        case CD.NW:
                        case CD.NE:
                        case CD.SE:
                        case CD.SW:
                            passThroughPts = new InfSKPoints(_game.SQList[inf.SQFrom.Row, inf.SQTo.Col], it);
                            this[i].LineTo(passThroughPts.SE);
                            this[i].LineTo(inf.To.SE);
                            break;

                        case CD.N:
                        case CD.E:
                        case CD.S:
                        case CD.W:
                            this[i].LineTo(inf.To.SE);
                            break;

                        case CD.Total:
                        default:
                            break;
                    }
                }
            }
        }
    }
}
