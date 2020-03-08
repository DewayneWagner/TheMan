using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.ViewModel.Services;
using static TheManXS.ViewModel.MapBoardVM.Action.ActionPanelGrid;
using RT = TheManXS.Model.ParametersForGame.ResourceTypeE;
using ST = TheManXS.Model.ParametersForGame.StatusTypeE;

namespace TheManXS.ViewModel.MapBoardVM.Action.ActionExecution
{    
    class ExploreAction
    {
        Game _game;
        PageService _pageServices;
        SQ _activeSQ;
        // can only explore SQ - not Unit
        public ExploreAction(Game game)
        {
            _game = game;
            _activeSQ = _game.ActiveSQ;
            ExecuteAction();
        }
        async void ExecuteAction()
        {
            string message = null;
            if (_activeSQ.ResourceType != RT.Nada || _activeSQ.ResourceType != RT.RealEstate)
            {
                message += "You have discovered " + Convert.ToString(_activeSQ.ResourceType);
                _activeSQ.Status = ST.Explored;
            }
            else 
            {
                message += "Nothing here!";
                _activeSQ.Status = ST.Explored;
            }
            await _pageServices.DisplayAlert("Exploration Results", message);
        }
    }
}
