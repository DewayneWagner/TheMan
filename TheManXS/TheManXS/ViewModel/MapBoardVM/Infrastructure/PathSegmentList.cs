using System;
using System.Collections.Generic;
using TheManXS.Model.Main;
using static TheManXS.ViewModel.MapBoardVM.Infrastructure.NewMapInitializer;
using IT = TheManXS.Model.ParametersForGame.InfrastructureType;

namespace TheManXS.ViewModel.MapBoardVM.Infrastructure
{
    class PathSegmentList : List<PathSegment>
    {
        List<SQ> _sList;
        IT _it;

        static byte[,] EntryAndExitPoints = new byte[3, 3]
        {
            { 0, 1, 2 },
            { 8, 0, 4 },
            { 7, 6, 5 }
        };

        public PathSegmentList(List<SQ> sList, IT it)
        {
            _sList = sList;
            _it = it;
            InitList();
            SetStraightIDNumbers();
        }
        private void InitList()
        {
            PathCalculations _calc = new PathCalculations();
            bool isNextToMapEdge = false;
            bool isStartingEdge = false;

            for (int i = 0; i < _sList.Count; i++)
            {
                isNextToMapEdge = _calc.IsMapEdge(_sList[i]);
                if (isNextToMapEdge)
                {
                    isStartingEdge = (i == 0 ? true : false);
                    addMapEdgePoint(i);
                }
                else
                {
                    PathSegment p = new PathSegment();
                    p.EntryPoint = getEntryPoint(i);
                    p.ExitPoint = getExitPoint(i);
                    p.SegmentType = getSegmentType(p.EntryPoint, p.ExitPoint);
                    p.SKPoint = _calc.GetInfrastructureSKPoint(_sList[i], _it);
                    this.Add(p);
                }

                if (isNextToMapEdge && !isStartingEdge)
                { addMapEdgePoint(i); }
            }

            void addMapEdgePoint(int i)
            {
                //DirectionsCompass entryPointDirection = _calc.GetMapEdge(_sList[i]);
                PathSegment edgeP = new PathSegment();
                edgeP.EntryPoint = isStartingEdge ? getMapEdgeEntryAndExitPoints(_calc.GetMapEdge(_sList[i])) : getEntryPoint(i);
                edgeP.ExitPoint = isStartingEdge ? getExitPoint(i) : getMapEdgeEntryAndExitPoints(_calc.GetMapEdge(_sList[i]));
                edgeP.SegmentType = isStartingEdge ? SegmentType.EdgePointStart : SegmentType.EdgePointEnd;
                edgeP.SKPoint = _calc.GetEdgePoint(_sList[i], _it);
                this.Add(edgeP);
            }

            SegmentType getSegmentType(byte entryPoint, byte exitPoint)
            {
                if (isNextToMapEdge) { return SegmentType.Straight; }

                if (isStraightPassThroughSegment()) { return SegmentType.Straight; }
                else { return SegmentType.Curve; }

                bool isStraightPassThroughSegment()
                {
                    byte differential = (byte)(entryPoint - exitPoint);
                    if (Math.Abs(differential) == 4) { return true; }
                    else { return false; }
                }
            }
        }
        private void SetStraightIDNumbers()
        {
            int counter = 0;
            bool isStraightSection = false;

            for (int i = 0; i < Count; i++)
            {
                if (this[i].SegmentType == SegmentType.Straight)
                {
                    if (isStraightSection) { this[i].StraightSegmentID = counter; }
                    else
                    {
                        counter++;
                        isStraightSection = true;
                        this[i].StraightSegmentID = counter;
                    }
                }
            }
        }

        private byte getEntryPoint(int i)
        {
            int index = i <= 2 ? 2 : i;
            return EntryAndExitPoints[scrubIndex(_sList[index - 1].Row - _sList[index].Row + 1),
                scrubIndex(_sList[index - 1].Col - _sList[index].Col + 1)];
        }

        private byte getExitPoint(int i)
        {
            int index = i >= (_sList.Count - 2) ? (_sList.Count - 2) : i;
            return EntryAndExitPoints[scrubIndex(_sList[index + 1].Row - _sList[index].Row + 1),
                scrubIndex(_sList[index + 1].Col - _sList[index].Col + 1)];
        }

        private int scrubIndex(int index)
        {
            if (index < 0) { return 0; }
            else if (index >= 2) { return 2; }
            else { return index; }
        }

        byte getMapEdgeEntryAndExitPoints(DirectionsCompass mapEdgeDirection)
        {
            switch (mapEdgeDirection)
            {
                case DirectionsCompass.N:
                    return 1;
                case DirectionsCompass.E:
                    return 3;
                case DirectionsCompass.S:
                    return 5;
                case DirectionsCompass.W:
                    return 7;
                case DirectionsCompass.Total:
                default:
                    return 0;
            }
        }
    }
}
