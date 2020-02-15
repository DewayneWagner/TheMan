using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using static TheManXS.Model.Settings.SettingsMaster;

namespace TheManXS.Model.Services.EntityFrameWork
{
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
