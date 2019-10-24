using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Financial;

namespace TheManXS.Services.EntityFrameWork
{
    public class NextActionDBConfig : IEntityTypeConfiguration<NextAction>
    {
        public void Configure(EntityTypeBuilder<NextAction> builder)
        {
            builder.HasKey(n => n.Key);

            builder.Property(n => n.Cost).IsRequired();
            builder.Property(n => n.Text).IsRequired();
        }
    }
}
