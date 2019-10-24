using System;
using System.Collections.Generic;
using System.Text;

namespace TheManX.Model.Rocks
{
    public enum InfrastructureType { MainTransporationCorridor, Secondary, Hub, Rail, Pipeline }
    public enum InfrastructurePhase { IsProposed, IsUnderConstruction, IsActive, IsOutOfCommision }
    public class Infrastructure
    {
        //// should there be a parameter for different costs for constructing a pipeline vs. rail vs. road????
        //Coordinate cityStartSQ;
        //int rowQ, colQ, lbForHubSpacing, ubForHubSpacing;
        //System.Random rnd = new System.Random();
        //SQ sq;
        //Game g = (Game)Xamarin.Forms.Application.Current.Properties["GAME"];

        //public Infrastructure() { }

        //public Infrastructure(SQ square)
        //{
        //    sq = square;
        //}
        //public Infrastructure(Game game, Coordinate cityStartSquare)
        //{
        //    g = game;
        //    rowQ = g.Constants.RowQ;
        //    colQ = g.Constants.ColQ;
        //    lbForHubSpacing = 8;
        //    ubForHubSpacing = 14;
        //    cityStartSQ = cityStartSquare;
        //    BuildMainTransportationCorridor();

        //    RoadSegmentList = new List<InfrastructureSegment>();
        //}

        //public bool IsRoadConnected { get; set; }
        //public bool IsMainTransportationCorridor { get; set; }
        //public bool IsSecondaryRoad { get; set; }
        //public bool IsPipelineConnected { get; set; }
        //public bool IsRailConnected { get; set; }
        //public bool IsHub { get; set; }

        //public InfrastructurePhase InfrastructurePhase { get; set; }        
        //public List<InfrastructureSegment> RoadSegmentList { get; set; }
        
        //public Coordinate ClosestInfrastructureConnectedSQ(InfrastructureType ClosestInfrastructureTypeToFind, SQ sq, Game g)
        //{
        //    int tRow, tCol;
        //    rowQ = g.Constants.RowQ;
        //    colQ = g.Constants.ColQ;

        //    int searchRadius = rowQ > colQ ? rowQ : colQ;

        //    int sqRow = sq.Tile.Coordinate.Row;
        //    int sqCol = sq.Tile.Coordinate.Col;
        //    Coordinate c;
            
        //    for (int x = 1; x < searchRadius; x++)
        //    {
        //        for (int xx = x; xx >= -x; xx--)
        //        {
        //            for (int xxx = x; xxx >= -x; xxx--)
        //            {
        //                tRow = sqRow + xx;
        //                tCol = sqCol + xxx;
        //                c = new Coordinate(tRow, tCol);

        //                if(Coordinate.DoesSquareExist(g,new Coordinate(tRow,tCol)))
        //                {
        //                    var s = g.SQDictionary[c.FullCoord].Infrastructure;

        //                    if (s.IsHub ||
        //                        s.IsMainTransportationCorridor && ClosestInfrastructureTypeToFind == InfrastructureType.MainTransporationCorridor ||
        //                        s.IsPipelineConnected && ClosestInfrastructureTypeToFind == InfrastructureType.Pipeline ||
        //                        s.IsRailConnected && ClosestInfrastructureTypeToFind == InfrastructureType.Rail ||
        //                        s.IsSecondaryRoad && ClosestInfrastructureTypeToFind == InfrastructureType.Secondary)
        //                    {
        //                        return c;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return sq.Tile.Coordinate;
        //}    
        //private void BuildMainTransportationCorridor()
        //{
        //    int row, col;
        //    SQ sq;
        //    Coordinate coord;
        //    bool reachedEdgeOfMap = false;

        //    int counterForPlacingTransportationHubs = 0, hubSpacing = rnd.Next(lbForHubSpacing, ubForHubSpacing);

        //    for (int i = -1; i < 2; i += 2)
        //    {
        //        row = cityStartSQ.Row;
        //        col = i == -1 ? cityStartSQ.Col + i : cityStartSQ.Col + 3;

        //        coord = new Coordinate(row, col);

        //        do
        //        {
        //            sq = g.SQDictionary[coord.FullCoord];
        //            sq.Infrastructure.IsRoadConnected = true;
        //            sq.Infrastructure.IsMainTransportationCorridor = true;

        //            counterForPlacingTransportationHubs++;
        //            if (counterForPlacingTransportationHubs == hubSpacing)
        //            {
        //                sq.Infrastructure.IsHub = true;
        //                hubSpacing = rnd.Next(lbForHubSpacing, ubForHubSpacing);
        //                counterForPlacingTransportationHubs = 0;
        //            }
                    
        //            row += rnd.Next(-1, 2);
        //            col += i;
        //            coord = new Coordinate(row, col);
                                       
        //            reachedEdgeOfMap = col < 0 || col > colQ - 1 || !Coordinate.DoesSquareExist(g,coord) ? true : false;

        //        } while (!reachedEdgeOfMap);
        //    }        
        //}
        //public List<Coordinate> FindRouteForRoadBetweenTwoPoints(Coordinate from, Coordinate to, Game g)
        //{
        //    List<Coordinate> roadCoordinates = new List<Coordinate>();

        //    int rDirection = GetDirection(to.Row - from.Row);
        //    int cDirection = GetDirection(to.Col - from.Col);

        //    int tCol = from.Col;
        //    int tRow = from.Row;

        //    do
        //    {
        //        if (tRow != to.Row)
        //            tRow += rDirection;
        //        if (tCol != to.Col)
        //            tCol += cDirection;

        //        Coordinate c = new Coordinate(tRow, tCol);

        //        if (Coordinate.DoesSquareExist(g, c))
        //            roadCoordinates.Add(c);

        //    } while (tRow != to.Row || tCol != to.Col);

        //    return roadCoordinates;

        //    int GetDirection(int diff)
        //    {
        //        if (diff < 0)
        //            return -1;
        //        else if (diff == 0)
        //            return 0;
        //        else
        //            return 1;
        //    }
        //}
        //public double EstimateRoadConstructionCost(List<Coordinate> roadCoordinateList)
        //{
        //    int grassland = 0, forest = 0, mountain = 0;
        //    SQ sq;

        //    foreach (Coordinate coord in roadCoordinateList)
        //    {
        //        sq = g.SQDictionary[coord.FullCoord];
        //        switch (sq.Terrain.Type)
        //        {
        //            case TT.Forest:
        //                forest++;
        //                break;
        //            case TT.Grassland:
        //                grassland++;
        //                break;
        //            case TT.Mountain:
        //                mountain++;
        //                break;
        //        }
        //    }
        //    return (grassland * g.Parameter.GetConstantParameter(AP.TransTT, (int)TT.Grassland)) +
        //        (forest * g.Parameter.GetConstantParameter(AP.TransTT, (int)TT.Forest)) +
        //        (mountain * g.Parameter.GetConstantParameter(AP.TransTT, (int)TT.Mountain));
        //}
    }
}
