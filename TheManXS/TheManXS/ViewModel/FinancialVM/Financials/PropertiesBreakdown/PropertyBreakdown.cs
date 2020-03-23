using System;
using System.Collections.Generic;
using System.Text;
using RT = TheManXS.Model.ParametersForGame.ResourceTypeE;
using ST = TheManXS.Model.ParametersForGame.StatusTypeE;
using QC = TheManXS.Model.Settings.QuickConstants;
using TheManXS.Model.Main;
using TheManXS.Model.Units;

namespace TheManXS.ViewModel.FinancialVM.Financials.PropertiesBreakdown
{
    public class PropertyBreakdown
    {
        Game _game;
        SQ _sq;
        Unit _unit;
        private double _opexPerUnit;
        public PropertyBreakdown(Game game, SQ sq)
        {
            _game = game;
            _sq = sq;
            IsSQNotUnit = true;
            initPropertiesForSQ();
        }
        public PropertyBreakdown(Game game, Unit unit)
        {
            _game = game;
            _unit = unit;
            IsSQNotUnit = false;
            initPropertiesForUnit();
        }
        public int ID { get; set; }
        public bool IsSQNotUnit { get; set; }
        public string CompanyName { get; set; }
        public RT Resource { get; set; }
        public ST Status { get; set; }
        public int Production { get; set; }
        public double PPE { get; set; }
        public double Revenue => (Production * _game.CommodityList[(int)Resource].Price);
        public double OPEX => (Production * _opexPerUnit);
        public double GrossProfitD => (Revenue - OPEX);
        public double GrossProfitP => (GrossProfitD / Revenue);

        private void initPropertiesForSQ()
        {
            ID = _sq.Key;
            CompanyName = _sq.OwnerName;
            Resource = _sq.ResourceType;
            Status = _sq.Status;
            Production = _sq.Production;
            _opexPerUnit = _sq.OPEXPerUnit;
        }
        private void initPropertiesForUnit()
        {
            ID = _unit.Number;
            CompanyName = _unit.PlayerName;
            Resource = _unit.ResourceType;
            Status = _unit.Status;
            Production = _unit.Production;
            _opexPerUnit = _unit.OPEXAveragePerUnit;
        }
    }
}
