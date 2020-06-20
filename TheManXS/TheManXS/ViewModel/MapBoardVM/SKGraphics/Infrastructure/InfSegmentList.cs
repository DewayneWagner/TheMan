using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Map.Surface;
using IT = TheManXS.Model.ParametersForGame.InfrastructureType;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Infrastructure
{
    class InfSegmentList : List<InfSegment>
    {
        public InfSegmentList(Game game, List<SQ> sqList)
        {
            InitList(game, sqList);
        }
        void InitList(Game game, List<SQ> sqList)
        {
            addStartingInfSegments();
            updateListWithSegments();
            addAdjSQConnectDirectionsToAllInfSegments();

            void addStartingInfSegments()
            {
                foreach (SQ sq in sqList)
                {
                    if (sq.IsRoadConnected) { addInfSegment(sq, IT.Road); }
                    if (sq.IsPipelineConnected) { addInfSegment(sq, IT.Pipeline); }
                    if (sq.IsTrainConnected) { addInfSegment(sq, IT.RailRoad); }
                    if (sq.IsMainRiver) { addInfSegment(sq, IT.MainRiver); }
                    if (sq.IsTributary) { addInfSegment(sq, IT.Tributary); }
                }
                void addInfSegment(SQ sq, IT it)
                {
                    this.Add(new InfSegment()
                    {
                        SQFrom = sq,
                        InfrastructureType = it,
                    });
                }
            }
            void updateListWithSegments()
            {
                int length = this.Count;

                for (int i = 0; i < length; i++)
                {
                    for (int row = -1; row < 2; row++)
                    {
                        for (int col = -1; col < 2; col++)
                        {
                            if (row == 0 && col == 0) { }
                            else
                            {
                                InfSegment inf = this[i];

                                int adjRow = inf.SQFrom.Row + row;
                                int adjCol = inf.SQFrom.Col + col;

                                if (Coordinate.DoesSquareExist(adjRow, adjCol))
                                {
                                    SQ adjSQ = game.SQList[adjRow, adjCol];
                                    if (adjSQHasSameInf(adjSQ, inf.InfrastructureType))
                                    {
                                        if (inf.SQTo == null) { inf.SQTo = adjSQ; }
                                        else if (!isInListAlready(inf.SQTo, adjSQ, inf.InfrastructureType))
                                        {
                                            this.Add(new InfSegment()
                                            {
                                                SQFrom = inf.SQFrom,
                                                SQTo = adjSQ,
                                                InfrastructureType = inf.InfrastructureType,
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                bool adjSQHasSameInf(SQ sq, IT it)
                {
                    switch (it)
                    {
                        case IT.Road:
                            if (sq.IsRoadConnected) { return true; }
                            break;

                        case IT.Pipeline:
                            if (sq.IsPipelineConnected) { return true; }
                            break;

                        case IT.RailRoad:
                            if (sq.IsTrainConnected) { return true; }
                            break;

                        case IT.Tributary:
                            if (sq.IsTributary) { return true; }
                            break;

                        case IT.MainRiver:
                            if (sq.IsMainRiver) { return true; }
                            break;

                        case IT.Total:
                        case IT.Hub:
                        default:
                            return false;
                    }
                    return false;
                }
            }
            bool isInListAlready(SQ fromSQ, SQ toSQ, IT it)
            {
                return this.Any(i => (i.SQFrom == fromSQ || i.SQFrom == toSQ)
                            && (i.SQTo == toSQ || i.SQTo == fromSQ)
                            && i.InfrastructureType == it);
                
            }
            void addAdjSQConnectDirectionsToAllInfSegments()
            {
                foreach (InfSegment infSegment in this)
                {
                    infSegment.ConnectionDirection = getConnectionDirection(infSegment);
                    infSegment.IsDiagonal = isDiagonal(infSegment.ConnectionDirection);
                }
                CD getConnectionDirection(InfSegment inf)
                {
                    int rowDiff = inf.SQFrom.Row - inf.SQTo.Row;
                    int colDiff = inf.SQFrom.Col - inf.SQTo.Col;

                    if (rowDiff == -1)
                    {
                        if (colDiff == -1) { return CD.NW; }
                        else if (colDiff == 0) { return CD.N; }
                        else { return CD.NE; }
                    }
                    else if (rowDiff == 0)
                    {
                        if (colDiff == -1) { return CD.W; }
                        else if (colDiff == 1) { return CD.E; }
                    }
                    else
                    {
                        if (colDiff == -1) { return CD.SW; }
                        else if (colDiff == 0) { return CD.S; }
                        else { return CD.SE; }
                    }
                    return CD.S;
                }
                bool isDiagonal(CD cd)
                {
                    if((int)cd % 2 == 0) { return true; }
                    else { return false; }
                }
            }
        }
    }
}
