using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using TheManXS.ViewModel.MapBoardVM.MainElements;
using TheManXS.ViewModel.MapBoardVM.Tiles;
using Xamarin.Forms;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.Scroll
{
    public class Segment
    {
        public Segment()
        {
            TilesToRemove = new List<Tile>();
            TilesToAdd = new List<Tile>();
        }
        public List<Tile> TilesToRemove { get; set; }
        public List<Tile> TilesToAdd { get; set; }
        public bool IsPositiveShift { get; set; }
        public bool IsVerticalShift { get; set; }
    }
}
