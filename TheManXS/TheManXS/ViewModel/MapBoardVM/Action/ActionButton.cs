using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.ViewModel.MapBoardVM.MainElements;
using TheManXS.ViewModel.Services;
using Xamarin.Forms;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.Action
{
    class ActionButton : Button
    {
        PageService _pageServices;
        GameBoardVM _gameBoardVM;

        public ActionButton(GameBoardVM gameBoardVM, ActionPanelGrid.PanelType pt)
        {
            setPropertiesOfButton();
            _gameBoardVM = gameBoardVM;
            Text = GetNextActionText(pt);
            _pageServices = new PageService();
            Clicked += ExecuteAction;
        }
        private void setPropertiesOfButton()
        {
            // button size, margin, and height set in ActionPanelGrid class
            HorizontalOptions = LayoutOptions.CenterAndExpand;
            BackgroundColor = Color.Crimson;
            FontAttributes = FontAttributes.Bold;
            TextColor = Color.White;
            WidthRequest = QC.WidthOfActionPanel * 0.8;
        }
        private string GetNextActionText(ActionPanelGrid.PanelType pt)
        {
            if (pt == ActionPanelGrid.PanelType.SQ) 
                { return _gameBoardVM.MapVM.ActiveSQ.NextActionText; }
            else { return _gameBoardVM.MapVM.ActiveUnit[0].NextActionText; }
        }
        public async void ExecuteAction(object sender, EventArgs e)
        {
            IsEnabled = false;
            await _pageServices.DisplayAlert("stuff has been done");
        }
        private void BuyAction(SQ sq)
        {

        }
        private void ExploreAction(SQ sq)
        {

        }
        private void DevelopAction(SQ sq)
        {

        }
        private void SuspendAction(SQ sq)
        {

        }
        private void ReclaimAction(SQ sq)
        {

        }
        private void ReActivateAction(SQ sq)
        {

        }
    }
}
