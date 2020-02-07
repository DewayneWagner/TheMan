using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Financial;

namespace TheManXS.Services.EntityFrameWork
{
    public class NextActionDBConfig : IEntityTypeConfiguration<NextAction>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<NextAction> builder)
        {
            builder.HasKey(n => n.Key);

            //builder.Ignore(n => n._sq);
            //builder.HasOne(n => n._sq).WithOne();
            builder.Property(n => n.NextActionCost).IsRequired();
            builder.Property(n => n.NetActionText).IsRequired();
        }
    }
}
