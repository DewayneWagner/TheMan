using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.InfrastructureStuff;
using TheManXS.Model.Main;
using TheManXS.Model.Map;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.Model.Map.Surface
{
    public class MainRiver
    {
        private SQMapConstructArray _SQmap;
        private System.Random rnd = new System.Random();
        private int _lb = -1;
        private int _ub = 2;
        private int _lbDistanceBetweenTributaries = 8;
        private int _ubDistanceBetweenTributaries = 16;
        private int _tributaryCounter = 0;
        public MainRiver(SQMapConstructArray map)
        {
            //_map = map;
            InitWestRiver();
            InitEastRiver();
        }

        private SQ_Infrastructure[,] _map;
        private SQ _cityStartSQ;

        public MainRiver(SQ_Infrastructure[,] map, SQMapConstructArray sqMap)
        {
            _map = map;
            _SQmap = sqMap;
            _cityStartSQ = sqMap.CityStartSQ;
            InitWestRiver();
            InitEastRiver();
        }

        private void InitWestRiver()
        {
            int row = _cityStartSQ.Row - 1;
            int nextTributaryCol = GetNextTributaryCol(_cityStartSQ.Col, false);

            for (int col = _cityStartSQ.Col; col >= 0; col--)
            {
                row += rnd.Next(_lb, _ub);
                if(col == nextTributaryCol)
                {
                    new Tributary(_map,_SQmap, row, col, _tributaryCounter);
                    _tributaryCounter++;
                    nextTributaryCol = GetNextTributaryCol(col, false);
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
            int row = _cityStartSQ.Row - 1;
            int nextTributaryCol = GetNextTributaryCol(_cityStartSQ.Col, true);

            for (int col = (_cityStartSQ.Col + 1); col < QC.ColQ; col++)
            {
                if(col < (_cityStartSQ.Col + 2)) { _map[row, col].IsMainRiver = true; }
                else if (_map[row, col].IsRoadConnected)
                {
                    row++;
                    _map[row, col].IsMainRiver = true;
                }
                else { _map[row, col].IsMainRiver = true; }
                if(col == nextTributaryCol)
                {
                    new Tributary(_map, _SQmap, row, col, _tributaryCounter);
                    _tributaryCounter++;
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
