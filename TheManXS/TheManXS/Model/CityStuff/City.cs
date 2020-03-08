using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using QC = TheManXS.Model.Settings.QuickConstants;
using TheManXS.Model.Map;
using TT = TheManXS.Model.ParametersForGame.TerrainTypeE;
using ST = TheManXS.Model.ParametersForGame.StatusTypeE;
using RT = TheManXS.Model.ParametersForGame.ResourceTypeE;

namespace TheManXS.Model.CityStuff
{
    public class City
    {
        public City() { }
        System.Random rnd = new System.Random();
        private static double _cityFromEdgeRatio = 0.3;
        private int _startRow;
        private int _endRow;
        private int _startCol;
        private int _endCol;
        private SQMapConstructArray _map;

        public City(SQMapConstructArray map)
        {
            _map = map;
            _startRow = (int)(QC.RowQ * _cityFromEdgeRatio);
            _endRow = QC.RowQ - _startRow;
            _startCol = (int)(QC.ColQ * _cityFromEdgeRatio);
            _endCol = QC.ColQ - _startCol;

            InitNewCity(); 
        }
        private void InitNewCity()
        {
            SQ cityStartSQ = GetCityStartSQ();
            InitCitySQ(cityStartSQ, QC.PlayerIndexActual);
            SQ citySQ;
            int startRow = cityStartSQ.Row;
            int startCol = cityStartSQ.Col;
            _map.CityStartSQ = cityStartSQ;

            for (int i = 1; i < QC.PlayerQ; i++)
            {
                if(i < 3)
                {
                    citySQ = _map[startRow, (startCol + i)];
                    InitCitySQ(citySQ, i);
                }
                else
                {
                    citySQ = _map[(startRow + 1), (startCol + (i - 2))];
                    InitCitySQ(citySQ, i);
                }                
            }
        }
        private SQ GetCityStartSQ()
        {
            bool isSuitableForCity = false;
            SQ sq = _map[QC.RowQ / 2, QC.ColQ / 2];

            while (!isSuitableForCity)
            {
                int row = rnd.Next(_startRow, _endRow);
                int col = rnd.Next(_startCol, _endCol);
                sq = _map[row, col];
                if(IsSuitableForCity(sq)) { return sq; }
            }
            return sq;
        }
        private bool IsSuitableForCity(SQ sq)
        {
            if (sq.TerrainType == TT.Grassland &&
                sq.ResourceType == RT.Nada && 
                sq.OwnerNumber == QC.PlayerIndexTheMan)
            { return true; }
            { return false; }
        }
        private void InitCitySQ(SQ sq, int playerNum)
        {
            sq.ResourceType = RT.RealEstate;
            sq.TerrainType = TT.City;
            sq.Production = 8;
            sq.OPEXPerUnit = 20;
            sq.Status = ST.Producing;
            sq.OwnerNumber = playerNum;
        }
    }
}
