using SkiaSharp;
using TheManXS.Model.Main;
using TheManXS.ViewModel.MapBoardVM.SKGraphics.Nature.Forest;
using TheManXS.ViewModel.MapBoardVM.SKGraphics.Nature.Mountains;
using TheManXS.ViewModel.MapBoardVM.SKGraphics.Structures;
using TheManXS.ViewModel.MapBoardVM.SKGraphics.Structures.CityStructures;
using QC = TheManXS.Model.Settings.QuickConstants;
using RT = TheManXS.Model.ParametersForGame.ResourceTypeE;
using ST = TheManXS.Model.ParametersForGame.StatusTypeE;
using TT = TheManXS.Model.ParametersForGame.TerrainTypeE;

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
                foreach (SQ s in _game.SQList)
                {
                    // terrain
                    if (s.TerrainType == TT.Mountain) { initMountain(); }
                    else if (s.TerrainType == TT.Forest) { initForest(); }

                    if (s.OwnerNumber != QC.PlayerIndexTheMan) { initOwnedSQs(); }

                    // structures
                    if (s.Status == ST.Producing)
                    {
                        if (s.ResourceType == RT.Oil) { initPumpJack(); }
                        else if (s.TerrainType == TT.City) { initCitySQ(); }
                        else { initMineShaft(); }
                    }
                    void initOwnedSQs()
                    {
                        OwnedSQsPaint.Color = _game.PlayerList[s.OwnerNumber].SKColor.WithAlpha(0x50);
                        canvas.DrawRect(s.SKRect, OwnedSQsPaint);
                    }
                    void initMountain()
                    {
                        TwoPeakMountain m = new TwoPeakMountain(s.SKRect);
                        canvas.DrawPath(m.MountainPath, m.MountainPaint);
                        canvas.DrawPath(m.MountainPath, m.MountainStroke);
                    }
                    void initForest()
                    {
                        TreesList treeList = new TreesList(s.SKRect, _game.PaletteColors);
                        foreach (Tree tree in treeList)
                        {
                            canvas.DrawPath(tree.TreeBranchesPath, tree.FillPaint);
                            canvas.DrawPath(tree.TreeBranchesPath, tree.StrokePaint);
                            canvas.DrawRect(tree.TreeTrunkRect, tree.TreeTrunkFill);
                            canvas.DrawRect(tree.TreeTrunkRect, tree.StrokePaint);
                        }
                    }

                    void initCitySQ() { new LowDensityStructure(canvas, s.SKRect, _game.PlayerList[s.OwnerNumber].SKColor.WithAlpha(0x50)); }
                    void initPumpJack() { new PumpJack(_game, s); }
                    void initMineShaft() { new MineShaft(_game, s); }
                }
                canvas.Save();
            }
        }
    }
}
