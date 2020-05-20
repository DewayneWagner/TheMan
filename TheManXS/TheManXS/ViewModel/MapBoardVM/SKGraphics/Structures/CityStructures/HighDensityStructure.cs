using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Structures.CityStructures
{
    class HighDensityStructure : CityStructure
    {
        public HighDensityStructure(SKCanvas canvas, SKRect position, SKColor companyColor) : base(canvas, position, companyColor)
        {
            InitHighRiseBuildings();
        }
        private void InitHighRiseBuildings()
        {
            float highRiseRectHeight = BlockSizeVertical / NumberOfHighRiseBuildingsPerRowPerBlock;
            float highRiseRectWidth = BlockSizeHorizontal / NumberOfHighRiseBuildingsPerColumnPerBlock;

            foreach (SKRect block in CityBlocks)
            {
                for (int row = 0; row < NumberOfHighRiseBuildingsPerRowPerBlock; row++)
                {
                    for (int col = 0; col < NumberOfHighRiseBuildingsPerColumnPerBlock; col++)
                    {
                        float left = block.Left + highRiseRectWidth * col;
                        float top = block.Top + highRiseRectHeight * row;
                        float right = left + highRiseRectWidth;
                        float bottom = top + highRiseRectHeight;

                        new HighRiseBuilding(new SKRect(left, top, right, bottom), Canvas);
                    }
                }
            }
        }
    }
}
