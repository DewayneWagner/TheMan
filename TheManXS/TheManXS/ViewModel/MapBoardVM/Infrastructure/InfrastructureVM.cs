using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Text;
using TheManXS.Model.Map.Surface;
using TheManXS.Model.Main;
using QC = TheManXS.Model.Settings.QuickConstants;
using TheManXS.Model.Services.EntityFrameWork;
using TheManXS.Model.InfrastructureStuff;

namespace TheManXS.ViewModel.MapBoardVM.Infrastructure
{
    public enum DirectionsCompass { N, NE, E, SE, S, SW, W, NW, Total }
    public enum DegreesOfRotation { N = 0, NE = 45, E = 90, SE = 135, S = 180, SW = 225, W = 270, NW = 315 }

    public class InfrastructureVM : BoxView
    {
        /*********************************
         * Need parameters for different road types
         *    Color
         *    thickness
         *    solid / hatched 
         * *******************************/
        // this needs to be set-up as a parameter (for each type)
        Game g = (Game)Xamarin.Forms.Application.Current.Properties["GAME"];
        public InfrastructureVM() { }

        // need property for solid / dashed line here...
        /***********************************************
         * road directions...lines-up with DirectionsCompass enum
         *  7 - 0 - 1
         *  6 - X - 2
         *  5 - 4 - 3
         * *********************************************/
        public void ConstructAllInfrastructureAtMapInitialization()
        {
            // search order:  start 0,0, search 3, 4, 5, 6
            int[,] searchOrderRowCol = new int[4, 2] { { 0, 1 }, { 1, 0 }, { 0, -1 }, { 0, -1 } };
            int adjacentSQRow, adjacentSQCol;
            
            SQ sq, adjacentSQ;

            using (DBContext db = new DBContext())
            {
                for (int r = 0; r < QC.RowQ; r++)
                {
                    for (int c = 0; c < QC.ColQ; c++)
                    {
                        if (Coordinate.DoesSquareExist(r, c))
                        {
                            sq = db.SQ.Find(Coordinate.GetSQKey(r, c));
                            if (sq.IsRoadConnected)
                            {
                                // find which squares are connected adjacent
                                for (int searchIndex = (int)DirectionsCompass.E; searchIndex <= (int)DirectionsCompass.SW; searchIndex++)
                                {
                                    adjacentSQRow = sq.Row + searchOrderRowCol[searchIndex, 0];
                                    adjacentSQCol = sq.Col + searchOrderRowCol[searchIndex, 1];                                    

                                    if (Coordinate.DoesSquareExist(adjacentSQRow,adjacentSQCol))
                                    {
                                        adjacentSQ = db.SQ.Find(Coordinate.GetSQKey(adjacentSQRow, adjacentSQCol));
                                        if (adjacentSQ.IsRoadConnected)
                                        {
                                            //if (adjacentSQ.IsSecondaryRoad)
                                            //    adjacentSQ.RoadSegmentList.Add(new InfrastructureSegment(adjacentSQ, searchIndex, InfrastructureType.Secondary));
                                            //if (adjacentSQ.IsMainTransportationCorridor)
                                            //    adjacentSQ.Infrastructure.RoadSegmentList.Add(new InfrastructureSegment(adjacentSQ, searchIndex, InfrastructureType.MainTransporationCorridor));
                                            //if (adjacentSQ.Infrastructure.IsHub)
                                            //    adjacentSQ.Infrastructure.RoadSegmentList.Add(new InfrastructureSegment(adjacentSQ, searchIndex, InfrastructureType.Hub));
                                            //if (adjacentSQ.Infrastructure.IsRailConnected)
                                            //    adjacentSQ.Infrastructure.RoadSegmentList.Add(new InfrastructureSegment(adjacentSQ, searchIndex, InfrastructureType.Rail));
                                            //if (adjacentSQ.Infrastructure.IsPipelineConnected)
                                            //    adjacentSQ.Infrastructure.RoadSegmentList.Add(new InfrastructureSegment(adjacentSQ, searchIndex, InfrastructureType.Pipeline));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public void ConstructNewInfrastructure(List<Coordinate> coordinatesList)
        {

        }

    }
}
