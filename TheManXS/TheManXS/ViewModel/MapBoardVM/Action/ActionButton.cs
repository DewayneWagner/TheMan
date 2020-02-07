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
        ActionPanel _actionPanel;
        SQ _activeSQ;

        public ActionButton(ActionPanel a)
        {
            _actionPanel = a;
            _activeSQ = (SQ)Application.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.ActiveSQ)];

            Text = _activeSQ.NextActionText;
            HorizontalOptions = LayoutOptions.CenterAndExpand;
            BackgroundColor = Color.Crimson;
            FontAttributes = FontAttributes.Bold;
            TextColor = Color.White;

            WidthRequest = (QC.ScreenWidth * QC.WidthOfActionPaneRatioOfScreenSize) * 0.8;

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
