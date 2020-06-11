using SkiaSharp;
using System;
using TheManXS.Model.Main;
using Xamarin.Forms;
using QC = TheManXS.Model.Settings.QuickConstants;
using RT = TheManXS.Model.ParametersForGame.ResourceTypeE;
using TT = TheManXS.Model.ParametersForGame.TerrainTypeE;

namespace TheManXS.Model.Map.Surface
{
    public class Coordinate
    {
        System.Random rnd = new System.Random();
        private static Game _game;
        private SQMapConstructArray _map;
        //private SKPoint _topLeftCorner;
        //private int _skSqSize;

        public Coordinate(int row, int col)
        {
            Row = row;
            Col = col;
            // don't think these are needed here...
            //SQKey = GetSQKey(Row, Col);
            //SKRect = new SKRect(Col * QC.SqSize, Row * QC.SqSize, (Col + 1) * QC.SqSize, (Row + 1) * QC.SqSize);
        }

        //public Coordinate(SKPoint p)
        //{
        //    if (_game == null) { _game = (Game)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.Game)]; }
        //    //setValuesForFields();

        //    Row = (int)((_topLeftCorner.Y + p.Y) / _skSqSize);
        //    Col = (int)((_topLeftCorner.X + p.X) / _skSqSize);
        //    SQKey = GetSQKey(Row, Col);
        //    SKRect = new SKRect(Col * QC.SqSize, Row * QC.SqSize, (Col + 1) * QC.SqSize, (Row + 1) * QC.SqSize);
        //}

        public Coordinate(Point p)
        {
            Row = (int)(p.Y / QC.SqSize);
            Col = (int)(p.X / QC.SqSize);
            SQKey = GetSQKey(Row, Col);
        }

        public Coordinate(bool isStartSQ, SQMapConstructArray map)
        {
            _map = map;
            GetPoolStartCoordinate(isStartSQ);
        }

        public int SQKey { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public SKRect SKRect { get; set; }

        //void setValuesForFields()
        //{
        //    var m = _game.GameBoardVM.MapVM.MapMatrix;
        //    var displayInformation = DisplayInformation.GetForCurrentView();
        //    float ratio = (float)displayInformation.RawPixelsPerViewPixel;
        //    _topLeftCorner = new SKPoint(Math.Abs(m.TransX * m.ScaleX), Math.Abs(m.TransY * m.ScaleY));
        //    _skSqSize = (int)((QC.SqSize / ratio) * m.ScaleX);
        //}

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
        public static bool DoesSquareExist(int row, int col) => (row < 0 || row >= QC.RowQ ||
            col < 0 || col >= QC.ColQ) ? false : true;
        private bool SqExists(int r, int c) => (r >= 0 && r < QC.RowQ && c >= 0 && c < QC.ColQ) ? true : false;
    }
}
