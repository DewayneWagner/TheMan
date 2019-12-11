using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using TheManXS.Model.Main;
using TheManXS.Model.Map.Surface;
using TheManXS.Model.Services.EntityFrameWork;
using TheManXS.ViewModel.MapBoardVM.Scroll;
using TheManXS.ViewModel.MapBoardVM.MainElements;
using TheManXS.ViewModel.MapBoardVM.Tiles;
using Xamarin.Forms;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.Scroll
{
    public class FocusedABS : ObservableCollection<Tile>
    {
        private MapScrollView _mapVM;
        private Segment Segment;

        public FocusedABS(ActualGameBoardVM a)
        {
            _mapVM = a.GameBoardSplitScreenGrid.MapScrollView;
            Row = new Dimensions(true, 0, 0);
            Col = new Dimensions(false, 0, 0);

            //GameBoardVM g = (GameBoardVM)Application.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.GameBoardVM)];
            //_mapVM = g.ActualGameBoardVM.GameBoardSplitScreenGrid.MapScrollView;

            InitOC();
        }      

        public Dimensions Row { get; set; }
        public Dimensions Col { get; set; }        
        
        private void InitOC()
        {
            using (DBContext db = new DBContext())
            {
                for (int row = 0; row < Row.Quantity; row++)
                {
                    for (int col = 0; col < Col.Quantity; col++)
                    {
                        int key = Coordinate.GetSQKey(row, col);
                        SQ sq = db.SQ.Find(key);
                        this.Add(new Tile(sq,QC.SqSize));
                    }
                }
            }
        }
        public void ExecuteShift(Segment segment)
        {            
            Segment = segment;

            AddTile();
            RemoveTile();
            UpdateFocusedOCDimensions();
        }
        private void AddTile()
        {
            foreach (Tile t in Segment.TilesToAdd)
            {
                this.Add(t);
                var m = _mapVM.PinchToZoomContainer.GameBoard;
                //m.Children.Add(t, t.PositionRectangle);
                m.Children.Add(t, t.Row, t.Col);
                t.MapIndex = m.Children.IndexOf(t);
            }
        }
        private void RemoveTile()
        {
            foreach (Tile t in Segment.TilesToRemove)
            {
                if(t != null)
                {
                    this.Remove(t);
                    _mapVM.PinchToZoomContainer.GameBoard.Children.Remove(t);
                }
            }
        }
        private void UpdateFocusedOCDimensions()
        {
            int v = Segment.IsVerticalShift ? 1 : 0;
            int h = v == 1 ? 0 : 1;
            int direction = Segment.IsPositiveShift ? 1 : (-1);

            Row.Start += v * direction;
            Row.End += v * direction;

            Col.Start += h * direction;
            Col.End += h * direction;
        }
        public Tile GetTile(int row, int col) => this.Where(t => t.Row == row).Where(t => t.Col == col).FirstOrDefault();
    }
}
