using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SkiaSharp;
using TheManXS.Model.CityStuff;
using TheManXS.Model.Financial;
using TheManXS.Model.Map.Surface;
using TheManXS.Model.ParametersForGame;
using QC = TheManXS.Model.Settings.QuickConstants;
using ST = TheManXS.Model.ParametersForGame.StatusTypeE;

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

        public bool IsMainTransportationCorridor { get; set; }
        public bool IsRoadConnected { get; set; }
        public bool IsTrainConnected { get; set; }
        public bool IsPipelineConnected { get; set; }
        public bool IsHub { get; set; }
        public bool IsMainRiver { get; set; }
        public bool IsTributary { get; set; }

        public int TributaryNumber { get; set; }
        public bool IsTributaryFlowingFromNorth { get; set; }
        public string NextActionText { get; set; }
        public double NextActionCost { get; set; }
        public NextAction.NextActionType NextActionType { get; set; }
        public CityDensity CityDensity { get; set; }

        // not included in DB
        public City City { get; set; }
        public SKRect SKRect
        {
            get
            {
                float left = Col * QC.SqSize;
                float top = Row * QC.SqSize;
                float right = (Col + 1) * QC.SqSize;
                float bottom = (Row + 1) * QC.SqSize;
                return new SKRect(left, top, right, bottom);
            }
        }

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

            builder.Property(s => s.CityDensity)
                .HasConversion(new EnumToStringConverter<CityDensity>());

            builder.Ignore(s => s.City);
            builder.Ignore(s => s.SKRect);
        }
    }
}
