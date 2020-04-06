﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using QC = TheManXS.Model.Settings.QuickConstants;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using TheManXS.Model.Services.EntityFrameWork;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SkiaSharp;
using TheManXS.Model.Company;
using TheManXS.Model.ParametersForGame;

namespace TheManXS.Model.Settings
{
    public class GameSpecificParameters 
    {
        public GameSpecificParameters() { }
                
        public int Slot { get; set; }
        public int PlayerNumber { get; set; }               
        public Difficulty Diff { get; set; }
        public string CompanyName { get; set; }
        public string Ticker { get; set; }
        public DateTime LastPlayed { get; set; }
        public SKColor CompanyColor { get; set; }
        public CompanyColorGenerator CompanyColorGenerator { get; set; }
        public string Quarter { get; set; }
        public int TurnNumber { get; set; }
        public int ActivePlayerNumber { get; set; }
        
        public static List<GameSpecificParameters> GetListOfSavedGameData()
        {
            List<GameSpecificParameters> _gsp = new List<GameSpecificParameters>();
            using (DBContext db = new DBContext())
            {
                if(db.GameSpecificParameters.Count() != 0)
                {
                    return db.GameSpecificParameters.ToList();
                }
            }
            _gsp = InitListWithStarterValues();
            return _gsp;
        }
        private static List<GameSpecificParameters> InitListWithStarterValues()
        {
            List<GameSpecificParameters> _gsp = new List<GameSpecificParameters>();

            _gsp.Add(new GameSpecificParameters()
            {
                CompanyName = "Talisman Energy",
                Diff = Difficulty.Easy,
                Slot = 1,
                Ticker = "TLM",
                LastPlayed = DateTime.Now,
            });
            _gsp.Add(new GameSpecificParameters()
            {
                CompanyName = "PennWest Exploration",
                Diff = Difficulty.Medium,
                Slot = 2,
                Ticker = "PWT",
                LastPlayed = DateTime.Now,
            });
            _gsp.Add(new GameSpecificParameters()
            {
                CompanyName = "Crescent Point Energy",
                Diff = Difficulty.Hard,
                Slot = 3,
                Ticker = "CPG",
                LastPlayed = DateTime.Now,
            });

            using(DBContext db = new DBContext())
            {
                db.AddRange(_gsp);
                db.SaveChanges();
            }
            return _gsp;
        }
    }
    public class GSPDBConfig : IEntityTypeConfiguration<GameSpecificParameters>
    {
        public void Configure(EntityTypeBuilder<GameSpecificParameters> builder)
        {
            builder.HasKey(g => g.Slot);

            builder.Property(g => g.Diff)
                .HasConversion(new EnumToStringConverter<Difficulty>());

            builder.Ignore(g => g.CompanyColorGenerator);

            builder.Ignore(g => g.CompanyColor);
        }
    }
    
}
