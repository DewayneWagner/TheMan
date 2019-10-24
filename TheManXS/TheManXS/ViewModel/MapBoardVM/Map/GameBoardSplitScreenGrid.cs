using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.ViewModel.MapBoardVM.Action;
using TheManXS.ViewModel.MapBoardVM.MainElements;
using TheManXS.ViewModel.MapBoardVM.Tiles;
using Xamarin.Forms;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.Map
{
    public class GameBoardSplitScreenGrid : Grid
    {
        private ActualGameBoardVM _actualGameBoardVM;

        public GameBoardSplitScreenGrid(ActualGameBoardVM a)
        {
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.FillAndExpand;

            CompressedLayout.SetIsHeadless(this, true);
            _actualGameBoardVM = a;
            InitGrid();
            MapScrollView = new MapScrollView(_actualGameBoardVM);
            Children.Add(MapScrollView, 0, 0);
        }

        public MapScrollView MapScrollView { get; set; }
        public bool SidePanelExists { get; set; }
        public ActionPanel ActionPanel { get; set; }

        void InitGrid()
        {
            ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
            RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star });
        }

        public void AddSideActionPanel(Tile tile)
        {
            if (!SidePanelExists)
            {
                ActionPanel = new ActionPanel();
                ColumnDefinitions.Add(new ColumnDefinition() { Width = QC.ScreenWidth * QC.WidthOfActionPaneRatioOfScreenSize });
                Children.Add(ActionPanel, 1, 0);
                HorizontalOptions = LayoutOptions.End;
                SidePanelExists = true;
                tile.OverlayGrid.SetColorsOfAllSides(Color.Red);
            }            
        }
    }
}
