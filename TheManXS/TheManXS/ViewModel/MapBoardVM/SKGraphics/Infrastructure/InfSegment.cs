using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using IT = TheManXS.Model.ParametersForGame.InfrastructureType;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Infrastructure
{
    public enum SegmentType
    {
        NW_out_to_W, NW_out_to_N,   NE_out_to_N, NE_out_to_E,
        SW_out_to_S, SW_out_to_W,   SE_out_to_E, SE_out_to_S,
        TotalAdjSqSegments, 

        NWxNE, NExSE, SWxNW, SExSW,
        Total
    }

    class InfSegment
    {
        public SQ SQ { get; set; }
        public SQ AdjSQ { get; set; }
        public int Row => SQ.Row;
        public int Col => SQ.Col;
        public IT InfrastructureType { get; set; }
        public SegmentType SegmentType { get; set; }
        public InfSKPoints ThisSQSKPoints => new InfSKPoints(SQ, InfrastructureType);
        public InfSKPoints AdjSqSKPoints => new InfSKPoints(AdjSQ, InfrastructureType);
    }
}
