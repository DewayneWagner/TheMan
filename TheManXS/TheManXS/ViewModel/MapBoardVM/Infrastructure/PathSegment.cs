using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.InfrastructureStuff;
using IT = TheManXS.Model.ParametersForGame.InfrastructureType;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.Infrastructure
{
    public enum SegmentType { EdgePointStart, Curve, Straight, EdgePointEnd }

    class PathSegment
    {
        public PathSegment() { }
        public SegmentType SegmentType { get; set; }
        public SKPoint SKPoint { get; set; } 
        public float Radius { get; set; }
        public int StraightSegmentID { get; set; }

        /************
         *  0 - 1 - 2
         *  7 - X - 3
         *  6 - 5 - 4
         *  *********/

        public byte EntryPoint { get; set; }
        public byte ExitPoint { get; set; }
    }    
    
}
