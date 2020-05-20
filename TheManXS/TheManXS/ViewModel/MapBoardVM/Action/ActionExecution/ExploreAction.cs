using System;
using TheManXS.Model.Main;
using TheManXS.ViewModel.MapBoardVM.SKGraphics.Structures;
using RT = TheManXS.Model.ParametersForGame.ResourceTypeE;
using ST = TheManXS.Model.ParametersForGame.StatusTypeE;

namespace TheManXS.ViewModel.MapBoardVM.Action.ActionExecution
{
    class ExploreAction : SQAction
    {
        // can only explore SQ - not Unit
        public ExploreAction(Game game) : base(game)
        {
            ExecuteAction();
        }
        private void ExecuteAction()
        {
            var s = Game.ActiveSQ;
            var p = Game.ActivePlayer;

            if (s.OwnerNumber == p.Number)
            {
                s.Status = ST.Explored;
                p.Cash -= s.NextActionCost;

                if (s.ResourceType != RT.RealEstate)
                {
                    if (s.ResourceType == RT.Nada)
                    {
                        new DudSymbol(Game, s);
                    }
                    else
                    {
                        DisplayAlertAboutSuccessfulExploration(Convert.ToString(s.ResourceType));
                    }
                }
            }
            else { DisplayAlertAboutNonOwner(); }
        }
        private async void DisplayAlertAboutNonOwner() => await PageServices.DisplayAlert("Heh Dumbass - you can only explore Squares you own.");
        private async void DisplayAlertAboutSuccessfulExploration(string resourceType) => await PageServices.DisplayAlert("Congrats!  You found " + resourceType + "!!!");
    }
}
