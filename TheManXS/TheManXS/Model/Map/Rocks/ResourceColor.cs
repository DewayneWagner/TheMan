using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using Xamarin.Forms;
using RT = TheManXS.Model.Settings.SettingsMaster.ResourceTypeE;

namespace TheManXS.Model.Map.Rocks
{
    public class ResourceColor
    {
        public static Color GetColor(RT rt)
        {
            switch (rt)
            {
                case RT.Coal:
                    return Color.DarkGray;
                case RT.Gold:
                    return Color.Gold;
                case RT.Iron:
                    return Color.SteelBlue;
                case RT.Nada:
                    return Color.White;
                case RT.Oil:
                    return Color.Black;
                case RT.RealEstate:
                    return Color.Blue;
                case RT.Silver:
                    return Color.Silver;
                default:
                    return Color.White;
            }
        }
    }
}
