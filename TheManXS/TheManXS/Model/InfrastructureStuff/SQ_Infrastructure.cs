using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Map.Surface;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.Model.InfrastructureStuff
{
    public class SQ_Infrastructure
    {
        public SQ_Infrastructure() { }
        public SQ_Infrastructure(SQ sq) 
        { 
            Key = sq.Key;
            Row = sq.Row;
            Col = sq.Col;
        }
        public SQ_Infrastructure(int row, int col)
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
    }
    public class InfrastructureDBConfig : IEntityTypeConfiguration<SQ_Infrastructure>
    {
        public void Configure(EntityTypeBuilder<SQ_Infrastructure> builder)
        {
            builder.HasKey(s => s.Key);
        }
    }
}
