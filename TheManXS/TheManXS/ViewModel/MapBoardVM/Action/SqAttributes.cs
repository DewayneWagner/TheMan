using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using ST = TheManXS.Model.Settings.SettingsMaster.StatusTypeE;
using Xamarin.Forms;
using TheManXS.Services;
using TheManXS.Model.Financial;
using TheManXS.ViewModel.MapBoardVM.MainElements;

namespace TheManXS.ViewModel.MapBoardVM.Action
{
    public class SqAttributes
    {
        Game _game;
        public enum AllSQAttributes { Owner, Status, Resource, Production, Revenue, OPEX, TransportCost, 
            GrossProfitD, GrossProfitP, ActionCost, Total }

        private Cash _cash;

        public SqAttributes() { }
        public SqAttributes(Game game) 
        {
            _game = game;
            _cash = new Calculations().GetCash(_game.ActiveSQ);
        }

        public ST StatusType { get; set; }
        public AllSQAttributes SqAttribute { get; set; }
        public bool IsVisible { get; set; }

        public string GetValue(AllSQAttributes sqAttribute)
        {
            switch (sqAttribute)
            {
                case AllSQAttributes.Owner:
                    return _game.ActiveSQ.OwnerName;
                case AllSQAttributes.Status:
                    return Convert.ToString(StatusType);
                case AllSQAttributes.Resource:
                    return Convert.ToString(_game.ActiveSQ.ResourceType);
                case AllSQAttributes.Production:
                    return Convert.ToString(_game.ActiveSQ.Production);
                case AllSQAttributes.Revenue:
                    return _cash.Revenue.ToString("C0");
                case AllSQAttributes.OPEX:
                    return _cash.OPEX.ToString("C0");
                case AllSQAttributes.TransportCost:
                    return _cash.Transport.ToString("C0");
                case AllSQAttributes.GrossProfitD:
                    return _cash.ProfitDollar.ToString("C0");
                case AllSQAttributes.GrossProfitP:
                    return _cash.ProfitPercent.ToString("P1");
                case AllSQAttributes.ActionCost:
                    return _game.ActiveSQ.NextActionCost.ToString("C0");
                default:
                    return null;
            }
        }
    }
}
