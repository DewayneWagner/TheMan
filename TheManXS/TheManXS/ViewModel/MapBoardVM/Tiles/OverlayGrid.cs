using SkiaSharp;
using System.Linq;
using TheManXS.Model.Main;
using TheManXS.Model.Services.EntityFrameWork;
using Xamarin.Forms;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.Tiles
{
    public enum Sides { Left, Up, Right, Down, All, Dummy }
    public class OverlayGrid : Grid
    {
        private SQ _activeSQ;
        private static double InsideRatio = 0.8;
        private static double OutsideRatio = 0.1;

        public OverlayGrid() { }
        public OverlayGrid(SQ sq)
        {
            _activeSQ = sq;

            CompressedLayout.SetIsHeadless(this, true);
            RowSpacing = 0;
            ColumnSpacing = 0;
            InitInteriorSquare();

            InitGrid();
            if (sq.OwnerNumber != QC.PlayerIndexTheMan)
            {
                SetOutsideColor();
                SetColorsOfAllSides(OutsideColor);
            }
        }

        public BoxView InteriorSquare { get; set; }
        public Color InsideColor { get; set; }
        public Color OutsideColor { get; set; }
        private void InitGrid()
        {
            int sqDimension;
            for (int i = 0; i < 3; i++)
            {
                sqDimension = (int)(i == 1 ? InsideRatio * QC.SqSize : OutsideRatio * QC.SqSize);
                RowDefinitions.Add(new RowDefinition() { Height = sqDimension });
                ColumnDefinitions.Add(new ColumnDefinition() { Width = sqDimension });
            }
        }
        private void InitInteriorSquare()
        {
            InteriorSquare = new BoxView()
            {
                BackgroundColor = InsideColor,
                Opacity = 0.5,
                Margin = 0,
            };
            CompressedLayout.SetIsHeadless(InteriorSquare, true);
            Children.Add(InteriorSquare, 1, 1);
        }
        private void SetOutsideColor()
        {
            using (DBContext db = new DBContext())
            {
                Player player = db.Player.Where(p => p.Number == _activeSQ.OwnerNumber).FirstOrDefault();
                //CompanyColors cc = new CompanyColors(player.Color);
                // OutsideColor = cc.ColorXamarin;
            }
        }
        public object this[int row, int col]
        {
            get => this[row, col];
            set => this[row, col] = value;
        }
        public void SetColorOfAllSides(SKColor color)
        {

        }
        public void SetColorsOfAllSides(Color color)
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (row == 1 && col == 1) {; }
                    else
                    {
                        BoxView b = new BoxView()
                        {
                            BackgroundColor = color,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.FillAndExpand,
                        };
                        this.Children.Add(b, col, row);
                    }
                }
            }
        }
        //public void RemoveOutsideBorders(Tile tile)
        //{
        //    int length = tile.Children.Count();
        //    for (int i = 0; i < length; i++) { tile.Children.RemoveAt(0); }
        //}
        public void RemoveOutsideBorders()
        {
            int length = Children.Count();
            for (int i = 0; i < length; i++) { Children.RemoveAt(0); }
        }
    }
}
