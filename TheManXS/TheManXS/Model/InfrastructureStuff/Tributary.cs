using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Map;
using TheManXS.Model.Map.Surface;
using QC = TheManXS.Model.Settings.QuickConstants;
using TT = TheManXS.Model.Settings.SettingsMaster.TerrainTypeE;

namespace TheManXS.Model.InfrastructureStuff
{
    public class Tributary
    {
        private SQMapConstructArray _map;
        private System.Random rnd = new System.Random();
        private int _startRow;
        private int _startCol;
        private int _lb = -1;
        private int _ub = 2;
        private int _LBForestWidthAroundTributary = 0;
        private int _UBforestUBWidthAroundTributary = 5;
        private bool _isNorth;

        public Tributary(SQMapConstructArray map, int startRow, int startCol)
        {
            _map = map;
            _startRow = startRow;
            _startCol = startCol;
            _isNorth = GetIsNorth();

            if (_isNorth) { InitNewNorthTributary(); }
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
                    if (_map[row, col].TerrainType == TT.Mountain)
                    {
                        InitSideForest(row, col);
                    }
                    col += rnd.Next(_lb,_ub);
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
                    if (_map[row, col].TerrainType == TT.Mountain)
                    {
                        InitSideForest(row, col);
                    }
                    col += rnd.Next(_lb,_ub);
                }                
            }
        }
        private void InitSideForest(int tribRow, int tribCol)
        {
            int forestWidth = rnd.Next(_LBForestWidthAroundTributary, _UBforestUBWidthAroundTributary);
            int col = (tribCol - (forestWidth / 2));
            if (Coordinate.DoesSquareExist(tribRow, col))
            {
                for (int i = 0; i < forestWidth; i++)
                {
                    _map[tribRow, col].TerrainType = TT.Forest;
                    col++;
                }
            }            
        }
        private bool GetIsNorth() => (rnd.Next(0, 3) <= 1 ? true : false);
    }
}
