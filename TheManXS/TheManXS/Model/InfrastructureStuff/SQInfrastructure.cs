using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Map.Surface;
using TheManXS.Model.Services.EntityFrameWork;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.Model.InfrastructureStuff
{
    public class SQInfrastructure
    {
        public SQInfrastructure() { }
        public SQInfrastructure(int row, int col)
        {
            Row = row;
            Col = col;
            Key = Coordinate.GetSQKey(row, col);
            SavedGameSlot = QC.CurrentSavedGameSlot;
        }
        public int Key { get; }
        public int Row { get; }
        public int Col { get; }
        public int SavedGameSlot { get; set; }
        public bool IsMainTransportationCorridor { get; set; }
        public bool IsRoadConnected { get; set; }
        public bool IsSecondaryRoad { get; set; }
        public bool IsTrainConnected { get; set; }
        public bool IsPipelineConnected { get; set; }
        public bool IsHub { get; set; }
        public bool IsMainRiver { get; set; }
        public bool IsTributary { get; set; }
        public int TributaryNumber { get; set; }
        public bool IsTributaryFlowingFromNorth { get; set; }
        public static void WriteArrayToDB(SQInfrastructure[,] infrastructureMapArray)
        {
            using (DBContext db = new DBContext())
            {
                var list = getList();
                //db.SQInfrastructure.AddRange(list); doesn't fix error
                
                db.BulkInsert<SQInfrastructure>(list);
                db.SaveChanges();
            }

            List<SQInfrastructure> getList()
            {
                List<SQInfrastructure> sqList = new List<SQInfrastructure>();
                for (int row = 0; row < infrastructureMapArray.GetLength(0); row++)
                {
                    for (int col = 0; col < infrastructureMapArray.GetLength(1); col++)
                    {
                        sqList.Add(infrastructureMapArray[row, col]);
                    }
                }
                return sqList;
            }
        }
    }
    public class SQInfrastructureDBConfig : IEntityTypeConfiguration<SQInfrastructure>
    {
        public void Configure(EntityTypeBuilder<SQInfrastructure> builder)
        {
            builder.HasKey(s => s.Key);
        }
    }
}
