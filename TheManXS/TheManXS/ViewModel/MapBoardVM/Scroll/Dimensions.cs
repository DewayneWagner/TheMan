using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.ViewModel.MapBoardVM.Scroll;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.Scroll
{
    public class Dimensions
    {
        private double _start_XY;

        public Dimensions(bool isRow, double x, double y)
        {
            if (isRow)
            {
                _start_XY = y;
                Start = GetStart();
                Quantity = GetQuantity(true);
                End = Start + Quantity - 1;
            }
            else
            {
                _start_XY = x;
                Start = GetStart();
                Quantity = GetQuantity(false);
                End = Start + Quantity - 1;            
            }
        }
        
        public int Start { get; set; }
        public int End { get; set; }
        public int Quantity { get; set; }

        private int GetStart()
        {
            int c = (int)_start_XY / QC.SqSize;
            if(_start_XY == 0) { return 0; }
            else if(c % 2 == 0) { return c; }
            else { return c - 1; }
        }
        private int GetQuantity(bool isVertical)
        {
            double ratio = (isVertical) ? ScrollConstants.FocusedGridLargerThenScreenRatioY :
                ScrollConstants.FocusedGridLargerThenScreenRatioX;
            int q = (isVertical) ? (int)((ratio * QC.ScreenHeight)/QC.SqSize) : (int)((ratio * QC.ScreenWidth)/QC.SqSize);
            return (q % 2 == 0) ? q : (q - 1);
        }
    }
}
