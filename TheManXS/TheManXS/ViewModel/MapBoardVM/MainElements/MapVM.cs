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
using static TheManXS.Model.Settings.SettingsMaster;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.MainElements
{
    public class MapVM : BaseViewModel
    {
        private SKPaint tile = new SKPaint() { Style = SKPaintStyle.StrokeAndFill };
        private System.Random rnd = new System.Random();

        public MapVM(bool isForAppDictionary) { }

        public MapVM()
        {
            GameBoardVM g = (GameBoardVM)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.GameBoardVM)];
            g.MapVM = this;
            LoadDictionaries();
            QC.IsNewGame = true;

            if (QC.IsNewGame) 
            {
                Map = new SKBitmap((QC.SqSize * QC.ColQ), (QC.SqSize * QC.RowQ));                
                new Map(this);
                InfrastructureBuilder = new Infrastructure.Builder(this);
            }
            else { LoadMap(); }

            MapTouchList = new MapTouchListOfMapTouchIDLists();
            MapMatrix = SKMatrix.MakeIdentity();
            SqAttributesList = new SqAttributesList(this);
            TouchEffectsEnabled = true;
        }

        private SKBitmap _map;
        public SKBitmap Map
        {
            get => _map;
            set
            {
                _map = value;
                SetValue(ref _map, value);
            }
        }

        public MapTouchListOfMapTouchIDLists MapTouchList { get; set; }
        public SQ ActiveSQ { get; set; }
        public Unit ActiveUnit { get; set; }
        public SKCanvasView MapCanvasView { get; set; }
        public StockTickerBarVM StockTicker { get; set; }
        public SqAttributesList SqAttributesList { get; set; }
        public Infrastructure.Builder InfrastructureBuilder { get; set; }
        public Dictionary<int, SQ> SquareDictionary { get; set; } = new Dictionary<int, SQ>();
        public Dictionary<int, Player> PlayerDictionary { get; set; } = new Dictionary<int, Player>();
        
        public SKMatrix MapMatrix;
        public bool TouchEffectsEnabled { get; set; }
        public MapBoard MapBoard { get; set; } = new MapBoard(true);

        // copied from gameboardsplitscreengrid class - not sure if these will be needed?
        public bool SideSQActionPanelExists { get; set; }
        public bool UnitActionPanelExists { get; set; }
        public bool IsThereActiveUnit { get; set; }

        private void LoadDictionaries()
        {
            using (DBContext db = new DBContext())
            {
                SquareDictionary = db.SQ.ToDictionary(sq => sq.Key);
                PlayerDictionary = db.Player.ToDictionary(p => p.Key);
            }
        }
        // from gameboardsplitscreengrid class
        public void AddSideActionPanel(ActionPanelGrid.PanelType pt)
        {
            //if (!SideSQActionPanelExists || !UnitActionPanelExists)
            //{
            //    ActionPanel = new ActionPanel(pt, this);
            //    ColumnDefinitions.Add(new ColumnDefinition() { Width = QC.ScreenWidth * QC.WidthOfActionPaneRatioOfScreenSize });
            //    Children.Add(ActionPanel, 1, 0);
            //    HorizontalOptions = LayoutOptions.End;

            //    if (pt == ActionPanel.PanelType.SQ) { SideSQActionPanelExists = true; }
            //    else if (pt == ActionPanel.PanelType.Unit) { UnitActionPanelExists = true; }
            //}
        }

        private async void SaveMap()
        {
            /***********************
             * using (Stream s = await file.OpenAsync(FileAccess.ReadAndWrite)) {
    SKData d = SKImage.FromBitmap(bitmap).Encode(SKEncodedImageFormat.Png, 100);
    d.SaveTo(s);
             * ***************************/



        }

        private void LoadMap() { }
    }   
    
}
