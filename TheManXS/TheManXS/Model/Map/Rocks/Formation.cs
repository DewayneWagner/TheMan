using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Linq;
using TheManXS.Model.ParametersForGame;
using TheManXS.Model.Services.EntityFrameWork;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.Model.Map.Rocks
{
    public class Formation
    {
        public Formation() { SavedGameSlot = QC.CurrentSavedGameSlot; }
        public int ID { get; set; }
        public string Name { get; set; }
        public bool HasBeenDiscovered { get; set; }
        public ResourceTypeE ResourceType { get; set; }
        public int SavedGameSlot { get; set; }

        public void SetFormationToDiscovered(int formationNum)
        {
            using (DBContext db = new DBContext())
            {
                var formation = db.Formation.Where(f => f.ID == formationNum);
                Formation form = (Formation)formation;
                form.HasBeenDiscovered = true;
                db.SaveChanges();
            }
        }
    }
    public class FormationDBConfig : IEntityTypeConfiguration<Formation>
    {
        public void Configure(EntityTypeBuilder<Formation> builder)
        {
            builder.Property(f => f.ResourceType)
                .HasConversion(new EnumToStringConverter<ResourceTypeE>());
        }
    }
}
