using TheManXS.Model.Main;
using AP = TheManXS.Model.ParametersForGame.AllBoundedParameters;
using QC = TheManXS.Model.Settings.QuickConstants;
using TC = TheManXS.Model.ParametersForGame.TerrainBoundedConstructSecondary;

namespace TheManXS.Model.Map.Surface
{
    public class MainRiver
    {
        private SQMapConstructArray _SQmap;
        private System.Random rnd = new System.Random();
        private int _lb = -1;
        private int _ub = 2;
        private int _tributaryCounter = 0;
        public MainRiver(SQMapConstructArray map)
        {
            InitWestRiver();
            InitEastRiver();
        }

        private SQ[,] _sqInfrastructureArray;
        private SQ _cityStartSQ;
        Game _game;

        public MainRiver(SQ[,] map, SQMapConstructArray sqMap, Game game)
        {
            _game = game;
            _sqInfrastructureArray = map;
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
                if (Coordinate.DoesSquareExist(row, col))
                {
                    if (col == nextTributaryCol)
                    {
                        new Tributary(_SQmap, _sqInfrastructureArray, row, col, _tributaryCounter, _game);
                        //new Tributary(_map, _SQmap, row, col, _tributaryCounter, _game);
                        _tributaryCounter++;
                        nextTributaryCol = GetNextTributaryCol(col, false);
                    }
                    if (_sqInfrastructureArray[row, col].IsRoadConnected)
                    {
                        row++;
                        _sqInfrastructureArray[row, col].IsMainRiver = true;
                    }
                    else { _sqInfrastructureArray[row, col].IsMainRiver = true; }
                }
            }
        }
        private void InitEastRiver()
        {
            int row = _cityStartSQ.Row - 1;
            int nextTributaryCol = GetNextTributaryCol(_cityStartSQ.Col, true);

            for (int col = (_cityStartSQ.Col + 1); col < QC.ColQ; col++)
            {
                if (Coordinate.DoesSquareExist(row, col))
                {
                    if (col < (_cityStartSQ.Col + 2)) { _sqInfrastructureArray[row, col].IsMainRiver = true; }
                    else if (_sqInfrastructureArray[row, col].IsRoadConnected)
                    {
                        row++;
                        _sqInfrastructureArray[row, col].IsMainRiver = true;
                    }
                    else { _sqInfrastructureArray[row, col].IsMainRiver = true; }
                    if (col == nextTributaryCol)
                    {
                        new Tributary(_SQmap, _sqInfrastructureArray, row, col, _tributaryCounter, _game);
                        //new Tributary(_map, _SQmap, row, col, _tributaryCounter, _game);
                        _tributaryCounter++;
                        GetNextTributaryCol(col, true);
                    }
                    row += rnd.Next(_lb, _ub);
                }
            }
        }
        private int GetNextTributaryCol(int currentCol, bool isEastRiver)
        {
            int tributarySpacing = (int)_game.ParameterBoundedList.GetRandomValue(AP.TerrainConstruct, (int)TC.TributaryFrequencySQs);
            if (isEastRiver) { return (tributarySpacing + currentCol); }
            else { return (currentCol - tributarySpacing); }
        }
    }
}
