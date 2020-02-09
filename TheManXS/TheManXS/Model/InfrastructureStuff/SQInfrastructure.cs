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
        public static void WriteArrayToDB(SQInfrastructure[,] infrastructureMapArray)
        {
            //using (DBContext db = new DBContext())
            //{
                //var list = getList(); // this works - creates list of 6000 items.

                // var eistingSQ = db.SQ.ToList(); this works
                // var existing = db.SQInfrastructure.ToList(); this errors out
                //var existing = db.SQInfrastructure.Select(x => x).ToList();
                //db.SQInfrastructure.RemoveRange(existing); this errors out

                //db.SQInfrastructure.UpdateRange(list); // this works!  save changes works with this method...but can't change map size
                //db.SQInfrastructure.AddRange(list); save changes does not work with this method

                //db.BulkInsert<SQInfrastructure>(list);
                //db.SQInfrastructure.AddRange(list);
                //db.SaveChanges();
            //}

            //List<SQInfrastructure> getList()
            //{
            //    List<SQInfrastructure> sqList = new List<SQInfrastructure>();
            //    for (int row = 0; row < infrastructureMapArray.GetLength(0); row++)
            //    {
            //        for (int col = 0; col < infrastructureMapArray.GetLength(1); col++)
            //        {
            //            sqList.Add(infrastructureMapArray[row, col]);
            //        }
            //    }
            //    return sqList;
            //}
        }
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
                db.SQInfrastructure.UpdateRange(this);
                db.SaveChanges();
            }
        }
    }
    public class SQInfrastructureDBConfig : IEntityTypeConfiguration<SQInfrastructure>
    {
        public void Configure(EntityTypeBuilder<SQInfrastructure> builder)
        {
            //builder.HasOne<SQ>().WithOne(s => s.Key)

            builder.HasKey(s => s.Key);
            builder.Ignore(s => s.ThisSQ);

            //    modelBuilder.Entity<Author>()
            //.HasOne(a => a.AuthorBiography).WithOne(b => b.Author)
            //.HasForeignKey<AuthorBiography>(e => e.AuthorId);
            //    modelBuilder.Entity<Author>().ToTable("Authors");
            //    modelBuilder.Entity<AuthorBiography>().ToTable("Authors");


            //builder.HasOne(s => s.ThisSQ).WithOne();
            //builder.Property(s => s.Row).IsRequired();
            //builder.Property(s => s.Col).IsRequired();




        }
    }
}
