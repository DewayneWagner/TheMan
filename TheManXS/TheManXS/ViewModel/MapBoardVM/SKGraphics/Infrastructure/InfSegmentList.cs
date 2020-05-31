using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TheManXS.Model.Main;
using IT = TheManXS.Model.ParametersForGame.InfrastructureType;
using TheManXS.Model.Map.Surface;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Infrastructure
{
    class InfSegmentList : List<InfSegment>
    {
        List<SQ> _listSQsThatNeedInf;
        //List<InfSegment> _listOfRequiredInfSegments = new List<InfSegment>();
        InfMaster _infMaster;

        // for rendering inf on new map
        public InfSegmentList(SQList sqList, InfMaster infMaster)
        {
            _listSQsThatNeedInf = new List<SQ>();
            _infMaster = infMaster;

            InitSQListThatNeedInfAtBeginningOfGame(sqList);            
            CreateListOfInfSegmentsToRender();
        }
        // for adding inf during gameplay
        public InfSegmentList(List<SQ> sqList, InfMaster infMaster)
        {
            _listSQsThatNeedInf = new List<SQ>();
            _listSQsThatNeedInf = sqList;
            _infMaster = infMaster;
        }
        private void InitSQListThatNeedInfAtBeginningOfGame(SQList sqList)
        {
            foreach (SQ sq in sqList)
            {
                if (sq.IsRoadConnected) { _listSQsThatNeedInf.Add(sq); }
                if (sq.IsTrainConnected) { _listSQsThatNeedInf.Add(sq); }
                if (sq.IsPipelineConnected) { _listSQsThatNeedInf.Add(sq); }
                if (sq.IsMainRiver) { _listSQsThatNeedInf.Add(sq); }
                if (sq.IsTributary) { _listSQsThatNeedInf.Add(sq); }
            }
        }
        private void CreateListOfInfSegmentsToRender()
        {            
            //int length = _listOfRequiredInfSegments.Count;
            int length = _listSQsThatNeedInf.Count;
            InfSegment thisInfSeg;
            InfSegment adjInfSeg = new InfSegment();
            SQ adjSQ = new SQ();

            SearchModifierList searchModifierList = new SearchModifierList();
            SearchModifier sm;

            initListOfInfSegmentsThemselves();
            initializeListOfRequiredSegmentsWithSegmentTypesAndAdditionalRequiredSegments();

            void initializeListOfRequiredSegmentsWithSegmentTypesAndAdditionalRequiredSegments()
            {
                for (int i = 0; i < length; i++)
                {
                    thisInfSeg = this[i];
                    //thisInfSeg = _listOfRequiredInfSegments[i];
                    if (adjSQExistsAndHasSameInfType(thisInfSeg))
                    {
                        thisInfSeg.SegmentType = adjInfSeg.SegmentType;

                        if(sm.AreAdditionalSegmentTypesToAdd) 
                        {
                            this.Add(new InfSegment()
                            {
                                SQ = thisInfSeg.SQ,
                                AdjSQ = adjSQ,
                                SegmentType = sm.AdditionalSegmentTypeToAdd,
                                InfrastructureType = thisInfSeg.InfrastructureType,
                            });
                        }
                    }   
                }
            }

            bool adjSQExistsAndHasSameInfType(InfSegment infSeg)
            {
                sm = searchModifierList[infSeg.SegmentType];

                int adjRow = infSeg.Row + sm.Row;
                int adjCol = infSeg.Col + sm.Col;

                if (Coordinate.DoesSquareExist(adjRow, adjCol))
                {
                    if(this.Any(i => i.Row == adjRow && i.Col == adjCol))
                    {
                        adjSQ = _listSQsThatNeedInf.Where(i => i.Row == adjRow && i.Col == adjCol).FirstOrDefault();
                        adjInfSeg = this.Where(i => i.Row == adjRow && i.Col == adjCol).FirstOrDefault();
                        return true;
                    }
                    //if (_listOfRequiredInfSegments.Any(i => i.Row == adjRow && i.Col == adjCol))
                    //{
                    //    adjSQ = _listSQsThatNeedInf.Where(i => i.Row == adjRow && i.Col == adjCol).FirstOrDefault();
                    //    adjInfSeg = _listOfRequiredInfSegments.Where(i => i.Row == adjRow && i.Col == adjCol).FirstOrDefault();
                    //    return true;
                    //}
                }
                return false;
            }
            void initListOfInfSegmentsThemselves()
            {
                foreach (SQ sq in _listSQsThatNeedInf)
                {
                    this.Add(new InfSegment()
                    {
                        SQ = sq,
                        InfrastructureType = getIT(sq),
                    });
                    //_listOfRequiredInfSegments.Add(new InfSegment()
                    //{
                    //    SQ = sq,
                    //    InfrastructureType = getIT(sq),
                    //});
                }
            }
            IT getIT(SQ sq)
            {
                if(sq.IsRoadConnected) { return IT.Road; }
                else if(sq.IsTrainConnected) { return IT.RailRoad; }
                else if(sq.IsPipelineConnected) { return IT.Pipeline; }
                else if(sq.IsMainRiver) { return IT.MainRiver; }
                else if(sq.IsTributary) { return IT.Tributary; }
                return IT.Road;
            }
        }
    }
}
