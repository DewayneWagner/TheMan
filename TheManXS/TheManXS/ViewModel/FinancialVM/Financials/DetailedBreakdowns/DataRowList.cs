using Microsoft.EntityFrameworkCore.Internal;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Main;
using Xamarin.Forms;

namespace TheManXS.ViewModel.FinancialVM.Financials.DetailedBreakdowns
{
    public enum DataRowType { MainHeading, SubHeading, Data, Subtotal, GrandTotal }
    class DataRowList : List<Label>
    {
        Game _game;
        DataRowType _dataRowType;
        List<string> _dataValuesList;

        private static bool _colorsHaveBeenInitialized;
        private static Color _mainHeadingColor;
        private static Color _subHeadingColor;
        private static Color _grandTotalColor;

        public DataRowList(Game game, DataRowType dataRowType, List<string> dataValues)
        {
            _game = game;
            _dataRowType = dataRowType;
            _dataValuesList = dataValues;
            if (!_colorsHaveBeenInitialized) { InitColors(); }
            InitList();
        }
        private void InitList()
        {
            foreach (string dataValue in _dataValuesList)
            {
                Label label = new Label()
                {
                    Text = dataValue,
                    HorizontalOptions = LayoutOptions.Fill,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    TextColor = Color.Black,
                    Margin = 0,
                    Padding = 0,
                };

                switch (_dataRowType)
                {
                    case DataRowType.MainHeading:
                        initMainHeadingLabel(label);
                        break;
                    case DataRowType.SubHeading:
                        initSubHeadingLabel(label);
                        break;
                    case DataRowType.Data:
                        initDataLabel(label);
                        break;
                    case DataRowType.Subtotal:
                        initSubTotalLabel(label);
                        break;
                    case DataRowType.GrandTotal:
                        initGrandTotalLabel(label);
                        break;
                    default:
                        break;
                }
                this.Add(label);
            }

            void initMainHeadingLabel(Label label)
            {
                label.BackgroundColor = _mainHeadingColor;
                label.TextColor = Color.White;
                label.FontAttributes = FontAttributes.Bold;
            }
            void initSubHeadingLabel(Label label)
            {
                label.BackgroundColor = _subHeadingColor;
                label.FontAttributes = FontAttributes.Bold;
            }
            void initDataLabel(Label label)
            {

            }
            void initSubTotalLabel(Label label)
            {
                label.FontAttributes = FontAttributes.Bold;
            }
            void initGrandTotalLabel(Label label)
            {
                label.FontAttributes = FontAttributes.Bold;
                label.BackgroundColor = _grandTotalColor;
            }
        }
        private void InitColors()
        {
            _mainHeadingColor = _game.PaletteColors
                    .Where(c => c.Description == "Conklin 2")
                    .Select(c => c.Color)
                    .FirstOrDefault();

            _subHeadingColor = _game.PaletteColors
                    .Where(c => c.Description == "Conklin 4")
                    .Select(c => c.Color)
                    .FirstOrDefault();

            _grandTotalColor = _game.PaletteColors
                    .Where(c => c.Description == "Conklin 1")
                    .Select(c => c.Color)
                    .FirstOrDefault();

            _colorsHaveBeenInitialized = true;
        }
    }
}
