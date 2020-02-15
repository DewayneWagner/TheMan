using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Map.Surface;
using TheManXS.Model.Services.EntityFrameWork;
using TheManXS.ViewModel.MapBoardVM.MainElements;
using TheManXS.ViewModel.MapBoardVM.TouchTracking;

namespace TheManXS.ViewModel.MapBoardVM.TouchExecution
{
    public class ExecuteOneFingerSelect
    {
        MapVM _mapVM;
        public ExecuteOneFingerSelect(MapVM mapVM)
        {
            _mapVM = mapVM;
            ExecuteOneFingerSelectAction();
        }
        private void ExecuteOneFingerSelectAction()
        {
            Coordinate tp = new Coordinate(getTouchPointOnBitMap());
            _mapVM.SquareDictionary[tp.SQKey].Tile.OverlayGrid.SetColorsOfAllSides(SKColors.Black);
            
            SKPoint getTouchPointOnBitMap()
            {
                SKPoint pt = _mapVM.MapTouchList[0].FirstOrDefault(p => p.Type == TouchActionType.Pressed).SKPoint;
                float bitmapX = ((pt.X - _mapVM.MapMatrix.TransX) / _mapVM.MapMatrix.ScaleX);
                float bitmapY = ((pt.Y - _mapVM.MapMatrix.TransY) / _mapVM.MapMatrix.ScaleY);

                return new SKPoint(bitmapX, bitmapY);
            }
        }
    }
}
