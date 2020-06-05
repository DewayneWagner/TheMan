using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Map.Surface;
using IT = TheManXS.Model.ParametersForGame.InfrastructureType;
using ST = TheManXS.ViewModel.MapBoardVM.SKGraphics.Infrastructure.SegmentType;
using CD = TheManXS.ViewModel.MapBoardVM.SKGraphics.Infrastructure.ConnectDirection;
using SkiaSharp;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Infrastructure
{
    class InfrastructureMaster
    {
        Game _game;
        List<SQ> _sqList;
        SearchModifier _sm { get; set; } = new SearchModifier();

        private SKPath[] _paths;
        private List<SQ> _listOfSqsThatNeedInf { get; set; } = new List<SQ>();

        // at start of game
        public InfrastructureMaster(Game game)
        {
            _game = game;
            foreach(SQ sq in game.SQList) { _sqList.Add(sq); }
            _paths = getInitializedArray();
        }

        // for adding new infrastructure during game
        public InfrastructureMaster(Game game, List<SQ> sqList)
        {
            _game = game;
            _sqList = sqList;
        }

        private SKPath[] getInitializedArray()
        {
            SKPath[] paths = new SKPath[(int)IT.Total];

            for (int i = 0; i < (int)IT.Total; i++)
            {
                paths[i] = new SKPath();
            }

            return paths;
        }

        private List<InfSegment> ListOfInfSegments
        {
            get
            {
                List<InfSegment> _listOfInfSegments = new List<InfSegment>();
                createStartingListOfInfSegment();


                return _listOfInfSegments;                

                void createStartingListOfInfSegment()
                {
                    addStartingInfSegments();
                    updateListWithSegments();
                    addAdjSQConnectDirectionsToAllInfSegments();
                    addSegmentTypesToExistingInfSegmentsAndAddNewSegmentsAsNeeded();

                    void addStartingInfSegments()
                    {
                        foreach (SQ sq in _sqList)
                        {
                            if (sq.IsRoadConnected) { addInfSegment(sq, IT.Road); }
                            if (sq.IsPipelineConnected) { addInfSegment(sq, IT.Pipeline); }
                            if (sq.IsTrainConnected) { addInfSegment(sq, IT.RailRoad); }
                            if (sq.IsMainRiver) { addInfSegment(sq, IT.MainRiver); }
                            if (sq.IsTributary) { addInfSegment(sq, IT.Tributary); }
                        }
                        void addInfSegment(SQ sq, IT it)
                        {
                            _listOfInfSegments.Add(new InfSegment()
                            {
                                SQFrom = sq,
                                InfrastructureType = it,
                            });
                        }
                    }                    
                    void updateListWithSegments()
                    {
                        int length = _listOfInfSegments.Count;

                        for (int i = 0; i < length; i++)
                        {
                            for (int row = -1; row < 2; row++)
                            {
                                for (int col = -1; col < 2; col++)
                                {
                                    InfSegment inf = _listOfInfSegments[i];
                                    int adjRow = inf.SQFrom.Row + row;
                                    int adjCol = inf.SQFrom.Col + col;

                                    if (Coordinate.DoesSquareExist(adjRow, adjCol))
                                    {
                                        SQ adjSQ = _game.SQList[adjRow, adjCol];
                                        if (adjSQHasSameInf(adjSQ, inf.InfrastructureType))
                                        {
                                            if (inf.SQTo == null) { inf.SQTo = adjSQ; }
                                            else if (!isInListAlready(inf.SQTo, adjSQ, inf.InfrastructureType))
                                            {
                                                _listOfInfSegments.Add(new InfSegment()
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
                        return _listOfInfSegments.Any(i => i.SQFrom == fromSQ
                                    && i.SQTo == toSQ
                                    && i.InfrastructureType == it);
                    }
                    void addAdjSQConnectDirectionsToAllInfSegments()
                    {
                        foreach (InfSegment infSegment in _listOfInfSegments)
                        {
                            infSegment.ConnectionDirection = getConnectionDirection(infSegment);
                        }
                        ConnectDirection getConnectionDirection(InfSegment inf)
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
                    }
                    void addSegmentTypesToExistingInfSegmentsAndAddNewSegmentsAsNeeded()
                    {
                        foreach (InfSegment infSegment in _listOfInfSegments)
                        {
                            setSegmentType(infSegment);
                        }
                        void setSegmentType(InfSegment inf)
                        {
                            IT it = inf.InfrastructureType;
                            int i = (int)it;
                            InfSKPoints passThroughPts;
                            _paths[i].MoveTo(inf.From.NW);

                            if (inf.InfrastructureType == IT.MainRiver || inf.InfrastructureType == IT.Tributary)
                            {
                                switch (inf.ConnectionDirection)
                                {
                                    case CD.NW:
                                        passThroughPts = new InfSKPoints(_game.SQList[inf.SQFrom.Row, inf.SQTo.Col], it);
                                                                                
                                        _paths[i].LineTo(inf.From.NE);
                                        _paths[i].LineTo(passThroughPts.NW);
                                        _paths[i].LineTo(inf.To.NW);
                                        break;

                                    case CD.N:                                        
                                        _paths[i].LineTo(inf.To.NW);
                                        break;

                                    case CD.NE:
                                        passThroughPts = new InfSKPoints(_game.SQList[inf.SQFrom.Row, inf.SQTo.Col], it);
                                                                                
                                        _paths[i].LineTo(inf.From.NE);
                                        _paths[i].LineTo(passThroughPts.NW);
                                        _paths[i].LineTo(inf.To.NW);
                                        break;

                                    case CD.E:                                        
                                        _paths[i].LineTo(inf.To.NW);
                                        break;

                                    case CD.SE:
                                        passThroughPts = new InfSKPoints(_game.SQList[inf.SQTo.Row, inf.SQFrom.Col], it);
                                                                                
                                        _paths[i].LineTo(passThroughPts.NW);
                                        _paths[i].LineTo(inf.To.NW);
                                        break;

                                    case CD.S:
                                        _paths[i].LineTo(inf.To.NW);
                                        break;

                                    case CD.SW:
                                        passThroughPts = new InfSKPoints(_game.SQList[inf.SQTo.Row, inf.SQFrom.Col], it);

                                        _paths[i].LineTo(passThroughPts.NW);
                                        _paths[i].LineTo(inf.To.NW);
                                        break;

                                    case CD.W:
                                        _paths[i].LineTo(inf.To.NW);
                                        break;

                                    case CD.Total:
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                _paths[i].MoveTo(inf.From.SE);
                                switch (inf.ConnectionDirection)
                                {
                                    case CD.NW:
                                        passThroughPts = new InfSKPoints(_game.SQList[inf.SQFrom.Row, inf.SQTo.Col], it);
                                        _paths[i].LineTo(passThroughPts.SE);
                                        _paths[i].LineTo(inf.To.SE);                                        
                                        break;

                                    case CD.N:
                                        _paths[i].LineTo(inf.To.SE);
                                        break;

                                    case CD.NE:
                                        passThroughPts = new InfSKPoints(_game.SQList[inf.SQFrom.Row, inf.SQTo.Col], it);
                                        _paths[i].LineTo(passThroughPts.SE);
                                        _paths[i].LineTo(inf.To.SE);
                                        break;

                                    case CD.E:
                                        _paths[i].LineTo(inf.To.SE);
                                        break;

                                    case CD.SE:
                                        passThroughPts = new InfSKPoints(_game.SQList[inf.SQFrom.Row, inf.SQTo.Col], it);
                                        _paths[i].LineTo(passThroughPts.SE);
                                        _paths[i].LineTo(inf.To.SE);
                                        break;

                                    case CD.S:
                                        _paths[i].LineTo(inf.To.SE);
                                        break;

                                    case CD.SW:
                                        passThroughPts = new InfSKPoints(_game.SQList[inf.SQFrom.Row, inf.SQTo.Col], it);
                                        _paths[i].LineTo(passThroughPts.SE);
                                        _paths[i].LineTo(inf.To.SE);
                                        break;

                                    case CD.W:
                                        _paths[i].LineTo(inf.To.SE);
                                        break;

                                    case CD.Total:
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }                
            }
        }
    }
}
