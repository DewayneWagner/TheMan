using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.Model.Financial.CommodityStuff
{
    public class Commodity
    {
        public Commodity() { }
        public Commodity(double price, double delta, int resourceType, int turnNumber)
        {
            Price = price;
            Delta = delta;
            Turn = turnNumber;
            ResourceTypeNumber = resourceType;
            SavedGameSlot = QC.CurrentSavedGameSlot;
        }

        public int ID { get; set; }
        public double Price { get; set; }
        public double Delta { get; set; }
        public int ResourceTypeNumber { get; set; }
        public int Turn { get; set; }
        public int SavedGameSlot { get; set; }
        public double FourTurnMovingAvgPricing { get; set; }
    }
    public class CommodityDBConfig : IEntityTypeConfiguration<Commodity>
    {
        public void Configure(EntityTypeBuilder<Commodity> builder)
        {
            builder.Property(c => c.ID).ValueGeneratedOnAdd();
            builder.HasKey(c => c.ID);
        }
    }
}
