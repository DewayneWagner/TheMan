using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using static TheManXS.ViewModel.FinancialVM.Financials.FinancialsVM;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.FinancialVM.Financials
{
    class CalculatedFinancialValuesList : List<CalculatedFinancialValues>
    {
        Game _game;
        List<FinancialsLineItems> _financialsLineItemsList;
        public CalculatedFinancialValuesList(Game game, List<FinancialsLineItems> financialsLineItemsList)
        {
            _game = (Game)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.Game)];
            _financialsLineItemsList = financialsLineItemsList;
            InitList();
            AssignValuesToFinancialLineItemsArrays();
        }
        void InitList()
        {
            for (int i = 0; i < QC.PlayerQ; i++)
            {
                Add(new CalculatedFinancialValues(_game, _game.PlayerList[i]));
            }
        }
        void AssignValuesToFinancialLineItemsArrays()
        {
            for (int i = 0; i < _financialsLineItemsList.Count; i++)
            {
                _financialsLineItemsList[(int)LineItemType.CAPEXCosts].ValuesArray[i] = this[i].CAPEXThisTurn.ToString("C0");
                _financialsLineItemsList[(int)LineItemType.Cash].ValuesArray[i] = this[i].Cash.ToString("c0");
                _financialsLineItemsList[(int)LineItemType.DebtPayment].ValuesArray[i] = this[i].DebtPayment.ToString("c0");
                _financialsLineItemsList[(int)LineItemType.GrossProfitD].ValuesArray[i] = this[i].GrossProfitD.ToString("c0");
                _financialsLineItemsList[(int)LineItemType.GrossProfitP].ValuesArray[i] = this[i].GrossProfitP.ToString("p1");
                _financialsLineItemsList[(int)LineItemType.InterestExpense].ValuesArray[i] = this[i].InterestExpense.ToString("c0");
                _financialsLineItemsList[(int)LineItemType.LongTermDebt].ValuesArray[i] = this[i].Debt.ToString("c0");
                _financialsLineItemsList[(int)LineItemType.NetProfitD].ValuesArray[i] = this[i].NetProfitD.ToString("c0");
                _financialsLineItemsList[(int)LineItemType.NetProfitP].ValuesArray[i] = this[i].NetProfitP.ToString("p0");
                _financialsLineItemsList[(int)LineItemType.OPEX].ValuesArray[i] = this[i].TotalOPEX.ToString("c0");
                _financialsLineItemsList[(int)LineItemType.PPE].ValuesArray[i] = this[i].PPE.ToString("c0");
                _financialsLineItemsList[(int)LineItemType.Revenue].ValuesArray[i] = this[i].Revenue.ToString("c0");
                _financialsLineItemsList[(int)LineItemType.TheManCut].ValuesArray[i] = this[i].TheManCut.ToString("c0");
                _financialsLineItemsList[(int)LineItemType.TotalAssets].ValuesArray[i] = this[i].TotalAssets.ToString("c0");
                _financialsLineItemsList[(int)LineItemType.TotalCapital].ValuesArray[i] = this[i].TotalCapital.ToString("c0");
            }
        }
    }
}
