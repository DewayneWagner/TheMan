using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Map.Surface;
using IT = TheManXS.Model.ParametersForGame.InfrastructureType;
using ST = TheManXS.ViewModel.MapBoardVM.SKGraphics.Infrastructure.SegmentType;
using SkiaSharp;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Infrastructure
{
    public enum CD { NW, N, NE, E, SE, S, SW, W, Total }
    class InfrastructureMaster
    {        
        Game _game;
        List<SQ> _sqList;
        private InfSegmentList _listOfInfSegments;
        private SKPathList _skPathList;

        // for adding new infrastructure during game
        public InfrastructureMaster(Game game, List<SQ> sqList)
        {
            _game = game;
            _sqList = sqList;
            InitPaths();
        }
        public InfrastructureMaster(Game game)
        {
            _game = game;
            _sqList = new List<SQ>();
            foreach(SQ sq in game.SQList) { _sqList.Add(sq); }
            InitPaths();
        }

        private void InitPaths()
        {
            _listOfInfSegments = new InfSegmentList(_game, _sqList);
            _skPathList = new SKPathList(_game, _listOfInfSegments);
            PaintAllPathsOnCanvas();
        }

        void PaintAllPathsOnCanvas()
        {
            PaintTypes pt = new PaintTypes();
            using (SKCanvas canvas = new SKCanvas(_game.GameBoardVM.MapVM.SKBitMapOfMap))
            {
                for (int i = 0; i < (int)IT.Total; i++)
                {
                    if (i != (int)IT.Hub)
                    {
                        canvas.DrawPath(_skPathList[i], pt[i]);
                    }                
                }
                canvas.Save();
            }
        }
    }
}
