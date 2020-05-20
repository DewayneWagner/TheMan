using System;
using System.Collections.Generic;
using TheManXS.Model.Main;
using Xamarin.Forms;
using static TheManXS.ViewModel.FinancialVM.Financials.PropertiesBreakdown.PropertyBreakdownGrid;
using RT = TheManXS.Model.ParametersForGame.ResourceTypeE;
using ST = TheManXS.Model.ParametersForGame.StatusTypeE;

namespace TheManXS.ViewModel.FinancialVM.Financials.PropertiesBreakdown
{
    class HeaderFilterBox : Picker
    {
        Game _game;
        HeaderFilterBoxType _type;
        public HeaderFilterBox(Game game, HeaderFilterBoxType type)
        {
            _game = game;
            _type = type;
            InitProperties();
            InitValues();
        }
        private void InitProperties()
        {
            BackgroundColor = _game.PaletteColors.GetColorFromColorScheme(ViewModel.Style.PaletteColorList.ColorTypes.C0);
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.FillAndExpand;
            FontAttributes = FontAttributes.Bold;
            TextColor = Color.White;
            AutomationId = PropertyBreakdownGrid.HeaderAutomationID;
        }
        private void InitValues()
        {
            List<string> valuesList = new List<string>();
            valuesList.Add("All");
            initList();
            ItemsSource = valuesList;

            void initList()
            {
                switch (_type)
                {
                    case HeaderFilterBoxType.Company:
                        initCompanyList();
                        break;
                    case HeaderFilterBoxType.Resource:
                        initResourceList();
                        break;
                    case HeaderFilterBoxType.Status:
                        initStatusList();
                        break;
                    default:
                        break;
                }
            }
            void initCompanyList()
            {
                foreach (Player player in _game.PlayerList)
                {
                    valuesList.Add(player.Name);
                }
            }
            void initResourceList()
            {
                for (int i = 0; i < (int)RT.Total; i++)
                {
                    valuesList.Add(Convert.ToString((RT)i));
                }
            }
            void initStatusList()
            {
                for (int i = 0; i < (int)ST.Total; i++)
                {
                    valuesList.Add(Convert.ToString((ST)i));
                }
            }
        }
    }
}
