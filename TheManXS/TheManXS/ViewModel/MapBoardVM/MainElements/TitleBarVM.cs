using System;
using System.Windows.Input;
using TheManXS.Model.Main;
using TheManXS.View;
using TheManXS.ViewModel.Services;
using Xamarin.Forms;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.MainElements
{
    public enum ViewType { Terrain, Resources, Roads, Pipeline, TrainTracks, Total }
    public class TitleBarVM : BaseViewModel
    {
        private string _quarter;
        private string _companyName;
        private PageService _pageService;
        private Game _game;
        private const double _topToolBarHeightRatio = 0.075;

        public TitleBarVM(bool isForInitGameBoardVM) { }
        public TitleBarVM()
        {
            _game = (Game)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.Game)];
            _game.GameBoardVM.TitleBar = this;
            _pageService = new PageService();

            CompressedLayout.SetIsHeadless(this, true);

            ShowFinancials = new Command(ShowFinancialsPage);
            EndTurn = new Command(EndTurnAction);
            ShowResources = new Command(ShowResourceMap);

            Quarter = _game.Quarter;
            CompanyName = _game.ActivePlayer.Name;
            SetHeight();
        }

        public string Quarter
        {
            get => _quarter;
            set
            {
                _quarter = value;
                SetValue(ref _quarter, value);
            }
        }

        public string CompanyName
        {
            get => _companyName;
            set
            {
                _companyName = value;
                SetValue(ref _companyName, value);
            }
        }

        private string _currentPlayerCash;
        public string CurrentPlayerCash
        {
            get => _currentPlayerCash;
            set
            {
                _currentPlayerCash = value;
                SetValue(ref _currentPlayerCash, value);
            }
        }

        private double _topToolBarHeight;
        public double TopToolBarHeight
        {
            get => _topToolBarHeight;
            set
            {
                _topToolBarHeight = value;
                SetValue(ref _topToolBarHeight, value);
            }
        }

        public ICommand ShowFinancials { get; set; }
        public ICommand EndTurn { get; set; }
        public ICommand ShowResources { get; set; }

        private async void ShowResourceMap() => await _pageService.PushAsync(new PoolBreakdown());
        private void EndTurnAction(object obj) => new EndTurnAction(_game);
        private async void ShowFinancialsPage(object obj) => await _pageService.PushAsync(new FinancialsView());
        private void SetHeight()
        {
            HeightRequest = QC.TopToolBarHeight = TopToolBarHeight = (QC.ScreenHeight * _topToolBarHeightRatio);
        }
    }
}
