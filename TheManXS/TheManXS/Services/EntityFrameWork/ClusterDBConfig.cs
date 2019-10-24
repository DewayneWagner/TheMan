using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Gameplay;

namespace TheManXS.Model.Services.EntityFrameWork
{
    public class ClusterDBConfig : IEntityTypeConfiguration<Cluster>
    {
        public void Configure(EntityTypeBuilder<Cluster> builder)
        {
            builder.HasKey(c => c.ID);

            builder.Property(c => c.Headline)
                .HasMaxLength(250);

            builder.Property(c => c.ID)
                .IsRequired();
                
            builder.Property(c => c.VariableImpacted)
                .HasConversion(new EnumToStringConverter<VariableImpactedE>());

            builder.Property(c => c.Applicability)
                .HasConversion(new EnumToStringConverter<ApplicabilityE>());
        }
    }
}
