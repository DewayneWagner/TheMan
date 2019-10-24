using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Map;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.Model.InfrastructureStuff
{
    public class MainRiver
    {
        private SQMapConstructArray _map;
        private System.Random rnd = new System.Random();
        private int _lb = -1;
        private int _ub = 2;
        private int _lbDistanceBetweenTributaries = 8;
        private int _ubDistanceBetweenTributaries = 16;
        public MainRiver(SQMapConstructArray map)
        {
            _map = map;
            InitWestRiver();
            InitEastRiver();
        }

        private void InitWestRiver()
        {
            int row = _map.CityStartSQ.Row - 1;
            int nextRiverCol = GetNextTributaryCol(_map.CityStartSQ.Col, false);

            for (int col = _map.CityStartSQ.Col; col >= 0; col--)
            {
                row += rnd.Next(_lb, _ub);
                if(col == nextRiverCol)
                {
                    new Tributary(_map, row, col);
                    nextRiverCol = GetNextTributaryCol(col, false);
                }
                if (_map[row, col].IsRoadConnected)
                {
                    row++;
                    _map[row, col].IsMainRiver = true;
                }
                else { _map[row, col].IsMainRiver = true; }
            }
        }
        private void InitEastRiver()
        {
            int row = _map.CityStartSQ.Row - 1;
            int nextRiverCol = GetNextTributaryCol(_map.CityStartSQ.Col, true);

            for (int col = (_map.CityStartSQ.Col + 1); col < QC.ColQ; col++)
            {
                if(col < (_map.CityStartSQ.Col + 2)) { _map[row, col].IsMainRiver = true; }
                else if (_map[row, col].IsRoadConnected)
                {
                    row++;
                    _map[row, col].IsMainRiver = true;
                }
                else { _map[row, col].IsMainRiver = true; }
                if(col == nextRiverCol)
                {
                    new Tributary(_map, row, col);
                    GetNextTributaryCol(col, true);
                }
                row += rnd.Next(_lb, _ub);
            }
        }
        private int GetNextTributaryCol(int currentCol, bool isEastRiver)
        {
            int tributarySpacing = rnd.Next(_lbDistanceBetweenTributaries,_ubDistanceBetweenTributaries);
            if (isEastRiver) { return (tributarySpacing + currentCol); }
            else { return (currentCol - tributarySpacing); }
        }
    }

}
