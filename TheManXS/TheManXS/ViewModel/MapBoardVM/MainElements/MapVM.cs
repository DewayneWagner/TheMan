﻿using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using TheManXS.Model.Main;
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
using TheManXS.ViewModel.MapBoardVM.Infrastructure;
using TheManXS.ViewModel.MapBoardVM.SKGraphics.Nature.Mountains;

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

        public SKMatrix MapMatrix;

        private void CreateNewMap()
        {
            SKBitMapOfMap = new SKBitmap((QC.SqSize * QC.ColQ), (QC.SqSize * QC.RowQ));
            new Map(_game);
            
            new NewMapInitializer(this);
            new SavedMap(_game).SaveMap();
            
        }

        private void LoadMap() => this.SKBitMapOfMap = new SavedMap(_game).LoadMap();

        private SKPaint OwnedSQsPaint = new SKPaint()
        {
            IsAntialias = true,
            Style = SKPaintStyle.Fill,
        };

        private void InitOwnedSQs()
        {
            var ownedSQs = _game.SquareDictionary
                            .Where(s => s.Value.OwnerNumber != QC.PlayerIndexTheMan)
                            .ToList();

            using (SKCanvas canvas = new SKCanvas(_game.GameBoardVM.MapVM.SKBitMapOfMap))
            {
                foreach (KeyValuePair<int, SQ> sq in ownedSQs)
                {
                    OwnedSQsPaint.Color = _game.PlayerList[sq.Value.OwnerNumber].SKColor.WithAlpha(0x75);
                    canvas.DrawRect(sq.Value.SKRect, OwnedSQsPaint);
                }
                canvas.Save();
            }

            
        }
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
            var forestSQs = _game.SquareDictionary
                .Where(s => s.Value.TerrainType == TerrainTypeE.Forest)
                .ToList();

            new TreesList(forestSQs, SKBitMapOfMap, _game.PaletteColors);
        }
        private void InitMountains() { new MountainRange(_game); }
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
