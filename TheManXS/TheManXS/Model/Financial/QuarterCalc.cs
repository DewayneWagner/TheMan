using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;

namespace TheManXS.Model.Financial
{
    class QuarterCalc
    {
        Game _game;
        bool _advanceYear;
        int _currentYear, _currentQuarter;
        public QuarterCalc(Game game)
        {
            _game = game;
            _currentQuarter = GetQuarterNumber();
            _advanceYear = _currentQuarter == 4 ? true : false;
            _currentYear = GetYearNumber();
        }
        public string GetNextQuarter()
        {
            int newYear = _advanceYear ? (_currentYear + 1) : _currentYear;
            int newQuarterNumber = _advanceYear ? 1 : (_currentQuarter + 1);
            return CompileQuarterString(newYear, newQuarterNumber);
        }
        private string GetPreviousQuarter(string previousQuarter)
        {
            int previousYear = GetYearNumber(previousQuarter);
            int previousQuarterNumber = GetQuarterNumber(previousQuarter);            

            int newYear = previousQuarterNumber == 1 ? (previousYear - 1) : previousYear;
            int newQuarterNumber = previousQuarterNumber == 1 ? 4 : (previousQuarterNumber - 1);
            return CompileQuarterString(newYear, newQuarterNumber);
        }
        public List<string> GetLastXQuarters(int x)
        {
            List<string> quarters = new List<string>();
            quarters.Add(_game.Quarter);

            for (int i = 1; i < x; i++)
            {
                quarters.Add(GetPreviousQuarter(quarters[(i - 1)]));
            }

            return quarters;
        }
        private string CompileQuarterString(int year, int quarter) => Convert.ToString(year) + "-Q" + Convert.ToString(quarter);        
        private int GetQuarterNumber() => Convert.ToInt32(_game.Quarter.Substring(6));
        private int GetYearNumber() => Convert.ToInt32(_game.Quarter.Substring(0, 4));
        private int GetQuarterNumber(string quarter) => Convert.ToInt32(quarter.Substring(6));
        private int GetYearNumber(string quarter) => Convert.ToInt32(quarter.Substring(0, 4));

    }
}
