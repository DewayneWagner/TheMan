using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Units;
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
        public bool SideSQActionPanelExists { get; set; }
        public bool UnitActionPanelExists { get; set; }
        public ActionPanel ActionPanel { get; set; }
        public Unit ActiveUnit { get; set; }
        public bool IsThereActiveUnit { get; set; }

        void InitGrid()
        {
            ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
            RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star });
        }
        public void AddSideActionPanel(ActionPanel.PanelType pt, Tile tile)
        {
            if(!SideSQActionPanelExists || !UnitActionPanelExists)
            {
                ActionPanel = new ActionPanel(pt);
                ColumnDefinitions.Add(new ColumnDefinition() { Width = QC.ScreenWidth * QC.WidthOfActionPaneRatioOfScreenSize });
                Children.Add(ActionPanel, 1, 0);
                HorizontalOptions = LayoutOptions.End;

                if (pt == ActionPanel.PanelType.SQ)
                {
                    SideSQActionPanelExists = true;
                    tile.OverlayGrid.SetColorsOfAllSides(Color.Red);
                }
                else if (pt == ActionPanel.PanelType.Unit)
                {
                    UnitActionPanelExists = true;
                }
            }
        }        
    }
}
