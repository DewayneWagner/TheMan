using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;
using TT = TheManXS.Model.Settings.SettingsMaster.TerrainTypeE;

namespace TheManXS.ViewModel.MapBoardVM.Tiles
{
    class AllImages
    {        
        public static Image GetTerrainImage(TT tt)
        {
            Image i = new Image();
            switch (tt)
            {
                case TT.Grassland:
                    i.Source = ImageSource.FromResource("TheManXS.Graphics.GrasslandTileBeth.png");
                    return i;
                case TT.Forest:
                    i.Source = ImageSource.FromResource("TheManXS.Graphics.ForestTileBeth.png");
                    return i;
                case TT.Mountain:
                    i.Source = ImageSource.FromResource("TheManXS.Graphics.MountainTileBeth.png");
                    return i;
                case TT.City:
                    i.Source = ImageSource.FromResource("TheManXS.Graphics.CityTileBeth.png");
                    return i;
                default:
                    i.Source = ImageSource.FromResource("TheManXS.Graphics.CityTileBeth.png");
                    return i;
            }
        }
        public enum ImagesAvailable { Logo }
        public static Image GetImage(ImagesAvailable ia)
        {
            Image i = new Image();
            i.Source = ImageSource.FromResource("TheManXS.Graphics.Logo.png");
            return i;
        }
    }
}
