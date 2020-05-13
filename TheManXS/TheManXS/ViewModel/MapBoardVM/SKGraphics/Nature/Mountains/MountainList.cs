using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Main;
using TT = TheManXS.Model.ParametersForGame.TerrainTypeE;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Nature.Mountains
{
    class MountainList : List<Mountain>
    {
        Game _game;
        public MountainList(Game game)
        {
            _game = game;
            InitAllMountains();
            DrawAllMountains();
        }
        void InitAllMountains()
        {
            var sList = _game.SQList
                .Where(s => s.TerrainType == TT.Mountain)
                .Select(s => s.SKRect)
                .ToList();

            foreach(SKRect rect in sList) { this.Add(new TwoPeakMountain(rect)); }
        }
        void DrawAllMountains()
        {
            using (SKCanvas canvas = new SKCanvas(_game.GameBoardVM.MapVM.SKBitMapOfMap))
            {
                foreach (Mountain mountain in this)
                {
                    //mountain.SetGradient();
                    canvas.DrawPath(mountain.MountainPath, mountain.MountainPaint);
                    canvas.DrawPath(mountain.MountainPath, mountain.MountainStroke);
                }
                canvas.Save();
            }
        }
    }
}
