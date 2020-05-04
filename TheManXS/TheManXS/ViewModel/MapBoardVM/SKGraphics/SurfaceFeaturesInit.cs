using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using TT = TheManXS.Model.ParametersForGame.TerrainTypeE;
using ST = TheManXS.Model.ParametersForGame.StatusTypeE;
using RT = TheManXS.Model.ParametersForGame.ResourceTypeE;
using QC = TheManXS.Model.Settings.QuickConstants;
using TheManXS.ViewModel.MapBoardVM.SKGraphics.Nature.Mountains;
using TheManXS.ViewModel.MapBoardVM.SKGraphics.Nature.Forest;
using TheManXS.ViewModel.MapBoardVM.SKGraphics.Structures;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics
{
    class SurfaceFeaturesInit
    {
        Game _game;
        System.Random rnd = new System.Random();

        private SKPaint OwnedSQsPaint = new SKPaint()
        {
            IsAntialias = true,
            Style = SKPaintStyle.Fill,
        };

        public SurfaceFeaturesInit(Game game)
        {
            _game = game;
            InitSurfaceFeatures();
        }
        private void InitSurfaceFeatures()
        {
            using (SKCanvas canvas = new SKCanvas(_game.GameBoardVM.MapVM.SKBitMapOfMap))
            {
                foreach (KeyValuePair<int,SQ> sq in _game.SquareDictionary)
                {
                    var s = sq.Value;
                    
                    // terrain
                    if(s.TerrainType == TT.Mountain) { initMountain(); }
                    else if(s.TerrainType == TT.Forest) { initForest(); }

                    if (s.OwnerNumber != QC.PlayerIndexTheMan) { initOwnedSQs(); }

                    // structures
                    if (s.Status == ST.Producing)
                    {
                        if(s.ResourceType == RT.Oil) { initPumpJack(); }
                        else if(s.TerrainType == TT.City) { initCitySQ(); }
                        else { initMineShaft(); }
                    }

                    void initOwnedSQs()
                    {
                        OwnedSQsPaint.Color = _game.PlayerList[s.OwnerNumber].SKColor.WithAlpha(0x75);
                        canvas.DrawRect(sq.Value.SKRect, OwnedSQsPaint);
                    }
                    void initMountain() 
                    { 
                        TwoPeakMountain m = new TwoPeakMountain(s.SKRect);
                        canvas.DrawPath(m.MountainPath, m.MountainPaint);
                        canvas.DrawPath(m.MountainPath, m.MountainStroke);
                    }
                    void initForest()
                    {                        
                        int r = rnd.Next(0, 5);
                        if (r <= 2) 
                        { 
                            PoplarTree tree = new PoplarTree(s.SKRect, SKColors.ForestGreen);
                            canvas.DrawPath(tree.TreeBranchesPath, tree.FillPaint);
                            canvas.DrawPath(tree.TreeBranchesPath, tree.StrokePaint);
                        }
                        else 
                        { 
                            SpruceTree tree = new SpruceTree(s.SKRect, _game.PaletteColors.GetRandomColor(TT.Forest));
                            canvas.DrawPath(tree.TreeBranchesPath, tree.FillPaint);
                            canvas.DrawPath(tree.TreeBranchesPath, tree.StrokePaint);
                        }                        
                    }

                    void initCitySQ() { new LowDensity(_game, s); }
                    void initPumpJack() { new PumpJack(_game, s); }
                    void initMineShaft() { new MineShaft(_game, s); }
                }
                canvas.Save();
            }
        }
    }
}
