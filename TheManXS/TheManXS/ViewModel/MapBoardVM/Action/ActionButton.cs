﻿using System;
using System.Linq;
using TheManXS.Model.Financial;
using TheManXS.Model.Main;
using TheManXS.ViewModel.MapBoardVM.Action.ActionExecution;
using TheManXS.ViewModel.Services;
using Xamarin.Forms;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.Action
{
    class ActionButton : Button
    {
        Game _game;
        ActionPanelGrid.PanelType _panelType;
        PageService _pageServices;
        bool _additionalDebtApproved;
        public ActionButton(Game game, ActionPanelGrid.PanelType pt)
        {
            _game = game;
            _pageServices = new PageService();
            setPropertiesOfButton();

            _panelType = pt;

            Text = GetNextActionText(pt);
            AssignMethodToButton();
        }
        void setPropertiesOfButton()
        {
            // button size, margin, and height set in ActionPanelGrid class
            HorizontalOptions = LayoutOptions.CenterAndExpand;
            VerticalOptions = LayoutOptions.CenterAndExpand;
            BackgroundColor = _game.PaletteColors.Where(c => c.Description == "Banff 2").Select(c => c.Color).FirstOrDefault();
            TextColor = Color.White;
            WidthRequest = QC.WidthOfActionPanel * 0.8;
            FontAttributes = FontAttributes.Bold;
        }

        string GetNextActionText(ActionPanelGrid.PanelType pt)
        {
            if (pt == ActionPanelGrid.PanelType.SQ)
            { return _game.ActiveSQ.NextActionText; }
            else { return _game.ActiveUnit[0].NextActionText; }
        }

        void AssignMethodToButton()
        {
            NextAction.NextActionType n = _panelType == ActionPanelGrid.PanelType.SQ ? _game.ActiveSQ.NextActionType :
                _game.ActiveUnit.NextActionType;

            switch (n)
            {
                case NextAction.NextActionType.Purchase:
                    Clicked += BuyAction;
                    break;
                case NextAction.NextActionType.Explore:
                    Clicked += ExploreAction;
                    break;
                case NextAction.NextActionType.Develop:
                    Clicked += DevelopAction;
                    break;
                case NextAction.NextActionType.Suspend:
                    Clicked += SuspendAction;
                    break;
                case NextAction.NextActionType.Reactivate:
                    Clicked += ReactivateAction;
                    break;
                case NextAction.NextActionType.NotEnabled:
                default:
                    break;
            }

        }

        void BuyAction(object sender, EventArgs e)
        {
            ProcessCashTransaction();
            if (_additionalDebtApproved) { new BuyAction(_game, _panelType); }
            _game.GameBoardVM.SidePanelManager.ResetSidePanel(_panelType);
        }

        void ExploreAction(object sender, EventArgs e)
        {
            ProcessCashTransaction();
            if (_additionalDebtApproved) { new ExploreAction(_game); }
            _game.GameBoardVM.SidePanelManager.ResetSidePanel(_panelType);
        }

        void DevelopAction(object sender, EventArgs e)
        {
            ProcessCashTransaction();
            if (_additionalDebtApproved) { new DevelopAction(_game, _panelType); }
            _game.GameBoardVM.SidePanelManager.ResetSidePanel(_panelType);
        }

        void SuspendAction(object sender, EventArgs e)
        {
            ProcessCashTransaction();
            if (_additionalDebtApproved) { new SuspendAction(_game, _panelType); }
            _game.GameBoardVM.SidePanelManager.ResetSidePanel(_panelType);
        }

        void ReactivateAction(object sender, EventArgs e)
        {
            ProcessCashTransaction();
            if (_additionalDebtApproved) { new ReActivateAction(_game, _panelType); }
            _game.GameBoardVM.SidePanelManager.ResetSidePanel(_panelType);
        }

        async void ProcessCashTransaction()
        {
            double currentPlayerCash = _game.ActivePlayer.Cash;
            double transactionCost = _panelType == ActionPanelGrid.PanelType.SQ ?
                        _game.ActiveSQ.NextActionCost :
                        _game.ActiveUnit.NextActionCost;
            _additionalDebtApproved = true; // reset to false if it isn't

            if (transactionCost < currentPlayerCash) { _game.ActivePlayer.Cash -= transactionCost; }
            else
            {
                double cashShortfall = transactionCost - currentPlayerCash;
                bool additionalDebtIsApproved = await _pageServices.DisplayAlert("Borrow Cash", "You do not have enough cash to " + "\n"
                    + "to complete this transaction - would you like " + "\n"
                    + "to add " + cashShortfall.ToString("C0") + "to you long-term debt?",
                    "Approved", "Declined");

                if (additionalDebtIsApproved)
                {
                    _game.ActivePlayer.Cash = 0;
                    _game.ActivePlayer.Debt += (transactionCost - currentPlayerCash);
                    _additionalDebtApproved = true;
                }
                else { _additionalDebtApproved = false; }
            }
        }
    }
}
