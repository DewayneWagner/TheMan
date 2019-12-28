using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Map;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.Model.InfrastructureStuff
{
   public class MainRoad
    {
        //private SQMapConstructArray _map;
        private SQ_Infrastructure[,] _map;
        private SQ _cityStartSQ;

        private int _lb = -1;
        private int _ub = 2;
        private System.Random rnd = new System.Random();
        private int _lbHubDistance = 8;
        private int _ubHubDistance = 16;

        public MainRoad(SQMapConstructArray map)
        {
            //_map = map;
            InitWestRoad();
            InitEastRoad();
        }
        public MainRoad(SQ_Infrastructure[,] sqInfrastructure, SQ cityStartSQ)
        {
            _cityStartSQ = cityStartSQ;
            _map = sqInfrastructure;
            InitWestRoad();
            InitEastRoad();
        }
        private void InitWestRoad()
        {
            int row = _cityStartSQ.Row;
            int hubCol = GetNextHubCol(_cityStartSQ.Col, false);

            for (int col = (_cityStartSQ.Col - 1); col >= 0; col--)
            {
                row += rnd.Next(_lb, _ub);

                if (col == hubCol)
                {
                    _map[row, col].IsHub = true;
                    hubCol = GetNextHubCol(col, false);
                }
                
                _map[row, col].IsRoadConnected = true;
                _map[row, col].IsPipelineConnected = true;
                _map[row, col].IsTrainConnected = true;
                _map[row, col].IsMainTransportationCorridor = true;
            }
        }
        private void InitEastRoad()
        {
            int row = _cityStartSQ.Row;
            int hubCol = GetNextHubCol(_cityStartSQ.Col, true);

            for (int col = (_cityStartSQ.Col + 3); col < QC.ColQ; col++)
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
                _map[row, col].IsMainTransportationCorridor = true;
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
