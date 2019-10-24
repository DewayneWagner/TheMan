using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Text;

namespace TheManX.ViewModel.GameBoardVM
{
    public enum DirectionsCompass { E, SE, S, SW, W, NW, N, NE }
    public enum DegreesOfRotation { E = 90, SE = 135, S = 180, SW = 225, W = 270, NW = 315, N = 0, NE = 45 }
 
    //public class InfrastructureVM : BoxView
    //{
    //    /*********************************
    //     * Need parameters for different road types
    //     *    Color
    //     *    thickness
    //     *    solid / hatched 
    //     * *******************************/
    //    // this needs to be set-up as a parameter (for each type)
    //    Game g = (Game)Xamarin.Forms.Application.Current.Properties["GAME"];
    //    public InfrastructureVM() { }        

    //    // need property for solid / dashed line here...
    //    /***********************************************
    //     * road directions...lines-up with DirectionsCompass enum
    //     *  5 - 6 - 7
    //     *  4 - X - 0
    //     *  3 - 2 - 1
    //     * *********************************************/
    //    public void ConstructAllInfrastructureAtMapInitialization()
    //    {
    //        // search order:  start 0,0, search 3, 4, 5, 6
    //        int[,] searchOrderRowCol = new int[4, 2] { { 0, 1 }, { 1, 0 }, { 0, -1 }, { 0, -1 } };
    //        int adjacentSQRow, adjacentSQCol;
    //        Coordinate adjacentCoordinate;

    //        Coordinate coord;
    //        SQ sq, adjacentSQ;
    //        for (int r = 0; r < g.Constants.RowQ; r++)
    //        {
    //            for (int c = 0; c < g.Constants.ColQ; c++)
    //            {
    //                coord = new Coordinate(r, c);
    //                if(Coordinate.DoesSquareExist(g, coord))
    //                {
    //                    sq = g.SQDictionary[coord.FullCoord];
    //                    if (sq.Infrastructure.IsRoadConnected)
    //                    {
    //                        // find which squares are connected adjacent
    //                        for (int searchIndex = (int)DirectionsCompass.E; searchIndex <= (int)DirectionsCompass.SW; searchIndex++)
    //                        {
    //                            adjacentSQRow = sq.Tile.Coordinate.Row + searchOrderRowCol[searchIndex, 0];
    //                            adjacentSQCol = sq.Tile.Coordinate.Col + searchOrderRowCol[searchIndex, 1];
    //                            adjacentCoordinate = new Coordinate(adjacentSQRow, adjacentSQCol);

    //                            if (Coordinate.DoesSquareExist(g, adjacentCoordinate))
    //                            {
    //                                adjacentSQ = g.SQDictionary[adjacentCoordinate.FullCoord];
    //                                if (adjacentSQ.Infrastructure.IsRoadConnected)
    //                                {                                        
    //                                    if (adjacentSQ.Infrastructure.IsSecondaryRoad)
    //                                        adjacentSQ.Infrastructure.RoadSegmentList.Add(new InfrastructureSegment(adjacentSQ, searchIndex, InfrastructureType.Secondary));
    //                                    if (adjacentSQ.Infrastructure.IsMainTransportationCorridor)
    //                                        adjacentSQ.Infrastructure.RoadSegmentList.Add(new InfrastructureSegment(adjacentSQ, searchIndex, InfrastructureType.MainTransporationCorridor));
    //                                    if (adjacentSQ.Infrastructure.IsHub)
    //                                        adjacentSQ.Infrastructure.RoadSegmentList.Add(new InfrastructureSegment(adjacentSQ, searchIndex, InfrastructureType.Hub));
    //                                    if (adjacentSQ.Infrastructure.IsRailConnected)
    //                                        adjacentSQ.Infrastructure.RoadSegmentList.Add(new InfrastructureSegment(adjacentSQ, searchIndex, InfrastructureType.Rail));
    //                                    if (adjacentSQ.Infrastructure.IsPipelineConnected)
    //                                        adjacentSQ.Infrastructure.RoadSegmentList.Add(new InfrastructureSegment(adjacentSQ, searchIndex, InfrastructureType.Pipeline));
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    public void ConstructNewInfrastructure(List<Coordinate> coordinatesList)
    //    {

    //    }
        
    //}    
}
