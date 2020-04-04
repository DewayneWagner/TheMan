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

        // copied from gameboardsplitscreengrid class - not sure if these will be needed?
        

        private void CreateNewMap()
        {
            SKBitMapOfMap = new SKBitmap((QC.SqSize * QC.ColQ), (QC.SqSize * QC.RowQ));
            new Map(_game);
            InfrastructureBuilder = new Infrastructure.Builder(this);
            new SavedMap(_game).SaveMap();
        }

        private void LoadMap() => this.SKBitMapOfMap = new SavedMap(_game).LoadMap();
    }   
}
