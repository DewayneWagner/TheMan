using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Map;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.Model.InfrastructureStuff
{
   public class MainRoad
    {
        private SQMapConstructArray _map;
        private int _lb = -1;
        private int _ub = 2;
        private System.Random rnd = new System.Random();
        private int _lbHubDistance = 8;
        private int _ubHubDistance = 16;

        public MainRoad(SQMapConstructArray map)
        {
            _map = map;
            InitWestRoad();
            InitEastRoad();
        }
        private void InitWestRoad()
        {
            int row = _map.CityStartSQ.Row;
            int hubCol = GetNextHubCol(_map.CityStartSQ.Col, false);

            for (int col = (_map.CityStartSQ.Col - 1); col >= 0; col--)
            {
                if(col == hubCol)
                {
                    _map[row, col].IsHub = true;
                    hubCol = GetNextHubCol(col, false);
                }
                row += rnd.Next(_lb, _ub);
                _map[row, col].IsRoadConnected = true;
                _map[row, col].IsPipelineConnected = true;
                _map[row, col].IsTrainConnected = true;
            }
        }
        private void InitEastRoad()
        {
            int row = _map.CityStartSQ.Row;
            int hubCol = GetNextHubCol(_map.CityStartSQ.Col, true);

            for (int col = (_map.CityStartSQ.Col + 3); col < QC.ColQ; col++)
            {
                if (col == hubCol)
                {
                    _map[row, col].IsHub = true;
                    hubCol = GetNextHubCol(col, false);
                }
                row += rnd.Next(_lb, _ub);
                _map[row, col].IsRoadConnected = true;
                _map[row, col].IsPipelineConnected = true;
                _map[row, col].IsTrainConnected = true;
            }
        }
        private int GetNextHubCol(int currentCol, bool isEastRoad)
        {
            int hubColSpacing = rnd.Next(_lbHubDistance, _ubHubDistance);
            if (isEastRoad) { return (hubColSpacing + currentCol); }
            else { return (currentCol - hubColSpacing); }
        }  
   }
}
