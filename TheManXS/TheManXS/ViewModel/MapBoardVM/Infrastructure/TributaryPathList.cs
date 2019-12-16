using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Services.EntityFrameWork;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.Infrastructure
{
    public class TributaryPathList : List<SKPath>
    {
        private List<SQ> _listOfAllSQsThatHaveTributaries;
        private List<List<SQ>> _listOfAllTributaries = new List<List<SQ>>();
        private PathCalculations _calc;

        public TributaryPathList(List<SQ> listOfAllSQsThatArePartOfTributary)
        {
            _listOfAllSQsThatHaveTributaries = listOfAllSQsThatArePartOfTributary;
            _calc = new PathCalculations();

            BuildListOfTributaries();
            InitTributaryPaths();
        }

        private void BuildListOfTributaries()
        {
            List<int> tributaryNumbers = new List<int>();
            foreach (SQ sq in _listOfAllSQsThatHaveTributaries)
            {
                if (!tributaryNumbers.Contains(sq.TributaryNumber)) { tributaryNumbers.Add(sq.TributaryNumber); }
            }
            foreach (int tributaryNumber in tributaryNumbers)
            {
                var tributary = _listOfAllSQsThatHaveTributaries
                    .Where(t => t.TributaryNumber == tributaryNumber)
                    .OrderBy(s => s.Row)
                    .ToList();

                _listOfAllTributaries.Add(tributary);
            }
        }
        private void InitTributaryPaths()
        {
            foreach (List<SQ> listSQ in _listOfAllTributaries)
            {
                SKPath tributary = new SKPath();
                this.Add(tributary);

                for (int i = 0; i < listSQ.Count; i++)
                {
                    foreach (SQ sq in listSQ)
                    {
                        if (sq.Row == 0 || sq.Col == 0) { _calc.setStartPoint(sq, ref tributary); }
                        tributary.LineTo(_calc.GetInfrastructureSKPoint(sq, InfrastructureType.Tributary));
                        if (sq.Row == (QC.RowQ - 1) || sq.Col == (QC.ColQ - 1)) { _calc.lineToEndPoint(sq, ref tributary); }
                    }
                }
            }
        }
    }
}
