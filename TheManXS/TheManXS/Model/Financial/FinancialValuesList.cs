using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Services.EntityFrameWork;
using TheManXS.ViewModel.FinancialVM.Financials;
using Windows.UI.Input.Preview.Injection;
using static TheManXS.ViewModel.FinancialVM.Financials.FinancialsVM;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.Model.Financial
{
    public class FinancialValuesList : List<FinancialValues>
    {
        private static Game _game;
        private int _playerNumber;

        public FinancialValuesList(Game game)
        {
            _game = game;
            InitListWithCurrentFinancialsForAllPlayers();
            WriteListToDBAfterFullTurnComplete();
        }
        public FinancialValuesList(Game game, DataPanelType dataPanelType, int playerNum = -1)
        {
            _playerNumber = playerNum == -1 ? QC.PlayerIndexActual : playerNum;
            // this constructor is for creating lists to display
            _game = game;
            if(dataPanelType == DataPanelType.AllPlayers) { InitListWithCurrentFinancialsForAllPlayers(); }
            else { InitListWithQuarterlyFinancialsForSinglePlayer(); }
        }

        public FinancialValuesList(bool isForLoadedGame)
        {
            using (DBContext db = new DBContext())
            {
                var fList = db.FinancialValues.Where(f => f.SavedGameSlot == QC.CurrentSavedGameSlot).ToList();
                foreach(FinancialValues fv in fList) { this.Add(fv); }
            }
        }

        void InitListWithCurrentFinancialsForAllPlayers()
        {
            for (int i = 0; i < QC.PlayerQ; i++)
            {
                Add(new FinancialValues(_game, _game.PlayerList[i]));
            }
        }
        void InitListWithQuarterlyFinancialsForSinglePlayer()
        {
            int firstQuarter = _game.TurnNumber;
            int lastQuarter = _game.TurnNumber - getNumberOfQuarters();

            if (firstQuarter == lastQuarter)
            {
                this.Add(_game.FinancialValuesList[_playerNumber]);
            }
            else
            {
                using (DBContext db = new DBContext())
                {
                    for (int q = firstQuarter; q <= lastQuarter; q--)
                    {
                        var f = db.FinancialValues.Where(v => v.TurnNumber == q)
                                    .Where(v => v.PlayerNumber == _playerNumber)
                                    .FirstOrDefault();
                        this.Add(f);
                    }
                }
            }
            int getNumberOfQuarters()
            {
                if(_game.TurnNumber < 5) { return _game.TurnNumber; }
                else { return 5; }
            }
        }

        public void AssignValuesToFinancialLineItemsArrays(FinancialsLineItems[] financialsLineItemsArray, DataPanelType dataPanelType)
        {
            for (int i = 0; i < this.Count; i++)
            {
                financialsLineItemsArray[(int)LineItemType.CompanyNamesOrTurnNumber].ValuesArray[i] = getCompanyNameOrQuarter(i);
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
            
            string getCompanyNameOrQuarter(int i)
            {
                if(dataPanelType == DataPanelType.AllPlayers) { return _game.PlayerList[i].Name; }
                else if(dataPanelType == DataPanelType.Quarter) { return QuarterCalc.GetQuarter(this[i].TurnNumber); }
                return null;
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
