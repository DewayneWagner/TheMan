using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.ViewModel.MapBoardVM.Infrastructure;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.Infrastructure
{
    class Paths : SKPath
    {
        PathCalculations _calc;
        public Paths(InfrastructureType type)
        {
            Type = type;
            _calc = new PathCalculations();
        }
        public InfrastructureType Type { get; set; }
        public InfrastructureDirection Direction { get; set; }
        public int SegmentNumber { get; set; }
    }
}
