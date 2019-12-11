using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using Xamarin.Forms;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.Tiles
{
    public class Tile : AbsoluteLayout
    {
        private GameBoardVM g;
        public Tile() { OverlayGrid = new OverlayGrid(); }
        public Tile(SQ sq, int sqSize)
        {
            g = (GameBoardVM)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.GameBoardVM)];
            SQ = sq;

            CompressedLayout.SetIsHeadless(this, true);     

            Terrain = AllImages.GetTerrainImage(sq.TerrainType);
            Rectangle rect = new Rectangle(0,0, sqSize,sqSize);
            Children.Add(Terrain, rect);

            OverlayGrid = new OverlayGrid(sq);
            Children.Add(OverlayGrid, rect);

            Row = sq.Row;
            Col = sq.Col;
            
            XCoord = Col * sqSize;
            YCoord = Row * sqSize;
            PositionRectangle = new Rectangle(XCoord, YCoord, sqSize, sqSize);

            sq.Tile = this;

            InitEventHandlers();
        }
        public Image Terrain { get; set; }
        public int MapIndex { get; set; }
        public OverlayGrid OverlayGrid { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public double XCoord { get; set; }
        public double YCoord { get; set; }
        public Rectangle PositionRectangle { get; set; }
        public GestureHandlers GestureHandlers { get; set; }
        public SQ SQ { get; set; }

        public event EventHandler SingleTapped;
        public event EventHandler DoubleTapped;
        //public event EventHandler LongHold;

        private void InitEventHandlers()
        {
            TapGestureRecognizer doubleTap = new TapGestureRecognizer();
            doubleTap.NumberOfTapsRequired = 2;
            doubleTap.Tapped += (sender, args) =>
            {
                DoubleTapped?.Invoke(this, EventArgs.Empty);
            };
            OverlayGrid.GestureRecognizers.Add(doubleTap);

            TapGestureRecognizer singleTap = new TapGestureRecognizer();
            singleTap.NumberOfTapsRequired = 1;
            singleTap.Tapped += (sender, args) =>
            {
                SingleTapped?.Invoke(this, EventArgs.Empty);
            };
            OverlayGrid.GestureRecognizers.Add(singleTap);

            GestureHandlers = new GestureHandlers(this);

            GestureHandlers = new GestureHandlers(this);
            SingleTapped += GestureHandlers.SingleTap;
            DoubleTapped += GestureHandlers.DoubleTap;
        }
    }
}
