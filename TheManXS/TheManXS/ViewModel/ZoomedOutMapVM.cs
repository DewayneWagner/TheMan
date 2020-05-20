using System.Collections.Generic;
using System.Linq;
using TheManXS.Model.Main;
using TheManXS.Model.Services.EntityFrameWork;
using Xamarin.Forms;
using RT = TheManXS.Model.ParametersForGame.ResourceTypeE;

namespace TheManXS.ViewModel
{
    public class ZoomedOutMapVM : AbsoluteLayout
    {
        private double _sqSize;
        private double _marginSize = 0.5;
        public ZoomedOutMapVM()
        {
            BackgroundColor = Color.White;
            //_sqSize = GetSQSize();
            _sqSize = 15;
            InitResourcesMap();
            CompressedLayout.SetIsHeadless(this, true);
        }

        private void InitResourcesMap()
        {
            using (DBContext db = new DBContext())
            {
                List<SQ> _sqList = db.SQ.ToList();
                foreach (SQ sq in _sqList)
                {
                    Rectangle rect = new Rectangle(_sqSize * sq.Col, _sqSize * sq.Row, _sqSize, _sqSize);
                    BoxView bv = new BoxView()
                    {
                        BackgroundColor = GetResourceColor(sq.ResourceType),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        Margin = _marginSize,
                    };
                    Children.Add(bv, rect);
                }
            }
        }

        private Color GetResourceColor(RT rt)
        {
            switch (rt)
            {
                case RT.Oil:
                    return Color.Black;
                case RT.Gold:
                    return Color.Gold;
                case RT.Coal:
                    return Color.DarkSlateGray;
                case RT.Iron:
                    return Color.Red;
                case RT.Silver:
                    return Color.Silver;
                case RT.RealEstate:
                    return Color.Blue;
                default:
                    return Color.AliceBlue;
            }
        }
    }
}
