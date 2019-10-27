using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using Xamarin.Forms;
using QC = TheManXS.Model.Settings.QuickConstants;
using IC = TheManXS.Model.InfrastructureStuff.InfrastructureConstants;

namespace TheManXS.ViewModel.MapBoardVM.Tiles
{
    class InfrastructureSegment : BoxView
    {
        private SQ sq;
        public InfrastructureSegment(SQ square)
        {
            sq = square;
            InitInfrastructure();
        }
        private void InitInfrastructure()
        {
            
        }
    }
}
