using System;
using System.Collections.Generic;
using System.Text;
using QC = TheManXS.Model.Settings.QuickConstants;
using TT = TheManXS.Model.ParametersForGame.TerrainTypeE;
using RT = TheManXS.Model.ParametersForGame.ResourceTypeE;
using TheManXS.Model.Main;
using SkiaSharp;
using Xamarin.Forms;
using Xamarin.Essentials;
using TheManXS.Model.ParametersForGame;
using A = TheManXS.Model.ParametersForGame.AllBoundedParameters;
using Windows.Graphics.Display;

namespace TheManXS.Model.Map.Surface
{
    public class Coordinate
    {
        System.Random rnd = new System.Random();
        private static Game _game;
        private SQMapConstructArray _map;

        public Coordinate(SKPoint p) 
        {            
            if(_game == null) { _game = (Game)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.Game)]; }

            //testDimensionalInfo();        

            Row = (int)((topLeftCorner.Y + p.Y) / skSqSize);
            Col = (int)((topLeftCorner.X + p.X) / skSqSize);
            SQKey = GetSQKey(Row, Col);
            SKRect = new SKRect(Col * QC.SqSize, Row * QC.SqSize, (Col + 1) * QC.SqSize, (Row + 1) * QC.SqSize);
        }

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

        private SKPoint topLeftCorner
        {
            get
            {
                var m = _game.GameBoardVM.MapVM.MapMatrix;
                return new SKPoint((Math.Abs(m.TransX * m.ScaleX))/ratio, (Math.Abs(m.TransY * m.ScaleY))/ratio);
            }            
        }

        private float ratio
        {
            get
            {
                var displayInformation = DisplayInformation.GetForCurrentView();
                return (float)displayInformation.RawPixelsPerViewPixel;
            }
        }

        private int skSqSize => (int)(QC.SqSize / ratio);

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

        private void testDimensionalInfo()
        {
            double x = _game.GameBoardVM.SplitScreenGrid.X; // doesn't work.  only works if grid offscreen
            double y = _game.GameBoardVM.SplitScreenGrid.Y; // doesn't work

            double x2 = _game.GameBoardVM.MapVM.SKBitMapOfMap.Info.Rect.Left; // just returns 0
            double y2 = _game.GameBoardVM.MapVM.SKBitMapOfMap.Info.Rect.Top; // returns 0

            double x3 = _game.GameBoardVM.MapVM.X;
            double y3 = _game.GameBoardVM.MapVM.Y;

            double x4 = _game.GameBoardVM.MapVM.MapCanvasView.AnchorX;
            double y4 = _game.GameBoardVM.MapVM.MapCanvasView.AnchorY;

            double x5 = _game.GameBoardVM.MapVM.MapCanvasView.Bounds.X;
            double y5 = _game.GameBoardVM.MapVM.MapCanvasView.Bounds.Y;

            double x6 = _game.GameBoardVM.MapVM.MapCanvasView.TranslationX;
            double y6 = _game.GameBoardVM.MapVM.MapCanvasView.TranslationY;

            double x7 = _game.GameBoardVM.MapVM.MapMatrix.TransX;
            double y7 = _game.GameBoardVM.MapVM.MapMatrix.TransY;

            double x8 = _game.GameBoardVM.MapVM.MapMatrix.ScaleX;
            double y8 = _game.GameBoardVM.MapVM.MapMatrix.ScaleY;

            //double bitmapX = (touchPoint.X - bitmap.Matrix.TransX) / bitmap.Matrix.ScaleX;
            //double bitmapY = (touchPoint.Y - bitmap.Matrix.TransY) / bitmap.Matrix.ScaleY;
        }
    }    
}
