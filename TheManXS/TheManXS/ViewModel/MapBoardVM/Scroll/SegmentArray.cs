using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.ViewModel.MapBoardVM.MainElements;
using Xamarin.Forms;
using System.Threading;
using TheManXS.Model.Services.EntityFrameWork;
using QC = TheManXS.Model.Settings.QuickConstants;
using TheManXS.Model.Main;
using TheManXS.Model.Map.Surface;
using TheManXS.ViewModel.MapBoardVM.Tiles;
using System.Linq;

namespace TheManXS.ViewModel.MapBoardVM.Scroll
{
    public class SegmentArray
    {
        private Queue<Segment>[] _qArray;
        private MapScrollView _mapVM;
        public SegmentArray(ActualGameBoardVM a)
        {
            _mapVM = a.GameBoardSplitScreenGrid.MapScrollView;
            _qArray = new Queue<Segment>[(int)MapScrollView.Direction.Total];
        }
        public Segment GetNextSegment(MapScrollView.Direction d) => _qArray[(int)d].Dequeue();
        public int CountSegments(MapScrollView.Direction d) => _qArray[(int)d].Count;

        public void InitNewSegments()
        {
            int numberOfSegmentsToAdd = 0;

            for (int d = 0; d < (int)MapScrollView.Direction.Total; d++)
            {
                numberOfSegmentsToAdd = GetNumberOfSegmentsToAdd((MapScrollView.Direction)d);
                
                if (numberOfSegmentsToAdd != 0)
                {
                    _qArray[d] = new Queue<Segment>();

                    for (int i = _qArray[d].Count; i < numberOfSegmentsToAdd; i++)
                    {
                        AddSegmentToQArray((MapScrollView.Direction)d,(i + 1));
                    }
                }
            }
        }
        private void AddSegmentToQArray(MapScrollView.Direction d, int segmentNumber)
        {
            var m = _mapVM.PinchToZoomContainer.GameBoard.FocusedGameBoard;

            int v = (d == MapScrollView.Direction.N || d == MapScrollView.Direction.S) ? 1 : 0;
            int h = v == 1 ? 0 : 1;

            int p = (d == MapScrollView.Direction.S || d == MapScrollView.Direction.E) ? 1 : 0;
            int n = p == 1 ? 0 : 1;

            int direction = p == 1 ? 1 : (-1);
            int inverseDirection = direction * (-1);

            //                  W                           N                 E                          S               set direction of shift
            int addRow = h * n * m.Row.Start +   v * n * m.Row.Start + h * p * m.Row.Start + v * p * m.Row.End +   (v * direction * segmentNumber);
            int addCol = h * n * m.Col.Start +   v * n * m.Col.Start + h * p * m.Col.End +   v * p * m.Col.Start + (h * direction * segmentNumber);

            int rowLoopCounter = v == 1 ? v : m.Row.Quantity;
            int colLoopCounter = h == 1 ? h : m.Col.Quantity;

            int rowOfTileToRemove = addRow + v * (m.Row.Quantity * inverseDirection);
            int colOfTileToRemove = addCol + h * (m.Col.Quantity * inverseDirection);

            Segment s = new Segment();
                s.IsPositiveShift = p == 1 ? true : false;
                s.IsVerticalShift = v == 1 ? true : false;

            SQ sqToAdd;
            Tile tileToRemove;

            using (DBContext db = new DBContext())
            {
                for (int row = 0; row < rowLoopCounter; row++)
                {
                    for (int col = 0; col < colLoopCounter; col++)
                    {
                        if (Coordinate.DoesSquareExist(addRow, addCol) && Coordinate.DoesSquareExist(rowOfTileToRemove, colOfTileToRemove))
                        {
                            sqToAdd = db.SQ.Find(Coordinate.GetSQKey(addRow, addCol));
                            s.TilesToAdd.Add(new Tile(sqToAdd,QC.SqSize));

                            tileToRemove = _mapVM.PinchToZoomContainer.GameBoard.FocusedGameBoard.Where(t => t.Row == rowOfTileToRemove)
                                .Where(t => t.Col == colOfTileToRemove)
                                .FirstOrDefault();

                            s.TilesToRemove.Add(tileToRemove);

                            addRow += h;
                            addCol += v;

                            rowOfTileToRemove += h;
                            colOfTileToRemove += v;
                        }
                        else { continue; }
                    }
                }
            }
            _qArray[(int)d].Enqueue(s);            
        }
        private int MaxSegmentsThereIsRoomFor(MapScrollView.Direction d)
        {
            var m = _mapVM.PinchToZoomContainer.GameBoard.FocusedGameBoard;

            switch (d)
            {
                case MapScrollView.Direction.W:
                    return m.Col.Start;
                case MapScrollView.Direction.N:
                    return m.Row.Start;
                case MapScrollView.Direction.E:
                    return QC.ColQ - m.Col.End;
                case MapScrollView.Direction.S:
                    return QC.RowQ - m.Row.End;
                default:
                    return 0;
            }
        }
        private int GetNumberOfSegmentsToAdd(MapScrollView.Direction d)
        {
            int maxSegmentsThereIsRoomFor = MaxSegmentsThereIsRoomFor(d);
            if(maxSegmentsThereIsRoomFor == 0) { return 0; }
            else if(maxSegmentsThereIsRoomFor <= ScrollConstants.NumberOfSegmentsToHaveReady) { return maxSegmentsThereIsRoomFor; }
            else { return ScrollConstants.NumberOfSegmentsToHaveReady; }
        }
    }
}
