using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public SQInfrastructure(SQ thisSQ)
        {
            ThisSQ = thisSQ;
            Row = ThisSQ.Row;
            Col = ThisSQ.Col;
            Key = ThisSQ.Key;        
            SavedGameSlot = QC.CurrentSavedGameSlot;
        }
        public int Key { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
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
        public SQ ThisSQ { get; set; }
    }
    public class SQInfrastructureList : List<SQInfrastructure>
    {
        private SQInfrastructure[,] _mapArray;
        public SQInfrastructureList(SQInfrastructure[,] infrastructureMapArray)
        {
            _mapArray = infrastructureMapArray;
            InitList();
            WriteToDB();
        }
        private void InitList()
        {
            using (DBContext db = new DBContext())
            {
                for (int row = 0; row < _mapArray.GetLength(0); row++)
                {
                    for (int col = 0; col < _mapArray.GetLength(1); col++)
                    {
                        this.Add(new SQInfrastructure(db.SQ.Find(Coordinate.GetSQKey(row,col)))
                        {
                            ThisSQ = db.SQ.Find(Coordinate.GetSQKey(row, col)),
                        });
                    }
                }
            }
        }
        private void InitList(bool testingAboveMethod)
        {
            for (int row = 0; row < _mapArray.GetLength(0); row++)
            {
                for (int col = 0; col < _mapArray.GetLength(1); col++)
                {
                    this.Add(_mapArray[row, col]);
                }
            }
        }
        private void WriteToDB()
        {
            using (DBContext db = new DBContext())
            {
                if (db.SQInfrastructure.Any(s => s.SavedGameSlot == QC.CurrentSavedGameSlot))
                {
                    db.SQInfrastructure.UpdateRange(this);
                }
                else { db.SQInfrastructure.AddRange(this); }

                db.SaveChanges();
            }
        }
    }
    public class SQInfrastructureDBConfig : IEntityTypeConfiguration<SQInfrastructure>
    {
        public void Configure(EntityTypeBuilder<SQInfrastructure> builder)
        {
            builder.HasKey(s => s.Key);
            builder.Ignore(s => s.ThisSQ);
        }
    }
}
