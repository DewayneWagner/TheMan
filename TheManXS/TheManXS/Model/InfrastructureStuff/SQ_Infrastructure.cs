using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;

namespace TheManXS.Model.InfrastructureStuff
{
    public class SQ_Infrastructure
    {
        public SQ_Infrastructure(SQ sq) { Key = sq.Key; }
        public int Key { get; set; }
        public bool IsMainTransportationCorridor { get; set; }
        public bool IsRoadConnected { get; set; }
        public bool IsSecondaryRoad { get; set; }
        public bool IsTrainConnected { get; set; }
        public bool IsPipelineConnected { get; set; }
        public bool IsHub { get; set; }
        public bool IsMainRiver { get; set; }
        public bool IsTributary { get; set; }
        public int TributaryNumber { get; set; }
        public bool IsTributaryFlowingFromNorth { get; set; }
    }
}
