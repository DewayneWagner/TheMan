using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Main;
using Xamarin.Forms;

namespace TheManXS.ViewModel.MapBoardVM.Action
{
    class SidePanelLabel : Label
    {
        public enum LabelType { TopHeading, RowHeading, Data }

        private Game _game;
        private double _buttonAndTitleHeight = 35;

        public SidePanelLabel(LabelType labelType, string text)
        {
            switch (labelType)
            {
                case LabelType.RowHeading:
                    InitRowHeading(text);
                    break;

                case LabelType.Data:
                    InitDataRow(text);
                    break;

                case LabelType.TopHeading:
                default:
                    break;
            }
        }

        public SidePanelLabel(Game game, LabelType labelType, string text)
        {
            _game = game;
            switch (labelType)
            {
                case LabelType.TopHeading:
                    InitTopHeading(text);
                    break;
                case LabelType.RowHeading:
                    InitRowHeading(text);
                    break;
                case LabelType.Data:
                    InitDataRow(text);
                    break;
                default:
                    break;
            }
        }
        void InitTopHeading(string text)
        {
            Text = text;
            FontAttributes = FontAttributes.Bold;
            HorizontalOptions = LayoutOptions.FillAndExpand;
            HorizontalTextAlignment = TextAlignment.Center;
            VerticalTextAlignment = TextAlignment.Center;
            BackgroundColor = _game.PaletteColors.Where(c => c.Description == "Banff 2")
                                .Select(c => c.Color)
                                .FirstOrDefault();
            TextColor = Color.White;
            HeightRequest = _buttonAndTitleHeight;
            Margin = (_buttonAndTitleHeight * 0.1);
            FontSize = (_buttonAndTitleHeight * 0.5);
        }
        void InitRowHeading(string text)
        {
            Text = text;
            HorizontalOptions = LayoutOptions.StartAndExpand;
        }
        void InitDataRow(string text)
        {
            Text = text;
            VerticalOptions = LayoutOptions.Center;
            HorizontalOptions = LayoutOptions.CenterAndExpand;
        }
    }
}
