using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using ST = TheManXS.Model.Settings.SettingsMaster.StatusTypeE;
using Xamarin.Forms;
using TheManXS.Services;
using TheManXS.Model.Financial;

namespace TheManXS.ViewModel.MapBoardVM.Action
{
    public class SqAttributes
    {
        private SQ sq;
        GameBoardVM g;
        public enum AllSQAttributes { Owner, Status, Resource, Production, Revenue, OPEX, TransportCost, 
            GrossProfitD, GrossProfitP, ActionCost, Total }

        public SqAttributes() { }

        public SqAttributes(SQ square, AllSQAttributes attribute)
        {
            g = (GameBoardVM)Application.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.GameBoardVM)];
            sq = square;
            StatusType = square.Status;
            SqAttribute = attribute;
            IsVisible = g.SqAttributesList.GetVisibility(sq.Status, attribute);
            if(IsVisible) { Value = GetValue(); }
        }

        public ST StatusType { get; set; }
        public AllSQAttributes SqAttribute { get; set; }
        public bool IsVisible { get; set; }
        public string Value { get; set; }

        public string GetValue()
        {
            Cash cash = new Calculations().GetCash(sq);

            switch (SqAttribute)
            {
                case AllSQAttributes.Owner:
                    return sq.OwnerName;
                case AllSQAttributes.Status:
                    return Convert.ToString(StatusType);
                case AllSQAttributes.Resource:
                    return Convert.ToString(sq.ResourceType);
                case AllSQAttributes.Production:
                    return Convert.ToString(sq.Production);
                case AllSQAttributes.Revenue:
                    return cash.Revenue.ToString("C0");
                case AllSQAttributes.OPEX:
                    return cash.OPEX.ToString("C0");
                case AllSQAttributes.TransportCost:
                    return cash.Transport.ToString("C0");
                case AllSQAttributes.GrossProfitD:
                    return cash.ProfitDollar.ToString("C0");
                case AllSQAttributes.GrossProfitP:
                    return cash.ProfitPercent.ToString("P1");
                case AllSQAttributes.ActionCost:
                    return sq.NextAction.Cost.ToString("C0");
                default:
                    return null;
            }
        }
    }
}
