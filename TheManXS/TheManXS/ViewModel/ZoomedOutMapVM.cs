using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xamarin.Forms;
using TT = TheManXS.Model.Settings.SettingsMaster.TerrainTypeE;
using RT = TheManXS.Model.Settings.SettingsMaster.ResourceTypeE;
using TheManXS.Model.Services.EntityFrameWork;
using TheManXS.Model.Main;
using QC = TheManXS.Model.Settings.QuickConstants;
using TheManXS.Model.Company;

namespace TheManXS.ViewModel
{
    public class ZoomedOutMapVM : AbsoluteLayout
    {
        public enum ViewType { Terrain,Resources }
        private double _sqSize = 15;
        private double _marginSize = 0.5;
        public ZoomedOutMapVM(ViewType vt)
        {
            BackgroundColor = Color.Black;

            if (vt == ViewType.Terrain) { InitTerrainMap(); }
            else { InitResourcesMap(); }
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

        private void InitTerrainMap()
        {
            using (DBContext db = new DBContext())
            {
                List<SQ> _sqList = db.SQ.ToList();
                foreach (SQ sq in _sqList)
                {
                    Rectangle rect = new Rectangle(_sqSize * sq.Col, _sqSize * sq.Row, _sqSize, _sqSize);
                    //Color c = GetColor(sq);                    

                    //BoxView bv = new BoxView()
                    //{
                    //    BackgroundColor = c,                            
                    //    HorizontalOptions = LayoutOptions.FillAndExpand,
                    //    VerticalOptions = LayoutOptions.FillAndExpand,
                    //    Margin = _marginSize,
                    //};
                    //Children.Add(bv, rect);
                }
            }
        }
        private Color GetTerrainColor(TT tt)
        {
            switch (tt)
            {
                case TT.Grassland:
                    return Color.LightGreen;
                case TT.Forest:
                    return Color.ForestGreen;
                case TT.Mountain:
                    return Color.SlateGray;
                case TT.City:
                    return Color.Blue;
                default:
                    return Color.White;
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
                    return Color.LightGray;
                case RT.Silver:
                    return Color.Silver;
                case RT.RealEstate:
                    return Color.Blue;
                default:
                    return Color.Blue;
            }
        }
        //private Color GetColor(SQ sq)
        //{
        //    Color c;

        //    //if (sq.IsHub) { c = Color.White; }
        //    //else if (sq.IsRoadConnected) { c = Color.Black; }
        //    //else if (sq.IsMainRiver) { c = Color.Blue; }
        //    //else if (sq.IsTributary) { c = Color.Blue; }
        //    if (sq.OwnerNumber == QC.PlayerIndexTheMan) { c = GetTerrainColor(sq.TerrainType); }
        //    else
        //    {
        //        using (DBContext db = new DBContext())
        //        {
        //            Player owner = db.Player.Find(PlayerList.GetPlayerKey(sq.OwnerNumber));
        //            //CompanyColors cc = new CompanyColors(owner.Color);
        //            //c = cc.ColorXamarin;

        //        }
        //    }
        //    return c;
        //}
    }
}
