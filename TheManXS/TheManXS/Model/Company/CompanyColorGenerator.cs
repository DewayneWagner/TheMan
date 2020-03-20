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
        public static int _maxColorsInList;
        public CompanyColorGenerator()
        {
            _listOfSKColors = new List<SKColor>();
            _listOfAvailableColors = new List<string>();

            InitListOfSKColors();
            InitListOfStringColors();
        }
        public bool GeneratorNeedsToBeInitialized() => _listOfSKColors.Count == _maxColorsInList ? false : true;
        private void InitListOfSKColors()
        {
            _listOfSKColors.Add(SKColors.AliceBlue); // #FFF0F8FF
            _listOfSKColors.Add(SKColors.Blue); // #FF0000FF
            _listOfSKColors.Add(SKColors.Cyan); // #FF00FFFF
            _listOfSKColors.Add(SKColors.Green); // #FF008000
            _listOfSKColors.Add(SKColors.Lavender); // #FFE6E6FA
            _listOfSKColors.Add(SKColors.Red); // #FFFF0000
            _listOfSKColors.Add(SKColors.Violet); // #FFEE82EE
            _maxColorsInList = _listOfSKColors.Count;
        }
        private void InitListOfStringColors()
        {
            _listOfAvailableColors.Add("Alice Blue");
            _listOfAvailableColors.Add("Blue");
            _listOfAvailableColors.Add("Cyan");
            _listOfAvailableColors.Add("Green");
            _listOfAvailableColors.Add("Lavender");
            _listOfAvailableColors.Add("Red");
            _listOfAvailableColors.Add("Violet");
        }
        public void RemoveSelectedColorFromOptions(SKColor skColor) => _listOfSKColors.Remove(skColor);
        public SKColor GetSKColor(string colorName)
        {
            int indexOfSelectedIndexColor = _listOfAvailableColors.IndexOf(colorName);
            return _listOfSKColors[indexOfSelectedIndexColor];
        }
        public SKColor GetSKColor(int index) => _listOfSKColors[index];
        public SKColor GetRandomSKColor()
        {
            SKColor c = _listOfSKColors[rnd.Next(0, _listOfSKColors.Count)];
            _listOfSKColors.Remove(c);
            return c;
        }
        public List<string> GetListOfAvailableSKColors() => _listOfAvailableColors;
    }
}
