using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Services.EntityFrameWork;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.MapConstruct
{
    public class RiverSQsList
    {
        private List<SQ> _mainRiverSQs = new List<SQ>();
        private List<SQ> _listOfAllSQsThatHaveTributaries = new List<SQ>();
        private System.Random rnd = new System.Random();
        private List<List<SQ>> _listOfAllTributaries = new List<List<SQ>>();

        public RiverSQsList() 
        { 
            InitList();
            BuildMainRiverPath();
            BuildListOfTributaries();
            AddAllTributariesToMap();
        }
        public SKPath MainRiver { get; set; } = new SKPath();
        public List<SKPath> TributariesList { get; set; } = new List<SKPath>();
        private void InitList()
        {
            using (DBContext db = new DBContext())
            {
                _mainRiverSQs = db.SQ.Where(s => s.IsMainRiver == true)
                    .OrderBy(s => s.Col)
                    .ToList();

                _listOfAllSQsThatHaveTributaries = db.SQ.Where(s => s.IsTributary == true).ToList();
            }
        }
        private void BuildMainRiverPath()
        {
            MainRiver.Convexity = SKPathConvexity.Convex;

            foreach (SQ sq in _mainRiverSQs)
            {
                if(sq.Row == 0 || sq.Col == 0) { setStartPoint(sq); }

                float x = getX(sq.Col);
                float y = getY(sq.Row);

                MainRiver.LineTo(new SKPoint(x, y));

                if(sq.Row == (QC.RowQ - 1) || sq.Col == (QC.ColQ - 1)) { lineToEndPoint(sq); }
            }            
        }
        void setStartPoint(SQ sq)
        {
            if(sq.Row == 0) { MainRiver.MoveTo(((sq.Col * QC.SqSize) + (QC.SqSize / 2)), 0); }
            else { MainRiver.MoveTo(0, ((sq.Row * QC.SqSize) + (QC.SqSize / 2))); }
        }

        void lineToEndPoint(SQ sq)
        {
            if(sq.Row == (QC.RowQ - 1)) { MainRiver.LineTo(((sq.Col * QC.SqSize) + (QC.SqSize / 2)), ((QC.RowQ + 1) * QC.SqSize)); }
            else { MainRiver.LineTo(((QC.ColQ + 1) * QC.SqSize), ((sq.Row * QC.SqSize) + (QC.SqSize / 2))); }
        }

        private void BuildListOfTributaries()
        {
            List<int> tributaryNumbers = new List<int>();
            foreach (SQ sq in _listOfAllSQsThatHaveTributaries)
            {
                if(!tributaryNumbers.Contains(sq.TributaryNumber)) { tributaryNumbers.Add(sq.TributaryNumber); }
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
        private void AddAllTributariesToMap()
        {
            for (int t = 0; t < _listOfAllTributaries.Count; t++)
            {
                SKPath tributary = new SKPath();

                if (_listOfAllTributaries[t][0].IsTributaryFlowingFromNorth) { InitTributaryFlowingFromNorth(); }
                else { InitTributaryFlowingFromSouth(); }

                void InitTributaryFlowingFromNorth()
                {
                    foreach (SQ sq in _listOfAllTributaries[t])
                    {
                        if(sq.Row == 0) { tributary.MoveTo((sq.Col * QC.SqSize + QC.SqSize / 2), 0); }
                        tributary.LineTo((sq.Col * QC.SqSize + QC.SqSize / 2), (sq.Row * QC.SqSize + QC.SqSize / 2));
                    }
                }
                void InitTributaryFlowingFromSouth()
                {
                    foreach (SQ sq in _listOfAllTributaries[t])
                    {
                        if(sq.Row == (QC.RowQ - 1)) { tributary.LineTo((sq.Col * QC.SqSize + QC.SqSize / 2), (QC.RowQ + 1) * QC.SqSize); }
                        tributary.LineTo((sq.Row * QC.SqSize + QC.SqSize / 2), (sq.Col * QC.SqSize + QC.SqSize / 2));
                    }
                }
            }

            foreach (List<SQ> listSQ in _listOfAllTributaries)
            {
                SKPath tributary = new SKPath();
                TributariesList.Add(tributary);

                for (int i = 0; i < listSQ.Count; i++) 
                {

                    foreach (SQ sq in listSQ)
                    {
                        if (sq.Row == 0 || sq.Col == 0) { setStartPoint(sq); }

                        float x = getX(sq.Col);
                        float y = getY(sq.Row);

                        MainRiver.LineTo(new SKPoint(x, y));

                        if (sq.Row == (QC.RowQ - 1) || sq.Col == (QC.ColQ - 1)) { lineToEndPoint(sq); }
                    }
                }
            }
        }
        private void AddAllTributariesToMap(bool scrap)
        {
            MainRiver.Convexity = SKPathConvexity.Convex;

            SQ fromSQ = new SQ(true, true);
            SQ toSQ = new SQ(true, true);

            foreach (List<SQ> listSQ in _listOfAllTributaries)
            {
                SKPath tributary = new SKPath();
                TributariesList.Add(tributary);

                for (int i = 0; i < listSQ.Count; i++) { tributary.MoveTo(getX(listSQ[i].Col), getY(listSQ[i].Row)); }
            }

        }
        private float getX(int col) => (col * QC.SqSize) + QC.SqSize / 2;
        private float getY(int row) => (QC.SqSize * row) + QC.SqSize / 2;
    }
}
