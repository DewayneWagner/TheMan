using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TheManXS.Model.Main;
using IT = TheManXS.Model.ParametersForGame.InfrastructureType;
using TheManXS.Model.Map.Surface;
using Windows.Media.Devices;
using Windows.Networking.Connectivity;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Infrastructure
{
    public enum ConnectDirection { NW, N, NE, E, SE, S, SW, W, Total }

    class InfSegmentList : List<InfSegment>
    {
        List<SQ> _listSQsThatNeedInf;
        SearchModifierList _searchModifierList;
        SQList _sqList;

        // for rendering inf on new map
        public InfSegmentList(SQList sqList)
        {
            _sqList = sqList;
            _listSQsThatNeedInf = new List<SQ>();
            _searchModifierList = new SearchModifierList();
      
            CreateListOfInfSegmentsToRender();
        }

        // for adding inf during gameplay
        public InfSegmentList(List<SQ> sqList)
        {
            _listSQsThatNeedInf = new List<SQ>();
            _listSQsThatNeedInf = sqList;
        }
        private void CreateListOfInfSegmentsToRender()
        {
            //initSQListThatNeedInfAtBeginningOfGame();
            //initListOfInfSegmentsThemselves();

            //void initSQListThatNeedInfAtBeginningOfGame()
            //{
            //    foreach (SQ sq in _sqList)
            //    {
            //        if (sq.IsRoadConnected) { _listSQsThatNeedInf.Add(sq); }
            //        if (sq.IsTrainConnected) { _listSQsThatNeedInf.Add(sq); }
            //        if (sq.IsPipelineConnected) { _listSQsThatNeedInf.Add(sq); }
            //        if (sq.IsMainRiver) { _listSQsThatNeedInf.Add(sq); }
            //        if (sq.IsTributary) { _listSQsThatNeedInf.Add(sq); }
            //    }
            //}
            //void initListOfInfSegmentsThemselves(bool oldCrapThatDoesntWork)
            //{
            //    foreach (SQ sq in _listSQsThatNeedInf)
            //    {
            //        this.Add(new InfSegment()
            //        {
            //            SQ = sq,
            //            InfrastructureType = getIT(sq),
            //        });
            //    }
            //}

            initListOfInfSegmentsThemselves();
            initListOfConnectionDirectionsForEachInfSegmentInThis();
            addAdditionalSegments();

            void initListOfInfSegmentsThemselves()
            {
                foreach (SQ sq in _sqList)
                {
                    if(sq.IsRoadConnected) { addSegmentToList(sq, IT.Road); }
                    if(sq.IsTrainConnected) { addSegmentToList(sq, IT.RailRoad); }
                    if(sq.IsPipelineConnected) { addSegmentToList(sq, IT.Pipeline); }
                    if(sq.IsMainRiver) { addSegmentToList(sq, IT.MainRiver); }
                    if(sq.IsTributary) { addSegmentToList(sq, IT.Tributary); }
                }

                void addSegmentToList(SQ sq, IT it)
                {
                    this.Add(new InfSegment()
                    {
                        SQ = sq,
                        InfrastructureType = it,
                    });
                }
            }
            void initListOfConnectionDirectionsForEachInfSegmentInThis()
            {
                int adjRow, adjCol;
                int length = this.Count;

                for (int i = 0; i < length; i++)
                {
                    for (int ii = 0; ii < (int)ConnectDirection.Total; ii++)
                    {
                        adjRow = _searchModifierList[(ConnectDirection)ii].Row + this[i].Row;
                        adjCol = _searchModifierList[(ConnectDirection)ii].Col + this[i].Col;

                        if(this.Any(inf => inf.Row == adjRow 
                            && inf.Col == adjCol 
                            && inf.InfrastructureType == this[i].InfrastructureType))
                        {
                            this[i].ListOfConnectionDirections.Add((ConnectDirection)ii);
                        }
                    }
                }
            }
            void addAdditionalSegments()
            {
                int lengthNow = this.Count;

                for (int i = 0; i < this.Count; i++)
                {
                    foreach (ConnectDirection cd in this[i].ListOfConnectionDirections)
                    {
                        this[i].ListOfSegmentTypes = getSegmentTypeList(this[i], cd);
                        List<SegmentType> segmentTypesRequiredForThisSQ = getSegmentTypeList(this[i], cd);
                    }
                }
                List<SegmentType> getSegmentTypeList(InfSegment infSeg, ConnectDirection cd)
                {
                    List<SegmentType> listOfSegmentTypesRequired = new List<SegmentType>();

                    bool isWater = (infSeg.InfrastructureType == IT.MainRiver 
                        || infSeg.InfrastructureType == IT.Tributary) ? true : false;

                    // all water connects to NW
                    // all non-water connects to SE

                    switch (cd)
                    {
                        case ConnectDirection.NW:
                            if (isWater)
                            {
                                listOfSegmentTypesRequired.Add(SegmentType.NW_out_to_W);                                
                            }
                            else
                            {
                                listOfSegmentTypesRequired.Add(SegmentType.SExSW);
                                listOfSegmentTypesRequired.Add(SegmentType.SW_out_to_W);                                
                            }
                            addPassThroughSQ(infSeg, ConnectDirection.W);
                            break;

                        case ConnectDirection.N:
                            if (isWater)
                            {
                                listOfSegmentTypesRequired.Add(SegmentType.NW_out_to_N);
                            }
                            else
                            {
                                listOfSegmentTypesRequired.Add(SegmentType.NE_out_to_N);
                                listOfSegmentTypesRequired.Add(SegmentType.NExSE);
                            }
                            break;

                        case ConnectDirection.NE:
                            if (isWater)
                            {
                                listOfSegmentTypesRequired.Add(SegmentType.NWxNE);
                                listOfSegmentTypesRequired.Add(SegmentType.NE_out_to_E);
                            }
                            else
                            {
                                listOfSegmentTypesRequired.Add(SegmentType.SE_out_to_E);                                
                            }
                            addPassThroughSQ(infSeg, ConnectDirection.E);
                            break;

                        case ConnectDirection.E:
                            if (isWater)
                            {
                                listOfSegmentTypesRequired.Add(SegmentType.NWxNE);
                                listOfSegmentTypesRequired.Add(SegmentType.NE_out_to_E);
                            }
                            else
                            {
                                listOfSegmentTypesRequired.Add(SegmentType.SE_out_to_E);
                            }
                            break;

                        case ConnectDirection.SE:
                            if (isWater)
                            {
                                listOfSegmentTypesRequired.Add(SegmentType.NExSE);
                                listOfSegmentTypesRequired.Add(SegmentType.NE_out_to_E);
                            }
                            else
                            {
                                listOfSegmentTypesRequired.Add(SegmentType.SE_out_to_E);
                                addPassThroughSQ(infSeg, ConnectDirection.E);
                            }
                            break;

                        case ConnectDirection.S:
                            if (isWater)
                            {
                                listOfSegmentTypesRequired.Add(SegmentType.SWxNW);
                                listOfSegmentTypesRequired.Add(SegmentType.SW_out_to_S);
                            }
                            else
                            {
                                listOfSegmentTypesRequired.Add(SegmentType.SE_out_to_S);
                            }
                            break;

                        case ConnectDirection.SW:
                            if (isWater)
                            {
                                listOfSegmentTypesRequired.Add(SegmentType.NExSE);
                                listOfSegmentTypesRequired.Add(SegmentType.SE_out_to_S);
                            }
                            else
                            {
                                listOfSegmentTypesRequired.Add(SegmentType.SE_out_to_S);
                            }
                            addPassThroughSQ(infSeg, ConnectDirection.S);
                            break;

                        case ConnectDirection.W:
                            if (isWater)
                            {
                                listOfSegmentTypesRequired.Add(SegmentType.NW_out_to_W);
                            }
                            else
                            {
                                listOfSegmentTypesRequired.Add(SegmentType.SExSW);
                                listOfSegmentTypesRequired.Add(SegmentType.SW_out_to_W);
                            }
                            break;

                        case ConnectDirection.Total:
                        default:
                            break;
                    }

                    return listOfSegmentTypesRequired;
                }
                void addPassThroughSQ(InfSegment infSeg, ConnectDirection connectionDirection)
                {
                    int passThroughRow = infSeg.Row + _searchModifierList[connectionDirection].Row;
                    int passThroughCol = infSeg.Col + _searchModifierList[connectionDirection].Col;

                    if(!this.Any(inf => inf.Row == passThroughRow 
                        && inf.Col == passThroughCol)
                        && Coordinate.DoesSquareExist(passThroughRow, passThroughCol))
                    {
                        InfSegment newInfSeg = new InfSegment()
                        {
                            SQ = _sqList[passThroughRow, passThroughCol],
                            InfrastructureType = infSeg.InfrastructureType,                            
                        };
                        newInfSeg.ListOfConnectionDirections.Add(getInverseDirection(connectionDirection));
                        this.Add(newInfSeg);
                    }

                    ConnectDirection getInverseDirection(ConnectDirection cd)
                    {
                        if(cd == ConnectDirection.E) { return ConnectDirection.W; }
                        else if(cd == ConnectDirection.N) { return ConnectDirection.S; }
                        else if(cd == ConnectDirection.W) { return ConnectDirection.E; }
                        else { return ConnectDirection.N; }
                    }
                }
            }

            IT getIT(SQ sq)
            {
                if (sq.IsRoadConnected) { return IT.Road; }
                else if (sq.IsTrainConnected) { return IT.RailRoad; }
                else if (sq.IsPipelineConnected) { return IT.Pipeline; }
                else if (sq.IsMainRiver) { return IT.MainRiver; }
                else if (sq.IsTributary) { return IT.Tributary; }
                return IT.Road;
            }
        }
    }
}
