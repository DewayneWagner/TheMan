using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Financial.CommodityStuff;

namespace TheManXS.Services.EntityFrameWork
{
    public class CommodityDBConfig : IEntityTypeConfiguration<Commodity>
    {
        public void Configure(EntityTypeBuilder<Commodity> builder)
        {
            builder.Property(c => c.Delta).IsRequired();
            builder.Property(c => c.ID).ValueGeneratedOnAdd();
            builder.Property(c => c.Price).IsRequired();
            builder.Property(c => c.ResourceTypeNumber).IsRequired();
            builder.Property(c => c.Turn).IsRequired();
        }
    }
}
