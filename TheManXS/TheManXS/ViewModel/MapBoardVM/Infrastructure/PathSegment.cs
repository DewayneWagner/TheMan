using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.InfrastructureStuff;
using IT = TheManXS.Model.ParametersForGame.InfrastructureType;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.Infrastructure
{
    public enum SegmentType { EdgePointStart, StartArc, MidPointArc, EndArc, Straight, EndEdgePoint }

    class PathSegment
    {
        public PathSegment() { }
        public SegmentType SegmentType { get; set; }
        public SKPoint SKPoint { get; set; } 
        public float Radius { get; set; }
        public int StraightSegmentID { get; set; }

        /************
         *  0 - 1 - 2
         *  7 - X - 3
         *  6 - 5 - 4
         *  *********/

        public byte EntryPoint { get; set; }
        public byte ExitPoint { get; set; }
    }    
    class PathSegmentList : List<PathSegment>
    {
        List<SQInfrastructure> _sList;
        IT _it;
        float _radiusOfCurves;
        float _ratioOfRadiusToSQSize = 0.25f;

        public PathSegmentList(List<SQInfrastructure> sList, IT it)
        {
            _sList = sList;
            _it = it;
            _radiusOfCurves = QC.SqSize * _ratioOfRadiusToSQSize;
            InitList();
            ModifySegmentTypesForArcs();
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
                if(isNextToMapEdge) { isStartingEdge = (i == 0 ? true : false); }
                PathSegment p = new PathSegment();
                p.EntryPoint = getEntryOrExitPoint(i, true);
                p.ExitPoint = getEntryOrExitPoint(i, false);
                p.SegmentType = getSegmentType(p.EntryPoint, p.ExitPoint);
                p.SKPoint = _calc.GetInfrastructureSKPoint(_sList[i], _it);

                this.Add(p);
            }
            SegmentType getSegmentType(byte entryPoint, byte exitPoint)
            {
                if (isNextToMapEdge)
                {
                    if (isStartingEdge) { return SegmentType.EdgePointStart; }
                    else { return SegmentType.EndEdgePoint; }
                }

                if(isStraightPassThroughSegment()) { return SegmentType.Straight; }
                else { return SegmentType.MidPointArc; }

                bool isStraightPassThroughSegment()
                {
                    byte differential = (byte)(entryPoint - exitPoint);
                    if(Math.Abs(differential) == 4) { return true; }
                    else { return false; }
                }                
            }
        }

        private void ModifySegmentTypesForArcs()
        {
            for (int i = 0; i < Count; i++)
            {
                if(this[i].SegmentType == SegmentType.MidPointArc)
                {
                    this[i - 1].SegmentType = SegmentType.StartArc;
                    this[i + 1].SegmentType = SegmentType.EndArc;
                    this[i].Radius = _radiusOfCurves;
                }
            }
        }

        private void SetStraightIDNumbers()
        {
            int counter = 0;
            bool isStraightSection = false;

            for (int i = 0; i < Count; i++)
            {
                if(this[i].SegmentType == SegmentType.Straight)
                {
                    if(isStraightSection) { this[i].StraightSegmentID = counter; }
                    else
                    {
                        counter++;
                        isStraightSection = true;
                        this[i].StraightSegmentID = counter;
                    }
                }
            }
        }

        private byte getEntryOrExitPoint(int index, bool isEntryPoint)
        {
            if (isMapEdge()) { return getMapEdgeEntryPoints(); }
            else
            {
                int mod = isEntryPoint ? -1 : 1;
                int rowChange = (_sList[index].Row - _sList[index + mod].Row);
                int colChange = (_sList[index].Col - _sList[index + mod].Col);

                if (rowChange == 1)
                {
                    if (colChange == 1) { return 0; }
                    else if (colChange == 0) { return 1; }
                    else { return 2; }
                }
                else if (rowChange == 0)
                {
                    if (colChange == 1) { return 7; }
                    else { return 3; }
                }
                else
                {
                    if (colChange == -1) { return 4; }
                    else if (colChange == 0) { return 5; }
                    else { return 6; }
                }
            }            
            bool isMapEdge()
            {
                if (_sList[index].Row == 0 || _sList[index].Row == (QC.RowQ-1) || _sList[index].Col == 0 || _sList[index].Col == (QC.ColQ - 1)) { return true; }
                else { return false; }
            }
            byte getMapEdgeEntryPoints()
            {
                if(_sList[index].Row == 0) { return 1; }
                else if(_sList[index].Row == QC.RowQ) { return 5; }
                else if(_sList[index].Col == 0) { return 7; }
                else { return 3; }
            }
        }
    }
}
