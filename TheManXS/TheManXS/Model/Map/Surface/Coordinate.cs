using System;
using System.Collections.Generic;
using System.Text;
using QC = TheManXS.Model.Settings.QuickConstants;
using TT = TheManXS.Model.Settings.SettingsMaster.TerrainTypeE;
using AS = TheManXS.Model.Settings.SettingsMaster.AS;
using RT = TheManXS.Model.Settings.SettingsMaster.ResourceTypeE;
using TheManXS.Model.Main;
using SkiaSharp;
using Xamarin.Forms;

namespace TheManXS.Model.Map.Surface
{
    public class Coordinate
    {
        System.Random rnd = new System.Random();
        private SQMapConstructArray _map;
        public Coordinate(SKPoint p) 
        {
            Row = (int)(p.Y / QC.RenderedSQSize);
            Col = (int)(p.X / QC.RenderedSQSize);
            SQKey = ((100 + Row) * 1000 + (100 + Col)) * 10 + QC.CurrentSavedGameSlot;
        }
        public Coordinate(Point p)
        {
            Row = (int)(p.Y / QC.SqSize);
            Col = (int)(p.X / QC.SqSize);
            SQKey = ((100 + Row) * 1000 + (100 + Col)) * 10 + QC.CurrentSavedGameSlot;
        }
        public Coordinate(int row, int col)
        {
            Row = row;
            Col = col;
            SQKey = ((100 + Row) * 1000 + (100 + Col)) * 10 + QC.CurrentSavedGameSlot;
            // row 1, col 1, saved game slot 1 - 1011011;
        }
        public Coordinate(bool isStartSQ, SQMapConstructArray map)
        {
            _map = map;
            GetPoolStartCoordinate(isStartSQ);
        }
        public int SQKey { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }

        public static int GetSQKey(int row, int col) => ((100 + row) * 1000 + (100 + col)) * 10 + QC.CurrentSavedGameSlot;
        public void GetPoolStartCoordinate(bool IsPlayerStartSQ)
        {
            int rowQ = QC.RowQ;
            int colQ = QC.ColQ;

            int row, col;
            SQ sq;

            if (IsPlayerStartSQ)
            {
                int outerLoopCounter = 0;
                do
                {
                    int loopCounter = 0;
                    do
                    {
                        row = rnd.Next(rowQ);
                        col = rnd.Next(colQ);
                        loopCounter++;
                        if (loopCounter == 5)
                            break;
                    } while (!SqExists(row, col));

                    sq = _map[row, col];
                    outerLoopCounter++;
                    if (outerLoopCounter == 5)
                        break;
                } while (sq.TerrainType != TT.Grassland && sq.ResourceType != RT.Nada);
            }
            else
            {
                int outerLoopCounter = 0;
                do
                {
                    int loopCounter = 0;
                    do
                    {
                        row = rnd.Next(rowQ);
                        col = rnd.Next(colQ);
                        loopCounter++;
                        if (loopCounter == 5)
                            break;
                    } while (!SqExists(row, col));

                    sq = _map[row, col];
                    
                    outerLoopCounter++;
                    if (outerLoopCounter == 5)
                        break;
                } while (sq.ResourceType != RT.Nada);
            }
            Row = row;
            Col = col;
            SQKey = GetSQKey(row, col);
        }        
        public static bool DoesSquareExist(int row, int col) => (row >= 0 && row < QC.RowQ &&
            col >= 0 && col < QC.ColQ) ? true : false;        
        private bool SqExists(int r, int c) => (r >= 0 && r < QC.RowQ && c >= 0 && c < QC.ColQ) ? true : false;
    }    
}
