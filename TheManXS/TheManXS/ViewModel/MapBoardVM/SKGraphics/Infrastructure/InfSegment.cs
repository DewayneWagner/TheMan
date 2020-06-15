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
        NW_out_to_W,    NW_out_to_N,   NE_out_to_N,     NE_out_to_E,
        SW_out_to_S,    SW_out_to_W,   SE_out_to_E,     SE_out_to_S,
        TotalAdjSqSegments, 

        NWxNE, NExSE, SWxNW, SExSW,
        Total
    }

    class InfSegment
    {
        public InfSegment() {}
        public SQ SQFrom { get; set; }
        public SQ SQTo { get; set; }
        public IT InfrastructureType { get; set; }
        public CD ConnectionDirection { get; set; }
        public bool IsDiagonal { get; set; }
        public InfSKPoints From => new InfSKPoints(SQFrom, InfrastructureType);
        public InfSKPoints To => new InfSKPoints(SQTo, InfrastructureType);
    }
}
