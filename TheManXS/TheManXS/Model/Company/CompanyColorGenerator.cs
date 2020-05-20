using SkiaSharp;
using System.Collections.Generic;

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
        private void InitListOfSKColors()
        {
            _listOfSKColors.Add(SKColors.LightSlateGray);
            _listOfSKColors.Add(SKColors.Orange);
            _listOfSKColors.Add(SKColors.Blue); // #FF0000FF            
            _listOfSKColors.Add(SKColors.Green); // #FF008000
            _listOfSKColors.Add(SKColors.Lavender); // #FFE6E6FA
            _listOfSKColors.Add(SKColors.Red); // #FFFF0000
            _maxColorsInList = _listOfSKColors.Count;
        }
        private void InitListOfStringColors()
        {
            _listOfAvailableColors.Add("Slate Gray");
            _listOfAvailableColors.Add("Orange");
            _listOfAvailableColors.Add("Blue");
            _listOfAvailableColors.Add("Green");
            _listOfAvailableColors.Add("Lavender");
            _listOfAvailableColors.Add("Red");
        }
        public void RemoveSelectedColorFromOptions(SKColor skColor) => _listOfSKColors.Remove(skColor);
        public SKColor GetSKColor(int index) => _listOfSKColors[index];
        public SKColor GetRandomSKColor()
        {
            if (_listOfSKColors.Count == 0) { InitListOfSKColors(); }
            SKColor c = _listOfSKColors[rnd.Next(0, _listOfSKColors.Count)];
            _listOfSKColors.Remove(c);
            return c;
        }
        public List<string> GetListOfAvailableSKColors() => _listOfAvailableColors;
    }
}
