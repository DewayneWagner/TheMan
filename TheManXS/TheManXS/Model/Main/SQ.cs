using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Services.EntityFrameWork;
using QC = TheManXS.Model.Settings.QuickConstants;
using ST = TheManXS.Model.ParametersForGame.StatusTypeE;
using Xamarin.Forms;
using System.Linq;
using TheManXS.Model.Financial;
using TheManXS.Model.CityStuff;
using TheManXS.Model.Map.Surface;
using TheManXS.ViewModel.MapBoardVM.Tiles;
using TheManXS.Model.InfrastructureStuff;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TheManXS.Model.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TheManXS.Model.ParametersForGame;

namespace TheManXS.Model.Main
{
    public enum NextActions { Purchase, Explore, Develop, Suspend, ReclaimReactivate, Total }
    public class SQ
    {
        public SQ() { }
        public SQ(bool isForPropertyDictionary) { }
        Game _game;
        
        public SQ(int row, int col, Game game)
        {
            _game = game;
            Row = row;
            Col = col;
            SavedGameSlot = QC.CurrentSavedGameSlot;
            Key = Coordinate.GetSQKey(row, col);
            SQInfrastructure = new SQInfrastructure(this);
            OwnerNumber = QC.PlayerIndexTheMan;
            ResourceType = ResourceTypeE.Nada;
            OwnerName = QC.NameOfOwnerOfUnOwnedSquares;
            SetNextActionCostAndText();
        }

        public int Key { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public int SavedGameSlot { get; set; }
        public TerrainTypeE TerrainType { get; set; }
        public ResourceTypeE ResourceType { get; set; }

        private ST _status;
        public ST Status
        {
            get => _status;
            set
            {
                _status = value;
                SetNextActionCostAndText();
            }
        }

        public int OwnerNumber { get; set; }
        public string OwnerName { get; set; }
        public bool IsStartSquare { get; set; }
        public bool IsPartOfUnit { get; set; }
        public int UnitNumber { get; set; }
        public int Production { get; set; }
        public double OPEXPerUnit { get; set; }
        public int FormationID { get; set; }
        public double Transport { get; set; }

        public string NextActionText { get; set; }
        public double NextActionCost { get; set; }
        public NextAction.NextActionType NextActionType { get; set; }

        // not included in DB
        public SQInfrastructure SQInfrastructure { get; set; }
        public City City { get; set; }
        public Coordinate FullCoordinate { get; set; }

        private void SetNextActionCostAndText()
        {
            NextAction n = new NextAction(this, _game);
            NextActionText = n.Text;
            NextActionCost = n.Cost;
            NextActionType = n.ActionType;
        }        
    }
    public class SQDBConfig : IEntityTypeConfiguration<SQ>
    {
        public void Configure(EntityTypeBuilder<SQ> builder)
        {
            builder.HasKey(s => s.Key);

            builder.Property(s => s.Row).IsRequired();
            builder.Property(s => s.Col).IsRequired();

            builder.Property(s => s.SavedGameSlot).IsRequired();

            builder.Property(s => s.TerrainType)
                .HasConversion(new EnumToStringConverter<TerrainTypeE>());

            builder.Property(s => s.ResourceType)
                .HasConversion(new EnumToStringConverter<ResourceTypeE>());

            builder.Property(s => s.Status)
                .HasConversion(new EnumToStringConverter<StatusTypeE>());

            builder.Ignore(s => s.City);
            builder.Ignore(s => s.FullCoordinate);
            builder.Ignore(s => s.SQInfrastructure);
        }
    }
}
