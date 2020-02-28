using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TheManXS.Model.Company
{   
    public class CompanyColorGenerator
    {
        System.Random rnd = new System.Random();
        List<SKColor> _listOfSKColors;
        List<string> _listOfAvailableColors;
        public CompanyColorGenerator()
        {
            _listOfSKColors = new List<SKColor>();
            _listOfAvailableColors = new List<string>();

            InitListOfSKColors();
            InitListOfStringColors();
        }
        private void InitListOfSKColors()
        {
            _listOfSKColors.Add(SKColors.AliceBlue); // #FFF0F8FF
            _listOfSKColors.Add(SKColors.Blue); // #FF0000FF
            _listOfSKColors.Add(SKColors.Cyan); // #FF00FFFF
            _listOfSKColors.Add(SKColors.Green); // #FF008000
            _listOfSKColors.Add(SKColors.Lavender); // #FFE6E6FA
            _listOfSKColors.Add(SKColors.Red); // #FFFF0000
            _listOfSKColors.Add(SKColors.Violet); // #FFEE82EE
        }
        private void InitListOfStringColors()
        {
            foreach (SKColor color in _listOfSKColors)
            {
                _listOfAvailableColors.Add(Convert.ToString(color));
            }
        }
        public void RemoveSelectedColorFromOptions(SKColor skColor) => _listOfSKColors.Remove(skColor);
        public SKColor GetSKColor(string colorName)
        {
            int indexOfSelectedIndexColor = _listOfAvailableColors.IndexOf(colorName);
            return _listOfSKColors[indexOfSelectedIndexColor];
        }
        public SKColor GetRandomSKColor()
        {
            SKColor c = _listOfSKColors[rnd.Next(0, _listOfSKColors.Count)];
            _listOfSKColors.Remove(c);
            return c;
        }
        public List<string> GetListOfAvailableSKColors() => _listOfAvailableColors;
    }
}
