using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Map.Surface;
using QC = TheManXS.Model.Settings.QuickConstants;
using IT = TheManXS.Model.ParametersForGame.InfrastructureType;

namespace TheManXS.ViewModel.MapBoardVM.Infrastructure
{
    class SideRoadInitializer
    {
        Game _game;
        PaintTypes _paintTypes = new PaintTypes();
        List<SQ> _sqsThatNeedARoad;
        List<SKPath> _listOfSKPaths;

        public SideRoadInitializer(Game game)
        {
            _game = game;
            _sqsThatNeedARoad = SqsThatNeedARoad();
            _listOfSKPaths = GetListOfPaths();
            DrawAllPathsOnCanvas();
        }
        private List <SQ> SqsThatNeedARoad()
        {
            return _game.SquareDictionary.Values
                            .Where(s => s.Status == Model.ParametersForGame.StatusTypeE.Producing)
                            .Where(s => s.ResourceType != Model.ParametersForGame.ResourceTypeE.RealEstate)
                            .ToList();
        }
        private List<SKPath> GetListOfPaths()
        {
            List<SKPath> _listOfPaths = new List<SKPath>();
                foreach (SQ sq in _sqsThatNeedARoad)
                {
                    _listOfPaths.Add(new SideRoad(_game, sq).SKPath);
                }
                return _listOfPaths;
        }
        void DrawAllPathsOnCanvas()
        {
            SKPaint roadPaint = new PaintTypes()[(int)IT.Road];
            using (SKCanvas canvas = new SKCanvas(_game.GameBoardVM.MapVM.SKBitMapOfMap))
            {
                foreach (SKPath path in _listOfSKPaths)
                {
                    canvas.DrawPath(path, roadPaint);
                }
                canvas.Save();
            }            
        }

        //public double EstimateRoadConstructionCost(List<SQ> roadRouteList)
        //{
        //    int grassland = 0, forest = 0, mountain = 0;

        //    foreach (SQ sq in roadRouteList)
        //    {
        //        switch (sq.TerrainType)
        //        {
        //            case TT.Forest:
        //                forest++;
        //                break;
        //            case TT.Grassland:
        //                grassland++;
        //                break;
        //            case TT.Mountain:
        //                mountain++;
        //                break;
        //        }
        //    }
        //    double costs = 0;

        //    var c = _game.ParameterConstantList;
        //    costs += (grassland * c.GetConstant(AllConstantParameters.InfrastructureConstructionRatiosTT, (int)TT.Grassland));
        //    costs += (forest * c.GetConstant(AllConstantParameters.InfrastructureConstructionRatiosTT, (int)TT.Forest));
        //    costs += (mountain * c.GetConstant(AllConstantParameters.InfrastructureConstructionRatiosTT, (int)TT.Mountain));
        //    return costs;
        //}
    }
}
