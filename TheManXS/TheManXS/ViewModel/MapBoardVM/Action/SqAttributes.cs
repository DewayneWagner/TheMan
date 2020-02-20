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
        MapVM _mapVM;
        public enum AllSQAttributes { Owner, Status, Resource, Production, Revenue, OPEX, TransportCost, 
            GrossProfitD, GrossProfitP, ActionCost, Total }

        private Cash _cash;
        public SqAttributes(MapVM mapVM) 
        { 
            _mapVM = mapVM;
            _cash = new Calculations().GetCash(_mapVM.ActiveSQ);
        }

        public ST StatusType { get; set; }
        public AllSQAttributes SqAttribute { get; set; }
        public bool IsVisible { get; set; }

        public string GetValue(AllSQAttributes sqAttribute)
        {
            switch (sqAttribute)
            {
                case AllSQAttributes.Owner:
                    return _mapVM.ActiveSQ.OwnerName;
                case AllSQAttributes.Status:
                    return Convert.ToString(StatusType);
                case AllSQAttributes.Resource:
                    return Convert.ToString(_mapVM.ActiveSQ.ResourceType);
                case AllSQAttributes.Production:
                    return Convert.ToString(_mapVM.ActiveSQ.Production);
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
                    return _mapVM.ActiveSQ.NextActionCost.ToString("C0");
                default:
                    return null;
            }
        }
    }
}
