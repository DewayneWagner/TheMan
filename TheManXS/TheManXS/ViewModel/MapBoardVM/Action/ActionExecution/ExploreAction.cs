using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.ViewModel.MapBoardVM.SKGraphics.Structures;
using TheManXS.ViewModel.Services;
using Xamarin.Forms;
using static TheManXS.ViewModel.MapBoardVM.Action.ActionPanelGrid;
using RT = TheManXS.Model.ParametersForGame.ResourceTypeE;
using ST = TheManXS.Model.ParametersForGame.StatusTypeE;
using System.Windows;

namespace TheManXS.ViewModel.MapBoardVM.Action.ActionExecution
{    
    class ExploreAction : SQAction
    {
        // can only explore SQ - not Unit
        public ExploreAction(Game game) : base(game)
        {
            DisplayTestAlert();
            //Application.Current.MainPage.DisplayAlert("test", "test", "test");
            //ExecuteAction();            
        }
        private void ExecuteAction()
        {
            var s = Game.ActiveSQ;
            var p = Game.ActivePlayer;

            if(s.OwnerNumber == p.Number)
            {
                s.Status = ST.Explored;
                p.Cash -= s.NextActionCost;

                if (s.ResourceType != RT.Nada || s.ResourceType != RT.RealEstate)
                {
                    DisplayAlertAboutSuccessfulExploration(Convert.ToString(s.ResourceType));
                }
                else
                {                    
                    new DudSymbol(Game, s);
                }
            }
            else { DisplayAlertAboutNonOwner(); }
        }
        private async void DisplayAlertAboutNonOwner() => await PageServices.DisplayAlert("Heh Dumbass - you can only explore Squares you own.");
        private async void DisplayAlertAboutSuccessfulExploration(string resourceType) => await PageServices.DisplayAlert("Congrats!  You found " + resourceType + "!!!");
        //private async void DisplayTestAlert() => await PageServices.DisplayAlert("Test");
        private async void DisplayTestAlert() => await App.Current.MainPage.DisplayAlert("test", "test", "test");
    }
}
