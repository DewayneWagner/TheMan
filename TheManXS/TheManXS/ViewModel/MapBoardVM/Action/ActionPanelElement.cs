using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TheManXS.ViewModel.MapBoardVM.Action
{
    enum ActionPanelElementType { HeadingAndCloseButton, RowHeadingAndData, Slider, RowHeadingAndDataAndUpDownButtons, 
        ActionButton, Total }
    class ActionPanelElement : Grid
    {
        public ActionPanelElementType ActionPanelElementType { get; set; }

    }
    class PositiveNegativeSelector : Grid
    {
        private const int rowQ = 1;
        private const int colQ = 3;
        List<string> _listOfPossibleValues;

        public PositiveNegativeSelector(List<string> listOfPossibleValues, string startValue)
        {
            _listOfPossibleValues = listOfPossibleValues;
            for (int i = 0; i < rowQ; i++) { RowDefinitions.Add(new RowDefinition()); }
            for (int i = 0; i < colQ; i++) { ColumnDefinitions.Add(new ColumnDefinition()); }
        }

        public Label SelectionBox
        {
            get
            {
                Label l = new Label()
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                };
                return l;
            }
        }

        public IncrementButton NegButton
        {
            get
            {
                IncrementButton n = new IncrementButton("-");
                n.Clicked += OnNegClick();
                return n;
            }
        }

        private EventHandler OnNegClick()
        {
            throw new NotImplementedException();
        }

        public IncrementButton PosButton
        {
            get
            {
                IncrementButton n = new IncrementButton("+");
                n.Clicked += OnPositiveClick();
                return n;
            }
        }

        private EventHandler OnPositiveClick()
        {
            throw new NotImplementedException();
        }
    }
    class ActionPanelList : List<ActionPanelElement>
    {

    }
    class IncrementButton : Button
    {
        public IncrementButton(string text)
        {
            HorizontalOptions = LayoutOptions.CenterAndExpand;
            VerticalOptions = LayoutOptions.CenterAndExpand;
            FontAttributes = FontAttributes.Bold;
            BackgroundColor = Color.LightGray;
            Text = text;
        }
    }
}
