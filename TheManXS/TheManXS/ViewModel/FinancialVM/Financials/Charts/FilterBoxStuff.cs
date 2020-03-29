using System;
using System.Collections.Generic;
using System.Text;
using static TheManXS.ViewModel.FinancialVM.Financials.Charts.FinancialChartsVM;

namespace TheManXS.ViewModel.FinancialVM.Financials.Charts
{
    class FilterBoxStuff
    {
        public enum FilterBox1Items { ResourceType, Profit, PPE, Total }
        public enum FilterBox2Items { Company, AllCompanies, Total }
        public enum FilterBox3Items { Item1, Item2, Item3, Total }

        public FilterBoxStuff() { }

        public List<string> GetListOfItems(FilterBoxes fb)
        {
            List<string> items = new List<string>();

            switch (fb)
            {
                case FilterBoxes.FilterBox1:
                    setFilterBox1Items();
                    break;
                case FilterBoxes.FilterBox2:
                    setFilterBox2Items();
                    break;
                case FilterBoxes.FilterBox3:
                    setFilterBox3Items();
                    break;

                case FilterBoxes.Total:
                default:
                    break;
            }
            return items;

            void setFilterBox1Items()
            {
                for (int i = 0; i < (int)FilterBox1Items.Total; i++)
                {
                    items.Add(Convert.ToString((FilterBox1Items)i));
                }
            }
            void setFilterBox2Items()
            {
                for (int i = 0; i < (int)FilterBox2Items.Total; i++)
                {
                    items.Add(Convert.ToString((FilterBox2Items)i));
                }
            }
            void setFilterBox3Items()
            {
                for (int i = 0; i < (int)FilterBox3Items.Total; i++)
                {
                    items.Add(Convert.ToString((FilterBox3Items)i));
                }
            }
        }
    }
}
