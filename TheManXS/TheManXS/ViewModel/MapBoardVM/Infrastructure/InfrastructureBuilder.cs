using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.ViewModel.MapBoardVM.MainElements;

namespace TheManXS.ViewModel.MapBoardVM.Infrastructure
{
    public enum InfrastructureType { MainRiver, Tributary, Road, Pipeline, RailRoad, Hub, Total }
    public enum InfrastructureDirection { FromWest, FromNorth, FromEast, FromSouth }
    public class InfrastructureBuilder
    {
        public InfrastructureBuilder() { }

        public InfrastructureBuilder(MapVM mapVM)
        {
            Formats = new InfrastructurePaintTypes();
            new NewMapInitializer(mapVM,this);
        }

        public InfrastructurePaintTypes Formats { get; set; }
        
        public void UpdateInfrastructure(SKPath infrastructurePath, InfrastructureType it)
        {

        }
    }
}
