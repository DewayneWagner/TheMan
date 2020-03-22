using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Units;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.FinancialVM.Financials.PropertiesBreakdown
{
    public class AllPropertyBreakdownList : List<PropertyBreakdown>
    {
        Game _game;
        public AllPropertyBreakdownList(Game game)
        {
            _game = game;
            LoadListWithAllOwnedSQs();
            LoadListWithAllOwnedUnits();
        }

        private void LoadListWithAllOwnedSQs()
        {
            List<SQ> sqList = _game.SquareDictionary.Values.Where(p => p.OwnerNumber != QC.PlayerIndexTheMan)
                .Where(p => !p.IsPartOfUnit)
                .ToList();

            foreach (SQ sq in sqList)
            {
                Add(new PropertyBreakdown(_game, sq));
            }            
        }
        public List<PropertyBreakdown> PropertyBreakdownDisplayList { get; set; } = new List<PropertyBreakdown>();
        private void LoadListWithAllOwnedUnits()
        {
            foreach (Unit unit in _game.ListOfCreatedProductionUnits)
            {
                Add(new PropertyBreakdown(_game, unit));
            }
        }
    }    
}
