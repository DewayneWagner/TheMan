using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Structures.CityStructures
{
    class MediumDensityStructure : CityStructure
    {
        public MediumDensityStructure(SKCanvas canvas, SKRect position, SKColor companyColor) : base(canvas, position, companyColor)
        {
            InitLowRiseBuildings();
        }
        private void InitLowRiseBuildings()
        {
            float lowRiseRectHeight = BlockSizeVertical / NumberOfLowRiseBuildingsPerRowPerBlock;
            float lowRiseRectWidth = BlockSizeHorizontal / NumberOfLowRiseBuildingsPerColumnPerBlock;

            foreach (SKRect block in CityBlocks)
            {
                for (int row = 0; row < NumberOfLowRiseBuildingsPerRowPerBlock; row++)
                {
                    for (int col = 0; col < NumberOfLowRiseBuildingsPerColumnPerBlock; col++)
                    {
                        float left = block.Left + lowRiseRectWidth * col;
                        float top = block.Top + lowRiseRectHeight * row;
                        float right = left + lowRiseRectWidth;
                        float bottom = top + lowRiseRectHeight;

                        new LowRiseBuilding(new SKRect(left, top, right, bottom), Canvas);
                    }
                }
            }
        }
    }
}
