using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Settings;
using static TheManXS.Model.Settings.SettingsMaster;

namespace TheManXS.Model.Services.EntityFrameWork
{
    public class SettingsDBConfig : IEntityTypeConfiguration<Setting>
    {
        public void Configure(EntityTypeBuilder<Setting> builder)
        {
            builder.HasKey(s => s.Key);

            builder.Property(s => s.PrimaryIndex)
                .HasConversion(new EnumToStringConverter<AS>());
        }
    }
}
