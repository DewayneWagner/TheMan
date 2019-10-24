using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;
using TheManXS.Model.Services.EntityFrameWork;
using Xamarin.Forms;
using QC = TheManXS.Model.Settings.QuickConstants;
using System.Linq;
using AS = TheManXS.Model.Settings.SettingsMaster.AS;
using TheManXS.Model.Company;
using TheManXS.Model.Settings;

namespace TheManXS.Model.Main
{
    public class Player
    {
        public Player() { }
        
        public int Key { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string Ticker { get; set; }
        public double Cash { get; set; }
        public double Debt { get; set; }
        public bool IsComputer { get; set; }
        public AllAvailableCompanyColors Color { get; set; }
    }
    public class PlayerDBConfig : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.HasKey(p => p.Key);

            builder.Property(p => p.Ticker)
                .HasMaxLength(3);

            builder.Property(p => p.Color).HasConversion(new EnumToStringConverter<AllAvailableCompanyColors>());
        }
    }    
}
