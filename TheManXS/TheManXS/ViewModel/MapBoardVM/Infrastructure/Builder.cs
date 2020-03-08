using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.ViewModel.MapBoardVM.MainElements;
using IT = TheManXS.Model.ParametersForGame.InfrastructureType;

namespace TheManXS.ViewModel.MapBoardVM.Infrastructure
{    
    public enum DirectionsCompass { N, E, S, W, Total }
    public class Builder
    {
        private PathCalculations _calc;
        private MapVM _mapVM;
        public Builder() { }
        
        public Builder(MapVM mapVM)
        {
            Formats = new PaintTypes();
            _calc = new PathCalculations();
            _mapVM = mapVM;
            new NewMapInitializer(mapVM,this);

            // need to save raw terrain map here - before putting player owned sq's on
            //SaveRawMap();
        }
        public PaintTypes Formats { get; set; }
        private void SaveRawMap()
        {

        }
    }
}
