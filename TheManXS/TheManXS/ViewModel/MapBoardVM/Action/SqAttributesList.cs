using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using ST = TheManXS.Model.Settings.SettingsMaster.StatusTypeE;
using SA = TheManXS.ViewModel.MapBoardVM.Action.SqAttributes.AllSQAttributes;
using System.Linq;
using TheManXS.ViewModel.MapBoardVM.MainElements;

namespace TheManXS.ViewModel.MapBoardVM.Action
{
    public class SqAttributesList : List<SqAttributes>
    {
        private MapVM _mapVM;
        public SqAttributesList(MapVM mapVM)
        {
            _mapVM = mapVM;
            InitListWithDefaults();
            InitTrueBools();
        }
        public bool GetVisibility(ST st, SA sa)
        {
            var sqAttribute = this.Where(s => s.SqAttribute == sa)
                       .Where(s => s.StatusType == st)
                       .FirstOrDefault();
            return sqAttribute.IsVisible;
        }

        private void InitListWithDefaults()
        {
            for (int s = 0; s < (int)ST.Total; s++)
            {
                for (int a = 0; a < (int)SA.Total; a++)
                {
                    Add(new SqAttributes()
                    {
                        IsVisible = false,
                        StatusType = (ST)s,
                        SqAttribute = (SA)a,
                    });
                }
            }
        }
        private void InitTrueBools()
        {
            foreach (SqAttributes s in this)
            {
                switch (s.StatusType)
                {
                    case ST.Nada:
                    case ST.Unexplored:
                        switch (s.SqAttribute)
                        {
                            // is visible
                            case SA.Owner:
                            case SA.Status:
                            case SA.ActionCost:
                                s.IsVisible = true;
                                break;

                            // is not visible
                            case SA.Resource:
                            case SA.Production:
                            case SA.Revenue:
                            case SA.OPEX:
                            case SA.TransportCost:
                            case SA.GrossProfitD:
                            case SA.GrossProfitP:
                            default:
                                break;
                        }
                        break;
                    default:
                        s.IsVisible = true;
                        break;
                }
            }
        }
    }
}
