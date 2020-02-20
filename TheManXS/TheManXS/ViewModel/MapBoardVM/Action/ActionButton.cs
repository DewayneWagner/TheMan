using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.ViewModel.Services;
using Xamarin.Forms;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.Action
{
    class ActionButton : Button
    {
        PageService _pageServices;
        ActionPanelGrid _actionPanelGrid;
        SQ _activeSQ;

        public ActionButton(ActionPanelGrid actionPanelGrid)
        {
            _actionPanelGrid = actionPanelGrid;
            _activeSQ = (SQ)Application.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.ActiveSQ)];

            Text = _activeSQ.NextActionText;
            HorizontalOptions = LayoutOptions.CenterAndExpand;
            BackgroundColor = Color.Crimson;
            FontAttributes = FontAttributes.Bold;
            TextColor = Color.White;

            WidthRequest = QC.WidthOfActionPanel * 0.8;

            IsEnabled = true;

            _pageServices = new PageService();
            Clicked += ExecuteAction;
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
