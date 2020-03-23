using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.ViewModel.MapBoardVM.Tiles;
using Xamarin.Forms;
using static TheManXS.ViewModel.FinancialVM.Financials.PropertiesBreakdown.PropertyBreakdownGrid;

namespace TheManXS.ViewModel.FinancialVM.Financials.PropertiesBreakdown
{
    public class SortButton : ImageButton
    {
        enum SortedState { SortedDescending, SortedAscending, NotSorted }
        List<Image> _buttomImages;
        private SortedState _currentSortedState;
        private PropertyBreakdownColumns _propertyBreakdownColumn;
        private AllPropertyBreakdownList _allPropertyBreakdownList;
        public SortButton(PropertyBreakdownColumns propertyBreakdownColumns, AllPropertyBreakdownList allPropertyBreakdownList)
        {
            _propertyBreakdownColumn = propertyBreakdownColumns;
            _allPropertyBreakdownList = allPropertyBreakdownList;
            _currentSortedState = SortedState.NotSorted;
            InitPropertiesOfButton();
            Clicked += SortButton_Clicked;
        }

        private void SortButton_Clicked(object sender, EventArgs e)
        {
            if(_currentSortedState == SortedState.SortedDescending)
            {
                _currentSortedState = SortedState.SortedAscending;
                _allPropertyBreakdownList.SortDataByColumnAscending(_propertyBreakdownColumn);
            }
            else if(_currentSortedState == SortedState.SortedAscending || _currentSortedState == SortedState.NotSorted)
            {
                _currentSortedState = SortedState.SortedDescending;
                _allPropertyBreakdownList.SortDataByColumnDescending(_propertyBreakdownColumn);
            }
            Source = GetImage();
        }

        void InitPropertiesOfButton()
        {
            Margin = 2;
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.FillAndExpand;
            CornerRadius = 2;
            Source = GetImage();
        }
        ImageSource GetImage()
        {
            string path = getImagePath();
            Image image = new Image();
            image.Source = ImageSource.FromResource(path);
            return ImageSource.FromResource(path);

            string getImagePath()
            {
                switch (_currentSortedState)
                {
                    case SortedState.SortedAscending:
                        return "TheManXS.Graphics.AscendingSort.PNG";
                    case SortedState.SortedDescending:
                        return "TheManXS.Graphics.DescendingSort.PNG";
                    case SortedState.NotSorted:
                        return "TheManXS.Graphics.UnSorted.PNG";
                    default:
                        break;
                }
                return "TheManXS.Graphics.AscendingSort.PNG";
            }
        }

    }
}
