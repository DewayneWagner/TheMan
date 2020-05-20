﻿using TheManXS.ViewModel.FinancialVM.Financials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TheManXS.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FinancialsView : ContentPage
    {
        public FinancialsView()
        {
            FinancialsVM fvm = new FinancialsVM();
            InitializeComponent();
            BindingContext = fvm;
            DataPresentationAreaSV.Content = fvm.DataPresentationArea;
            //DataPresentationAreaSV.Content = fvm.FinancialsGrid;
        }
    }
}