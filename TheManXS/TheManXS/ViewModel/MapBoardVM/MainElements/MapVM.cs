using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Services.EntityFrameWork;
using TheManXS.Model.Units;
using TheManXS.View;
using TheManXS.ViewModel.MapBoardVM.Action;
using TheManXS.ViewModel.MapBoardVM.MapConstruct;
using TheManXS.ViewModel.MapBoardVM.TouchTracking;
using TheManXS.ViewModel.Services;
using Xamarin.Forms;
using TheManXS.Model.ParametersForGame;
using QC = TheManXS.Model.Settings.QuickConstants;
using TheManXS.Services.IO;
using TheManXS.ViewModel.MapBoardVM.SKGraphics.Structures;
using TheManXS.ViewModel.MapBoardVM.SKGraphics.Nature;
using TheManXS.ViewModel.MapBoardVM.SKGraphics.Borders;
using TheManXS.ViewModel.MapBoardVM.SKGraphics.Nature.Forest;

namespace TheManXS.ViewModel.MapBoardVM.MainElements
{
    public class MapVM : BaseViewModel
    {
        private SKPaint tile = new SKPaint() { Style = SKPaintStyle.StrokeAndFill };
        private System.Random rnd = new System.Random();
        Game _game;

        public MapVM(bool isForAppDictionary) { }

        public MapVM(Game game)
        {
            _game = game;
            _game.GameBoardVM.MapVM = this;
            QC.IsNewGame = true;
            CompressedLayout.SetIsHeadless(this, true);

            if (QC.IsNewGame) { CreateNewMap(); }
            else { LoadMap(); }

            MapTouchList = new MapTouchListOfMapTouchIDLists();
            MapMatrix = SKMatrix.MakeIdentity();
            SqAttributesList = new SqAttributesList(this);
        }

        private SKBitmap _map;
        public SKBitmap SKBitMapOfMap
        {
            get => _map;
            set
            {
                _map = value;
                SetValue(ref _map, value);
            }
        }

        public MapTouchListOfMapTouchIDLists MapTouchList { get; set; }
        public SKCanvasView MapCanvasView { get; set; }
        public SqAttributesList SqAttributesList { get; set; }
        public Infrastructure.Builder InfrastructureBuilder { get; set; }

        public SKMatrix MapMatrix;

        private void CreateNewMap()
        {
            SKBitMapOfMap = new SKBitmap((QC.SqSize * QC.ColQ), (QC.SqSize * QC.RowQ));
            new Map(_game);
            InitTrees();
            InitMountains();
            InfrastructureBuilder = new Infrastructure.Builder(this);
            new SavedMap(_game).SaveMap();            
            InitMineShafts();
            InitCity();
            InitPumpJacks();
            new StartingBorders(_game);
        }

        private void LoadMap() => this.SKBitMapOfMap = new SavedMap(_game).LoadMap();

        private void InitMineShafts()
        {
            var _sqsOwnedByPlayers = _game.SquareDictionary
                                        .Where(s => s.Value.Status == StatusTypeE.Producing)
                                        .Where(s => s.Value.OwnerNumber != QC.PlayerIndexTheMan)
                                        .Where(s => s.Value.TerrainType != TerrainTypeE.City)
                                        .Where(s => s.Value.ResourceType != ResourceTypeE.Oil)
                                        .ToList();

            foreach (KeyValuePair<int,SQ> unit in _sqsOwnedByPlayers)
            {
                new MineShaft(_game, unit.Value);
            }
        }
        private void InitCity()
        {
            var _citySQs = _game.SquareDictionary
                                .Where(s => s.Value.TerrainType == TerrainTypeE.City)
                                .ToList();

            foreach (KeyValuePair<int,SQ> keyValuePair in _citySQs)
            {
                new LowDensity(_game, keyValuePair.Value);
            }
        }
        private void InitTrees()
        {
            var _forestSQs = _game.SquareDictionary
                .Where(s => s.Value.TerrainType == TerrainTypeE.Forest)
                .ToList();

            ForestConstants fc = new ForestConstants();
            List<SpruceTree> listOfTrees = new List<SpruceTree>();

            foreach (KeyValuePair<int,SQ> item in _forestSQs)
            {
                float startOfRectFromSides = (ForestConstants.WidthVsHeightRatio / 2 * QC.SqSize);

                float left = item.Value.Col * QC.SqSize + startOfRectFromSides;
                float top = item.Value.Row * QC.SqSize;
                float right = (item.Value.Col + 1) * QC.SqSize - startOfRectFromSides;
                float bottom = (item.Value.Row + 1) * QC.SqSize;

                SKRect rect = new SKRect(left, top, right, bottom);
                SKColor treeBranchColor = _game.PaletteColors.GetRandomColor(TerrainTypeE.Forest);
                SKColor trunkColor = SKColors.Brown;

                listOfTrees.Add(new SpruceTree(rect, treeBranchColor));
            }

            using (SKCanvas canvas = new SKCanvas(_game.GameBoardVM.MapVM.SKBitMapOfMap))
            {
                foreach (SpruceTree tree in listOfTrees)
                {
                    canvas.DrawPath(tree.TreeBranchesPath, tree.FillPaint);
                    canvas.DrawPath(tree.TreeBranchesPath, tree.StrokePaint);
                    canvas.DrawRect(tree.TreeTrunkRect, tree.TreeTrunkFill);
                    canvas.DrawRect(tree.TreeTrunkRect, tree.StrokePaint);
                    canvas.Save();
                }
            }
        }
        private void InitTrees(bool oldVersion)
        {
            var _forestSQs = _game.SquareDictionary
                .Where(s => s.Value.TerrainType == TerrainTypeE.Forest)
                .ToList();

            foreach (KeyValuePair<int,SQ> item in _forestSQs)
            {
                int halfSQSize = QC.SqSize / 2;

                float left = item.Value.Col * QC.SqSize;
                float top = item.Value.Row * QC.SqSize;
                float right = (item.Value.Col + 1) * QC.SqSize;
                float bottom = (item.Value.Row + 1) * QC.SqSize;

                var s = item.Value;

                new Tree(_game, new SKRect(left, top, (left + halfSQSize), (top + halfSQSize)));
                new Tree(_game, new SKRect((left + halfSQSize), top, right, (top + halfSQSize)));
                new Tree(_game, new SKRect(left, (top + halfSQSize), (left + halfSQSize), bottom));
                new Tree(_game, new SKRect((left + halfSQSize), (top + halfSQSize), right, bottom));
            }
        }
        private void InitMountains()
        {
            var _mountainSQs = _game.SquareDictionary
                .Where(s => s.Value.TerrainType == TerrainTypeE.Mountain)
                .ToList();

            foreach (KeyValuePair<int,SQ> item in _mountainSQs)
            {
                new Mountain(_game, item.Value);
            }
        }
        private void InitPumpJacks()
        {
            var oilProducingSQs = _game.SquareDictionary
                .Where(s => s.Value.Status == StatusTypeE.Producing)
                .Where(s => s.Value.ResourceType == ResourceTypeE.Oil)
                .ToList();

            foreach (KeyValuePair<int,SQ> item in oilProducingSQs)
            {
                new PumpJack(_game, item.Value);
            }
        }        
    }   
}
