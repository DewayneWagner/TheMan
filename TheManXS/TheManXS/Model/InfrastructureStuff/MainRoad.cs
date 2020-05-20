using TheManXS.Model.Main;
using TheManXS.Model.Map.Surface;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.Model.InfrastructureStuff
{
    public class MainRoad
    {
        private SQ[,] _map;
        private SQ _cityStartSQ;

        private int _lb = -1;
        private int _ub = 2;
        private System.Random rnd = new System.Random();
        private int _lbHubDistance = 8;
        private int _ubHubDistance = 16;

        public MainRoad(SQ[,] sqInfrastructure, SQ cityStartSQ)
        {
            _cityStartSQ = cityStartSQ;
            _map = sqInfrastructure;

            InitMainTransportationCorridor();
        }
        private void InitMainTransportationCorridor()
        {
            int hubCol = GetNextHubCol(_cityStartSQ.Col, -1);

            for (int i = -1; i < 2; i += 2)
            {
                int row = _cityStartSQ.Row;
                int col = i == (-1) ? _cityStartSQ.Col - 1 : _cityStartSQ.Col + 3;

                do
                {
                    if (!Coordinate.DoesSquareExist(row, col)) { break; }
                    if (col == hubCol)
                    {
                        _map[row, col].IsHub = true;
                        hubCol = GetNextHubCol(col, i);
                    }

                    setMainTransportationCorridor(row, col);
                    row = getNextRow(row);
                    col += i;

                } while (!isEdgeOfMap(col));
            }

            void setMainTransportationCorridor(int r, int c)
            {
                _map[r, c].IsRoadConnected = true;
                _map[r, c].IsPipelineConnected = true;
                _map[r, c].IsTrainConnected = true;
                _map[r, c].IsMainTransportationCorridor = true;
            }

            bool isEdgeOfMap(int col) => col < 0 || col > (QC.ColQ - 1) ? true : false;

            int getNextRow(int row)
            {
                int nextRow = rnd.Next(_lb, _ub);
                if (row + nextRow == 0) { return 1; }
                else if (row + nextRow == QC.RowQ) { return -1; }
                else { return row + nextRow; }
            }

            int GetNextHubCol(int currentCol, int increment) => currentCol + rnd.Next(_lbHubDistance, _ubHubDistance) * increment;
        }
    }
}
