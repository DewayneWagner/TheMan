using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Structures.CityStructures
{
    class LowDensityStructure : CityStructure
    {
        public LowDensityStructure(SKCanvas canvas, SKRect position, SKColor companyColor) : base(canvas, position, companyColor)
        {
            InitHouses();
        }

        private void InitHouses()
        {
            float houseRectVerticalSize = BlockSizeVertical / NumberOfHouseRowsPerBlock;
            float houseRectHorizontalSize = BlockSizeHorizontal / NumberOfHouseColumnsPerBlock;

            foreach (SKRect block in CityBlocks)
            {
                for (int row = 0; row < NumberOfHouseRowsPerBlock; row++)
                {
                    for (int col = 0; col < NumberOfHouseColumnsPerBlock; col++)
                    {
                        float left = block.Left + houseRectVerticalSize * col;
                        float top = block.Top + houseRectHorizontalSize * row;
                        float right = left + houseRectVerticalSize;
                        float bottom = top + houseRectHorizontalSize;

                        new House(new SKRect(left, top, right, bottom), Canvas);
                    }
                }
            }
        }
    }
}
