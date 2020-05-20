using Xamarin.Forms;
using TT = TheManXS.Model.ParametersForGame.TerrainTypeE;

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
        public enum ImagesAvailable { Logo, UpArrow, DownArrow, NoChangeArrow, AscendingSort, DescendingSort, UnSorted }
        public static Image GetImage(ImagesAvailable ia)
        {
            string path = getImagePath();
            Image image = new Image();
            image.Source = ImageSource.FromResource(path);
            return image;

            string getImagePath()
            {
                switch (ia)
                {
                    case ImagesAvailable.Logo:
                        return "TheManXS.Graphics.Logo.png";
                    case ImagesAvailable.UpArrow:
                        return "TheManXS.Graphics.UpArrow.png";
                    case ImagesAvailable.DownArrow:
                        return "TheManXS.Graphics.DownArrow.png";
                    case ImagesAvailable.NoChangeArrow:
                        return "TheManXS.Graphics.NoChangeArrows.png";
                    case ImagesAvailable.AscendingSort:
                        return "TheManXS.Graphics.AscendingSort.PNG";
                    case ImagesAvailable.DescendingSort:
                        return "TheManXS.Graphics.DescendingSort.PNG";
                    case ImagesAvailable.UnSorted:
                        return "TheManXS.Graphics.UnSorted.PNG";
                    default:
                        return "TheManXS.Graphics.ForestTile.png";
                }
            }
        }
    }
}
