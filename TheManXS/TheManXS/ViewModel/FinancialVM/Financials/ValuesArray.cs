using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using static TheManXS.ViewModel.FinancialVM.Financials.FinancialsVM;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.FinancialVM.Financials
{
    class ValuesArray
    {
        Game _game;
        public ValuesArray(Game game)
        {
            _game = game;
        }
        public string[] Values { get; set; } = new string[5];

        private void LoadArrayWithValues()
        {
            for (int i = 0; i < QC.PlayerQ; i++)
            {
                CalculatedFinancialValues c = new CalculatedFinancialValues(_game,_game.PlayerList[i]);
            }
        }
        
    }
}
