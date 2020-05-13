using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Main;
using TT = TheManXS.Model.ParametersForGame.TerrainTypeE;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Nature.Mountains
{
    class MountainRange
    {
        Game _game;
        public MountainRange(Game game)
        {
            _game = game;
            InitAllMountains();
        }
        void InitAllMountains()
        {
            var sList = _game.SQList
                .Where(s => s.TerrainType == TT.Mountain)
                .Select(s => s.SKRect)
                .ToList();

            using (SKCanvas canvas = new SKCanvas(_game.GameBoardVM.MapVM.SKBitMapOfMap))
            {
                foreach (SKRect skRect in sList)
                {
                    Mountain m = new TwoPeakMountain(skRect);
                    canvas.DrawPath(m.MountainPath, m.MountainPaint);
                    canvas.DrawPath(m.MountainPath, m.MountainStroke);
                }
                canvas.Save();
            }
        }
    }
}
