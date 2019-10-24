using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TheManXS.Model.Company
{    
    public enum AllAvailableCompanyColors { Red, Green, Yellow, Blue, Violet, Orange, Total }

    public class AllAvailableCompanyColorsList : List<Color>
    {
        public AllAvailableCompanyColorsList()
        {
            Add(Color.Red);
            Add(Color.Green);
            Add(Color.Yellow);
            Add(Color.Blue);
            Add(Color.Violet);
            Add(Color.Orange);
        }
        public AllAvailableCompanyColors GetEnum(Color color) => (AllAvailableCompanyColors)this.IndexOf(color);
    }
    public class CompanyColors
    {
        public CompanyColors() { }
        public CompanyColors(AllAvailableCompanyColors a)
        {
            ColorEnum = a;
            AllAvailableCompanyColorsList all = new AllAvailableCompanyColorsList();
            ColorXamarin = all[(int)a];
            ColorString = ColorToStringConverter(ColorXamarin);
        }
        public Color ColorXamarin { get; set; }
        public string ColorString { get; set; }
        public AllAvailableCompanyColors ColorEnum { get; set; }
        public static string ColorToStringConverter(Color c)
        {
            int red = (int)(c.R * 255);
            int green = (int)(c.G * 255);
            int blue = (int)(c.B * 255);
            int alpha = (int)(c.A * 255);

            return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", red, green, blue, alpha);
        }
    }
    public class AvailableCompanyColorsList : List<string>
    {
        public AvailableCompanyColorsList(bool isForStartNewGamePage)
        {
            for (int i = 0; i < (int)AllAvailableCompanyColors.Total; i++)
            {
                Add(Convert.ToString((AllAvailableCompanyColors)i));
            }            
        }
    }
    public class CompanyColorsList : List<CompanyColors>
    {
        System.Random rnd = new System.Random();
        private List<Color> _colorList;
        public CompanyColorsList()
        {
            _colorList = new AllAvailableCompanyColorsList();
            InitList();            
        }
        private void InitList()
        {
            for (int i = 0; i < (int)AllAvailableCompanyColors.Total; i++)
            {                
                Add(new CompanyColors()
                {
                    ColorEnum = (AllAvailableCompanyColors)i,
                    ColorXamarin = _colorList[i],
                    ColorString = CompanyColors.ColorToStringConverter(_colorList[i])
                });
            }
        }
        public void RemoveColorFromList(Color c) => _colorList.Remove(c);
        public CompanyColors GetRandomColor()
        {
            int index = rnd.Next(0, (int)this.Count);
            CompanyColors cc = this[index];
            this.RemoveAt(index);
            return cc;
        }
    }
}
