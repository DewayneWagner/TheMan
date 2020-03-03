using Microsoft.EntityFrameworkCore;
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
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SkiaSharp;
using static TheManXS.Model.Settings.SettingsMaster;

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
        public double StockPrice { get; set; }
        public double Delta { get; set; }
        public CreditRatingsE CreditRating { get; set; }
        public bool IsComputer { get; set; }

        private SKColor _skColor;
        public SKColor SKColor
        {
            get => _skColor;
            set
            {
                _skColor = value;
                ColorString = _skColor.ToString();
            }
        }

        public string ColorString { get; set; }
        public int SavedGameSlot { get; set; }
    }
    public class PlayerDBConfig : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.HasKey(p => p.Key);

            builder.Property(p => p.Ticker).HasMaxLength(3);

            builder.Ignore(p => p.SKColor);

            builder.Property(p => p.CreditRating).HasConversion(new EnumToStringConverter<CreditRatingsE>());
        }
    }    
}
