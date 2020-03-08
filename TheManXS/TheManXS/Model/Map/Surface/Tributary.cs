using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.InfrastructureStuff;
using TheManXS.Model.Main;
using TheManXS.Model.Map;
using TheManXS.Model.Map.Surface;
using QC = TheManXS.Model.Settings.QuickConstants;
using TT = TheManXS.Model.ParametersForGame.TerrainTypeE;
using TheManXS.Model.ParametersForGame;

namespace TheManXS.Model.Map.Surface
{
    public class Tributary
    {
        private SQMapConstructArray _SQmap;
        private SQInfrastructure[,] _map;
        private System.Random rnd = new System.Random();
        private int _startRow;
        private int _startCol;
        private bool _isFlowingToNorth;
        private int _tributaryNumber;
        Game _game;

        public Tributary(SQMapConstructArray map, int startRow, int startCol, int tributaryNumber, bool forNewConcept, Game game)
        {
            _game = game;
            _startRow = startRow;
            _startCol = startCol;
            _isFlowingToNorth = GetIsNorth();            

            _map[startRow, startCol].IsTributary = true;
            _map[startRow, startCol].TributaryNumber = _tributaryNumber = tributaryNumber;

            if (_isFlowingToNorth) { InitNewNorthTributary(); }
            else { InitNewSouthTributary(); }
        }
        private void InitNewNorthTributary()
        {
            int col = _startCol;
            for (int row = (_startRow - 1); row >= 0; row--)
            {
                if (Coordinate.DoesSquareExist(row, col))
                {
                    _map[row, col].IsTributary = true;
                    _map[row, col].IsTributaryFlowingFromNorth = true;
                    _map[row, col].TributaryNumber = _tributaryNumber;

                    if (_SQmap[row, col].TerrainType == TT.Mountain) { InitSideForest(row, col); }
                    col += (int)_game.ParameterBoundedList.GetRandomValue(AllBoundedParameters.TerrainConstruct, (int)TerrainBoundedConstructSecondary.TributaryOffset);
                }                
            }
        }
        private void InitNewSouthTributary()
        {
            int col = _startCol;
            for (int row = (_startRow + 1); row < QC.RowQ; row++)
            {
                if (Coordinate.DoesSquareExist(row, col))
                {
                    _map[row, col].IsTributary = true;
                    _map[row, col].IsTributaryFlowingFromNorth = false;
                    _map[row, col].TributaryNumber = _tributaryNumber;

                    if (_SQmap[row, col].TerrainType == TT.Mountain) { InitSideForest(row, col); }
                    col += (int)_game.ParameterBoundedList.GetRandomValue(AllBoundedParameters.TerrainConstruct, (int)TerrainBoundedConstructSecondary.TributaryOffset);
                }                
            }
        }
        private void InitSideForest(int tribRow, int tribCol)
        {
            int forestWidth = (int)_game.ParameterBoundedList.GetRandomValue(AllBoundedParameters.TerrainConstruct, (int)TerrainBoundedConstructSecondary.ForestAroundTributaryWidth);
            
            int col = (tribCol - (forestWidth / 2));

            for (int i = 0; i < forestWidth; i++)
            {
                if (Coordinate.DoesSquareExist(tribRow, col))
                {
                    _SQmap[tribRow, col].TerrainType = TT.Forest;
                    col++;
                }
            }
        }
        private bool GetIsNorth() => (rnd.Next(0, 3) <= 1 ? true : false);
    }
}
