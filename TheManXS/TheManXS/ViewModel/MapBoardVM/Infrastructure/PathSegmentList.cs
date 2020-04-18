using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.InfrastructureStuff;
using QC = TheManXS.Model.Settings.QuickConstants;
using IT = TheManXS.Model.ParametersForGame.InfrastructureType;

namespace TheManXS.ViewModel.MapBoardVM.Infrastructure
{
    class PathSegmentList : List<PathSegment>
    {
        List<SQInfrastructure> _sList;
        IT _it;
        float _radiusOfCurves;
        float _ratioOfRadiusToSQSize = 0.25f;

        static byte[,] EntryAndExitPoints = new byte[3, 3]
        {
            { 0, 1, 2 },
            { 8, 0, 4 },
            { 7, 6, 5 }
        };

        public PathSegmentList(List<SQInfrastructure> sList, IT it)
        {
            _sList = sList;
            _it = it;
            _radiusOfCurves = QC.SqSize * _ratioOfRadiusToSQSize;
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
                    //if (isStartingEdge) { addMapEdgePoint(i); }
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
                DirectionsCompass entryPointDirection = _calc.GetMapEdge(_sList[i]);
                PathSegment edgeP = new PathSegment();
                edgeP.EntryPoint = isStartingEdge? getMapEdgeEntryAndExitPoints(_calc.GetMapEdge(_sList[i])) : getEntryPoint(i);
                edgeP.ExitPoint = isStartingEdge ? getExitPoint(i) : getMapEdgeEntryAndExitPoints(_calc.GetMapEdge(_sList[i]));

                //edgeP.EntryPoint = getEntryOrExitPoint(i, true, entryPointDirection);
                //edgeP.ExitPoint = getOppositeDirection(edgeP.EntryPoint);
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
            byte getOppositeDirection(byte b) => (byte)(b >= 4 ? (b - 4) : (b + 4));
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

        private byte getEntryPoint(int index) => EntryAndExitPoints[(_sList[index - 1].Row - _sList[index].Row + 1), (_sList[index - 1].Col - _sList[index].Col + 1)];
        private byte getExitPoint(int index) => EntryAndExitPoints[(_sList[index + 1].Row - _sList[index].Row + 1), (_sList[index + 1].Col - _sList[index].Col + 1)];
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



        //private byte getEntryOrExitPoint(int index, bool isEntryPoint, DirectionsCompass mapEdgeDirection = DirectionsCompass.N)
        //{

        //    if (isMapEdge()) { return getMapEdgeEntryPoints(); }
        //    else
        //    {
        //        byte rowchange = _sList[index]
        //    }

        //    else if(index == -1)
        //    {
        //        int mod = isEntryPoint ? -1 : 1;
        //        int secondIndex = index - mod;

        //        if (secondIndex < 0 || secondIndex >= _sList.Count) { return 0; }

        //        int rowChange = (_sList[index].Row - _sList[secondIndex].Row);
        //        int colChange = (_sList[index].Col - _sList[secondIndex].Col);

        //        if (rowChange == 1)
        //        {
        //            if (colChange == 1) { return 0; }
        //            else if (colChange == 0) { return 1; }
        //            else { return 2; }
        //        }
        //        else if (rowChange == 0)
        //        {
        //            if (colChange == 1) { return 7; }
        //            else { return 3; }
        //        }
        //        else
        //        {
        //            if (colChange == -1) { return 4; }
        //            else if (colChange == 0) { return 5; }
        //            else { return 6; }
        //        }
        //    }
        //    bool isMapEdge()
        //    {
        //        if (_sList[index].Row == 0 || _sList[index].Row == (QC.RowQ - 1) || _sList[index].Col == 0 || _sList[index].Col == (QC.ColQ - 1)) { return true; }
        //        else { return false; }
        //    }
            
        //}
    }
}
