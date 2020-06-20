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

    }
    class ActionPanelList : List<ActionPanelElement>
    {

    }
}
