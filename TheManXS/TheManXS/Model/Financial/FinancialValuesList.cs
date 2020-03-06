using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Services.EntityFrameWork;
using TheManXS.ViewModel.FinancialVM.Financials;
using static TheManXS.ViewModel.FinancialVM.Financials.FinancialsVM;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.Model.Financial
{
    public class FinancialValuesList : List<FinancialValues>
    {
        Game _game;

        public FinancialValuesList(Game game)
        {
            _game = (Game)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.Game)];
            InitList();
            WriteListToDBAfterFullTurnComplete();
        }

        void InitList()
        {
            for (int i = 0; i < QC.PlayerQ; i++)
            {
                Add(new FinancialValues(_game, _game.PlayerList[i]));
            }
        }

        public void AssignValuesToFinancialLineItemsArrays(FinancialsLineItems[] financialsLineItemsArray)
        {
            for (int i = 0; i < FinancialsVM.QDATACOLUMNS; i++)
            {
                financialsLineItemsArray[(int)LineItemType.CAPEXCosts].ValuesArray[i] = this[i].CAPEXThisTurn.ToString("C0");
                financialsLineItemsArray[(int)LineItemType.Cash].ValuesArray[i] = this[i].Cash.ToString("c0");
                financialsLineItemsArray[(int)LineItemType.DebtPayment].ValuesArray[i] = this[i].DebtPayment.ToString("c0");
                financialsLineItemsArray[(int)LineItemType.GrossProfitD].ValuesArray[i] = this[i].GrossProfitD.ToString("c0");
                financialsLineItemsArray[(int)LineItemType.GrossProfitP].ValuesArray[i] = this[i].GrossProfitP.ToString("p1");
                financialsLineItemsArray[(int)LineItemType.InterestExpense].ValuesArray[i] = this[i].InterestExpense.ToString("c0");
                financialsLineItemsArray[(int)LineItemType.LongTermDebt].ValuesArray[i] = this[i].Debt.ToString("c0");
                financialsLineItemsArray[(int)LineItemType.NetProfitD].ValuesArray[i] = this[i].NetProfitD.ToString("c0");
                financialsLineItemsArray[(int)LineItemType.NetProfitP].ValuesArray[i] = this[i].NetProfitP.ToString("p0");
                financialsLineItemsArray[(int)LineItemType.OPEX].ValuesArray[i] = this[i].TotalOPEX.ToString("c0");
                financialsLineItemsArray[(int)LineItemType.PPE].ValuesArray[i] = this[i].PPE.ToString("c0");
                financialsLineItemsArray[(int)LineItemType.Revenue].ValuesArray[i] = this[i].Revenue.ToString("c0");
                financialsLineItemsArray[(int)LineItemType.TheManCut].ValuesArray[i] = this[i].TheManCut.ToString("c0");
                financialsLineItemsArray[(int)LineItemType.TotalAssets].ValuesArray[i] = this[i].TotalAssets.ToString("c0");
                financialsLineItemsArray[(int)LineItemType.TotalCapital].ValuesArray[i] = this[i].TotalCapital.ToString("c0");
                financialsLineItemsArray[(int)LineItemType.CreditRating].ValuesArray[i] = this[i].CreditRating;
                financialsLineItemsArray[(int)LineItemType.InterestRate].ValuesArray[i] = this[i].InterestRate.ToString("p0");
                financialsLineItemsArray[(int)LineItemType.StockPrice].ValuesArray[i] = this[i].StockPrice.ToString("c2");
            }
        }

        private void WriteListToDBAfterFullTurnComplete()
        {
            using (DBContext db = new DBContext())
            {
                db.FinancialValues.UpdateRange(this);
                db.SaveChanges();
            }            
        }
    }
}
