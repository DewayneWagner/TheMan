using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Units;
using TheManXS.ViewModel.MapBoardVM.Action;
using TheManXS.ViewModel.MapBoardVM.MainElements;
using TheManXS.ViewModel.MapBoardVM.Tiles;
using Xamarin.Forms;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.MainElements
{
    public class GameBoardSplitScreenGrid : Grid
    {
        private GameBoardVM _gameBoardVM;
        public GameBoardSplitScreenGrid()
        {
            GameBoardVM g = (GameBoardVM)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.GameBoardVM)];
            _gameBoardVM = g;
            g.GameBoardSplitScreenGrid = this;
        }

        private MapVM _mapVM;
        public MapVM MapVM
        {
            get => _mapVM;
            set
            {
                _mapVM = value;
                _gameBoardVM.SetValue(ref _mapVM, value);
            }
        }

        private ActionPanel _actionPanel;
        public ActionPanel ActionPanel
        {
            get => _actionPanel;
            set
            {
                _actionPanel = value;
                _gameBoardVM.SetValue(ref _actionPanel, value);
            }
        }

        public bool SideSQActionPanelExists { get; set; }
        public bool UnitActionPanelExists { get; set; }
        
        public Unit ActiveUnit { get; set; }
        public bool IsThereActiveUnit { get; set; }

        public void AddSideActionPanel(ActionPanel.PanelType pt)
        {
            if (!SideSQActionPanelExists || !UnitActionPanelExists)
            {
                ActionPanel = new ActionPanel(pt,this);
                ColumnDefinitions.Add(new ColumnDefinition() { Width = QC.ScreenWidth * QC.WidthOfActionPaneRatioOfScreenSize });
                Children.Add(ActionPanel, 1, 0);
                HorizontalOptions = LayoutOptions.End;

                if (pt == ActionPanel.PanelType.SQ) { SideSQActionPanelExists = true; }
                else if (pt == ActionPanel.PanelType.Unit) { UnitActionPanelExists = true; }
            }
        }
    }
}
