using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Financial;
using TheManXS.Model.Units;
using TheManXS.Services;
using Xamarin.Forms;

namespace TheManXS.ViewModel.MapBoardVM.Action
{
    public class UnitAttributes
    {
        public enum AllUnitAttributes { Owner, Status, Resource, Production, Revenue, OPEX, OPEXDisCount,
            TransportCost, GrossProfitD, GrossProfitP, ActionCost, ActionCostDiscount }

        private Unit _unit;
        private Cash _unitCash;
        public UnitAttributes(Unit unit)
        {
            _unit = (Unit)Application.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.ActiveUnit)];
            _unitCash = new Calculations().GetCash(unit);
        }

        public string GetValue(AllUnitAttributes a)
        {
            switch (a)
            {
                case AllUnitAttributes.Owner:
                    return _unit.PlayerName;
                case AllUnitAttributes.Status:
                    return Convert.ToString(_unit.Status);
                case AllUnitAttributes.Resource:
                    return Convert.ToString(_unit.ResourceType);
                case AllUnitAttributes.Production:
                    return Convert.ToString(_unitCash.UnitProduction);
                case AllUnitAttributes.Revenue:
                    return _unitCash.Revenue.ToString("c0");
                case AllUnitAttributes.OPEX:
                    return _unitCash.OPEX.ToString("c0");
                case AllUnitAttributes.OPEXDisCount:
                    return _unit.OPEXDiscount.ToString("p1");
                case AllUnitAttributes.TransportCost:
                    return _unitCash.Transport.ToString("c0");
                case AllUnitAttributes.GrossProfitD:
                    return _unitCash.ProfitDollar.ToString("c0");
                case AllUnitAttributes.GrossProfitP:
                    return _unitCash.ProfitPercent.ToString("p1");
                case AllUnitAttributes.ActionCost:
                    return _unitCash.UnitNexActionCost.ToString("c0");
                case AllUnitAttributes.ActionCostDiscount:
                    return _unit.DevelopmentDiscount.ToString("c0");
                default:
                    return null;
            }
        }
    }
}
