using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.ViewModel.MapBoardVM.MainElements;
using IT = TheManXS.Model.Settings.SettingsMaster.InfrastructureType;

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
        }
        public PaintTypes Formats { get; set; }
        
        public void UpdateInfrastructure(List<SQ> sqList, IT it)
        {
            SKPath path = new SKPath();
            path.MoveTo(_calc.GetInfrastructureSKPoint(sqList[0], it));
            foreach (SQ sq in sqList)
            {
                path.LineTo(_calc.GetInfrastructureSKPoint(sq, it));
            }
            path.Close();

            using (SKCanvas canvas = new SKCanvas(_mapVM.Map))
            {
                canvas.DrawPath(path, Formats[(int)it]);
                canvas.Save();
            }
        }
    }
}
