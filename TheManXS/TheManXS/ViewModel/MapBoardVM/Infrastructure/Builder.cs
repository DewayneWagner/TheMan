using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.ViewModel.MapBoardVM.MainElements;

namespace TheManXS.ViewModel.MapBoardVM.Infrastructure
{
    public enum InfrastructureType { MainRiver, Tributary, Road, Pipeline, RailRoad, Hub, Total }
    public enum InfrastructureDirection { FromWest, FromNorth, FromEast, FromSouth }
    public class Builder
    {
        public Builder() { }

        public Builder(MapVM mapVM)
        {
            Formats = new PaintTypes();
            new NewMapInitializer(mapVM,this);
        }
        public PaintTypes Formats { get; set; }
        
        public void UpdateInfrastructure(SKPath infrastructurePath, InfrastructureType it)
        {
            // this will be for adding infrastructure segments throughout game
        }
    }
}
