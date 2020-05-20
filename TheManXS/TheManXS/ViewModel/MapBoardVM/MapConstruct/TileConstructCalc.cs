using SkiaSharp;
using System;
using TheManXS.Model.Main;
using TheManXS.Model.ParametersForGame;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.MapConstruct
{
    public class TileConstructCalc
    {
        private int _row;
        private int _col;
        System.Random rnd = new System.Random();
        Game _game;
        public enum sqFormats { LinearGradient, SolidColor, SweepGradient, VerticalSplit, HorizontalSplit, Pixelly }

        public TileConstructCalc(int row, int col, Game game)
        {
            _game = game;
            _row = row;
            _col = col;
        }
        public SKPoint GetCenterPoint()
        {
            float x = (float)((_col * QC.SqSize) + (rnd.NextDouble() * QC.SqSize));
            float y = (float)((_row * QC.SqSize) + (rnd.NextDouble() * QC.SqSize));
            return new SKPoint(x, y);
        }
        public SKColor[] GetGradientColors(int q, TerrainTypeE tt)
        {
            SKColor[] colors = new SKColor[q];
            for (int i = 0; i < q; i++) { colors[i] = _game.PaletteColors.GetRandomColor(tt); }
            return colors;
        }
        public Tuple<SKPoint, SKPoint> GetGradientPoints()
        {
            int k = rnd.Next(0, 2);
            int kk = k == 1 ? 0 : 1;

            SKPoint from = new SKPoint(
                (float)((_col * QC.SqSize) + (QC.SqSize * rnd.NextDouble() * k)),
                (float)((_row * QC.SqSize) + (QC.SqSize * rnd.NextDouble() * kk)));

            SKPoint to = new SKPoint(
                (float)(((_col + 1) * QC.SqSize) + (QC.SqSize * rnd.NextDouble() * kk)),
                (float)(((_row + 1) * QC.SqSize) + (QC.SqSize * rnd.NextDouble() * k)));

            return new Tuple<SKPoint, SKPoint>(from, to);
        }
        public float[] GetColorPosition(int q)
        {
            float[] positions = new float[q];
            for (int i = 0; i < q; i++) { positions[i] = (float)rnd.NextDouble(); }
            return positions;
        }
        public sqFormats GetFormat(TerrainTypeE tt)
        {
            switch (tt)
            {
                case TerrainTypeE.Grassland:
                    return (sqFormats)(rnd.Next(0, (int)sqFormats.Pixelly));
                case TerrainTypeE.Forest:
                    return sqFormats.Pixelly;
                case TerrainTypeE.Mountain:
                    return sqFormats.SweepGradient;
                case TerrainTypeE.City:
                    return sqFormats.SolidColor;
                default:
                    return sqFormats.SolidColor;
            }
        }
    }
}
