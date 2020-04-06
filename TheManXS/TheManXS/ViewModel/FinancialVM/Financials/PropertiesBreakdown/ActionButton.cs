using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.ViewModel.Services;
using Xamarin.Forms;

namespace TheManXS.ViewModel.FinancialVM.Financials.PropertiesBreakdown
{
    public class ActionButton : Button
    {
        Game _game;
        PropertyBreakdown _propertyBreakdown;
        bool _isOwnedByActivePlayer;
        PageService _pageService = new PageService();

        public ActionButton(Game game, PropertyBreakdown propertyBreakdown)
        {
            _game = game;
            _propertyBreakdown = propertyBreakdown;
            _pageService = new PageService();
            _isOwnedByActivePlayer = propertyBreakdown.CompanyName == _game.ActivePlayer.Name ? true : false;
            InitPropertiesOfButton();
            Clicked += ActionButton_Clicked;
        }

        private async void ActionButton_Clicked(object sender, EventArgs e)
        {
            string message = null;
            if (_isOwnedByActivePlayer)
            {
                double askingPrice = _propertyBreakdown.PPE * 1.5;
                message += "Would you like to try to sell this property?" + "\n" +
                    "Current Market Value would be approximately " + "\n" +
                    askingPrice.ToString("c0");
                bool execute = await _pageService.DisplayAlert("Sell this property?", message, "Go For It!", "Nope!");
                if(execute) { ExecuteSale(); }
            }
            else
            {
                double offerPrice = _propertyBreakdown.PPE * 1.5;
                message += "Would you like to make an offer on this property?" + "\n" +
                    "Current Market Value would be approximately " + "\n" +
                    offerPrice.ToString("c0");
                bool execute = await _pageService.DisplayAlert("Make an offer?", message, "Go for it!", "Nope!");
                if (execute) { ExecutePurchase(); }
            }
        }

        void InitPropertiesOfButton()
        {
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.FillAndExpand;
            WidthRequest = 25;
            HeightRequest = 25;
            Margin = 5;
            CornerRadius = 5;
            BorderColor = Color.Black;
            BorderWidth = 2;
            AutomationId = PropertyBreakdownGrid.DataTableAutomationID;

            if (_isOwnedByActivePlayer)
            {
                BackgroundColor = Color.Green;
                Text = "Sell?";
            }
            else
            {
                BackgroundColor = Color.Red;
                Text = "Buy?";
                TextColor = Color.White;
            }
        }
        void ExecutePurchase() { }
        void ExecuteSale() { }
    }
}
