using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.ViewModel.MapBoardVM.Action;
using Xamarin.Forms;

namespace TheManXS.ViewModel.MapBoardVM.MainElements
{
    public class GameBoardGrid : Grid
    {
        GameBoardVM _gameBoardVM;
        public GameBoardGrid(GameBoardVM gameBoardVM)
        {
            _gameBoardVM = gameBoardVM;

            //HorizontalOptions = LayoutOptions.FillAndExpand;
            //VerticalOptions = LayoutOptions.FillAndExpand;
        }

        private ActionPanelGrid _actionPanelGrid;
        public ActionPanelGrid ActionPanelGrid
        {
            get => _actionPanelGrid;
            set
            {
                _actionPanelGrid = value;
                _gameBoardVM.SetValue(ref _actionPanelGrid, value);
            }
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

        public void AddSidePanel()
        {
            this.ColumnDefinitions.Add(new ColumnDefinition());
            //{
            //    Width = new GridLength(100, GridUnitType.Absolute),
            //});

            BoxView bv = new BoxView()
            {
                Color = Color.Red,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
            };

            this.Children.Add(bv, 1, 0);
            
            //_gameBoardVM.GameBoardGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
            //_gameBoardVM.GameBoardGrid.ActionPanelGrid = new ActionPanelGrid(ActionPanelGrid.PanelType.SQ, _gameBoardVM.GameBoardGrid.MapVM);
            //_gameBoardVM.GameBoardGrid.Children.Add(_gameBoardVM.GameBoardGrid.ActionPanelGrid, 1, 0);
            //_gameBoardVM.SideSQActionPanelExists = true;
        }
    }
}
